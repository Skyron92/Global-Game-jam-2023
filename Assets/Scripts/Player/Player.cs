using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Random = System.Random;

public class Player : MonoBehaviour
{
    //CharacterController
    private CharacterController characterController;
    private bool _isGrounded => characterController.isGrounded;
    
    
    //Move Settings
    private Vector3 _input;
    private Vector3 _direction;
    [Header("Movements Settings")][Range(0,10)][SerializeField] private float speed;
    private float _startSpeed;
   
    //Jump Settings
    [Range(0, 10)] [SerializeField] private float jumpPower;
    private bool canJump;
    
    //Gravity Settings
    private float _gravity = -9.81f;
    [Range(0,10)][SerializeField] private float heigh;
    private float _velocity;
    
    [Header("Effects Settings")]
    
    //Damage Settings
    [Range(0, 10)] [SerializeField] private float recoilPower;
    [Range(0, 1)] [SerializeField] private float Damages;
    [Range(0, 10)] [SerializeField] private float invulnerabilityDuration;
    private float invulnerabilityTimer;
    private bool _isHurt;
    
    //Thirst Settings 
    private float thirst = 1;
    [Tooltip("Vitesse à laquelle la barre de soif diminue.")] [Range(0, 0.1f)] [SerializeField] private float DecreaseSpeed;
    [Tooltip("Quantité d'eau redonnée par chaque goutte.")][Range(0, 1)] [SerializeField] private float WaterValue;
    [SerializeField] private Slider slider;
    
    //Menu Settings
    [Header("Menu")] [SerializeField] private GameObject menu;

    void Awake() {
        characterController = GetComponent<CharacterController>();
        _direction = new Vector3(_input.x, 0, 0);
        _startSpeed = speed;
        canJump = true;
        menu.SetActive(false);
    }

    void Update() {
        Gravity();
        MoveCharacter();
        if (speed < 0) speed = 0;
        if (speed > _startSpeed) speed = _startSpeed;
        if (_isHurt) {
           // Recoil();
            invulnerabilityTimer += Time.deltaTime;
            if (invulnerabilityTimer >= invulnerabilityDuration) {
                _isHurt = false;
            }
        }
        if (thirst > 1) thirst = 1;
        if (thirst < 0) thirst = 0;
        thirst -= Time.deltaTime * DecreaseSpeed;
        slider.value = thirst;
        if(thirst <= 0) GameOver();
    }

    public void Move(InputAction.CallbackContext context) {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0, 0);
    }

    private void MoveCharacter() {
        characterController.Move(_direction * (speed * Time.deltaTime));
    }

    private void Gravity() {
        if (_isGrounded && _velocity < 0) _velocity = -1.0f;
        _velocity += _gravity * heigh * Time.deltaTime;
        _direction.y = _velocity;
    }

    public void Jump(InputAction.CallbackContext context) {
        if(!context.started) return;
        if(!_isGrounded) return;
        if(!canJump) return;
        _velocity += jumpPower;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Grass")) {
            speed = Mathf.Lerp(speed, 1, 5f);
            canJump = !canJump;
        }

        if (other.CompareTag("Enemy")) {
            thirst -= Damages;
            Invulnerability();
        }

        if (other.CompareTag("WaterDrop")) {
            thirst += WaterValue;
            Destroy(other.gameObject);
        }
        
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.collider.CompareTag("BrokenGround")) {
            BrokenGround context = hit.collider.GetComponent<BrokenGround>();
            context.wobble = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Grass")) {
            speed = Mathf.Lerp(speed, _startSpeed, 1f);
            canJump = !canJump;
        }
    }

    private void Invulnerability() {
        _isHurt = true;
        invulnerabilityTimer = 0;
    }

    private void GameOver() {
        menu.SetActive(true);
    }
}