using UnityEngine;

public class Button : MonoBehaviour
{
    public Animator anim;
    
    [Header("버튼이 눌렸을 때 올라갈 점프력")]
    public float bonusJumpForce = 20f; 
    private bool _isPressed = false; // 중복 실행 방지용 변수

    void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        // 침 프리팹에 "Sip" 스크립트가 붙어있거나, 태그가 설정되어 있다면 체크
        if ((other.gameObject.CompareTag("Sip") || other.gameObject.GetComponent<Sip>() != null) && !_isPressed)
        {
            Destroy(other.gameObject);
            TriggerButton();
            Debug.Log("침이랑 버튼 충돌");
        }
    }
    
    void TriggerButton()
    {
        _isPressed = true; // 버튼은 한 번만 눌리도록 고정
        
        SoundManagers.instance.PlaySFX(SoundManagers.instance.switchOnSFX);

        // 씬에서 "Player" 태그를 가진 플레이어 오브젝트를 찾음
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // 플레이어에게서 PlayerMove 스크립트 가져옴
            PlayerMove playerMove = player.GetComponent<PlayerMove>();

            if (playerMove != null)
            {
                // 점프포스를 원하는 수치로 변경
                playerMove.jumpForce = bonusJumpForce;
                
                Debug.Log("침 적중! 토끼의 점프력이 " + bonusJumpForce + "(으)로 상승했습니다!");
            }
        }
        
        anim.Play("ButtonOnclick");
    }
}