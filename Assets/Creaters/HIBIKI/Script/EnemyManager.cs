using System.Collections;
using System.Threading;
using UnityEngine;
using static EnemyBulletManager;

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
        ShootEnemyOne,
        ShootEnemyTwo
    }

    [Header("体力系")]
    [ReadOnly, Tooltip("敵の最大体力")]
    float _maxHealth;
    [SerializeField, ReadOnly, Tooltip("現在の体力")]
    float _currentHealth;
    public bool _moveActive;

    [SerializeField]
    float _invincibleTime;
    float _invincibleTimer;

    [Header("移動系")]
    [SerializeField, Tooltip("移動速度")]
    float _moveSpeed;
    [Tooltip("初期のスケール")]
    Vector2 _firstScale;

    [Header("攻撃系")]
    [SerializeField]
    GameObject EnemyBullet;

    [SerializeField, Tooltip("攻撃間隔")]
    float _attackInterval;
    [Tooltip("攻撃間隔のタイマー")]
    float _attackIntervalTimer;

    [SerializeField, Tooltip("弾ダメージ")]
    float _bulletDamage;
    [SerializeField, Tooltip("弾スピード")]
    float _bulletSpeed;
    [SerializeField, Tooltip("攻撃範囲")]
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
            if (_player.transform.position.x - transform.position.x > 0)
            {
                _rigidbody2D.velocity = new Vector2(_moveSpeed, _rigidbody2D.velocity.y);
                transform.localScale = new Vector2(_firstScale.x, transform.localScale.y);
            }
            else
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

            if (_invincibleTime + _invincibleTimer < Time.time)
            {
                _playerController.SpiritPowerIncrease(5);
            }
 
            HitDamage(_playerController._attackDamage);
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletManager bulletManager = collision.GetComponent<BulletManager>();
            HitDamage(Mathf.Clamp(Vector2.Distance(bulletManager._firstPos, transform.position) / bulletManager.inBullet_bulletAttenuation * bulletManager.inBullet_bulletMaxDamage, bulletManager.inBullet_bulletMinDamage, bulletManager.inBullet_bulletMaxDamage));
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

    public void HitDamage(float damage)
    {
        if (_invincibleTime + _invincibleTimer < Time.time)
        {
            _invincibleTimer = Time.time;

            _currentHealth -= damage;
            Debug.Log($"現在の体力は{_currentHealth}");

            StartCoroutine(HitEffect());

            if (_currentHealth <= 0)
            {
                SceneChanger.KillEnemey();
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator HitEffect()
    {
        Debug.Log("エフェクトを生成");

        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
    }
}
