using UnityEngine;

public class FadeEnemy : MonoBehaviour
{
    /*
    * 플레이어가 detectRange 거리 근처에 있으면 투명으로 변하는 외계인
    * 선형 보간법을 사용하여 투명도가 부드럽게 변화하도록 구현.
    */
    
    public Transform player;
    public float detectRange = 13f; // 감지 범위
    public float fadeSpeed = 5f; // 투명화 속도

    SpriteRenderer spriteRenderer;
    float currentAlpha = 1f; // 초기 투명화값

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            player = found.transform;
        }
    }

    void Update()
    {
        if (player == null) return;
        // 플레이어 거리 계산
        float distance = Vector2.Distance(transform.position, player.position);

        ApplyAlpha(distance);
    }

    // 플레이어와 거리를 기반으로 투명
    void ApplyAlpha(float distance)
    {
        float targetAlpha = 1f;

        if (distance <= detectRange)
        {
            // 조금이라도 확인은 가능하도록 0.005f
            targetAlpha = 0.005f;
        }
        
        // 선형보간으로 서서히 투명
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha * 2, Time.deltaTime * fadeSpeed);

        Color color = spriteRenderer.color;
        color.a = currentAlpha;
        spriteRenderer.color = color;
    }
}