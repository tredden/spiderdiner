using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        OnPlace();
    }

    protected virtual void OnDestroy()
    {
        OnRemove();
    }

    void OnPlace()
    {
        FlyManager.GetInstance().RegisterObstacle(this);
    }

    void OnRemove()
    {
        FlyManager.GetInstance().DeregisterObstacle(this);
    }

    public virtual int GetActOrder()
    {
        return 0; // 0 = other flies
    }

    // OVERRIDE ME BASED ON PHYSICS SPACE
    public virtual bool GetDoesInteract(ref Fly fly, float dt)
    {
        // NOP
        return false;
    }

    public virtual void InfluenceFly(ref Fly fly, float dt)
    {
        // NOP
    }
}
