using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField,ReadOnly,Tooltip("ìGÇÃç≈ëÂëÃóÕ")]
    float _maxHealth;
    [ReadOnly, Tooltip("åªç›ÇÃëÃóÕ")]
    public float _currentHealth;

    public bool _moveActive;

    SpriteRenderer _spriteRenderer;

    PlayerController _playerController;


    void Start()
    {
        _currentHealth = _maxHealth;
        _moveActive = true;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_moveActive)
        {
            _spriteRenderer.color = Color.white;
        } else
        {
            _spriteRenderer.color = Color.red;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Melee"))
        {
            _currentHealth -= _playerController._attackDamage;

            Debug.Log($"åªç›ÇÃëÃóÕÇÕ{_currentHealth}");
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletManager bulletManager = collision.GetComponent<BulletManager>();
            _currentHealth -= Mathf.Clamp(Vector2.Distance(bulletManager.firstPos, transform.position) / bulletManager.inBullet_bulletAttenuation * bulletManager.inBullet_bulletMaxDamage, bulletManager.inBullet_bulletMinDamage, bulletManager.inBullet_bulletMaxDamage);

            Debug.Log(Mathf.Clamp(Vector2.Distance(bulletManager.firstPos, transform.position) / bulletManager.inBullet_bulletAttenuation * bulletManager.inBullet_bulletMaxDamage, bulletManager.inBullet_bulletMinDamage, bulletManager.inBullet_bulletMaxDamage));

            Debug.Log($"åªç›ÇÃëÃóÕÇÕ{_currentHealth}");
        }
    }
}
