
using System;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    public ParticleSystem drop;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) drop.Play();
    }
}