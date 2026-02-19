# Scorecard (Weekly)

## Week: 2026-02-16 ~ 2026-02-22
### Scores (1–5)
- P0安定性: 2/5
- バグ調査力: 2/5
- 次の一手: 2/5
- Unityライフサイクル耐性: 2/5
- アーキ設計: 2/5
- 可読性: 2/5
- 保守性: 2/5
- パフォーマンス/GC: 2/5

**Overall: 40/100**

### Highlights (what improved)
- ファンネル・ホーミング・パリィ・ビームと複数の機能を連続実装できた
- FunnelMove で一部 null チェック (`if (!target) { yield break; }`) を書けている
- Destroy 後の `funnels[i] = null` など stale ref を意識した箇所がある

### Biggest risks (what can break next)
- [SerializeField] 未検証が全体に蔓延 → インスペクタ設定漏れで即クラッシュ
- PlayerAttackGenarator の無制限 Instantiate → プレイ時間が長くなるほどメモリ増大
- FunnelMove の肥大化 → 機能追加のたびに副作用が広がる

### Next focus (max 3)
1) Awake に `Debug.Assert` / `enabled = false` ガードを追加（P0解消）
2) GetComponent 結果の null チェックを全箇所に統一（P0解消）
3) PlayerAttackGenarator に Object Pool を導入（P1解消 + GC改善）

### Evidence
- review_log entries:
  - 2026-02-19 (P0 scan — EnemyAttackGenarator, FunnelMove, meManager 等)
- PR/commit (optional):
  - b2b844f ファンネルの新挙動-オールレンジ攻撃-を作成
  - 908a9e7 パリィの実装開始。判定まで完了
  - 3a52f25 ホーミング完成
