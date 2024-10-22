using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerTrail;
public class EnemyFollow : MonoBehaviour
{
    public List<Vector3> trailPoints; // 玩家的轨迹
    public float moveSpeed = 5f; // 敌人的移动速度
    private int pointIndex = 0; // 当前要移动到的轨迹点的索引
    public Transform player;
    void Start()
    {
        trailPoints=player.GetComponent<PlayerTrail>().trailPoints;
        StartCoroutine(FollowTrail());
    }
   
    IEnumerator FollowTrail()
    {
        while (true)
        {
            
            if (pointIndex < trailPoints.Count)
            {
                transform.position = Vector3.MoveTowards(transform.position, trailPoints[pointIndex], moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, trailPoints[pointIndex]) < 0.1f)
                {
                    pointIndex++; // 移动到下一个点
                }
            }
            else
            {
                Destroy(gameObject);
                //moveSpeed=-moveSpeed;
            }
            yield return null;
          
        }
    }
}