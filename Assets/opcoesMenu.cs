using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class opcoesMenu : MonoBehaviour
{
    [SerializeField]
    TMP_Text nameField;
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
}
