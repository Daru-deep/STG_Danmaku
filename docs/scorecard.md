# Scorecard (Weekly)

## Week: 2026-02-16 ~ 2026-02-22
### Scores (1–5)
- P0安定性: 2.5/5　← +0.5
- バグ調査力: 2/5
- 次の一手: 2/5
- Unityライフサイクル耐性: 2.5/5　← +0.5
- アーキ設計: 2/5
- 可読性: 2/5
- 保守性: 2/5
- パフォーマンス/GC: 2/5

**Overall: 43/100**　（前回 40 → +3）

### Highlights (what improved)
- FNAllRangeAttack の内側 while に target null チェックを追加し MissingReferenceException を根本解決
- Awake Fail-Fast パターン（`enabled = false` + Debug.LogError）を 4 ファイルに適用
- `yield return null` の前後でオブジェクトが破壊されうる、という Unity ライフサイクルの核心を理解した

### Biggest risks (what can break next)
- PlayerAttackGenarator の無制限 Instantiate → プレイ時間が長くなるほどメモリ増大
- FunnelMove の肥大化 → 機能追加のたびに副作用が広がる
- コルーチン内の残存 GetComponent（TryGetComponent 未移行箇所）→ GC 負荷

### Next focus (max 3)
1) PlayerAttackGenarator に Object Pool を導入（P1解消 + GC改善）
2) FunnelMove 内の残存 GetComponent を TryGetComponent に移行（P1/GC改善）
3) FunnelMove のコルーチン肥大化を整理（P1/保守性）

### Evidence
- review_log entries:
  - 2026-02-19 (P0 scan — EnemyAttackGenarator, FunnelMove, meManager 等)
- PR/commit (optional):
  - b2b844f ファンネルの新挙動-オールレンジ攻撃-を作成
  - 908a9e7 パリィの実装開始。判定まで完了
  - 3a52f25 ホーミング完成
