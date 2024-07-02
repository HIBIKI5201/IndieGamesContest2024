using DG.Tweening;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤー")]
    [ReadOnly, Tooltip("陰陽の形態")]
    public PlayerMode _playerMode;
    public enum PlayerMode
    {
        Sun,
        Moon
    }
    [SerializeField, Tooltip("陰陽切り替えのクールタイム")]
    float _modeChangeInterval = 1;

    [Tooltip("陰陽変形のタイマー")]
    float _modeTimer;

    [Space(10)]

    [SerializeField]
    SpriteRenderer _spriteRenderer;


    [Header("バフ・デバフ")]
    [SerializeField, ReadOnly, Tooltip("朱雀スキルの攻撃力上昇バフの有無")]
    bool _skillOneBuffActive;
    [Tooltip("朱雀スキルのバフの残り時間")]
    float _skillOneBuffTimer;

    [ReadOnly, Tooltip("朱雀スキルの効果量")]
    public float _skillOneBuffValue;

    [Space(10)]

    [SerializeField]
    GameObject _skillOneBuffIcon;
    [SerializeField]
    TextMeshProUGUI _skillOneBuffText;
    [SerializeField]
    TextMeshProUGUI _skillOneBuffTimerText;

    [Space(20)]

    [SerializeField, ReadOnly, Tooltip("玄武スキルのシールドの有無")]
    bool _skillFourBuffActive;
    [Tooltip("玄武スキルのバフの残り時間")]
    float _skillFourBuffTimer;

    [SerializeField, Tooltip("玄武スキルのシールド量")]
    float _skillFourShieldQuantity;

    [SerializeField]
    Image ShieldGauge;

    [Space(10)]

    [SerializeField]
    GameObject _skillFourBuffIcon;
    [SerializeField]
    TextMeshProUGUI _skillFourBuffText;
    [SerializeField]
    TextMeshProUGUI _skillFourBuffTimerText;


    [Header("体力ステータス")]
    [SerializeField, Tooltip("プレイヤーの最大ヘルス")]
    float _maxHealth = 1500;
    [SerializeField, ReadOnly, Tooltip("現在のヘルス")]
    float _currentHealth;

    [Space(10)]

    [SerializeField, Tooltip("プレイヤーの最大妖力")]
    float _maxSpiritPower = 1500;
    [SerializeField, ReadOnly, Tooltip("現在の妖力")]
    float _currentSpiritPower;

    [SerializeField, ReadOnly, Tooltip("無敵時間")]
    bool _invincibleActive;

    [Space(10)]

    [SerializeField]
    Image HealthGauge;
    [SerializeField]
    Image SpiritGauge;


    [Header("移動ステータス")]
    Rigidbody2D PlayerRigidBody;
    [Tooltip("重力の初期値")]
    float _gravity;
    [SerializeField, ReadOnly, Tooltip("地面に付いているかの判定")]
    bool _isGround;

    [Space(20)]

    [SerializeField, Tooltip("移動速度")]
    float _moveSpeed = 5;

    [SerializeField, Tooltip("ジャンプ力")]
    float _jumpPower = 8;
    [SerializeField, Tooltip("ジャンプ入力の長押し最長時間")]
    float _jumpMaxTime = 0.5f;
    [Tooltip("ジャンプ入力のタイマー")]
    float _jumpTimer;
    [SerializeField, Tooltip("ジャンプ長押し入力中の重力低下量")]
    float _jumpGravity = 0.8f;

    [SerializeField, ReadOnly, Tooltip("ジャンプが可能な状態か")]
    bool _canJump;
    [Tooltip("壁に当たっている時の法線水平方向")]
    float _wallTouch;
    [Tooltip("最後に当たったGroundの法線方向")]
    Vector2 _lastNormal;

    [SerializeField, ReadOnly, Tooltip("移動可能な状態か")]
    bool _moveActive = true;

    [SerializeField, Tooltip("初期の大きさ")]
    Vector2 _firstScale;


    [Header("攻撃ステータス")]
    [SerializeField, Tooltip("通常攻撃を行っているか")]
    bool _attackActive;

    [SerializeField, Tooltip("近接判定を持つオブジェクト")]
    GameObject AttackRangeObject;

    [SerializeField, Tooltip("次の攻撃を行えるまでのリキャストタイム")]
    float _attackSpeed = 0.25f;
    [Tooltip("攻撃リキャストタイムのタイマー")]
    float _attackTimer;

    [Tooltip("近接のダメージ")]
    public float _attackDamage = 50;

    [Space(20)]

    [SerializeField, Tooltip("発射する弾丸プレハブ")]
    GameObject Bullet;
    [SerializeField, Tooltip("弾丸が発射される地点のオフセット")]
    Vector2 _bulletFirePosOffset;

    [SerializeField, Tooltip("次の射撃を行えるまでのリキャストタイム")]
    float _fireSpeed = 0.1f;
    [Tooltip("射撃リキャストタイマー")]
    float _fireTimer;

    [Space(10)]

    [SerializeField, Tooltip("弾丸のスピード")]
    float _bulletVelocity = 10;
    [SerializeField, Tooltip("弾丸が消滅するまでの時間")]
    float _bulletDestroyTime = 10;

    [Space(10)]

    [SerializeField, Tooltip("弾丸の最大ダメージ")]
    float _bulletMaxDamage = 100;
    [SerializeField, Tooltip("弾丸の最小ダメージ")]
    float _bulletMinDamage = 30;
    [SerializeField, Tooltip("弾丸の距離減衰")]
    float _bulletAttenuation = 5;


    [Header("スキル")]

    [Tooltip("スキルが発動しているか")]
    bool _skillActive;

    [SerializeField, Tooltip("朱雀スキルクールタイム")]
    float _skillOneCT = 10;
    [Tooltip("朱雀スキルクールタイムタイマー")]
    float _skillOneCTimer;
    [SerializeField]
    Image SkillOneIcon;
    [SerializeField]
    Image SkillOneIconGauge;

    [Space(10)]

    [SerializeField, Tooltip("朱雀スキルのオブジェクト")]
    GameObject _skillOneObject;
    [SerializeField, Tooltip("朱雀スキルのチャージ時間")]
    float _skillOneChargeTime = 1;
    [SerializeField, Tooltip("発動時の初撃のダメージ量")]
    float _skillOneAttackDamage = 120;

    [Space(10)]

    [SerializeField, Tooltip("朱雀のスキル効果時間")]
    float _skillOneDuration = 5;
    [SerializeField, Tooltip("朱雀スキルの攻撃範囲")]
    float _skillOneRange = 3;
    [SerializeField, Tooltip("炎ダメージを与える間隔")]
    float _skillOneFireInterval = 0.2f;

    [SerializeField, Tooltip("朱雀スキルのバフ効果時間")]
    float _skillOneBuffTime = 8;
    [SerializeField, Tooltip("朱雀スキルのバフの一体ごとの増加量")]
    float _skillOneBuffQuantityPerHit = 0.8f;
    [SerializeField, Tooltip("朱雀スキルのバフの基礎量")]
    float _skillOneBuffBaseQuantity = 0.5f;

    [Space(20)]

    [SerializeField, Tooltip("白虎スキルクールタイム")]
    float _skillTwoCT = 10;
    [Tooltip("白虎スキルクールタイムタイマー")]
    float _skillTwoCTimer;
    [SerializeField]
    Image SkillTwoIcon;
    [SerializeField]
    Image SkillTwoIconGauge;

    [Space(10)]

    [SerializeField, Tooltip("白虎スキルのダッシュ速度倍率")]
    float _skillTwoDashSpeed = 25;
    [SerializeField, Tooltip("白虎スキル発動時に何秒間操作不能にするか")]
    float _skillTwoWaitTime = 0.5f;

    [Space(20)]

    [SerializeField, Tooltip("青龍スキルクールタイム")]
    float _skillThreeCT = 10;
    [Tooltip("青龍スキルクールタイムタイマー")]
    float _skillThreeCTimer;
    [SerializeField]
    Image SkillThreeIcon;
    [SerializeField]
    Image SkillThreeIconGauge;

    [Space(10)]

    [SerializeField, Tooltip("回復トーテムのオブジェクト")]
    GameObject SkillThreeObject;
    [SerializeField, Tooltip("青龍のスキル効果時間")]
    float _skillThreeDuration = 8;

    [Space(20)]

    [SerializeField, Tooltip("玄武スキルクールタイム")]
    float _skillFourCT = 10;
    [Tooltip("玄武スキルクールタイムタイマー")]
    float _skillFourCTimer;
    [SerializeField]
    Image SkillFourIcon;
    [SerializeField]
    Image SkillFourIconGauge;

    [Space(10)]

    [SerializeField]
    GameObject _skillFourChain;
    [SerializeField]
    float _skillFourActivationTime = 1;

    [Space(10)]

    [SerializeField, Tooltip("玄武スキルの命中量上限")]
    float _skillFourHitValue;

    [SerializeField, Tooltip("玄武スキルの拘束時間")]
    float _skillFourRestraintTime = 6;
    [SerializeField, Tooltip("玄武スキルの効果範囲")]
    float _skillFourRange = 5;

    [SerializeField, Tooltip("玄武スキルのシールド維持時間")]
    float _skillFourShieldTime = 10;
    [SerializeField, Tooltip("玄武スキルのシールド量")]
    float _skillFourShieldQuantityPerHit;


    [Header("アニメーション関係")]
    Animator PlayerAnimator;

    [SerializeField]
    Animator ModeAnimator;

    [SerializeField]
    SpriteLibraryAsset skillIconLibrary;

    [Space(20)]

    Coroutine effectCoroutine;

    [SerializeField, Tooltip("被ダメを受けた時のカラー")]
    Color _hitDamageColor;
    [SerializeField, Tooltip("回復を受けた時のカラー")]
    Color _hitHealColor;
    [SerializeField, Tooltip("無敵になった時のカラー")]
    Color _invincibleColor;

    //プロパティ
    [HideInInspector, Tooltip("プレイヤーの移動速度")]
    public float PlayerSpeed { get { return _moveSpeed; } }
    [HideInInspector, Tooltip("プレイヤーの移動制限")]
    public bool PlayerMoveActive { get { return _moveActive; } }


    void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();

        _playerMode = PlayerMode.Sun;

        _currentHealth = _maxHealth;
        _currentSpiritPower = _maxSpiritPower;
        _invincibleActive = false;

        _moveActive = true;
        _canJump = false;

        PlayerAnimator.SetBool("SunMoon", true);

        _gravity = PlayerRigidBody.gravityScale;
        _firstScale = transform.localScale;

        _skillOneBuffValue = 1;
        _skillFourShieldQuantity = 0;
        ShieldGauge.fillAmount = 0;

        _modeTimer = Time.time;

        _skillOneCTimer = Time.time;
        _skillTwoCTimer = Time.time;
        _skillThreeCTimer = Time.time;
        _skillFourCTimer = Time.time;

        DOTween.To(() => (float)1, x => SkillOneIconGauge.fillAmount = x, 0, _skillOneCT).SetEase(Ease.Linear)
            .OnStart(() => SkillOneIcon.sprite = skillIconLibrary.GetSprite("SkillOne", "Normal"))
            .OnComplete(() => SkillOneIcon.sprite = skillIconLibrary.GetSprite("SkillOne", "Charge"));

        DOTween.To(() => (float)1, x => SkillTwoIconGauge.fillAmount = x, 0, _skillTwoCT).SetEase(Ease.Linear)
             .OnStart(() => SkillTwoIcon.sprite = skillIconLibrary.GetSprite("SkillTwo", "Normal"))
            .OnComplete(() => SkillTwoIcon.sprite = skillIconLibrary.GetSprite("SkillTwo", "Charge"));

        DOTween.To(() => (float)1, x => SkillThreeIconGauge.fillAmount = x, 0, _skillThreeCT).SetEase(Ease.Linear)
         .OnStart(() => SkillThreeIcon.sprite = skillIconLibrary.GetSprite("SkillThree", "Normal"))
            .OnComplete(() => SkillThreeIcon.sprite = skillIconLibrary.GetSprite("SkillThree", "Charge"));

        DOTween.To(() => (float)1, x => SkillFourIconGauge.fillAmount = x, 0, _skillFourCT).SetEase(Ease.Linear)
         .OnStart(() => SkillFourIcon.sprite = skillIconLibrary.GetSprite("SkillFour", "Normal"))
            .OnComplete(() => SkillFourIcon.sprite = skillIconLibrary.GetSprite("SkillFour", "Charge"));
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (_moveActive)
        {
            //スキル系
            #region
            //陰陽切り替え
            if (Input.GetKeyDown(KeyCode.RightShift) && _modeTimer + _modeChangeInterval < Time.time)
            {
                //陰形態に変形
                if (_playerMode == PlayerMode.Sun)
                {
                    _playerMode = PlayerMode.Moon;
                    ModeAnimator.SetBool("Exchange", true);
                    PlayerAnimator.SetBool("SunMoon", false);

                    SoundManager.ChangeMoon();
                }
                //陽形態に変形
                else
                {
                    _playerMode = PlayerMode.Sun;
                    ModeAnimator.SetBool("Exchange", false);
                    PlayerAnimator.SetBool("SunMoon", true);

                    SoundManager.ChangeSun();
                }

                _modeTimer = Time.time;
            }

            bool skillActive = false;

            //スキル１
            if (Input.GetKeyDown(KeyCode.Q) && !skillActive)
            {
                skillActive = true;

                if (_playerMode == PlayerMode.Sun && _skillOneCT + _skillOneCTimer < Time.time)
                {
                    //朱雀スキルを発動
                    StartCoroutine(SkillOne());
                }
                else if (_playerMode == PlayerMode.Moon && _skillThreeCT + _skillThreeCTimer < Time.time)
                {
                    //青龍スキルを発動
                    StartCoroutine(SkillThree());
                }
                else
                {
                    Debug.Log("スキル１ リキャストタイム中");
                }
            }


            //スキル２
            if (Input.GetKeyDown(KeyCode.E) && !skillActive)
            {
                skillActive = true;

                if (_playerMode == PlayerMode.Sun && _skillTwoCT + _skillTwoCTimer < Time.time)
                {
                    //白虎スキルを発動
                    StartCoroutine(SkillTwo());

                }
                else if (_playerMode == PlayerMode.Moon && _skillFourCT + _skillFourCTimer < Time.time)
                {
                    //玄武スキルを発動
                    StartCoroutine(SkillFour());
                }
                else
                {
                    Debug.Log("スキル２ リキャストタイム中");
                }
            }
            #endregion

            //移動
            if (!skillActive)
            {
                StartCoroutine(Move(horizontal));
            }

            //通常攻撃
            if (Input.GetKeyDown(KeyCode.Return) && !skillActive)
            {
                StartCoroutine(Attack());
            }

            //アニメーション系
            if (_skillActive)
            {
                PlayerAnimator.SetInteger("AnimationNumber", 3);
            }
            else if (_attackActive)
            {
                PlayerAnimator.SetInteger("AnimationNumber", 2);
            }

            else if (horizontal != 0)
            {
                PlayerAnimator.SetInteger("AnimationNumber", 1);
            }
            else
            {
                PlayerAnimator.SetInteger("AnimationNumber", 0);
            }
        }


        BuffIcon();

    }

    //当たり判定処理
    #region
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].normal != _lastNormal)
        {
            //地面に当たったらジャンプ回数を回復
            //地面の側面に当たったら反発する
            if (collision.contacts[0].normal.y > 0.8f)
            {
                _isGround = true;
                _canJump = true;
                _jumpTimer = 0;
            }

            //壁に当たった時に方向を保存する
            if (collision.gameObject.CompareTag("Ground"))
            {
                if (Mathf.Abs(collision.contacts[0].normal.x) > 0.8f)
                {
                    _wallTouch = Mathf.Sign(collision.contacts[0].normal.x);
                }
                else
                {
                    _wallTouch = 0;
                }
            }
            _lastNormal = collision.contacts[0].normal;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = false;
            _wallTouch = 0;
            _canJump = false;

            _lastNormal = Vector2.zero;
        }
    }

    #endregion

    //移動系
    IEnumerator Move(float horizontal)
    {
        //移動
        if (_wallTouch == 0 || _wallTouch == horizontal)
        {
            PlayerRigidBody.velocity = new Vector2(horizontal * _moveSpeed, PlayerRigidBody.velocity.y);
        }
        //移動方向に合わせてキャラクターの方向を変更
        if (horizontal != 0)
        {
            transform.localScale = new Vector2(horizontal * _firstScale.x, _firstScale.y);
        }

        //ジャンプ
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && _canJump)
        {
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, 0);
            PlayerRigidBody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);

            PlayerRigidBody.gravityScale = _gravity - _jumpGravity;

            _moveActive = false;

            yield return new WaitForSeconds(0.1f);

            _moveActive = true;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _jumpTimer += Time.deltaTime;

            if (_jumpTimer > _jumpMaxTime && !_isGround)
            {
                _canJump = false;
                PlayerRigidBody.gravityScale = _gravity;
            }
        }

        else if (PlayerRigidBody.gravityScale != _gravity)
        {
            PlayerRigidBody.gravityScale = _gravity;
        }

        if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)))
        {
            if (!_isGround)
            {
                _canJump = false;
                PlayerRigidBody.gravityScale = _gravity;
            }
        }
    }

    //通常攻撃の処理
    IEnumerator Attack()
    {
        //近接攻撃
        if (_playerMode == PlayerMode.Sun && _attackTimer + _attackSpeed < Time.time)
        {
            _attackTimer = Time.time;
            _attackActive = true;

            //近接攻撃当たり判定を出現
            AttackRangeObject.SetActive(true);

            _moveActive = false;
            PlayerRigidBody.AddForce(PlayerRigidBody.velocity.x * Vector2.left, ForceMode2D.Impulse);

            SoundManager.Attack();

            yield return new WaitForSeconds(0.2f);

            AttackRangeObject.SetActive(false);
            _moveActive = true;

            _attackActive = false;
        }
        //遠距離攻撃
        else if (_playerMode == PlayerMode.Moon && _fireTimer + _fireSpeed < Time.time && _currentSpiritPower >= 10)
        {
            _fireTimer = Time.time;
            _attackActive = true;

            //弾丸を発射
            GameObject bullet = Instantiate(Bullet, transform.position + new Vector3(Mathf.Sign(transform.localScale.x) * _bulletFirePosOffset.x, _bulletFirePosOffset.y, 0), Quaternion.Euler(0, 0, -90 * Mathf.Sign(transform.localScale.x)));
            bullet.transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x) * bullet.transform.localScale.x, bullet.transform.localScale.y, bullet.transform.localScale.z);

            BulletManager bulletManager = bullet.GetComponent<BulletManager>();
            bulletManager.StartDestroy(_bulletDestroyTime);
            bulletManager.SetProperty(_bulletMaxDamage, _bulletMinDamage, _bulletAttenuation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletVelocity * Mathf.Sign(transform.localScale.x), 0);

            _currentSpiritPower -= 10;
            SpiritGauge.fillAmount = _currentSpiritPower / _maxSpiritPower;

            _moveActive = false;

            SoundManager.Fire();
            PlayerRigidBody.AddForce(PlayerRigidBody.velocity.x * Vector2.left, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.2f);

            _moveActive = true;

            _attackActive = false;
        }
        else
        {
            Debug.Log("通常攻撃 リキャストタイム中");
        }
    }

    IEnumerator SkillOne()
    {
        _skillOneCTimer = Time.time;

        SoundManager.SkillOne();
        Debug.Log("朱雀スキル発動");

        _skillActive = true;
        PlayerAnimator.SetInteger("SkillNumber", 11);

        _moveActive = false;
        PlayerRigidBody.gravityScale += 1;

        DOTween.To(() => (float)1, x => SkillOneIconGauge.fillAmount = x, 0, _skillOneCT).SetEase(Ease.Linear)
            .OnStart(() => SkillOneIcon.sprite = skillIconLibrary.GetSprite("SkillOne", "Normal"))
            .OnComplete(() => SkillOneIcon.sprite = skillIconLibrary.GetSprite("SkillOne", "Charge"));

        //攻撃チャージ時間
        yield return new WaitForSeconds(_skillOneChargeTime);

        PlayerAnimator.SetInteger("SkillNumber", 12);

        _moveActive = true;
        PlayerRigidBody.gravityScale = _gravity;

        //初撃ダメージ
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right * Mathf.Sign(transform.localScale.x), _skillOneRange)
            .Where(hit => hit.collider.CompareTag("Enemy")).ToArray();

        foreach (RaycastHit2D hit in hits)
        {
            hit.collider.GetComponent<EnemyManager>().HitDamage(_skillOneAttackDamage);
            Debug.Log("朱雀スキル初撃命中");
        }

        //初撃内の弾丸を破壊
        GameObject[] EnemyBullets = hits
            .Select(hit => hit.collider.gameObject)
            .Where(go => go.CompareTag("EnemyBullet"))
            .ToArray();

        foreach (GameObject bullet in EnemyBullets)
        {
            Destroy(bullet);
            SpiritPowerIncrease(2);
        }

        //攻撃範囲 兼 炎のオブジェクトを配置
        GameObject skillObject = Instantiate(_skillOneObject,
            transform.position + new Vector3(
                _skillOneRange / 2 * Mathf.Sign(transform.localScale.x),
                transform.localScale.y / 2 * -1, 0),
            Quaternion.identity);

        //炎のステータスを設定
        SkillOneManager skillManager = skillObject.GetComponent<SkillOneManager>();
        skillManager._skillOneDuration = _skillOneDuration;
        skillManager._fireTime = _skillOneFireInterval;
        skillObject.transform.localScale = new Vector3(_skillOneRange, 1, 1);

        //朱雀バフを発動

        _skillOneBuffActive = true;
        _skillOneBuffTimer = _skillOneBuffTime;

        _skillOneBuffValue = hits.Length * _skillOneBuffQuantityPerHit + 1;

        _skillOneBuffText.text = $"+{(_skillOneBuffValue - 1 + _skillOneBuffBaseQuantity) * 100}%";

        yield return new WaitForSeconds(0.3f);

        _skillActive = false;

        yield return new WaitForSeconds(_skillOneBuffTime - 0.3f);

        //朱雀バフ解除

        _skillOneBuffActive = false;

        _skillOneBuffValue = 1;
    }

    IEnumerator SkillTwo()
    {
        _skillTwoCTimer = Time.time;

        SoundManager.SkillTwo();
        Debug.Log("白虎スキル発動");

        _skillActive = true;
        PlayerAnimator.SetInteger("SkillNumber", 2);

        _moveActive = false;
        _invincibleActive = true;
        StartCoroutine(Effect(2));

        //進行方向に向けて加速
        PlayerRigidBody.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * _moveSpeed * _skillTwoDashSpeed, 0);

        PlayerRigidBody.gravityScale = 0.5f;

        DOTween.To(() => (float)1, x => SkillTwoIconGauge.fillAmount = x, 0, _skillTwoCT).SetEase(Ease.Linear)
             .OnStart(() => SkillTwoIcon.sprite = skillIconLibrary.GetSprite("SkillTwo", "Normal"))
            .OnComplete(() => SkillTwoIcon.sprite = skillIconLibrary.GetSprite("SkillTwo", "Charge"));

        yield return new WaitForSeconds(_skillTwoWaitTime);

        PlayerRigidBody.gravityScale = _gravity;

        _skillActive = false;
        _invincibleActive = false;
        _moveActive = true;
    }


    IEnumerator SkillThree()
    {
        _skillThreeCTimer = Time.time;

        SoundManager.SkillThree();
        Debug.Log("青龍スキル発動");

        _skillActive = true;
        PlayerAnimator.SetInteger("SkillNumber", 3);

        _moveActive = false;
        PlayerRigidBody.AddForce(PlayerRigidBody.velocity.x * Vector2.left, ForceMode2D.Impulse);

        //スキルのオブジェクトを出現させる
        GameObject skillObject = Instantiate(SkillThreeObject, transform.position, Quaternion.identity);
        skillObject.GetComponent<SkillThreeManager>()._skillThreeDuration = _skillThreeDuration;


        DOTween.To(() => (float)1, x => SkillThreeIconGauge.fillAmount = x, 0, _skillThreeCT).SetEase(Ease.Linear)
         .OnStart(() => SkillThreeIcon.sprite = skillIconLibrary.GetSprite("SkillThree", "Normal"))
            .OnComplete(() => SkillThreeIcon.sprite = skillIconLibrary.GetSprite("SkillThree", "Charge"));

        yield return new WaitForSeconds(0.5f);

        _moveActive = true;
        _skillActive = false;
    }

    IEnumerator SkillFour()
    {
        _skillFourCTimer = Time.time;

        SoundManager.SkillFour();
        Debug.Log("玄武スキル発動");

        _skillActive = true;
        PlayerAnimator.SetInteger("SkillNumber", 4);

        _moveActive= false;
        PlayerRigidBody.gravityScale = 0;
        PlayerRigidBody.velocity = Vector2.zero;

        _invincibleActive = true;

        GameObject chainRight = Instantiate(_skillFourChain, transform.position, Quaternion.identity);
        chainRight.transform.localScale = new Vector3(chainRight.transform.localScale.x * Mathf.Sign(transform.localScale.x), chainRight.transform.localScale.y, chainRight.transform.localScale.z);
        SpriteRenderer spriteRendererRight = chainRight.GetComponent<SpriteRenderer>();
        DOTween.To(() => new Vector2(0, spriteRendererRight.size.y), x => spriteRendererRight.size = x, new Vector2(_skillFourRange * Mathf.Sign(transform.localScale.x), spriteRendererRight.size.y), _skillFourActivationTime).SetEase(Ease.Linear);
        DOTween.To(() => transform.position, x => chainRight.transform.position = x, transform.position + (_skillFourRange / 2) * Mathf.Sign(transform.localScale.x) * Vector3.right, _skillFourActivationTime).SetEase(Ease.Linear);

        GameObject chainLeft = Instantiate(_skillFourChain, transform.position, Quaternion.identity);
        chainLeft.transform.localScale = new Vector3(chainLeft.transform.localScale.x * -Mathf.Sign(transform.localScale.x), chainLeft.transform.localScale.y, chainLeft.transform.localScale.z);
        SpriteRenderer spriteRendererLeft = chainLeft.GetComponent<SpriteRenderer>();
        DOTween.To(() => new Vector2(0, spriteRendererLeft.size.y), x => spriteRendererLeft.size = x, new Vector2(_skillFourRange * -Mathf.Sign(transform.localScale.x), spriteRendererLeft.size.y), _skillFourActivationTime).SetEase(Ease.Linear);
        DOTween.To(() => transform.position, x => chainLeft.transform.position = x, transform.position + (_skillFourRange / 2) * -Mathf.Sign(transform.localScale.x) * Vector3.right, _skillFourActivationTime).SetEase(Ease.Linear);

        //敵を全て取得し処理
        GameObject[] closestEnemies = Physics2D.RaycastAll(transform.position, Vector2.right * Mathf.Sign(transform.localScale.x), _skillFourRange)
            .Concat(Physics2D.RaycastAll(transform.position, Vector2.left * Mathf.Sign(transform.localScale.x), _skillFourRange))
            .Select(hit => hit.collider.gameObject)
            .Where(go => go.CompareTag("Enemy"))
            .OrderBy(go => Vector2.Distance(go.transform.position, transform.position))
            .Take((int)_skillFourHitValue)
            .ToArray();


        foreach (GameObject enemy in closestEnemies)
        {
            if (enemy.TryGetComponent<EnemyManager>(out EnemyManager enemyManager))
            {
                enemyManager._moveActive = false;
            }
        }

        DOTween.To(() => (float)1, x => SkillFourIconGauge.fillAmount = x, 0, _skillFourCT).SetEase(Ease.Linear)
         .OnStart(() => SkillFourIcon.sprite = skillIconLibrary.GetSprite("SkillFour", "Normal"))
            .OnComplete(() => SkillFourIcon.sprite = skillIconLibrary.GetSprite("SkillFour", "Charge"));

        _skillFourBuffActive = true;
        _skillFourBuffTimer = _skillFourShieldTime;

        _skillFourShieldQuantity = _skillFourShieldQuantityPerHit * closestEnemies.Length;

        _skillFourBuffText.text = _skillFourShieldQuantity.ToString();
        ShieldGauge.fillAmount = _skillFourShieldQuantity / (_skillFourShieldQuantityPerHit * _skillFourHitValue);

        yield return new WaitForSeconds(_skillFourActivationTime);

        Destroy(chainRight);
        Destroy(chainLeft);
        _skillActive = false;

        _moveActive = true;
        PlayerRigidBody.gravityScale = _gravity;

        _invincibleActive = true;
        StartCoroutine(Effect(2));

        //効果時間終了の処理
        yield return new WaitForSeconds(_skillFourRestraintTime - _skillFourActivationTime);

        Debug.Log("玄武スキルの拘束時間終了");
        foreach (GameObject obj in closestEnemies)
        {
            if (obj)
            {
                if (obj.TryGetComponent<EnemyManager>(out EnemyManager enemyManager))
                {
                    enemyManager._moveActive = true;
                }
            }
        }

        yield return new WaitForSeconds(_skillFourShieldTime - _skillFourRestraintTime - _skillFourActivationTime);

        Debug.Log("玄武スキルのシールド維持時間終了");

        _skillFourShieldQuantity = 0;
        _skillFourBuffActive = false;
    }

    //ダメージを受けた時
    public void HitDamage(float damage)
    {
        if (!_invincibleActive)
        {
            if (_skillFourShieldQuantity > 0)
            {
                _skillFourShieldQuantity -= damage;

                ShieldGauge.fillAmount = _skillFourShieldQuantity / (_skillFourShieldQuantityPerHit * _skillFourHitValue);

                if (_skillFourShieldQuantity > 0)
                {
                    _skillFourBuffText.text = _skillFourShieldQuantity.ToString();
                }
                else
                {
                    _skillFourBuffActive = false;
                }
            }
            else
            {
                _currentHealth -= damage;
                HealthGauge.fillAmount = _currentHealth / _maxHealth;
            }

            if (effectCoroutine != null)
            {
                StopCoroutine(effectCoroutine);
            }
            effectCoroutine = StartCoroutine(Effect(0));

            if (_currentHealth <= 0)
            {
                SceneChanger.LoadScene(SceneChanger.SceneKind.InGame);
            }
        }
        else
        {
            Debug.Log("無敵時間中ヒット");
        }
    }

    //回復を受けた時
    public void HitHeal(float healAmount)
    {
        _currentHealth = Mathf.Min(_currentHealth + healAmount, _maxHealth);
        HealthGauge.fillAmount = _currentHealth / _maxHealth;

        if (effectCoroutine != null)
        {
            StopCoroutine(effectCoroutine);
        }
        effectCoroutine = StartCoroutine(Effect(1));
    }

    //妖力が回復した時
    public void SpiritPowerIncrease(float number)
    {
        int increasePower = 0;

        switch (number)
        {
            case 1:
                increasePower = 5;
                break;

            case 2:
                increasePower = 2;
                break;
        }

        _currentSpiritPower = Mathf.Min(_currentSpiritPower + increasePower, _maxSpiritPower);
        SpiritGauge.fillAmount = _currentSpiritPower / _maxSpiritPower;
    }

    //体のエフェクト効果
    IEnumerator Effect(int number)
    {
        switch (number)
        {
            //被ダメを受けたと時のカラー
            case 0:
                _spriteRenderer.color = _hitDamageColor;

                yield return new WaitForSeconds(0.1f);

                _spriteRenderer.color = Color.white;
                break;

            //回復した時のカラー
            case 1:
                _spriteRenderer.color = _hitHealColor;

                yield return new WaitForSeconds(0.1f);
                _spriteRenderer.color = Color.white;
                break;

            case 2:
                _spriteRenderer.color = _invincibleColor;

                yield return new WaitForSeconds(0.1f);
                _spriteRenderer.color = Color.white;

                break;
        }
    }

    void BuffIcon()
    {
        if (_skillOneBuffActive)
        {
            if (!_skillOneBuffIcon.activeSelf)
            {
                _skillOneBuffIcon.SetActive(true);
            }
            _skillOneBuffTimer -= Time.deltaTime;
            _skillOneBuffTimerText.text = _skillOneBuffTimer.ToString("00.0");
        }
        else if (_skillOneBuffIcon.activeSelf)
        {
            _skillOneBuffIcon.SetActive(false);
        }

        if (_skillFourBuffActive)
        {
            if (!_skillFourBuffIcon.activeSelf)
            {
                _skillFourBuffIcon.SetActive(true);
            }

            _skillFourBuffTimer -= Time.deltaTime;
            _skillFourBuffTimerText.text = _skillFourBuffTimer.ToString("00.0");
        }
        else if (_skillFourBuffIcon.activeSelf)
        {
            _skillFourBuffIcon.SetActive(false);
        }

    }
}
