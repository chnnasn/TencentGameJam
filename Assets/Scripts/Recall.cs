using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerTrail;
public enum TimeState
{
    Before,
    StartRecall,
    Recall,
    After,
}

public class Recall : MonoBehaviour
{
    public TimeState timeState;
    public Movement Player;
    private LineRenderer lineRenderer;
    List<Vector3> points = new List<Vector3>();
    public static Recall instance;
    bool CanGetPo = false;


    private void Awake()
    {
        if (instance != null) 
        {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timeState = TimeState.Before;
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // 初始化点的数量
        }

        StartCoroutine(WaitForGrounded()) ;

    }

    IEnumerator WaitForGrounded()
    {
        // 等待直到Player.jump状态变为JumpState.Grounded
        while (Player.jump != JumpState.Grounded)
        {
            yield return null; // 每帧检查一次
        }

        // 当jump状态变为Grounded时，触发相应行为
        UpdateLineRenderer();
        CanGetPo = true;
        yield break;
    }

    bool MakingEnemy = false;

    // Update is called once per frame
    void Update()
    {
        if (timeState == TimeState.StartRecall)
        {
            timeState = TimeState.Recall;

            UpdateLineRenderer();
            // 如果正在倒着移动，则调用倒退轨迹的方法
            StartReversing();
        }
        // 检查物体是否在运动
        if (timeState == TimeState.Before && CanGetPo && Player.jump != JumpState.Grounded)
        {
            UpdateLineRenderer();
        }

        if (timeState == TimeState.After && !MakingEnemy) {
            StartCoroutine(MakeEnemy());
        }
    }

    IEnumerator MakeEnemy() {

        MakingEnemy = true;
        while (true) {
            Debug.LogError("出现一个敌人");
            isenemycome = true;
            yield return new WaitForSeconds(2f);
        }
    
    }




    void UpdateLineRenderer()
    {
        if (lineRenderer == null)
            return;

        // 将当前物体的位置添加到轨迹点列表中
        Vector3 currentPosition = Player.transform.position;

        // 检查是否需要添加新点（避免连续多个相同位置的点）
        if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], currentPosition) > 0.1f)
        {
            points.Add(currentPosition);
            lineRenderer.positionCount = points.Count; // 更新点数量
            lineRenderer.SetPosition(points.Count - 1, currentPosition); // 设置最新的点
        }
    }

    public void StartReversing()
    {
        if (points.Count > 0)
        {
            StartCoroutine(ReverseMovement(points.Count - 1));
        }
    }

    IEnumerator ReverseMovement(int currentPointIndex)
    {
        Time.timeScale = 2;
        while (currentPointIndex >= 0)
        {
            // 移动到轨迹的前一个点
            Player.transform.position = Vector3.MoveTowards(Player.transform.position, points[currentPointIndex], Player.MoveSpeed * Time.deltaTime);

            // 如果到达了该点，继续倒退到下一个点
            if (Vector3.Distance(Player.transform.position, points[currentPointIndex]) < 0.01f)
            {
                currentPointIndex --;
            }

            // 等待下一帧
            yield return null;
        }

        timeState = TimeState.After ;
        Time.timeScale = 1;
    }

}
