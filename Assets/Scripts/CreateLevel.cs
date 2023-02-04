using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    public List<GameObject> level;
    public int mapSize;
    private int blockWidth = 5;
    private MeshRenderer levelSize;
    public GameObject start;
    public GameObject finish;
    
    public static float remainingTime = 15;


    // Start is called before the first frame update
    void Start()
    {
        GameObject displayedLevel = start;

        if(mapSize > 0){

        for(int i = 0; i < mapSize; i++){

            int rand = Random.Range(0, level.Count);

                if(IsLevelValid(level[rand], displayedLevel)){
                    Instantiate(level[rand], new Vector3((i+1)*blockWidth, 0, 0), Quaternion.Euler(-90f, 0f, 0f));
                }else{
                    i--;
                }

            displayedLevel = level[rand];
            
        }

        }
        
        Instantiate(finish, new Vector3((mapSize)*blockWidth+blockWidth, 0, 0), Quaternion.Euler(-90f, 0f, 0f));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool IsLevelValid(GameObject toDisplay, GameObject lastDisplayed){

        return (toDisplay.name[0] == lastDisplayed.name[1]);
        
    }
}
