using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : TankScript
{
    private List<GameObject> tanks = new List<GameObject>();
    private NavMeshAgent navMeshAgent;
    private CanonScript canon;
    private RaycastHit raycastHit;
    private Vector3 targetPos;
    public GameObject closestTarget;
    private float actionChangeTimer;
    private float delayBeforeActionChange = 2;
    private float enemyProximity = 1;

    public enum State
    {
        GUN,
        MISSILE,
        NOTHING,
        MISSING
    }
    public State state = State.NOTHING;

    private GameObject GetClosestTarget()
    {
        GameObject target = tanks[0];
        if (!target)
            return null;
        float targetDistance = 0;
        float tankDistance = 0;

        foreach (GameObject tank in tanks)
        {
            targetDistance = Vector3.Distance(transform.position, target.transform.position);
            if (tank == null)
                return null;
            tankDistance = Vector3.Distance(transform.position, tank.transform.position);

            if (tankDistance < targetDistance)
                target = tank;
        }
        return target;
    }

    new void Start()
    {
        base.Start();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        canon = transform.Find("canon").GetComponent<CanonScript>();

        foreach (GameObject tank in GameObject.FindGameObjectsWithTag("Tanks"))
        {
            if (tank != gameObject)
                tanks.Add(tank);
        }
        closestTarget = GetClosestTarget();
        actionChangeTimer = delayBeforeActionChange;
        closestTarget = GetClosestTarget();
    }

    private bool IsAtOptimalHeight()
    {
        return Math.Abs(transform.position.y - closestTarget.transform.position.y) < 2;
    }

    private void FixedUpdate()
    {
        // Acquire target if there are no
        if (!closestTarget && tanks.Count > 0)
        {
            tanks.Remove(closestTarget);
            closestTarget = GetClosestTarget();
        }

        if (life <= 0)
        {
            navMeshAgent.isStopped = true;
            state = State.NOTHING;
        }

        if (closestTarget && life > 0)
        {
            // Set travel to a safe distance from target
            if (Vector3.Distance(transform.position, closestTarget.transform.position)
                > canon.fireRange / enemyProximity)
            {
                canon.StopRotary();
                navMeshAgent.SetDestination(closestTarget.transform.position);
                navMeshAgent.isStopped = false;
            }
            else
            {
                targetPos = transform.position - closestTarget.transform.position;
                targetPos.y = 0;
                if (state != State.MISSING)
                    targetPos = transform.position - closestTarget.transform.position;
                else
                    targetPos = transform.position - closestTarget.transform.position + transform.right * 3;
                transform.rotation = Quaternion.LookRotation(-targetPos);
                navMeshAgent.isStopped = true;

                // Randomly picks between these actions
                if (IsAtOptimalHeight())
                {
                    if (state == State.NOTHING)
                    {
                        if (canon.isFiring)
                            canon.StopRotary();
                    }
                    else if (state == State.MISSILE && IsAtOptimalHeight())
                    {
                        canon.FireMissile();
                        state = State.GUN;
                    }
                    // Reset safe distance
                    enemyProximity = 1;
                }
                if (!canon.isFiring && (state == State.GUN || state == State.MISSING))
                    canon.StartRotary();

                // Reduce distance between enemy if he cannot be shooted
                if (!IsAtOptimalHeight() && enemyProximity < 8)
                    enemyProximity *= 1.2f;
            }
        }
    }

    void Update()
    {
        actionChangeTimer -= Time.deltaTime;
        if (actionChangeTimer <= 0 && life > 0)
        {
            state = (State)UnityEngine.Random.Range(0, 4);
            delayBeforeActionChange = UnityEngine.Random.Range(1, 3);
            actionChangeTimer = delayBeforeActionChange;
        }
    }
}
