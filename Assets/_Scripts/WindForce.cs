using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForce : MonoBehaviour
{
    #region Fields

    [Header("Force Settings")]
    [SerializeField] private float _minForce = -1f;
    [SerializeField] private float _maxForce = 1f;

    private Vector3 _windForce;

    #endregion

    #region LifeCycle

    private void Awake()
    {
        RandomizeWindForce();
    }

    #endregion

    #region Private Methods

    private void RandomizeWindForce()
    {
        _windForce = new Vector3(
            UnityEngine.Random.Range(_minForce, _maxForce),
            UnityEngine.Random.Range(_minForce, _maxForce),
            UnityEngine.Random.Range(_minForce, _maxForce)
        );
    }

    #endregion

    #region Public Methods

    public Vector3 GetWindForce()
    {
        return _windForce;
    }

    #endregion
}