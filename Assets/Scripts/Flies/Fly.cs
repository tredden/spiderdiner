using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Color { UNSET = 0, BLUE = 1, }

[System.Serializable]
public struct Fly {
    public float x;
    public float y;
    public float dx;
    public float dy;
    public Color color;
    int spiceLevel;
}    
