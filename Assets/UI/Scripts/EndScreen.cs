using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class EndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Webcam webcam;
    [SerializeField]
    TMP_Text patternText;
    [SerializeField]
    AudioClip startSound;
    [SerializeField]
    bool gameOver;
    void Start()
    {
        Time.timeScale=0;
        webcam.stop();
        if(gameOver){
        Vector2 patternCounts = FindObjectOfType<Pattern>().GetPatternCounts();
        patternText.SetText("Você completou "+patternCounts[0]+" de "+patternCounts[1]+" feitiços antes de seu castelo ser destruído!");
        }
        
        GetComponent<AudioSource>().PlayOneShot(startSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RestartGame(){
        SceneManager.UnloadSceneAsync("Main");
        Time.timeScale=1;
        SceneManager.LoadScene("Main");
        
    }
    public void Menu(){
        
        webcam.stop();
        SceneManager.UnloadSceneAsync("Main");
        Time.timeScale=1;
        SceneManager.LoadScene("Menu");
    }
}
