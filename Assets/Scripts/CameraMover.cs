using UnityEngine;

public class CameraMover : MonoBehaviour
{
/*
* 생동감 넘치는 카메라를 표현하기 위해
* 뒤따라오는 카메라를 구현하였다.
*/
    [SerializeField]
    Transform player;
    [SerializeField]
    float smoothSpeed = 5f; // 카메라 부드럽게 이동하기 위해 지연 이동
    [SerializeField]
    Vector3 offset = new Vector3(3f, 2f, -10f); // 플레이어 기준 카메라 시점 이동
    
    void Update()
    {
        if (player == null) return;
        
        // 카메라 시점 + 플레이어 포지션
        Vector3 desiredPosition = player.position + offset;
        // 뒤늦게 따라오는 카메라 계산 공식
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}