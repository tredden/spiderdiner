using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceCloud : CircleInfluencer
{
    public override bool GetDoesInteract(ref Fly fly, float dt)
    {
        float x1 = fly.x + fly.vx * dt + fly.dvx * dt; // note: would be dt^2, but we apply vel before pos
        float y1 = fly.y + fly.vy * dt + fly.dvy * dt;
        return PointIsInCircle(x1, y1) && !PointIsInCircle(fly.x, fly.y);
    }

    public override void InfluenceFly(ref Fly fly, float dt)
    {
        fly.spiceLevel += 1;
    }

    public override int GetActOrder()
    {
        return 100;
    }
}
