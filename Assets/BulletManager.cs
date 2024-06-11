using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [ReadOnly, Tooltip("�e�ۂ����ł���܂ł̎���")]
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
