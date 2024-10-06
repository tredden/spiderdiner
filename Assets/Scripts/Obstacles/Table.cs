using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : CircleInfluencer
{
    [SerializeField]
    ParticleSystem particles;

    [SerializeField]
    Guest activeGuest;

    [SerializeField]
    Dish dish = new Dish();

    private void Start()
    {
        base.Start();
        GuestManager.GetInstance().RegisterTable(this);
    }

    protected virtual void OnDestroy()
    {
        GuestManager.GetInstance().DeregisterTable(this);
    }

    void AddFlyToCount()
    {
        particles.Emit(1);
        dish.flyAmount++;
        if (activeGuest != null) {
            activeGuest.UpdateOrderStatus(dish);
        }
    }

    public void SetGuest(Guest activeGuest)
    {
        this.activeGuest = activeGuest;
        activeGuest.transform.position = this.transform.position;
        activeGuest.transform.parent = this.transform;
        activeGuest.UpdateOrderStatus(dish);
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
        dish.Clear();
    }

    public override void InfluenceFly(ref Fly fly, float dt)
    {
        // TODO: needs to trigger an "on influence" function for the table so that guests can eat the flies
        fly.disable = true;
        AddFlyToCount();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rawInfluenceRadius);
    }
}
