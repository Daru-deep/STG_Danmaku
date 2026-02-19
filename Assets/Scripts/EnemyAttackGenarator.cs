using System.Collections;
using UnityEngine;

public class EnemyAttackGenarator : MonoBehaviour
{
   
    public GameObject missile;
    

    public Transform sendTarget ; 

    [SerializeField] GameObject Beam; 
    BeamTelegraphLine beam;
    
    [SerializeField] FunnelMove fnMove;


    void Awake()
    {
        if (Beam == null)   { Debug.LogError($"[{name}] Beam is not assigned");   enabled = false; return; }
        if (fnMove == null) { Debug.LogError($"[{name}] fnMove is not assigned"); enabled = false; return; }
        beam = Beam.GetComponent<BeamTelegraphLine>();
        if (beam == null)   { Debug.LogError($"[{name}] BeamTelegraphLine not found on Beam"); enabled = false; return; }
    }
    void Start()
    {
        StartCoroutine (AttackRoutine());
    }
    IEnumerator AttackRoutine()//デバッグ用の攻撃パターンを手書きで書いています：実装ではインスペクターから操作するように
    {   
       var wait = new WaitForSeconds(3);
       
       yield return wait;
    
        MissileAttack(10,60);

        yield return wait;

        BeamAttack(3,3,5);

        yield return wait;

        FunnelAttack();

        yield return new WaitForSeconds(15f);

        FunnelSimau();

        yield return new WaitForSeconds(10f);

        fnMove.GoAllRangeAttack();

        yield return wait;

        MissileAttack(20,170);
    }

void FunnelAttack()
    {
        fnMove.FirstMove();
    }

void FunnelSimau()
    {
        fnMove.FNSimau(transform);
    }
void MissileAttack(int missileCount,float spreadAngle)
{

    
    float baseAngle = transform.eulerAngles.z;

    float step  = (missileCount <= 1) ? 0f : spreadAngle / (missileCount - 1);
    float start = -spreadAngle * 0.5f;

    for (int i = 0; i < missileCount; i++)
    {
        float angle = baseAngle + start + step * i +180;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        GameObject missileOb = Instantiate(missile, transform.position, rot);
        missileOb.GetComponent<HomingMissileScript>().SetTarget(sendTarget);

        
    }
}


    void BeamAttack(float tt,float ft,float fw)
    {       
        beam.Fire(tt,ft,fw);
    }
}
