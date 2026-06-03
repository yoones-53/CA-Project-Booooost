using UnityEngine;

public class Rocket : MonoBehaviour
{
    /*
    * 플레이어가 조작하는 로켓
    * 추진: 좌클릭, Space, W, 윗방향키 ( 추진 파티클과 추진음 재생 )
    * 좌회전: A, 좌방향키 / 우회전: D, 우방향키
    * 로켓 충돌시 충돌 파티클과 충돌음 재생
    */

    [Header("Movement")]
    public float thrustForce = 4f;   // 추진 속도
    public float rotateSpeed = 120f; // 회전 속도
    public float maxSpeed = 9f;      // 최고 속도

    [Header("Sounds")]
    public AudioSource engineSound;  // 엔진 사운드
    public AudioSource explodeSound; // 폭발 사운드

    [Header("Rocket")]
    public Sprite idleSprite;               // 대기 상태 스프라이트
    public ParticleSystem thrustParticle;   // 추진 파티클
    public ParticleSystem explodeParticle;  // 게임오버 파티클

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private bool isGameOver = false;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isGameOver) return;
        
        RocketThrust();
        RocketRotation();

        // 로켓이 y.300을 넘기면 y.10으로 순간이동
        if (transform.position.y > 300f)
            transform.position = new Vector3(transform.position.x, 10f, 0f);
    }

    // 로켓 추진
    void RocketThrust()
    {
        // 좌클릭, Space, W, 윗방향키
        bool isThrusting = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) ||
                           Input.GetKey(KeyCode.W)     || Input.GetMouseButton(0);

        /*
        * 로켓이 추진할 때 추진음, 추진 파티클이 재생되고
        * 추진을 멈추면 재생을 멈춘다.
        */
        if (isThrusting) // 추진 중일때
        {
            // 로켓의 위쪽 방향으로 추진력 적용
            rigidBody.AddForce(transform.up * thrustForce * Time.deltaTime);
            // 최고 속도 제한
            rigidBody.linearVelocity = Vector2.ClampMagnitude(rigidBody.linearVelocity, maxSpeed);
            
            if (!engineSound.isPlaying)
            {
                thrustParticle.Play();
                engineSound.Play();
            }
        }

        else // 추진 중이 아닐때
        {
            if (!engineSound.isPlaying) return;

            thrustParticle.Stop();
            engineSound.Stop();
        }
    }
    
    // 로켓 회전
    void RocketRotation()
    {
        float rotateInput = 0f;

        // 왼쪽: A, 좌 방향키
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rotateInput = 1f;
        }
        // 오른쪽: D, 우 방향키
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rotateInput = -1f;
        }

        transform.Rotate(0f, 0f, rotateInput * rotateSpeed * Time.deltaTime);
    }

    // 로켓 폭발(게임오버)
    public void Explode()
    {
        if(isGameOver) return;
        isGameOver = true;
        
        thrustParticle.Stop();
        engineSound.Stop();
        
        explodeParticle.Play();
        explodeSound.Play();
        
        spriteRenderer.enabled = false;
        rigidBody.simulated = false; // 폭발시 로켓 위치 고정
        GameManager.Instance.GameOver(); // 게임매니저에 게임오버 상태 전송
    }
}
