using System;
using UnityEngine;
using UnityEngine.UIElements;

public class KartController : MonoBehaviour
{
    //statistiche del kart
    [SerializeField] public float _acceleration = 30f;
    //[SerializeField] public float _deceleration;
    [SerializeField] public float _steeringSpeed = 40f;
    [SerializeField] public float _speed;
    //[SerializeField] public float _driftMaxSpeed;
    //[SerializeField] public float _driftMaxSteeringSpeed;

    [SerializeField] private SphereCollider _kartSphere;
    [SerializeField] private Rigidbody _kartRigidbody;
    private float _horizontalInput;

    private float currentSpeed;
    float rotate, currentRotate;

    [SerializeField] public Transform _kartModel;
    public float gravity = 10f;

    private int _driftDirection;
    private bool _drifting;

    private void Awake()
    {
    }//Awake


    void Update()
    {
        transform.position = _kartRigidbody.transform.position - new Vector3(0, .2f, 0);

        //acceleratore
        if (Input.GetKey(KeyCode.Z))
        {
            _speed = _acceleration;
        }
        else
        {
            _speed = 0;
        }

        //sterzo
        if (Input.GetAxis("Horizontal") != 0 && _speed != 0)
        {
            int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs((Input.GetAxis("Horizontal")));
            Steer(dir, amount);
        }

        //drifting
        if (Input.GetKeyDown(KeyCode.C) && !_drifting && Input.GetAxis("Horizontal") != 0)
        {
            _drifting = true;
            _driftDirection = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
        }

        if (_drifting)
        {
            float control = (_driftDirection == 1) ? ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 0, 2) : ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 2, 0);
            Steer(_driftDirection, 1 * control);
        }

        if (Input.GetKeyUp(KeyCode.C) && _drifting)
        {
            _drifting = false;
        }

        currentSpeed = Mathf.SmoothStep(currentSpeed, _speed, Time.deltaTime * 12f); _speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); rotate = 0f;


    }//Update

    private void FixedUpdate()
    {
        //Kart acceleration
        _kartRigidbody.AddForce(_kartModel.transform.forward * currentSpeed, ForceMode.Acceleration);

        //Gravity
        _kartRigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);

        RaycastHit hitOn;
        RaycastHit hitNear;

        Physics.Raycast(transform.position, Vector3.down, out hitOn, 1.1f);
        Physics.Raycast(transform.position, Vector3.down, out hitNear, 2.0f);

        //Normal rotation
        //_kartModel.parent.up = Vector3.Lerp(_kartModel.parent.up, hitNear.normal, Time.deltaTime * 8f);
        //_kartModel.parent.Rotate(0, transform.eulerAngles.y, 0);


    }//FixedUpdate

    public void Steer(int direction, float amount)
    {
        rotate = (_steeringSpeed * direction) * amount;
    }//Steer

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - (transform.up * 2));
    }


}//KartController

public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}