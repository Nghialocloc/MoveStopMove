using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : GameUnit
{
    [Header("Info")]
    [SerializeField] private GameObject model;
    [SerializeField] private BulletType bulletType;

    [Header("Stats")]
    [SerializeField] [Range(-0.5f, 0.5f)] private float reloadTimeBuff = 0;
    [SerializeField] [Range(0.1f, 1.9f)] private float bulletSpeedBuff = 1;
    [SerializeField] [Range(0.1f, 1.9f)] private float bulletSizeBuff = 1;

    public bool IsCanAttack => model.activeSelf;

    private void OnEnable()
    {
        ActiveWeapon(true);
    }

    public void ActiveWeapon(bool active)
    {
        model.SetActive(active);
    }

    public void Throw(Character character, Vector3 target, float size)
    {
        Bullet bullet = SimplePool.Spawn<Bullet>((PoolType)bulletType, TF.position, Quaternion.identity);
        // Khi sinh ra dan, ap dung cac thong so cua vu khi vao dan moi sinh ra
        bullet.OnInit(character, target, size * bulletSizeBuff, bulletSpeedBuff);
        ActiveWeapon(false);

        // Hoi lai vu khi sau 1 khoang thoi gian
        Invoke(nameof(OnEnable), 1f - reloadTimeBuff);
    }
}
