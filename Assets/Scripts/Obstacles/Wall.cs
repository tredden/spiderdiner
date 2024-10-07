using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BoxInfluencer
{
    public override void InfluenceFly(ref Fly fly, float dt)
    {
        fly.disable = true;
    }
}
