using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class opcoesMenu : MonoBehaviour
{
    [SerializeField]
    TMP_Text nameField;
    [SerializeField]
    Slider volumeSlider;
    [SerializeField]
    Slider musicVolSlider;
    [SerializeField]
    Slider castleHealthSlider;
    [SerializeField]
    TMP_Text castleHealthDisplay;
    [SerializeField]
    GameObject btnFacilChecked;
    [SerializeField]
    GameObject btnMedioChecked;
    [SerializeField]
    GameObject btnCustomChecked;
    [SerializeField]
    TMP_Text currentSequenceText;
    [SerializeField]
    Toggle nMandalasToggle;
    Menu menu;
    private void Start() {
        menu = FindObjectOfType<Menu>();
        nMandalasToggle.isOn = PlayerPrefs.GetInt("MostrarNumeroMandalas")==1?true:false;
        castleHealthSlider.value=PlayerPrefs.GetFloat("CastleHealth");
        volumeSlider.value=PlayerPrefs.GetFloat("Volume");
        musicVolSlider.value=PlayerPrefs.GetFloat("volMusica");
    }
    // Start is called before the first frame update
    public void setDFácil(){
        PlayerPrefs.SetString("Dificuldade","fácil");
        menu.playClickSound();
    }
    public void setDMédio(){
         PlayerPrefs.SetString("Dificuldade","médio");
         menu.playClickSound();
    }
    public void setCustom(){
        PlayerPrefs.SetString("Dificuldade","custom");
         menu.playClickSound();
    }
    public void setName(){
        PlayerPrefs.SetString("PlayerName",nameField.text);
        menu.playClickSound();
    }
    public void downloadReport(){
        GeradorRelatório.downloadReport();
    }
    public void setVolume(){
        PlayerPrefs.SetFloat("Volume",volumeSlider.value);
    }
    public void setVolMusica(){
        PlayerPrefs.SetFloat("volMusica",musicVolSlider.value);
    }
    public void setCastleHealth(){
        PlayerPrefs.SetFloat("CastleHealth",castleHealthSlider.value);
    }
    public void setP1Nodo(){
        PlayerPrefs.SetInt("ModoPunicao",1);
        menu.playClickSound();
    }
    public void setPTodos(){
        PlayerPrefs.SetInt("ModoPunicao",2);
        menu.playClickSound();
    }
    public void changeMostrarNMandalas(){
        PlayerPrefs.SetInt("MostrarNumeroMandalas",nMandalasToggle.isOn?1:0);
        if(menu){
            menu.playClickSound();
        }
    }
    public void GerarRelatorio(){
        PlayerPrefs.SetString("GerarRelatorio","sim");
        menu.playClickSound();
    }
    public void NGerarRelatorio(){
        PlayerPrefs.SetString("GerarRelatorio","nao");
        menu.playClickSound();
    }
     private void LateUpdate() {
         castleHealthDisplay.SetText(Mathf.RoundToInt(castleHealthSlider.value).ToString()+"s");
         currentSequenceText.text = "Sequência personalizada escolhida:\n"+PlayerPrefs.GetString("currentSequence");
        if(PlayerPrefs.GetString("Dificuldade")=="fácil"){
            btnFacilChecked.SetActive(true);
            btnMedioChecked.SetActive(false);
            btnCustomChecked.SetActive(false);
        }
        if(PlayerPrefs.GetString("Dificuldade")=="médio"){
            btnFacilChecked.SetActive(false);
            btnMedioChecked.SetActive(true);
            btnCustomChecked.SetActive(false);
        }
        if(PlayerPrefs.GetString("Dificuldade")=="custom"){
            btnFacilChecked.SetActive(false);
            btnMedioChecked.SetActive(false);
            btnCustomChecked.SetActive(true);
        }
     }
}
