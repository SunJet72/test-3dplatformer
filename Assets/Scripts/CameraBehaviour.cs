using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform followed;

    [SerializeField] private float distanceAway = 5, distanceUp = 2, smooth = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var toPosition = followed.position
            - followed.forward * distanceAway
            + followed.up * distanceUp;
        transform.position = Vector3.Lerp(transform.position, toPosition, smooth * Time.deltaTime);
        transform.LookAt(followed);
    }
}
