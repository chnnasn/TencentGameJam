using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CheckPhysics
{
    // Start is called before the first frame update
    Vector2 MoveMent;
    bool CanMove = false;

    new void Start()
    {

        StartCoroutine(WaitForGrounded());
    }

    IEnumerator WaitForGrounded()
    {
        // 等待直到Player.jump状态变为JumpState.Grounded
        while (jump != JumpState.Grounded)
        {
            yield return null; // 每帧检查一次
        }

        // 当jump状态变为Grounded时，触发相应行为
        CanMove = true;
        yield break;
    }

    public float MoveSpeed;
    // Update is called once per frame
    void Update()
    {
        // 正常获取输入并移动物体
        if (CanMove && Recall.instance.timeState != TimeState.Recall) {

            MoveMent.x = Input.GetAxis("Horizontal");

            transform.Translate(MoveMent * MoveSpeed * Time.deltaTime);
        }

        // 按下空格键后，开始倒着移动
        if (jump == JumpState.Grounded && Input.GetKeyDown(KeyCode.Space))
        {
            base.PlayerJump();
        }

    }

    private void FixedUpdate()
    {
        if (Recall.instance.timeState != TimeState.Recall) {
                base.UseGravity();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("End"))
        {
            if (Recall.instance.timeState != TimeState.After) {
                Recall.instance.timeState = TimeState.StartRecall;
            }
            else {
                Debug.LogWarning("胜利");
            }
           

        }
    }

}
