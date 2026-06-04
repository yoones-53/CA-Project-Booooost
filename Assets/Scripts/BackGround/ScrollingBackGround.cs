using UnityEngine;

public class ScrollingBackGround : MonoBehaviour
{
    /*
    * 패럴랙스 백그라운드 구현
    * 1. 백그라운드 타일 파츠화
    * 2. 백그라운드 뒷 타일일 수록 느리게, 앞 타일일 수록 빠르게 프로퍼티 부여
    */
    
    public Transform mainCamera;

    public float scrollSpeed = 0.5f;

    private Vector3 lastCameraPosition;

    void Start()
    {
        lastCameraPosition = mainCamera.position;
    }

    void Update()
    {
        Vector3 deltaMovement = mainCamera.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * scrollSpeed, deltaMovement.y * scrollSpeed, 0f);
        lastCameraPosition = mainCamera.position;
    }
}