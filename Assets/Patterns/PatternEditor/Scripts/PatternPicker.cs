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
    void Start()
    {
        string saveDirectory = Path.Combine(Application.persistentDataPath, "PadroesPatternMagic");
        DirectoryInfo dInfo = new DirectoryInfo(saveDirectory);//Assuming Test is your Folder
        FileInfo[] fileArray = dInfo.GetFiles(); //Getting Text files
        foreach (FileInfo item in fileArray)
        {
            availablePatterns.options.Add(new TMP_Dropdown.OptionData(item.Name.Replace(".csv","")));
        }
        StartCoroutine(UpdateSelectedPatterns());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        availablePatterns.RefreshShownValue();
        selectedPatterns.RefreshShownValue();
    }
    IEnumerator UpdateSelectedPatterns(){
        while(true){
          string saveDirectory = Path.Combine(Application.persistentDataPath, "sets");
        if(Directory.Exists(saveDirectory)){
        string saveFilePath = Path.Combine(saveDirectory, "teste.csv");
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
    public void add(){
        string saveDirectory = Path.Combine(Application.persistentDataPath, "sets");
        if(!Directory.Exists(saveDirectory))
        {
        Directory.CreateDirectory(saveDirectory);
        }
        File.AppendAllText(Path.Combine(saveDirectory, "teste.csv"),availablePatterns.options[availablePatterns.value].text+"\n");
    }
    public void remove(){
        string saveDirectory = Path.Combine(Application.persistentDataPath, "sets");
        if(!Directory.Exists(saveDirectory))
        {
        return;
        }
        string saveFilePath =Path.Combine(saveDirectory, "teste.csv");
        string[] lines = File.ReadAllLines(saveFilePath);
        string[] newLines= new string[lines.Length-1];
        for(int i=0;i<newLines.Length;i++){
            newLines[i]=lines[i];
        }
        File.WriteAllLines(saveFilePath, newLines);
    }
}
