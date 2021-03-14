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
    Slider castleHealthSlider;
    [SerializeField]
    TMP_Text castleHealthDisplay;
    [SerializeField]
    GameObject btnFacilChecked;
    [SerializeField]
    GameObject btnMedioChecked;
    [SerializeField]
    GameObject btnPTodos;
    Menu menu;
    private void Start() {
        menu = FindObjectOfType<Menu>();
        castleHealthSlider.value=PlayerPrefs.HasKey("CastleHealth")?PlayerPrefs.GetFloat("CastleHealth"):100;
        volumeSlider.value=PlayerPrefs.HasKey("Volume")?PlayerPrefs.GetFloat("Volume"):.6f;
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
    public void setName(){
        PlayerPrefs.SetString("PlayerName",nameField.text);
        menu.playClickSound();
    }
    public void setVolume(){
        PlayerPrefs.SetFloat("Volume",volumeSlider.value);
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
    public void GerarRelatorio(){
        PlayerPrefs.SetString("GerarRelatorio","sim");
    }
    public void NGerarRelatorio(){
        PlayerPrefs.SetString("GerarRelatorio","nao");
    }
     private void LateUpdate() {
         castleHealthDisplay.SetText(Mathf.RoundToInt(castleHealthSlider.value).ToString());
        if(PlayerPrefs.GetInt("ModoPunicao")==2){
            btnPTodos.SetActive(true);
        }
        if(PlayerPrefs.GetString("Dificuldade")=="fácil"){
            btnFacilChecked.SetActive(true);
            btnMedioChecked.SetActive(false);
        }
        if(PlayerPrefs.GetString("Dificuldade")=="médio"){
            btnFacilChecked.SetActive(false);
            btnMedioChecked.SetActive(true);
        }
     }
}
