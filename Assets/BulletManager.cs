using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [HideInInspector,Tooltip("�e�ۂ����ł���܂ł̎���")]
    public float inBullet_bulletDestroyTime = 5;
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
