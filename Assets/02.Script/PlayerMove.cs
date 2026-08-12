using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    [Header("움직임 관련 변수")]
    public float speed = 5f;
    public Rigidbody2D rb;
    public Animator anim;
    private Vector2 _moveInput;
    
    [Header("캐릭터 변경 관련 함수")]
    public bool rabbit = true;
    public bool durumi = false;
    public bool rama = false;
    
    [Header("라마 침 뱉는 거 관련")]
    public bool ramaClick = false;
    public GameObject sip;

    [Header("점프 관련 변수")]
    public bool isGround;
    public float jumpForce = 12f;
    public int jumpCount = 0;
    public bool JumpQueued;
    
    [Header("대시 관련 변수")]
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    private bool _isDashing = false;
    private bool _canDash = true;

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        anim.SetFloat("Animal", 0);
        
        StartCoroutine(PlayBGMOnStart());
    }
    
    IEnumerator PlayBGMOnStart()
    {
        yield return null; // 한 프레임 대기 (유니티 오디오 시스템 초기화 대기)
        if (SoundManagers.instance != null)
        {
            SoundManagers.instance.PlayBGM(SoundManagers.instance.stageBGM);
        }
    }
    
    void Update()
    {
        // 움직임
        float x = Input.GetAxisRaw("Horizontal");
        _moveInput = new Vector2(x, 0f);

        // 좌우 그림 반전
        if (x > 0)
        {
            anim.SetBool("isWalking", true);
            transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        } else if (x < 0)
        {
            transform.localScale = new Vector3(-0.2f, 0.2f, 1f);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        
        // 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpQueued = true;
        }
        
        // 대시
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash && durumi)
        {
            StartCoroutine(Dash());
        }
        
        // 바꾸는 거
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            rabbit = true;
            durumi = false;
            rama = false;
            
            anim.SetFloat("Animal", 0);
            anim.Play("IdleControl", 0, 0f);
            
            SoundManagers.instance.PlaySFX(SoundManagers.instance.switchOnSFX);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rabbit = false;
            durumi = true;
            rama = false;
            
            anim.SetFloat("Animal", 1);
            anim.Play("IdleControl", 0, 0f);
            
            SoundManagers.instance.PlaySFX(SoundManagers.instance.switchOnSFX);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            rabbit = false;
            durumi = false;
            rama = true;
            
            anim.SetFloat("Animal", 2);
            anim.Play("IdleControl", 0, 0f);
            
            SoundManagers.instance.PlaySFX(SoundManagers.instance.switchOnSFX);
        }

        // 침뱉기 활성화
        if (rama && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Rama();
        }
    }

    void FixedUpdate()
    {
        if (_isDashing) return;
        if (rabbit) Jump();
        
        Move();
    }

    void Move()
    {
        Vector2 velo = rb.linearVelocity;
        velo.x = _moveInput.x * speed;
        rb.linearVelocity = velo;
    }

    void Jump()
    {
        if (!JumpQueued) return;
        JumpQueued = false;

        if (jumpCount >= 1) return;
        jumpCount++;
        
        Vector2 velo = rb.linearVelocity;
        velo.y = 0f;
        rb.linearVelocity = velo;
        
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        SoundManagers.instance.PlaySFX(SoundManagers.instance.jumpSFX);
    }

    IEnumerator Dash()
    {
        anim.SetBool("isDash", true);
        SoundManagers.instance.PlaySFX(SoundManagers.instance.dashSFX);
        _canDash = false;
        _isDashing = true;

        // 원래 중력 저장 & 중력 0으로 만들기
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        
        // 플레이어가 보는 방향
        float dashDirection = Input.GetAxisRaw("Horizontal");
        if (dashDirection == 0)
        {
            // localScale.x가 0보다 크면 오른쪽(1f), 작으면 왼쪽(-1f)으로 방향 설정
            dashDirection = transform.localScale.x > 0 ? 1f : -1f;
        }

        // 이제 정상적으로 20 또는 -20의 속도로 시원하게 날아갑니다!
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

        // 움직이는 동안 기다림
        yield return new WaitForSeconds(dashTime);
        
        anim.SetBool("isDash", false);

        // 기다림 끝났으니까 다시 중력 적용
        rb.gravityScale = originalGravity;
        _isDashing = false;
        
        yield return new WaitForSeconds(dashCooldown); 
        _canDash = true;
    }

    void Rama()
    {
        // 소환!
        GameObject gm = Instantiate(sip);
        SoundManagers.instance.PlaySFX(SoundManagers.instance.ramaSpitSFX);
    
        // 플레이어가 바라보는 방향 (오른쪽이면 1f, 왼쪽이면 -1f)
        float direction = transform.localScale.x > 0 ? 1f : -1f;
    
        // 캐릭터 중심에서 살짝 앞, 그리고 살짝 위(Y + 0.2f)에서 소환되도록 보정
        Vector3 spawnPosition = transform.position + new Vector3(direction * 0.5f, 0.8f, 0f);
    
        gm.transform.position = spawnPosition;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isGround", true);
            
            SoundManagers.instance.StopDashSound();
            
            isGround = true;
            JumpQueued = false;
            
            jumpCount = 0;
        }

        if (other.gameObject.CompareTag("Hurt"))
        {
            StartCoroutine(PlayerDeathRoutine());
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isGround", false);
            isGround = false;
        }
    }

    IEnumerator PlayerDeathRoutine()
    {
        // 1. 💨 대시 소리나 다른 효과음 꺼버리기
        SoundManagers.instance.StopDashSound(); // 작성하신 함수 이름 적용
        
        // 3. 💀 죽는 소리 빵 터뜨리기
        SoundManagers.instance.PlaySFX(SoundManagers.instance.deathSFX);

        // 4. 👁️ 플레이어의 그래픽과 충돌체만 화면에서 안 보이게 숨깁니다.
        // (바로 Destroy를 하면 코루틴도 같이 멈춰버리기 때문에 숨기기만 해야 합니다!)
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        // 만약 이동 물리도 멈추고 싶다면
        GetComponent<Rigidbody2D>().simulated = false; 

        // 5. ⏳ 죽는 소리가 들릴 시간을 줍니다 (0.6초 대기. 소리 길이에 맞춰 조절하세요!)
        yield return new WaitForSeconds(0.6f);

        // 6. 🔄 소리가 다 났으니 이제 씬을 안전하게 재시작합니다!
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}