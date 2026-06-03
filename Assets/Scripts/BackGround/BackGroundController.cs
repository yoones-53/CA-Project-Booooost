using UnityEngine;

public class BackgroundController : MonoBehaviour
{
/* 
* 두 개의 맵 타일을 이어붙여서 한 개의 타일이 카메라 밖으로 나가면 
* 선두의 타일의 오른쪽으로 이어붙이는 형식으로 구현하였지만 
* 로켓이 뒤로가는 경우도 있는 게임이기에 응용을 하였다.
* 한 개의 타일이 오른쪽 카메라 밖으로 나가면 => GetCameraRightX()
* 왼쪽으로 이어붙이는 방식도 구현하였다. => movingRenderer.bounds.min.x > GetCameraRightX()
*/

    [SerializeField]
    Transform background1;
    [SerializeField]
    Transform background2;

    [SerializeField]
    Camera targetCamera;

    private SpriteRenderer background1Renderer;
    private SpriteRenderer background2Renderer;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (background1 == null || background2 == null)
            return;

        background1Renderer = background1.GetComponent<SpriteRenderer>();
        background2Renderer = background2.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (background1 == null || background2 == null || background1Renderer == null || background2Renderer == null)
            return;

        RepositionIfOutside(background1, background1Renderer, background2Renderer);
        RepositionIfOutside(background2, background2Renderer, background1Renderer);
    }

    
    void RepositionIfOutside(Transform movingBackground, SpriteRenderer movingRenderer, SpriteRenderer otherRenderer)
    {
        // 타일이 카메라 왼쪽으로 나가면 오른쪽으로 이어붙이기
        if (movingRenderer.bounds.max.x < GetCameraLeftX())
        {
            float newX = otherRenderer.bounds.max.x + movingRenderer.bounds.extents.x;
            movingBackground.position = new Vector3(newX, movingBackground.position.y, movingBackground.position.z);
        }

        // 타일이 카메라 오른쪽으로 나가면 왼쪽으로 이어붙이기
        else if (movingRenderer.bounds.min.x > GetCameraRightX())
        {
            float newX = otherRenderer.bounds.min.x - movingRenderer.bounds.extents.x;
            movingBackground.position = new Vector3(newX, movingBackground.position.y, movingBackground.position.z);
        }
    }

    // 카메라 왼쪽 끝 계산
    float GetCameraLeftX()
    {
        if (targetCamera == null)
            return float.NegativeInfinity;

        float distance = Mathf.Abs(targetCamera.transform.position.z - transform.position.z);
        return targetCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, distance)).x;
    }

    // 카메라 오른쪽 끝 계산
    float GetCameraRightX()
    {
        if (targetCamera == null)
            return float.PositiveInfinity;

        float distance = Mathf.Abs(targetCamera.transform.position.z - transform.position.z);
        return targetCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, distance)).x;
    }
}