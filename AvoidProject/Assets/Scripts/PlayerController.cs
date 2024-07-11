using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;          //Rigidbody를 사용하지 않는 충돌체 캐릭터 컨트롤 용
    public Animator animator;

    //기본 스텟 선언
    public float speed = 5.0f;
    public float runSpeed = 10.0f;
    public float acceleraction = 10.0f;
    public float currentSpeed = 0.0f;
    //점프와 물리
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    //대시 관련 설정
    public float dashSpeed = 20.0f;
    public float dashTime = 2.0f;
    public float dashCooldown = 5.0f;

    private float dashCounter;
    private float dashTimer;
    private bool isDashing = false;
    //액션 상태
    public bool isAction = false;
    //속도 및 방향 값
    private Vector3 velocity;
    private bool canJumpAgain = true;
    public Transform cam;
    Vector3 moveDir;

    public ActionData[] actionDataList = new ActionData[10];
    public ComboSystem comboSystem;
    public Transform fxTransform;

    //액션 관련 변수
    private float actionTimer = 0f;
    private ActionData currentAction = null;
    private GameObject currentFx = null;

  
    void Update()
    {
        bool isGrounded = controller.isGrounded;            //ChracterController에서 땅에 있는지 판별 해준다.

        if(!isAction)
        {
            //액션 함수들을 만든다.
            MoveLogic(isGrounded);
            JumpLogic(isGrounded);
        }
        else
        {
            UpdateAction();
        }

        //중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void MoveLogic(bool isGrounded)
    {
        if(isGrounded)                                      //그라운드 판정이 되었을 때
        {
            velocity.y = -2f;
            canJumpAgain = true;
            animator.SetBool("IsFalling", false);           //떨어짐 false
            animator.SetBool("IsLanding", true);            //랜딩 true
        }
        else
        {
            if(velocity.y < 0)                              //y축 속도값을 보고 판단한다.
            {
                animator.SetBool("IsFalling", true);        //음수이므로 떨어지는 중
            }
            else
            {
                animator.SetBool("IsFalling", false);
            }
        }

        float horizontal = Input.GetAxisRaw("Horizontal");                          //기본 이동키 입력 값
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;       //이동값을 0 ~ 1 사이로 만들어준다.

        animator.SetFloat("MoveSpeed", Mathf.Lerp(0, 1, currentSpeed / speed));

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;      //카메라가 보는 방향이 앞으로 되게 한다.
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            currentSpeed = Mathf.MoveTowards(currentSpeed, speed, acceleraction * Time.deltaTime);

            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0.0f;
        }
    }    

    public void JumpLogic(bool isGrounded)                              //점프 함수
    {
        if(Input.GetKeyDown(KeyCode.Space))                             //스페이스를 눌렀을 때
        {
            if(isGrounded)                                              //땅에 있을때
            {
                animator.SetBool("IsJumping", true);
                animator.SetBool("IsFalling", false);
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);    //점프 최대 높이
            }
            else if(canJumpAgain)
            {
                animator.SetBool("IsJumping", true);
                animator.SetBool("IsFalling", false);
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);    //점프 최대 높이
                canJumpAgain = false;
            }
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }
    }

    void UpdateAction()
    {
        if (currentAction == null) return;          //액션이 없다면 리턴
        actionTimer += Time.deltaTime;

        if(actionTimer >= currentAction.fxTime && currentFx == null)        //VFX 생성 시간이 되면
        {
            SpawnFx();
        }

        if(actionTimer >= currentAction.waitTime)           //기다리는 시간 종료 후 액션을 종료 시킨다.
        {
            EndAction();
        }
    }

    void SpawnFx()                                  //VFX 생성 함수
    {
        if(currentAction.fxObject != null)          //VFX가 NULL값이 아닐때
        {
            currentFx = Instantiate(currentAction.fxObject, fxTransform);       //설정한 VFX 위치와 회전 값을 가져와서 생성한다.
            currentFx.transform.localPosition = Vector3.zero;
            currentFx.transform.localRotation = Quaternion.identity;
        }
    }

    void EndAction()            //액션 종료시 남아있는 것들 다 초기화
    {
        isAction = false;
        if(currentFx != null)
        {
            Destroy(currentFx);
        }
        currentAction = null;
        currentFx = null;
    }
}
