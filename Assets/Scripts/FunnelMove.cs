using System.Collections;
using UnityEngine;

public class FunnelMove : MonoBehaviour
{

    [SerializeField] GameObject[] funnels;
    private Coroutine fire;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fire = StartCoroutine(FunnelFire1());
    }

IEnumerator FunnelFire1()
    {
        while(true){
        yield return new WaitForSeconds(3f);
        foreach(GameObject fn in funnels)
        {
            fn.GetComponent<FunnelManager>().InstansBeam();   
        }
        }
    }
}
