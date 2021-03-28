using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
public class CornerSnapper : MonoBehaviour
{
    // Start is called before the first frame update
    Transform[] points;
    [SerializeField]
    GameObject buttonPrefab;
    GameObject currentButton;
    [SerializeField]
    GameObject pointParent;
    [SerializeField]
    TMP_InputField nameField;
    [SerializeField]
    Transform btnParent;
    [SerializeField]
    GameObject saveScreen;
    [SerializeField]
    GameObject previewScreen;
    [SerializeField]
    GameObject lineDrawer;
    [SerializeField]
    Sprite lineImage;
    [SerializeField]
    float lineThickness = 10f;
    [SerializeField]
    TMP_InputField idField;
    Transform lastClosestNode;
    List<int[]> listOfCoords = new List<int[]>();
    float lastSmallestDistance=1000f;
    List<Transform> btnList = new List<Transform>();
    List<SnappPoint> pointList = new List<SnappPoint>();
    bool canCreateButtons=true;

    void Start(){
        currentButton = Instantiate(buttonPrefab,Input.mousePosition,Quaternion.identity);
        int count = pointParent.transform.childCount;
        points = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            points[i]=pointParent.transform.GetChild(i);
        }
        PointMapper.indexObjects(pointParent.transform);
    }
    
    // Update is called once per frame
    void Update()
    {
    if(saveScreen.activeSelf)return;
    StartCoroutine(updatePos());
    DrawLines();
    UpdateButtonLabels();
    }
    void UpdateButtonLabels(){
        for(int i=0;i<btnList.Count;i++){
            btnList[i].GetChild(0).GetComponent<TMP_Text>().text=i.ToString();
        }
    }
    void DrawLines(){
        LineDrawer.clearLines(lineDrawer);
        if(btnList.Count<2){return;}
        
        for (int i=1; i<btnList.Count;i++)
        {
            LineDrawer.drawLine(btnList[i].gameObject,btnList[i-1].gameObject,lineImage,lineDrawer,lineThickness);
        }
        LineDrawer.drawLine(btnList[btnList.Count-1].gameObject,btnList[0].gameObject,lineImage,lineDrawer,lineThickness);
    }
    IEnumerator updatePos(){
        yield return new WaitForSeconds(.1f);
        Vector3 mousePos = Input.mousePosition;
    for(int i=0;i<points.Length;i++){
        float currDistance=Vector3.Distance(mousePos,points[i].position);
        if(currDistance<lastSmallestDistance){
            lastClosestNode=points[i];
            lastSmallestDistance=currDistance;
        }
        
    }
    
    lastSmallestDistance=1000f;
    currentButton.transform.parent = lastClosestNode;
    currentButton.GetComponent<RectTransform>().anchoredPosition= Vector3.zero;
    SnappPoint snappPoint = lastClosestNode.GetComponent<SnappPoint>();
    currentButton.transform.SetParent(btnParent);
    
    if(canCreateButtons && Input.GetMouseButtonDown(0) && !snappPoint.hasButtonAttached && inBounds()){
        snappPoint.hasButtonAttached=true;
        listOfCoords.Add(new int[2]);
        listOfCoords.LastOrDefault()[0]=snappPoint.getRow();
        listOfCoords.LastOrDefault()[1]=snappPoint.getCollumn();
        btnList.Add(currentButton.transform);
        pointList.Add(snappPoint);
        currentButton = Instantiate(buttonPrefab,mousePos,Quaternion.identity);
    }
    }
    bool inBounds(){
        return Input.mousePosition.x>180 && Input.mousePosition.x<1000 && Input.mousePosition.y>20 && Input.mousePosition.y<520;
    }
    public void OpenSaveWindow(){
        saveScreen.SetActive(true);
    }
    public void CloseSaveWindow(){
        saveScreen.SetActive(false);
    }
    public void showPreview(){
        previewScreen.SetActive(true);
        previewScreen.GetComponent<PreviewGenerator>().showPreviewElements();
        canCreateButtons=false;
    }
    public void closePreview(){
        previewScreen.GetComponent<PreviewGenerator>().clearPreview();
        previewScreen.SetActive(false);
        StartCoroutine(enableButtonCreationAfterDelay(.5f));
    }
    IEnumerator enableButtonCreationAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        canCreateButtons=true;
    }
    public void savePattern()
    {
        if(listOfCoords.Count<=1){return;}
    string saveDirectory = Path.Combine(Application.persistentDataPath, "PadroesPatternMagic");
    string saveFilePath = Path.Combine(saveDirectory, nameField.text+".csv");
    if(!Directory.Exists(saveDirectory))
    {
    Directory.CreateDirectory(saveDirectory);
    }
    string lines="";
    for(int i=0;i<listOfCoords.Count;i++){
        lines+= listOfCoords[i][0].ToString()+","+listOfCoords[i][1].ToString()+"\n";
    }
    lines+= idField.text;
    File.WriteAllText(saveFilePath,lines);
    CloseSaveWindow();
    clearAll();
   }
   void clearAll(){
       btnList= new List<Transform>();
       listOfCoords= new List<int[]>();
       for(int i=0;i<btnParent.childCount-1;i++){
           GameObject.Destroy(btnParent.GetChild(i).gameObject);
           pointList[i].hasButtonAttached=false;
       }
       pointList = new List<SnappPoint>();
   }
   public void clear(){
      if(btnParent.childCount>1){

          if(btnList.Count>0)btnList.RemoveAt(btnList.Count-1);
          if(listOfCoords.Count>0)listOfCoords.RemoveAt(listOfCoords.Count-1);
          pointList[pointList.Count-1].hasButtonAttached=false;
          if(pointList.Count>0)pointList.RemoveAt(pointList.Count-1);
          GameObject.Destroy(btnParent.GetChild(btnParent.childCount-2).gameObject);
      }
          

   }
   public void voltar(){
       SceneManager.LoadScene("Menu");
   }
}
