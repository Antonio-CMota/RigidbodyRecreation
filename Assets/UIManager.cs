using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private BallMovement _ballMovement;
    [SerializeField] private Explosive _explosive;
    [SerializeField] private TextMeshProUGUI _velText;
    [SerializeField] private TextMeshProUGUI _accelText;
    [SerializeField] private TMP_InputField _forceX;
    [SerializeField] private TMP_InputField _forceY;
    [SerializeField] private TMP_InputField _forceZ;
    [SerializeField] private TMP_InputField _impulseTime;
    [SerializeField] private TMP_InputField _explosionForce;
    [SerializeField] private TMP_InputField _explosionRadius;

    private void Update()
    {
        UpdateText();
    }

    public void ChangeGravity(int index)
    {
        switch (index)
        {
            case 0: _ballMovement.Gravity = -9.8f;
                break;
            case 1: _ballMovement.Gravity = -3.7f;
                break;
            case 2:
                _ballMovement.Gravity = -24.8f;
                break;
            case 3:
                _ballMovement.Gravity = -10.4f;
                break;
            case 4:
                _ballMovement.Gravity = -1.62f;
                break;
        }
    }

    public void ChangeForceMode(int index)
    {
        switch (index)
        {
            case 0: _ballMovement._forceMode = BallMovement.ForceMode.Instant;
                break;
            case 1: _ballMovement._forceMode = BallMovement.ForceMode.Impulse;
                break;
        }
    }

    private void UpdateText()
    {
        _velText.text = "Current Velocity: " + _ballMovement._velocity.ToString();
        _accelText.text = "Current Acceleration: " + _ballMovement.acceleration.ToString();
    }

    public void SetForce()
    {
        _ballMovement._appliedForce = new Vector3(float.Parse(_forceX.text), float.Parse(_forceY.text), float.Parse(_forceZ.text));
        _ballMovement._forceDuration = float.Parse(_impulseTime.text);
    }

    public void SetExplosion()
    {
        _explosive.explosionForce = float.Parse(_explosionForce.text);
        _explosive.explosionForce = float.Parse(_explosionRadius.text);
    }

    public void UseWind(bool toggle)
    {
        if (toggle == true)
            _ballMovement.useWind = true;
        if (toggle == false)
            _ballMovement.useWind = false;
    }
}
