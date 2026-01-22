using System.Collections;
using UnityEngine;


public class PlayerAttackGenarator : MonoBehaviour
{

    public GameObject bullet;
    public GameObject me;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(BulletFire());
    }

    void Update()
    {
        transform.position = me.transform.position;
    }

    private IEnumerator BulletFire()
    {  
        while(true)
        {   Quaternion rot = Quaternion.Euler(0f, 0f, 0f);
            Instantiate(bullet,transform.position,rot);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
