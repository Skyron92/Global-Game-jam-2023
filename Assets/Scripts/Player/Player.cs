using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController characterController;
   
    void Awake(){
        characterController = GetComponent<CharacterController>;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
