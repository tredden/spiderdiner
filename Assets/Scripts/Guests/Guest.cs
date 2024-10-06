using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GuestStatus { UNSET = 0, WAITING_FOR_TABLE, PONDERING_ORDER, WAITING_FOR_ORDER, EATING, WAITING_FOR_CHECK, FINISHED };

public class Guest : MonoBehaviour
{
    [SerializeField]
    public GuestOrder activeOrder;

    [SerializeField]
    TMPro.TMP_Text statusText;

    [SerializeField]
    GuestStatus status = GuestStatus.UNSET;
    [SerializeField]
    float eatingTime = 3;
    [SerializeField]
    float eatingTimeLeft = 3;

    public bool ReceiveFly(Fly fly)
    {
        bool received = activeOrder.ReceiveFly(fly);
        
        
        // Possibly animate eating...
        return received;
    }

    void UpdateOrderString()
    {

        statusText.text = "";
        foreach (Dish dish in activeOrder.dishes)
        {
            int firstText = dish.fliesEaten;
            int secondText = dish.fliesInDish;
            if (firstText > secondText)
            {
                firstText = secondText;
                statusText.fontStyle = TMPro.FontStyles.Bold & TMPro.FontStyles.Strikethrough;
            }
            else
            {
                statusText.fontStyle = TMPro.FontStyles.Bold;
            }
            statusText.text += firstText + " / " + secondText + "\n";
        }

    }

    void CheckForSatisfied(GuestOrder order)
    {
        if (!order.CheckDone() && status == GuestStatus.WAITING_FOR_ORDER) {
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
