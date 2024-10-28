using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Object Properties")]
public class ObjectProperties : MonoBehaviour
{
    [Header("Object Settings")]
    [SerializeField] private float _density;
    [SerializeField] private float _radius;
    private float _area;
    [SerializeField] private float _mass;
    [SerializeField] private float _dragCoefficient;
}
