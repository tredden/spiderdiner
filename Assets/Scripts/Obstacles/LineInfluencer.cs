using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LineInfluencer : ObstacleBase
{
    [SerializeField]
    protected Vector2 pointA;
    [SerializeField]
    protected Vector2 pointB;
    [SerializeField]
    protected float width;

    [SerializeField]
    SpriteRenderer lineRender;
    [SerializeField]
    bool changeLineRenderWidth = false;
    [SerializeField]
    bool tryPreserveOrientation = false;

    [SerializeField]
    NavMeshObstacle navPrefab;
    [SerializeField]
    protected bool carvesNavigation = false;
    NavMeshObstacle navObstacle;

    protected float x;
    protected float y;

    float fx0;
    float fy0;
    float fx1;
    float fy1;

    float fdx;
    float fdy;
    float pdx;
    float pdy;

    public void SetPointA(float x, float y)
    {
        pointA.x = x;
        pointA.y = y;
        UpdateVisual();
        UpdateNavObstacle();
    }

    public void SetPointB(float x, float y)
    {
        pointB.x = x;
        pointB.y = y;
        UpdateVisual();
        UpdateNavObstacle();
    }

    protected override NavMeshObstacle GetNavObstacle()
    {
        if (navObstacle != null) {
            return navObstacle;
        }
        if (navObstacle == null && carvesNavigation) {
            navObstacle = GameObject.Instantiate<NavMeshObstacle>(navPrefab);
            UpdateNavObstacle();
            return navObstacle;
        }
        return null;
    }

    [ExecuteInEditMode]
    protected virtual void UpdateNavObstacle()
    {
        if (navObstacle != null) {
            Vector3 pos;
            pos.x = (pointA.x + pointB.x) / 2f;
            pos.y = (pointA.y + pointB.y) / 2f;
            pos.z = navObstacle.transform.position.z;
            navObstacle.transform.position = pos;
            float dx = pointB.x - pointA.x;
            float dy = pointB.y - pointA.y;
            navObstacle.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dy, dx) * Mathf.Rad2Deg);
            Vector3 scale = navObstacle.transform.localScale;
            scale.x = Mathf.Sqrt(dx * dx + dy * dy);
            scale.z = width;
            navObstacle.transform.localScale = scale;
        }
    }

    public float GetDist()
    {
        return Vector2.Distance(pointA, pointB);
    }

    protected void GetIntersectPoint(ref float x, ref float y, float fx0, float fy0, float fx1, float fy1, float pxa, float pya, float pxb, float pyb)
    {
        fdx = fx1 - fx0;
        fdy = fy1 - fy0;
        pdx = pxb - pxa;
        pdy = pyb - pya;
        if (Mathf.Abs(fdx) <= 0.0001f && Mathf.Abs(pdx) <= 0.0001f) {
            // Debug.Log("both vertical");
            x = fx0;
            y = Mathf.Clamp(fy0, Mathf.Min(pya, pyb), Mathf.Max(pya, pyb));
        } else if (Mathf.Abs(fdy) <= 0.0001f && Mathf.Abs(pdy) <= 0.0001f) {
            // Debug.Log("both horizontal");
            y = fy0;
            x = Mathf.Clamp(fx0, Mathf.Min(pxa, pxb), Mathf.Max(pxa, pxb));
        } else if (Mathf.Abs(fdx) <= 0.0001f) {
            // Debug.Log("f vertical");
            x = fx0;
            float w = (pdy / pdx);
            y = w * (x - pxa) + pya;
        } else if (Mathf.Abs(pdx) <= 0.0001f) {
            // Debug.Log("p vertical");
            x = fx0;
            float q = (fdy / fdx);
            y = q * (x - fx0) + fy0;
        } else if (Mathf.Abs(fdy) < 0.0001f) {
            // Debug.Log("f horizontal");
            y = fy0;
            float wi = (pdx / pdy);
            x = wi * (y - pya) + pxa;
        } else if (Mathf.Abs(pdy) < 0.0001f) {
            // Debug.Log("p horizontal");
            y = fy0;
            float qi = (fdx / fdy);
            x = qi * (y - fy0) + fx0;
        } else { 
            // Debug.Log("no vertical: fdx = " + fdx + ", pdx = " + pdx);
            float q = (fdy / fdx);
            float w = (pdy / pdx);

            // y = q(x - fx0) + fy0
            // y = w(x - pxa) + pya
            x = (q * fx0 - fy0 - w * pxa + pya) / (q - w);
            y = q * (x - fx0) + fy0;
        }
    }

    public override bool GetDoesInteract(ref Fly fly, float dt)
    {
        float pxa = pointA.x;
        float pxb = pointB.x;
        float pdx = pxb - pxa;
        float pxMin = Mathf.Min(pxa, pxb);
        float pxMax = Mathf.Max(pxa, pxb);

        float pya = pointA.y;
        float pyb = pointB.y;
        float pdy = pyb - pya;
        float pyMin = Mathf.Min(pya, pyb);
        float pyMax = Mathf.Max(pya, pyb);

        fdx = fly.vx + fly.dvx * dt;
        fdy = fly.vy + fly.dvy * dt;

        fx0 = fly.x;
        fx1 = fly.x + fdx * dt;
        float fxMin = Mathf.Min(fx0, fx1);
        float fxMax = Mathf.Max(fx0, fx1);

        fy0 = fly.y;
        fy1 = fly.y + fdy * dt;
        float fyMin = Mathf.Min(fy0, fy1);
        float fyMax = Mathf.Max(fy0, fy1);

        GetIntersectPoint(ref x, ref y, fx0, fy0, fx1, fy1, pxa, pya, pxb, pyb);

        // now check that x,y is on <f1 - f0> and on <B - A>
        bool xfMinCheck = x >= fxMin - width / 2f;
        bool xfMaxCheck = x <= fxMax + width / 2f;
        bool xpMinCheck = x >= pxMin - width / 2f;
        bool xpMaxCheck = x <= pxMax + width / 2f;
        bool yfMinCheck = y >= fyMin - width / 2f;
        bool yfMaxCheck = y <= fyMax + width / 2f;
        bool ypMinCheck = y >= pyMin - width / 2f;
        bool ypMaxCheck = y <= pyMax + width / 2f;

        // Debug.Log("fx0 = " + fx0 + ", fdx = "+fdx+", fy0 = " + fy0 + ", fdy = " + fdy + ", x = " + x + ", y = " + y + ", xfMinCheck: " + xfMinCheck + ", xfMaxCheck: " + xfMaxCheck + ", xpMinCheck: " + xpMinCheck + ", xpMaxCheck: " + xpMaxCheck + ", yfMinCheck: " + yfMinCheck + ", yfMaxCheck: " + yfMaxCheck + ", ypMinCheck: " + ypMinCheck + ", ypMaxCheck: " + ypMaxCheck);
        return xfMinCheck && xfMaxCheck && xpMinCheck && xpMaxCheck && yfMinCheck && yfMaxCheck && ypMinCheck && ypMaxCheck;
    }

    public override int GetActOrder()
    {
        return 10;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        // TODO: draw as circles + 2 lines for width
        Gizmos.DrawWireSphere(new Vector3(pointA.x, pointA.y, 0), width / 2f);
        Gizmos.DrawWireSphere(new Vector3(pointB.x, pointB.y, 0), width / 2f);
        Gizmos.DrawLine(new Vector3(pointA.x, pointA.y + width / 2f, 0), new Vector3(pointB.x, pointB.y + width / 2f, 0));
        Gizmos.DrawLine(new Vector3(pointA.x, pointA.y - width / 2f, 0), new Vector3(pointB.x, pointB.y - width / 2f, 0));
        Gizmos.DrawLine(new Vector3(pointA.x + width / 2f, pointA.y, 0), new Vector3(pointB.x + width / 2f, pointB.y, 0));
        Gizmos.DrawLine(new Vector3(pointA.x - width / 2f, pointA.y, 0), new Vector3(pointB.x - width / 2f, pointB.y, 0));


        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(fx0, fy0, 0), new Vector3(fx0 + (fx1 - fx0) * 1000f, fy0 + (fy1 - fy0)*1000f, 0));
        Gizmos.DrawSphere(new Vector3(x, y, 0), 0.25f);
        
    }

    Vector3 pos;
    Vector2 size;

    [ExecuteInEditMode]
    private void UpdateVisual()
    {
        pos.x = (pointA.x + pointB.x) / 2f;
        pos.y = (pointA.y + pointB.y) / 2f;
        pos.z = -pos.y;
        float dx = pointB.x - pointA.x;
        float dy = pointB.y - pointA.y;
        lineRender.transform.position = pos;
        float ez = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        size.x = Mathf.Sqrt(dx * dx + dy * dy);
        size.y = changeLineRenderWidth ? width : lineRender.size.y;
        if (tryPreserveOrientation) {
            ez = (ez + 360f) % 360f; // cut any weird negative angle below -1 rots, should never happen
            if (ez <= 45 || ez >= 315) {
                // Do nothing
            } else if (ez > 45 && ez < 135) {
                ez -= 90;
                float temp = size.x;
                size.x = size.y;
                size.y = temp;
            } else if (ez >= 135 && ez <= 225) {
                ez -= 180;
            } else {
                ez -= 270;
                float temp = size.x;
                size.x = size.y;
                size.y = temp;
            }
        }
        lineRender.transform.rotation = Quaternion.Euler(0f, 0f, ez);
        lineRender.size = size;
    }

    [ExecuteInEditMode]
    private void Update()
    {
        UpdateVisual();
        UpdateNavObstacle();
    }
}
