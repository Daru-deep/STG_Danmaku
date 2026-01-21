using System.Collections;
using UnityEngine;

public class EnemyAttackGenarator : MonoBehaviour
{
    int missileCount = 5;
    public GameObject missile;
    private float baseAngle = 0;
    public float spreadAngle = 60;

    public Transform sendTarget ; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MissileAttackCoroutine());
    }
    IEnumerator MissileAttackCoroutine()
    {
        MissileAttack();
        yield return new WaitForSeconds(10f);
        StartCoroutine(MissileAttackCoroutine());
    }

    // Update is called once per frame
    void MissileAttack()
    {
        baseAngle = transform.eulerAngles.z;
        float step = (missileCount <= 1) ? 0f : spreadAngle / (missileCount - 1);
        float start = -spreadAngle *0.5f;

    for (int i = 0; i < missileCount; i++)
    {
        float angle = baseAngle + start + step * i;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);
        GameObject missileOb = Instantiate(missile, transform.position, rot);
        missileOb.GetComponent<HomingMissileScript>().target = sendTarget;
    }
        
    }
}
