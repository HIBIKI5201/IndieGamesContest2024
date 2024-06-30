using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
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
    [Tooltip("陰陽変形のタイマー")]
    float _modeTimer;


    [Header("体力ステータス")]
    [SerializeField, Tooltip("プレイヤーの最大ヘルス")]
    float _maxHealth = 1500;
    [SerializeField, ReadOnly, Tooltip("現在のヘルス")]
    float _currentHealth;

    [SerializeField, Tooltip("プレイヤーの最大妖力")]
    float _maxSpiritPower = 1500;
    [SerializeField, ReadOnly, Tooltip("現在の妖力")]
    float _currentSpiritPower;

    [Space]
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

    [Space]

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
    [Tooltip("ジャンプの最初の処理")]
    bool firstJump;
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

    [Space]

    [SerializeField, Tooltip("発射する弾丸プレハブ")]
    GameObject Bullet;
    [SerializeField, Tooltip("弾丸が発射される地点のオフセット")]
    Vector2 _bulletFirePosOffset;

    [SerializeField, Tooltip("次の射撃を行えるまでのリキャストタイム")]
    float _fireSpeed = 0.1f;
    [Tooltip("射撃リキャストタイマー")]
    float _fireTimer;

    [SerializeField, Tooltip("弾丸のスピード")]
    float _bulletVelocity = 10;
    [SerializeField, Tooltip("弾丸が消滅するまでの時間")]
    float _bulletDestroyTime = 10;

    [SerializeField, Tooltip("弾丸の最大ダメージ")]
    float _bulletMaxDamage = 100;
    [SerializeField, Tooltip("弾丸の最小ダメージ")]
    float _bulletMinDamage = 30;
    [SerializeField, Tooltip("弾丸の距離減衰")]
    float _bulletAttenuation = 5;


    [Header("スキル")]

    [SerializeField, Tooltip("朱雀スキルクールタイム")]
    float _skillOneCT = 10;
    [Tooltip("朱雀スキルクールタイムタイマー")]
    float _skillOneCTtimer;
    [SerializeField]
    Image SkillOneIconGauge;

    [SerializeField, Tooltip("朱雀スキルのオブジェクト")]
    GameObject _skillOneObjecct;
    [SerializeField, Tooltip("朱雀スキルのチャージ時間")]
    float _skillOneChargeTime = 1;

    [SerializeField, Tooltip("朱雀のスキル効果時間")]
    float _skillOneDuration = 5;
    [SerializeField, Tooltip("朱雀スキルの攻撃範囲")]
    float _skillOneRange = 3;
    [SerializeField, Tooltip("炎ダメージを与える間隔")]
    float _skillOneFireInterval = 0.2f;

    [Space]

    [SerializeField, Tooltip("白虎スキルクールタイム")]
    float _skillTwoCT = 10;
    [Tooltip("白虎スキルクールタイムタイマー")]
    float _skillTwoCTtimer;
    [SerializeField]
    Image SkillTwoIconGauge;

    [SerializeField, Tooltip("白虎スキルのダッシュ速度倍率")]
    float _skillTwoDashSpeed = 25;
    [SerializeField, Tooltip("白虎スキル発動時に何秒間操作不能にするか")]
    float _skillTwoWaitTime = 0.5f;

    [Space]

    [SerializeField, Tooltip("青龍スキルクールタイム")]
    float _skillThreeCT = 10;
    [Tooltip("青龍スキルクールタイムタイマー")]
    float _skillThreeCTtimer;
    [SerializeField]
    Image SkillThreeIconGauge;

    [SerializeField, Tooltip("回復トーテムのオブジェクト")]
    GameObject SkillThreeObject;
    [SerializeField, Tooltip("青龍のスキル効果時間")]
    float _skillThreeDuration = 8;

    [Space]

    [SerializeField, Tooltip("玄武スキルクールタイム")]
    float _skillFourCT = 10;
    [Tooltip("玄武スキルクールタイムタイマー")]
    float _skillFourCTtimer;
    [SerializeField]
    Image SkillFourIconGauge;

    [SerializeField, Tooltip("玄武スキルの拘束時間")]
    float _skillFourRestraintTime = 6;
    [SerializeField, Tooltip("玄武スキルのシールド維持時間")]
    float _skillFourShieldTime = 10;
    [SerializeField, Tooltip("玄武スキルの効果範囲")]
    float _skillFourRange = 5;

    [SerializeField]
    SpriteRenderer _spriteRenderer;

    Coroutine effectCoroutine;


    [Header("アニメーション関係")]
    Animator PlayerAnimator;

    [SerializeField]
    Animator ModeAnimator;

    [Space]

    [SerializeField, Tooltip("被ダメを受けた時のカラー")]
    Color _hitDamageColor;
    [SerializeField, Tooltip("回復を受けた時のカラー")]
    Color _hitHealColor;

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

        _moveActive = true;
        _canJump = false;

        PlayerAnimator.SetBool("SunMoon", true);

        _gravity = PlayerRigidBody.gravityScale;
        _firstScale = transform.localScale;

        _skillOneCTtimer = Time.time;
        _skillTwoCTtimer = Time.time;
        _skillThreeCTtimer = Time.time;
        _skillFourCTtimer = Time.time;

        DOTween.To(() => (float)0, x => SkillOneIconGauge.fillAmount = x, 1, _skillOneCT).SetEase(Ease.Linear);
        DOTween.To(() => (float)0, x => SkillTwoIconGauge.fillAmount = x, 1, _skillTwoCT).SetEase(Ease.Linear);
        DOTween.To(() => (float)0, x => SkillThreeIconGauge.fillAmount = x, 1, _skillThreeCT).SetEase(Ease.Linear);
        DOTween.To(() => (float)0, x => SkillFourIconGauge.fillAmount = x, 1, _skillFourCT).SetEase(Ease.Linear);
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (_moveActive)
        {
            //スキル系
            #region
            //陰陽切り替え
            if (Input.GetKeyDown(KeyCode.RightShift) && _modeTimer + 1 < Time.time)
            {
                //陰形態に変形
                if (_playerMode == PlayerMode.Sun)
                {
                    _playerMode = PlayerMode.Moon;
                    Debug.Log("陰形態に変形");
                    ModeAnimator.SetBool("Exchange", true);
                    PlayerAnimator.SetBool("SunMoon", false);

                    SoundManager.ChangeMoon();
                }
                //陽形態に変形
                else
                {
                    _playerMode = PlayerMode.Sun;
                    Debug.Log("陽形態に変形");
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

                if (_playerMode == PlayerMode.Sun && _skillOneCT + _skillOneCTtimer < Time.time)
                {
                    //朱雀スキルを発動
                    StartCoroutine(SkillOne());
                }
                else if (_playerMode == PlayerMode.Moon && _skillThreeCT + _skillThreeCTtimer < Time.time)
                {
                    //青龍スキルを発動
                    SkillThree();
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

                if (_playerMode == PlayerMode.Sun && _skillTwoCT + _skillTwoCTtimer < Time.time)
                {
                    //白虎スキルを発動
                    StartCoroutine(SkillTwo());

                }
                else if (_playerMode == PlayerMode.Moon && _skillFourCT + _skillFourCTtimer < Time.time)
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
                StartCoroutine(Attack(horizontal));
            }

            //アニメーション系
            if (_attackActive)
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            HitDamage(collision.gameObject.GetComponent<EnemyBulletManager>()._bulletDamage);
            Destroy(collision.gameObject);
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

    IEnumerator Attack(float horizontal)
    {
        //近接攻撃
        if (_playerMode == PlayerMode.Sun && _attackTimer + _attackSpeed < Time.time)
        {
            _attackTimer = Time.time;
            _attackActive = true;
            Debug.Log("近接攻撃発動");

            //近接攻撃当たり判定を出現
            AttackRangeObject.SetActive(true);
            _moveActive = false;

            PlayerRigidBody.AddForce(Vector2.left * PlayerRigidBody.velocity.x * 1.2f, ForceMode2D.Impulse);

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
            Debug.Log("遠距離攻撃発動");

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
            PlayerRigidBody.AddForce(Vector2.left * PlayerRigidBody.velocity.x * 1.2f, ForceMode2D.Impulse);

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
        _skillOneCTtimer = Time.time;

        SoundManager.SkillOne();
        Debug.Log("朱雀スキル発動");

        _moveActive = false;
        PlayerRigidBody.gravityScale += 1f;

        DOTween.To(() => (float)0, x => SkillOneIconGauge.fillAmount = x, 1, _skillOneCT).SetEase(Ease.Linear);

        //攻撃チャージ時間
        yield return new WaitForSeconds(_skillOneChargeTime);

        _moveActive = true;
        PlayerRigidBody.gravityScale = _gravity;

        //攻撃範囲 兼 炎のオブジェクトを配置
        GameObject skillObject = Instantiate(_skillOneObjecct,
            transform.position + new Vector3(
                _skillOneRange / 2 * Mathf.Sign(transform.localScale.x),
                transform.localScale.y / 2 * -1, 0),
            Quaternion.identity);

        SkillOneManager skillManager = skillObject.GetComponent<SkillOneManager>();
        skillManager._skillOneDuration = _skillOneDuration;
        skillManager._fireTime = _skillOneFireInterval;
        skillObject.transform.localScale = new Vector3(_skillOneRange, 1, 1);
    }

    IEnumerator SkillTwo()
    {
        _skillTwoCTtimer = Time.time;

        SoundManager.SkillTwo();
        Debug.Log("白虎スキル発動");

        _moveActive = false;

        //進行方向に向けて加速
        PlayerRigidBody.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * _moveSpeed * _skillTwoDashSpeed, 0);

        PlayerRigidBody.gravityScale = 0.5f;

        DOTween.To(() => (float)0, x => SkillTwoIconGauge.fillAmount = x, 1, _skillTwoCT).SetEase(Ease.Linear);

        yield return new WaitForSeconds(_skillTwoWaitTime);

        PlayerRigidBody.gravityScale = _gravity;
        _moveActive = true;
    }


    void SkillThree()
    {
        _skillThreeCTtimer = Time.time;

        SoundManager.SkillThree();
        Debug.Log("青龍スキル発動");

        //スキルのオブジェクトを出現させる
        GameObject skillObject = Instantiate(SkillThreeObject, transform.position, Quaternion.identity);
        skillObject.GetComponent<SkillThreeManager>()._skillThreeDuration = _skillThreeDuration;

        DOTween.To(() => (float)0, x => SkillThreeIconGauge.fillAmount = x, 1, _skillThreeCT).SetEase(Ease.Linear);
    }

    IEnumerator SkillFour()
    {
        _skillFourCTtimer = Time.time;

        SoundManager.SkillFour();
        Debug.Log("玄武スキル発動");

        //敵を全て取得し処理
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right * Mathf.Sign(transform.localScale.x), _skillFourRange);

        GameObject[] closestEnemies = new GameObject[hits.Length];
        if (hits.Length > 0)
        {
            closestEnemies = hits
                .Select(hit => hit.collider.gameObject)
                .Where(go => go.CompareTag("Enemy"))
                .OrderBy(go => Vector2.Distance(go.transform.position, transform.position))
                .Take(5)
                .ToArray();

            foreach (GameObject enemy in closestEnemies)
            {
                enemy.GetComponent<EnemyManager>()._moveActive = false;
            }
        }

        foreach (GameObject obj in closestEnemies)
        {
            Debug.Log($"{obj.name}が拘束された");
        }


        DOTween.To(() => (float)0, x => SkillFourIconGauge.fillAmount = x, 1, _skillFourCT).SetEase(Ease.Linear);

        //効果時間終了の処理

        yield return new WaitForSeconds(_skillFourRestraintTime < _skillFourShieldTime ? _skillFourRestraintTime : _skillFourShieldTime);

        Debug.Log("玄武スキルの拘束時間終了");
        foreach (GameObject obj in closestEnemies)
        {
            if (obj.TryGetComponent<EnemyManager>(out EnemyManager enemyManager))
            {
                enemyManager._moveActive = true;
            }

            Debug.Log(obj.name);
        }

        yield return new WaitForSeconds(_skillFourRestraintTime < _skillFourShieldTime ? _skillFourShieldTime - _skillFourRestraintTime : _skillFourRestraintTime - _skillFourShieldTime);

        Debug.Log("玄武スキルのシールド維持時間終了");
    }

    //ダメージを受けた時
    void HitDamage(float damage)
    {
        _currentHealth -= damage;
        HealthGauge.fillAmount = _currentHealth / _maxHealth;

        if (effectCoroutine != null)
        {
            StopCoroutine(effectCoroutine);
        }
        effectCoroutine = StartCoroutine(Effect(0));

        if (_currentHealth <= 0)
        {
            SceneChanger.LoadHIBIKIScene();
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
        }
    }
}
