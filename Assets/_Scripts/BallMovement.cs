using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BallMovement : MonoBehaviour
{
    #region Fields

    [field: Header("Physics Settings")] public float Gravity { get; set; } = -9.8f;
    private static readonly float GravitationalConstant = 6.674f * Mathf.Pow(10, -5);
    private readonly float _groundCheckDistance = 0.5f;
    private Vector3 _previousPosition;
    private float _interpolationFactor;
    public Vector3 acceleration;
   

    [Header("Drag Settings")]
    [SerializeField] private float _density = 1.2f;
    [SerializeField] private float _radius = 0.5f;
    private float _area;
    [SerializeField] private float _mass = 1.0f;
    [SerializeField] private float _dragCoefficient = 0.1f;

    [Header("Force Settings")]
    public Vector3 _velocity = Vector3.zero;
    public Vector3 _appliedForce = Vector3.zero;
    public ForceMode _forceMode;
    public float _forceDuration;

    [Header("Wind Settings")]
    [SerializeField] private WindForce _windForceComponent;

    [FormerlySerializedAs("useWInd")] public bool useWind;

    #endregion

    #region Enums

    public enum ForceMode
    {
        Instant,
        Impulse
    }

    #endregion

    #region LifeCycle

    private void Awake()
    {
        CalculateArea();
        _previousPosition = transform.position;
    }

    private void Update()
    {
        _interpolationFactor = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        transform.position = Vector3.Lerp(_previousPosition, transform.position, _interpolationFactor);
    }
    private void FixedUpdate()
    {
        UpdateVelocity();
        UpdatePosition();
        _previousPosition = transform.position;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        _velocity = Vector3.zero;
    }

    #endregion

    #region Public Methods
    
    public void ApplyExplosionForce(Vector3 explosionCenter, float explosionForce, float explosionRadius)
    {
        Debug.Log("Explosion Called");
        float distance = Vector3.Distance(transform.position, explosionCenter);

        if (distance <= explosionRadius)
        {
            Vector3 direction = (transform.position - explosionCenter).normalized;
            float forceMagnitude = explosionForce * (1 - (distance / explosionRadius));
            _appliedForce = direction * forceMagnitude;
            _velocity += _appliedForce;
            _appliedForce = Vector3.zero;
        }
    }
    
    #endregion
    
    #region Private Methods

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, out _, _groundCheckDistance);
    }

    
    private void CalculateArea()
    {
        _area = Mathf.PI * _radius * _radius;
    }

    private void UpdateVelocity()
    {
        if (!IsGrounded())
        {
            Vector3 dragForce = CalculateDragForce();
            Vector3 gravitationalForce = CalculatePlanetGravitationalForce();
            Vector3 gravitationalForceBetweenObjects = CalculateGravitationalForceBetweenObjects();
            Vector3 netForce = CalculateNetForce(dragForce, gravitationalForce, _windForceComponent.GetWindForce(), gravitationalForceBetweenObjects);

            acceleration = CalculateAcceleration(netForce);

            _velocity += acceleration * Time.fixedDeltaTime;
        }
        else
        {
            _velocity.y = 0;
        }
    }

    public void ApplyForce()
    {
        if (_forceMode == ForceMode.Instant)
        {
            _velocity += _appliedForce;
            _appliedForce = Vector3.zero;
        }
        else if (_forceMode == ForceMode.Impulse)
        {
            if (_forceDuration > 0)
            {
                _velocity += (_appliedForce / _mass) * Time.deltaTime;
                _forceDuration -= Time.deltaTime;
            }
            else
            {
                _appliedForce = Vector3.zero;
            }
        }
    }

    private Vector3 CalculateDragForce()
    {
        float velocityMagnitude = _velocity.magnitude;
        float dragForceMagnitude = 0.5f * _density * velocityMagnitude * velocityMagnitude * _area * _dragCoefficient;
        Vector3 dragForceDirection = -_velocity.normalized;
        return dragForceDirection * dragForceMagnitude;
    }

    private Vector3 CalculateGravitationalForceBetweenObjects()
    {
        Vector3 totalGravitationalForce = Vector3.zero;
        GameObject[] gravitationalObjects = GameObject.FindGameObjectsWithTag("Gravitational");

        foreach (GameObject gravitationalObject in gravitationalObjects)
        {
            if (gravitationalObject != this.gameObject)
            {
                Vector3 direction = (gravitationalObject.transform.position - transform.position).normalized;
                float distance = (gravitationalObject.transform.position - transform.position).magnitude;
                float forceMagnitude = (GravitationalConstant * _mass * gravitationalObject.GetComponent<BallMovement>()._mass) / (distance * distance);
                totalGravitationalForce += direction * forceMagnitude;
            }
        }

        return totalGravitationalForce;
    }
    
    private Vector3 CalculatePlanetGravitationalForce()
    {
        return Vector3.up * (_mass * Gravity);
    }

    private Vector3 CalculateNetForce(Vector3 dragForce, Vector3 gravitationalForce, Vector3 windForce, Vector3 gravitationalForceBetweenObjects)
    {
        if (useWind)
        {
            return gravitationalForce + dragForce + windForce + gravitationalForceBetweenObjects;
        }
        else
        {
            return gravitationalForce + dragForce + gravitationalForceBetweenObjects;
        }
    }

    private Vector3 CalculateAcceleration(Vector3 netForce)
    {
        return netForce / _mass;
    }

    private void UpdatePosition()
    {
        transform.position += _velocity * Time.fixedDeltaTime;
       // Debug.Log(_velocity);
    }
    #endregion
}