using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    const int MAX_FLIES = 10000;
    Fly[] activeFlies = new Fly[10000];

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

    public void SpawnFly(Fly details)
    {
		// TODO

    }
}
