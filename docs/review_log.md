# Review Log

> Purpose: keep evidence of reviews, bugs, fixes, and learning outcomes.

---

## 2026-02-19
### Context
- Project: Bullet Hell (Unity/C#)
- Trigger: P0 scan 実施 → MissingReferenceException / NullReferenceException の多発リスクを発見
- Command used: P0 scan → APPLY × 4

### Evidence
- Console / Stacktrace:
  - `MissingReferenceException` — FNAllRangeAttack() 内の内側 while で target 破壊後に target.position へアクセス
  - `NullReferenceException` 予備軍 — Beam/fnMove/gm/mM が Inspector 未設定時に即クラッシュ
- Repro steps:
  1) プレイヤーが死亡（または敵が攻撃開始）する
  2) FNAllRangeAttack コルーチンが yield 後に破壊済み target を参照 → 例外
- Related files:
  - `Assets/Scripts/Funnel/FunnelMove.cs`
  - `Assets/Scripts/EnemyAttackGenarator.cs`
  - `Assets/Scripts/meManager.cs`
  - `Assets/Scripts/meControler.cs`

### Findings
- P0:
  - FunnelMove.cs:150 — 内側 while が yield 後に target.position を無チェックでアクセス → MissingReferenceException
  - EnemyAttackGenarator.cs:20 — Beam null のまま GetComponent → NullReferenceException
  - EnemyAttackGenarator.cs:48,52,57,62 — fnMove null のまま呼び出し → NullReferenceException
  - FunnelMove.cs:143 — GetComponent<FunnelManager>() 結果を未チェックで使用
  - meManager.cs:49 — gm null のまま gm.GameOver() → NullReferenceException
  - meControler.cs:44 — mM null のまま mM.isParry → NullReferenceException
- P1:
  - GetComponent をコルーチン内で呼ぶ → GC アロケーション（TryGetComponent 推奨）
- P2:
  - クラス名タイポ（Genarator, Controler）、命名規則混在

### Fix plan (file-by-file)
- [x] `Assets/Scripts/Funnel/FunnelMove.cs` — 内側 while を `while(true)` に展開し先頭で `if(!target) yield break`
- [x] `Assets/Scripts/Funnel/FunnelMove.cs` — GetComponent 結果に null チェック追加
- [x] `Assets/Scripts/EnemyAttackGenarator.cs` — Awake に Beam/fnMove/beam の Fail-Fast ガード追加
- [x] `Assets/Scripts/meManager.cs` — Awake に gm の Fail-Fast ガード追加
- [x] `Assets/Scripts/meControler.cs` — Awake に mM の null チェック追加

### Result
- Fixed? Y
- Root cause: yield を含む内側ループ内での外部オブジェクト参照に null チェックがなかった。また SerializeField の未検証参照が全体的に放置されていた。
- Prevention: yield を含む while ループの先頭に `if (!ref) yield break` を置く。Awake で全 SerializeField を Debug.Assert / enabled=false で検証する。
- Notes: Awake/Start 以外の GetComponent は TryGetComponent に移行すると GC ヒントも消える。

### Implementation
- Applied by: Shiori(APPLY)
- Notes: 内側 while の構造を自分で分析して原因を特定できていた。ガードパターンは Shiori の提示を参考に適用。
- Learning: `yield return null` の前後でオブジェクトが破壊されうる。外側の null チェックは内側 while を守れない。

### Score impact
- p0_stability: +0.5 (MissingReferenceException の根本原因を特定・修正)
- unity_lifecycle: +0.5 (Awake Fail-Fast パターンを複数ファイルに適用)

---

## YYYY-MM-DD
### Context
- Project: Bullet Hell (Unity/C#)
- Trigger: (ex. NullReferenceException / feature design stuck / refactor)
- Command used: (P0 scan / Next step / Architecture sweep / Readability pass)

### Evidence
- Console / Stacktrace:
  - (paste key lines)
- Repro steps:
  1)
  2)
- Related files:
  - `Assets/.../X.cs`
  - `Assets/.../Y.cs`

### Findings
- P0:
  - 
- P1:
  - 
- P2:
  - 

### Fix plan (file-by-file)
- [ ] `Assets/.../X.cs` — (intent)
- [ ] `Assets/.../Y.cs` — (intent)

### Result
- Fixed? (Y/N)
- Root cause: (1行)
- Prevention: (1行)
- Notes:

### Implementation
- Applied by: (Manual / Shiori(APPLY) / Pair)
- Notes: (自分で考えた点、詩織が効いた点を一言)
- Learning: (今回の学びを1行)


### Score impact (optional)
- p0_stability: +0.5 (reason)
- bug_investigation: +0.5 (reason)
- architecture: +0.5 (reason)


