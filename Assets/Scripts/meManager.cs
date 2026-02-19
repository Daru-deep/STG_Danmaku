using System.Collections;
using UnityEngine;

public class meManager : MonoBehaviour
{

    [SerializeField] private GameManager gm;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private ParticleSystem particle;

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

        //この上に書く
        yield return new WaitForSeconds(0.5f);
        isParry = false;
    }

    public void PlayerDown()
    {
        if(!isParry)
        {
            Destroy(this.gameObject);
            gm.GameOver();
        }
        else
        {
            // パーティクルシステムのインスタンスを生成する。
			ParticleSystem newParticle = Instantiate(particle);
			// パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所にする。
			newParticle.transform.position = this.transform.position;
			// パーティクルを発生させる。
			newParticle.Play();
			// インスタンス化したパーティクルシステムのGameObjectを5秒後に削除する。(任意)
			// ※第一引数をnewParticleだけにするとコンポーネントしか削除されない。
			Destroy(newParticle.gameObject, 5.0f);
        }
    }
}
