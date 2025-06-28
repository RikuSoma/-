using UnityEngine;

public class BlueBoundEnemy : TargetingEnemy
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall Detection")]
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private BlueBoundMovementConfig config;

    private BlueBoundEnemyMovement movement;

    new void Start()
    {
        base.Start();

        // Movementコンポーネントの取得または追加
        movement = GetComponent<BlueBoundEnemyMovement>();
        if (movement == null)
        {
            movement = gameObject.AddComponent<BlueBoundEnemyMovement>();
        }

        // --- ここから修正箇所 ---

        // ターゲットオブジェクトを設定
        movement.SetTargetObject(TargetObject);

        // 各種設定をメソッド呼び出しではなく、直接代入する
        movement.groundCheck = groundCheck;
        movement.groundLayer = groundLayer;
        movement.wallLayer = wallLayer;

        // Configファイルからの設定も直接代入する
        if (config != null)
        {
            movement.jumpForce = config.jumpForce;
            movement.moveForce = config.moveForce;
            movement.crouchDuration = config.crouchDuration;
            movement.crouchScaleY = config.crouchScaleY;
            movement.wallDetectionDistance = config.wallDetectionDistance;
        }

        // --- 修正箇所ここまで ---

        if (healthManager != null)
        {
            healthManager.OnDeath += OnDeath;
        }
    }

    private void OnDeath()
    {
        // エフェクト生成
        Explode();
        // 自身を破棄
        Destroy(gameObject);
    }
}