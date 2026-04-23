using UnityEngine;
using TMPro;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    [SerializeField]ParticleSystem[] part;
    [SerializeField]TextMeshProUGUI hptxt ;

    [SerializeField]SceneChange sceneChange;

    public bool inGame = true;

    public void ChangeHPTxt(float hp)
    {
        hptxt.text = $"HP_:{hp}";
    }

    public void GameOver()
    {
        Invoke("GoStartScene",5f);
    }

    public void GameClear()
    {
        Invoke("GoStartScene",5f);
    }

    private void GoStartScene()
    {
        
        if(sceneChange != null)
        {
            sceneChange.ChangeScene(2);
        } else
        {
            Debug.LogError("SceneManager is null in the GameManager");
        }
    }

    public void StartParticle(int key,Transform tr ,  float time,float size)
    {
        ParticleSystem system = (part.Length>key)? Instantiate(part[key]):Instantiate(part[0]);
        system.transform.position = tr.position;
        system.gameObject.transform.localScale = new Vector3(size,size,0);
        Destroy(system.gameObject,time);        
    }

}
