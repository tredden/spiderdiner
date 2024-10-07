using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class CircleInfluencer : ObstacleBase
{
    [SerializeField]
    NavMeshObstacle navPrefab;
    
    [SerializeField]
    protected float rawInfluenceRadius;

    [SerializeField]
    protected bool carvesNavigation = false;

    NavMeshObstacle navObstacle;

    protected virtual float getNavObsRadius()
    {
        return rawInfluenceRadius;
    }

    protected override NavMeshObstacle GetNavObstacle()
    {
        if (Application.isEditor) {
            return null;
        }
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

    protected virtual void UpdateNavObstacle()
    {
        if (Application.isEditor) {
            return;
        }
        Vector3 pos = navObstacle.transform.position;
        pos.x = this.transform.position.x;
        pos.y = this.transform.position.y;
        navObstacle.transform.position = pos;
        navObstacle.radius = getNavObsRadius();
    }

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
