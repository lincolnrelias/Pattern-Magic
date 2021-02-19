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
    // Start is called before the first frame update
    public void setDFácil(){
        PlayerPrefs.SetString("Dificuldade","fácil");
    }
    public void setDMédio(){
         PlayerPrefs.SetString("Dificuldade","médio");
    }
    public void setName(){
        PlayerPrefs.SetString("PlayerName",nameField.text);
    }
    public void setVolume(){
        PlayerPrefs.SetFloat("Volume",volumeSlider.value);
    }
    public void setCastleHealth(){
        PlayerPrefs.SetFloat("CastleHealth",castleHealthSlider.value);
    }
    public void setP1Nodo(){
        PlayerPrefs.SetInt("ModoPunicao",1);
    }
    public void setPTodos(){
        PlayerPrefs.SetInt("ModoPunicao",2);
    }
     private void LateUpdate() {
         castleHealthDisplay.SetText(Mathf.RoundToInt(castleHealthSlider.value).ToString());
     }
}
