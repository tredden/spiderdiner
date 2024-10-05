using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyColor { UNSET = 0, BLUE = 1, }

public struct BoidsUpdate {
	// Separation is handled immediately by adding to dvx and dvy.
    // The other rules need to know the number of neighbors, so
    // they are applied to dvx and dvy at the end.

    public int neighboring_boids;
    // Alignment
    public float xvel_avg;
    public float yvel_avg;


    // Cohesion
    public float xpos_avg;
    public float ypos_avg;
}

[System.Serializable]
public struct Fly {
    public float x;
    public float y;
    public float vx;
    public float vy;
    public float dvx;
    public float dvy;
    public FlyColor color;
    int spiceLevel;

    // Temporary values for looping through and updating boids behavior.
    // These get applied to dvx and dvy at the end of the boids update.
    BoidsUpdate boidsUpdate;
}    
