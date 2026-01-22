using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]private float HP = 100;
    [SerializeField] private GameManager gm;

    int bulletLayer;

    void Awake()
    {
        bulletLayer = LayerMask.NameToLayer("Bullet");
    }
    public void ChangHP(float right)
    {
        HP -= right;
    }

    public float GetHP()
    {
        return HP;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        int layer = other.gameObject.layer;
        if(layer == bulletLayer)
        {
            ChangHP(1);
            gm.ChangeHPTxt(GetHP());
        }
    }
}
