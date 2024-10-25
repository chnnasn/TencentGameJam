using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpState
{
    Grounded,
    Falling,
    Jumping
}

public class CheckPhysics : MonoBehaviour
{
    protected Vector3 Gravity;
    public JumpState jump;
    public float jumpHeight;
    public float GravityWeight;
    public LayerMask GroundedLayer;

    // Start is called before the first frame update
    public void Start()
    {
        // 初始化代码（如果有的话）
    }

    // Update is called once per frame
    void Update()
    {
        UseGravity();
    }

    public void UseGravity()
    {
        // 检测是否在地面上
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector3.down, 0.01f + (float)(transform.localScale.y * 0.38), GroundedLayer);
        if (groundHit.collider != null)
        {
            Gravity.y = 0; // 重置垂直速度
            jump = JumpState.Grounded;

            // 获取射线起点到碰撞点之间的距离
            float distanceToGround = groundHit.distance;

            if (distanceToGround < transform.localScale.y * 0.38f)
            {
                float offset = (transform.localScale.y * 0.38f) - distanceToGround;
                transform.position += new Vector3(0, offset, 0);
            }
        }
        else if (jump != JumpState.Jumping)
        {
            jump = JumpState.Falling;
        }

        Debug.DrawRay(transform.position, Vector3.down * (0.01f + (float)(transform.localScale.y * 0.38)), Color.blue);

        // 左右各偏转从0到30度，间隔为5度的射线
        bool grounded = false; // 用于标记是否检测到地面

        for (int angle = 0; angle <= 30; angle += 5)
        {
            // 计算当前角度下的射线长度
            float rayLength = 0.01f + (float)(transform.localScale.y * 0.38) + (angle / 5) * 0.01f;

            // 左偏转射线
            Vector3 leftRayDirection = Quaternion.Euler(0, 0, angle) * Vector3.down;
            RaycastHit2D leftGroundHit = Physics2D.Raycast(transform.position, leftRayDirection, rayLength, GroundedLayer);
            Debug.DrawRay(transform.position, leftRayDirection * rayLength, Color.red);

            // 右偏转射线
            Vector3 rightRayDirection = Quaternion.Euler(0, 0, -angle) * Vector3.down;
            RaycastHit2D rightGroundHit = Physics2D.Raycast(transform.position, rightRayDirection, rayLength, GroundedLayer);
            Debug.DrawRay(transform.position, rightRayDirection * rayLength, Color.green);

            // 检测左右偏转射线是否碰到地面
            if (leftGroundHit.collider != null || rightGroundHit.collider != null)
            {
                Gravity.y = 0; // 重置垂直速度
                jump = JumpState.Grounded;
                grounded = true;
                break; // 只需要一个射线检测到地面即可
            }
        }

        // 如果没有任何射线检测到地面，则继续应用重力
        if (!grounded && jump != JumpState.Grounded)
        {
            Gravity.y += (GravityWeight * Physics2D.gravity.y) * Time.deltaTime;
        }

        // 移动敌人
        transform.Translate(Gravity * Time.deltaTime);
    }

    public virtual void PlayerJump()
    {
        // 跳跃
        jump = JumpState.Jumping;
        Gravity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(GravityWeight * Physics2D.gravity.y));
        transform.Translate(Gravity * Time.deltaTime);
    }
}
