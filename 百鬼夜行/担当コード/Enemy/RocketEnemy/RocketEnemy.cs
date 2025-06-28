using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class RocketEnemy : TargetingEnemy
{
    [SerializeField] private RocketEnemyMovementConfig rocketMovementConfig;
    [SerializeField] private LayerMask groundLayer;

    private ParticleSystem effect;

    new void Start()
    {
        base.Start();

        healthManager = GetComponent<CharacterHealthManager>();

        if (healthManager != null)
        {
            healthManager.OnDeath += Explode;
        }

        effect = GetComponentInChildren<ParticleSystem>();

        if (effect != null)
        {
            effect.Play();
        }

        RocketEnemyMovement enemyMovement = GetComponent<RocketEnemyMovement>();
        if(enemyMovement == null)
        {
            enemyMovement = gameObject.AddComponent<RocketEnemyMovement>();
        }

        enemyMovement.Initialize(rocketMovementConfig,TargetObject);
        enemyMovement.SetGroundLayer(groundLayer);
    }

    void FixedUpdate()
    {
        // パーティクルを強制的に物理フレームと同期
        if (effect != null && !effect.isStopped)
        {
            effect.Simulate(Time.fixedDeltaTime, true, false, true);
        }
    }


    void Update()
    {

    }

    new private void OnDestroy()
    {
        if (healthManager != null)
        {
            healthManager.OnDeath -= Explode;
        }

        if (EnemyCounter.Instance != null)
        {
            EnemyCounter.Instance.RemoveEnemy(gameObject);
            Debug.Log($"{gameObject.name} が破棄されたため、敵カウンターから削除しました。現在の敵数: {EnemyCounter.Instance.EnemyCountInView}");
        }

    }

    override protected void Explode()
    {
        for (int i = 0; i < GomiNum; i++)
        {
            GameObject gomi = Instantiate(piecesPrefab, transform.position, Quaternion.identity);
            gomi.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10f, 10f), Random.Range(10f, 30f), 0.0f));
            Destroy(gomi, GomiLifeTime);
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 地面 or Player に当たったら死亡
        if (((1 << collision.gameObject.layer) & groundLayer) != 0 ||
            collision.gameObject.CompareTag("Player"))
        {
            healthManager.ApplyDamage(healthManager.GetHelth());
        }
    }

}
