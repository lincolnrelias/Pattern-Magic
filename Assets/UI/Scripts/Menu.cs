using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject optionsPanel;
    [SerializeField]
    GameObject mainPanel;
    [SerializeField]
    GameObject editPanel;
    [SerializeField]
    AudioClip btnClickSound;
    AudioSource audioSource;
    [SerializeField]
    AudioSource musicAs;
    IEnumerator Start() {
        audioSource= GetComponent<AudioSource>();
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
        checkDificulties();
        if(!PlayerPrefs.HasKey("ModoPunicao")){
            PlayerPrefs.SetInt("ModoPunicao",2);}
        if(!PlayerPrefs.HasKey("PlayerName")){
            PlayerPrefs.SetString("PlayerName","Jogador");
        }
        if(!PlayerPrefs.HasKey("CastleHealth")){
            PlayerPrefs.SetFloat("CastleHealth",40);}
        if(!PlayerPrefs.HasKey("Dificuldade")){
            PlayerPrefs.SetString("Dificuldade","fácil");}  
        if(!PlayerPrefs.HasKey("volMusica")){
            PlayerPrefs.SetFloat("volMusica",.8f);}  
        if(!PlayerPrefs.HasKey("currentSequence")){
            PlayerPrefs.SetString("currentSequence","fácil");
        }
        if(!PlayerPrefs.HasKey("Dificuldade")){
            PlayerPrefs.SetString("Dificuldade","fácil");
        }    
        if(!PlayerPrefs.HasKey("Volume")){
            PlayerPrefs.SetFloat("Volume",.7f);
        }
        if(!PlayerPrefs.HasKey("currentId")){
            PlayerPrefs.SetInt("currentId",14);
        }
        if(!PlayerPrefs.HasKey("MostrarNumeroMandalas")){
            PlayerPrefs.SetInt("MostrarNumeroMandalas",0);
        }
        
    }
    void checkDificulties(){
        string saveDirectory = Path.Combine(Application.persistentDataPath, "PadroesPatternMagic");
        if (!(Directory.Exists(saveDirectory)))
            Directory.CreateDirectory(saveDirectory);

        string setDirectory = Path.Combine(Application.persistentDataPath, "sets");
        if (!(Directory.Exists(setDirectory)))
            Directory.CreateDirectory(setDirectory);
        string readEasyFilePath =Path.Combine(setDirectory, "fácil.csv");
        string readMediumFilePath =Path.Combine(setDirectory, "médio.csv");
        if(!File.Exists(readEasyFilePath)){
            WriteEasyFiles(saveDirectory);
            string lines = "facil1\nfacil2\nfacil3\nfacil4\nfacil5\nfacil6\nfacil7";
            File.WriteAllText(readEasyFilePath,lines);
            Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
        }
        if(!File.Exists(readMediumFilePath)){
            WriteMediumFiles(saveDirectory);
            string lines = "medio1\nmedio2\nmedio3\nmedio4\nmedio5\nmedio6";
            File.WriteAllText(readMediumFilePath,lines);
            Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
        }
        
    }
    void WriteEasyFiles(string path){
        string lines="";
        lines = "4,2\n4,11\n1";
        File.WriteAllText(path+"/facil1.csv",lines);
        lines = "1,3\n8,7\n1,11\n2";
        File.WriteAllText(path+"/facil2.csv",lines);
        lines = "1,3\n7,3\n7,9\n1,9\n3";
        File.WriteAllText(path+"/facil3.csv",lines);
        lines = "2,4\n6,3\n8,6\n6,9\n2,8\n4";
        File.WriteAllText(path+"/facil4.csv",lines);
        lines = "1,3\n5,2\n8,4\n8,8\n5,10\n1,9\n5";
        File.WriteAllText(path+"/facil5.csv",lines);
        lines = "1,4\n4,2\n8,3\n9,7\n8,11\n4,12\n1,10\n6";
        File.WriteAllText(path+"/facil6.csv",lines);
        lines = "1,5\n2,2\n5,2\n8,5\n8,9\n5,12\n2,12\n1,9\n7";
        File.WriteAllText(path+"/facil7.csv",lines);
    }
    void WriteMediumFiles(string path){
        string lines="";
        lines = "7,4\n7,9\n2,4\n2,9\n8";
        File.WriteAllText(path+"/medio1.csv",lines);
        lines = "1,4\n1,10\n6,11\n8,7\n6,3\n9";
        File.WriteAllText(path+"/medio2.csv",lines);
        lines = "1,2\n4,2\n2,5\n6,5\n4,7\n8,7\n6,9\n10";
        File.WriteAllText(path+"/medio3.csv",lines);
        lines = "3,1\n6,2\n7,6\n5,9\n3,10\n0,7\n5,6\n11";
        File.WriteAllText(path+"/medio4.csv",lines);
        lines = "3,3\n8,3\n5,0\n5,12\n8,9\n3,9\n12";
        File.WriteAllText(path+"/medio5.csv",lines);
        lines = "4,2\n1,4\n7,4\n9,6\n1,9\n2,11\n7,12\n9,11\n13";
        File.WriteAllText(path+"/medio6.csv",lines);
    }
    private void Update(){
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space)){
            PlayerPrefs.DeleteAll();
        }
        #endif
        musicAs.volume = PlayerPrefs.GetFloat("volMusica");
        
    }
    public void playClickSound(){
        audioSource.clip = btnClickSound;
        audioSource.Play();
    }
   public void IniciarJogo(){
        SceneManager.LoadScene("Main",LoadSceneMode.Single);
    }
    public void FecharJogo(){
        Application.Quit();
    }
    public void AbrirEditor(){
        SceneManager.LoadScene("LevelEditor");
    }
    public void patternSet(){
        editPanel.SetActive(true);
        playClickSound();
    }
    public void OpcoesAbrir(){
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
        playClickSound();
    }
    public void OpcoesFechar(){
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        editPanel.SetActive(false);
        playClickSound();
    }
}
