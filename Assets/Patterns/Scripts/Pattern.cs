using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class Pattern : MonoBehaviour
{   
    AudioClip completionSound,pointSound,errorSound;
    Color32 checkedColor,uncheckedColor;
    int currentNode,exampleIndex;
    float interval;
    AudioSource audioSource;
    bool done=false,playing=true,reseting=true;
    string lastPatternName;
    float lastHitTime,lastErrorTime;
    [SerializeField]
    GameObject[] patternPrefabs;
    [SerializeField]
    float minErrorInterval=2f;
    [SerializeField]
    float intervalBetweenPatterns= 1.5f;
    List<KeyTime> parAcertos=new List<KeyTime>();
    // Start is called before the first frame update
    List<KeyTime> parErros=new List<KeyTime>();
    [SerializeField]
    EnemySpawner enemySpawner;
    int currPatternIndex=0;
    void Start()
    {
        lastHitTime = Time.time;
        audioSource = GetComponent<AudioSource>();
        PlayerPrefs.SetInt("Sessao",PlayerPrefs.GetInt("Sessao")+1);
        StartCoroutine(spawnPatternAfterDelay(intervalBetweenPatterns));
    }


    // Update is called once per frame
    void Update()
    {
        if(playing || done || reseting){
            lastHitTime = Time.time;
            return;
        }
        if(currentNode>=transform.childCount){if(currentNode==transform.childCount){PatternCompletion();}return;}
        Transform child = transform.GetChild(currentNode);
        Color currChildColor=child.GetComponent<Image>().color; 
        Button currBtn = child.GetComponent<Button>();
        currBtn.canCheck=true;
        if(currChildColor == checkedColor){
            currentNode++;
            parAcertos.Add(new KeyTime(currentNode-1,Time.time-lastHitTime));
            lastHitTime = Time.time;
        }
        
    }
    public void errorAdd(int btnIndex){
        if(Time.time-lastErrorTime<minErrorInterval || playing || currentNode<2){return;};
        parErros.Add(new KeyTime(btnIndex,Time.time-lastErrorTime));
        lastErrorTime = Time.time;
        Button btnNext = transform.GetChild(currentNode).GetComponent<Button>();
        currentNode=Mathf.Clamp(currentNode-1,1,transform.childCount);
        Button btnCurr = transform.GetChild(currentNode).GetComponent<Button>();
        btnNext.canCheck=false;
        btnCurr.canCheck=true;
        btnCurr.GetComponent<Animator>().SetTrigger("Error");
        btnCurr.switchColor();
        if(!audioSource.isPlaying){
            audioSource.PlayOneShot(errorSound);
        }
        
    }
    void PatternCompletion(){
        if(done || reseting){return;};
        done = true;
        lastPatternName = transform.GetChild(0).name.Replace("(Clone)","");
        Enemy enemy = FindObjectOfType<Enemy>();
        audioSource.PlayOneShot(completionSound);
        if(enemy)enemy.Die();
        reseting=true;
        int childs = transform.childCount;
        for (int i = childs - 1; i >= 0; i--){GameObject.Destroy(transform.GetChild(i).gameObject,1.5f);}
        if(currPatternIndex<patternPrefabs.Length){
           StartCoroutine(spawnPatternAfterDelay(1.75f)); 
        }
        
        WriteCSV();
        
    }
    IEnumerator spawnPatternAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        spawnPattern();
        StopCoroutine(spawnPatternAfterDelay(delay));
    }
    void spawnPattern()
    {
        Transform child = Instantiate(patternPrefabs[currPatternIndex], transform).transform;
        setParentTransform(child);
        currPatternIndex++;
    }

     void setParentTransform(Transform child)
    {
        int childs = child.childCount;
        for (int i = 0; i < childs; i++)
        {
            child.GetChild(0).SetParent(transform);
        };
    }

    public void newPattern(Color32 checkedC,Color32 uncheckedC,AudioClip pointS,AudioClip errorS,AudioClip completionS,bool play,float _interval){
        checkedColor = checkedC;
        uncheckedColor = uncheckedC;
        pointSound=pointS;
        errorSound=errorS;
        completionSound=completionS;
        interval=_interval;
        done=false;
        exampleIndex = 1;
        currentNode = 1;
        lastErrorTime = Time.time;
        enemySpawner.SpawnEnemy();
        setParentTransform(transform.GetChild(0));
        if(play){
            StartCoroutine(iterateTroughPoints());
        }else{playing=false;};
        reseting=false;
    }
    void WriteCSV(){
        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        Directory.CreateDirectory(path+"/Dados");
        int nSessao =PlayerPrefs.GetInt("Sessao");

        Directory.CreateDirectory(path+"/Dados/Sessão "+nSessao+"/Acertos");
        Directory.CreateDirectory(path+"/Dados/Sessão "+nSessao+"/Erros");
        PlayerPrefs.SetInt("AcertoN",PlayerPrefs.GetInt("AcertoN")+1);
        PlayerPrefs.SetInt("ErroN",PlayerPrefs.GetInt("ErroN")+1);
        StreamWriter swA = new StreamWriter(path+"/Dados/Sessão "+nSessao+"/Acertos/Acertos "+lastPatternName+" "+PlayerPrefs.GetInt("AcertoN")+".csv");
        swA.Write(ToCSV(parAcertos));
        swA.Close();
        if(parErros.Count>0){
        PlayerPrefs.SetInt("ErroN",PlayerPrefs.GetInt("ErroN")+1);
        StreamWriter swE = new StreamWriter(path+"/Dados/Sessão "+nSessao+"/Erros/Erros"+lastPatternName+" "+PlayerPrefs.GetInt("ErroN")+".csv");
        swE.Write(ToCSV(parErros));
        swE.Close();
        }
    }
    public string ToCSV(List<KeyTime> pairList)
{
    string str="";
    foreach(var par in pairList)
    {
        str+= par.value.ToString()+","+par.time.ToString("F2").Replace(",",".")+";\n";
    }

    return str;
}
    IEnumerator iterateTroughPoints(){
        playing=true;
        patternExample pattern = transform.GetChild(0).GetComponent<patternExample>();
        while(exampleIndex<transform.childCount){
            if(exampleIndex>1){
                transform.GetChild(exampleIndex-1).GetComponent<Image>().color=pattern.getColors()[1];
            }
            Transform child = transform.GetChild(exampleIndex);
            child.GetComponent<Image>().color=pattern.getColors()[0];
            exampleIndex++;
            yield return new WaitForSeconds(interval);
            audioSource.PlayOneShot(pointSound);
        }
        transform.GetChild(exampleIndex-1).GetComponent<Image>().color=pattern.getColors()[1];
        playing=false;
        StopCoroutine(iterateTroughPoints());
    }
}
