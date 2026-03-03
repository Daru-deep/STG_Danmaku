using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class meManager : MonoBehaviour
{

    [SerializeField] private GameManager gm;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] LayerMask missileLayer;
    [SerializeField] SpriteRenderer parryEffect;

    float EffectFeedSpeed = 1f;
  



    public bool isParry = false;
    
    void Awake()
    {
        if (gm == null) { Debug.LogError($"[{name}] GameManager is not assigned"); enabled = false; return; }
        if(parryEffect != null) parryEffect.color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {


        int layer = other.gameObject.layer;
        if ((missileLayer & (1 << layer)) != 0) PlayerDown("Missile");
    }

    void StartGoParry()
    {
        
    }
    
    IEnumerator GoParry(string type)
    {       
            string otherName = type;
            Debug.Log($"PARRY!!{otherName}");
            // パーティクルシステムのインスタンスを生成する。
			ParticleSystem newParticle = Instantiate(particle);
            StartCoroutine(EffectON());
            
			// パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所にする。
			newParticle.transform.position = this.transform.position;
            
			// パーティクルを発生させる。
			newParticle.Play();
			// インスタンス化したパーティクルシステムのGameObjectを5秒後に削除する。(任意)
			// ※第一引数をnewParticleだけにするとコンポーネントしか削除されない。
			Destroy(newParticle.gameObject, 1.0f);
            //isParry = false;
            yield return null;
        
    }

    IEnumerator EffectON()
    {
        float opacity = 1f;
        while (opacity > 0f)
        {
            opacity -= EffectFeedSpeed * Time.deltaTime;
            if (parryEffect != null)
                parryEffect.color = new Color(1f, 1f, 1f, Mathf.Max(0f, opacity));
            yield return null;
        }
    }

    public void PlayerDown(string type)
    {
        if(!isParry)
        {
            Destroy(this.gameObject);
            gm.GameOver();
        }
        else
        {

                StartCoroutine (GoParry(type));//円のエフェクトとか付けたいね。あとコントローラに登録してないよ

        }
    }
}
