using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderD : MonoBehaviour
{
    public Vector3 targetPos;
    public Vector3 currentPos;

    [SerializeField]
    NavMeshAgent agent;

    [SerializeField]
    Animator animator;
    [SerializeField]
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        this.currentPos = transform.position;
        this.targetPos = transform.position;
        NavManager.GetInstance().RegisterAgent(this);
    }

    public void SetTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        targetPos.z = agent.transform.position.z;
        if (agent != null) {
            agent.SetDestination(targetPos);
        }
    }

    public void SetAgent(NavMeshAgent agent)
    {
        this.agent = agent;
        Vector3 targetPos = this.targetPos;
        targetPos.z = agent.transform.position.z;
        agent.SetDestination(targetPos);
    }
    public void ClearAgent()
    {
        this.agent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent != null) {
            currentPos = agent.transform.position;
            currentPos.z = transform.position.z;
            transform.position = currentPos;

            bool isRunning = agent.desiredVelocity.magnitude > 0.1f;
            bool faceLeft = agent.desiredVelocity.x < -.1f;
            bool faceRight = agent.desiredVelocity.x > .1f;
            if (faceLeft && !spriteRenderer.flipX) {
                spriteRenderer.flipX = true;
            } else if (faceRight && spriteRenderer.flipX) {
                spriteRenderer.flipX = false;
            }
            // bool isRunning = moveVec.magnitude > stopThreshold;

            animator.SetBool("isRunning", isRunning);
        }
    }
}
