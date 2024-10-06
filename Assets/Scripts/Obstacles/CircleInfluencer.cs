using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleInfluencer : ObstacleBase
{
    [SerializeField]
    protected float rawInfluenceRadius;

    protected bool PointIsInCircle(float x, float y)
    {
        float dx = x - this.transform.position.x;
        float dy = y - this.transform.position.y;
        bool interact = Mathf.Sqrt(dx * dx + dy * dy) <= rawInfluenceRadius;
        return interact;
    }

    public override bool GetDoesInteract(ref Fly fly, float dt)
    {
        return PointIsInCircle(fly.x, fly.y);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rawInfluenceRadius);
    }
}
