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
    [HideInInspector]
    public TimeState timeState;
    public Movement Player;
    private LineRenderer lineRenderer;
    [HideInInspector]
    public List<Vector3> points = new List<Vector3>();
    public static Recall instance;
    public GameObject Enemy;
    Transform target; // 目标物体
    public Queue<GameObject> queue = new Queue<GameObject>();

    [HideInInspector]
    public GameObject BG1;
    [HideInInspector]
    public ShakeCamera shakeCamera;
    [HideInInspector]
    public Transform Mask;

    new AudioSource audio;

    GameObject hhh;

    private void OnEnable()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        target = GameObject.FindGameObjectWithTag("End").transform;
        timeState = TimeState.Before;
        lineRenderer = GetComponent<LineRenderer>();
        points.Clear();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // 初始化点的数量
        }

        audio = GetComponent<AudioSource>();

        hhh = GameObject.Find("HUISU");

        hhh.SetActive(false);
    }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
       

    }

    //bool MakingEnemy = false;

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
        if (timeState == TimeState.Before && Player.CanMove)
        {
            UpdateLineRenderer();
        }

        if (timeState == TimeState.After && queue.Count >0)
        {
            GameObject Enemy = queue.Dequeue();

            StartCoroutine(MakeEnemy(Enemy));
        }
    }

    IEnumerator MakeEnemy(GameObject Enemy )
    {
        yield return new WaitForSeconds(0.5f);

        Enemy.SetActive(true);
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


    //正在回溯
    IEnumerator ReverseMovement(int currentPointIndex)
    {
        audio.Play();

        BG1.GetComponent<Animator>().Play("1(1)");
        Time.timeScale = 2;
        float h = ((int)CalculateLineRendererLength() / 3);
        float[] distances = { 0, 2 * h}; // 需要生成敌人的距离值
        int enemyCount = 0; // 计数器，用于跟踪生成的敌人数量

        hhh.SetActive(true);

        while (currentPointIndex >= 0)
        {
            if (!shakeCamera.enabled) {
                shakeCamera.enabled = true;
            }

            // 移动到轨迹的前一个点
            Player.transform.position = Vector3.MoveTowards(Player.transform.position, points[currentPointIndex], Player.MoveSpeed * Time.deltaTime);

            float distanceToTarget = Vector3.Distance(Player.transform.position, target.position);

            if (enemyCount < distances.Length) {

                // 检查是否在特定距离生成敌人
                if (distanceToTarget> distances[enemyCount])
                {
                    Instantiate(Enemy, points[currentPointIndex], Quaternion.identity);
                    enemyCount++; // 生成敌人后，增加计数器
                }
            }

            // 如果到达了该点，继续倒退到下一个点
            if (Vector3.Distance(Player.transform.position, points[currentPointIndex]) < 0.01f)
            {
                currentPointIndex--;
            }

            // 等待下一帧
            yield return null;
        }


        hhh.SetActive(false);

        Vector3 StartScale = Mask.localScale;
        Vector3 EndScale = new Vector3(44, 44, 44);
        float elapsedTime = 0f;
        float duration = 3f; // 过渡时间

        // 使用while循环逐渐增加Vignette强度和Mask的缩放比例
        while (elapsedTime <= duration)
        {
            // 计算插值因子
            float t = elapsedTime / duration;

            // 线性插值调整Mask的缩放比例
            Mask.localScale = Vector3.Lerp(StartScale, EndScale, t);

            // 增加经过的时间
            elapsedTime += Time.deltaTime;

            // 等待下一帧
            yield return null;
        }

        Mask.localScale = EndScale;


        BG1.SetActive(false);
        if (shakeCamera.enabled)
        {
            shakeCamera.enabled = false;
        }
        Time.timeScale = 1;
        timeState = TimeState.After;

    }

    float CalculateLineRendererLength()
    {
        Vector3 h = points[0] - points[points.Count - 1];
        h.y = 0;
        float length = h.magnitude;
        return length;
    }

}
