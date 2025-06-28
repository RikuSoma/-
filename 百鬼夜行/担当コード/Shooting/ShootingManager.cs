using UnityEngine;
using System.Collections.Generic;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] private List<Shooting> shootingObjects = new();
    [SerializeField] private bool debugLogEnabled = false;

    private static ShootingManager _instance;
    public static ShootingManager Instance => _instance ?? throw new System.Exception("ShootingManager is not initialized");

    private IBulletFactory bulletFactory;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        bulletFactory = new DefaultBulletFactory(); // 将来的に差し替え可能
    }

    private void Update()
    {
        foreach (var shooter in shootingObjects)
        {
            while (shooter != null && shooter.HasShootRequests)
            {
                Vector3 direction = shooter.DequeueShootDirection();
                Fire(shooter, direction);
            }
        }
    }

    public void RegisterShooting(Shooting shooter)
    {
        if (shooter != null && !shootingObjects.Contains(shooter))
        {
            shootingObjects.Add(shooter);
        }
    }

    private void Fire(Shooting shooter, Vector3 direction)
    {
        var bulletPrefab = shooter.GetBulletObject();
        if (bulletPrefab == null)
        {
            Debug.LogWarning($"[{shooter.name}] 弾Prefabが設定されていません。");
            return;
        }

        Vector3 shootPos = shooter.transform.position + direction.normalized;
        bulletFactory.CreateBullet(bulletPrefab, shootPos, direction, shooter);

        if (debugLogEnabled)
        {
            Debug.Log($"[{shooter.name}] が方向 {direction} に弾を発射");
        }
    }
}
