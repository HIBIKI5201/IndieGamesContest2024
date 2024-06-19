using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyBulletManager;

public class EnemyManager : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    GameObject _player;
    PlayerController _playerController;

    [Header("ÉGÉlÉ~Å[ÇÃäTóv")]
    public EnemyKind _enemyKind;
    public enum EnemyKind
    {
        MeleeEnemy,
        ShootEnemyOne,
        ShootEnemyTwo
    }

    [Header("ëÃóÕån")]
    [ReadOnly,Tooltip("ìGÇÃç≈ëÂëÃóÕ")]
    public float _maxHealth;
    [SerializeField,ReadOnly,Tooltip("åªç›ÇÃëÃóÕ")]
    float _currentHealth;
    public bool _moveActive;

    [Header("à⁄ìÆån")]
    [SerializeField, Tooltip("à⁄ìÆë¨ìx")]
    float _moveSpeed;

    [Header("çUåÇån")]
    [SerializeField]
    GameObject EnemyBullet;

    [SerializeField, Tooltip("çUåÇä‘äu")]
    float _attackInterval;
    [Tooltip("çUåÇä‘äuÇÃÉ^ÉCÉ}Å[")]
    float _attackIntervalTimer;

    [SerializeField, Tooltip("íeÉ_ÉÅÅ[ÉW")]
    float _bulletDamage;
    [SerializeField, Tooltip("íeÉXÉsÅ[Éh")]
    float _bulletSpeed;
    void Start()
    {
        _maxHealth = 500;

        _currentHealth = _maxHealth;
        _moveActive = true;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _player = GameObject.Find("Player");
        _playerController = _player.GetComponent<PlayerController>();

        _attackIntervalTimer = Time.time;
    }

    void Update()
    {
        if (_moveActive)
        {
            _spriteRenderer.color = Color.white;

            if (_player.transform.position.x - transform.position.x > 0)
            {
                _rigidbody2D.velocity = new Vector2(_moveSpeed, _rigidbody2D.velocity.y);
                transform.localScale = new Vector2(1, transform.localScale.y);
            } else
            {
                _rigidbody2D.velocity = new Vector2(-_moveSpeed, _rigidbody2D.velocity.y);
                transform.localScale = new Vector2(-1, transform.localScale.y);
            }
        } else
        {
            _spriteRenderer.color = Color.red;
        }

        if (_attackIntervalTimer + _attackInterval < Time.time)
        {
            GameObject bullet = Instantiate(EnemyBullet, transform.position, Quaternion.Euler(0, 0, -90 * Mathf.Sign(transform.localScale.x)));
            EnemyBulletManager bulletManager = bullet.GetComponent<EnemyBulletManager>();
            bulletManager._bulletDamage = _bulletDamage;

            if (_enemyKind == EnemyKind.ShootEnemyOne)
            {
                bulletManager._enemyBulletKind = EnemyBulletKind.normalBullet;
                bulletManager._bulletSpeed = _bulletSpeed * Mathf.Sign(transform.localScale.x);
            }
            else if (_enemyKind == EnemyKind.ShootEnemyTwo)
            {
                bulletManager._enemyBulletKind = EnemyBulletKind.followBullet;
            }
            else if (_enemyKind == EnemyKind.MeleeEnemy)
            {

            }

            _attackIntervalTimer = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Melee"))
        {
            _currentHealth -= _playerController._attackDamage;

            HitDamage(_playerController._attackDamage);

            Debug.Log($"åªç›ÇÃëÃóÕÇÕ{_currentHealth}");
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletManager bulletManager = collision.GetComponent<BulletManager>();
            HitDamage(Mathf.Clamp(Vector2.Distance(bulletManager.firstPos, transform.position) / bulletManager.inBullet_bulletAttenuation * bulletManager.inBullet_bulletMaxDamage, bulletManager.inBullet_bulletMinDamage, bulletManager.inBullet_bulletMaxDamage));



            Debug.Log(Mathf.Clamp(Vector2.Distance(bulletManager.firstPos, transform.position) / bulletManager.inBullet_bulletAttenuation * bulletManager.inBullet_bulletMaxDamage, bulletManager.inBullet_bulletMinDamage, bulletManager.inBullet_bulletMaxDamage));

            Debug.Log($"åªç›ÇÃëÃóÕÇÕ{_currentHealth}");
        }
    }

    void HitDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
