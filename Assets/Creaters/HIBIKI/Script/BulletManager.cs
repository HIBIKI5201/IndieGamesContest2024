using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [ReadOnly, Tooltip("�e�ۂ̍ő�_���[�W")]
    public float inBullet_bulletMaxDamage;
    [ReadOnly, Tooltip("�e�ۂ̍ŏ��_���[�W")]
    public float inBullet_bulletMinDamage;
    [ReadOnly, Tooltip("�e�ۂ̋�������")]
    public float inBullet_bulletAttenuation;

    [ReadOnly, Tooltip("�e�ۂ����˂��ꂽ�n�_")]
    public Vector2 _firstPos;

    public void StartDestroy(float bulletDestroyTime)
    {
        _firstPos = transform.position;
        Invoke(nameof(Destroy), bulletDestroyTime);
    }

    public void SetProperty(float bulletMaxDamage, float bulletMinDamage, float bulletAttenuation)
    {
        inBullet_bulletMaxDamage = bulletMaxDamage;

        inBullet_bulletMinDamage = bulletMinDamage;

        inBullet_bulletAttenuation = bulletAttenuation;
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
