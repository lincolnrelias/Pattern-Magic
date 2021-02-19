using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class Button : MonoBehaviour{
    
     Webcam webcamInstance;
    [SerializeField]
    int offsetY = 100;
    RectTransform rt;
    public bool canCheck = false;
    Color32 checkedColor;
    GameObject checkedEffect;
    Color32 uncheckedColor;
    AudioSource audioSource;
    Vector3 effectOffset;
    AudioClip pointSound;
    Image img;
    int x,y,s;

    void Start(){
        webcamInstance = FindObjectOfType<Webcam>();
        img = GetComponent<Image>();
         rt = GetComponent<RectTransform>();
         audioSource = GetComponent<AudioSource>();
         //Tenta pegar o componente do pai
         patternExample pattern  = transform.parent.GetComponent<patternExample>();
         //Se não der certo pega do primeiro filho do pai
         if(!pattern){pattern = transform.parent.GetChild(0).GetComponent<patternExample>();};
         checkedEffect = pattern.getPointEffect();
         pointSound =pattern.getPointSound();
         uncheckedColor = pattern.getColors()[1];
         checkedColor = pattern.getColors()[0];
         effectOffset = pattern.getEffectOffset();
        }
    public void switchColor(){
        if(img.color==checkedColor){
            img.color = uncheckedColor;
        }else{
            img.color = checkedColor;
        }
    }
    void Update(){
        // int x = (int) (100*webcamInstance.scale),
        //     y = (int) (190*webcamInstance.scale),
        //     s = (int) (100*webcamInstance.scale);
             x = (int) (Mathf.Abs(rt.anchoredPosition.x)*webcamInstance.scale);
            y = (int) ((Mathf.Abs(rt.anchoredPosition.y-offsetY))*webcamInstance.scale);
            s = (int) (rt.rect.height*webcamInstance.scale);
        
        if(webcamInstance.checkArea( x, y, s, s ) ){
            if(canCheck && img.color!=checkedColor){
            img.color = checkedColor;
            GetComponent<Animator>().SetTrigger("Check");
            GameObject effectTemp = Instantiate(checkedEffect,transform.position+effectOffset,Quaternion.identity);
            effectTemp.transform.SetParent(transform.parent.parent);
            GameObject.Destroy(effectTemp,2f);
            if(!audioSource.isPlaying){
               audioSource.PlayOneShot(pointSound); 
                }
            }else if(img.color==uncheckedColor){
                int modoPun = PlayerPrefs.HasKey("ModoPunicao")?PlayerPrefs.GetInt("ModoPunicao"):2;
                FindObjectOfType<Pattern>().errorAdd(transform.GetSiblingIndex(),modoPun);
            }
        }
    }
}
