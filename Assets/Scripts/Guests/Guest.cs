using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GuestStatus { UNSET = 0, WAITING_FOR_TABLE, PONDERING_ORDER, WAITING_FOR_ORDER, EATING, WAITING_FOR_CHECK, FINISHED };

public class Guest : MonoBehaviour
{
    [SerializeField]
    GuestOrder activeOrder;

    [SerializeField]
    TMPro.TMP_Text statusText;

    [SerializeField]
    GuestStatus status = GuestStatus.UNSET;
    [SerializeField]
    float eatingTime = 3;
    [SerializeField]
    float eatingTimeLeft = 3;

    public void OnSeated(Dish d)
    {
        UpdateOrderStatus(d);
    }
    
    public void UpdateOrderStatus(Dish d)
    {
        UpdateDishString(activeOrder, d);
        CheckForSatisfied(activeOrder, d);
    }

    void UpdateDishString(GuestOrder order, Dish d)
    {
        int firstText = d.flyAmount;
        int secondText = order.flyAmount;
        if (firstText > secondText) {
            firstText = secondText;
            statusText.fontStyle = TMPro.FontStyles.Bold & TMPro.FontStyles.Strikethrough;
        } else {
            statusText.fontStyle = TMPro.FontStyles.Bold;
        }
        statusText.text = firstText + " / " + secondText;
    }

    void CheckForSatisfied(GuestOrder order, Dish d)
    {
        if (d.flyAmount >= order.flyAmount && status == GuestStatus.WAITING_FOR_ORDER) {
            SetStatus(GuestStatus.EATING);
            // do more stuff
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
