using UnityEngine;

public class Sip : MonoBehaviour
{
    public float speed = 10f; // 침이니까 속도를 좀 더 시원하게 올렸어요!
    private float _direction = 1f;

    void Start()
    {
        // 1. 소환될 때 플레이어를 찾습니다. (태그가 "Player"인 오브젝트)
        GameObject player = GameObject.FindWithTag("Player");
        
        if (player != null)
        {
            // 2. 플레이어의 좌우 반전 상태(localScale.x)를 보고 날아갈 방향을 결정합니다.
            // 플레이어가 오른쪽을 보면 1, 왼쪽을 보면 -1이 됩니다.
            _direction = player.transform.localScale.x;
            
            // 꼼수: 침 이미지도 플레이어가 바라보는 방향에 맞게 돌려줍니다.
            if (_direction > 0)
            {
                transform.localScale = new Vector3(0.15f, 0.15f, 1f);
            } 
            else if (_direction < 0)
            {
                transform.localScale = new Vector3(-0.15f, 0.15f, 1f);
            }
        }
    }

    void Update()
    {
        // 3. 결정된 방향으로만 매 프레임 직진합니다. (키 입력 영향 받지 않음!)
        transform.Translate(Vector2.right * _direction * speed * Time.deltaTime);
    }

    // 4. 벽이나 버튼에 부딪히면 침이 사라지게 처리 (안 하면 맵 밖으로 영원히 날아감)
    void OnCollisionEnter2D(Collision2D other)
    {
        // 가시나 땅, 스위치 등에 부딪히면 삭제 (태그는 프로젝트 상황에 맞게 조절하세요)
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Hurt") || other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}