using UnityEngine;
using UnityEngine.EventSystems;

public class Rocket : MonoBehaviour
{
    [Header("Movement")]
    public float thrustForce = 4f; // 추진 속도치
    public float rotateSpeed = 120f; // 회전 속도치
    public float maxSpeed = 9f; // 최고 속도 조정치

    [Header("Sounds")]
    public AudioSource engineSound; // 엔진 사운드
    public AudioSource explodeSound; // 폭발 사운드

    [Header("Rocket")]
    public Sprite idleSprite; // 대기 상태 스프라이트
    public ParticleSystem thrustParticle;
    public ParticleSystem explodeParticle;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private bool isGameOver = false;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null && idleSprite == null)
            return;
    }

    void Update()
    {
        if (isGameOver) return;
        
        HandleThrust();
        HandleRotation();

        if (transform.position.y > 300f)
            transform.position = new Vector3(transform.position.x, 10f, 0f);
    }

    void HandleThrust() // 로켓 추진
    {

        bool isThrusting = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) ||
                           Input.GetKey(KeyCode.W)     || (Input.GetMouseButton(0));

        if (isThrusting) // 추진 중일때
        {
            rigidBody.AddForce(transform.up * thrustForce * Time.deltaTime); // 가속 공식
            rigidBody.linearVelocity = Vector2.ClampMagnitude(rigidBody.linearVelocity, maxSpeed); // 최고 속도 조정
            
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

    void HandleRotation()
    {
        float rotateInput = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rotateInput = 1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rotateInput = -1f;
        }

        transform.Rotate(0f, 0f, rotateInput * rotateSpeed * Time.deltaTime);
    }

    public void Explode() // 로켓 폭발
    {
        if(isGameOver) return;

        isGameOver = true;
        
        thrustParticle.Stop();
        engineSound.Stop();
        
        explodeParticle.Play();
        explodeSound.Play();
        
        spriteRenderer.enabled = false;
        rigidBody.simulated = false; // 폭발시 로켓 위치 고정
        GameManager.Instance.GameOver();
    }
}
