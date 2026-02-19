using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FunnelManager : MonoBehaviour
{
    private Transform target;//プレイヤーを中心点に

    [SerializeField]BeamTelegraphLine bm;

    [SerializeField] float turnRate = 1f;       // 旋回速度(度/秒)

    [SerializeField] float spriteAngleOffset = 180;// 初期補正

    [SerializeField] FunnelMove funnelMove;

    private int myNum = 0;

    private bool isMoving = false;
    
    private bool homing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {   
        if(!funnelMove)
        {   
            Debug.LogError($"moveManager is null in Funnel at No.[{myNum}] :-( ");
            return;
        }

        float dt = Time.deltaTime;
        if(homing) return;
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

    public int GetBeamCondition()
    {
        return bm.beamCondition;
    }

    public bool GetHoming()
    {
        return homing;
    }

    public void SetHoming(bool hm)
    {
        homing = hm;
    }


    public void SetMyNum(int i) => myNum = i;

    public int GetMyNum()
    {
        return myNum;
    }

    public void SetTarget(Transform set)
    {
        target = set;
    }

    public void SetMoveManager(FunnelMove set)
    {
        funnelMove = set;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    public void SetIsMoving(bool moveing)
    {
        isMoving = moveing;
    }

    public void InstansBeam(float tt,float ft,float fw)
    {   
        bm.Fire(tt,ft,fw);
    }



    

    

    


}
