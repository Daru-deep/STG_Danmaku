# Scorecard (Weekly)

## Week: 2026-03-03
### Scores (1–5)
- P0安定性: 3.0/5　← +0.3
- バグ調査力: 2.7/5　← +0.2
- 次の一手: 2.2/5　← +0.2
- Unityライフサイクル耐性: 3.0/5　← +0.1
- アーキ設計: 2/5
- 可読性: 2.2/5　← +0.2
- 保守性: 2/5
- パフォーマンス/GC: 2/5

**Overall: 48/100**　（前回 45 → +3）

### Highlights (what improved)
- CompareTag を全廃し LayerMask SerializeField パターンへ統一（meManager / gunManager / HomingMissileScript）
- Color(255) → Color(1) スケールバグと `while(Opacity != 0)` 無限ループを自力で理解
- FunnelMove.cs の `UnityEditor` using によるビルドエラーを解消
- 子コライダーが親 Rigidbody2D に吸い上げられる Unity 物理仕様を理解 → Kinematic RB2D で解決
- Vector2.Reflect によるパリィ反射角計算を実装

### Biggest risks (what can break next)
- meControler.cs:54 — parryRingPrefab 未アサインで Instantiate クラッシュ（P0 未適用）
- GetParryItem.cs — `other.GetComponent<HomingMissileScript>().DestroyMissile()` null チェックなし（P0）
- PlayerAttackGenarator の無制限 Instantiate → メモリ増大（P1）

### Next focus (max 3)
1) meControler.cs — Awake に parryRingPrefab null チェック追加（P0 解消）
2) GetParryItem.cs — DestroyMissile 呼び出し前の null チェック追加（P0 解消）
3) PlayerAttackGenarator に Object Pool を導入（P1 解消 + GC 改善）

### Evidence
- review_log entries:
  - 2026-02-19 (P0 scan — EnemyAttackGenarator, FunnelMove, meManager 等)
  - 2026-02-27 (P0 scan — ParryRingEffect, meControler, meManager)
  - 2026-03-03 (LayerMask統一・Color修正・ビルドエラー解消・反射計算)
- PR/commit (optional):
  - 6ab567a パリィ、反撃、パリィのエフェクトについて完成
