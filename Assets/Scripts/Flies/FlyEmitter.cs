using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEmitter : MonoBehaviour
{
    [SerializeField]
    Vector2 initialVelocity;

    [SerializeField]
    FlyManager flyManager;

    [SerializeField]
    FlyDetails initialState;

    [SerializeField]
    float fliesPerSecond;

    [SerializeField]
    float spawnRegionSize;

    float mQueue = 0f;

    // TODO: should this be fixed update for reliability?
    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        mQueue += dt;

        while (mQueue >= 1f/fliesPerSecond) {
            // TODO: get spawn position from noise and do this without making new Vectors
            flyManager.SpawnFly(new Vector2(transform.position.x, transform.position.z), initialVelocity, initialState);
            mQueue -= (1f / fliesPerSecond);
        }
    }
}
