using System.Collections;
using UnityEngine;

public class HomingMissileScript : MonoBehaviour
{
    [SerializeField] float speed = 8f;          // 前進速度
    [SerializeField] float turnRate = 360f;     // 旋回速度(度/秒)
    [SerializeField] float homingDelay = 0.15f; // 最初は直進する時間

    public Transform target;

    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask bulletLayer;
    float timer;

    int mode = 0;

    public void SetTarget(Transform t) => target = t;

    void Awake()
    {
        if (gameObject.CompareTag("MyAttack"))
        {
            mode = 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        timer += dt;

        transform.position += -transform.up * speed * dt;

        if(timer < homingDelay || !target)return;

        Vector2 to = (Vector2)target.position - (Vector2)transform.position;
        float targetAngle = Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg + 90;


        
        float newAngle = Mathf.MoveTowardsAngle(
            transform.eulerAngles.z,
            targetAngle,
            turnRate * dt
            
        );
        
        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }



   private void OnTriggerEnter2D(Collider2D other) {
   
    {
        int layer = other.gameObject.layer;
        if(mode == 0)
        {
            if ((playerLayer & (1 << layer)) != 0 || (bulletLayer & (1 << layer)) != 0)//ミサイルヒット
            {
                if(mode == 0)DestroyMissile();
            }
        }

        if(mode == 1)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyManager>().ChangHP(100);
                DestroyMissile();
            }
        }

        
    }
   }

   public void DestroyMissile()
    {
         Destroy(this.gameObject);
    }

}


