using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject pauseScreen;
    [SerializeField]
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.P)){
            Time.timeScale=0;
            pauseScreen.SetActive(true);
        }
    }
    public void resume(){
        Time.timeScale=1;
        pauseScreen.SetActive(false);
    }
    public void menu(){
        SceneManager.UnloadSceneAsync("Main");
        SceneManager.LoadScene("Menu");
    }
}
