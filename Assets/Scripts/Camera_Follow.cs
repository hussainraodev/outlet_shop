using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{


    public Transform Target;
    public float smoothTime = 0.3f;
    public Vector3 OffSet;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            Vector3 target_pos = Target.position + OffSet;
            transform.position = Vector3.SmoothDamp(transform.position,target_pos, ref velocity, smoothTime);
        }
    }
}
