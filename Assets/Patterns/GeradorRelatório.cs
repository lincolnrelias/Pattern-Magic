﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class GeradorRelatório : MonoBehaviour
{
    string relatorioFinal;
    List<string> tables= new List<string>();
    string tablescsv="";
    [SerializeField]
    CastleHealth ch;
    List<string> patternTable=new List<string>();
    public void addContentCSV(List<KeyTime> TableContents,string patternName){
		
		
		// Dependendo de como você armazenou as informações do seu relatório no projeto,
		// crie um loop e percorra a estrutura onde elas estão e vá preenchendo a tabela.
		// Segue exemplo onde o vetor bidimensional do tipo String
		// "ItensDoMeuRelatorio" [qtdItens][6] foi utilizado para armazenar os dados:
        string convertedName = convertName(patternName);
        if(!patternTable.Contains(convertedName+":"+patternName)){
            patternTable.Add(convertedName+":"+patternName);}
		for (int i = 0; i < TableContents.Count; i++)
		{
            int tipo = TableContents[i].tipo=="Acerto"?1:0;
            tablescsv+=("\n");
            tablescsv+=(PlayerPrefs.GetString("PlayerName")+";");
            tablescsv+=(convertedName+";");
            tablescsv+=(System.DateTime.Now.ToString("dd'/'MM'/'yyyy' 'HH':'mm':'ss")+";");
			tablescsv+=(tipo+";");
			tablescsv+=(TableContents[i].value+";");
			tablescsv+=(TableContents[i].time.ToString("F2") +";");
            tablescsv+=(Mathf.Round(TableContents[i].distance)+";");
            tablescsv+=(Mathf.RoundToInt(PlayerPrefs.GetInt("CastleHealth")).ToString()+";");
            tablescsv+=((ch.getCurrHealth()));
		}
		
		// Feche sua tabela e finalize o relatório
		
    }
    public void GerarRelatorioFinalCSV()
    {
		
        string dataAtualFormatoExcel = System.DateTime.Now.ToString("dd'/'MM'/'yyyy' 'HH':'mm':'ss");
        
		
		
        string lines = "";
		// lines.Add ("<td>.... o que mais for relevante no seu relatório ......</td>
		// .....	
        lines+=tablescsv;
		relatorioFinal+=lines;
	}
    string convertName(string pName){
        string saveDirectory = Path.Combine(Application.persistentDataPath, "PadroesPatternMagic");
        string readFilePath =Path.Combine(saveDirectory, pName+".csv");
        string[] lines = File.ReadAllLines(readFilePath);
        return lines[lines.Length-1];
    }
    public void saveCSV(){
    string saveDirectory = Path.Combine(Application.persistentDataPath, "DadosPatternMagic");
    string patternDirectory = Path.Combine(Application.persistentDataPath, "PadroesPatternMagic");
    string endFilePath = Path.Combine(saveDirectory, "dadosCompilados.csv");
    string PatternTablePath =  Path.Combine(saveDirectory, "tableData.csv");
    string RawDataPath =  Path.Combine(saveDirectory, "rawData.csv");
    if(!Directory.Exists(saveDirectory))
    {
    Directory.CreateDirectory(saveDirectory);
    }
         
        string collumns=("Paciente;");
        collumns+=("ID do padrao;");
        collumns+=("Data/Hora;");
		collumns+=("Erro/Acerto;");
		collumns+=("Mandala atingida;");
        collumns+=("Tempo desde o ultimo erro/acerto;");
        collumns+=("Distância entre inimigo-castelo;");
        collumns+=("Vida inicial do castelo;");
        collumns+=("Vida atual do castelo;");
    File.AppendAllText(RawDataPath, relatorioFinal);
    string newTableData="";
    DirectoryInfo dInfo = new DirectoryInfo(patternDirectory);//Assuming Test is your Folder
        FileInfo[] fileArray = dInfo.GetFiles(); //Ge
        foreach (FileInfo item in fileArray)
        {
            string[] file = File.ReadAllLines(Path.Combine(patternDirectory,item.Name));
            newTableData+=file[file.Length-1]+":"+item.Name.Replace(".csv","")+"\n";
        }
    File.WriteAllText(PatternTablePath,newTableData);
    if(Application.platform == RuntimePlatform.WebGLPlayer){
        Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
    }
    string caption=("- - - - - Relatorio de dados coletados - - - - -\n");
         caption+="T/P -> Tipo de padrão, se refere a qual padrão estava sendo jogado no momento de registro da informação, seu significado segue a seguinte tabela:\n"+
    File.ReadAllText(PatternTablePath) +
    "Erro/Acerto ->  Erro(0) ou acerto(1);\n";
    File.WriteAllText(endFilePath,collumns+File.ReadAllText(RawDataPath)+"\n"+caption);
    
    if(Application.platform == RuntimePlatform.WebGLPlayer){
        Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
    }
    
    
    }
    public static void downloadReport(){
    if(Application.platform == RuntimePlatform.WebGLPlayer){
        string saveDirectory = Path.Combine(Application.persistentDataPath, "DadosPatternMagic");
        string endFilePath = Path.Combine(saveDirectory, "dadosCompilados.csv");  
        string result = File.ReadAllText(endFilePath);
        WebGLFileSaver.SaveFile(result, "Relatorio.csv");   
    }
    }
}
