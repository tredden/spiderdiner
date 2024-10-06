using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavManager : MonoBehaviour
{
    static NavManager instance;

    [SerializeField]
    NavMeshAgent agentPrefab;

    Dictionary<SpiderD, NavMeshAgent> agents = new Dictionary<SpiderD, NavMeshAgent>();
    Dictionary<ObstacleBase, NavMeshObstacle> obstacles = new Dictionary<ObstacleBase, NavMeshObstacle>();

    public static NavManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    public void RegisterAgent(SpiderD spiderD)
    {
        Vector3 pos = spiderD.transform.position;
        pos.z = this.transform.position.z;
        NavMeshAgent agent = GameObject.Instantiate<NavMeshAgent>(agentPrefab, pos, this.transform.rotation, this.transform);
        spiderD.SetAgent(agent);
        if (agents.ContainsKey(spiderD)) {
            GameObject.Destroy(agents[spiderD].gameObject);
        }
        agents[spiderD] = agent;
    }

    public void DeregisterAgent(SpiderD spiderD)
    {
        if (agents.ContainsKey(spiderD)) {
            GameObject.Destroy(agents[spiderD].gameObject);
            agents.Remove(spiderD);
        }
        spiderD.ClearAgent();
    }

    public void RegisterObstacle(ObstacleBase obstacle, NavMeshObstacle navObstacle)
    {
        Vector3 pos = navObstacle.transform.position;
        pos.z = this.transform.position.z;
        navObstacle.transform.position = pos;
        navObstacle.transform.rotation = this.transform.rotation;
        navObstacle.transform.parent = this.transform;
        obstacles[obstacle] = navObstacle;
    }

    public void DeregisterObstacle(ObstacleBase obstacle)
    {
        if (obstacles.ContainsKey(obstacle)) {
            obstacles.Remove(obstacle);
        }
    }
}
