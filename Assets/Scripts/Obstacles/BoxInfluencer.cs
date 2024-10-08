using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoxInfluencer : ObstacleBase
{
    [SerializeField]
    Rect bounds;
    Vector2 checkPoint;

    [SerializeField]
    SpriteRenderer boxRender;

    [SerializeField]
    NavMeshObstacle navPrefab;
    [SerializeField]
    protected bool carvesNavigation = false;
    NavMeshObstacle navObstacle;

    public override bool GetDoesInteract(ref Fly fly, float dt)
    {
        return fly.x <= bounds.xMax && fly.x >= bounds.xMin && fly.y <= bounds.yMax && fly.y >= bounds.yMin;
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

    protected override void ClearNavObstacle()
    {
        if (navObstacle != null) {
            GameObject.Destroy(navObstacle.gameObject);
        }
    }

    protected virtual void UpdateNavObstacle()
    {
        if (navObstacle != null) {
            Vector3 pos;
            pos.x = bounds.center.x;
            pos.y = bounds.center.y;
            pos.z = navObstacle.transform.position.z;
            navObstacle.transform.position = pos;
            Vector3 scale = navObstacle.transform.localScale;
            scale.x = bounds.width;
            scale.z = bounds.height;
            navObstacle.transform.localScale = scale;
        }
    }

    protected virtual void UpdateVisual()
    {
        Vector3 pos = boxRender.transform.position;
        pos.x = bounds.center.x;
        pos.y = bounds.center.y;
        boxRender.transform.position = pos;
        boxRender.size = bounds.size;
    }

    private void Update()
    {
        UpdateVisual();
        UpdateNavObstacle();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(bounds.xMin, bounds.yMin, 0), new Vector3(bounds.xMax, bounds.yMin, 0));
        Gizmos.DrawLine(new Vector3(bounds.xMin, bounds.yMax, 0), new Vector3(bounds.xMax, bounds.yMax, 0));
        Gizmos.DrawLine(new Vector3(bounds.xMin, bounds.yMin, 0), new Vector3(bounds.xMin, bounds.yMax, 0));
        Gizmos.DrawLine(new Vector3(bounds.xMax, bounds.yMin, 0), new Vector3(bounds.xMax, bounds.yMax, 0));
    }
}
