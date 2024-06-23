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

    [Header("�G�l�~�[�̊T�v")]
    public EnemyKind _enemyKind;
    public enum EnemyKind
    {
        MeleeEnemy,
        ShootEnemyOne,
        ShootEnemyTwo
    }

    [Header("�̗͌n")]
    [ReadOnly,Tooltip("�G�̍ő�̗�")]
    public float _maxHealth;
    [SerializeField,ReadOnly,Tooltip("���݂̗̑�")]
    float _currentHealth;
    public bool _moveActive;

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
                transform.localScale = new Vector2(1, transform.localScale.y);
            } else
            {
                _rigidbody2D.velocity = new Vector2(-_moveSpeed, _rigidbody2D.velocity.y);
                transform.localScale = new Vector2(-1, transform.localScale.y);
            }

            if (_enemyKind == EnemyKind.MeleeEnemy)
            {

            }
        }

        if (_attackIntervalTimer + _attackInterval < Time.time && (_enemyKind == EnemyKind.ShootEnemyOne || _enemyKind == EnemyKind.ShootEnemyTwo))
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

            Debug.Log($"���݂̗̑͂�{_currentHealth}");
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletManager bulletManager = collision.GetComponent<BulletManager>();
            HitDamage(Mathf.Clamp(Vector2.Distance(bulletManager._firstPos, transform.position) / bulletManager.inBullet_bulletAttenuation * bulletManager.inBullet_bulletMaxDamage, bulletManager.inBullet_bulletMinDamage, bulletManager.inBullet_bulletMaxDamage));

            Debug.Log($"���݂̗̑͂�{_currentHealth}");
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
