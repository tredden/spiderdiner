using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : CircleInfluencer
{
    enum TongueStatus
    {
        MOUTH_CLOSED,
        MOUTH_OPEN_EARLY,
        TONGUE_EXTENDING,
        TONGUE_EXTENDED,
        TONGUE_RETRACTING,
        MOUTH_OPEN_LATE
    }

    [SerializeField]
    Vector2 pointA;
    Vector2 pointB;
    Vector2 pointBTarget;
    float tongueMoveTime;
    [SerializeField]
    List<Vector2> tongueTargets = new List<Vector2>();
    int activeTongue = -1;

    [SerializeField]
    FrogTongue tongue;
    [SerializeField]
    Animator animator;

    [SerializeField]
    float earlyOpenTime;
    [SerializeField]
    float minTongueTime;
    [SerializeField]
    float maxTongueTime;
    [SerializeField]
    float lateOpenTime;
    [SerializeField]
    float minClosedTime;
    [SerializeField]
    float maxClosedTime;

    [SerializeField]
    float tongueLeaveSpeed;
    [SerializeField]
    float tongueReturnSpeed;

    float timeRemaining;

    TongueStatus tongueStatus;

    public override void InfluenceFly(ref Fly fly, float dt)
    {
        fly.disable = true;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator.SetBool("isOpen", false);
        tongue.gameObject.SetActive(false);
        timeRemaining = maxClosedTime;
    }

    // Update is called once per frame
    protected void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (tongueStatus == TongueStatus.TONGUE_EXTENDING) {
            pointB = pointA * timeRemaining / tongueMoveTime + pointBTarget * (1f - timeRemaining / tongueMoveTime);
            tongue.SetPointB(pointB.x, pointB.y);
        } else if (tongueStatus == TongueStatus.TONGUE_RETRACTING) {
            pointB = pointBTarget * timeRemaining / tongueMoveTime + pointA * (1f - timeRemaining / tongueMoveTime);
            tongue.SetPointB(pointB.x, pointB.y);
        }
        if (timeRemaining <= 0f) {
            switch (tongueStatus) { // tick to the next state
                case TongueStatus.MOUTH_CLOSED:
                    animator.SetBool("isOpen", true);
                    timeRemaining = earlyOpenTime;
                    tongueStatus++;
                    break;
                case TongueStatus.MOUTH_OPEN_EARLY:
                    int activeTongue = Random.Range(0, tongueTargets.Count);
                    pointBTarget = tongueTargets[activeTongue];
                    pointB = pointA;
                    tongue.SetPointA(pointA.x, pointA.y);
                    tongue.SetPointB(pointB.x, pointB.y);
                    tongue.gameObject.SetActive(true);
                    tongueMoveTime = (pointBTarget - pointA).magnitude / tongueLeaveSpeed;
                    timeRemaining = tongueMoveTime;
                    tongueStatus++;
                    break;
                case TongueStatus.TONGUE_EXTENDING:
                    timeRemaining = Random.Range(minTongueTime, maxTongueTime);
                    tongueStatus++;
                    break;
                case TongueStatus.TONGUE_EXTENDED:
                    tongueMoveTime = (pointBTarget - pointA).magnitude / tongueReturnSpeed;
                    timeRemaining = tongueMoveTime;
                    tongueStatus++;
                    break;
                case TongueStatus.TONGUE_RETRACTING:
                    tongue.gameObject.SetActive(false);
                    timeRemaining = lateOpenTime;
                    tongueStatus++;
                    break;
                case TongueStatus.MOUTH_OPEN_LATE:
                    timeRemaining = Random.Range(minClosedTime, maxClosedTime);
                    animator.SetBool("isOpen", false);
                    tongueStatus = TongueStatus.MOUTH_CLOSED;
                    break;
            }
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.magenta;
        Vector3 pointA = Vector3.zero;
        Vector3 pointB = Vector3.zero;
        pointA.x = this.pointA.x;
        pointA.y = this.pointA.y;
        Gizmos.DrawWireSphere(pointA, 1.5f);
        foreach (Vector2 target in tongueTargets) {
            pointB.x = target.x;
            pointB.y = target.y;
            Gizmos.DrawWireSphere(pointB, 1.5f);
            Gizmos.DrawLine(pointA, pointB);
        }
    }
}
