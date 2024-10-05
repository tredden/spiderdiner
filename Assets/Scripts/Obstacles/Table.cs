using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : CircleInfluencer
{
    public override void InfluenceFly(ref Fly fly, float dt)
    {
        // TODO: needs to trigger an "on influence" function for the table so that guests can eat the flies
        fly.disable = true;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rawInfluenceRadius);
    }
}
