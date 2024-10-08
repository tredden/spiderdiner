using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : CircleInfluencer
{
    [SerializeField]
    protected float attractionRadius;
    float attractionRadiusSquared;

    [SerializeField]
    float attractionForce;
    float realAttractionForce;

    [SerializeField]
    protected float destructorRadius;
    float destructorRadiusSquared;

    protected override float getNavObsRadius()
    {
        return destructorRadius;
    }

    private void Update()
    {
        rawInfluenceRadius = Mathf.Max(attractionRadius, destructorRadius);
        attractionRadiusSquared = attractionRadius * attractionRadius;
        destructorRadiusSquared = destructorRadius * destructorRadius;
        realAttractionForce = attractionForce * GetAttractionMult();
    }

    protected virtual float GetAttractionMult()
    {
        return 1f;
    }

    float dx;
    float dy;
    float rSquared;
    float accMag;
    float dvx;
    float dvy;

    public override void InfluenceFly(ref Fly fly, float dt)
    {
        dx = fly.x - transform.position.x;
        dy = fly.y - transform.position.y;
        rSquared = dx * dx + dy * dy;

        if (rSquared <= destructorRadiusSquared) {
            fly.disable = true;
        } else if (rSquared <= attractionRadiusSquared) {
            accMag = (realAttractionForce / rSquared);

            dvx = -dx * accMag;
            dvy = -dy * accMag;

            fly.dvx += dvx;
            fly.dvy += dvy;
            // Debug.Log("Attractor (dvx = " + fly.dvx +", "+dvx  + ", dvy = " + fly.dvy + "," +dvy + ")");
        }
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destructorRadius);
    }
}
