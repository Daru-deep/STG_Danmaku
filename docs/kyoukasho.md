# STG Danmaku — 座標・角度計算 教科書

作成日: 2026-03-03
対象: このプロジェクトで使われている数学・Unity関数の理解

---

## 1. Unityの座標系（2D）

### ワールド座標

Unity 2D は **右が X+、上が Y+** の直交座標系。
画面の中心が `(0, 0)` になるようにカメラを置くのが一般的。

```
     Y+
     |
     |
-----+-----> X+
     |
```

`transform.position` はそのオブジェクトのワールド座標 `(x, y, z)` を返す。
2Dゲームでは z=0 固定で、`Vector2` にキャストして使うことが多い。

```csharp
Vector2 pos = (Vector2)transform.position;
```

### ローカル座標 vs ワールド座標

- `transform.localPosition` — 親オブジェクトからの相対位置
- `transform.position` — ワールド基準の絶対位置
- 当たり判定や距離計算は **常にワールド座標**で行う

---

## 2. Vector2の基本演算

### 方向ベクトルと距離

2点A・B があるとき、**AからBへ向かうベクトル**は引き算で求める。

```csharp
Vector2 A = transform.position;       // 自分の位置
Vector2 B = target.position;         // ターゲットの位置
Vector2 dir = B - A;                 // AからBへ向かうベクトル
float distance = dir.magnitude;      // 距離（ベクトルの長さ）
Vector2 normalized = dir.normalized; // 長さ1に正規化した方向ベクトル
```

**注意**: `dir.magnitude` は `Mathf.Sqrt` を内部で呼ぶためコスト高。
「長さが0より大きいか」だけ知りたい場合は `dir.sqrMagnitude > 0` を使う。

### よく使うベクトル定数

```csharp
Vector2.up    = (0,  1)   // 上
Vector2.down  = (0, -1)   // 下
Vector2.right = (1,  0)   // 右
Vector2.left  = (-1, 0)   // 左
Vector2.zero  = (0,  0)   // ゼロ
```

---

## 3. 角度計算 — Atan2 と Unity Z 回転

### Mathf.Atan2 とは

`Mathf.Atan2(y, x)` は、原点から点 `(x, y)` への角度を **ラジアン** で返す関数。
**X+ 軸（右）を 0** として、反時計回りに増える。

```
         90°(上)
          |
180°(左)--+-- 0°(右)
          |
        -90°(下)
```

```csharp
Mathf.Atan2(1, 0)  // → 1.5708 rad = 90°  (真上)
Mathf.Atan2(0, 1)  // → 0 rad      = 0°   (真右)
Mathf.Atan2(1, 1)  // → 0.7854 rad = 45°  (右斜め上)
```

ラジアンを度に変換するには `* Mathf.Rad2Deg` を掛ける。

```csharp
float angleDeg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
```

### Unity の Z 回転との 90° ズレ

Unity の `transform.rotation.eulerAngles.z` は **Y+ 軸（上）を 0°** として時計回りに増える。
`Atan2` のゼロ点（右=0°）と **90° ズレている**ため、変換時に `+90f` を足す。

```csharp
// dir 方向を向かせたいとき
float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
transform.rotation = Quaternion.Euler(0f, 0f, angle);
```

```
Atan2 の 0° = 右
Unity の 0° = 上  → ズレ = 90°
```

このプロジェクトの `HomingMissileScript` と `GetParryItem` で同じ式を使っている。

---

## 4. transform.up とミサイルの移動

`transform.up` は「そのオブジェクトのローカル Y+ 方向」をワールド座標で表したベクトル。
回転しても常に「オブジェクトが向いている上方向」を返す。

```
rotation.z =  0°  → transform.up = (0,  1)   // 上
rotation.z = 90°  → transform.up = (-1, 0)   // 左
rotation.z = 180° → transform.up = (0, -1)   // 下
```

