using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{

    public void StartGame(){

        SceneManager.LoadScene("Level_1");

    }

    public void ExitGame(){

        Application.Quit();

    }

}
