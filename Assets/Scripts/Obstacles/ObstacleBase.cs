using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObstacleBase : MonoBehaviour
{
    protected virtual NavMeshObstacle GetNavObstacle()
    {
        return null;
    }

    protected virtual void ClearNavObstacle()
    {
        // NOP
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        OnPlace();
    }

    protected virtual void OnDestroy()
    {
        OnRemove();
    }

    public void OnPlace()
    {
        FlyManager.GetInstance().RegisterObstacle(this);
        //Debug.Log("Getting NavObstacle for : " + this.name);
        NavMeshObstacle obs = GetNavObstacle();
        if (obs != null) {
            //Debug.Log("Registering NavObstacle for : " + this.name);
            NavManager.GetInstance().RegisterObstacle(this, obs);
        } /*else {
            Debug.Log("Null NavObstacle for : " + this.name);
        }*/
    }

    public void OnRemove()
    {
        FlyManager.GetInstance().DeregisterObstacle(this);
        NavManager.GetInstance().DeregisterObstacle(this);
        ClearNavObstacle();
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
