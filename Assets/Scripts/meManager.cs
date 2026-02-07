using System.Collections;
using UnityEngine;

public class meManager : MonoBehaviour
{

    [SerializeField] private GameManager gm;
    [SerializeField] private LayerMask playerMask;

    public bool isParry = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyAttack"))
        {
            if(isParry)
            {
                StartCoroutine (GoParry());//円のエフェクトとか付けたいね。あとコントローラに登録してないよ
                return;
            }
            else
            {
                PlayerDown();
            }
        }
    }

    IEnumerator GoParry()
    {
        yield return new WaitForSeconds(0.5f);
        isParry = false;
    }

    public void PlayerDown()
    {
        Destroy(this.gameObject);
        gm.GameOver();
    }
}
