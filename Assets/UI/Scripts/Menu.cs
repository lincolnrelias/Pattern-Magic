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
    AudioClip btnClickSound;
    AudioSource audioSource;
    IEnumerator Start() {
        audioSource= GetComponent<AudioSource>();
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
        if(!PlayerPrefs.HasKey("ModoPunicao")){
            PlayerPrefs.SetInt("ModoPunicao",2);}
        if(!PlayerPrefs.HasKey("CastleHealth")){
            PlayerPrefs.SetFloat("CastleHealth",300f);}
        if(!PlayerPrefs.HasKey("Dificuldade")){
            PlayerPrefs.SetString("Dificuldade","fácil");}  
        
    }
    public void playClickSound(){
        audioSource.PlayOneShot(btnClickSound);
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
        playClickSound();
    }
    public void OpcoesFechar(){
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        playClickSound();
    }
}
