using System.Collections;
using System.Collections.Generic;
using UnityEngine;

const int MAX_FLIES = 10000;

public class FlyManager : MonoBehaviour
{
	// Active flies
    Fly[] activeFlies = new Fly[MAX_FLIES];
    // Inactive flies
    Fly[] inactiveFlies = new Fly[MAX_FLIES];

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
