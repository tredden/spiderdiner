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

    Vector3 flyPos = Vector3.zero;
    Vector3 flyVel = Vector3.zero;
    ParticleSystem.EmitParams emitParams;
    void RenderCapturedFly(ref Fly fly)
    {
        flyPos = (Random.insideUnitSphere * particles.shape.radius);
        flyPos.z = 0;
        flyPos += particles.shape.position;
        emitParams.position = flyPos;
        emitParams.velocity = flyVel;
        emitParams.startColor = fly.getUnityColor();
        particles.Emit(emitParams, 1);
    }

    public override void InfluenceFly(ref Fly fly, float dt)
    {
        if (fly.disable) {
            return;
        }
        fly.disable = true;
        RenderCapturedFly(ref fly);
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
