using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GuestStatus { UNSET = 0, WAITING_FOR_TABLE, PONDERING_ORDER, WAITING_FOR_ORDER, EATING, WAITING_FOR_CHECK, FINISHED };

public class Guest : MonoBehaviour
{
    [SerializeField]
    public GuestOrder activeOrder;

    [SerializeField]
    OrderCanvas orderCanvas;

    [SerializeField]
    GuestStatus status = GuestStatus.UNSET;
    [SerializeField]
    float eatingTime = 15;
    [SerializeField]
    float eatingTimeLeft = 15;

    public void UpdateText() {
        orderCanvas.UpdateOrder(activeOrder);
    }
    public bool ReceiveFly(Fly fly)
    {
        bool received = activeOrder.ReceiveFly(fly);
        orderCanvas.UpdateOrder(activeOrder);
        if (received)
        {
        
            CheckForSatisfied(activeOrder);
        }
        // Possibly animate eating...
        return received;
    }
    void CheckForSatisfied(GuestOrder order)
    {
        bool orderDone = order.CheckDone();
        if (status == GuestStatus.WAITING_FOR_ORDER && !orderDone) {
            SetStatus(GuestStatus.EATING);
            // do more stuff
        }
        else if (status == GuestStatus.EATING) {
            if (orderDone) {
                SetStatus(GuestStatus.WAITING_FOR_CHECK);
            }
        } else if (status != GuestStatus.WAITING_FOR_ORDER) {
            SetStatus(GuestStatus.WAITING_FOR_ORDER);
        }
    }

    public void UpdateEatingTime(float dt)
    {
        eatingTimeLeft -= dt;
        if (eatingTimeLeft <= 0f) {
            SetStatus(GuestStatus.WAITING_FOR_CHECK);
        }
    }

    private void Start()
    {
        GuestManager.GetInstance().RegisterGuest(this);
    }

    public GuestStatus GetStatus()
    {
        return status;
    }

    public void SetStatus(GuestStatus status)
    {
        this.status = status;
    }
}
