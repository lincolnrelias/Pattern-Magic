using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject optionsPanel;
    [SerializeField]
    GameObject mainPanel;
    [SerializeField]
    TMP_Text nameField;
    IEnumerator Start() {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
    }
   public void IniciarJogo(){
        SceneManager.LoadScene("Main",LoadSceneMode.Single);
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
    public void setName(){
        string newName="Convidado";
        if(nameField.text!=""){
            newName=nameField.text;
        }
        PlayerPrefs.SetString("PlayerName",newName);
    }
}
