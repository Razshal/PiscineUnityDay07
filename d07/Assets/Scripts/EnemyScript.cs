using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private GameObject[] tanks;
    private GameObject closestTarget;
    private CanonScript canon;

    private GameObject GetClosestTarget()
    {
        GameObject target = null;
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

    // Use this for initialization
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        tanks = GameObject.FindGameObjectsWithTag("Tanks");
        canon = transform.Find("canon").GetComponent<CanonScript>();
        closestTarget = tanks[0];
        closestTarget = GetClosestTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, closestTarget.transform.position) > canon.fireRange - 10 && !navMeshAgent.hasPath)
            navMeshAgent.SetDestination(closestTarget.transform.position);
        else if (!closestTarget)
            closestTarget = GetClosestTarget();
        else
        {
            navMeshAgent.isStopped = true;
            transform.LookAt()
        }
    }
}
