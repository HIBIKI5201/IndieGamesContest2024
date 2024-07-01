using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillOneManager : MonoBehaviour
{
    [HideInInspector]
    public float _skillOneDuration;

    float _timer;
    [HideInInspector]
    public float _fireTime;

    void Start()
    {
        Invoke("DestroyTime", _skillOneDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (_timer + _fireTime < Time.time)
            {
                _timer = Time.time;
                collision.GetComponent<EnemyManager>().HitDamage(10);
            }
        }
    }

    void DestroyTime()
    {
        Destroy(this.gameObject);
    }
}
