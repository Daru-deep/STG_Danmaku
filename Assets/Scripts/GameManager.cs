using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI hptxt ;

    public bool inGame = true;

    public void ChangeHPTxt(float hp)
    {
        hptxt.text = $"HP_:{hp}";
    }

    public void GameOver()
    {
        Debug.Log("げーむおーばー");
    }

    public void GameClear()
    {
        
    }


}
