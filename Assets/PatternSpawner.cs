using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    GameObject pointParent;
    [SerializeField]
    GameObject patternExamplePrefab;
    GameObject patternExampleInst;
    bool hasSpawned=false;
    void Start()
    {
        patternExampleInst = Instantiate(patternExamplePrefab,transform); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void spawnPattern(List<int[]> listOfCoords)
    {
        if(hasSpawned)return;
        for (int i = 0; i < listOfCoords.Count; i++)
       {
            foreach (Transform point in pointParent.transform)
            {
                int row= point.GetComponent<SnappPoint>().getRow();   
                int collumn= point.GetComponent<SnappPoint>().getCollumn();  
                if(row==listOfCoords[i][0] && collumn==listOfCoords[i][1]){
                    GameObject currBtn=Instantiate(buttonPrefab,point.transform.position,Quaternion.identity);
                    currBtn.transform.SetParent(patternExampleInst.transform);
                }
            }
        }
        hasSpawned=true;
   }
}
