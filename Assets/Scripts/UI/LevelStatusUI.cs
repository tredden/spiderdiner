using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatusUI : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text guestsText;
    [SerializeField]
    UnityEngine.UI.Image barFill;
    [SerializeField]
    UnityEngine.UI.Image tipThresholdIndicator;

    [SerializeField]
    float barWidth = 100;

    float currentSatisfaction = 0f;
    float maxSatisfaction = 100f;
    float satisfactionTargetFraction = .7f;
    int guestsRemaining;
    int maxGuests;

    public void SetMaxSatisfaction(float value)
    {
        if (maxSatisfaction != value) {
            maxSatisfaction = value;
            UpdateFillBar();
        }
    }

    public void SetCurrentSatisfaction(float value)
    {
        if (currentSatisfaction != value) {
            currentSatisfaction = value;
            UpdateFillBar();
        }
    }

    public void SetSatisfactionTarget(float fraction)
    {
        if (satisfactionTargetFraction != fraction) {
            satisfactionTargetFraction = fraction;
            UpdateFillBar();
        }
    }

    public bool GetMetTarget()
    {
        return currentSatisfaction / maxSatisfaction >= satisfactionTargetFraction;
    }

    public void SetMaxGuests(int value)
    {
        if (maxGuests != value) {
            maxGuests = value;
            UpdateGuestText();
        }
    }

    public void SetGuestsRemaining(int value)
    {
        if (guestsRemaining != value) {
            guestsRemaining = value;
            UpdateGuestText();
        }
    }

    public void UpdateGuestText()
    {
        guestsText.text = "" + guestsRemaining + " OF " + maxGuests + " GUESTS REMAINING";
    }

    public void UpdateFillBar()
    {
        Vector3 pos = tipThresholdIndicator.transform.localPosition;
        pos.x = satisfactionTargetFraction * barWidth;
        tipThresholdIndicator.transform.localPosition = pos;


        float filledFraction = currentSatisfaction / maxSatisfaction;
        Vector2 size = barFill.rectTransform.sizeDelta;
        size.x = filledFraction * barWidth;
        barFill.rectTransform.sizeDelta = size;
        barFill.color = filledFraction < 0.5 ? Color.Lerp(Color.red, Color.yellow, filledFraction * 2f) : Color.Lerp(Color.yellow, Color.green, filledFraction * 2f - 1f);
    }
}
