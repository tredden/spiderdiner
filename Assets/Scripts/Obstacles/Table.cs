using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : CircleInfluencer
{
    [SerializeField]
    ParticleSystem particles;

    [SerializeField]
    Guest activeGuest;

    protected override void Start()
    {
        base.Start();
        GuestManager.GetInstance().RegisterTable(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GuestManager.GetInstance().DeregisterTable(this);
    }

    public void SetGuest(Guest activeGuest)
    {
        this.activeGuest = activeGuest;
        activeGuest.UpdateText();
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
        if (fly.disable) {
            return;
        }
        fly.disable = true;
        particles.Emit(1);
        if (activeGuest == null)
        {
            return;
        }
        activeGuest.ReceiveFly(fly);
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rawInfluenceRadius);
    }
}
