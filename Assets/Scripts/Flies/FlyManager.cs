using System.Collections;
using System.Collections.Generic;
using UnityEngine;

const int MAX_FLIES = 10000;

[System.Serializable]
struct BoidsRules {
    public float turnfactor;
    public float visualRange;
    public float protectedRange;
    public float centeringfactor;
    public float avoidfactor;
    public float matchingfactor;
    public float maxspeed;
    public float minspeed;
}

public class FlyManager : MonoBehaviour
{
	// Active flies
    Fly[] activeFlies = new Fly[MAX_FLIES];

    BoidsRules boidsRules;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Update flies based on boids rules


        
    }

    public void SpawnFly(Vector2 position, Vector2 velocity)
    {
        Fly fly = new Fly(new FlyData {
            pos = position,
            vel = velocity,
            color = Color.BLUE,
            spiceLevel = 0
        });
    }
}
