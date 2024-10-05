using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : CircleInfluencer
{
    float _attractionRadius;
    [SerializeField]
    protected float attractionRadius {
        get {
            return _attractionRadius;
        }
        set {
            _attractionRadius = value;
            attractionRadiusSquared = attractionRadius * attractionRadius;
        }
    }
    float attractionRadiusSquared;

    [SerializeField]
    float _attractionForce;
    protected float attractionForce {
        get {
            return _attractionForce;
        }
        set {
            _attractionForce = value;
            realAttractionForce = attractionForce * GetAttractionMult();
        }
    }
    float realAttractionForce;

    float _destructorRadius;
    [SerializeField]
    protected float destructorRadius {
        get {
            return _destructorRadius;
        }
        set {
            _destructorRadius = value;
            destructorRadiusSquared = destructorRadius * destructorRadius;
        }
    }
    float destructorRadiusSquared;

    private void Start()
    {
        influenceRadius = Mathf.Max(attractionRadius, destructorRadius);
        attractionRadiusSquared = attractionRadius * attractionRadius;
        destructorRadiusSquared = destructorRadius * destructorRadius;
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
            accMag = (realAttractionForce / rSquared) * dt;

            dvx = -dx * accMag;
            dvy = -dy * accMag;

            fly.dvx += dvx;
            fly.dvy += dvy;
        }
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destructorRadius);
    }
}
