using DG.Tweening;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
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
    [SerializeField, Tooltip("�A�z�؂�ւ��̃N�[���^�C��")]
    float _modeChangeInterval = 1;

    [Tooltip("�A�z�ό`�̃^�C�}�[")]
    float _modeTimer;

    [Space(10)]

    [SerializeField]
    SpriteRenderer _spriteRenderer;


    [Header("�o�t�E�f�o�t")]
    [SerializeField, ReadOnly, Tooltip("�鐝�X�L���̍U���͏㏸�o�t�̗L��")]
    bool _skillOneBuffActive;
    [Tooltip("�鐝�X�L���̃o�t�̎c�莞��")]
    float _skillOneBuffTimer;

    [ReadOnly, Tooltip("�鐝�X�L���̌��ʗ�")]
    public float _skillOneBuffValue;

    [Space(10)]

    [SerializeField]
    GameObject _skillOneBuffIcon;
    [SerializeField]
    TextMeshProUGUI _skillOneBuffText;
    [SerializeField]
    TextMeshProUGUI _skillOneBuffTimerText;

    [Space(20)]

    [SerializeField, ReadOnly, Tooltip("�����X�L���̃V�[���h�̗L��")]
    bool _skillFourBuffActive;
    [Tooltip("�����X�L���̃o�t�̎c�莞��")]
    float _skillFourBuffTimer;

    [SerializeField, Tooltip("�����X�L���̃V�[���h��")]
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


    [Header("�̗̓X�e�[�^�X")]
    [SerializeField, Tooltip("�v���C���[�̍ő�w���X")]
    float _maxHealth = 1500;
    [SerializeField, ReadOnly, Tooltip("���݂̃w���X")]
    float _currentHealth;

    [Space(10)]

    [SerializeField, Tooltip("�v���C���[�̍ő�d��")]
    float _maxSpiritPower = 1500;
    [SerializeField, ReadOnly, Tooltip("���݂̗d��")]
    float _currentSpiritPower;

    [SerializeField, ReadOnly, Tooltip("���G����")]
    bool _invincibleActive;

    [Space(10)]

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

    [Space(20)]

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

    [Space(20)]

    [SerializeField, Tooltip("���˂���e�ۃv���n�u")]
    GameObject Bullet;
    [SerializeField, Tooltip("�e�ۂ����˂����n�_�̃I�t�Z�b�g")]
    Vector2 _bulletFirePosOffset;

    [SerializeField, Tooltip("���̎ˌ����s����܂ł̃��L���X�g�^�C��")]
    float _fireSpeed = 0.1f;
    [Tooltip("�ˌ����L���X�g�^�C�}�[")]
    float _fireTimer;

    [Space(10)]

    [SerializeField, Tooltip("�e�ۂ̃X�s�[�h")]
    float _bulletVelocity = 10;
    [SerializeField, Tooltip("�e�ۂ����ł���܂ł̎���")]
    float _bulletDestroyTime = 10;

    [Space(10)]

    [SerializeField, Tooltip("�e�ۂ̍ő�_���[�W")]
    float _bulletMaxDamage = 100;
    [SerializeField, Tooltip("�e�ۂ̍ŏ��_���[�W")]
    float _bulletMinDamage = 30;
    [SerializeField, Tooltip("�e�ۂ̋�������")]
    float _bulletAttenuation = 5;


    [Header("�X�L��")]

    [Tooltip("�X�L�����������Ă��邩")]
    bool _skillActive;

    [SerializeField, Tooltip("�鐝�X�L���N�[���^�C��")]
    float _skillOneCT = 10;
    [Tooltip("�鐝�X�L���N�[���^�C���^�C�}�[")]
    float _skillOneCTimer;
    [SerializeField]
    Image SkillOneIcon;
    [SerializeField]
    Image SkillOneIconGauge;

    [Space(10)]

    [SerializeField, Tooltip("�鐝�X�L���̃I�u�W�F�N�g")]
    GameObject _skillOneObject;
    [SerializeField, Tooltip("�鐝�X�L���̃`���[�W����")]
    float _skillOneChargeTime = 1;
    [SerializeField, Tooltip("�������̏����̃_���[�W��")]
    float _skillOneAttackDamage = 120;

    [Space(10)]

    [SerializeField, Tooltip("�鐝�̃X�L�����ʎ���")]
    float _skillOneDuration = 5;
    [SerializeField, Tooltip("�鐝�X�L���̍U���͈�")]
    float _skillOneRange = 3;
    [SerializeField, Tooltip("���_���[�W��^����Ԋu")]
    float _skillOneFireInterval = 0.2f;

    [SerializeField, Tooltip("�鐝�X�L���̃o�t���ʎ���")]
    float _skillOneBuffTime = 8;
    [SerializeField, Tooltip("�鐝�X�L���̃o�t�̈�̂��Ƃ̑�����")]
    float _skillOneBuffQuantityPerHit = 0.8f;
    [SerializeField, Tooltip("�鐝�X�L���̃o�t�̊�b��")]
    float _skillOneBuffBaseQuantity = 0.5f;

    [Space(20)]

    [SerializeField, Tooltip("���ՃX�L���N�[���^�C��")]
    float _skillTwoCT = 10;
    [Tooltip("���ՃX�L���N�[���^�C���^�C�}�[")]
    float _skillTwoCTimer;
    [SerializeField]
    Image SkillTwoIcon;
    [SerializeField]
    Image SkillTwoIconGauge;

    [Space(10)]

    [SerializeField, Tooltip("���ՃX�L���̃_�b�V�����x�{��")]
    float _skillTwoDashSpeed = 25;
    [SerializeField, Tooltip("���ՃX�L���������ɉ��b�ԑ���s�\�ɂ��邩")]
    float _skillTwoWaitTime = 0.5f;

    [Space(20)]

    [SerializeField, Tooltip("���X�L���N�[���^�C��")]
    float _skillThreeCT = 10;
    [Tooltip("���X�L���N�[���^�C���^�C�}�[")]
    float _skillThreeCTimer;
    [SerializeField]
    Image SkillThreeIcon;
    [SerializeField]
    Image SkillThreeIconGauge;

    [Space(10)]

    [SerializeField, Tooltip("�񕜃g�[�e���̃I�u�W�F�N�g")]
    GameObject SkillThreeObject;
    [SerializeField, Tooltip("���̃X�L�����ʎ���")]
    float _skillThreeDuration = 8;

    [Space(20)]

    [SerializeField, Tooltip("�����X�L���N�[���^�C��")]
    float _skillFourCT = 10;
    [Tooltip("�����X�L���N�[���^�C���^�C�}�[")]
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

    [SerializeField, Tooltip("�����X�L���̖����ʏ��")]
    float _skillFourHitValue;

    [SerializeField, Tooltip("�����X�L���̍S������")]
    float _skillFourRestraintTime = 6;
    [SerializeField, Tooltip("�����X�L���̌��ʔ͈�")]
    float _skillFourRange = 5;

    [SerializeField, Tooltip("�����X�L���̃V�[���h�ێ�����")]
    float _skillFourShieldTime = 10;
    [SerializeField, Tooltip("�����X�L���̃V�[���h��")]
    float _skillFourShieldQuantityPerHit;


    [Header("�A�j���[�V�����֌W")]
    Animator PlayerAnimator;

    [SerializeField]
    Animator ModeAnimator;

    [SerializeField]
    SpriteLibraryAsset skillIconLibrary;

    [Space(20)]

    Coroutine effectCoroutine;

    [SerializeField, Tooltip("��_�����󂯂����̃J���[")]
    Color _hitDamageColor;
    [SerializeField, Tooltip("�񕜂��󂯂����̃J���[")]
    Color _hitHealColor;
    [SerializeField, Tooltip("���G�ɂȂ������̃J���[")]
    Color _invincibleColor;

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
            //�X�L���n
            #region
            //�A�z�؂�ւ�
            if (Input.GetKeyDown(KeyCode.RightShift) && _modeTimer + _modeChangeInterval < Time.time)
            {
                //�A�`�Ԃɕό`
                if (_playerMode == PlayerMode.Sun)
                {
                    _playerMode = PlayerMode.Moon;
                    ModeAnimator.SetBool("Exchange", true);
                    PlayerAnimator.SetBool("SunMoon", false);

                    SoundManager.ChangeMoon();
                }
                //�z�`�Ԃɕό`
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

            //�X�L���P
            if (Input.GetKeyDown(KeyCode.Q) && !skillActive)
            {
                skillActive = true;

                if (_playerMode == PlayerMode.Sun && _skillOneCT + _skillOneCTimer < Time.time)
                {
                    //�鐝�X�L���𔭓�
                    StartCoroutine(SkillOne());
                }
                else if (_playerMode == PlayerMode.Moon && _skillThreeCT + _skillThreeCTimer < Time.time)
                {
                    //���X�L���𔭓�
                    StartCoroutine(SkillThree());
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

                if (_playerMode == PlayerMode.Sun && _skillTwoCT + _skillTwoCTimer < Time.time)
                {
                    //���ՃX�L���𔭓�
                    StartCoroutine(SkillTwo());

                }
                else if (_playerMode == PlayerMode.Moon && _skillFourCT + _skillFourCTimer < Time.time)
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
                StartCoroutine(Attack());
            }

            //�A�j���[�V�����n
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

    //�ʏ�U���̏���
    IEnumerator Attack()
    {
        //�ߐڍU��
        if (_playerMode == PlayerMode.Sun && _attackTimer + _attackSpeed < Time.time)
        {
            _attackTimer = Time.time;
            _attackActive = true;

            //�ߐڍU�������蔻����o��
            AttackRangeObject.SetActive(true);

            _moveActive = false;
            PlayerRigidBody.AddForce(PlayerRigidBody.velocity.x * Vector2.left, ForceMode2D.Impulse);

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
            PlayerRigidBody.AddForce(PlayerRigidBody.velocity.x * Vector2.left, ForceMode2D.Impulse);

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
        _skillOneCTimer = Time.time;

        SoundManager.SkillOne();
        Debug.Log("�鐝�X�L������");

        _skillActive = true;
        PlayerAnimator.SetInteger("SkillNumber", 11);

        _moveActive = false;
        PlayerRigidBody.gravityScale += 1;

        DOTween.To(() => (float)1, x => SkillOneIconGauge.fillAmount = x, 0, _skillOneCT).SetEase(Ease.Linear)
            .OnStart(() => SkillOneIcon.sprite = skillIconLibrary.GetSprite("SkillOne", "Normal"))
            .OnComplete(() => SkillOneIcon.sprite = skillIconLibrary.GetSprite("SkillOne", "Charge"));

        //�U���`���[�W����
        yield return new WaitForSeconds(_skillOneChargeTime);

        PlayerAnimator.SetInteger("SkillNumber", 12);

        _moveActive = true;
        PlayerRigidBody.gravityScale = _gravity;

        //�����_���[�W
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right * Mathf.Sign(transform.localScale.x), _skillOneRange)
            .Where(hit => hit.collider.CompareTag("Enemy")).ToArray();

        foreach (RaycastHit2D hit in hits)
        {
            hit.collider.GetComponent<EnemyManager>().HitDamage(_skillOneAttackDamage);
            Debug.Log("�鐝�X�L����������");
        }

        //�������̒e�ۂ�j��
        GameObject[] EnemyBullets = hits
            .Select(hit => hit.collider.gameObject)
            .Where(go => go.CompareTag("EnemyBullet"))
            .ToArray();

        foreach (GameObject bullet in EnemyBullets)
        {
            Destroy(bullet);
            SpiritPowerIncrease(2);
        }

        //�U���͈� �� ���̃I�u�W�F�N�g��z�u
        GameObject skillObject = Instantiate(_skillOneObject,
            transform.position + new Vector3(
                _skillOneRange / 2 * Mathf.Sign(transform.localScale.x),
                transform.localScale.y / 2 * -1, 0),
            Quaternion.identity);

        //���̃X�e�[�^�X��ݒ�
        SkillOneManager skillManager = skillObject.GetComponent<SkillOneManager>();
        skillManager._skillOneDuration = _skillOneDuration;
        skillManager._fireTime = _skillOneFireInterval;
        skillObject.transform.localScale = new Vector3(_skillOneRange, 1, 1);

        //�鐝�o�t�𔭓�

        _skillOneBuffActive = true;
        _skillOneBuffTimer = _skillOneBuffTime;

        _skillOneBuffValue = hits.Length * _skillOneBuffQuantityPerHit + 1;

        _skillOneBuffText.text = $"+{(_skillOneBuffValue - 1 + _skillOneBuffBaseQuantity) * 100}%";

        yield return new WaitForSeconds(0.3f);

        _skillActive = false;

        yield return new WaitForSeconds(_skillOneBuffTime - 0.3f);

        //�鐝�o�t����

        _skillOneBuffActive = false;

        _skillOneBuffValue = 1;
    }

    IEnumerator SkillTwo()
    {
        _skillTwoCTimer = Time.time;

        SoundManager.SkillTwo();
        Debug.Log("���ՃX�L������");

        _skillActive = true;
        PlayerAnimator.SetInteger("SkillNumber", 2);

        _moveActive = false;
        _invincibleActive = true;
        StartCoroutine(Effect(2));

        //�i�s�����Ɍ����ĉ���
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
        Debug.Log("���X�L������");

        _skillActive = true;
        PlayerAnimator.SetInteger("SkillNumber", 3);

        _moveActive = false;
        PlayerRigidBody.AddForce(PlayerRigidBody.velocity.x * Vector2.left, ForceMode2D.Impulse);

        //�X�L���̃I�u�W�F�N�g���o��������
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
        Debug.Log("�����X�L������");

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

        //�G��S�Ď擾������
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

        //���ʎ��ԏI���̏���
        yield return new WaitForSeconds(_skillFourRestraintTime - _skillFourActivationTime);

        Debug.Log("�����X�L���̍S�����ԏI��");
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

        Debug.Log("�����X�L���̃V�[���h�ێ����ԏI��");

        _skillFourShieldQuantity = 0;
        _skillFourBuffActive = false;
    }

    //�_���[�W���󂯂���
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
            Debug.Log("���G���Ԓ��q�b�g");
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
