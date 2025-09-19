using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class KartController : MonoBehaviour
{
    //statistiche del kart
    [SerializeField] public float _speed;

    [SerializeField] public float _acceleration = 40f;
    [SerializeField] public float _maxSpeed = 50f;
    //[SerializeField] public float _deceleration;

    [SerializeField] public float _steeringSpeed = 80f;
    //[SerializeField] public float _driftMaxSpeed;
    //[SerializeField] public float _driftMaxSteeringSpeed;

    [SerializeField] private SphereCollider _kartSphere;
    [SerializeField] private Rigidbody _kartRigidbody;

    private float _inputSpeed;
    private float _horizontalInput;

    private float currentSpeed;
    float rotate, currentRotate;

    [SerializeField] public Transform _kartModel;
    public float gravity = 10f;

    private bool _drifting = false;
    private int _driftDirection;

    private void Awake()
    {
    }//Awake


    void Update()
    {
        //Il modello del kart segue il suo collider
        transform.position = _kartRigidbody.transform.position;

        //Tasto per accelerare
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _inputSpeed = 1;
        }//if
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            _inputSpeed = 0;
        }

        //Tasto per frenare e retromarcia
        if (Input.GetKeyDown(KeyCode.X))
        {
            _inputSpeed = -1;
        }//if
        else if (Input.GetKeyUp(KeyCode.X))
        {
            _inputSpeed = 0;
        }
        
        _speed = _inputSpeed * _acceleration;

        _horizontalInput = Input.GetAxis("Horizontal");

        //Tasto per sterzare
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _horizontalInput * _steeringSpeed * Time.deltaTime * _inputSpeed, 0f));

        //Tasto per derapare
        if (Input.GetKey(KeyCode.C) && !_drifting && Input.GetAxis("Horizontal") != 0 )
        {
            _drifting = true;
            _driftDirection = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            
        }//if

        if (_drifting)
        {
            //float control = (_driftDirection == 1) ? ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 0, 2) : ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 2, 0);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            _drifting = false;
        }//if

        currentSpeed = _speed;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); rotate = 0f;

    }//Update

    private void FixedUpdate()
    {
        //Kart acceleration
        _kartRigidbody.AddForce(transform.forward * currentSpeed);

        //Gravity
        _kartRigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        RaycastHit hitOn;
        RaycastHit hitNear;

        Physics.Raycast(transform.position, Vector3.down, out hitOn, 1.1f);
        Physics.Raycast(transform.position, Vector3.down, out hitNear, 2.0f);

        //Normal rotation
        //_kartModel.parent.up = Vector3.Lerp(_kartModel.parent.up, hitNear.normal, Time.deltaTime * 8.0f);
        //_kartModel.parent.Rotate(0, transform.eulerAngles.y, 0);

    }//FixedUpdate

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - (transform.up * 2));
    }//OnDrawGizmos

}//KartController