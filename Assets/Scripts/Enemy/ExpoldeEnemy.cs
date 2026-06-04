using UnityEngine;
public class ExplodeEnemy : MonoBehaviour
{
    /*
    * 플레이어가 detectRange 거리 근처에 있으면 폭발 카운트가
    * 시작되며 explodeDelay 시간 뒤에 터지는 외계인
    * 터지면 비활성화 후 오브젝트 풀에 저장된다. 
    */
    public Transform player;
    
    public Transform explosionPoint;
    public float detectRange = 4.2f;  // 플레이어 감지 거리
    public float explodeDelay = 0.7f;    // 감지 후 폭발 시간

    bool isCountingDown = false; // 폭발 카운트다운 시작 여부
    float timer = 0f;
    SpriteRenderer spriteRenderer;

    // 활성화 후 폭발 타이머 초기화
    void OnEnable()
    {
        isCountingDown = false;
        timer = 0f;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
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
        PlayerDetect();

        if (!isCountingDown) return; // 카운트 중
        CountDown();
    }

    // 플레이어 감지
    void PlayerDetect()
    {
        // 플레이어 거리 계산
        float distance = Vector2.Distance(transform.position, player.position);

        // 범위 들어오면 카운트 시작
        if (!isCountingDown && distance <= detectRange)
        {
            isCountingDown = true;
            timer = explodeDelay;

             // 타이머 시작하면 색깔 붉게 변경
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1f, 0.6f, 0.6f);
            }
        }
    }

    // 카운트 다운 후 폭발
    void CountDown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f) // 타이머 0초가 되면 폭발
        {
            Explosion(); // 폭발
        }
    }

    // 터지면 비활성화 후 오브젝트 풀에 저장
    void Explosion()
        {
        GameObject explosionObj = PoolManager.Instance.GetExplosion();

        explosionObj.transform.position = explosionPoint.position;

        gameObject.SetActive(false);
    }
}