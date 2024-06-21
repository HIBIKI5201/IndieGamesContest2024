using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    [SerializeField, ReadOnly]
    float test;

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

    [Space]
    [SerializeField]
    Image HealthGauge;

    [Header("�ړ��X�e�[�^�X")]
    [SerializeField]
    Rigidbody2D PlayerRigidBody;
    [Tooltip("�d�͂̏����l")]
    float _gravity;
    [SerializeField,ReadOnly,Tooltip("�n�ʂɕt���Ă��邩�̔���")]
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

    [SerializeField,ReadOnly,Tooltip("�W�����v���\�ȏ�Ԃ�")]
    bool _canJump;
    [Tooltip("�W�����v�̍ŏ��̏���")]
    bool firstJump;
    [Tooltip("�ǂɓ������Ă��鎞�̖@����������")]
    float _wallTouch;

    [SerializeField, ReadOnly, Tooltip("�ړ��\�ȏ�Ԃ�")]
    bool _moveActive = true;

    [Header("�U���X�e�[�^�X")]
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

    [SerializeField, Tooltip("���̃X�L�����ʎ���")]
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

    [SerializeField, Tooltip("���ՃX�L���̃_�b�V�����x")]
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

    [Header("�A�j���[�V�����֌W")]
    [SerializeField]
    Animator PlayerAnimator;

    [SerializeField]
    Animator ModeAnimator;

    void Start()
    {
        _playerMode = PlayerMode.Sun;

        _currentHealth = _maxHealth;

        _moveActive = true;
        _canJump = true;

        _gravity = PlayerRigidBody.gravityScale;

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
            //�ړ�
            StartCoroutine(Move(horizontal));

            //�U���n
            #region
            //�ʏ�U��
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(Attack(horizontal));
            }
            

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
                }
                //�z�`�Ԃɕό`
                else
                {
                    _playerMode = PlayerMode.Sun;
                    Debug.Log("�z�`�Ԃɕό`");
                    ModeAnimator.SetBool("Exchange", false);
                }

                _modeTimer = Time.time;
            }

            //�X�L���P
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
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
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
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
            #endregion

            //�A�j���[�V�����n
            if (AttackRangeObject.activeSelf)
            {
                PlayerAnimator.SetInteger("AnimationNumber", 2);
            }

            else if (horizontal != 0)
            {
                PlayerAnimator.SetInteger("AnimationNumber", 1);
            } else
            {
                PlayerAnimator.SetInteger("AnimationNumber", 0);
            }
        }
    }

    //�����蔻�菈��
    #region
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�n�ʂɓ���������W�����v�񐔂���
        //�n�ʂ̑��ʂɓ��������甽������
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.contacts[0].normal.y > 0.8f)
            {
                _isGround = true;
                _canJump = true;
                _jumpTimer = 0;
                _wallTouch = 0;
            }

            //�ǂɓ����������ɕ�����ۑ�����
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitDamage(50);
        }

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
            transform.localScale = new Vector2(horizontal, transform.localScale.y);
        }

        //�W�����v
        if (Input.GetKeyDown(KeyCode.W) && _canJump)
        {
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, 0);
            PlayerRigidBody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);

            PlayerRigidBody.gravityScale = _gravity - _jumpGravity;

            _moveActive = false;

            yield return new WaitForSeconds(0.1f);

            _moveActive = true;
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
    }

    IEnumerator Attack(float horizontal)
    {
        //�ߐڍU��
        if (_playerMode == PlayerMode.Sun && _attackTimer + _attackSpeed < Time.time)
        {
            _attackTimer = Time.time;
            Debug.Log("�ߐڍU������");

            //�ߐڍU�������蔻����o��
            AttackRangeObject.SetActive(true);
            _moveActive = false;

            PlayerRigidBody.AddForce(Vector2.left * horizontal * 3, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.2f);

            AttackRangeObject.SetActive(false);
            _moveActive = true;
        }
        //�������U��
        else if (_playerMode == PlayerMode.Moon && _fireTimer + _fireSpeed < Time.time)
        {
            _fireTimer = Time.time;
            Debug.Log("�������U������");

            //�e�ۂ𔭎�
            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, -90 * Mathf.Sign(transform.localScale.x)));
            BulletManager bulletManager = bullet.GetComponent<BulletManager>();
            bulletManager.StartDestroy(_bulletDestroyTime);
            bulletManager.SetProperty(_bulletMaxDamage, _bulletMinDamage, _bulletAttenuation);

            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletVelocity * Mathf.Sign(transform.localScale.x), 0);
        }
        else
        {
            Debug.Log("�ʏ�U�� ���L���X�g�^�C����");
        }
    }

    IEnumerator SkillOne()
    {
        _skillOneCTtimer = Time.time;
        Debug.Log("�鐝�X�L������");

        _moveActive = false;
        PlayerRigidBody.gravityScale += 1f;

        DOTween.To(() => (float)0, x => SkillOneIconGauge.fillAmount = x, 1, _skillOneCT).SetEase(Ease.Linear);

        //�U���`���[�W����
        yield return new WaitForSeconds(_skillOneChargeTime);

        _moveActive = true;
        PlayerRigidBody.gravityScale = _gravity;

        //�U���͈� �� ���̃I�u�W�F�N�g��z�u
        GameObject skillObject = Instantiate(_skillOneObjecct, transform.position + new Vector3(_skillOneRange / 2 * Mathf.Sign(transform.localScale.x), 0, 0), Quaternion.identity);

        SkillOneManager skillManager = skillObject.GetComponent<SkillOneManager>();
        skillManager._skillOneDuration = _skillOneDuration;
        skillManager._fireTime = _skillOneFireInterval;
        skillObject.transform.localScale = new Vector3(_skillOneRange, 1, 1);


    }

    IEnumerator SkillTwo()
    {
        _skillTwoCTtimer = Time.time;
        Debug.Log("���ՃX�L������");

        _moveActive = false;

        //�i�s�����Ɍ����ĉ���
        PlayerRigidBody.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * _skillTwoDashSpeed, 0);
        PlayerRigidBody.gravityScale = 0.5f;

        DOTween.To(() => (float)0, x => SkillTwoIconGauge.fillAmount = x, 1, _skillTwoCT).SetEase(Ease.Linear);

        yield return new WaitForSeconds(_skillTwoWaitTime);

        PlayerRigidBody.gravityScale = _gravity;
        _moveActive = true;
    }


    void SkillThree()
    {
        _skillThreeCTtimer = Time.time;
        Debug.Log("���X�L������");

        //�X�L���̃I�u�W�F�N�g���o��������
        GameObject skillObject = Instantiate(SkillThreeObject, transform.position, Quaternion.identity);
        skillObject.GetComponent<SkillThreeManager>()._skillThreeDuration = _skillThreeDuration;

        DOTween.To(() => (float)0, x => SkillThreeIconGauge.fillAmount = x, 1, _skillThreeCT).SetEase(Ease.Linear);
    }

    IEnumerator SkillFour()
    {
        _skillFourCTtimer = Time.time;
        Debug.Log("�����X�L������");

        //�G��S�Ď擾������
        GameObject[] closestEnemies = GameObject.FindGameObjectsWithTag("Enemy")
            .OrderBy(go => Vector2.Distance(go.transform.position, transform.position))
            .Take(5)
            .ToArray();

        foreach (GameObject obj in closestEnemies)
        {
            if (Vector2.Distance(obj.transform.position, transform.position) < _skillFourRange)
            {
                obj.GetComponent<EnemyManager>()._moveActive = false;
            } else
            {
                Debug.Log($"{obj.gameObject.name}�͌��ʔ͈͊O");
            }
        }

        DOTween.To(() => (float)0, x => SkillFourIconGauge.fillAmount = x, 1, _skillFourCT).SetEase(Ease.Linear);

        //���ʎ��ԏI���̏���
        if (_skillFourRestraintTime < _skillFourShieldTime)
        {
            yield return new WaitForSeconds(_skillFourRestraintTime);

            Debug.Log("�����X�L���̍S�����ԏI��");
            foreach (GameObject obj in closestEnemies)
            {
                obj.GetComponent<EnemyManager>()._moveActive = true;
            }

            yield return new WaitForSeconds(_skillFourShieldTime - _skillFourRestraintTime);

            Debug.Log("�����X�L���̃V�[���h�ێ����ԏI��");
        }
        else
        {
            yield return new WaitForSeconds(_skillFourShieldTime);

            Debug.Log("�����X�L���̃V�[���h�ێ����ԏI��");

            yield return new WaitForSeconds(_skillFourRestraintTime - _skillFourShieldTime);

            Debug.Log("�����X�L���̍S�����ԏI��");

            foreach (GameObject obj in closestEnemies)
            {
                obj.GetComponent<EnemyManager>()._moveActive = true;
            }
        }
    }

    void HitDamage(float Damage)
    {
        _currentHealth -= Damage;
        HealthGauge.fillAmount = _currentHealth / _maxHealth;
    }

}