using System;
using UnityEngine;
using UnityEngine.UIElements;

public class KartController : MonoBehaviour
{
    //statistiche del kart
    [SerializeField] public float _acceleration = 30f;
    //[SerializeField] public float _deceleration;
    [SerializeField] public float _steeringSpeed = 80f;
    [SerializeField] public float _speed;
    //[SerializeField] public float _driftMaxSpeed;
    //[SerializeField] public float _driftMaxSteeringSpeed;

    [SerializeField] private SphereCollider _kartSphere;
    [SerializeField] private Rigidbody _kartRigidbody;
    private float _horizontalInput;

    private float currentSpeed;
    float rotate, currentRotate;

    [SerializeField] public Transform _kartTransform;
    public float gravity = 10f;


    private void Awake()
    {
    }//Awake


    void Update()
    {
        transform.position = _kartRigidbody.transform.position - new Vector3(0, .2f, 0);


        if (Input.GetKey(KeyCode.Z))
        {
            _speed = _acceleration;
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs((Input.GetAxis("Horizontal")));
            Steer(dir, amount);
        }

        currentSpeed = Mathf.SmoothStep(currentSpeed, _speed, Time.deltaTime * 12f); _speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); rotate = 0f;


    }//Update

    private void FixedUpdate()
    {
        //Kart acceleration
        _kartRigidbody.AddForce(_kartTransform.transform.forward * currentSpeed, ForceMode.Acceleration);

        //Gravity
        _kartRigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);
        
    }//FixedUpdate

    public void Steer(int direction, float amount)
    {
        rotate = (_steeringSpeed * direction) * amount;
    }//Steer


}//KartController