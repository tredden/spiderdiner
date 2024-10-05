using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyColor { UNSET = 0, BLUE = 1, }

public struct BoidsUpdate {
    // Separation
    public float close_dx;
    public float close_dy;

    // Alignment
    public float xvel_avg;
    public float yvel_avg;
    public int neighboring_boids;

    // Cohesion
    public float xpos_avg;
    public float ypos_avg;
}

public struct FlyData {
    public float x;
    public float y;
    public float vx;
    public float vy;
    public FlyColor color;
    int spiceLevel;
}

[System.Serializable]
public struct Fly {
    // One of these is the current frame
    // The other is the previous frame.
	FlyData[] data = new FlyData[2];



	public Fly(ref FlyData data) {
        this.data[0] = data;
        this.data[1] = data;
    }



}    
