using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Linq;
public class PatternPicker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TMP_Dropdown availablePatterns;
    [SerializeField]
    TMP_Dropdown selectedPatterns;
    [SerializeField]
    TMP_Dropdown selectedSequence;
    [SerializeField]
    TMP_Text errorMessage;
    [SerializeField]
    GameObject addSequenceScreen;
    [SerializeField]
    TMP_InputField sequenceName;
    void Start()
    {
        string saveDirectory = Path.Combine(Application.persistentDataPath, "PadroesPatternMagic");
        DirectoryInfo dInfo = new DirectoryInfo(saveDirectory);//Assuming Test is your Folder
        FileInfo[] fileArray = dInfo.GetFiles(); //Getting Text files
        foreach (FileInfo item in fileArray)
        {
            availablePatterns.options.Add(new TMP_Dropdown.OptionData(item.Name.Replace(".csv","")));
        }
        UpdateSelectedSequence();
        StartCoroutine(UpdateSelectedPatterns());
       

    }
    public void openAddSequenceScreen(){
        addSequenceScreen.SetActive(true);
    }
    public void saveNewSequence(){
            string saveDirectory = Path.Combine(Application.persistentDataPath, "sets");
            DirectoryInfo dInfo = new DirectoryInfo(saveDirectory);//Assuming Test is your Folder
            FileInfo[] fileArray = dInfo.GetFiles(); //Ge
            bool hasRepeatedName=false;
            foreach (FileInfo item in fileArray)
        {
            if(item.Name.Replace(".csv","")==sequenceName.text){
                hasRepeatedName=true;
                return;
            }
        }
        if(hasRepeatedName){
            StartCoroutine(showErrorMessage("Nome Repetido de Sequência!",3f));
        }else{
            string saveFilePath = Path.Combine(saveDirectory,sequenceName.text+".csv");
            File.WriteAllText(saveFilePath,"");
            addSequenceScreen.SetActive(false);
            UpdateSelectedSequence();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        selectedSequence.RefreshShownValue();
        availablePatterns.RefreshShownValue();
        selectedPatterns.RefreshShownValue();
        
    }
    void UpdateSelectedSequence(){
            selectedSequence.ClearOptions();
            string saveDirectory = Path.Combine(Application.persistentDataPath, "sets");
            DirectoryInfo dInfo = new DirectoryInfo(saveDirectory);//Assuming Test is your Folder
            FileInfo[] fileArray = dInfo.GetFiles(); //Ge
            foreach (FileInfo item in fileArray)
        {
            selectedSequence.options.Add(new TMP_Dropdown.OptionData(item.Name.Replace(".csv","")));
        }
        
    }
    public void setSelectedSequenceValue(){
        PlayerPrefs.SetString("currentSequence",selectedSequence.options[selectedSequence.value].text);
    }
    IEnumerator UpdateSelectedPatterns(){
        while(true){
          string saveDirectory = Path.Combine(Application.persistentDataPath, "sets");
        if(Directory.Exists(saveDirectory)){
        string saveFilePath = Path.Combine(saveDirectory, PlayerPrefs.GetString("currentSequence")+".csv");
        string[] fileArray = File.ReadAllLines(saveFilePath);
        selectedPatterns.ClearOptions();
        foreach (string item in fileArray)
        {
            selectedPatterns.options.Add(new TMP_Dropdown.OptionData(item));
        }

        }
        yield return new WaitForSeconds(.2f);  
        }
        
    }
    IEnumerator showErrorMessage(string message,float duration){
        errorMessage.text = message;
        yield return new WaitForSeconds(duration);
        errorMessage.text="";
    }
    public void add(){
        if(selectedSequence.options[selectedSequence.value].text==""){
            StartCoroutine(showErrorMessage("NENHUMA SEQUÊNCIA SELECIONADA!",3f));
            return;
        }
        if(availablePatterns.options[availablePatterns.value].text==""){
            StartCoroutine(showErrorMessage("NENHUMA PADRÃO SELECIONADO!",3f));
            return;
        }
        string saveDirectory = Path.Combine(Application.persistentDataPath, "sets");
        if(!Directory.Exists(saveDirectory))
        {
        Directory.CreateDirectory(saveDirectory);
        }
        File.AppendAllText(Path.Combine(saveDirectory, PlayerPrefs.GetString("currentSequence")+".csv"),availablePatterns.options[availablePatterns.value].text+"\n");
    }
    public void remove(){
        if(selectedSequence.options[selectedSequence.value].text==""){
            StartCoroutine(showErrorMessage("NENHUMA SEQUÊNCIA SELECIONADA!",3f));
            return;
        }
        if(selectedPatterns.options[selectedPatterns.value].text==""){
            StartCoroutine(showErrorMessage("NENHUM PADRÃO PARA APAGAR!",3f));
            return;
        }
        string saveDirectory = Path.Combine(Application.persistentDataPath, "sets");
        if(!Directory.Exists(saveDirectory))
        {
        return;
        }
        string saveFilePath =Path.Combine(saveDirectory, PlayerPrefs.GetString("currentSequence")+".csv");
        string[] lines = File.ReadAllLines(saveFilePath);
        string[] newLines= new string[lines.Length-1];
        for(int i=0;i<newLines.Length;i++){
            newLines[i]=lines[i];
        }
        File.WriteAllLines(saveFilePath, newLines);
    }
}
