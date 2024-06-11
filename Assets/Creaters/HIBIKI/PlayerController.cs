using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤー")]
    [Tooltip("陰陽の形態")]
    public static PlayerMode _playerMode;
    public enum PlayerMode
    {
        Sun,
        Moon
    }

    [Header("移動ステータス")]
    [SerializeField]
    Rigidbody2D PlayerRigidBody;

    [SerializeField,Tooltip("移動速度")]
    float _moveSpeed;

    [SerializeField,Tooltip("ジャンプ力")]
    float _jumpPower;
    [Tooltip("接地していてジャンプが可能かの判定")]
    bool _groundJump;
    [Tooltip("壁に当たっている時の法線水平方向")]
    float _wallTouch;

    [Header("攻撃ステータス")]
    [SerializeField, Tooltip("近接判定を持つオブジェクト")]
    GameObject AttackRangeObject;

    [SerializeField, Tooltip("次の攻撃を行えるまでのリキャストタイム")]
    float _attackSpeed;
    [Tooltip("攻撃リキャストタイムのタイマー")]
    float _attackTimer;

    [Space]

    [SerializeField, Tooltip("発射する弾丸プレハブ")]
    GameObject Bullet;

    [SerializeField, Tooltip("次の射撃を行えるまでのリキャストタイム")]
    float _fireSpeed;
    [Tooltip("射撃リキャストタイマー")]
    float _fireTimer;

    [SerializeField, Tooltip("弾丸のスピード")]
    float _bulletVelocity;
    [SerializeField, Tooltip("弾丸が消滅するまでの時間")]
    float _bulletDestroyTime;

    [Header("スキル")]

    [SerializeField, Tooltip("朱雀スキルクールタイム")]
    float _skillOneCT;
    [Tooltip("朱雀スキルクールタイムタイマー")]
    float _skillOneCTtimer;

    [Space]

    [SerializeField, Tooltip("白虎スキルクールタイム")]
    float _skillTwoCT;
    [Tooltip("白虎スキルクールタイムタイマー")]
    float _skillTwoCTtimer;

    [SerializeField, Tooltip("白虎スキルのダッシュ速度")]
    float _skillTwoDashSpeed;
    [SerializeField, Tooltip("白虎スキル発動時に何秒間操作不能にするか")]
    float _skillTwoWaitTime;
    [Tooltip("白虎スキルが発動されているか")]
    bool _skillTwoActive;

    void Start()
    {
        _playerMode = PlayerMode.Sun;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (!_skillTwoActive)
        {
            if (_wallTouch  == 0 || _wallTouch == horizontal)
            {
                PlayerRigidBody.velocity = new Vector2(horizontal * _moveSpeed, PlayerRigidBody.velocity.y);
            }

            if (horizontal != 0)
            {
                transform.localScale = new Vector2(horizontal, transform.localScale.y);
            }
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
                Debug.Log("陰形態に変形");
            }
            else
            {
                _playerMode = PlayerMode.Sun;
                Debug.Log("陽形態に変形");
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (_playerMode == PlayerMode.Sun && _skillOneCT + _skillOneCTtimer < Time.time)
            {
                StartCoroutine(SkillOne());
            }
            else if (_playerMode == PlayerMode.Moon)
            {
                StartCoroutine(SkillThree());
            }
            else
            {
                Debug.Log("スキル１ リキャストタイム中");
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (_playerMode == PlayerMode.Sun && _skillTwoCT + _skillTwoCTtimer < Time.time)
            {
                StartCoroutine(SkillTwo(horizontal));
                
            }
            else if (_playerMode == PlayerMode.Moon)
            {
                StartCoroutine(SkillFour());
            }
            else
            {
                Debug.Log("スキル２ リキャストタイム中");
            }
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
            Debug.Log("近接攻撃発動");

            AttackRangeObject.SetActive(true);
            
            yield return new WaitForSeconds(0.05f);
            
            AttackRangeObject.SetActive(false);
        }
        else if (_playerMode == PlayerMode.Moon && _fireTimer + _fireSpeed < Time.time)
        {
            _fireTimer = Time.time;
            Debug.Log("遠距離攻撃発動");

            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, -90 * Mathf.Sign(transform.localScale.x)));
            
            bullet.GetComponent<BulletManager>().inBullet_bulletDestroyTime = _bulletDestroyTime;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletVelocity * Mathf.Sign(transform.localScale.x), 0);
        }
        else
        {
            Debug.Log("通常攻撃 リキャストタイム中");
        }
    }

    IEnumerator SkillOne()
    {
        _skillOneCTtimer = Time.time;
        Debug.Log("朱雀スキル発動");
        return null;
    }

    IEnumerator SkillTwo(float horizontal)
    {
        _skillTwoCTtimer = Time.time;
        Debug.Log("白虎スキル発動");
        _skillTwoActive = true;
        PlayerRigidBody.velocity = new Vector2(horizontal * _skillTwoDashSpeed, 0);

        yield return new WaitForSeconds(_skillTwoWaitTime);

        _skillTwoActive = false;
    }

    IEnumerator SkillThree()
    {
        Debug.Log("青龍スキル発動");

        return null;
    }

    IEnumerator SkillFour()
    {
        Debug.Log("玄武スキル発動");

        return null;
    }
}
