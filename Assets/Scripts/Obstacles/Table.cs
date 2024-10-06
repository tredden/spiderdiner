using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : CircleInfluencer
{
    [SerializeField]
    ParticleSystem particles;

    [SerializeField]
    Guest activeGuest;

    private void Start()
    {
        base.Start();
        GuestManager.GetInstance().RegisterTable(this);
    }

    protected virtual void OnDestroy()
    {
        GuestManager.GetInstance().DeregisterTable(this);
    }

    public void SetGuest(Guest activeGuest)
    {
        this.activeGuest = activeGuest;
        activeGuest.transform.position = this.transform.position;
        activeGuest.transform.parent = this.transform;
    }

    public void RemoveGuest()
    {
        this.activeGuest = null;
    }

    public Guest GetGuest()
    {
        return activeGuest;
    }

    public bool IsOccupied()
    {
        return activeGuest != null;
    }

    public void ClearTable()
    {
        particles.Clear();
        if (activeGuest != null)
        {
            foreach (Dish d in activeGuest.activeOrder.eatenDishes)
            {
                // Maybe put tips in the eaten dishes?
                d.Clear();
            }
        }
    }

    public override void InfluenceFly(ref Fly fly, float dt)
    {
        // TODO: needs to trigger an "on influence" function for the table so that guests can eat the flies
        if (fly.disable || activeGuest == null)
        {
            return;
        }
        if (activeGuest.ReceiveFly(fly))
        {
            fly.disable = true;
            particles.Emit(1);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rawInfluenceRadius);
    }
}
