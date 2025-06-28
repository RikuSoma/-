using UnityEngine;

public class FoxSkill : BulletSkill
{
    private int counterCount = 0;
    private const int maxCounters = 3; // カウンター発動回数の上限
    private float counterCooldown = 0.1f;
    private float counterCooldownTimer = 0f;
    private bool recentlyCountered = false;
    private bool isInCounterMode = false; // カウンターモードかどうか
    private bool eventFlug = false;


    private CharacterHealthManager healthManager;
    private Shooting shooting;

    // 持続型スキルとして設定（Durationタイプ）
    protected override SkillType Type => SkillType.Duration;
    protected override float maxDuration => 5f;

    // 初期化時にプレイヤーの必要コンポーネント取得とイベント登録
    public override void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        base.Init(player, playerStateMachine);

        if (player == null) return;

        healthManager = player.GetComponent<CharacterHealthManager>();
        shooting = player.GetComponentInChildren<Shooting>();

        // プレイヤーの攻撃受けたイベントを購読しカウンター発動を検知
        if (!eventFlug)
        {
            player.OnAttackedByEnemy -= HandleAttackEvent;
            player.OnAttackedByEnemy += HandleAttackEvent;
            eventFlug = true;
        }

    }

    // スキル破棄時にイベント解除（メモリリーク防止）
    public override void Remove()
    {
        base.Remove();
        if (eventFlug)
        {
            player.OnAttackedByEnemy -= HandleAttackEvent;
            eventFlug = false;
            Debug.Log("[FoxSkill] イベント解除");
        }
    }

    // スキル開始時にカウンター状態に入り無敵化
    protected override void BeginSkill()
    {
        base.BeginSkill();
        isInCounterMode = true;
        counterCount = 0;

        if (healthManager != null)
            healthManager.SetIsInvincible(true); // ダメージ無効化

        Debug.Log("[FoxSkill] カウンターモード開始");
    }

    // スキル終了時にカウンター状態解除、無敵も解除
    protected override void EndSkill()
    {
        base.EndSkill();
        isInCounterMode = false;

        if (healthManager != null)
            healthManager.SetIsInvincible(false);

        Debug.Log("[FoxSkill] カウンターモード終了");
    }

    // 毎フレーム処理：カウンター時間管理
    public override void Update()
    {
        base.Update();

        if (!isInCounterMode) return;

        if (recentlyCountered)
        {
            counterCooldownTimer += Time.deltaTime;
            if (counterCooldownTimer >= counterCooldown)
            {
                recentlyCountered = false;
                counterCooldownTimer = 0f;
            }
        }

    }

    // プレイヤーが攻撃を受けた時に呼ばれる処理
    private void HandleAttackEvent()
    {
        if (!isInCounterMode || recentlyCountered) return;

        recentlyCountered = true;

        Debug.Log("[FoxSkill] カウンター発動！");
        ShootInEightDirections();

        counterCount += 1;
        Debug.Log("Count " + counterCount);

        if (counterCount >= maxCounters)
        {
            EndSkill();
        }
    }

    // XY平面（横スクロール画面に合わせてZは0）に8方向へ等間隔に弾を発射
    private void ShootInEightDirections()
    {
        if (shooting == null) return;

        int bulletCount = 8;
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float rad = angle * Mathf.Deg2Rad;

            // 弾の方向（XY平面、Z=0）
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f).normalized;

            shooting.RequestShoot(dir);

            angle += angleStep;
        }
    }
}
