using UnityEngine;
using UnityEngine.Pool;
public class ShooterEnemy : MonoBehaviour
{
    public Transform player;

    [Header("Attack")]
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint; // 총알 발사 위치
    public float shootInterval = 1.2f; // 발사 간격
    public float detectRange = 9f; // 플레이어 감지 거리

    private float shootTimer = 0f;
    private IObjectPool<Bullet> _Pool;

    void Awake()
    {
        _Pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize:10);
    }

    void Start()
    {
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            player = found.transform;
        }
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position); // 플레이어 거리 계산

        if (distance <= detectRange) // 플레이어가 범위 안에 있을 때만 작동
        {
            LookAtPlayer(); // 플레이어 방향으로 회전

            shootTimer += Time.deltaTime;

            if (shootTimer >= shootInterval) // 일정 시간마다 발사
            {
                Shoot();
                shootTimer = 0f;  // 타이머 초기화
            }
        }
    }

    void LookAtPlayer()
    {
        Vector2 direction = player.position - transform.position; // 플레이어 방향
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도 계산
        transform.rotation = Quaternion.Euler(0, 0, angle + 180f);// Z축 기준 회전
    }

    void Shoot()
    {
        Bullet bullet = _Pool.Get();

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        Vector2 direction = (player.position - firePoint.position).normalized;
        bullet.SetDirection(direction);
        /*
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); // 총알 생성
        Vector2 direction = (player.position - firePoint.position).normalized;
        Bullet bulletScript = bullet.GetComponent<Bullet>(); // Bullet 스크립트에 방향 전달
        bulletScript.SetDirection(direction); // 플레이어 방향
        */
    }

    Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
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