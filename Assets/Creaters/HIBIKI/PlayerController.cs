using System.Collections;
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

    [Header("�̗̓X�e�[�^�X")]
    [SerializeField, Tooltip("�v���C���[�̍ő�w���X")]
    float _maxHealth;
    [SerializeField, ReadOnly, Tooltip("���݂̃w���X")]
    float _currentHealth;

    [Header("�ړ��X�e�[�^�X")]
    [SerializeField]
    Rigidbody2D PlayerRigidBody;
    [Tooltip("�d�͂̏����l")]
    float _gravity;
    [SerializeField,ReadOnly,Tooltip("�n�ʂɕt���Ă��邩�̔���")]
    bool _isGround;

    [SerializeField, Tooltip("�ړ����x")]
    float _moveSpeed;

    [SerializeField, Tooltip("�W�����v��")]
    float _jumpPower;
    [SerializeField, Tooltip("�W�����v���͂̒������Œ�����")]
    float _jumpMaxTime;
    [Tooltip("�W�����v���͂̃^�C�}�[")]
    float _jumpTimer;
    [SerializeField, Tooltip("�W�����v���������͒��̏d�͒ቺ��")]
    float _jumpGravity;


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
    float _attackSpeed;
    [Tooltip("�U�����L���X�g�^�C���̃^�C�}�[")]
    float _attackTimer;

    [Tooltip("�ߐڂ̃_���[�W")]
    public float _attackDamage;

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
    float _bulletDestroyTime;

    [SerializeField, Tooltip("�e�ۂ̍ő�_���[�W")]
    float _bulletMaxDamage;
    [SerializeField, Tooltip("�e�ۂ̍ŏ��_���[�W")]
    float _bulletMinDamage;
    [SerializeField, Tooltip("�e�ۂ̋�������")]
    float _bulletAttenuation;

    [Header("�X�L��")]

    [SerializeField, Tooltip("�鐝�X�L���N�[���^�C��")]
    float _skillOneCT;
    [Tooltip("�鐝�X�L���N�[���^�C���^�C�}�[")]
    float _skillOneCTtimer;

    [SerializeField, Tooltip("�鐝�X�L���̃I�u�W�F�N�g")]
    GameObject _skillOneObjecct;
    [SerializeField, Tooltip("�鐝�X�L���̃`���[�W����")]
    float _skillOneChargeTime;

    [SerializeField, Tooltip("���̃X�L�����ʎ���")]
    float _skillOneDuration;
    [SerializeField, Tooltip("�鐝�X�L���̍U���͈�")]
    float _skillOneRange;
    [SerializeField, Tooltip("���_���[�W��^����Ԋu")]
    float _skillOneFireInterval;

    [Space]

    [SerializeField, Tooltip("���ՃX�L���N�[���^�C��")]
    float _skillTwoCT;
    [Tooltip("���ՃX�L���N�[���^�C���^�C�}�[")]
    float _skillTwoCTtimer;

    [SerializeField, Tooltip("���ՃX�L���̃_�b�V�����x")]
    float _skillTwoDashSpeed;
    [SerializeField, Tooltip("���ՃX�L���������ɉ��b�ԑ���s�\�ɂ��邩")]
    float _skillTwoWaitTime;

    [Space]

    [SerializeField, Tooltip("���X�L���N�[���^�C��")]
    float _skillThreeCT;
    [Tooltip("���X�L���N�[���^�C���^�C�}�[")]
    float _skillThreeCTtimer;

    [SerializeField, Tooltip("�񕜃g�[�e���̃I�u�W�F�N�g")]
    GameObject SkillThreeObject;
    [SerializeField, Tooltip("���̃X�L�����ʎ���")]
    float _skillThreeDuration;

    [Space]

    [SerializeField, Tooltip("�����X�L���N�[���^�C��")]
    float _skillFourCT;
    [Tooltip("�����X�L���N�[���^�C���^�C�}�[")]
    float _skillFourCTtimer;

    [SerializeField, Tooltip("�����X�L���̍S������")]
    float _skillFourRestraintTime;
    [SerializeField, Tooltip("�����X�L���̃V�[���h�ێ�����")]
    float _skillFourShieldTime;
    [SerializeField, Tooltip("�����X�L���̌��ʔ͈�")]
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
            //�ړ��n
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

            //�U���n
            #region
            //�ʏ�U��
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(Attack());
            }
            #endregion

            //�X�L���n
            #region
            //�A�z�؂�ւ�
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

            //�X�L���P(Z�L�[)
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
                    Debug.Log("�X�L���P ���L���X�g�^�C����");
                }
            }

            //�X�L���Q(X�L�[)
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
                    Debug.Log("�X�L���Q ���L���X�g�^�C����");
                }
            }
            #endregion
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
        //�ߐڍU��
        if (_playerMode == PlayerMode.Sun && _attackTimer + _attackSpeed < Time.time)
        {
            _attackTimer = Time.time;
            Debug.Log("�ߐڍU������");

            //�ߐڍU�������蔻����o��
            AttackRangeObject.SetActive(true);

            yield return new WaitForSeconds(0.05f);

            AttackRangeObject.SetActive(false);
        }
        //�������U��
        else if (_playerMode == PlayerMode.Moon && _fireTimer + _fireSpeed < Time.time)
        {
            _fireTimer = Time.time;
            Debug.Log("�������U������");

            //�e�ۂ𔭎�
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
            Debug.Log("�ʏ�U�� ���L���X�g�^�C����");
        }
    }

    IEnumerator SkillOne()
    {
        _skillOneCTtimer = Time.time;
        Debug.Log("�鐝�X�L������");

        _moveActive = false;
        PlayerRigidBody.gravityScale += 1f;

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

    IEnumerator SkillTwo(float horizontal)
    {
        _skillTwoCTtimer = Time.time;
        Debug.Log("���ՃX�L������");

        _moveActive = false;

        //�i�s�����Ɍ����ĉ���
        PlayerRigidBody.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * _skillTwoDashSpeed, 0);
        PlayerRigidBody.gravityScale = 0.5f;

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
    }

    IEnumerator SkillFour()
    {
        _skillFourCTtimer = Time.time;
        Debug.Log("�����X�L������");

        //�G��S�Ď擾������
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");

        Debug.Log($"�����X�L���ɓ��������G��{enemy.Length}��");

        foreach (GameObject obj in enemy)
        {
            if (Vector2.Distance(obj.transform.position, transform.position) < _skillFourRange)
            {
                obj.GetComponent<EnemyManager>()._moveActive = false;
            } else
            {
                Debug.Log($"{obj.gameObject.name}�͌��ʔ͈͊O");
            }
        }

        //���ʎ��ԏI���̏���
        if (_skillFourRestraintTime < _skillFourShieldTime)
        {
            yield return new WaitForSeconds(_skillFourRestraintTime);

            Debug.Log("�����X�L���̍S�����ԏI��");
            foreach (GameObject obj in enemy)
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

            foreach (GameObject obj in enemy)
            {
                obj.GetComponent<EnemyManager>()._moveActive = true;
            }
        }
    }

}
