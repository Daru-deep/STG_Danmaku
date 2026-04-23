using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Unity.Mathematics;

public class GetParryItem : MonoBehaviour
{

    [SerializeField]BeamTelegraphLine bm;
    [SerializeField]GameObject[] presentsList;
    [SerializeField]LayerMask missileLayer;

    [SerializeField] GameManager gm;

    [SerializeField] meManager mM;

    [SerializeField] SpriteRenderer parryEffect;

    [SerializeField] private ParticleSystem ParryParticle;
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
            other.GetComponent<HomingMissileScript>().IsParryMissile();
            Vector2 tr =new Vector2(transform.position.x,transform.position.y);
            GameObject presentMissile = Instantiate(presentsList[0],tr,quaternion.identity);

            HomingMissileScript MiSc = presentMissile.GetComponent<HomingMissileScript>();
            var enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (MiSc != null && enemy != null)
            {
                StartCoroutine(GoParry(other.name));
                MiSc.target = enemy.transform;
                Vector2 incomingDir = -(Vector2)other.transform.up;
                Vector2 normal = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
                Vector2 reflectedDir = Vector2.Reflect(incomingDir, normal);
                float angle = Mathf.Atan2(reflectedDir.y, reflectedDir.x) * Mathf.Rad2Deg + 90f;
                presentMissile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        
    }

    IEnumerator GoParry(string type)
    {       
            string otherName = type;
            Debug.Log($"PARRY!!{otherName}");
            // パーティクルシステムのインスタンスを生成する。
            ParticleSystem parryP = Instantiate(ParryParticle);
   
            
			// パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所にする。
            parryP.transform.position = this.transform.position;
            
			// パーティクルを発生させる。
            parryP.Play();
			// インスタンス化したパーティクルシステムのGameObjectを5秒後に削除する。(任意)
			// ※第一引数をnewParticleだけにするとコンポーネントしか削除されない。
            Destroy(parryP.gameObject,1.0f);
            //isParry = false;
            yield return null;
        
    }



    
}
