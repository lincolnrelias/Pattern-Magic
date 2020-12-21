using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    int score = 0;
    [SerializeField]
    TMP_Text scoreText;
    // Start is called before the first frame update

    // Update is called once per frame
    void LateUpdate()
    {
        updateScore();
    }
    public void addScore(int amount){
        score+=amount;
    }
    public void removeScore(int amount){
        score=Mathf.Max(score,0,score-amount);
    }
    void updateScore(){
        scoreText.SetText("Pontos: "+score);
    }

}
