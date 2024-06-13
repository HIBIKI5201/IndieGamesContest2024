using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [ReadOnly, Tooltip("弾丸が消滅するまでの時間")]
    public float inBullet_bulletDestroyTime;

    [ReadOnly, Tooltip("弾丸の最大ダメージ")]
    public float inBullet_bulletMaxDamage;
    [ReadOnly, Tooltip("弾丸の最小ダメージ")]
    public float inBullet_bulletMinDamage;
    [ReadOnly, Tooltip("弾丸の距離減衰")]
    public float inBullet_bulletAttenuation;

    [Tooltip("弾丸が発射された地点")]
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
