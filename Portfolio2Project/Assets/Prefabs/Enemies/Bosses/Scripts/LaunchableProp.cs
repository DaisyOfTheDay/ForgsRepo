using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchableProp : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] float projectileSpeed;

    [Header("----- Set By Boss (Ignore) -----")]
    public Transform targetToMoveTo;

    private void Start()
    {
        rigidBody.velocity = (targetToMoveTo.position - this.gameObject.transform.position).normalized * projectileSpeed;
    }
}
