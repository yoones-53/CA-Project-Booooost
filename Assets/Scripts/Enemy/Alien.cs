using UnityEngine;

public class Alien : MonoBehaviour 
{
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rocket rocket = other.GetComponent<Rocket>();
        if (rocket == null) return;
        rocket.Explode();
    }
    float GetCameraLeftX()
    {
        Camera cam = targetCamera != null ? targetCamera : Camera.main;
        if (cam == null)
            return float.NegativeInfinity;

        float distance = Mathf.Abs(cam.transform.position.z - transform.position.z);
        return cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, distance)).x;
    }
}
