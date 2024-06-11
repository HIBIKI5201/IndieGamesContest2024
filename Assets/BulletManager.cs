using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [ReadOnly, Tooltip("弾丸が消滅するまでの時間")]
    public float inBullet_bulletDestroyTime;
    void Start()
    {
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
