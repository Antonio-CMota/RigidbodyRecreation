using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] public float explosionForce = 10.0f;
    [SerializeField] public float explosionRadius = 5.0f;
    [SerializeField] private LayerMask affectedObjectsLayer;

    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, affectedObjectsLayer);

        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.name);
                BallMovement ballMovement = collider.GetComponent<BallMovement>();

                if (ballMovement != null)
                {
                    ballMovement.ApplyExplosionForce(transform.position, explosionForce, explosionRadius);
                }
        }
    }
}
