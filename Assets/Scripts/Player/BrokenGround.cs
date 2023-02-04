using System;
using UnityEngine;

public class BrokenGround : MonoBehaviour
{
    [SerializeField] private float speed;
    public bool canFall;
    public bool wobble;
    [SerializeField] private Animator _animator;
    private Rigidbody rb;
    private float fallingValue;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (wobble) {
            Wobble();
            fallingValue += Time.deltaTime * speed;
        }
        if (fallingValue >= 1f) canFall = true;
        if(canFall) Fall();
    }

    public void Fall() {
        _animator.SetBool("playerIsAbove", false);
        rb.useGravity = true;
    }

    void Wobble() {
        _animator.SetBool("playerIsAbove", true);
    }
}