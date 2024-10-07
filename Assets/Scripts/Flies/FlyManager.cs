using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct BoidsRules {
    public float visualRange;
    public float protectedRange;
    public float centeringFactor;
    public float avoidFactor;
    public float matchingFactor;
    public float maxSpeed;
    public float minSpeed;
    // Factor controlling how quickly the fly's speed returns to minSpeed
    // 0 is no restitution. 1 is restitution in one second.
    public float minSpeedRestitution;
}

[RequireComponent(typeof(ParticleSystem))]
public class FlyManager : MonoBehaviour
{
    static FlyManager instance;
    public static FlyManager GetInstance()
    {
        return instance;
    }
   
    // Active flies
    const int MAX_FLIES = 10000;
    Fly[] flies;
    Grid grid;
    int lastSpawnedFly = 0;
    [SerializeField]
    int flyCount = 0;

    ParticleSystem flyRender;

    [SerializeField]
    BoidsRules boidsRules;

    [SerializeField]
    List<ObstacleBase> obstacles = new List<ObstacleBase>();

    void Awake()
    {
        instance = this;
        flyRender = this.GetComponent<ParticleSystem>();
        flies = new Fly[10000];
        for (int i = 0; i < MAX_FLIES; i++) {
            flies[i] = new Fly();
            flies[i].enabled = false;
        }
        this.grid = new Grid(300, 300, (int) this.boidsRules.visualRange);
    }

    // Update is called once per frame
    float dt;
    void Update()
    {
        dt = Time.deltaTime;
        // Update flies based on boids rules
        UpdateBoids(dt);
        UpdateObstacles(dt);
        UpdateFlyState(dt);
        UpdateVisuals(dt);
    }

    List<int> neighborIndices = new List<int>(MAX_FLIES);
    List<int> candidateNeighbors = new List<int>(MAX_FLIES);

    void UpdateBoids(float dt)
    {
        grid.clear();
        grid.fill(flies, flyCount);
        // Update flies based on boids rules
        for (int i = 0; i < flyCount; i++) {
			Fly fly = flies[i];
            if (!fly.enabled) {
                continue;
            }

            // Separation
            getNeighborIndices(i, boidsRules.protectedRange);
            float close_dx = 0;
            float close_dy = 0;
            foreach (int neighborIndex in neighborIndices) {
				Fly neighbor = flies[neighborIndex];
                close_dx += fly.x - neighbor.x;
                close_dy += fly.y - neighbor.y;
            }
            fly.dvx += close_dx * boidsRules.avoidFactor;
            fly.dvy += close_dy * boidsRules.avoidFactor;

            // Alignment
            getNeighborIndices(i, boidsRules.visualRange);
            int neighborCount = neighborIndices.Count;
            float xvel_avg = 0;
            float yvel_avg = 0;
            foreach (int neighborIndex in neighborIndices) {
				Fly neighbor = flies[neighborIndex];
                xvel_avg += neighbor.vx;
                yvel_avg += neighbor.vy;
            }
            if (neighborCount > 0) {
                xvel_avg /= neighborCount;
                yvel_avg /= neighborCount;
            }
            fly.dvx += (xvel_avg - fly.vx) * boidsRules.matchingFactor;
            fly.dvy += (yvel_avg - fly.vy) * boidsRules.matchingFactor;

            // Cohesion
            float xpos_avg = 0;
            float ypos_avg = 0;
            foreach (int neighborIndex in neighborIndices) {
                Fly neighbor = flies[neighborIndex];
                xpos_avg += neighbor.x;
                ypos_avg += neighbor.y;
            }
            if (neighborCount > 0) {
                xpos_avg /= neighborCount;
                ypos_avg /= neighborCount;
            }
            fly.dvx += (xpos_avg - fly.x) * boidsRules.centeringFactor;
            fly.dvy += (ypos_avg - fly.y) * boidsRules.centeringFactor;
            flies[i] = fly;
        }
    }

