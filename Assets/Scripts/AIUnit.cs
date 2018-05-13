using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIUnit : MonoBehaviour {

    NavMeshAgent navMeshAgent;

    City.Resources holding;
    City heading;

	// Use this for initialization
	void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        holding = City.Resources.None;
        navMeshAgent.isStopped = false;

        SetDestination(AIManager.singleton.GetDestination());
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (navMeshAgent.remainingDistance - navMeshAgent.stoppingDistance < 2f)
        {
            if (heading == null)
                return;
            //arrived at destination!
            SellResource(heading);
            TakeResource(heading);
            SetDestination(AIManager.singleton.GetDestination());
        }
    }

    void TakeResource(City city)
    {
        if(city.AIBuy())
            holding = city.export;
    }

    void SellResource(City city)
    {
        if(holding == City.Resources.None)
        {
            return;
        }
        city.AISell(holding);
        holding = City.Resources.None;
    }

    void SetDestination(City dest)
    {
        heading = dest;
        navMeshAgent.SetDestination(dest.transform.position);
    }
}
