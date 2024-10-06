using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceWeb : LineInfluencer
{
    Vector2 dp;
    Vector2 df;
    Vector2 n;

    public override void InfluenceFly(ref Fly fly, float dt)
    {   
        float pxa = pointA.x;
        float pxb = pointB.x;
        float pdx = pxb - pxa;

        float pya = pointA.y;
        float pyb = pointB.y;
        float pdy = pyb - pya;

        float fdx = fly.vx + fly.dvx * dt;
        float fdy = fly.vy + fly.dvy * dt;

        //float fx0 = fly.x;
        //float fx1 = fly.x + fdx * dt;
        
        //float fy0 = fly.y;
        //float fy1 = fly.y + fdy * dt;

        // GetIntersectPoint(ref x, ref y, fx0, fy0, fx1, fy1, pxa, pya, pxb, pyb);

        if (pdx == 0) {
            fly.vy *= -1f;
            fly.dvy *= -1f;
            fly.y += fly.vy * width / 2f;
        } else if (pdy == 0) {
            fly.vx *= -1f;
            fly.dvx *= -1f;
            fly.x += fly.vx * width / 2f;
        } else {
            dp.x = pdx;
            dp.y = pdy;
            df.x = fdx;
            df.y = fdy;

            n.x = dp.y;
            n.y = -dp.x;
            n.Normalize();

            df = df - 2 * Vector2.Dot(df, n) * n;
            fly.vx = df.x;
            fly.vy = df.y;
            fly.dvx = 0;
            fly.dvy = 0;
            fly.x += df.normalized.x * width / 2f;
            fly.y += df.normalized.y * width / 2f;
            // calculate angle of incidence and set velocity and accelaration accordingly
        }
    }
}
