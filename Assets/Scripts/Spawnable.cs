using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{

    [SerializeField] private Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
    }
    private bool isSpawned = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isSpawned)
        {
            isSpawned = true;
            Spawn();
        }
    }

    public void Spawn()
    {
        transform.position = spawnPoint.position;
    }
}
