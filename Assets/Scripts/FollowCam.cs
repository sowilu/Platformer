using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    [Range(0.1f, 0.9f)]
    public float smoothing = 0.9f;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var targetPos = target.position + offset;
        transform.position = target.position + offset;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        print(other.relativeVelocity);
    }
}
