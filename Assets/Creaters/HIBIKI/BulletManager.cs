using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [ReadOnly, Tooltip("�e�ۂ����ł���܂ł̎���")]
    public float inBullet_bulletDestroyTime;

    [ReadOnly, Tooltip("�e�ۂ̍ő�_���[�W")]
    public float inBullet_bulletMaxDamage;
    [ReadOnly, Tooltip("�e�ۂ̍ŏ��_���[�W")]
    public float inBullet_bulletMinDamage;
    [ReadOnly, Tooltip("�e�ۂ̋�������")]
    public float inBullet_bulletAttenuation;

    [Tooltip("�e�ۂ����˂��ꂽ�n�_")]
    Vector2 firstPos;
    void Start()
    {
        firstPos = transform.position;
        Invoke("Destroy", inBullet_bulletDestroyTime);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyManager enemyManager = collision.GetComponent<EnemyManager>();
            enemyManager._currentHealth -= Mathf.Clamp(Vector2.Distance(firstPos, transform.position) / inBullet_bulletAttenuation * inBullet_bulletMaxDamage, inBullet_bulletMinDamage, inBullet_bulletMaxDamage);

            Debug.Log(Mathf.Clamp(Vector2.Distance(firstPos, transform.position) / inBullet_bulletAttenuation * inBullet_bulletMaxDamage, inBullet_bulletMinDamage, inBullet_bulletMaxDamage));
        }
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
