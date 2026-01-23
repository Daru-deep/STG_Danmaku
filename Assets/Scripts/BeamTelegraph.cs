using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamTelegraphLine : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Transform origin;        // 発射点（銃口）
    [SerializeField] Transform aim;           // 向き（空ならorigin.up）
    [SerializeField] LayerMask playerMask;    // プレイヤー判定

    [SerializeField] LayerMask bulletMask;
    [SerializeField] float maxLength = 30f;

    [Header("Timing")]
    public float telegraphTime = 0.35f; // 予兆時間（当たりなし）
    public float fireTime = 0.20f;      // 本ビーム時間（当たりあり）

    [Header("Width")]
    [SerializeField] float telegraphWidth = 0.03f; // 細い
    [SerializeField] float fireWidth = 0.20f;      // 太い（見た目＆判定の太さ）

    [Header("Hit")]

    LineRenderer lr;
    Coroutine co;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.enabled = false;
        SetWidth(telegraphWidth);
    }

    public void Fire()
    {
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        lr.enabled = true;

        // 1) 予兆：細い（当たり判定なし）
        SetWidth(telegraphWidth);
        float t = 0f;
        while (t < telegraphTime)
        {
            t += Time.deltaTime;
            UpdateLine(); // 見た目更新（追従させたいなら毎フレーム）
            yield return null;
        }

        // 2) 本ビーム：太い（当たり判定あり）
        SetWidth(fireWidth);
        t = 0f;
        while (t < fireTime)
        {
            t += Time.deltaTime;

            // 線更新 + 当たり判定（太さ込み）
            UpdateLine(out Vector2 start, out Vector2 end);
            if (HitPlayer(start, end, fireWidth))
            {
                // ここでゲームオーバー処理を呼ぶ
                var player = FindPlayerLife();
                if (player != null) player.PlayerDown();
                break;
            }

            yield return null;
        }

        lr.enabled = false;
        co = null;
    }

    void SetWidth(float w)
    {
        lr.startWidth = w;
        lr.endWidth = w;
    }

    void UpdateLine()
    {
        UpdateLine(out _, out _);
    }

    void UpdateLine(out Vector2 start, out Vector2 end)
    {
        start = origin ? (Vector2)origin.position : Vector2.zero;
        Vector2 dir = origin ? (aim ? (Vector2)aim.up : (Vector2)origin.up) : Vector2.up;

        float length = maxLength;

        end = start + dir * length;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

bool HitPlayer(Vector2 start, Vector2 end, float width)
{
    Vector2 dir = end - start;
    float dist = dir.magnitude;
    if (dist <= 0.001f) return false;
    dir /= dist;

    float radius = width * 0.5f;

    // player + bullet を拾う
    LayerMask hitMask = playerMask | bulletMask;

    // ほんの少し前に出して自分/発射点巻き込みを減らす（任意）
    Vector2 castStart = start + dir * 0.02f;

    RaycastHit2D hit = Physics2D.CircleCast(castStart, radius, dir, dist, hitMask);
    if (!hit.collider) return false;

    int layer = hit.collider.gameObject.layer;

    // Player に当たった時だけ true
    if (((1 << layer) & playerMask.value) != 0)
        return true;

    // Bullet に当たったら弾を消す（相殺したいなら）
    if (((1 << layer) & bulletMask.value) != 0)
    {
        Destroy(hit.collider.gameObject);
        return false;
    }

    return false;
}


    meManager FindPlayerLife()
    {
        // 最小：タグで拾う（レイヤー運用でもタグは付けておくと便利）
        var go = GameObject.FindWithTag("Player");
        return go ? go.GetComponent<meManager>() : null;
    }
}
