using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GuestOrder
{
    public int flyAmount;
}

[System.Serializable]
public struct Dish
{
    public int flyAmount;

    public void Clear()
    {
        flyAmount = 0;
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
