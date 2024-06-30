using System.Collections;
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
        BomberEnemy,
        ShootEnemyOne,
        ShootEnemyTwo
    }

    [Header("体力系")]
    [SerializeField, Tooltip("敵の最大体力")]
    float _maxHealth = 100;
    [SerializeField, ReadOnly, Tooltip("現在の体力")]
    float _currentHealth;
    [ReadOnly]
    public bool _moveActive;

    [SerializeField, Tooltip("無敵時間")]
    float _invincibleTime;
    float _invincibleTimer;

    [Header("移動系")]
    [SerializeField, Tooltip("移動速度")]
    float _moveSpeed;

    [SerializeField, Tooltip("デッドゾーンの中心点")]
    float _deadZonePoint;
    [SerializeField, Tooltip("デッドゾーンの範囲")]
    float _deadZoneWeight;

    [Tooltip("初期のスケール")]
    Vector2 _firstScale;

    [Header("共通攻撃系")]

    [SerializeField, Tooltip("攻撃範囲")]
    float _attackRange;

    [Header("ボマー攻撃系")]
    [SerializeField, Tooltip("プレイヤー発見時の移動速度上昇倍率")]
    float _bomberDushSpeed;

    [Tooltip("自爆シークエンス開始")]
    bool _bomberExplosionActive;
    [SerializeField, Tooltip("自爆するまでの距離")]
    float _bomberDistanceToExplosion;
    [SerializeField, Tooltip("自爆するまでの時間")]
    float _bomberExplosionTime;

    [Header("シューター攻撃系")]

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

    [SerializeField]
    ParticleSystem PS;

    void Start()
    {
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
            //プレイヤーに向けて歩く
            if (Vector2.Distance(transform.position, _player.transform.position) > _deadZonePoint + _deadZoneWeight / 2)
            {
                if (_enemyKind == EnemyKind.BomberEnemy && Vector2.Distance(transform.position, _player.transform.position) < _attackRange)
                {
                    _rigidbody2D.velocity = new Vector2(_moveSpeed * _bomberDushSpeed * Mathf.Sign(_player.transform.position.x - transform.position.x), _rigidbody2D.velocity.y);
                }
                else
                {
                    _rigidbody2D.velocity = new Vector2(_moveSpeed * Mathf.Sign(_player.transform.position.x - transform.position.x), _rigidbody2D.velocity.y);
                }
            }
            //プレイヤーから遠ざかる
            else if (Vector2.Distance(transform.position, _player.transform.position) < _deadZonePoint - _deadZoneWeight / 2)
            {
                _rigidbody2D.velocity = new Vector2(_moveSpeed * -1 * Mathf.Sign(_player.transform.position.x - transform.position.x), _rigidbody2D.velocity.y);
            }
            else
            {
                _rigidbody2D.velocity = Vector2.zero;
            }

            transform.localScale = new Vector2(_firstScale.x * Mathf.Sign(_player.transform.position.x - transform.position.x), transform.localScale.y);
        }

        if (_enemyKind == EnemyKind.BomberEnemy && Vector2.Distance(transform.position, _player.transform.position) < _bomberDistanceToExplosion && !_bomberExplosionActive)
        {
            _bomberExplosionActive = true;
            StartCoroutine(Explosion());
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

    IEnumerator Explosion()
    {
        for (int i = 0; i < _bomberExplosionTime; i++)
        {
            Debug.Log($"爆発まで{_bomberExplosionTime - i}秒");
            yield return new WaitForSeconds(1);
        }

        Destroy(gameObject);
    }

    IEnumerator Shoot()
    {
        PS.Play();

        yield return new WaitForSeconds(3);
        Vector2 axis = _player.transform.position - transform.position;
        GameObject bullet = Instantiate(EnemyBullet, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(axis.y, axis.x) * Mathf.Rad2Deg) - 90));
        EnemyBulletManager bulletManager = bullet.GetComponent<EnemyBulletManager>();
        bulletManager._bulletDamage = _bulletDamage;

        if (_enemyKind == EnemyKind.ShootEnemyOne)
        {
            bulletManager._enemyBulletKind = EnemyBulletKind.normalBullet;
            bulletManager._bulletSpeed = _bulletSpeed;
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
            Debug.Log($"受けたダメージは{damage}\n現在の体力は{_currentHealth}");

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
