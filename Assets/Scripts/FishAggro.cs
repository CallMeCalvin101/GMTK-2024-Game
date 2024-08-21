using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishAggro : MonoBehaviour
{
    public const int AGGRO_THRESHOLD = 100;
    public const int BITE_DISTANCE = 10;
    public const int DEFAULT_BITE_COOLDOWN = 4;

    public int aggro = 0;
    public bool isAggro = false;
    private float biteCooldown = 0;

    private Animation anim;

    public GameObject self;
    public NavMeshAgent agent;

    public GameObject target;

    void Start()
    {
        anim = self.GetComponent<Animation>();
    }
    void Update()
    {
        // set aggro state

        if (!isAggro && aggro >= AGGRO_THRESHOLD)
        {
            agent.SetDestination(target.transform.position);
        }
        isAggro = aggro >= AGGRO_THRESHOLD;

        if (isAggro
            && biteCooldown <= 0
            && Vector3.Distance(agent.transform.position, target.transform.position) <= BITE_DISTANCE)
        {
            bite();
            biteCooldown = DEFAULT_BITE_COOLDOWN;
        }

        if (biteCooldown > 0) biteCooldown -= Time.deltaTime;
    }

    public void raiseAggro(int amount)
    {
        aggro += amount;
    }

    public void bite()
    {
        // todo play animation and do smth to player
        anim.Play("bite");
        anim.PlayQueued("slowSwim_f");
    }
}
