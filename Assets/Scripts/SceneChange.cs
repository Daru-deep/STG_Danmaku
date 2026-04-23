using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ChangeScene(int num)
    {
        switch(num)
        {
            case 0:
                SceneManager.LoadScene("SousaScene");
                break;
            case 1:
                SceneManager.LoadScene("GameScene");
                break;
            case 2:
                SceneManager.LoadScene("StartScene");
                break;
        }
    }
}
