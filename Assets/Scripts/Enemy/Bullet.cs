using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 6f;
    public float destroyTimer = 1f;
    private Vector2 moveDirection;
    private IObjectPool<Bullet> _ManagedPool; // 오브젝트 풀
    void Start()
    {
        //Destroy(gameObject, destroyTimer);
        Invoke("DestroyBullet",destroyTimer);
    }
    void Update()
    {
        transform.position += (Vector3)(moveDirection * bulletSpeed * Time.deltaTime);
    }

    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        _ManagedPool = pool;
    }
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void DestroyBullet()
    {
        _ManagedPool.Release(this);
    }
}