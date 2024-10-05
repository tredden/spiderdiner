using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyBase { UNSET = 0, BLUE = 1, }

[System.Serializable]
public struct Fly {
    FlyBase flyBase;
    int spiceLevel;

    public Fly(FlyBase flyBase, int spiceLevel)
    {
        this.flyBase = flyBase;
        this.spiceLevel = spiceLevel;
    }
}
