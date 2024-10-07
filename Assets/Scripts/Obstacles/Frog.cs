using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : CircleInfluencer
{
    List<FrogTongue> frogTongues = new List<FrogTongue>();
    int activeTongue = -1;

    [SerializeField]
    float minTongueTime;
    [SerializeField]
    float maxTongueTime;
    [SerializeField]
    float minGloatTime;
    [SerializeField]
    float maxGloatTime;

    float timeRemaining;

    public override void InfluenceFly(ref Fly fly, float dt)
    {
        fly.disable = true;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        foreach (FrogTongue ft in GetComponentsInChildren<FrogTongue>()) {
            frogTongues.Add(ft);
            ft.gameObject.SetActive(false);
        }
        timeRemaining = maxGloatTime;
    }

    // Update is called once per frame
    protected void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f) {
            if (activeTongue == -1) {
                StartTongue();
            } else {
                EndTongue();
            }
        }
    }

    void StartTongue()
    {
        activeTongue = Random.Range(0, frogTongues.Count);
        frogTongues[activeTongue].gameObject.SetActive(true);
        // TODO: set anim data

        timeRemaining = Random.Range(minTongueTime, maxTongueTime);
    }

    void EndTongue()
    {
        frogTongues[activeTongue].gameObject.SetActive(false);
        activeTongue = -1;
        // TODO: set anim data

        timeRemaining = Random.Range(minGloatTime, maxGloatTime);
    }
}
