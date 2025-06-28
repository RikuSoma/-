using UnityEngine;

// 
// 前回スポーンした敵が倒されてから、一定時間後に次の敵をスポーンする
// 

public class LoopSpawner : MonoBehaviour
{
    [Header("スポーン設定")]
    [SerializeField] private bool CanSpawn = true;
    [SerializeField] private GameObject SpawnObject;
    [SerializeField] private float SpawnInterval = 2.0f;
    [SerializeField] private GameObject TargetObject;

    private float spawnTimer;
    private bool isTargetNeeded = false;
    private GameObject lastSpawnedObject;

    void Start()
    {
        // ... Startメソッドの中身は変更なし ...
        if (SpawnObject == null)
        {
            Debug.LogError("スポーンするオブジェクト(SpawnObject)が設定されていません。", this);
            CanSpawn = false;
            return;
        }
        if (SpawnObject.GetComponent<EnemyBase>() == null)
        {
            Debug.LogError("スポーンするオブジェクトが敵(EnemyBase)ではありません。", this);
            CanSpawn = false;
            return;
        }
        if (SpawnObject.GetComponent<TargetingEnemy>() != null)
        {
            isTargetNeeded = true;
            if (TargetObject == null)
            {
                Debug.LogError("ターゲットが必要な敵ですが、ターゲットオブジェクト(TargetObject)が設定されていません。", this);
                CanSpawn = false;
                return;
            }
        }
    }

    void Update()
    {
        if (!CanSpawn || SpawnObject == null)
        {
            return;
        }

        // ▼▼▼ ここからロジックを修正 ▼▼▼

        // 1. 前回スポーンした敵がまだ存在している場合は、何もしない
        if (lastSpawnedObject != null)
        {
            return; // 敵が倒されるまで待機
        }

        // 2. 敵がいない場合のみ、次のスポーンまでのタイマーを進める
        spawnTimer += Time.deltaTime;

        // 3. タイマーが設定時間を超えたら、新しい敵をスポーンする
        if (spawnTimer >= SpawnInterval)
        {
            // オブジェクトをスポーン
            GameObject spawnedObject = Instantiate(SpawnObject, transform.position, Quaternion.identity);

            // スポーンしたオブジェクトを記憶
            lastSpawnedObject = spawnedObject;

            if (isTargetNeeded)
            {
                TargetingEnemy targetingEnemy = spawnedObject.GetComponent<TargetingEnemy>();
                if (targetingEnemy != null)
                {
                    targetingEnemy.Target = TargetObject;
                }
            }

            // タイマーをリセットして、次の「敵が倒された後」のカウントダウンに備える
            spawnTimer = 0.0f;
        }
    }
}