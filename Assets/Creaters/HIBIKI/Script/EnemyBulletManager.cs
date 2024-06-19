using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    public float _bulletDamage;
    public float _bulletSpeed;
    public float _bulletTime;
    
    Transform PlayerPos;
    Rigidbody2D rb2D;

    public EnemyBulletKind _enemyBulletKind;
    public enum EnemyBulletKind
    {
        followBullet
    }

    void Start()
    {
        PlayerPos = GameObject.Find("Player").GetComponent<Transform>();
        rb2D = GetComponent<Rigidbody2D>();

        Invoke("Destroy", _bulletTime);
    }

    void Update()
    {
        if (_enemyBulletKind == EnemyBulletKind.followBullet)
        {
            Vector2 axis = PlayerPos.position - transform.position;
            GetComponent<Rigidbody2D>().velocity = axis.normalized * _bulletSpeed;

            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(axis.y, axis.x) * Mathf.Rad2Deg);
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
