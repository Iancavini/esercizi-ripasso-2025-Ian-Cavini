using UnityEngine;

public class KartController : MonoBehaviour
{
    private SphereCollider _kartSphere;
    private Rigidbody _kartRigidbody;
    [SerializeField] private MeshRenderer _playerMesh;
    private float _horizontalInput;
    private float _verticalInput;
    //statistiche del kart
    [SerializeField] public float _acceleration;
    [SerializeField] public float _deceleration;
    [SerializeField] public float _steeringSpeed = 80f;
    [SerializeField] public float _maxSpeed = 30f;
    [SerializeField] public float _driftMaxSpeed;
    [SerializeField] public float _driftMaxSteeringSpeed;


    private void Awake()
    {
        _kartSphere = GetComponent<SphereCollider>();
        _kartRigidbody = GetComponent<Rigidbody>();
    }//Awake


    void Update()
    {
        //leggo sempre l'input dell'asse orizzontale
        _horizontalInput = Input.GetAxis("Horizontal");

        //leggo sempre l'input dell'asse verticale
        _verticalInput = Input.GetAxis("Vertical");
    }//Update

    private void MoveHorizontal()
    {
        _kartRigidbody.MovePosition(transform.position + Vector3.right * (_horizontalInput * _maxSpeed * Time.fixedDeltaTime));
    }//MoveHorizontal

}//KartController
