using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
	private List<GameObject> tanks = new List<GameObject>();
    private NavMeshAgent navMeshAgent;
    private CanonScript canon;
    private RaycastHit raycastHit;
    private Vector3 targetPos;
	public GameObject closestTarget;

    private GameObject GetClosestTarget()
    {
        GameObject target = tanks[0];
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

    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
		canon = transform.Find("canon").GetComponent<CanonScript>();

        foreach(GameObject tank in GameObject.FindGameObjectsWithTag("Tanks"))
        {
            if (tank != gameObject)
                tanks.Add(tank);
        }
        closestTarget = GetClosestTarget();
    }

	private void FixedUpdate()
	{
        if (!closestTarget)
            closestTarget = GetClosestTarget();
        if (Vector3.Distance(transform.position, closestTarget.transform.position) > (canon.fireRange / 2))
        {
            canon.StopRotary();
            navMeshAgent.SetDestination(closestTarget.transform.position);
        }
        else
        {
            targetPos = transform.position - closestTarget.transform.position;
            targetPos.y = 0;
            transform.rotation = Quaternion.LookRotation(-targetPos);
            navMeshAgent.isStopped = true;
            if (!canon.isFiring)
                canon.StartRotary();
        }
	}

	// Update is called once per frame
	void Update()
    {

    }
}
