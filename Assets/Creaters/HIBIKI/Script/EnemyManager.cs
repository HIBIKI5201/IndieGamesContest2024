using System.Collections;
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
        BomberEnemy,
        ShootEnemyOne,
        ShootEnemyTwo
    }

    [Header("�̗͌n")]
    [SerializeField, Tooltip("�G�̍ő�̗�")]
    float _maxHealth = 100;
    [SerializeField, ReadOnly, Tooltip("���݂̗̑�")]
    float _currentHealth;
    [ReadOnly]
    public bool _moveActive;

    [SerializeField, Tooltip("���G����")]
    float _invincibleTime;
    float _invincibleTimer;

    [Header("�ړ��n")]
    [SerializeField, Tooltip("�ړ����x")]
    float _moveSpeed;

    [SerializeField, Tooltip("�f�b�h�]�[���̒��S�_")]
    float _deadZonePoint;
    [SerializeField, Tooltip("�f�b�h�]�[���͈̔�")]
    float _deadZoneWeight;

    [Tooltip("�����̃X�P�[��")]
    Vector2 _firstScale;

    [Header("���ʍU���n")]

    [SerializeField, Tooltip("�U���͈�")]
    float _attackRange;

    [Header("�{�}�[�U���n")]
    [SerializeField, Tooltip("�v���C���[�������̈ړ����x�㏸�{��")]
    float _bomberDushSpeed;

    [Tooltip("�����V�[�N�G���X�J�n")]
    bool _bomberExplosionActive;
    [SerializeField, Tooltip("��������܂ł̋���")]
    float _bomberDistanceToExplosion;
    [SerializeField, Tooltip("��������܂ł̎���")]
    float _bomberExplosionTime;
    [SerializeField, Tooltip("��������܂ł̃G�t�F�N�g��")]
    float _bomberExplosionEffectValue;

    [SerializeField, Tooltip("�����_���[�W")]
    float _explosionDamage;
    [SerializeField, Tooltip("�����͈�")]
    float _bomberExplosionRange;

    [Header("�V���[�^�[�U���n")]

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

    [SerializeField]
    ParticleSystem PS;

    void Start()
    {
        _currentHealth = _maxHealth;
        _moveActive = true;

        _deadZonePoint += Random.Range(-0.5f, 0.5f);

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
            //�v���C���[�Ɍ����ĕ���
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
            //�v���C���[���牓������
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
        else
        {
            _rigidbody2D.velocity = Vector2.zero;
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
        if (_invincibleTime + _invincibleTimer < Time.time)
        {
            if (collision.gameObject.CompareTag("Melee"))
            {
                _currentHealth -= _playerController._attackDamage;
                _playerController.SpiritPowerIncrease(1);

                HitDamage(_playerController._attackDamage * _playerController._skillOneBuffValue);
            }

            if (collision.gameObject.CompareTag("Bullet"))
            {
                BulletManager bulletManager = collision.GetComponent<BulletManager>();
                HitDamage(Mathf.Clamp(Vector2.Distance(bulletManager._firstPos, transform.position) / bulletManager.inBullet_bulletAttenuation * bulletManager.inBullet_bulletMaxDamage, bulletManager.inBullet_bulletMinDamage, bulletManager.inBullet_bulletMaxDamage) * _playerController._skillOneBuffValue);
            }
        }
    }

    IEnumerator Explosion()
    {
        for (int i = 0; i < _bomberExplosionTime; i++)
        {
            Debug.Log($"�����܂�{_bomberExplosionTime - i}�b");
            yield return new WaitForSeconds(1);
        }

        GameObject bullet = Instantiate(EnemyBullet, transform.position, Quaternion.identity);

        EnemyBulletManager bulletManager = bullet.GetComponent<EnemyBulletManager>();
        bulletManager._bulletDamage = _explosionDamage;
        bulletManager._explosionRange = _bomberExplosionRange;

        Dead();
    }

    IEnumerator Shoot()
    {
        PS.Play();

        yield return new WaitForSeconds(3);
        Vector2 axis = _player.transform.position - transform.position;
        GameObject bullet = Instantiate(EnemyBullet, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(axis.y, axis.x) * Mathf.Rad2Deg) - 90));

        EnemyBulletManager bulletManager = bullet.GetComponent<EnemyBulletManager>();
        bulletManager._bulletDamage = _bulletDamage;
        bulletManager._bulletSpeed = _bulletSpeed;

        switch (_enemyKind)
        {
            case EnemyKind.ShootEnemyOne:
                bulletManager._enemyBulletKind = EnemyBulletKind.normalBullet;
                break;

            case EnemyKind.ShootEnemyTwo:
                bulletManager._enemyBulletKind = EnemyBulletKind.followBullet;
                break;
        }
    }

    public void HitDamage(float damage)
    {
        if (_invincibleTime + _invincibleTimer < Time.time)
        {
            _invincibleTimer = Time.time;

            _currentHealth -= damage;
            Debug.Log($"�󂯂��_���[�W��{damage}\n���݂̗̑͂�{_currentHealth}");

            StartCoroutine(HitEffect());

            if (_currentHealth <= 0)
            {
                Dead();
            }
        }
    }

    IEnumerator HitEffect()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
    }

    void Dead()
    {
        if (_enemyKind == EnemyKind.BomberEnemy)
        {
            GameObject bullet = Instantiate(EnemyBullet, transform.position, Quaternion.identity);

            EnemyBulletManager bulletManager = bullet.GetComponent<EnemyBulletManager>();
            bulletManager._bulletDamage = _explosionDamage;
            bulletManager._explosionRange = _bomberExplosionRange;
        }

        SceneChanger.KillEnemy();
        Destroy(this.gameObject);
    }
}
