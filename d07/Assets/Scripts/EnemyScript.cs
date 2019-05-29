﻿using System.Collections;
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
    public float enemyProximity = 2;

    public enum State
    {
        GUN,
        MISSILE,
        NOTHING
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
    }

    private bool IsAtOptimalHeight()
    {
        return Math.Abs(transform.position.y - closestTarget.transform.position.y) < 2;
    }

    private void FixedUpdate()
    {
        // Acquire target if there are no
        if (!closestTarget && tanks.Count > 0)
            closestTarget = GetClosestTarget();
        
        // Set travel to a safe distance from target
        if (Vector3.Distance(transform.position, closestTarget.transform.position) 
            > canon.fireRange / enemyProximity)
        {
            canon.StopRotary();
            navMeshAgent.SetDestination(closestTarget.transform.position);
            navMeshAgent.isStopped = false;
        }
        else if (closestTarget)
        {
            targetPos = transform.position - closestTarget.transform.position;
            targetPos.y = 0;
            transform.rotation = Quaternion.LookRotation(-targetPos);
            navMeshAgent.isStopped = true;

            // Randomly picks between these actions
            if (state == State.NOTHING && IsAtOptimalHeight())
            {
                if (canon.isFiring)
                    canon.StopRotary();
            }
            else if (state == State.MISSILE && IsAtOptimalHeight())
            {
                canon.FireMissile();
                state = State.GUN;
            }
            else if (!canon.isFiring && state == State.GUN)
                canon.StartRotary();

            if (!IsAtOptimalHeight())
                enemyProximity *= 1.5f;
        }
    }

    void Update()
    {
        actionChangeTimer -= Time.deltaTime;
        if (actionChangeTimer <= 0)
        {
            state = (State)UnityEngine.Random.Range(0, 3);
            actionChangeTimer = delayBeforeActionChange;
        }
    }
}
