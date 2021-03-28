using System.Collections;
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
        patternTable.Add(convertedName+":"+patternName+"\n");
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
            tablescsv+=((PlayerPrefs.HasKey("CastleHealth")?Mathf.RoundToInt(PlayerPrefs.GetFloat("CastleHealth")):100).ToString()+";");
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
    if(File.Exists(PatternTablePath)){
      string[] tableData = File.ReadAllLines(PatternTablePath);
    string newTableData="";
    foreach (string sNew in patternTable.ToArray())
    {
        bool isDiff=true;
        foreach (string sOld in tableData)
        {
            print(sNew.Length+":"+sOld.Length);

            if(sNew.Substring(0,sNew.Length-1)==sOld){
                print("sda");
               isDiff=false;
               break;
            }
        }
        if(isDiff){
            newTableData+=sNew.Substring(0,sNew.Length-1);
        }
    }  
    File.AppendAllText(PatternTablePath,newTableData);
    }else{
        File.AppendAllText(PatternTablePath,string.Join("\n",patternTable.ToArray()));
    }
    
    string caption=("- - - - - Relatorio de dados coletados - - - - -\n");
         caption+="T/P -> Tipo de padrão, se refere a qual padrão estava sendo jogado no momento de registro da informação, seu significado segue a seguinte tabela:\n"+
    File.ReadAllText(PatternTablePath) +
    "Erro/Acerto ->  Erro(0) ou acerto(1);\n";
    File.WriteAllText(endFilePath,collumns+File.ReadAllText(RawDataPath)+"\n"+caption);
    if(Application.platform == RuntimePlatform.WebGLPlayer){
       Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
       string result = File.ReadAllText(endFilePath);
        WebGLFileSaver.SaveFile(result, "Relatorio.csv");   
    }

    
    
    }
}
