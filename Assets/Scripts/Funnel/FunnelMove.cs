using System.Collections;
using UnityEngine;

public class FunnelMove : MonoBehaviour
{

    [SerializeField] GameObject funnel;
    [SerializeField] GameObject[] funnels;
    [SerializeField] Transform[] instancePoint;
    private Coroutine fire;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstanceFunnel();
        fire = StartCoroutine(FunnelFire1());
        
        
    }
    void InstanceFunnel()
    {   
        int fnNum = 0;
        foreach(GameObject fn in funnels)
        {
            funnels[fnNum] = Instantiate(funnel,instancePoint[fnNum%2]);
            fnNum++;
        }
        
    }

IEnumerator FunnelFire1()
    {

    while (true)
    {
        yield return new WaitForSeconds(3f);

        foreach (GameObject fn in funnels)
        {
            if (!fn) continue;

            var fm = fn.GetComponent<FunnelManager>();
            if (!fm) continue;

            fm.InstansBeam();
        }
    }
    }
}
