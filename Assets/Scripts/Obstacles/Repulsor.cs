using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repulsor : Attractor
{
    protected override float GetAttractionMult()
    {
        return -1f;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destructorRadius);
    }
}
