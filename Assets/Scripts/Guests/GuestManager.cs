using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GuestOrder
{
	List<Dish> dishes;
    // If true, the guest will require dishes to be served in order.
    bool multiCourse;

    public bool CheckDone() {
        return dishes.Count == 0;
    }

    public bool ReceiveFly(Fly fly) {
        for (int i = 0; i < dishes.Count; i++) {
            if (multiCourse && i > 0) {
                // Only the first dish can receive flies in multi-course mode
                return false;
            }
            if (dishes[i].ReceiveFly(fly) && dishes[i].CheckDone()) {
                dishes.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public struct Dish
{
    public int flyAmount;
    public int spiceLevel;
    public int color;

    public bool CheckDone() {
        return flyAmount == 0;
    }

    public bool ReceiveFly(Fly fly) {
        if (fly.color == color && fly.spiceLevel == spiceLevel) {
            flyAmount--;
            return true;
        }
        return false;
    }
}

public class GuestManager : MonoBehaviour
{
    static GuestManager instance;
    public static GuestManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
