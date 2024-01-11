using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 targetPoint;
    [SerializeField] private float speed;

    private Vector3 startPoint, endPoint;
    private bool toEnd = true;
    private float time = 0;
    void Start()
    {
        startPoint = transform.position;
        endPoint = startPoint + targetPoint;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (toEnd)
        {
            transform.position = Vector3.Lerp(startPoint, endPoint, time * speed);
            if (transform.position == endPoint)
            {
                toEnd = false;
                time = 0;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(endPoint, startPoint, time * speed);
            if (transform.position == startPoint)
            {
                toEnd = true;
                time = 0;
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Application.isPlaying)
        {
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}
