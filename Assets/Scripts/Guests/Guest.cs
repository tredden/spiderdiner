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
    private GuestStatus status = GuestStatus.UNSET;

    [SerializeField]
    private GuestStatus previousStatus = GuestStatus.UNSET;
    [SerializeField]
    float ponderingOrderTime = 2;
    [SerializeField]
    float ponderingOrderTimeLeft = 2;

    [SerializeField]
    float waitingForOrderReduceSatisfactionTime = 8;
    float waitingForOrderReduceSatisfactionTimeLeft = 8;

    [SerializeField]
    float eatingTime = 5;
    [SerializeField]
    float eatingTimeLeft = 5;

    [SerializeField]
    float waitingForCheckTime = 4;
    [SerializeField]
    float waitingForCheckTimeLeft = 4;

    [SerializeField]
    bool happy = true;

    [SerializeField]
    float maxSatisfaction = 100;
    float currentSatisfaction = 100;

    [SerializeField]
    float eatingBounceHeight = .2f;
    [SerializeField]
    float eatingBouncePeriod = .2f;
    [SerializeField]
    float eatingBounceSpike = 3f;

    public void UpdateText() {
        orderCanvas.UpdateOrder(activeOrder);
    }
    public bool ReceiveFly(Fly fly)
    {
        bool received = activeOrder.ReceiveFly(fly);
        orderCanvas.UpdateOrder(activeOrder);
        Update();
        return received;
    }

    public void Tick(float dt)
    {
        ponderingOrderTimeLeft -= dt;
        waitingForOrderReduceSatisfactionTimeLeft -= dt;
        eatingTimeLeft -= dt;
        waitingForCheckTimeLeft -= dt;

        if (!happy) {
            currentSatisfaction -= dt * 2;
        }

        Update();
    }
    public void Update()
    {
		if (previousStatus != status)
		{
			StateLog(previousStatus + " -> " + status);
		}

        switch (status)
        {
            case GuestStatus.WAITING_FOR_TABLE:
                // GuestManager will handle placing the guest at a table.
                // TODO?: Lose satisfaction if waiting too long?
                break;
            case GuestStatus.PONDERING_ORDER:
                if (previousStatus != GuestStatus.PONDERING_ORDER)
                {
					ponderingOrderTimeLeft = ponderingOrderTime;
                }
                if (ponderingOrderTimeLeft <= 0)
                {
                    SetStatus(GuestStatus.WAITING_FOR_ORDER);
                }
                break;
            case GuestStatus.WAITING_FOR_ORDER:

                if (activeOrder.readyDishes.Count > 0)
                {
                    SetStatus(GuestStatus.EATING);
                }
                if (waitingForOrderReduceSatisfactionTimeLeft <= 0)
                {
                    happy = false;
                    StateLog("Guest is unhappy.");
                }

                break;
            case GuestStatus.EATING:
                if (previousStatus != GuestStatus.EATING)
                {
					happy = true;
                    eatingTimeLeft = eatingTime;
                }

                // Bounce up and down while eating
                Vector3 pos = transform.localPosition;
                pos.y = Mathf.Pow(Mathf.Sin((eatingTimeLeft / eatingBouncePeriod) * Mathf.PI * 2f), eatingBounceSpike) * eatingBounceHeight;
                this.transform.localPosition = pos;

                if (eatingTimeLeft <= 0)
                {
                    activeOrder.EatDish();
                    if (activeOrder.CheckDone())
                    {
                        SetStatus(GuestStatus.WAITING_FOR_CHECK);
                    }
                    else
                    {
                        SetStatus(GuestStatus.WAITING_FOR_ORDER);
                    }
                }
                break;
            case GuestStatus.WAITING_FOR_CHECK:
                if (previousStatus != GuestStatus.WAITING_FOR_CHECK)
                {
                    waitingForCheckTimeLeft = waitingForCheckTime;
                }
                if (waitingForCheckTimeLeft <= 0)
                {
                    SetStatus(GuestStatus.FINISHED);
                }
                break;
            case GuestStatus.FINISHED:
                // TODO: Add to score.
                break;
            default:
                break;
        }
        this.previousStatus = this.status;
    }

    private void StateLog(string message)
    {
        Debug.Log("Guest \"" + this.name + "\": " + message);
    }

    private void Start()
    {
        currentSatisfaction = maxSatisfaction;
        GuestManager.GetInstance().RegisterGuest(this);
    }

    public GuestStatus GetStatus()
    {
        return status;
    }

    public float GetMaxSatisfaction()
    {
        return maxSatisfaction;
    }

    public float GetCurrentSatisfaction()
    {
        return currentSatisfaction;
    }

    public void SetStatus(GuestStatus status)
    {
        this.status = status;
        this.Update();
    }
}
