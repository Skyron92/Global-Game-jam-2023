using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    
    //CharacterController
    private CharacterController characterController;
    private bool _isGrounded => characterController.isGrounded;
    private Animator _animator;
    public static bool playable = true;
    public bool gameFinished = false;
    private Vector3 _currentPosition;
    
    
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
    private float recoilTimer;
    private bool _isHurt;

    //Thirst Settings 
    [Tooltip("Vitesse à laquelle la barre de soif diminue.")] [Range(0, 0.100f)] [SerializeField] private float DecreaseSpeed;
    [Tooltip("Quantité d'eau redonnée par chaque goutte.")][Range(0, 100)] [SerializeField] private float WaterValue;

    //Menu Settings
    [Header("Menu")] [SerializeField] private GameObject menu;
    
    //Sound settings
    [SerializeField] private AudioSource walkSFX;
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource rootSFX;

    void Awake()
    {
        playable = true;
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _direction = new Vector3(_input.x, 0, 0);
        _startSpeed = speed;
        canJump = true;

    }
    
    void Update()
    {
        //_currentPosition = transform.position;
        FixPlayerPosition(transform.position);
       Gravity();
       MoveCharacter();
       RotatePlayer();
       TranslateToTheBottom();
       if(debuffJump > 0 && Input.GetKeyDown(KeyCode.Space)){
            debuffJump --;
            if(debuffJump == 0){
                canJump = true;
            }
        }
        _animator.SetFloat("Horizontal", _direction.x);
        if(_velocity > 0) _animator.SetFloat("Vertical", _direction.y);
        else _animator.SetFloat("Vertical", 0);
        if (speed < 0) speed = 0;
        if (speed > _startSpeed) speed = _startSpeed;
        if(HydratationManager.currentValue <= 0) {GameOver();}
    }

    public void Move(InputAction.CallbackContext context) {
        if(!playable) return;
        if (context.canceled) {
            walkSFX.Stop();
            _direction = Vector3.zero;
        }
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
        if(!walkSFX.isPlaying) walkSFX.Play();
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
        jumpSFX.Play();
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
            HydratationManager.currentValue -= Damages;
        }

        if (other.CompareTag("WaterDrop")) {
            HydratationManager.currentValue += WaterValue;
            ParticleSystem vfxDroplet = other.GetComponentInChildren<ParticleSystem>();
            vfxDroplet.Play();
            other.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;

        }

        if(other.CompareTag("Finish")){
            _animator.SetFloat("Horizontal", 0);
            _animator.SetBool("GameEnded", true);
            walkSFX.Stop();
            rootSFX.Play();
            TranslateToTheBottom();
            GameFinished();
            StartCoroutine(beforeEnd());
            Debug.Log("Finit !");
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

    private void GameOver() {
        menu.SetActive(true);
    }
    private void GameFinished(){
        //Ecran de victoire
        Debug.Log("Felicitation, vous avez gagné");
        gameFinished = true;
        menu.SetActive(true);
    }

    private void FixPlayerPosition(Vector3 playerPosition)
    {
        if (playerPosition.z == 0.0000) return;
        /*playerPosition = new Vector3(playerPosition.x, playerPosition.y, 0.000f);
        transform.position = playerPosition;*/
        playerPosition.z = 0.00f;
    }

    private void TranslateToTheBottom() {
        if(!gameFinished) return;
        playable = false;
        heigh = 0;
        jumpPower = 0;
        speed = 0;
        float timer = Time.deltaTime;
        Vector3 position = transform.position;
        float Y = Mathf.Lerp(position.y, 0.9f, timer/2);
        position.y = Y;
        transform.position = position;
        var rotation = transform.rotation;
        var target = new Quaternion(0, 180, 0,0);
        var rotate = Quaternion.Slerp(rotation, target, timer);
        transform.rotation = rotate;
    }
    IEnumerator beforeEnd(){
        yield return new WaitForSeconds(3);
    }
    /*private void Recoil() {
        Debug.Log("Aie");
        distance.y += recoilPower;
        characterController.Move(-distance * Time.deltaTime);
        if (_isGrounded) _isHurt = false;
        if (recoilTimer >= 2) _isHurt = false;
    }*/
}