using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    public List<GameObject> level;
    public List<GameObject> levelWithDroplet;
    public List<GameObject> levelWithRonce;

    public GameObject ground;

    public int mapSize;
    [Range(0,50)][SerializeField] private int difficulty;
    private int blockWidth = 5;
    private MeshRenderer levelSize;

    public GameObject start;
    public GameObject finish;
    public GameObject droplet;
    public GameObject playerGO;

    private Vector3 initialPosition = new Vector3(0, 1.5f, 0);
    private GameObject displayedLevel;
    private List<GameObject> currentList;

    
    public static float remainingTime = 15;


    // Start is called before the first frame update
    void Start()
    {
        displayedLevel = start;
        GenerateLevel();
        StartGame();
     
    }     


    private bool IsLevelValid(GameObject toDisplay, GameObject lastDisplayed){
        return (toDisplay.name[0] == lastDisplayed.name[1]);
    }

    private void GenerateLevel(){
        if(mapSize > 0){
             for(int i = 0; i < mapSize; i++){
                GameObject blockToDisplay = SelectBlock();
               if(IsLevelValid(blockToDisplay, displayedLevel)){
                    Instantiate(blockToDisplay, new Vector3((i+1)*blockWidth, 0, 0), Quaternion.Euler(-90f, 0f, 0f));
                    if(blockToDisplay.name.Contains("Platform")){ 
                        Instantiate(ground, new Vector3((i+1)*blockWidth, 0, 0), Quaternion.Euler(-90f, 0f, 0f));
                    }
                    displayedLevel = blockToDisplay;
                }else{
                    i--;
                }
            }
        }
        Instantiate(finish, new Vector3((mapSize)*blockWidth+blockWidth, 0, 0), Quaternion.Euler(-90f, 0f, 0f));
    }

    public void StartGame(){
        playerGO.transform.position = initialPosition;
        HydratationManager.currentValue = 100f;
    }

    public GameObject SelectBlock(){
        //lancer un dé
        int randNumber = Random.Range(0,100);
        if(randNumber <= difficulty && !displayedLevel.name.Contains("Ronce")){
            currentList = levelWithRonce;
        }
        if(randNumber > difficulty && randNumber <= 50 && !displayedLevel.name.Contains("Droplet")){
            currentList = levelWithDroplet;
        }
        else{
            currentList = level;
    }

    int randBlockFromList = Random.Range(0, currentList.Count);
    return currentList[randBlockFromList];
    }
    
}
