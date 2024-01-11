using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using UnityEngine;

public enum Behaviour { Chase, Intercept, Patrol, ChasePatrol, Hide, LOS }

public class Enemy : MonoBehaviour
{
    [SerializeField] private float chaseSpeed, normalSpeed;
    [SerializeField] private Rigidbody prey;
    [SerializeField] private Behaviour behaviour;

    [SerializeField] private Transform[] wayPoints;

    private Rigidbody rb;

    private int currentWayPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (behaviour)
        {
            case Behaviour.Chase:
                Chase(prey.position, chaseSpeed);
                break;
            case Behaviour.Intercept:
                Intercept(rb, prey, chaseSpeed);
                break;
            case Behaviour.Patrol:
                Patrol();
                break;
            case Behaviour.ChasePatrol:
                ChasePatrol();
                break;
            case Behaviour.Hide:
                break;
            case Behaviour.LOS:
                LineOfSight();
                break;
            default: break;
        }
    }

    private void Chase(Vector3 targetPos, float speed)
    {
        var dir = targetPos - transform.position;
        rb.velocity = dir.normalized * speed;
    }

    private void Intercept(Rigidbody intercepter, Rigidbody target, float speed)
    {
        var distance = Vector3.Distance(target.position, intercepter.position);
        var timeToReach = distance / intercepter.velocity.magnitude;
        var endPoint = target.position + (target.velocity * timeToReach);

        startPointG = intercepter.position;
        endPointG = endPoint;

        Chase(endPoint, speed);
    }
    private void Patrol()
    {
        Chase(wayPoints[currentWayPoint].position, normalSpeed);
    }
    private bool LineOfSight()
    {
        Physics.Raycast(transform.position, prey.position - transform.position, out var hit);
        Player player = hit.collider.GetComponent<Player>();
        if(player != null)
        {
            Chase(prey.position, chaseSpeed);
            return true;
        }
        return false;
    }
    private void ChasePatrol()
    {
        if(!LineOfSight()) Patrol();
    }

    Vector3 startPointG, endPointG;
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPointG, endPointG);
    }

    private void OnTriggerEnter(Collider other)
    {
        WayPoint point = other.GetComponent<WayPoint>();
        if(point != null)
        {
            currentWayPoint = (currentWayPoint < wayPoints.Length - 1) ? currentWayPoint + 1 : 0;
        }
    }
}
