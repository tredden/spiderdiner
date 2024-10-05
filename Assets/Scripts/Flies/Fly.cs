using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyColor { UNSET = 0, BLUE = 1, }


[System.Serializable]
public struct Fly {
    public float x;
    public float y;
    public float vx;
    public float vy;

    // Delta velocity without dt applied
    public float dvx;
    public float dvy;
    public FlyColor color;
    public int spiceLevel;
    public bool enabled;
    public bool disable;
}    
