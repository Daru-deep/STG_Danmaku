using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class FunnelMove : MonoBehaviour
{

    [SerializeField] GameObject funnelPrefab;

    [SerializeField] Transform standardPoint;

    [SerializeField] Transform target;



    



    GameObject[] funnels;     // ← 実体を保持する配列
    Coroutine fire;

    Coroutine move;

    Coroutine placeOnce;

    //移動
    [SerializeField] float moveSpeed = 10f;

    //回転
    [SerializeField] float radius = 3f;      // 半径
    [SerializeField] float angularSpeed = 90f; // 度/秒
    [SerializeField] float baseAngle;        // 全体の回転角（積算）

     private int CompMove = 0;




    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void FirstMove()

    {
        InstanceFunnel(4);
        GoFire(20);


    }

    public void GoAllRangeAttack()
    {   
       /* if(funnels.Length != 0)
        {
            FNSimau(gameObject.transform);
        }
        */
        if(move != null)StopCoroutine(move);
        move = StartCoroutine(FNAllRangeAttack());

    }

    public void GoFire(int fireNum)
    {
        if (fire != null) StopCoroutine(fire);
        fire = null;
        fire = StartCoroutine(FunnelFire(fireNum));
    }

    Vector3 center;

    void Update()
    {

    }
    public void FNSimau(Transform house)
    {
        if (move != null)
        {
            StopCoroutine(move);
            move = null;
        }
        move = StartCoroutine(FNRetreat(house));
    }
    IEnumerator FNPointAttack()
    {
        InstanceFunnel(4);
        yield return new WaitWhile(AnyFunnelMoving);
        while (true)
        {
            if (!target) { Debug.Log("ターゲットがいませんのでFNPointAttackを実行できません"); break; }
            if (funnels == null || funnels.Length == 0) { Debug.Log("ファンネルがいませんのでFNPointAttackを実行できません"); break; }
            baseAngle += angularSpeed * Time.deltaTime;

            for (int i = 0; i < funnels.Length; i++)
            {
                var fn = funnels[i];
                var fm = fn.GetComponent<FunnelManager>();
                float step = 360f / funnels.Length; // 等間隔
                float angle = baseAngle + step * i; // 各ファンネルの角度

                // 角度→位置（2D）
                float rad = angle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
                if (!fm.GetHoming()) fn.transform.position = target.position + offset;
                if (fm.GetHoming()) fn.transform.position = center;
            }

            yield return new WaitForEndOfFrame();
        }
        
    }

    IEnumerator FNAllRangeAttack()
    {   
        bool[] simpleAttacking = new bool [4];
        float[] attackAngle = new float [4];
        attackAngle[0] = 300;
        attackAngle[1] = 30;
        attackAngle[2] = 180;
        attackAngle[3] = 0;

        InstanceFunnel(4);
        yield return new WaitWhile(AnyFunnelMoving);
        for(int attackingCount = 0 ;attackingCount<funnels.Length;attackingCount++)
        {
            simpleAttacking[attackingCount] = true;
            while (simpleAttacking[attackingCount])
            {
                if (!target) { Debug.Log("ターゲットがいませんのでFNAllRangeAttackを実行できません");yield break; }
                if (funnels == null || funnels.Length == 0) { Debug.Log("ファンネルがいませんのでFNAllRangeAttackを実行できません");yield break; }



                    var fn = funnels[attackingCount];
                    var fm = fn.GetComponent<FunnelManager>();
                    if (fm == null) { Debug.LogError($"[FunnelMove] FunnelManager not found on {fn.name}"); yield break; }
                    float step = 360f / funnels.Length; // 等間隔
                    float angle = attackAngle[attackingCount]; // 各ファンネルの角度

                    // 角度→位置（2D）
                    float rad = angle * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
                    while (true)
                    {
                        if (!target) yield break;
                        if (StepMoveTowards(fn.transform, target.position + offset, 10f)) break;
                        yield return null;
                    }
                    GoFire(attackingCount);
                    simpleAttacking[attackingCount] = false;

            }
        }
    }

    IEnumerator FNRetreat(Transform house)
    {
        Debug.Log("START_FNRestert_");
        // houseが無いなら撤退処理できない
        if (!house)
        {
            Debug.LogWarning("FNRetreat: house が見つからないので撤退できない");
            yield break;
        }
        Debug.Log($"funnels={funnels.Length}");
        // funnelsが無い/空なら何もしない
        if (funnels == null || funnels.Length == 0)
        {
            Debug.Log("FNRetreat: funnels が空なので撤退不要");
            yield break;
        }

        // 攻撃停止（nullでも安全）
        if (fire != null)
        {
            StopCoroutine(fire);
            fire = null;
        }

       

        // 全員到達までループ
        while (true)
        {
            bool allReached = true;

            for (int i = 0; i < funnels.Length; i++)
            {

                var fn = funnels[i];
                if (!fn) continue;




                fn.transform.position = Vector3.MoveTowards(
                    fn.transform.position,
                    house.position,
                    moveSpeed * Time.deltaTime
                );

                if ((fn.transform.position - house.position).sqrMagnitude >= 0.01f)
                    allReached = false;
            }

            if (allReached) break;
            yield return null;
        }

        // 破壊＋配列クリア（後で再生成するなら大事）
        for (int i = 0; i < funnels.Length; i++)
        {
            if (funnels[i]) Destroy(funnels[i]);
            funnels[i] = null;
        }
    }

    public void InstanceFunnel(int funnelCount)
    {   
        
        //ファンネルをインスタンスする。
        //ファンネルを扇状に移動させる。
        if (!funnelPrefab)
        {
            Debug.LogError("funnelPrefab が未設定");
            return;
        }

        if (standardPoint == null)
        {
            Debug.LogError("instancePoint が未設定");
            return;
        }
        funnels = new GameObject[funnelCount];

        for (int i = 0; i < funnels.Length; i++)
        {
            Transform p = standardPoint;
            funnels[i] = Instantiate(funnelPrefab, p.position, p.rotation, p);
            funnels[i].name = $"Funnel_{i}";
            FunnelManager ifm = funnels[i].GetComponent<FunnelManager>();
            ifm.SetIsMoving(true);
            ifm.SetTarget(target);
            ifm.SetMyNum(i);
            ifm.SetMoveManager(this);
        }
        if (placeOnce != null) StopCoroutine(placeOnce);
        placeOnce = StartCoroutine(PlaceOnce());

    }
    [Header("Arc (Screen-based)")]
    [SerializeField] float avoidCenterDeg = 20f; // 上中央(90°)から±何度空ける
    [SerializeField] float radiusAdd = -1.5f;     // 半径加算

   

    IEnumerator PlaceOnce()
    {
        if (funnels == null || funnels.Length == 0) yield break;

        var cam = Camera.main;
        if (!cam)
        {
            Debug.LogError("cam is null");
            yield break;
        }

        Vector2 center = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)); // 画面下中心
        Vector2 top = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f)); // 画面上中心
        float radius = Vector2.Distance(center, top) + radiusAdd;

        int n = funnels.Length;
        // 0〜180 から 90±avoid を除外
        float rightEnd = 90f - avoidCenterDeg;
        float leftBeg = 90f + avoidCenterDeg;

        float arcRight = rightEnd - 0f;
        float arcLeft = 180f - leftBeg;
        float arcTotal = arcRight + arcLeft;


        int nRight = Mathf.RoundToInt(n * (arcRight / arcTotal));
        nRight = Mathf.Clamp(nRight, 0, n);
        int nLeft = n - nRight;
        int iR = 0;
        int iL = 0;

        for (int i = 0; i < n; i++)
        {
            float stepR = (nRight <= 1) ? 0f : 20f;
            float angleR(int i) => rightEnd - stepR * i; // i=0がrightEnd、増えるほど0へ

            float stepL = (nLeft <= 1) ? 0f : 20;
            float angleL(int j) => leftBeg + stepL * j;  // j=0がleftBeg、増えるほど180へ




                bool takeRight = false;
                float angleDeg;

                if (i % 2 == 0) takeRight = true;

                if (takeRight && iR < nRight)
                    angleDeg = angleR(iR++);
                else if (iL < nLeft)
                    angleDeg = angleL(iL++);
                else
                    angleDeg = angleR(iR++); // 左が尽きたら右だけ

                float rad = angleDeg * Mathf.Deg2Rad;
                Vector2 pos = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
                
                while(!StepMoveTowards(funnels[i].transform,pos,moveSpeed))
                {
                    yield return null;
                }

                funnels[i].GetComponent<FunnelManager>().SetIsMoving(false);
            

        }

        
    }

    bool StepMoveTowards(Transform tr, Vector3 targetPos, float speed)
    {   

        tr.position = Vector3.MoveTowards(tr.position, targetPos, speed * Time.deltaTime);
        bool check = (targetPos - tr.position).sqrMagnitude <= 0.01f;
        if(check)CompMove++;
        return check;
    }

    bool AnyFunnelMoving()//全部移動したか
{
    if (funnels == null) return false;

    foreach (var fn in funnels)
    {
        if (!fn) continue;
        var fm = fn.GetComponent<FunnelManager>();
        if (fm != null && fm.GetIsMoving()) return true;
    }
    return false; 
}







    [Header("攻撃パターン１")]


    [SerializeField] float fireTimeSpan = 1;
    IEnumerator FunnelFire(int fnNum)
{
    float ft = 1f;
    float tt = 0.5f;
    float fw = 0.5f;

    if (funnels == null || funnels.Length == 0) yield break;

    // 20: 1機ずつ連続
    if (fnNum == 20)
    {
        for (int i = 0; i < funnels.Length; i++)
        {
            var fn = funnels[i];
            if (!fn) continue;
            var fm = fn.GetComponent<FunnelManager>();
            if (!fm) continue;

            yield return new WaitWhile(AnyFunnelMoving);

            StartCoroutine(HomingTime(ft + tt, fm));
            fm.InstansBeam(tt, ft, fw);

            yield return new WaitForSeconds(fireTimeSpan);
        }
        yield break;
    }

    // 11: 全機同時
    if (fnNum == 11)
    {
        foreach (var fn in funnels)
        {
            if (!fn) continue;
            var fm = fn.GetComponent<FunnelManager>();
            if (!fm) continue;

            StartCoroutine(HomingTime(ft + tt, fm));
            fm.InstansBeam(tt, ft, fw);
        }
        yield break;
    }

    // それ以外: 指定番号だけ
    if (fnNum < 0 || fnNum >= funnels.Length) yield break;

    var targetFn = funnels[fnNum];
    if (!targetFn) yield break;

    var targetFm = targetFn.GetComponent<FunnelManager>();
    if (!targetFm) yield break;

    StartCoroutine(HomingTime(ft + tt, targetFm));
    targetFm.InstansBeam(tt, ft, fw);
}

    IEnumerator HomingTime(float time,FunnelManager fm)
    {
        center = !target? transform.position:target.transform.position;
        yield return new WaitUntil(()=> fm.GetBeamCondition() == 1||fm.GetBeamCondition()== 2);
        Debug.Log("BeamCondition=="+fm.GetBeamCondition());
        fm.SetHoming(true);
        yield return new WaitUntil(()=> fm.GetBeamCondition() == 0);
        fm.SetHoming(false);
    }
}