    void getNeighborIndices(int flyIndex, float radius) {
		// The grid must be set up before calling this.

        neighborIndices.Clear();
        var fly = flies[flyIndex];

        // TODO: This is horribly inefficient.
        this.grid.query(fly.x, fly.y, radius, candidateNeighbors);
        float radiusSquared = radius * radius;
        for (int i = 0; i < candidateNeighbors.Count; i++) {
            int otherFlyIndex = candidateNeighbors[i];
            var otherFly = flies[otherFlyIndex];
            if (!otherFly.enabled || otherFlyIndex == flyIndex) {
                continue;
            }
            float dx = otherFly.x - fly.x;
            float dy = otherFly.y - fly.y;
            if (dx * dx + dy * dy < radiusSquared) {
                neighborIndices.Add(otherFlyIndex);
            }
        }
    }

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
        for (int i = 0; i < flyCount; i++) {
            // TODO: is this making a copy or not?...
            Fly f = flies[i];
            if (f.enabled) {
                foreach (ObstacleBase obstacle in obstacles) {
                    if (!obstacle.isActiveAndEnabled) {
                        continue;
                    }
                    if (obstacle.GetDoesInteract(ref f, dt)) {
                        obstacle.InfluenceFly(ref f, dt);
                    }
                }
            }
            flies[i] = f;
        }
    }

    void UpdateFlyState(float dt)
    {
        for (int i = 0; i < flyCount; i++) {
            Fly fly = flies[i];
            // check if the fly is disabled this frame
            if (!fly.enabled) {
                continue;
            }
            if (fly.disable) {
                fly.enabled = false;
                fly.disable = false;
                flyCount--;
                if (flyCount > 0) {
                    flies[i] = flies[flyCount];
                    flies[flyCount].disable = false;
                    flies[flyCount].enabled = false;
                }
                continue;
            }
            
            // Apply dvx, dvy
            fly.vx += fly.dvx * dt;
            fly.vy += fly.dvy * dt;

            // Reset dvx, dvy
            fly.dvx = 0;
            fly.dvy = 0;
            
            // Clamp fly velocity
            float speed = Mathf.Sqrt(fly.vx * fly.vx + fly.vy * fly.vy);
            if (speed > boidsRules.maxSpeed) {
                fly.vx = fly.vx / speed * boidsRules.maxSpeed;
                fly.vy = fly.vy / speed * boidsRules.maxSpeed;
            }
            if (speed < boidsRules.minSpeed) {
                // Apply restitution
                // (1 - restitution) / fly_velocity + restitution * min_velocity
                float min_vx = fly.vx / speed * boidsRules.minSpeed;
                float min_vy = fly.vy / speed * boidsRules.minSpeed;
                float restitution = boidsRules.minSpeedRestitution * dt;
                fly.vx = (1 - restitution) * fly.vx + restitution * min_vx;
                fly.vy = (1 - restitution) * fly.vy + restitution * min_vy;
            }

            fly.x += fly.vx * dt;
            fly.y += fly.vy * dt;

            flies[i] = fly;
        }
    }

    public bool SpawnFly(Fly details)
    {
		if (flyCount >= MAX_FLIES) {
            Debug.LogError("No more space for flies!");
            return false;
        }
        details.enabled = true;
        flies[flyCount] = details;
        
        posVec.x = details.x;
        posVec.y = details.y;
        posVec.z = -details.y;
        velVec.x = details.vx;
        velVec.y = details.vy;
        velVec.z = 0f;
        flyRender.Emit(posVec, velVec, 1f, 10f, details.getUnityColor());
        flyCount++;
        return true;
    }
    Vector3 posVec = Vector3.zero;
    Vector3 velVec = Vector3.zero;
    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[10000];
    const float maxSpiceLevel = 3f;
    void UpdateVisuals(float dt)
    {
        posVec = Vector3.zero;
        velVec = Vector3.zero;

        int particleCount = flyRender.GetParticles(particles, flyRender.particleCount);
        if (particleCount < flyCount) {
            int delta = flyCount - particleCount;
            flyRender.Emit(delta);
            particleCount = flyRender.GetParticles(particles, delta, particleCount);
        }

        for(int i = 0; i < flyCount; i++) {
            Fly fly = flies[i];
            posVec.x = fly.x;
            posVec.y = fly.y;
            posVec.z = -fly.y;
            velVec.x = fly.vx;
            velVec.y = fly.vy;
            velVec.z = 0;
            particles[i].position = posVec;
            particles[i].velocity = velVec;
            //particles[i].startColor = (Color.red * (float)fly.spiceLevel + Color.white * (maxSpiceLevel - (float)fly.spiceLevel)) / maxSpiceLevel;
            particles[i].startColor = fly.getUnityColor();
        }

        for (int i = flyCount; i < particleCount; i++) {
            particles[i].remainingLifetime = 0f;
        }
        flyRender.SetParticles(particles, flyCount);
    }
}
