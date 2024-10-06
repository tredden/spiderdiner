using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyColor { WHITE = 0, BLUE = 1, RED = 2}

[System.Serializable]
public struct Fly {
    public float x;// = 0;
    public float y;// = 0;
    public float vx;// = 0;
    public float vy;// = 0;

    // Delta velocity without dt applied
    public float dvx;// = 0;
    public float dvy;// = 0;
    public FlyColor color;// = FlyColor.WHITE;
    public int spiceLevel;// = 0;
    public bool enabled;// = false;
    public bool disable;// = false;
}    
