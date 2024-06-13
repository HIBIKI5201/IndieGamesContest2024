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
    public Vector2 firstPos;
    void Start()
    {
        firstPos = transform.position;
        Invoke("Destroy", inBullet_bulletDestroyTime);
    }

    void Update()
    {
        
    }


    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
