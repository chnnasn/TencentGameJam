using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : CheckPhysics
{
    // Start is called before the first frame update
    Vector2 MoveMent;
    [HideInInspector]
    public bool CanMove = false;
    Animator Animator;
    SpriteRenderer sprite;

    private void OnEnable()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        Animator = gameObject.GetComponent<Animator>();
        StartCoroutine(WaitForGrounded());
    }

    new void Start()
    {
       
    }

    IEnumerator WaitForGrounded()
    {
        // 等待直到Player.jump状态变为JumpState.Grounded
        while (jump != JumpState.Grounded)
        {
            yield return null; // 每帧检查一次
        }

        // 当jump状态变为Grounded时，等待按下任意键
        while (!Input.anyKeyDown)
        {
            yield return null; // 每帧检查一次
        }

        // 当按下任意键时，触发相应行为
        CanMove = true;
    }

    public float MoveSpeed;
    // Update is called once per frame
    void Update()
    {
        // 正常获取输入并移动物体
        if (CanMove && Recall.instance.timeState != TimeState.Recall)
        {

            MoveMent.x = Input.GetAxis("Horizontal");

            if (MoveMent.x != 0)
            {
                sprite.flipX = MoveMent.x < 0;

                if (!IsAnimationPlaying("JumpIng") && !IsAnimation("Fall") && !IsAnimationPlaying("Ground")) {

                    Animator.Play("Walk");
                }

                transform.Translate(MoveMent * MoveSpeed * Time.deltaTime);

            }
            else if (!IsAnimationPlaying("JumpIng") && !IsAnimation("Fall") && !IsAnimationPlaying("Ground"))
            {
                Animator.Play("Idie");
            }


        }

        // 按下空格键后，开始跳
        if (jump == JumpState.Grounded && Input.GetKeyDown(KeyCode.Space))
        {
            Animator.Play("JumpIng");

            PlayerJump();

        }


        // 检查是否超出摄像机的范围
        if (IsOutOfCameraView())
        {
            // 执行相应的处理，比如重新加载场景
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    private bool IsAnimation(string animationName)
    {
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName);
    }

    bool IsOutOfCameraView()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1;
    }


    bool IsAnimationPlaying(string name)
    {
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(name) && stateInfo.normalizedTime < 1.0f;
    }

    private void FixedUpdate()
    {
        if (Recall.instance.timeState != TimeState.Recall)
        {
            base.UseGravity();

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("End"))
        {
            if (Recall.instance.timeState != TimeState.After)
            {
                Recall.instance.timeState = TimeState.StartRecall;
            }
            else
            {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                int totalScenes = SceneManager.sceneCountInBuildSettings;

                if (currentSceneIndex + 1 < totalScenes)
                {
                    SceneManager.LoadScene(currentSceneIndex + 1);
                }
                else
                {
                    Debug.Log("kkk");
                }

            }
        }

        if (collision.transform.CompareTag("Enemy") && Recall.instance.timeState == TimeState.After)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    // 重写PlayerJump方法
    public override void PlayerJump()
    {
        // 自定义的跳跃实现
        jump = JumpState.Jumping;
        Gravity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
        transform.Translate(Gravity * Time.deltaTime);

        StartCoroutine(Fall());
    }

    IEnumerator Fall() {
        while (true) {
            if (jump == JumpState.Grounded)
            {
                Animator.Play("Ground");
                break;
            }
            yield return null;
        }


    }

}
