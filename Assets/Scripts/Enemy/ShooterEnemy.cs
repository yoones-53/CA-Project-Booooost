using UnityEngine;
public class ShooterEnemy : MonoBehaviour
{
    public Transform player;

    [Header("Attack")]
    public Transform firePoint; // 총알 발사 위치
    public float shootInterval = 1.2f; // 발사 간격
    public float detectRange = 9f; // 플레이어 감지 거리
    
    private float shootTimer = 0f;

    void OnEnable()
    {
        shootTimer = 0f;
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
        GameObject bulletObj = PoolManager.Instance.GetBullet();

        bulletObj.transform.position = firePoint.position;

        Bullet bullet = bulletObj.GetComponent<Bullet>();

        Vector2 direction = (player.position - firePoint.position).normalized;
        bullet.SetDirection(direction);
    }
}