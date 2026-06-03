using UnityEngine;

public class CircleEnemy : MonoBehaviour
{
    /*
    * 원형으로 일정거리를 움직이는 외계인
    */
    public float radius = 2f;       // 원 크기
    public float moveSpeed = 2f;    // 회전 속도

    private Vector2 centerPos; // 기준점

    void Start()
    {
        centerPos = transform.position;
    }

    void Update()
    {
        /*
        * Cos 함수로 X축 위치를, Sin 함수로 Y축 위치를 계산하여 원형 경로를 만든다.
        * 각각 동일한 radius을 곱해주어 값에 따라 원의 크기가 달라진다.
        */
        float x = Mathf.Cos(Time.time * moveSpeed) * radius;
        float y = Mathf.Sin(Time.time * moveSpeed) * radius;
        transform.position = new Vector2(centerPos.x + x, centerPos.y + y);
    }
}