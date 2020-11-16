using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    [SerializeField]float MaxHealth = 100f;
    [SerializeField]Image healthBarImage;
    [SerializeField]float smoothness=5f;
    float currentHealth;
    // Start is called before the first frame update
    void Start()
    {   
        currentHealth = MaxHealth;
        healthBarImage.fillAmount = 1;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            TakeDamage(10f);
        }
        updateHealthBar();
    }
    public void TakeDamage(float damage){
        currentHealth-=damage;
        if(currentHealth<=0){
            GameOverSequence();
        }
    }

    void updateHealthBar(){
        healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount,
        Mathf.Clamp(currentHealth/MaxHealth,0,1),
        smoothness*Time.deltaTime);
    }

    void GameOverSequence(){
        print("Game Over");
    }
}
