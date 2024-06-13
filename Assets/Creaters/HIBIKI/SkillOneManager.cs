using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillOneManager : MonoBehaviour
{
    [HideInInspector]
    public float _skillOneDuration;

    bool _firstAttack;

    float _timer;
    [HideInInspector]
    public float _fireTime;

    void Start()
    {
        _firstAttack = true;
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
            if (_firstAttack)
            {
                Debug.Log("朱雀スキルの敵のコンポーネント取得とダメージ");
                _firstAttack = false;
            }

            if (_timer + _fireTime < Time.time)
            {
                Debug.Log("炎ダメージ");
                _timer = Time.time;
            }
        }
    }

    void DestroyTime()
    {
        Destroy(this.gameObject);
    }
}
