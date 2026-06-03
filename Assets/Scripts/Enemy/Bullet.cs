using UnityEngine;

public class Bullet : MonoBehaviour
{
    /*
    * 총알을 Instantate, Destroy 방식이 아닌 오브젝트풀링을
    * 사용하여 오브젝트 풀 매니저에 담긴 총알 클론이
    * 생성된 뒤 destroyTimer후에 비활성화 되며, 필요할때마다 활성화 한다.
    *
    * 참고로 자폭 외계인에게도 총알 스크립트가 달려있으며
    * bulletSpeed 0f, destroyTimer 0.4f로 재사용 했다.
    */

    public float bulletSpeed = 6f;  // 총알 빠르기
    public float destroyTimer = 1f;  // 발사된 뒤 비활성화 시간
    private Vector2 moveDirection;

    // 총알이 활성화 되면 destroyTimer에 저장된 시간뒤에 비활성화
    void OnEnable()
    {
        CancelInvoke();
        Invoke("DisableBullet", destroyTimer);
    }

    void Update()
    {
        transform.position += (Vector3)(moveDirection * bulletSpeed * Time.deltaTime);
    }

    // 위치, 회전값 설정
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;

        /* 
        * 𝜃=arctan(𝑦/𝑥) 사분면을 구분하여 방향벡터를 회전 각도로 변환
        * Mathf.Rad2Deg를 곱해 라디안 => 도로 변환
        */
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 총알 비활성화  
    void DisableBullet()
    {
        gameObject.SetActive(false);
    }
}