using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("--CONTROLS--")]
    [SerializeField] private KeyCode _crouchKey;
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private KeyCode _runKey;

    [Header("--MOVEMENT AND AIMING--")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private Transform _camRoot;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _jumpAmount;
    

    [Header("Private Vars")]
    CharacterController _charCon;
    float verticalRotation;
    Vector3 movement;
    float currSpeed;

    //this will be used to know if player died or not
    public bool gameOver = false;

    void Start()
    {
        _charCon = GetComponent<CharacterController>();
        currSpeed = _walkSpeed;
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
            transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            currSpeed = _crouchSpeed;

        }else if(Input.GetKeyUp(_crouchKey)){
            transform.localScale = new Vector3(1,1,1);
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
