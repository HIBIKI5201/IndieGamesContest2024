using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [ReadOnly, Tooltip("弾丸の最大ダメージ")]
    public float inBullet_bulletMaxDamage;
    [ReadOnly, Tooltip("弾丸の最小ダメージ")]
    public float inBullet_bulletMinDamage;
    [ReadOnly, Tooltip("弾丸の距離減衰")]
    public float inBullet_bulletAttenuation;

    [ReadOnly, Tooltip("弾丸が発射された地点")]
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
