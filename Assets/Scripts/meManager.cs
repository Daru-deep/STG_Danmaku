using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class meManager : MonoBehaviour
{

    [SerializeField] private GameManager gm;
    
    [SerializeField] LayerMask missileLayer;



  



    public bool isParry = false;
    
    void Awake()
    {
        if (gm == null) { Debug.LogError($"[{name}] GameManager is not assigned"); enabled = false; return; }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {


        int layer = other.gameObject.layer;
        if (!isParry&&(missileLayer & (1 << layer)) != 0) PlayerDown("Missile");
    }

    void StartGoParry()
    {
        
    }
    




    public void PlayerDown(string type)
    {

            Destroy(this.gameObject);
            gm.GameOver();

    }
}
