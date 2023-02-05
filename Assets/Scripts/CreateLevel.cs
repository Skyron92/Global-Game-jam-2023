using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    public List<GameObject> level;
    public List<GameObject> levelWithDroplet;
    public GameObject ground;
    public int mapSize;
    private int blockWidth = 5;
    private MeshRenderer levelSize;
    public GameObject start;
    public GameObject finish;
    public GameObject droplet;
    public GameObject playerGB;

    private Vector3 initialPosition = new Vector3(0, 1.5f, 0);

    
    public static float remainingTime = 15;


    // Start is called before the first frame update
    void Start()
    {
        generateLevel();
        startGame();
     
    }     


    private bool IsLevelValid(GameObject toDisplay, GameObject lastDisplayed){

        return (toDisplay.name[0] == lastDisplayed.name[1]);
        
    }

    private void generateLevel(){
         GameObject displayedLevel = start;
        List<GameObject> isDropletOnBlock = level;

        if(mapSize > 0){
        for(int i = 0; i < mapSize; i++){
            int rand = Random.Range(0, level.Count);
            int isDroplet = Random.Range(1,100);
            if(isDroplet < 20){
               isDropletOnBlock = levelWithDroplet;
               rand = Random.Range(0,isDropletOnBlock.Count);
            }
            if(IsLevelValid(isDropletOnBlock[rand], displayedLevel)){
                Instantiate(isDropletOnBlock[rand], new Vector3((i+1)*blockWidth, 0, 0), Quaternion.Euler(90f, 0f, 0f));
                if(isDropletOnBlock[rand].name.Contains("Platform")){ 
                    Instantiate(ground, new Vector3((i+1)*blockWidth, 0, 0), Quaternion.Euler(90f, 0f, 0f));
                }
            }
            else{
                    i--;
                }
            displayedLevel = isDropletOnBlock[rand];
        }
        }

        Instantiate(finish, new Vector3((mapSize)*blockWidth+blockWidth, 0, 0), Quaternion.Euler(90f, 0f, 0f));

    }

    public void startGame(){
        playerGB.transform.position = initialPosition;
        HydratationManager.currentValue = 100;
    }
}
