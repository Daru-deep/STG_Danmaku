using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class gunManager : MonoBehaviour
{
    Vector2 top ;
    public float speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         var cam = Camera.main;
        if (!cam)
        {
            Debug.LogError("cam is null");
            return;
        }

        Vector2 center = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)); // 画面下中心
        top = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f)); // 画面上中心
    }

    // Update is called once per frame
    private void FixedUpdate() 
    {
        if(transform.position.y > top.y)
        {
            Destroy(this.gameObject);
        }
        else
        {
            float dt = Time.deltaTime;
            transform.Translate(transform.up * dt *speed);
        }

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
