using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaugeScrolling : MonoBehaviour
{
    [Range(-10, 10)] [SerializeField] private float YDistance;
    [Range(-10, 10)] [SerializeField] private float XDistance;
    public Transform playerTransform;
    void Update()
    {
        Vector3 transformPosition = transform.position;
        transformPosition.x = playerTransform.position.x + XDistance;
        transformPosition.y = playerTransform.position.y + YDistance;
        transform.position = transformPosition;
    }
}
