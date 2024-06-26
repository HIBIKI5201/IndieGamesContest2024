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

    [Header("�G�l�~�[�̊T�v")]
    public EnemyKind _enemyKind;
    public enum EnemyKind
    {
        MeleeEnemy,
        ShootEnemyOne,
        ShootEnemyTwo
    }

    [Header("�̗͌n")]
    [ReadOnly, Tooltip("�G�̍ő�̗�")]
    float _maxHealth;
    [SerializeField, ReadOnly, Tooltip("���݂̗̑�")]
    float _currentHealth;
    public bool _moveActive;

    [SerializeField]
    float _invincibleTime;
    float _invincibleTimer;

    [Header("�ړ��n")]
    [SerializeField, Tooltip("�ړ����x")]
    float _moveSpeed;
    [Tooltip("�����̃X�P�[��")]
    Vector2 _firstScale;

    [Header("�U���n")]
    [SerializeField]
    GameObject EnemyBullet;

    [SerializeField, Tooltip("�U���Ԋu")]
    float _attackInterval;
    [Tooltip("�U���Ԋu�̃^�C�}�[")]
    float _attackIntervalTimer;

    [SerializeField, Tooltip("�e�_���[�W")]
    float _bulletDamage;
    [SerializeField, Tooltip("�e�X�s�[�h")]
    float _bulletSpeed;
    [SerializeField, Tooltip("�U���͈�")]
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
            Debug.Log($"���݂̗̑͂�{_currentHealth}");

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
        Debug.Log("�G�t�F�N�g�𐶐�");

        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
    }
}
