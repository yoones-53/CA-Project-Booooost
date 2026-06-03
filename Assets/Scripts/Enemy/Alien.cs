using UnityEngine;

public class Alien : MonoBehaviour 
{
    /*
    * 외계인 장애물의 충돌 및 비활성화를 담당하는 스크립트
    * 외계인에게 충돌 판정을 주며, 플레이어가 충돌시 
    * 게임매니저의 Explode메소드를 가져와서 플레이어를 게임오버 시킨다. 
    *
    * 그리고 카메라 왼쪽 끝에서 destroyOffset 만큼 
    * 더 벗어나면 외계인이 비활성화 되며 오브젝트 풀에 저장된다.
    *
    * Ground 지형도 Alien 함수가 들어갔지만 사라지면 안되기 때문에
    * destroyOffset의 값을 무한대로 늘려 사라지지 않도록 하였다.
    */

    [HideInInspector]
    public Camera targetCamera;
    [SerializeField]
    float destroyOffset = 30f;

    void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.isTrigger = true;
    }

    void Update()
    {
        if (transform.position.x < GetCameraLeftX() - destroyOffset)
            gameObject.SetActive(false);
    }
    
    // 충돌시 게임오버
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rocket rocket = other.GetComponent<Rocket>();
        if (rocket == null) return;
        rocket.Explode(); // 플레이어 폭발 및 게임오버
    }

    // 카메라 왼쪽 끝값 계산
    float GetCameraLeftX()
    {
        Camera cam = targetCamera != null ? targetCamera : Camera.main;
        if (cam == null)
            return float.NegativeInfinity;

        float distance = Mathf.Abs(cam.transform.position.z - transform.position.z);
        return cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, distance)).x;
    }
}
