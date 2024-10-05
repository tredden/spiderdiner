using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleInfluencer : ObstacleBase
{
    protected float influenceRadius;

    public override bool GetDoesInteract(ref Fly fly, float dt)
    {
        float dx = fly.x - this.transform.position.x;
        float dy = fly.y - this.transform.position.y;

        return Mathf.Sqrt(dx*dx + dy*dy) <= influenceRadius;
    }
}
