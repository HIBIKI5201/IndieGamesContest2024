using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[")]
    [Tooltip("�A�z�̌`��")]
    public static PlayerMode _playerMode;
    public enum PlayerMode
    {
        Sun,
        Moon
    }

    [Header("�ړ��X�e�[�^�X")]
    [SerializeField]
    Rigidbody2D PlayerRigidBody;

    [SerializeField,Tooltip("�ړ����x")]
    float _moveSpeed;

    [SerializeField,Tooltip("�W�����v��")]
    float _jumpPower;
    [Tooltip("�ڒn���Ă��ăW�����v���\���̔���")]
    bool _groundJump;
    [SerializeField,Tooltip("�ǂɓ������Ă��鎞�̖@����������")]
    float _wallTouch;

    [Header("�U���X�e�[�^�X")]
    [SerializeField, Tooltip("�ߐڔ���")]
    GameObject AttackRangeObject;

    [SerializeField, Tooltip("���̍U�����s����܂ł̃��L���X�g�^�C��")]
    float _attackSpeed;
    [Tooltip("�U�����L���X�g�^�C���̃^�C�}�[")]
    float _attackTimer;

    [Space]

    [SerializeField, Tooltip("���˂���e�ۃv���n�u")]
    GameObject Bullet;
    [SerializeField, Tooltip("���̎ˌ����s����܂ł̃��L���X�g�^�C��")]
    float _fireSpeed;
    [Tooltip("�ˌ����L���X�g�^�C�}�[")]
    float _fireTimer;
    [SerializeField, Tooltip("�e�ۂ̃X�s�[�h")]
    float _bulletVelocity;
    [SerializeField, Tooltip("�e�ۂ����ł���܂ł̎���")]
    float _bulletDestroyTime = 5;

    void Start()
    {
        _playerMode = PlayerMode.Sun;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (_wallTouch  == 0 || _wallTouch == horizontal)
        {
            PlayerRigidBody.velocity = new Vector2(horizontal * _moveSpeed, PlayerRigidBody.velocity.y);
        }

        if (horizontal != 0)
        {
            transform.localScale = new Vector2(horizontal, transform.localScale.y);
        }
        
        
        if (Input.GetKeyDown(KeyCode.W) && _groundJump)
        {
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, 0);
            PlayerRigidBody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (_playerMode == PlayerMode.Sun)
            {
                _playerMode = PlayerMode.Moon;
                Debug.Log("�A�`�Ԃɕό`");
            }
            else
            {
                _playerMode = PlayerMode.Sun;
                Debug.Log("�z�`�Ԃɕό`");
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Attack());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.contacts[0].normal.y > 0.8f)
            {
                _groundJump = true;
                _wallTouch = 0;
            }
            if (Mathf.Abs(collision.contacts[0].normal.x) > 0.8f)
            {
                _wallTouch = Mathf.Sign(collision.contacts[0].normal.x);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _groundJump = false;
            _wallTouch = 0;
        }
    }

    IEnumerator Attack()
    {
        if (_playerMode == PlayerMode.Sun && _attackTimer + _attackSpeed < Time.time)
        {
            _attackTimer = Time.time;
            Debug.Log("�ߐڍU������");

            AttackRangeObject.SetActive(true);
            
            yield return new WaitForSeconds(0.05f);
            
            AttackRangeObject.SetActive(false);
        }
        else if (_playerMode == PlayerMode.Moon)
        {
            Debug.Log("�������U������");
            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, -90 * Mathf.Sign(transform.localScale.x)));
            bullet.GetComponent<BulletManager>().inBullet_bulletDestroyTime = _bulletDestroyTime;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletVelocity * Mathf.Sign(transform.localScale.x), 0);
        }
        else
        {
            Debug.Log("�ʏ�U�� ���L���X�g�^�C����");
        }
    }
}
