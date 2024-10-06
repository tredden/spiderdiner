using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderD : MonoBehaviour
{
    public Vector3 targetPos;
    public Vector3 currentPos;
    public Vector3 moveVec;

    [SerializeField]
    Animator animator;

    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    float stopThreshold = .1f;

    public void SetTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        currentPos = transform.position;

        moveVec = targetPos - currentPos;
        moveVec.z = 0;
        if (moveVec.magnitude > moveSpeed * dt) {
            moveVec = moveVec.normalized * moveSpeed * dt;
        }

        animator.SetBool("isRunning", moveVec.magnitude > stopThreshold);

        transform.position = currentPos + moveVec;
    }
}
