using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleInfluencer : ObstacleBase
{
    [SerializeField]
    protected float rawInfluenceRadius;

    public override bool GetDoesInteract(ref Fly fly, float dt)
    {
        float dx = fly.x - this.transform.position.x;
        float dy = fly.y - this.transform.position.y;

        bool interact = Mathf.Sqrt(dx * dx + dy * dy) <= rawInfluenceRadius;
        // Debug.Log("Check interact (dx = " + dx + ", dy = " + dy + ", radius = " + rawInfluenceRadius + ", interact = " + interact + ")");
        return interact;
    }
}
