using System;
using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    void Update() {
        Vector3 transformPosition = transform.position;
        transformPosition.x = playerTransform.position.x;
        transform.position = transformPosition;
    }
    
    
}
