using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    
    //CharacterController
    private CharacterController characterController;
    private bool _isGrounded => characterController.isGrounded;
    private Animator _animator;
    public static bool playable = true;
    public bool gameFinished = false;
    
    
    //Move Settings
    private Vector3 _input;
    private Vector3 _direction;
    [Header("Movements Settings")][Range(0,10)][SerializeField] private float speed;
    private float _startSpeed;
    
   
    //Jump Settings
    [Range(0, 10)] [SerializeField] private float jumpPower;
    private bool canJump;
    [SerializeField] private ParticleSystem jumpParticle;
    //Set to rand between 7 and 15 when enter in grass
    private int debuffJump = 0;
    
    //Gravity Settings
    private float _gravity = -9.81f;
    [Range(0,10)][SerializeField] private float heigh;
    private float _velocity;
    
    [Header("Effects Settings")]
    
    //Damage Settings
    [Range(0, 10)] [SerializeField] private float recoilPower;
    [Range(0, 100)] [SerializeField] private float Damages;
    [Range(0, 10)] [SerializeField] private float invulnerabilityDuration;
    private float invulnerabilityTimer;
    private float recoilTimer;
    private bool _isHurt;
    private Vector3 distance = new Vector3();
    
    //Thirst Settings 
    private float thirst = 1;
    [Tooltip("Vitesse à laquelle la barre de soif diminue.")] [Range(0, 0.100f)] [SerializeField] private float DecreaseSpeed;
    [Tooltip("Quantité d'eau redonnée par chaque goutte.")][Range(0, 100)] [SerializeField] private float WaterValue;

    //Menu Settings
    [Header("Menu")] [SerializeField] private GameObject menu;

    //Particule System
    public ParticleSystem waterSpread;

    void Awake()
    {
        playable = true;
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _direction = new Vector3(_input.x, 0, 0);
        _startSpeed = speed;
        canJump = true;
    }

    public static bool _isActive;
    public bool IsActive
    {
        set => menu.SetActive(_isActive);
    }
    

    void Update(){
        Gravity();
        MoveCharacter();
        RotatePlayer();
        if(debuffJump > 0 && Input.GetKeyDown(KeyCode.Space)){
            debuffJump --;
            if(debuffJump == 0){
                canJump = true;
            }
        }
        /*if(!playable) menu.SetActive(true);
        else menu.SetActive(false);*/
        _animator.SetFloat("Horizontal", _direction.x);
        if(_velocity > 0) _animator.SetFloat("Vertical", _direction.y);
        else _animator.SetFloat("Vertical", 0);
        if (speed < 0) speed = 0;
        if (speed > _startSpeed) speed = _startSpeed;
        Debug.Log(playable);
        if(HydratationManager.currentValue <= 90) {GameOver();}
    }

    public void Move(InputAction.CallbackContext context) {
        if(!playable) return;
        if(_isHurt) return;
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0, 0);
    }

    private void MoveCharacter() {
        if(!playable) return;
        characterController.Move(_direction * (speed * Time.deltaTime));
    }
    
    private void RotatePlayer() {
        if(!playable) return;
        if(_input == Vector3.zero) return;
        float rotationAngle = Vector3.Angle(_direction, Vector3.forward);
        if (_direction.x < 0) rotationAngle = -rotationAngle;
        Quaternion rotation = Quaternion.Euler(0,rotationAngle,0);
        if (90 + rotationAngle < 180 && 90 + rotationAngle > 0) rotation.y = 90;
        if (-90 - rotationAngle > 0 && -90 - rotationAngle < -180) rotation.y = -90;
        transform.rotation = rotation;
    }


    private void Gravity() {
        if(!playable) return;
        if (_isGrounded && _velocity < 0) _velocity = -1.0f;
        _velocity += _gravity * heigh * Time.deltaTime;
        _direction.y = _velocity;
    }

    public void Jump(InputAction.CallbackContext context) {
        if(!playable) return;
        if(!context.started) return;
        if(!_isGrounded) return;
        if(!canJump) return;
        _velocity += jumpPower;
        jumpParticle.Play();
    }

    private void OnTriggerEnter(Collider other) {
        if(!playable) return;
        if (other.CompareTag("Grass")) {
            speed = Mathf.Lerp(speed, 1, 5f);
            if(_isGrounded){
                canJump = !canJump;
                debuffJump = Random.Range(7,15);
            }
            jumpParticle.enableEmission = false;
        }

        if (other.CompareTag("Enemy")) {
            _isHurt = true;
            HydratationManager.currentValue -= Damages;
            distance = transform.position - other.transform.position;
            Invulnerability();
        }

        if (other.CompareTag("WaterDrop")) {
            HydratationManager.currentValue += WaterValue;
            waterSpread.Play();
            Destroy(other.gameObject);
        }

        if(other.CompareTag("Finish")){
            GameFinished();
        }
         if(other.CompareTag("Fall")){
            GameOver();
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if(!playable) return;
        if(collision.gameObject.CompareTag("WaterDrop")){
            HydratationManager.currentValue += WaterValue;
            Destroy(collision.gameObject);
        }
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if(!playable) return;
        if (hit.collider.CompareTag("BrokenGround")) {
            BrokenGround context = hit.collider.GetComponent<BrokenGround>();
            context.wobble = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(!playable) return;
        if (other.CompareTag("Grass")) {
            speed = Mathf.Lerp(speed, _startSpeed, 1f);
            canJump = !canJump;
            jumpParticle.enableEmission = true;
        }
    }

    private void Invulnerability() {
        invulnerabilityTimer = 0;
    }

    private void GameOver() {
        menu.SetActive(false);
    }
    private void GameFinished(){
        //Ecran de victoire
        Debug.Log("Felicitation, vous avez gagné");
        gameFinished = true;
    }

    /*private void Recoil() {
        Debug.Log("Aie");
        distance.y += recoilPower;
        characterController.Move(-distance * Time.deltaTime);
        if (_isGrounded) _isHurt = false;
        if (recoilTimer >= 2) _isHurt = false;
    }*/
}