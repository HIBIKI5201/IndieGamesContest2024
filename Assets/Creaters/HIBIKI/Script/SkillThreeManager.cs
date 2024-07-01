using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillThreeManager : MonoBehaviour
{
    [HideInInspector]
    public float _skillThreeDuration;

    DamageAndHealUIManager _damageAndHealUIManager;

    float _timer;
    void Start()
    {
        _damageAndHealUIManager = GameObject.FindAnyObjectByType<DamageAndHealUIManager>();

        Invoke(nameof(DestroyTimer), _skillThreeDuration);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_timer + 1 < Time.time)
            {
                _timer = Time.time;

                Debug.Log("‰ñ•œ");
                collision.GetComponent<PlayerController>().HitHeal(100);

                _damageAndHealUIManager.InstantiateHealText(collision.transform, 100, Mathf.Sign(collision.transform.localScale.x));
            }
        }
    }

    void DestroyTimer()
    {
        Destroy(this.gameObject);
    }
}
