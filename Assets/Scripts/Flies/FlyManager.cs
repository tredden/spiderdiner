using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct BoidsRules {
    public float turnFactor;
    public float visualRange;
    public float protectedRange;
    public float centeringFactor;
    public float avoidFactor;
    public float matchingFactor;
    public float maxSpeed;
    public float minSpeed;
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
        float dt = Time.deltaTime;
        // Update flies based on boids rules
        UpdateBoids(dt);

        for (int i = 0; i < flies.Length; i++) {
            Fly fly = flies[i];
            if (!fly.active) continue;
            // Apply dvx, dvy
            fly.vx += fly.dvx * dt;
            fly.vy += fly.dvy * dt;

			// Clamp fly velocity
			float speed = Mathf.Sqrt(fly.vx * fly.vx + fly.vy * fly.vy);
			if (speed > boidsRules.maxSpeed) {
				fly.vx = fly.vx / speed * boidsRules.maxSpeed;
				fly.vy = fly.vy / speed * boidsRules.maxSpeed;
			}
			if (speed < boidsRules.minSpeed) {
				fly.vx = fly.vx / speed * boidsRules.minSpeed;
				fly.vy = fly.vy / speed * boidsRules.minSpeed;
			}
        }
    }

    void UpdateBoids(float dt)
    {
        // Update flies based on boids rules
        for (int i = 0; i < flies.Length; i++) {
			Fly fly = flies[i];
            if (!fly.active) continue;

            // Separation
            List<int> protectedNeighbors = getNeighborIndices(i, boidsRules.protectedRange);
            float close_dx = 0;
            float close_dy = 0;
            foreach (int neighborIndex in protectedNeighbors) {
				Fly neighbor = flies[neighborIndex];
                close_dx += fly.x - neighbor.x;
                close_dy += fly.y - neighbor.y;
            }
            fly.dvx += close_dx * boidsRules.avoidFactor;
            fly.dvy += close_dy * boidsRules.avoidFactor;

            // Alignment
            List<int> neighbors = getNeighborIndices(i, boidsRules.visualRange);
            float xvel_avg = 0;
            float yvel_avg = 0;
            foreach (int neighborIndex in neighbors) {
				Fly neighbor = flies[neighborIndex];
                xvel_avg += neighbor.vx;
                yvel_avg += neighbor.vy;
            }
            if (neighbors.Count > 0) {
                xvel_avg /= neighbors.Count;
                yvel_avg /= neighbors.Count;
            }
            fly.dvx += (xvel_avg - fly.vx) * boidsRules.matchingFactor;
            fly.dvy += (yvel_avg - fly.vy) * boidsRules.matchingFactor;

            // Cohesion
            float xpos_avg = 0;
            float ypos_avg = 0;
            foreach (int neighborIndex in neighbors) {
                Fly neighbor = flies[neighborIndex];
                xpos_avg += neighbor.x;
                ypos_avg += neighbor.y;
            }
            if (neighbors.Count > 0) {
                xpos_avg /= neighbors.Count;
                ypos_avg /= neighbors.Count;
            }
            fly.dvx += (xpos_avg - fly.x) * boidsRules.centeringFactor;
            fly.dvy += (ypos_avg - fly.y) * boidsRules.centeringFactor;
        }
    }

    List<int> getNeighborIndices(int flyIndex, float radiusSquared) {
		List<int> neighbors = new List<int>();
		var fly = flies[flyIndex];

        // TODO: This is horribly inefficient.
        for (int i = 0; i < flies.Length; i++) {
            var otherFly = flies[i];
            if (!otherFly.active || i == flyIndex) {
                continue;
            }
            float dx = otherFly.x - fly.x;
            float dy = otherFly.y - fly.y;
            if (dx * dx + dy * dy < radiusSquared) {
				neighbors.Add(i);
            }
        }
        return neighbors;
    }

    public void SpawnFly(Fly details)
    {
		// TODO

    }
}
