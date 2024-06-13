using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [ReadOnly, Tooltip("íeä€Ç™è¡ñ≈Ç∑ÇÈÇ‹Ç≈ÇÃéûä‘")]
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
