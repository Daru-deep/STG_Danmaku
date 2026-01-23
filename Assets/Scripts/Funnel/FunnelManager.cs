using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FunnelManager : MonoBehaviour
{
    [SerializeField]Transform target;//プレイヤーを中心点に

    [SerializeField]BeamTelegraphLine bm;

    [SerializeField] float speed = 8f;           // 前進速度
    [SerializeField] float turnRate = 1f;       // 旋回速度(度/秒)
    [SerializeField] float homingDelay = 0.15f; // 最初は直進する時間
    [SerializeField] float spriteAngleOffset = 180;// 初期補正
    
    public bool homing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
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
    Coroutine co;
    public void InstansBeam()
    {
        bm.Fire();
        
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(HomingTime());

        

    }

    IEnumerator HomingTime()
    {
        homing = true;
        yield return new WaitForSeconds(bm.telegraphTime + bm.fireTime);
        homing = false;
    }

    

    

    


}
