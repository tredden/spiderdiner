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
    Fly initialState;

    [SerializeField]
    float fliesPerSecond;

    [SerializeField]
    float spawnRegionSize;

    float mQueue = 0f;

    // bool hasEmitted = false;

    private void Start()
    {
        flyManager = FlyManager.GetInstance();
    }

    // TODO: should this be fixed update for reliability?
    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        mQueue += dt;

        while (mQueue >= 1f/fliesPerSecond /*&& !hasEmitted*/) {
            // hasEmitted = true;
            // TODO: get spawn position from noise and do this without making new Vectors
            float theta = Random.Range(0, Mathf.PI * 2f);
            float dist = Random.Range(0, spawnRegionSize);
            initialState.x = transform.position.x + dist * Mathf.Cos(theta);
            initialState.y = transform.position.y + dist * Mathf.Sin(theta);
            initialState.vx = initialVelocity.x * Random.Range(-0.1f, 0.1f);
            initialState.vy = initialVelocity.y;

            flyManager.SpawnFly(initialState);
            mQueue -= (1f / fliesPerSecond);
        }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRegionSize);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(initialVelocity.x, initialVelocity.y, 0f));
    }
}
