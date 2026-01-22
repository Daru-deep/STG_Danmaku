using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI hptxt ;

    public void ChangeHPTxt(float hp)
    {
        hptxt.text = $"HP_:{hp}";
    }

    public void GameOver()
    {
        Debug.Log("げーむおーばー");
    }


}
