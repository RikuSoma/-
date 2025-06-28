using UnityEngine;

public class BalloonEnemy : EnemyBase
{
    [SerializeField] private BalloonEnemyMovementConfig config;
    [SerializeField] private Shooting shootingComponent;
    private BalloonEnemyMovement movement;
    private Rigidbody rb;
    private bool isDead = false;

    // new void Start() の 'new' は基底クラスに同名のメソッドがない場合は不要
    void Awake() // コンポーネント参照はAwakeで行うのが一般的
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<BalloonEnemyMovement>();
        healthManager = GetComponent<CharacterHealthManager>(); // healthManagerもここで取得
    }

    new void Start()
    {
        // base.Start(); // 基底クラスのStartを呼び出す場合はこのように記述（今回は中身がないので不要）

        if(movement == null)
        {
            movement = gameObject.AddComponent<BalloonEnemyMovement>();
        }

        if (config == null)
        {
            Debug.LogError("[BalloonEnemy] Config が設定されていません！");
            // isDead = true; // エラー時に動作を停止させる
            enabled = false; // コンポーネント自体を無効化する方が安全
            return;
        }

        if (movement != null)
        {
            movement.Initialize(config);
        }

        if (shootingComponent != null && ShootingManager.Instance != null)
        {
            shootingComponent.SetBulletByName(config.bullet.name);
        }

        if (healthManager != null)
        {
            healthManager.OnDeath += OnDeath;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        // configがnullの場合を考慮
        if (config == null) return;

        foreach (string tag in config.hitIgnoreTags)
        {
            if (collision.gameObject.CompareTag(tag))
                return;
        }

        if (healthManager != null)
        {
            // health.GetHelth() は healthManager.GetHealth() のタイポと仮定
            healthManager.ApplyDamage(healthManager.GetHelth());
        }
        else
        {
            // HealthManagerがない場合は直接Explodeを呼ぶ
            Explode();
        }
    }

    private void OnDeath()
    {

        Explode();
    }

    // OnDestroyを追加してイベントの購読を解除する
    new private void OnDestroy()
    {
        base.OnDestroy(); // 基底クラスのOnDestroyも呼び出す

        if (healthManager != null)
        {
            healthManager.OnDeath -= OnDeath;
        }
    }


    override protected void Explode()
    {
        if (isDead) return;
        isDead = true;

        if (rb != null) rb.linearVelocity = Vector3.zero;
        if (movement != null) movement.Freeze(); // ここでmovementを停止

        Vector3 finalScale = transform.localScale * config.explosionScaleMultiplier;
        Vector3 peakScale = finalScale * config.peakScaleFactor;

        LeanTween.scale(gameObject, peakScale, config.explosionDuration)
            .setOnComplete(() =>
            {
                LeanTween.delayedCall(config.peakDuration, () =>
                {
                    LeanTween.scale(gameObject, finalScale, 0.1f)
                        .setOnComplete(() =>
                        {
                            EmitBulletsInPattern(config.pattern);
                            Destroy(gameObject, config.destroyDelay);
                        });
                });
            });
    }

    private void EmitBulletsInPattern(PatternType pattern)
    {
        if (shootingComponent == null) return;

        int bulletCount = (pattern == PatternType.Hexagon) ? 6 : 10;
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            if (pattern == PatternType.Star && i % 2 == 1)
                angle += angleStep / 2f;

            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
            shootingComponent.RequestShoot(dir);
        }
    }

    public enum PatternType { Hexagon, Star }
}