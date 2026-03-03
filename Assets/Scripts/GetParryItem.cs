using Unity.Mathematics;
using UnityEngine;

public class GetParryItem : MonoBehaviour
{

    [SerializeField]BeamTelegraphLine bm;
    [SerializeField]GameObject[] presentsList;
    [SerializeField]LayerMask missileLayer;

    [SerializeField] meManager mM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if ((missileLayer & (1 << layer)) != 0 && mM.isParry)
        {
            other.GetComponent<HomingMissileScript>().DestroyMissile();
            GameObject presentMissile = Instantiate(presentsList[0], this.transform);
            HomingMissileScript MiSc = presentMissile.GetComponent<HomingMissileScript>();
            var enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (MiSc != null && enemy != null)
            {
                MiSc.target = enemy.transform;
                Vector2 incomingDir = -(Vector2)other.transform.up;
                Vector2 normal = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
                Vector2 reflectedDir = Vector2.Reflect(incomingDir, normal);
                float angle = Mathf.Atan2(reflectedDir.y, reflectedDir.x) * Mathf.Rad2Deg + 90f;
                presentMissile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        
    }

    
}