`HomingMissileScript` はミサイルが **上向きをデフォルト** として設計されており、
`-transform.up` 方向（オブジェクトの「下」）に毎フレーム移動させることで、
回転角がそのまま飛行方向になる。

```csharp
// rotation.z = 0° → (0,-1) 方向（下）に飛ぶ
transform.position += -transform.up * speed * dt;
```

---

## 5. ホーミング計算 — MoveTowardsAngle

### 毎フレームの追尾処理

```csharp
// ① ターゲットへの方向ベクトル
Vector2 to = (Vector2)target.position - (Vector2)transform.position;

// ② その方向を角度に変換（Unity Z回転基準）
float targetAngle = Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg + 90f;

// ③ 現在の角度から targetAngle へ毎フレーム少しずつ回転
float newAngle = Mathf.MoveTowardsAngle(
    transform.eulerAngles.z,   // 現在の角度
    targetAngle,               // 目標の角度
    turnRate * dt              // 最大回転量（度/秒 × 経過時間）
);

transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
```

`Mathf.MoveTowardsAngle` は 359° → 1° のような**角度の折り返し**を自動処理する。
通常の `Mathf.MoveTowards` では 359→1 に 358° 回転してしまうが、
`MoveTowardsAngle` は正しく 2° で折り返す。

### homingDelay による直進時間

```csharp
if (timer < homingDelay || !target) return;  // 追尾開始まで直進
```

発射直後は直進させることで、「慣性で飛んでから曲がる」見た目になる。

---

## 6. LayerMask とビット演算

### レイヤー番号 vs LayerMask

Unity のレイヤーは **番号（int）** と **ビットマスク（LayerMask）** の2種類ある。

| 種類 | 値の例 | 取得方法 |
|------|--------|---------|
| レイヤー番号 | `6` | `gameObject.layer` / `LayerMask.NameToLayer("Player")` |
| LayerMask | `64` (= 1<<6) | `LayerMask.GetMask("Player")` / `[SerializeField] LayerMask` |

`gameObject.layer` が返すのは**番号**。`LayerMask` は**ビット列**なので直接比較できない。

### ビットシフト `1 << layer`

```
レイヤー番号 6 → 1 << 6 = 0b01000000 = 64
レイヤー番号 7 → 1 << 7 = 0b10000000 = 128
```

「この layer 番号はマスクに含まれているか」の判定:

```csharp
int layer = other.gameObject.layer;

// layerMask（ビット列）の対応ビットが立っているか？
if ((layerMask & (1 << layer)) != 0)
{
    // このオブジェクトはマスクに含まれるレイヤー
}
```

### & 演算（ビット AND）

```
layerMask    = 01000000  (レイヤー6 = Player)
1 << layer   = 01000000  (比較したいレイヤーも6)
AND結果      = 01000000  → ≠ 0 → 含まれる！

layerMask    = 01000000  (レイヤー6 = Player)
1 << layer   = 10000000  (比較したいレイヤーは7)
AND結果      = 00000000  → = 0 → 含まれない
```

---

## 7. 反射計算 — Vector2.Reflect

### 反射の公式

```csharp
Vector2 reflected = Vector2.Reflect(incomingDir, normal);
```

- `incomingDir` — 入射方向ベクトル（正規化推奨）
- `normal` — 反射面の法線ベクトル（面に対して垂直、外向き）

物理的な反射: **入射角 = 反射角**

```
    入射 ↘    ↗ 反射
          |  法線
      ----+----（反射面）
```

### このプロジェクトでの使用（GetParryItem.cs）

パリィ円は「円形の壁」なので、法線は**円の中心から衝突点への方向**。

```csharp
// ミサイルの進行方向（-transform.up で移動しているため）
Vector2 incomingDir = -(Vector2)other.transform.up;

// パリィ円の中心（this）から衝突点（ミサイル位置）への外向き法線
Vector2 normal = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;

// 反射方向を計算
Vector2 reflectedDir = Vector2.Reflect(incomingDir, normal);

// 反射方向を Unity Z 回転に変換して適用
float angle = Mathf.Atan2(reflectedDir.y, reflectedDir.x) * Mathf.Rad2Deg + 90f;
presentMissile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
```

