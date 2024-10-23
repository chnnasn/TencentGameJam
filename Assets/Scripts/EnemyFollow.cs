using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerTrail;
public class EnemyFollow : MonoBehaviour
{
    List<Vector3> trailPoints; // 玩家的轨迹
    public float moveSpeed = 5f; // 敌人的移动速度
    //private int pointIndex = 0; // 当前要移动到的轨迹点的索引
    //public Transform player;
    SpriteRenderer sprite;
    Animator animator;
    void Start()
    {
        //trailPoints = player.GetComponent<PlayerTrail>().trailPoints;
    }


    IEnumerator WaiTForAfter() {
        while (true) {

            if (Recall.instance.timeState == TimeState.After) {
                StartCoroutine(FollowTrail(GetClosestPointIndex(transform.position)));
                break;
            }
            else {
                yield return null;
                continue;
            }

        }
    
    }

    int GetClosestPointIndex(Vector3 Position)
    {
        int closestIndex = 0;
        float closestDistance = Vector3.Distance(Position, trailPoints[0]);

        for (int i = 1; i < trailPoints.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, trailPoints[i]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    IEnumerator FollowTrail(int pointIndex)
    {
        while (true)
        {
            if (pointIndex >= 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, trailPoints[pointIndex], moveSpeed * Time.deltaTime);

                if (trailPoints[pointIndex].y > transform.position.x)
                {
                    animator.Play("Jump");
                }

                if (!IsAnimationPlaying("Jump")) {
                    animator.Play("Walk");
                }



                sprite.flipX = trailPoints[pointIndex].x < transform.position.x;

                if (Vector3.Distance(transform.position, trailPoints[pointIndex]) < 0.1f)
                {
                    pointIndex -- ; // 移动到下一个点
                }
            }
            else
            {
                gameObject.gameObject.SetActive(false);
                Recall.instance. queue.Enqueue(gameObject);
                transform.position = trailPoints[trailPoints.Count - 1];
                //moveSpeed=-moveSpeed;
            }
            yield return null;

        }
    }

    bool IsAnimationPlaying(string name)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(name) && stateInfo.normalizedTime < 1.0f;
    }
    private void OnEnable()
    {
        trailPoints = Recall.instance.points;
        StartCoroutine(WaiTForAfter());
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
}