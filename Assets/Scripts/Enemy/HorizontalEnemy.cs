using UnityEngine;

public class HorizontalEnemy : MonoBehaviour
{
    /*
    * 좌우로 일정거리를 움직이는 외계인
    * 단순히 direction을 사용하면 움직임이 딱딱하기에
    * 삼각함수로 부드러운 움직임을 구현하였다.
    */
    private Vector2 startPos; // 시작 위치
    public float moveDistance = 2f;   // 시작점 기준 좌우 이동 거리
    public float moveSpeed = 2f;      // 이동 속도

    void Start()
    {
        startPos = transform.position; // 현재 위치를 시작 위치로 저장
    }

    void Update()
    {
        /* 
        * 삼각함수를 활용하여 부드러운 좌우 이동 구현
        * Time.time * moveSpeed = 시간경과 * 속도, moveDistance = 진폭
        */
        float x = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector2(startPos.x + x, startPos.y);
    }
}