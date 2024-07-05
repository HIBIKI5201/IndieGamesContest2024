using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    [ReadOnly]
    public float _bulletDamage;
    [ReadOnly]
    public float _bulletSpeed;
    [ReadOnly]
    public float _bulletTime;

    [Space]

    [ReadOnly]
    public float _explosionRange;

    Transform PlayerPos;
    Rigidbody2D rb2D;

    CircleCollider2D circleCollider;

    public EnemyBulletKind _enemyBulletKind;
    public enum EnemyBulletKind
    {
        bomberExplosion,
        normalBullet,
        followBullet
    }

    void Start()
    {
        switch (_enemyBulletKind)
        {
            case EnemyBulletKind.normalBullet:
            case EnemyBulletKind.followBullet:

                PlayerPos = GameObject.Find("Player").GetComponent<Transform>();
                rb2D = GetComponent<Rigidbody2D>();
                Invoke(nameof(Destroy), _bulletTime);

                break;

            case EnemyBulletKind.bomberExplosion:
                circleCollider = GetComponent<CircleCollider2D>();
                circleCollider.radius = _explosionRange;

                Invoke(nameof(Destroy), 0.4f);
                break;
        }



        if (_enemyBulletKind == EnemyBulletKind.normalBullet)
        {
            Vector2 axis = PlayerPos.position - transform.position;
            rb2D.velocity = axis.normalized * _bulletSpeed;
        }
    }

    void Update()
    {
        //í«îˆíeÇ»ÇÁÉvÉåÉCÉÑÅ[Çí«îˆÇ∑ÇÈ
        if (_enemyBulletKind == EnemyBulletKind.followBullet)
        {
            Vector2 axis = PlayerPos.position - transform.position;
            rb2D.velocity = axis.normalized * _bulletSpeed;

            transform.rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(axis.y, axis.x) * Mathf.Rad2Deg) - 90);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ãﬂê⁄çUåÇÇ…ìñÇΩÇ¡ÇΩÇÁîjâÛÇ≥ÇÍÇÈ
        if (collision.gameObject.CompareTag("Melee"))
        {
            switch (_enemyBulletKind)
            {
                case EnemyBulletKind.normalBullet:
                case EnemyBulletKind.followBullet:

                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SpiritPowerIncrease(2);
                    Destroy(gameObject);
                    break;
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().HitDamage(_bulletDamage);
            switch (_enemyBulletKind)
            {
                case EnemyBulletKind.normalBullet:
                case EnemyBulletKind.followBullet:
                    Destroy(gameObject);
                    break;

                case EnemyBulletKind.bomberExplosion:
                    circleCollider.enabled = false;
                    break;
            }
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
