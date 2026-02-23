# Scorecard (Weekly)

## Week: 2026-02-16 ~ 2026-02-22
### Scores (1–5)
- P0安定性: 2.5/5
- バグ調査力: 2.3/5　← +0.3
- 次の一手: 2/5
- Unityライフサイクル耐性: 2.7/5　← +0.2
- アーキ設計: 2/5
- 可読性: 2/5
- 保守性: 2/5
- パフォーマンス/GC: 2/5

**Overall: 44/100**　（前回 43 → +1）

### Highlights (what improved)
- `Destroy(obj, t)` が C# コルーチンではなくエンジン内部タイマーであることを自分で疑問視・理解
- Particle の生成〜破棄を `Destroy(t)` で正しく管理
- PlayerDown() に `if(!isParry)` ガードを追加（二重発火防止）
- パリィ判定をコルーチン（PushParyCount）で時間管理する設計を自分で考えた

### Biggest risks (what can break next)
- meControler.cs:50 — `StopCoroutine(PushParyCount())` が動いているコルーチンを止められない（P1）
- PlayerAttackGenarator の無制限 Instantiate → メモリ増大（P1）
- FunnelMove 内の残存 GetComponent → GC 負荷

### Next focus (max 3)
1) meControler.cs の StopCoroutine を Coroutine 変数管理に修正（P1解消）
2) PlayerAttackGenarator に Object Pool を導入（P1解消 + GC改善）
3) FunnelMove 内の残存 GetComponent を TryGetComponent に移行

### Evidence
- review_log entries:
  - 2026-02-19 (P0 scan — EnemyAttackGenarator, FunnelMove, meManager 等)
- PR/commit (optional):
  - b2b844f ファンネルの新挙動-オールレンジ攻撃-を作成
  - 908a9e7 パリィの実装開始。判定まで完了
  - 3a52f25 ホーミング完成
