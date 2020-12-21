using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject optionsPanel;
    [SerializeField]
    GameObject mainPanel;
    IEnumerator Start() {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
    }
   public void IniciarJogo(){
        SceneManager.LoadScene("Main");
    }
    public void FecharJogo(){
        Application.Quit();
    }
    public void OpcoesAbrir(){
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void OpcoesFechar(){
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
}
