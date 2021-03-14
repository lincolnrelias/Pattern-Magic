using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CornerSnapper : MonoBehaviour
{
    // Start is called before the first frame update
    Transform[] points;
    [SerializeField]
    GameObject buttonPrefab;
    GameObject currentButton;
    [SerializeField]
    GameObject pointParent;
    Transform lastClosestNode;
    GameObject[] setButtons= new GameObject[150];
    List<int[]> listOfCoords = new List<int[]>();
    float lastSmallestDistance=1000f;
    int numberOfPoints=0;

    void Start(){
        currentButton = Instantiate(buttonPrefab,Input.mousePosition,Quaternion.identity);
        int count = pointParent.transform.childCount;
        points = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            points[i]=pointParent.transform.GetChild(i);
        }
        indexObjects();
    }
    void indexObjects(){
        int k=0;
        for(int i=0;i<14;i++){
            for(int j=0;j<10;j++){
                pointParent.transform.GetChild(k).GetComponent<SnappPoint>().setCoordinates(j,i);
                k++;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
    StartCoroutine(updatePos());

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
    currentButton.transform.SetParent(transform.parent);
    
    if(Input.GetMouseButtonDown(0) && !snappPoint.hasButtonAttached){
        snappPoint.hasButtonAttached=true;
        setButtons[numberOfPoints]=currentButton;
        listOfCoords.Add(new int[2]);
        listOfCoords.LastOrDefault()[0]=snappPoint.getRow();
        listOfCoords.LastOrDefault()[1]=snappPoint.getCollumn();
        currentButton = Instantiate(buttonPrefab,mousePos,Quaternion.identity);
        numberOfPoints++;
    }
    if(Input.GetKeyDown(KeyCode.Space)){spawnPattern();};
    }
    void spawnPattern()
    {
        FindObjectOfType<PatternSpawner>().spawnPattern(listOfCoords);
   }
}
