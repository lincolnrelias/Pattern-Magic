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
    Rigidbody2D rb;
    int x,y,h,w;

    void Start(){
        webcamInstance = FindObjectOfType<Webcam>();
        img = GetComponent<Image>();
         rt = GetComponent<RectTransform>();
         audioSource = GetComponent<AudioSource>();
        rb=gameObject.AddComponent<Rigidbody2D>();
         rb.bodyType=RigidbodyType2D.Static;
         rt.localScale=new Vector3(1.3f,1.3f,1f);
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
    public void ejectButton(){
        rb.bodyType=RigidbodyType2D.Dynamic;
        rb.gravityScale=50;
         rb.mass=10;
         float xForce=Random.Range(-800f,800f);
        rb.AddForce(new Vector2(xForce,1000f),ForceMode2D.Impulse);
    }
    void Update(){
        // int x = (int) (100*webcamInstance.scale),
        //     y = (int) (190*webcamInstance.scale),
        //     s = (int) (100*webcamInstance.scale);
             x = (int) (Mathf.Abs(rt.anchoredPosition.x)*webcamInstance.scaleHorizontal);
            y = (int) ((Mathf.Abs(rt.anchoredPosition.y-offsetY))*webcamInstance.scaleHorizontal);
            w = (int) (rt.rect.width*webcamInstance.scaleHorizontal);
            h= (int) (rt.rect.width*webcamInstance.scaleVertical);
        if(webcamInstance.checkArea( x, y, w, w ) ){
            if(canCheck && img.color!=checkedColor){
            img.color = checkedColor;
            GetComponent<Animator>().SetTrigger("Check");
            GameObject effectTemp = Instantiate(checkedEffect,transform.position+effectOffset,Quaternion.identity);
            effectTemp.transform.SetParent(transform.parent.parent);
            effectTemp.transform.SetSiblingIndex(1);
            GameObject.Destroy(effectTemp,2f);
            if(!audioSource.isPlaying){
               audioSource.PlayOneShot(pointSound); 
                }
            }else if(img.color==uncheckedColor){
                int modoPun = PlayerPrefs.HasKey("ModoPunicao")?PlayerPrefs.GetInt("ModoPunicao"):2;
                FindObjectOfType<Pattern>().errorAdd(transform.GetSiblingIndex()+1,modoPun);
            }
        }
    }
}
