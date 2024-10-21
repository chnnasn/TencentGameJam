using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Before,
    After,
}

public class Recall : MonoBehaviour
{
    public static Recall instance;
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private Vector3 previousPosition;
    private bool isReversing = false; // 标记是否正在倒着移动
    private int currentPointIndex;    // 用于倒着移动时的索引

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // 初始化点的数量
        }

        // 初始化前一个位置为当前物体的位置
        previousPosition = transform.position;
    }

    Vector2 MoveMent;

    // Update is called once per frame
    void Update()
    {
        if (isReversing)
        {
            // 如果正在倒着移动，则调用倒退轨迹的方法
            ReverseMovement();
        }
        else
        {
            // 正常获取输入并移动物体
            MoveMent.x = Input.GetAxis("Horizontal");
            MoveMent.y = Input.GetAxis("Vertical");
            transform.Translate(MoveMent * 5 * Time.deltaTime);

            // 检查物体是否在运动
            if (IsMoving() && !isReversing)
            {
                UpdateLineRenderer();
            }

            //// 按下空格键后，开始倒着移动
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    StartReversing();
            //}
        }
    }

    public float distance;
    bool IsMoving()
    {
        // 计算当前位置与上一次位置的距离
        float distanceMoved = Vector3.Distance(transform.position, previousPosition);

        // 如果移动距离大于阈值（例如0.01），则视为在运动
        if (distanceMoved > distance)
        {
            // 更新之前的位置为当前位置
            previousPosition = transform.position;
            return true;
        }

        return false;
    }

    void UpdateLineRenderer()
    {
        if (lineRenderer == null)
            return;

        // 将当前物体的位置添加到轨迹点列表中
        Vector3 currentPosition = transform.position;

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
            isReversing = true;
            currentPointIndex = points.Count - 1; // 从最后一个点开始倒退
        }
    }

    void ReverseMovement()
    {
        // 如果还有点需要回退
        if (currentPointIndex >= 0)
        {
            // 移动到轨迹的前一个点
            transform.position = Vector3.MoveTowards(transform.position, points[currentPointIndex], 5 * Time.deltaTime);

            // 如果到达了该点，继续倒退到下一个点
            if (Vector3.Distance(transform.position, points[currentPointIndex]) < 0.01f)
            {
                currentPointIndex--;
            }
        }
        else
        {
            // 当所有点都回退完成后，停止倒退
            isReversing = false;
            points.Clear();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("End")) {
            StartReversing();
        }
    }
}
