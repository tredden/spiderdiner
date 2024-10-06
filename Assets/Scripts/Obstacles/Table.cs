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

    void AddFlyToCount()
    {
        // TODO: update guest
        particles.Emit(1);
        dish.flyAmount++;
        if (activeGuest != null) {
            activeGuest.UpdateOrderStatus(dish);
        }
    }

    void ClearTable()
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
