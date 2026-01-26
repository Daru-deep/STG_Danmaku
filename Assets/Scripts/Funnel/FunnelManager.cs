using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FunnelManager : MonoBehaviour
{
    private Transform target;//プレイヤーを中心点に

    [SerializeField]BeamTelegraphLine bm;

    [SerializeField] float turnRate = 1f;       // 旋回速度(度/秒)

    [SerializeField] float spriteAngleOffset = 180;// 初期補正

    [SerializeField] FunnelMove fm;

    private int myNum = 0;
    
    public bool homing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {   
        if(!fm)
        {   
            Debug.LogError($"moveManager is null in Funnel at No.[{myNum}] :-( ");
            return;
        }

        float dt = Time.deltaTime;
        if(fm.homing) return;
        if (!target) return;
        Vector2 to = (Vector2)target.position - (Vector2)transform.position;
        float targetAngle = Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg + 90;
        targetAngle += spriteAngleOffset;

        float newAngle = Mathf.MoveTowardsAngle(
            transform.eulerAngles.z,
            targetAngle,
            turnRate * dt
        );

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }
    public void SetMyNum(int i) => myNum = i;

    public void SetTarget(Transform set)
    {
        target = set;
    }

    public void SetMoveManager(FunnelMove set)
    {
        fm = set;
    }
    Coroutine co;
    public void InstansBeam(float ft,float tt,float fw)
    {
        bm.Fire(ft,tt,fw);
    }



    

    

    


}
