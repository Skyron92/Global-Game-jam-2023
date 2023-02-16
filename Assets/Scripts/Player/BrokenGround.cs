using System;
using UnityEngine;

public class BrokenGround : MonoBehaviour
{
    private bool canFall;

    [Tooltip("Une faible valeur ralentit la chute de la plateforme")] [Range(0.1f, 1.5f)] [SerializeField]
    private float speedDecrease;
    [HideInInspector] public bool wobble;
    [SerializeField] private Transform mesh;
    public AnimationCurve wobbleCurve;
    private Rigidbody rb;
    private float fallingValue;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (wobble) {
            Wobble();
            fallingValue += Time.deltaTime * speedDecrease;
        }
        if (fallingValue >= 1f) canFall = true;
        if (canFall) rb.useGravity = true;
    }

    void Wobble() {
        if(canFall) return;
        Vector3 position = mesh.position;
        position.z = wobbleCurve.Evaluate(Time.deltaTime);
        mesh.position = position;
    }
}