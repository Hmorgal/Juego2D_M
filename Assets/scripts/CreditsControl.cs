using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsControl : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

        float delay = 3;

        delay = delay - Time.deltaTime;

        if (Input.anyKey && delay < 0){
            
            SceneManager.LoadScene("MainMenu");

        }
        
    }
}
