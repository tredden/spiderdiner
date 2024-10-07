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

    void UpdateGuestText()
    {
        guestsText.text = "(" + guestsRemaining + " OF " + maxGuests + " GUESTS REMAINING)";
    }

    void UpdateFillBar()
    {
        Vector3 pos = tipThresholdIndicator.transform.localPosition;
        pos.x = satisfactionTargetFraction * barWidth;

        float filledFraction = currentSatisfaction / maxSatisfaction;
        Vector2 size = barFill.rectTransform.sizeDelta;
        size.x = pos.x;
        barFill.rectTransform.sizeDelta = size;
        Color.Lerp(Color.red, Color.green, filledFraction);
        barFill.color = Color.Lerp(Color.red, Color.green, filledFraction); ;
    }
}
