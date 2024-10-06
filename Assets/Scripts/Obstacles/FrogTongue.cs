using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogTongue : LineInfluencer
{
    public override void InfluenceFly(ref Fly fly, float dt)
    {
        fly.disable = true;
    }
}
