using UnityEngine;

// 攻撃ロジックは別のコンポーネントが担当するため、
// このクラスはEnemyAttackコンポーネントを持つことだけを要求する
[RequireComponent(typeof(EnemyAttack))]
public class HomingEnemy : TargetingEnemy
{
    // 攻撃の振る舞いを担当するモジュール
    private EnemyAttack attackModule;

    [SerializeField] private HomingEnemyConfig config;
    [SerializeField] private float attackInterval = 2.5f;
    private float attackTimer;

    new void Start()
    {
        base.Start();

        if (config == null)
        {
            Debug.LogError("HomingEnemyConfigが設定されていません。", gameObject);
            this.enabled = false;
            return;
        }

        // 自身が持つ攻撃モジュールを取得
        attackModule = GetComponent<HomingEnemyAttack>();
        if (attackModule != null)
        {
            // 攻撃モジュールをConfigで初期化
            attackModule.Initialize(config);
            attackModule.SetTarget(Target);
        }
        else
        {
            // RequireComponentがあるので通常は発生しない
            Debug.LogError("EnemyAttackコンポーネントが見つかりません。", gameObject);
            this.enabled = false;
            return;
        }

        attackTimer = attackInterval;

        healthManager.OnDeath += Dead;
    }

    void Update()
    {
        if (TargetObject == null) return;

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            attackTimer = 0f;
            // 攻撃の意思決定のみ行い、実行はモジュールに委譲する
            attackModule.PerformAttack();
        }
    }

    private void Dead()
    {
        Explode();

        Destroy(gameObject);
    }
}