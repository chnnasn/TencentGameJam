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
    Vector3 Gravity;
    public JumpState jump;
    public float jumpHeight;
    // Start is called before the first frame update
   public void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public LayerMask GroundedLayer;

    public void UseGravity() {

        // 检测是否在地面上
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector3.down, 0.01f + (float)(transform.localScale.y * 0.5), GroundedLayer);
        if (groundHit.collider != null)
        {
            Gravity.y = 0; // 重置垂直速度
            jump = JumpState.Grounded;

            // 获取射线起点到碰撞点之间的距离
            float distanceToGround = groundHit.distance;

            if (distanceToGround < transform.localScale.y * 0.5)
            {
                float offset = (transform.localScale.y * 0.5f) - distanceToGround;
                transform.position += new Vector3(0, offset, 0);
            }

        }
        else if (jump != JumpState.Jumping)
        {
            jump = JumpState.Falling;
        }

            Debug.DrawRay(transform.position, Vector3.down * (0.01f + (float)(transform.localScale.y * 0.5)), Color.blue);


        // 应用重力
        if (jump != JumpState.Grounded)
        {
            Gravity.y += Physics2D.gravity.y * Time.deltaTime;
        }

        // 移动敌人
        transform.Translate(Gravity * Time.deltaTime);

    }

    public void PlayerJump()
    {
        //跳跃
        jump = JumpState.Jumping;
        Gravity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
        transform.Translate(Gravity * Time.deltaTime);
    }
}
