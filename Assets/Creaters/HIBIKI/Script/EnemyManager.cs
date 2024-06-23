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

    [Header("ƒGƒlƒ~[‚ÌŠT—v")]
    public EnemyKind _enemyKind;
    public enum EnemyKind
    {
        MeleeEnemy,
        ShootEnemyOne,
        ShootEnemyTwo
    }

    [Header("‘Ì—ÍŒn")]
    [ReadOnly,Tooltip("“G‚ÌÅ‘å‘Ì—Í")]
    float _maxHealth;
    [SerializeField,ReadOnly,Tooltip("Œ»İ‚Ì‘Ì—Í")]
    float _currentHealth;
    public bool _moveActive;

    [Header("ˆÚ“®Œn")]
    [SerializeField, Tooltip("ˆÚ“®‘¬“x")]
    float _moveSpeed;
    [Tooltip("‰Šú‚ÌƒXƒP[ƒ‹")]
    Vector2 _firstScale;

    [Header("UŒ‚Œn")]
    [SerializeField]
    GameObject EnemyBullet;

    [SerializeField, Tooltip("UŒ‚ŠÔŠu")]
    float _attackInterval;
    [Tooltip("UŒ‚ŠÔŠu‚Ìƒ^ƒCƒ}[")]
    float _attackIntervalTimer;

    [SerializeField, Tooltip("’eƒ_ƒ[ƒW")]
    float _bulletDamage;
    [SerializeField, Tooltip("’eƒXƒs[ƒh")]
    float _bulletSpeed;
    [SerializeField, Tooltip("UŒ‚”ÍˆÍ")]
    float _attackRange;

    [SerializeField]
    ParticleSystem PS;
    void Start()
    {
        _maxHealth = 500;

        _currentHealth = _maxHealth;
        _moveActive = true;

        _firstScale = transform.localScale;

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
                transform.localScale = new Vector2(_firstScale.x, transform.localScale.y);
            } else
            {
                _rigidbody2D.velocity = new Vector2(-_moveSpeed, _rigidbody2D.velocity.y);
                transform.localScale = new Vector2(-_firstScale.x, transform.localScale.y);
            }

            if (_enemyKind == EnemyKind.MeleeEnemy)
            {

            }
        }

        if ((_enemyKind == EnemyKind.ShootEnemyOne || _enemyKind == EnemyKind.ShootEnemyTwo) &&
            _attackIntervalTimer + _attackInterval < Time.time &&
            Vector2.Distance(transform.position, _player.transform.position) < _attackRange)
        {
            _attackIntervalTimer = Time.time;
            StartCoroutine(Shoot());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Melee"))
        {
            _currentHealth -= _playerController._attackDamage;

            HitDamage(_playerController._attackDamage);

            Debug.Log($"Œ»İ‚Ì‘Ì—Í‚Í{_currentHealth}");
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletManager bulletManager = collision.GetComponent<BulletManager>();
            HitDamage(Mathf.Clamp(Vector2.Distance(bulletManager._firstPos, transform.position) / bulletManager.inBullet_bulletAttenuation * bulletManager.inBullet_bulletMaxDamage, bulletManager.inBullet_bulletMinDamage, bulletManager.inBullet_bulletMaxDamage));

            Debug.Log($"Œ»İ‚Ì‘Ì—Í‚Í{_currentHealth}");
        }
    }

    IEnumerator Shoot()
    {
            PS.Play();

        yield return new WaitForSeconds(3);

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
