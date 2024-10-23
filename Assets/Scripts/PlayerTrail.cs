using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    public List<Vector3> trailPoints = new List<Vector3>(); // 玩家轨迹的列表
    public float trailInterval = 1f; // 记录轨迹的时间间隔
    public GameObject enemy;//获取敌人对象
    public static bool isenemycome;//敌人是非出现
    void Start()
    {
        isenemycome=false;
        InvokeRepeating("RecordPosition", 0f, trailInterval);
        enemy.SetActive(false);
    }
    private void Update()
    {
        if(isenemycome)
        {
            enemy.SetActive(true);
        }
    }
    void RecordPosition()
    {
        trailPoints.Add(transform.position);
    }
}