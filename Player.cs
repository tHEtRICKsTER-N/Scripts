using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("--CONTROLS--")]
    [SerializeField] private KeyCode _crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _runKey = KeyCode.LeftShift;

    [Header("--MOVEMENT AND AIMING--")]
    [SerializeField] private float _walkSpeed = 2.5f;
    [SerializeField] private float _runSpeed = 5;
    [SerializeField] private float _crouchSpeed = 1;
    [SerializeField] private Transform _camRoot;
    [SerializeField] private Transform _crouchPosition;
    [SerializeField] private Transform _standPosition;
    [SerializeField] private float _sensitivity = 0.3f;
    [SerializeField] private float _jumpAmount = 3.5f;
    [SerializeField] private float _crouchSmooth = 3;
    

    [Header("Private Vars")]
    CharacterController _charCon;
    float verticalRotation;
    Vector3 movement;
    float currSpeed;
    bool isCrouching;

    //this will be used to know if player died or not
    public bool gameOver;

    void Start()
    {
        _charCon = GetComponent<CharacterController>();
        currSpeed = _walkSpeed;

        isCrouching = false;
        gameOver = false;
    }

    void Update()
    {
        Movement();
        Looking();
    }

    void Movement()
    {
        float yVel = movement.y;
        movement = (Input.GetAxisRaw("Vertical") * transform.forward) + (Input.GetAxisRaw("Horizontal") * transform.right).normalized;
        movement.y = yVel;

        if (_charCon.isGrounded)
        {
            movement.y = 0f;
        }


        //jumping
        if (Input.GetKeyDown(_jumpKey))
        {
            movement.y = _jumpAmount;
        }

        movement.y += Physics.gravity.y * Time.deltaTime;

        //crouching
        if (Input.GetKey(_crouchKey) && _charCon.isGrounded){
            isCrouching = true;
        }
        else if(Input.GetKeyUp(_crouchKey))
        {
            isCrouching = false;
        }

        if (isCrouching)
        {
            _camRoot.transform.position = Vector3.Lerp(_camRoot.transform.position,_crouchPosition.position,Time.deltaTime * _crouchSmooth);
            currSpeed = _crouchSpeed;
        }
        else
        {
            _camRoot.transform.position = Vector3.Lerp(_camRoot.transform.position,_standPosition.position,Time.deltaTime * _crouchSmooth);
            currSpeed = _walkSpeed;
        }


        //running
        if(Input.GetKey(_runKey)){
            currSpeed = _runSpeed;
        }else if(Input.GetKeyUp(_runKey)){
            currSpeed = _walkSpeed;
        }
       
        _charCon.Move(movement * Time.deltaTime * currSpeed);

    }

    void Looking()
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X"),transform.rotation.eulerAngles.z));
        verticalRotation += Input.GetAxis("Mouse Y");
        verticalRotation = Mathf.Clamp(verticalRotation, -60, 60);
        _camRoot.rotation = Quaternion.Euler(-verticalRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z); 
    }

    void Jump()
    {
        if (_charCon.isGrounded)
        {
            movement.y = _jumpAmount;
        }
    }
}