反射後はホーミングが `homingDelay` 後に始まるため、
反射初速の方向だけを決めれば自動的に敵へ向かう。

---

## 8. Unity 物理 — OnTriggerEnter2D とRigidbody2D

### トリガーイベントの届き先

Unity 2D では、トリガーイベントは **最も近い Rigidbody2D を持つ GameObject** に届く。

```
[Player]          ← Rigidbody2D あり → OnTriggerEnter2D が呼ばれる
  └[ParryZone]    ← Rigidbody2D なし → イベントは親に吸い上げられる
```

子オブジェクトに独自の当たり判定スクリプトを持たせたい場合は、
**子に Kinematic Rigidbody2D を追加**することで解決する。

```
[Player]          ← Rigidbody2D (Dynamic)
  └[ParryZone]    ← Rigidbody2D (Kinematic) → 子の OnTriggerEnter2D が呼ばれる
```

### CircleCast（BeamTelegraph）

`Physics2D.CircleCast` は「円を指定方向に飛ばして当たり判定を取る」関数。
細長いビームの幅を持たせた当たり判定に使う。

```csharp
Physics2D.CircleCast(
    origin,      // 発射位置
    radius,      // 円の半径（＝ビームの太さ/2）
    direction,   // 飛ばす方向
    distance,    // 飛ばす距離
    layerMask    // 対象レイヤー
);
```

---

## 9. Unity Input System — CallbackContext

`PlayerInput` の「Invoke Unity Events」モードでは、
1つのActionに対して**3つのフェーズ**でイベントが呼ばれる。

| フェーズ | タイミング | 用途 |
|---------|----------|------|
| `ctx.started` | ボタンを押し始めた瞬間 | 押し始め検知 |
| `ctx.performed` | 入力が「確定」したとき（Buttonなら押した時） | 主な処理実行 |
| `ctx.canceled` | 入力が解除されたとき | 入力リセット |

```csharp
public void OnMoveEvent(InputAction.CallbackContext ctx)
{
    move = ctx.ReadValue<Vector2>();          // 入力値を読む
    if (ctx.canceled) move = Vector2.zero;   // 離したらリセット
}
```

**注意**: `ctx.canceled` を忘れると、キーを離しても最後の値が残り「ずっと動き続ける」バグになる。

### Action Type の違い

| Action Type | canceled の発火 | 用途 |
|-------------|----------------|------|
| `Value` | キーを離したとき発火する | 移動（WASD / スティック） |
| `Button` | 押した瞬間のみ | ジャンプ・攻撃 |

移動入力は必ず `Value` + `Control Type: Vector 2` に設定する。

---

## 10. C# キャスト — `(Vector2)` の使い方

`transform.position` は `Vector3` 型（x, y, z）だが、
2D計算では `Vector2`（x, y）として扱う。

```csharp
Vector2 pos = (Vector2)transform.position;  // z を切り捨てて Vector2 に変換
```

逆に `Vector2 → Vector3` への暗黙変換もある（z=0になる）。

```csharp
transform.position = new Vector2(1f, 2f);  // Vector3(1, 2, 0) として代入される
```

---

## まとめ — よく使うパターン

| やりたいこと | コード |
|------------|--------|
| AからBへの方向ベクトル | `(B - A).normalized` |
| 方向ベクトルを角度に変換 | `Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f` |
| 角度を方向ベクトルに変換 | `new Vector2(Mathf.Sin(rad), Mathf.Cos(rad))` |
| レイヤー判定 | `(mask & (1 << layer)) != 0` |
| 反射方向 | `Vector2.Reflect(incomingDir, normal)` |
| 角度の追尾 | `Mathf.MoveTowardsAngle(current, target, speed * dt)` |
| 回転を適用 | `Quaternion.Euler(0f, 0f, angle)` |
