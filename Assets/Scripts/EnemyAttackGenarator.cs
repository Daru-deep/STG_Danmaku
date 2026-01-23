using System.Collections;
using UnityEngine;

public class EnemyAttackGenarator : MonoBehaviour
{
    int missileCount = 5;
    public GameObject missile;
    public float spreadAngle = 60;

    public Transform sendTarget ; 

    [SerializeField] GameObject Beam;    
    Coroutine missileLoop;

    Coroutine BeamLoop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       missileLoop = StartCoroutine(MissileAttackCoroutine());
       BeamLoop = StartCoroutine(BeamAttack());
    }
    IEnumerator MissileAttackCoroutine()
    {
        while(true)
        {
        MissileAttack();
        yield return new WaitForSeconds(10f);
        }
    }

    
void MissileAttack()
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

    IEnumerator BeamAttack()
    {

        BeamTelegraphLine beam = Beam.GetComponent<BeamTelegraphLine>();
        while (true)
        {   
            yield return new WaitForSeconds(15f);
            beam.Fire();                      
            
            
        }
    }
}
