using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaugeScrolling : MonoBehaviour
{
    public Transform playerTransform;
    void Update()
    {
        Vector3 transformPosition = transform.position;
        transformPosition.x = playerTransform.position.x - 0.5f;
        transformPosition.x = playerTransform.position.y + 0.5f;
        transform.position = transformPosition;
    }
}
