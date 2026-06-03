using UnityEngine;

public class VerticalBackgroundController : MonoBehaviour
{
/* 
* 왼쪽 오른쪽 무한맵을 응용하여 위아래 무한맵 제작
*/

    [SerializeField]
    Transform topBackground;
    [SerializeField]
    Transform bottomBackground;

    [SerializeField]
    Camera targetCamera;

    private SpriteRenderer topRenderer;
    private SpriteRenderer bottomRenderer;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (topBackground == null || bottomBackground == null)
            return;

        topRenderer = topBackground.GetComponent<SpriteRenderer>();
        bottomRenderer = bottomBackground.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (topBackground == null || bottomBackground == null || topRenderer == null || bottomRenderer == null)
            return;

        RepositionIfOutside(topBackground, topRenderer, bottomRenderer);
        RepositionIfOutside(bottomBackground, bottomRenderer, topRenderer);
    }

    void RepositionIfOutside(Transform movingBackground, SpriteRenderer movingRenderer, SpriteRenderer otherRenderer)
    {
        // 타일이 카메라 아래쪽으로 나가면 윗쪽으로 이어붙이기
        if (movingRenderer.bounds.max.y < GetCameraBottomY())
        {
            float newY = otherRenderer.bounds.max.y + movingRenderer.bounds.extents.y;
            movingBackground.position = new Vector3(movingBackground.position.x, newY, movingBackground.position.z);
        }

        // 타일이 카메라 윗쪽으로 나가면 아래쪽으로 이어붙이기
        else if (movingRenderer.bounds.min.y > GetCameraTopY())
        {
            float newY = otherRenderer.bounds.min.y - movingRenderer.bounds.extents.y;
            movingBackground.position = new Vector3(movingBackground.position.x, newY, movingBackground.position.z);
        }
    }

    float GetCameraBottomY()
    {
        if (targetCamera == null)
            return float.NegativeInfinity;

        float distance = Mathf.Abs(targetCamera.transform.position.z - transform.position.z);

        return targetCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, distance)).y;
    }

    float GetCameraTopY()
    {
        if (targetCamera == null)
            return float.PositiveInfinity;

        float distance = Mathf.Abs(targetCamera.transform.position.z - transform.position.z);

        return targetCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, distance)).y;
    }
}