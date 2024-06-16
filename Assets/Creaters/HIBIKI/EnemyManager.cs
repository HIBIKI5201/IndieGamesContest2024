using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    GameObject _player;
    PlayerController _playerController;

    [Header("エネミーの概要")]
    public EnemyKind _enemyKind;
    public enum EnemyKind
    {
        MeleeEnemy,
        ShootEnemy
    }

    [Header("体力系")]
    [ReadOnly,Tooltip("敵の最大体力")]
    public float _maxHealth;
    [SerializeField,ReadOnly,Tooltip("現在の体力")]
    float _currentHealth;
    public bool _moveActive;

    [Header("移動系")]
    [SerializeField, Tooltip("移動速度")]
    float _moveSpeed;
    void Start()
    {
        _currentHealth = _maxHealth;
        _moveActive = true;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _player = GameObject.Find("Player");
        _playerController = _player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_moveActive)
        {
            _spriteRenderer.color = Color.white;

            if (_player.transform.position.x - transform.position.x > 0)
            {
                _rigidbody2D.velocity = new Vector2(_moveSpeed, _rigidbody2D.velocity.y);
            } else
            {
                _rigidbody2D.velocity = new Vector2(-_moveSpeed, _rigidbody2D.velocity.y);
            }
        } else
        {
            _spriteRenderer.color = Color.red;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Melee"))
        {
            _currentHealth -= _playerController._attackDamage;

            Debug.Log($"現在の体力は{_currentHealth}");
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletManager bulletManager = collision.GetComponent<BulletManager>();
            _currentHealth -= Mathf.Clamp(Vector2.Distance(bulletManager.firstPos, transform.position) / bulletManager.inBullet_bulletAttenuation * bulletManager.inBullet_bulletMaxDamage, bulletManager.inBullet_bulletMinDamage, bulletManager.inBullet_bulletMaxDamage);

            Debug.Log(Mathf.Clamp(Vector2.Distance(bulletManager.firstPos, transform.position) / bulletManager.inBullet_bulletAttenuation * bulletManager.inBullet_bulletMaxDamage, bulletManager.inBullet_bulletMinDamage, bulletManager.inBullet_bulletMaxDamage));

            Debug.Log($"現在の体力は{_currentHealth}");
        }
    }
}
