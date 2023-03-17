using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothTime = 0.3f;
    
    private Vector3 velocity;
    public PlayerManager playerManager;
    
    private void LateUpdate()
    {
        if (target!=null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, smoothTime);
        }
    }
    private void Update() {
        if (target==null)
        {
         playerManager.Death();
        }
    }
}
