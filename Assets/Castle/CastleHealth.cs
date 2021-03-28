using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    [SerializeField]float MaxHealth = 100f;
    [SerializeField]Image healthBarImage;
    [SerializeField]float smoothness=5f;
    [SerializeField]Pattern pattern;
    [SerializeField]GameObject gameOverScreen;
    [SerializeField]AudioSource musicAs;
    float currentHealth;
    // Start is called before the first frame update
    void Start()
    {   
        MaxHealth =PlayerPrefs.HasKey("CastleHealth")?PlayerPrefs.GetFloat("CastleHealth"):300f;
        PlayerPrefs.SetFloat("CastleHealth",MaxHealth);
        currentHealth = MaxHealth;
        healthBarImage.fillAmount = 1;
        musicAs.volume = PlayerPrefs.GetFloat("volMusica");
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            TakeDamage(5f);
        }
        updateHealthBar();
    }
    public void TakeDamage(float damage){
        currentHealth-=damage;
        if(currentHealth<=0){
            GameOverSequence();
        }
    }
    public float getCurrHealth(){
        return currentHealth;
    }
    void updateHealthBar(){
        healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount,
        Mathf.Clamp(currentHealth/MaxHealth,0,1),
        smoothness*Time.deltaTime);
    }

    void GameOverSequence(){
        musicAs.Stop();
        gameOverScreen.SetActive(true);
        if(PlayerPrefs.GetString("GerarRelatório")=="sim"){
        FindObjectOfType<Pattern>().GerarRelatório();
        }
    }
}
