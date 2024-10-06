using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest : MonoBehaviour
{
    [SerializeField]
    GuestOrder activeOrder;

    [SerializeField]
    TMPro.TMP_Text status;

    bool fullyServed = false;
    bool doneEating = false;

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
        status.text = d.flyAmount + " / " + order.flyAmount;
    }

    void CheckForSatisfied(GuestOrder order, Dish d)
    {
        if (d.flyAmount >= order.flyAmount) {
            fullyServed = true;
            // do more stuff
        } else {
            fullyServed = false;
        }
    }
}
