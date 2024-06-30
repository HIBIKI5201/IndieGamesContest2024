using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyBulletManager : MonoBehaviour
{
    [ReadOnly]
    public float _bulletDamage;
    [ReadOnly]
    public float _bulletSpeed;
    [ReadOnly]
    public float _bulletTime;
    
    Transform PlayerPos;
    Rigidbody2D rb2D;

    public EnemyBulletKind _enemyBulletKind;
    public enum EnemyBulletKind
    {
        normalBullet,
        followBullet
    }

    void Start()
    {
        PlayerPos = GameObject.Find("Player").GetComponent<Transform>();
        rb2D = GetComponent<Rigidbody2D>();

        if (_enemyBulletKind == EnemyBulletKind.normalBullet)
        {
            Vector2 axis = PlayerPos.position - transform.position;
            rb2D.velocity = axis.normalized * _bulletSpeed;
        }

        Invoke("Destroy", _bulletTime);
    }

    void Update()
    {
        //�ǔ��e�Ȃ�v���C���[��ǔ�����
        if (_enemyBulletKind == EnemyBulletKind.followBullet)
        {
            Vector2 axis = PlayerPos.position - transform.position;
            rb2D.velocity = axis.normalized * _bulletSpeed;

            transform.rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(axis.y, axis.x) * Mathf.Rad2Deg) - 90);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�ߐڍU���ɓ���������j�󂳂��
        if (collision.gameObject.CompareTag("Melee"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SpiritPowerIncrease(2);
            Destroy(gameObject);
            Debug.Log("�a�藎�Ƃ�");
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
