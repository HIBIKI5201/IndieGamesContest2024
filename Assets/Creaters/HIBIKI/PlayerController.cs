using System.Collections;
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

    [Header("体力ステータス")]
    [SerializeField, Tooltip("プレイヤーの最大ヘルス")]
    float _maxHealth;
    [SerializeField, ReadOnly, Tooltip("現在のヘルス")]
    float _currentHealth;

    [Header("移動ステータス")]
    [SerializeField]
    Rigidbody2D PlayerRigidBody;
    [Tooltip("重力の初期値")]
    float _gravity;
    [SerializeField,ReadOnly,Tooltip("地面に付いているかの判定")]
    bool _isGround;

    [SerializeField, Tooltip("移動速度")]
    float _moveSpeed;

    [SerializeField, Tooltip("ジャンプ力")]
    float _jumpPower;
    [SerializeField, Tooltip("ジャンプ入力の長押し最長時間")]
    float _jumpMaxTime;
    [Tooltip("ジャンプ入力のタイマー")]
    float _jumpTimer;
    [SerializeField, Tooltip("ジャンプ長押し入力中の重力低下量")]
    float _jumpGravity;


    [SerializeField,ReadOnly,Tooltip("ジャンプが可能な状態か")]
    bool _canJump;
    [Tooltip("ジャンプの最初の処理")]
    bool firstJump;
    [Tooltip("壁に当たっている時の法線水平方向")]
    float _wallTouch;

    [SerializeField, ReadOnly, Tooltip("移動可能な状態か")]
    bool _moveActive = true;

    [Header("攻撃ステータス")]
    [SerializeField, Tooltip("近接判定を持つオブジェクト")]
    GameObject AttackRangeObject;

    [SerializeField, Tooltip("次の攻撃を行えるまでのリキャストタイム")]
    float _attackSpeed;
    [Tooltip("攻撃リキャストタイムのタイマー")]
    float _attackTimer;

    [Tooltip("近接のダメージ")]
    public float _attackDamage;

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

    [SerializeField, Tooltip("弾丸の最大ダメージ")]
    float _bulletMaxDamage;
    [SerializeField, Tooltip("弾丸の最小ダメージ")]
    float _bulletMinDamage;
    [SerializeField, Tooltip("弾丸の距離減衰")]
    float _bulletAttenuation;

    [Header("スキル")]

    [SerializeField, Tooltip("朱雀スキルクールタイム")]
    float _skillOneCT;
    [Tooltip("朱雀スキルクールタイムタイマー")]
    float _skillOneCTtimer;

    [SerializeField, Tooltip("朱雀スキルのオブジェクト")]
    GameObject _skillOneObjecct;
    [SerializeField, Tooltip("朱雀スキルのチャージ時間")]
    float _skillOneChargeTime;

    [SerializeField, Tooltip("青龍のスキル効果時間")]
    float _skillOneDuration;
    [SerializeField, Tooltip("朱雀スキルの攻撃範囲")]
    float _skillOneRange;
    [SerializeField, Tooltip("炎ダメージを与える間隔")]
    float _skillOneFireInterval;

    [Space]

    [SerializeField, Tooltip("白虎スキルクールタイム")]
    float _skillTwoCT;
    [Tooltip("白虎スキルクールタイムタイマー")]
    float _skillTwoCTtimer;

    [SerializeField, Tooltip("白虎スキルのダッシュ速度")]
    float _skillTwoDashSpeed;
    [SerializeField, Tooltip("白虎スキル発動時に何秒間操作不能にするか")]
    float _skillTwoWaitTime;

    [Space]

    [SerializeField, Tooltip("青龍スキルクールタイム")]
    float _skillThreeCT;
    [Tooltip("青龍スキルクールタイムタイマー")]
    float _skillThreeCTtimer;

    [SerializeField, Tooltip("回復トーテムのオブジェクト")]
    GameObject SkillThreeObject;
    [SerializeField, Tooltip("青龍のスキル効果時間")]
    float _skillThreeDuration;

    [Space]

    [SerializeField, Tooltip("玄武スキルクールタイム")]
    float _skillFourCT;
    [Tooltip("玄武スキルクールタイムタイマー")]
    float _skillFourCTtimer;

    [SerializeField, Tooltip("玄武スキルの拘束時間")]
    float _skillFourRestraintTime;
    [SerializeField, Tooltip("玄武スキルのシールド維持時間")]
    float _skillFourShieldTime;
    [SerializeField, Tooltip("玄武スキルの効果範囲")]
    float _skillFourRange;

    void Start()
    {
        _playerMode = PlayerMode.Sun;
        _currentHealth = _maxHealth;
        _moveActive = true;
        _canJump = true;

        _gravity = PlayerRigidBody.gravityScale;
    }

    void Update()
    {

        
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (_moveActive)
        {
            //移動系
            #region
            if (_wallTouch == 0 || _wallTouch == horizontal)
            {
                PlayerRigidBody.velocity = new Vector2(horizontal * _moveSpeed, PlayerRigidBody.velocity.y);
            }

            if (horizontal != 0)
            {
                transform.localScale = new Vector2(horizontal, transform.localScale.y);
            }


            if (Input.GetKeyDown(KeyCode.W) && _canJump)
            {
                PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, 0);
                PlayerRigidBody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);

                PlayerRigidBody.gravityScale = _gravity - _jumpGravity;
            }
            if (Input.GetKey(KeyCode.W))
            {
                _jumpTimer += Time.deltaTime;

                if (_jumpTimer > _jumpMaxTime && !_isGround) 
                {
                    _canJump = false;
                    PlayerRigidBody.gravityScale = _gravity;
                }
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                if (!_isGround)
                {
                    _canJump = false;
                    PlayerRigidBody.gravityScale = _gravity;
                }
            }
            
            #endregion

            //攻撃系
            #region
            //通常攻撃
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(Attack());
            }
            #endregion

            //スキル系
            #region
            //陰陽切り替え
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

            //スキル１(Zキー)
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (_playerMode == PlayerMode.Sun && _skillOneCT + _skillOneCTtimer < Time.time)
                {
                    StartCoroutine(SkillOne());
                }
                else if (_playerMode == PlayerMode.Moon && _skillThreeCT + _skillThreeCTtimer < Time.time)
                {
                    SkillThree();
                }
                else
                {
                    Debug.Log("スキル１ リキャストタイム中");
                }
            }

            //スキル２(Xキー)
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (_playerMode == PlayerMode.Sun && _skillTwoCT + _skillTwoCTtimer < Time.time)
                {
                    StartCoroutine(SkillTwo(horizontal));

                }
                else if (_playerMode == PlayerMode.Moon && _skillFourCT + _skillFourCTtimer < Time.time)
                {
                    StartCoroutine(SkillFour());
                }
                else
                {
                    Debug.Log("スキル２ リキャストタイム中");
                }
            }
            #endregion
        }
    }

    //当たり判定処理
    #region
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //地面に当たったらジャンプ回数を回復
        //地面の側面に当たったら反発する
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.contacts[0].normal.y > 0.8f)
            {
                _isGround = true;
                _canJump = true;
                _jumpTimer = 0;
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
            _isGround = false;
            _wallTouch = 0;
        }
    }
    #endregion


    IEnumerator Attack()
    {
        //近接攻撃
        if (_playerMode == PlayerMode.Sun && _attackTimer + _attackSpeed < Time.time)
        {
            _attackTimer = Time.time;
            Debug.Log("近接攻撃発動");

            //近接攻撃当たり判定を出現
            AttackRangeObject.SetActive(true);

            yield return new WaitForSeconds(0.05f);

            AttackRangeObject.SetActive(false);
        }
        //遠距離攻撃
        else if (_playerMode == PlayerMode.Moon && _fireTimer + _fireSpeed < Time.time)
        {
            _fireTimer = Time.time;
            Debug.Log("遠距離攻撃発動");

            //弾丸を発射
            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, -90 * Mathf.Sign(transform.localScale.x)));

            BulletManager bulletManager = bullet.GetComponent<BulletManager>();
            bulletManager.inBullet_bulletDestroyTime = _bulletDestroyTime;
            bulletManager.inBullet_bulletMaxDamage = _bulletMaxDamage;
            bulletManager.inBullet_bulletMinDamage = _bulletMinDamage;
            bulletManager.inBullet_bulletAttenuation = _bulletAttenuation;
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

        _moveActive = false;
        PlayerRigidBody.gravityScale += 1f;

        //攻撃チャージ時間
        yield return new WaitForSeconds(_skillOneChargeTime);

        _moveActive = true;
        PlayerRigidBody.gravityScale = _gravity;

        //攻撃範囲 兼 炎のオブジェクトを配置
        GameObject skillObject = Instantiate(_skillOneObjecct, transform.position + new Vector3(_skillOneRange / 2 * Mathf.Sign(transform.localScale.x), 0, 0), Quaternion.identity);

        SkillOneManager skillManager = skillObject.GetComponent<SkillOneManager>();
        skillManager._skillOneDuration = _skillOneDuration;
        skillManager._fireTime = _skillOneFireInterval;
        skillObject.transform.localScale = new Vector3(_skillOneRange, 1, 1);


    }

    IEnumerator SkillTwo(float horizontal)
    {
        _skillTwoCTtimer = Time.time;
        Debug.Log("白虎スキル発動");

        _moveActive = false;

        //進行方向に向けて加速
        PlayerRigidBody.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * _skillTwoDashSpeed, 0);
        PlayerRigidBody.gravityScale = 0.5f;

        yield return new WaitForSeconds(_skillTwoWaitTime);

        PlayerRigidBody.gravityScale = _gravity;
        _moveActive = true;
    }

    void SkillThree()
    {
        _skillThreeCTtimer = Time.time;
        Debug.Log("青龍スキル発動");

        //スキルのオブジェクトを出現させる
        GameObject skillObject = Instantiate(SkillThreeObject, transform.position, Quaternion.identity);
        skillObject.GetComponent<SkillThreeManager>()._skillThreeDuration = _skillThreeDuration;
    }

    IEnumerator SkillFour()
    {
        _skillFourCTtimer = Time.time;
        Debug.Log("玄武スキル発動");

        //敵を全て取得し処理
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");

        Debug.Log($"玄武スキルに当たった敵は{enemy.Length}体");

        foreach (GameObject obj in enemy)
        {
            if (Vector2.Distance(obj.transform.position, transform.position) < _skillFourRange)
            {
                obj.GetComponent<EnemyManager>()._moveActive = false;
            } else
            {
                Debug.Log($"{obj.gameObject.name}は効果範囲外");
            }
        }

        //効果時間終了の処理
        if (_skillFourRestraintTime < _skillFourShieldTime)
        {
            yield return new WaitForSeconds(_skillFourRestraintTime);

            Debug.Log("玄武スキルの拘束時間終了");
            foreach (GameObject obj in enemy)
            {
                obj.GetComponent<EnemyManager>()._moveActive = true;
            }

            yield return new WaitForSeconds(_skillFourShieldTime - _skillFourRestraintTime);

            Debug.Log("玄武スキルのシールド維持時間終了");
        }
        else
        {
            yield return new WaitForSeconds(_skillFourShieldTime);

            Debug.Log("玄武スキルのシールド維持時間終了");

            yield return new WaitForSeconds(_skillFourRestraintTime - _skillFourShieldTime);

            Debug.Log("玄武スキルの拘束時間終了");

            foreach (GameObject obj in enemy)
            {
                obj.GetComponent<EnemyManager>()._moveActive = true;
            }
        }
    }

}
