using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[")]
    [ReadOnly, Tooltip("�A�z�̌`��")]
    public PlayerMode _playerMode;
    public enum PlayerMode
    {
        Sun,
        Moon
    }
    [Tooltip("�A�z�ό`�̃^�C�}�[")]
    float _modeTimer;


    [Header("�̗̓X�e�[�^�X")]
    [SerializeField, Tooltip("�v���C���[�̍ő�w���X")]
    float _maxHealth = 1500;
    [SerializeField, ReadOnly, Tooltip("���݂̃w���X")]
    float _currentHealth;

    [SerializeField, Tooltip("�v���C���[�̍ő�d��")]
    float _maxSpiritPower = 1500;
    [SerializeField, ReadOnly, Tooltip("���݂̗d��")]
    float _currentSpiritPower;

    [Space]
    [SerializeField]
    Image HealthGauge;
    [SerializeField]
    Image SpiritGauge;


    [Header("�ړ��X�e�[�^�X")]
    Rigidbody2D PlayerRigidBody;
    [Tooltip("�d�͂̏����l")]
    float _gravity;
    [SerializeField, ReadOnly, Tooltip("�n�ʂɕt���Ă��邩�̔���")]
    bool _isGround;

    [Space]

    [SerializeField, Tooltip("�ړ����x")]
    float _moveSpeed = 5;

    [SerializeField, Tooltip("�W�����v��")]
    float _jumpPower = 8;
    [SerializeField, Tooltip("�W�����v���͂̒������Œ�����")]
    float _jumpMaxTime = 0.5f;
    [Tooltip("�W�����v���͂̃^�C�}�[")]
    float _jumpTimer;
    [SerializeField, Tooltip("�W�����v���������͒��̏d�͒ቺ��")]
    float _jumpGravity = 0.8f;

    [SerializeField, ReadOnly, Tooltip("�W�����v���\�ȏ�Ԃ�")]
    bool _canJump;
    [Tooltip("�W�����v�̍ŏ��̏���")]
    bool firstJump;
    [Tooltip("�ǂɓ������Ă��鎞�̖@����������")]
    float _wallTouch;
    [Tooltip("�Ō�ɓ�������Ground�̖@������")]
    Vector2 _lastNormal;

    [SerializeField, ReadOnly, Tooltip("�ړ��\�ȏ�Ԃ�")]
    bool _moveActive = true;

    [SerializeField, Tooltip("�����̑傫��")]
    Vector2 _firstScale;


    [Header("�U���X�e�[�^�X")]
    [SerializeField, Tooltip("�ʏ�U�����s���Ă��邩")]
    bool _attackActive;

    [SerializeField, Tooltip("�ߐڔ�������I�u�W�F�N�g")]
    GameObject AttackRangeObject;

    [SerializeField, Tooltip("���̍U�����s����܂ł̃��L���X�g�^�C��")]
    float _attackSpeed = 0.25f;
    [Tooltip("�U�����L���X�g�^�C���̃^�C�}�[")]
    float _attackTimer;

    [Tooltip("�ߐڂ̃_���[�W")]
    public float _attackDamage = 50;

    [Space]

    [SerializeField, Tooltip("���˂���e�ۃv���n�u")]
    GameObject Bullet;
    [SerializeField, Tooltip("�e�ۂ����˂����n�_�̃I�t�Z�b�g")]
    Vector2 _bulletFirePosOffset;

    [SerializeField, Tooltip("���̎ˌ����s����܂ł̃��L���X�g�^�C��")]
    float _fireSpeed = 0.1f;
    [Tooltip("�ˌ����L���X�g�^�C�}�[")]
    float _fireTimer;

    [SerializeField, Tooltip("�e�ۂ̃X�s�[�h")]
    float _bulletVelocity = 10;
    [SerializeField, Tooltip("�e�ۂ����ł���܂ł̎���")]
    float _bulletDestroyTime = 10;

    [SerializeField, Tooltip("�e�ۂ̍ő�_���[�W")]
    float _bulletMaxDamage = 100;
    [SerializeField, Tooltip("�e�ۂ̍ŏ��_���[�W")]
    float _bulletMinDamage = 30;
    [SerializeField, Tooltip("�e�ۂ̋�������")]
    float _bulletAttenuation = 5;


    [Header("�X�L��")]

    [SerializeField, Tooltip("�鐝�X�L���N�[���^�C��")]
    float _skillOneCT = 10;
    [Tooltip("�鐝�X�L���N�[���^�C���^�C�}�[")]
    float _skillOneCTtimer;
    [SerializeField]
    Image SkillOneIconGauge;

    [SerializeField, Tooltip("�鐝�X�L���̃I�u�W�F�N�g")]
    GameObject _skillOneObjecct;
    [SerializeField, Tooltip("�鐝�X�L���̃`���[�W����")]
    float _skillOneChargeTime = 1;

    [SerializeField, Tooltip("�鐝�̃X�L�����ʎ���")]
    float _skillOneDuration = 5;
    [SerializeField, Tooltip("�鐝�X�L���̍U���͈�")]
    float _skillOneRange = 3;
    [SerializeField, Tooltip("���_���[�W��^����Ԋu")]
    float _skillOneFireInterval = 0.2f;

    [Space]

    [SerializeField, Tooltip("���ՃX�L���N�[���^�C��")]
    float _skillTwoCT = 10;
    [Tooltip("���ՃX�L���N�[���^�C���^�C�}�[")]
    float _skillTwoCTtimer;
    [SerializeField]
    Image SkillTwoIconGauge;

    [SerializeField, Tooltip("���ՃX�L���̃_�b�V�����x�{��")]
    float _skillTwoDashSpeed = 25;
    [SerializeField, Tooltip("���ՃX�L���������ɉ��b�ԑ���s�\�ɂ��邩")]
    float _skillTwoWaitTime = 0.5f;

    [Space]

    [SerializeField, Tooltip("���X�L���N�[���^�C��")]
    float _skillThreeCT = 10;
    [Tooltip("���X�L���N�[���^�C���^�C�}�[")]
    float _skillThreeCTtimer;
    [SerializeField]
    Image SkillThreeIconGauge;

    [SerializeField, Tooltip("�񕜃g�[�e���̃I�u�W�F�N�g")]
    GameObject SkillThreeObject;
    [SerializeField, Tooltip("���̃X�L�����ʎ���")]
    float _skillThreeDuration = 8;

    [Space]

    [SerializeField, Tooltip("�����X�L���N�[���^�C��")]
    float _skillFourCT = 10;
    [Tooltip("�����X�L���N�[���^�C���^�C�}�[")]
    float _skillFourCTtimer;
    [SerializeField]
    Image SkillFourIconGauge;

    [SerializeField, Tooltip("�����X�L���̍S������")]
    float _skillFourRestraintTime = 6;
    [SerializeField, Tooltip("�����X�L���̃V�[���h�ێ�����")]
    float _skillFourShieldTime = 10;
    [SerializeField, Tooltip("�����X�L���̌��ʔ͈�")]
    float _skillFourRange = 5;

    [SerializeField]
    SpriteRenderer _spriteRenderer;

    Coroutine effectCoroutine;


    [Header("�A�j���[�V�����֌W")]
    Animator PlayerAnimator;

    [SerializeField]
    Animator ModeAnimator;

    [Space]

    [SerializeField, Tooltip("��_�����󂯂����̃J���[")]
    Color _hitDamageColor;
    [SerializeField, Tooltip("�񕜂��󂯂����̃J���[")]
    Color _hitHealColor;

    //�v���p�e�B
    [HideInInspector, Tooltip("�v���C���[�̈ړ����x")]
    public float PlayerSpeed { get { return _moveSpeed; } }
    [HideInInspector, Tooltip("�v���C���[�̈ړ�����")]
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
            //�X�L���n
            #region
            //�A�z�؂�ւ�
            if (Input.GetKeyDown(KeyCode.RightShift) && _modeTimer + 1 < Time.time)
            {
                //�A�`�Ԃɕό`
                if (_playerMode == PlayerMode.Sun)
                {
                    _playerMode = PlayerMode.Moon;
                    Debug.Log("�A�`�Ԃɕό`");
                    ModeAnimator.SetBool("Exchange", true);
                    PlayerAnimator.SetBool("SunMoon", false);

                    SoundManager.ChangeMoon();
                }
                //�z�`�Ԃɕό`
                else
                {
                    _playerMode = PlayerMode.Sun;
                    Debug.Log("�z�`�Ԃɕό`");
                    ModeAnimator.SetBool("Exchange", false);
                    PlayerAnimator.SetBool("SunMoon", true);

                    SoundManager.ChangeSun();
                }

                _modeTimer = Time.time;
            }

            bool skillActive = false;

            //�X�L���P
            if (Input.GetKeyDown(KeyCode.Q) && !skillActive)
            {
                skillActive = true;

                if (_playerMode == PlayerMode.Sun && _skillOneCT + _skillOneCTtimer < Time.time)
                {
                    //�鐝�X�L���𔭓�
                    StartCoroutine(SkillOne());
                }
                else if (_playerMode == PlayerMode.Moon && _skillThreeCT + _skillThreeCTtimer < Time.time)
                {
                    //���X�L���𔭓�
                    SkillThree();
                }
                else
                {
                    Debug.Log("�X�L���P ���L���X�g�^�C����");
                }
            }


            //�X�L���Q
            if (Input.GetKeyDown(KeyCode.E) && !skillActive)
            {
                skillActive = true;

                if (_playerMode == PlayerMode.Sun && _skillTwoCT + _skillTwoCTtimer < Time.time)
                {
                    //���ՃX�L���𔭓�
                    StartCoroutine(SkillTwo());

                }
                else if (_playerMode == PlayerMode.Moon && _skillFourCT + _skillFourCTtimer < Time.time)
                {
                    //�����X�L���𔭓�
                    StartCoroutine(SkillFour());
                }
                else
                {
                    Debug.Log("�X�L���Q ���L���X�g�^�C����");
                }
            }
            #endregion

            //�ړ�
            if (!skillActive)
            {
                StartCoroutine(Move(horizontal));
            }

            //�ʏ�U��
            if (Input.GetKeyDown(KeyCode.Return) && !skillActive)
            {
                StartCoroutine(Attack(horizontal));
            }

            //�A�j���[�V�����n
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

    //�����蔻�菈��
    #region
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].normal != _lastNormal)
        {
            //�n�ʂɓ���������W�����v�񐔂���
            //�n�ʂ̑��ʂɓ��������甽������
            if (collision.contacts[0].normal.y > 0.8f)
            {
                _isGround = true;
                _canJump = true;
                _jumpTimer = 0;
            }

            //�ǂɓ����������ɕ�����ۑ�����
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

    //�ړ��n
    IEnumerator Move(float horizontal)
    {
        //�ړ�
        if (_wallTouch == 0 || _wallTouch == horizontal)
        {
            PlayerRigidBody.velocity = new Vector2(horizontal * _moveSpeed, PlayerRigidBody.velocity.y);
        }
        //�ړ������ɍ��킹�ăL�����N�^�[�̕�����ύX
        if (horizontal != 0)
        {
            transform.localScale = new Vector2(horizontal * _firstScale.x, _firstScale.y);
        }

        //�W�����v
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
        //�ߐڍU��
        if (_playerMode == PlayerMode.Sun && _attackTimer + _attackSpeed < Time.time)
        {
            _attackTimer = Time.time;
            _attackActive = true;
            Debug.Log("�ߐڍU������");

            //�ߐڍU�������蔻����o��
            AttackRangeObject.SetActive(true);
            _moveActive = false;

            PlayerRigidBody.AddForce(Vector2.left * PlayerRigidBody.velocity.x * 1.2f, ForceMode2D.Impulse);

            SoundManager.Attack();

            yield return new WaitForSeconds(0.2f);

            AttackRangeObject.SetActive(false);
            _moveActive = true;

            _attackActive = false;
        }
        //�������U��
        else if (_playerMode == PlayerMode.Moon && _fireTimer + _fireSpeed < Time.time && _currentSpiritPower >= 10)
        {
            _fireTimer = Time.time;
            _attackActive = true;
            Debug.Log("�������U������");

            //�e�ۂ𔭎�
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
            Debug.Log("�ʏ�U�� ���L���X�g�^�C����");
        }
    }

    IEnumerator SkillOne()
    {
        _skillOneCTtimer = Time.time;

        SoundManager.SkillOne();
        Debug.Log("�鐝�X�L������");

        _moveActive = false;
        PlayerRigidBody.gravityScale += 1f;

        DOTween.To(() => (float)0, x => SkillOneIconGauge.fillAmount = x, 1, _skillOneCT).SetEase(Ease.Linear);

        //�U���`���[�W����
        yield return new WaitForSeconds(_skillOneChargeTime);

        _moveActive = true;
        PlayerRigidBody.gravityScale = _gravity;

        //�U���͈� �� ���̃I�u�W�F�N�g��z�u
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
        Debug.Log("���ՃX�L������");

        _moveActive = false;

        //�i�s�����Ɍ����ĉ���
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
        Debug.Log("���X�L������");

        //�X�L���̃I�u�W�F�N�g���o��������
        GameObject skillObject = Instantiate(SkillThreeObject, transform.position, Quaternion.identity);
        skillObject.GetComponent<SkillThreeManager>()._skillThreeDuration = _skillThreeDuration;

        DOTween.To(() => (float)0, x => SkillThreeIconGauge.fillAmount = x, 1, _skillThreeCT).SetEase(Ease.Linear);
    }

    IEnumerator SkillFour()
    {
        _skillFourCTtimer = Time.time;

        SoundManager.SkillFour();
        Debug.Log("�����X�L������");

        //�G��S�Ď擾������
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
            Debug.Log($"{obj.name}���S�����ꂽ");
        }


        DOTween.To(() => (float)0, x => SkillFourIconGauge.fillAmount = x, 1, _skillFourCT).SetEase(Ease.Linear);

        //���ʎ��ԏI���̏���

        yield return new WaitForSeconds(_skillFourRestraintTime < _skillFourShieldTime ? _skillFourRestraintTime : _skillFourShieldTime);

        Debug.Log("�����X�L���̍S�����ԏI��");
        foreach (GameObject obj in closestEnemies)
        {
            if (obj.TryGetComponent<EnemyManager>(out EnemyManager enemyManager))
            {
                enemyManager._moveActive = true;
            }

            Debug.Log(obj.name);
        }

        yield return new WaitForSeconds(_skillFourRestraintTime < _skillFourShieldTime ? _skillFourShieldTime - _skillFourRestraintTime : _skillFourRestraintTime - _skillFourShieldTime);

        Debug.Log("�����X�L���̃V�[���h�ێ����ԏI��");
    }

    //�_���[�W���󂯂���
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

    //�񕜂��󂯂���
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

    //�d�͂��񕜂�����
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

    //�̂̃G�t�F�N�g����
    IEnumerator Effect(int number)
    {
        switch (number)
        {
            //��_�����󂯂��Ǝ��̃J���[
            case 0:
                _spriteRenderer.color = _hitDamageColor;

                yield return new WaitForSeconds(0.1f);

                _spriteRenderer.color = Color.white;
                break;

            //�񕜂������̃J���[
            case 1:
                _spriteRenderer.color = _hitHealColor;

                yield return new WaitForSeconds(0.1f);
                _spriteRenderer.color = Color.white;
                break;
        }
    }
}
