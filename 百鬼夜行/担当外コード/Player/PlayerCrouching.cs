using UnityEngine;

/// <summary>
/// プレイヤーの「しゃがみ」状態を管理するステート。
/// 一方通行の床をすり抜ける処理も担当する。
/// </summary>
public class PlayerCrouching : IPlayerState
{
    private Player player;
    private Rigidbody rb;
    private PlayerStateMachine playerStateMachine;
    private Collider playerCollider; // すり抜け処理に必要

    // しゃがみ関連の変数
    private Vector3 playerOriginalScale;
    private float playerOriginalHeightPosition;
    private bool isCurrentlyCrouching; // crouchingから名前変更
    [SerializeField] private Vector3 crouchingScale = new Vector3(1f, 0.5f, 1f);
    [SerializeField] private float crouchSpeed = 10f;

    // ▼▼▼ PlayerController3Dから移動してきた変数 ▼▼▼
    [Header("すり抜け設定")]
    public float fallInitialForce = 10f; // publicに変更して調整可能に
    private int playerOriginalLayer;
    private OneWayPlatform3D currentFallingPlatform = null;
    private const string DEBUG_KEY = "[PlayerCrouching_FallThrough]";
    // ▲▲▲ PlayerController3Dから移動してきた変数 ▲▲▲

    public bool GetCrouching() => isCurrentlyCrouching;

    public void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.rb = player.GetComponent<Rigidbody>();
        this.playerCollider = player.GetComponent<Collider>(); // Colliderを取得

        // オリジナルのスケールとレイヤーを記憶
        this.playerOriginalScale = player.transform.localScale;
        this.playerOriginalLayer = player.gameObject.layer;

        // Nullチェック
        if (playerCollider == null) Debug.LogError($"{DEBUG_KEY}: PlayerにColliderがアタッチされていません！", player);
    }

    public void HandleInput()
    {
        // このステートではUpdateで入力を直接見るため、ここは空でOK
    }

    public void Update()
    {
        // ▼▼▼ PlayerController3Dから移動してきた処理 ▼▼▼
        HandleFallThroughPlatform();
        // ▲▲▲ PlayerController3Dから移動してきた処理 ▲▲▲

        // しゃがみ状態かどうかを確認し、状態を切り替える
        if (playerStateMachine.IsCrouching && player.IsGrounded())
        {
            StartCrouch();
        }
        else
        {
            StopCrouch();
        }

        // 見た目の補間処理
        if (isCurrentlyCrouching)
        {
            DoCrouchVisuals();
        }
        else
        {
            DoStandVisuals();
        }
    }

    public void Remove()
    {
        // この状態が完全に終了したときは、即座に元の状態に戻す
        ResetScaleAndPosition();
        RestorePlatformCollision(); // すり抜け中だった場合も考慮
        isCurrentlyCrouching = false;
    }

    // --- しゃがみ関連のメソッド ---

    private void StartCrouch()
    {
        if (isCurrentlyCrouching) return;
        isCurrentlyCrouching = true;
        playerOriginalHeightPosition = player.transform.position.y;
    }

    public void StopCrouch()
    {
        if (isCurrentlyCrouching)
        {
            isCurrentlyCrouching = false;
        }
    }

    private void DoCrouchVisuals()
    {
        if (player.IsGrounded())
        {
            float newScaleY = Mathf.MoveTowards(player.transform.localScale.y, crouchingScale.y, crouchSpeed * Time.deltaTime);
            float heightDifference = (playerOriginalScale.y - newScaleY) * 0.5f;

            player.transform.localScale = new Vector3(playerOriginalScale.x, newScaleY, playerOriginalScale.z);
            player.transform.position = new Vector3(
                player.transform.position.x,
                playerOriginalHeightPosition - heightDifference,
                player.transform.position.z
            );
        }
    }

    private void DoStandVisuals()
    {
        if (player.IsGrounded())
        {
            float newScaleY = Mathf.MoveTowards(player.transform.localScale.y, playerOriginalScale.y, crouchSpeed * Time.deltaTime);
            float heightDifference = (playerOriginalScale.y - newScaleY) * 0.5f;

            player.transform.localScale = new Vector3(playerOriginalScale.x, newScaleY, playerOriginalScale.z);
            player.transform.position = new Vector3(
                player.transform.position.x,
                playerOriginalHeightPosition - heightDifference,
                player.transform.position.z
            );
        }
    }

    private void ResetScaleAndPosition()
    {
        player.transform.localScale = playerOriginalScale;
    }


    // ▼▼▼ PlayerController3Dから移動してきたメソッド群 ▼▼▼

    /// <summary>
    /// 一方通行の床のすり抜け処理を管理する
    /// </summary>
    private void HandleFallThroughPlatform()
    {
        bool isCrouchHeld = playerStateMachine.IsCrouching;

        if (isCrouchHeld)
        {
            // すでにすり抜け中、または空中にいる場合は何もしない
            if (currentFallingPlatform != null || !player.IsGrounded()) return;

            // 真下にRayを飛ばして、すり抜け可能な床があるかチェック
            if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit, 1.0f))
            {
                var platform = hit.collider.GetComponentInParent<OneWayPlatform3D>();
                if (platform != null)
                {
                    currentFallingPlatform = platform;
                    platform.IgnoreCollision(playerCollider);
                    player.gameObject.layer = LayerMask.NameToLayer("FallingPlayer");
                    rb.AddForce(Vector3.down * fallInitialForce, ForceMode.Impulse);
                    Debug.Log($"{DEBUG_KEY}: すり抜け処理を開始します！");
                }
            }
        }
        else
        {
            // しゃがみキーが離されたら、衝突を元に戻す
            RestorePlatformCollision();
        }
    }

    /// <summary>
    /// すり抜け中の床との衝突判定を元に戻す
    /// </summary>
    private void RestorePlatformCollision()
    {
        if (currentFallingPlatform != null)
        {
            currentFallingPlatform.RestoreCollision(playerCollider);
            player.gameObject.layer = playerOriginalLayer;
            currentFallingPlatform = null;
            Debug.Log($"{DEBUG_KEY}: すり抜け処理を終了しました。");
        }
    }
    // ▲▲▲ PlayerController3Dから移動してきたメソッド群 ▲▲▲
}