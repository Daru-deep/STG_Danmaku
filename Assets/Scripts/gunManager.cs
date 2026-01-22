using TMPro;
using UnityEngine;

public class gunManager : MonoBehaviour
{
    
    public float speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate() {
        float dt = Time.deltaTime;
        transform.Translate(transform.up * dt *speed);

    }

 private void OnTriggerEnter2D(Collider2D other) {
   
    {
        if (other.gameObject.CompareTag("Enemy")||other.gameObject.CompareTag("Bullet"))//ミサイルヒット
        {
            Destroy(this.gameObject);

        }

        
    }
 }

}
