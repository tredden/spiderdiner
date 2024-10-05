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
    static FlyManager instance;
    public static FlyManager GetInstance()
    {
        return instance;
    }

    List<ObstacleBase> obstacles = new List<ObstacleBase>();

    public void RegisterObstacle(ObstacleBase ob)
    {
        if (obstacles.Contains(ob)) {
            return;
        }
        int index = 0;
        int actOrder = ob.GetActOrder();
        for (; index < obstacles.Count; index++) {
            if (actOrder <= obstacles[index].GetActOrder()) {
                break;
            }
        }
        obstacles.Insert(index, ob);
    }

    public void DeregisterObstacle(ObstacleBase ob)
    {
        if (!obstacles.Contains(ob)) {
            return;
        }
        obstacles.Remove(ob);
    }

    void UpdateObstacles(float dt)
    {
        for (int i = 0; i < flies.Length; i++) {
            // TODO: is this making a copy or not?...
            Fly f = flies[i];
            if (f.enabled) {
                foreach (ObstacleBase obstacle in obstacles) {
                    if (obstacle.GetDoesInteract(ref f, dt)) {
                        obstacle.InfluenceFly(ref f, dt);
                    }
                }
            }
            flies[i] = f;
        }
    }

    // Active flies
    const int MAX_FLIES = 10000;
    Fly[] flies = new Fly[10000];

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
