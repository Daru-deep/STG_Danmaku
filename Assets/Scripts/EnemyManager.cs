using UnityEngine;
using System.Collections;


public class EnemyManager : MonoBehaviour
{
    [SerializeField]private float HP = 10;
    [SerializeField] private GameManager gm;

    [SerializeField] private ParticleSystem breakEffect;


    
    int bulletLayer;
    int parryLayer;

    void Awake()
    {
        bulletLayer = LayerMask.NameToLayer("Bullet");
        parryLayer = LayerMask.NameToLayer("Parry");
    }
    public void ChangHP(float right)
    {
        HP -= right;
        if (HP <= 0) BreakBoss();
        gm.ChangeHPTxt(GetHP());
    }

    public float GetHP()
    {
        return HP;
    }

    void BreakBoss()
    {
            // パーティクルシステムのインスタンスを生成する。
			ParticleSystem newParticle = Instantiate(breakEffect);
			// パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所にする。
			newParticle.transform.position = this.transform.position - new Vector3  (0,2,0);
            newParticle.transform.localScale = new Vector3(10,10,0);
			// パーティクルを発生させる。
			newParticle.Play();
			// インスタンス化したパーティクルシステムのGameObjectを5秒後に削除する。(任意)
			// ※第一引数をnewParticleだけにするとコンポーネントしか削除されない。
			Destroy(newParticle.gameObject, 3.0f);
            Destroy(gameObject);
    }



    private void OnTriggerEnter2D(Collider2D other)
     {
        
    
        int layer = other.gameObject.layer;
        if(layer == bulletLayer)
        {
            ChangHP(1);
            gm.StartParticle(1,other.transform,1,0.5f);
        }

    }



}
