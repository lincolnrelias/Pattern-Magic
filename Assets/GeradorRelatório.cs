using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GeradorRelatório : MonoBehaviour
{
    string relatorioFinal;
    List<string> tables= new List<string>();
    string tablescsv="";
    public void addContentCSV(List<KeyTime> TableContents,string patternName){
		
		
		// Dependendo de como você armazenou as informações do seu relatório no projeto,
		// crie um loop e percorra a estrutura onde elas estão e vá preenchendo a tabela.
		// Segue exemplo onde o vetor bidimensional do tipo String
		// "ItensDoMeuRelatorio" [qtdItens][6] foi utilizado para armazenar os dados:
		for (int i = 0; i < TableContents.Count; i++)
		{
            int tipo = TableContents[i].tipo=="Acerto"?1:0;
            tablescsv+=("\n");
            tablescsv+=(PlayerPrefs.GetString("PlayerName")+";");
            tablescsv+=(convertName(patternName)+";");
            tablescsv+=(System.DateTime.Now.ToString("dd'/'MM'/'yyyy' 'HH':'mm':'ss")+";");
			tablescsv+=(tipo+";");
			tablescsv+=(TableContents[i].value+";");
			tablescsv+=(TableContents[i].time.ToString("F2") +";");
            tablescsv+=(Mathf.Round(TableContents[i].distance)+";");
            tablescsv+=((PlayerPrefs.HasKey("CastleHealth")?Mathf.RoundToInt(PlayerPrefs.GetFloat("CastleHealth")):100).ToString()+";");
            tablescsv+=(PlayerPrefs.HasKey("ModoPunicao")?PlayerPrefs.GetInt("ModoPunicao"):2);
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
        switch(pName){
            case "Reta 2 pontos":
                return "102";
            case "Triangulo 3 pontos":
                return "103";
            case "Cubo 4 pontos":
                return "104";
            case "pentagono 5 pontos":
                return "105";
            case "Hexagono 6 pontos":
                return "106";
            case "Heptagono 7 pontos":
                return "107";
            case "Octagono 8 pontos":
                return "108";
            case "Nonagono 9 pontos":
                return "109";
            case "Circulo 10 pontos":
                return "110";
            default:
                return"";
        }
    }
    public void saveCSV(){
    string saveDirectory = Path.Combine(Application.persistentDataPath, "DadosPatternMagic");
    string saveFilePath = Path.Combine(saveDirectory, "dados.csv");
    if(!Directory.Exists(saveDirectory))
    {
    Directory.CreateDirectory(saveDirectory);
    }
    if(!File.Exists(saveFilePath)){
         string lines=("- - - - - Relatorio de dados coletados - - - - -\n");
         lines+="T/P -> Tipo de padrão, se refere a qual padrão estava sendo jogado no momento de registro da informação, seu significado segue a seguinte tabela:\n"+
"102;Reta 2 Pontos\n"+
"103;Triângulo 3 pontos\n"+
"104;Cubo 4 pontos\n"+
"105;Pentágono 5 pontos\n"+
"106;Hexágono 6 pontos\n"+
"107;Heptágono 7 pontos\n"+
"108;Octágono 8 pontos\n"+
"109;Nonágono 9 pontos\n"+
"110;Círculo 10 pontos\n"+
"* Atualizarei essa tabela ao adicionar padrões novos*;\n"+
"EvA ->  Erro(0) ou acerto(1);\n"+
"N/C ->  nodo que foi tocado(em relação a ordem em que devem ser tocados);\n"+
"T/AvE ->  Tempo decorrido em relação ao último registro do mesmo tipo (tempo decorrido desde o último acerto se esse for o registro de um acerto, e vice-versa);\n"+
"D/I ->  Distância entre o inimigo e o castelo;\n"+
"HP/C ->  Vida máxima do castelo;\n"+
"PUN ->  Tipo de punição(1=1 nodo, 2=todos os nodos);\n";
        lines+=("Paciente;");
        lines+=("T/P;");
        lines+=("Data/Hora;");
		lines+=("E∨A;");
		lines+=("N/C;");
        lines+=("T/A∨E;");
        lines+=("D/I;");
        lines+=("HP/C;");
        lines+=("PUN;");
        File.WriteAllText(saveFilePath, lines);
    }
    print(saveFilePath);
    File.AppendAllText(saveFilePath, relatorioFinal);
    Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});");
    string result = File.ReadAllText(saveFilePath);
    WebGLFileSaver.SaveFile(result, "Relatorio"); 
    }
}
