using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repulsor : Attractor
{
    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destructorRadius);
    }
}
