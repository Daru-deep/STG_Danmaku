using System;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.Timeline;

public class FunnelMove : MonoBehaviour
{

    [SerializeField] GameObject funnelPrefab;

    [SerializeField] Transform[] instancePoint;

    [SerializeField] Transform target;



    public bool homing;

    

    GameObject[] funnels;     // ← 実体を保持する配列
    Coroutine fire;

    Coroutine move;

    //移動
    [SerializeField] float moveSpeed = 10f;

    //回転
    [SerializeField] float radius = 3f;      // 半径
    [SerializeField] float angularSpeed = 90f; // 度/秒
    [SerializeField] float baseAngle;        // 全体の回転角（積算）



    

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void FirstMove()

    {
        InstanceFunnel(2);
        if(fire != null)StopCoroutine(fire);
        fire = StartCoroutine(FunnelFire1());
        
        
    }

    Vector3 center;

    void Update()
    {

    }
    public void FNSimau(Transform house)
    {   if(move != null)
        {
        StopCoroutine(move);
        move = null;
        }
        move = StartCoroutine(FNRetreat(house));
    }
    IEnumerator FNPointAttack()
    {
        InstanceFunnel(4);
        while(true)
        {
        if (!target){Debug.Log("ターゲットがいませんのでFNPointAttackを実行できません");break;}
        if (funnels == null || funnels.Length == 0){Debug.Log("ファンネルがいませんのでFNPointAttackを実行できません");break;}
        baseAngle += angularSpeed * Time.deltaTime;

        for (int i = 0; i < funnels.Length; i++)
        {
            var fn = funnels[i];

            float step = 360f / funnels.Length; // 等間隔
            float angle = baseAngle + step * i; // 各ファンネルの角度

            // 角度→位置（2D）
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
            if(!homing)fn.transform.position = target.position + offset;
            if(homing) fn.transform.position = center;
        }

        yield return new WaitForEndOfFrame();
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

    const float arriveDist = 0.005f;
    float arriveSqr = arriveDist * arriveDist;

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

            if ((fn.transform.position - house.position).sqrMagnitude > arriveSqr)
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
        if (!funnelPrefab)
        {
            Debug.LogError("funnelPrefab が未設定");
            return;
        }

        if (instancePoint == null || instancePoint.Length == 0)
        {
            Debug.LogError("instancePoint が未設定");
            return;
        }
        funnels = new GameObject[funnelCount];

         for (int i = 0; i < funnels.Length; i++)
        {
            Transform p = instancePoint[i % instancePoint.Length];
            funnels[i] = Instantiate(funnelPrefab, p.position, p.rotation, p);
            funnels[i].name = $"Funnel_{i}";
            FunnelManager ifm = funnels[i].GetComponent<FunnelManager>();
            ifm.SetTarget(target);
            ifm.SetMyNum(i);
            ifm.SetMoveManager(this);
        }
        
    }

    
[Header ("攻撃パターン１")]
[SerializeField] float fnWaitTime = 10f;
IEnumerator FunnelFire1()
    {
        
        float ft = 3;
        float tt = 5;
        float fw = 6;

    while (true)
    {
        yield return new WaitForSeconds(fnWaitTime);

        foreach (GameObject fn in funnels)
        {
            if (!fn) continue;

            var fm = fn.GetComponent<FunnelManager>();
            if (!fm) continue;

            fm.InstansBeam(ft,tt,fw);
            Coroutine hm = StartCoroutine(HomingTime(ft+fw));
        }
    }
    }
        IEnumerator HomingTime(float time)
    {

        homing = true;
        center = this.target.transform.position;
        yield return new WaitForSeconds(time);
        homing = false;
       yield return new WaitForSeconds(10f);
    }
}
