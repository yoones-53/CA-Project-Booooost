using UnityEngine;
public class ShooterEnemy : MonoBehaviour
{
    /*
    * 총알을 발사하는 외계인
    * 플레이어가 detectRange 거리 근처에 있으면 총알을 발사한다.
    */
    public Transform player;

    public Transform firePoint; // 총알 발사 위치
    public float shootInterval = 1.2f; // 연사 속도
    public float detectRange = 9f; // 사정거리
    float shootTimer = 0f;

    // 활성화 후 총알 타이머 초기화
    void OnEnable()
    {
        shootTimer = 0f;
    }

    void Start()
    {
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
                player = found.transform;
        }
    }

    void Update()
    {
        if (player == null) return;
        // 플레이어 거리 계산
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRange) // 플레이어가 범위 안에 있을 때만 작동
        {
            LookAtPlayer(); // 플레이어 방향으로 회전
            ShootTimer();
        }
    }

    // 다음 발사시간
    void ShootTimer()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval) // 일정 시간마다 발사
        {
            Shoot();
            shootTimer = 0f;  // 타이머 초기화
        }
    }

    void LookAtPlayer()
    {
        Vector2 direction = player.position - transform.position; // 플레이어 방향

        /* 
        * 𝜃=arctan(𝑦/𝑥) 사분면을 구분하여 방향벡터를 회전 각도로 변환
        * Mathf.Rad2Deg를 곱해 라디안 => 도로 변환
        */
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도 계산

        // 이미지가 반대쪽 보고있어서 180도 더해줌
        transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
    }

    // 오브젝트 풀에서 총알 가져와 발사
    void Shoot()
    {
        GameObject bulletObj = PoolManager.Instance.GetBullet();

        bulletObj.transform.position = firePoint.position;

        Bullet bullet = bulletObj.GetComponent<Bullet>();

        Vector2 direction = (player.position - firePoint.position).normalized;
        bullet.SetDirection(direction);
    }
}