using System.Collections;
using UnityEngine;

public class EnemyAttackGenarator : MonoBehaviour
{
    int missileCount = 5;
    public GameObject missile;
    private float BaseAngle = 0;
    public float spreadAngle = 60;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MissileAttack();
    }
    IEnumerator MissileAttack()
    {
        MissileAttack();
        yield return new WaitForSeconds(10f);
        StartCoroutine(MissileAttack());
    }

    // Update is called once per frame
    void MissleAttack()
    {
        BaseAngle = transform.eulerAngles.z;
        float step = (missileCount <= 1) ? 0f : spreadAngle / (missileCount - 1);
        
       
        
    }
}
