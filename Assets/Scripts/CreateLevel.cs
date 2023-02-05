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

    
    public static float remainingTime = 15;


    // Start is called before the first frame update
    void Start()
    {
        GameObject displayedLevel = start;
        List<GameObject> isDropletOnBlock = level;

        if(mapSize > 0){
        //Je veux creer une terrain avec un size
        for(int i = 0; i < mapSize; i++){
            //Je prendre un element au pif dans ma liste
            int rand = Random.Range(0, level.Count);
            //Je jette un "dé" pour savoir si le terrain aura une goutte ou non
            int isDroplet = Random.Range(1,100);
            //Si le terrain à une goutte, je change de liste, et d'element prit au pif.
            if(isDroplet < 20){
               isDropletOnBlock = levelWithDroplet;
               rand = Random.Range(0,isDropletOnBlock.Count);
            }
            //Si l'élement prit au pif est valide
            if(IsLevelValid(isDropletOnBlock[rand], displayedLevel)){
                //J'instancie le block
                Instantiate(isDropletOnBlock[rand], new Vector3((i+1)*blockWidth, 0, 0), Quaternion.Euler(-90f, 0f, 0f));
                //Si le block instancier est une plateform, je lui ajoute le sol
                if(isDropletOnBlock[rand].name.Contains("Platform")){ 
                    Instantiate(ground, new Vector3((i+1)*blockWidth, 0, 0), Quaternion.Euler(-90f, 0f, 0f));
                }
            }
            //Si il n'est pas valide, je refais un tour
            else{
                    i--;
                }
            //Je garde en mémoire le dernier block placé pour verifier que le prochain à placer est valide. 
            Debug.Log(isDropletOnBlock);
            Debug.Log(rand);
            displayedLevel = isDropletOnBlock[rand];
        }
        }

        Instantiate(finish, new Vector3((mapSize)*blockWidth+blockWidth, 0, 0), Quaternion.Euler(-90f, 0f, 0f));

     
    }     


    private bool IsLevelValid(GameObject toDisplay, GameObject lastDisplayed){

        return (toDisplay.name[0] == lastDisplayed.name[1]);
        
    }
}
