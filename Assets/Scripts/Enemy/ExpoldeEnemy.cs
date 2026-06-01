using UnityEngine;
using UnityEngine.Pool;

public class ExplodeEnemy : MonoBehaviour
{
    public Transform player;
    
    public Transform explosionPoint;
    public float detectDistance = 4.2f; // 플레이어 감지 거리
    public float explodeDelay = 0.7f; // 감지 후 폭발 시간 
    public GameObject exploderPrefab;

    private bool isCountingDown = false; // 폭발 카운트다운 시작 여부
    private float timer = 0f; // 타이머
    private SpriteRenderer spriteRenderer;
    private IObjectPool<Bullet> _Pool;

    void Awake()
    {
        _Pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize:10);
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (player == null) // 플레이어 태그로 플레이어 찾기
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            player = found.transform;
        }
    }
    void Update()
    {
        // 플레이어 거리 계산
        float distance = Vector2.Distance(transform.position, player.position);
        
        // 범위 들어오면 카운트 시작
        if (!isCountingDown && distance <= detectDistance)
        {
            isCountingDown = true; // 타이머 시작
            timer = explodeDelay; // 타이머 값 부여

            if (spriteRenderer != null) // 타이머 시작하면 색깔 붉게 변경
            {
                spriteRenderer.color = new Color(1f, 0.6f, 0.6f);
            }
        }

        if (isCountingDown) // 카운트 중
        {
            timer -= Time.deltaTime;

            if (timer <= 0f) // 타이머 0초가 되면 폭발
            {
                Explosion(); // 폭발
            }
        }
    }

    void Explosion()
    {
        // 폭발 이펙트 생성
        Bullet bullet = _Pool.Get();
        
        bullet.transform.position = explosionPoint.position;

        Destroy(gameObject); // 기존 몬스터 삭제
    }

    Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(exploderPrefab).GetComponent<Bullet>();
        bullet.SetManagedPool(_Pool);
        return bullet;
    }

    void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}