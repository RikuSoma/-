using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private float spawnDistanceThreshold = 10f; // スポーンする距離の閾値 (XY平面)
    [SerializeField] private TextAsset csvFile; // TextAsset として CSV ファイルをアサイン
    [SerializeField] private List<EnemyPrefabMapping> prefabMappings; // 敵の種類とPrefabの対応
    [SerializeField] private bool spawnOnce = true; // 各敵の配置につき一度のみスポーンするかどうか

    private List<Dictionary<string, string>> enemySpawnData;
    private HashSet<string> spawnedEnemyKeys = new HashSet<string>(); // スポーン済みの敵のキー（種類 + 位置）を記録

    [System.Serializable]
    public struct EnemyPrefabMapping
    {
        public string enemyType;
        public GameObject prefab;
    }

    void Start()
    {
        enemySpawnData = LoadEnemyDataFromTextAsset(csvFile);
    }

    void Update()
    {
        if (playerObject == null || enemySpawnData == null) return;

        List<Dictionary<string, string>> enemiesToSpawn = new List<Dictionary<string, string>>();
        List<Dictionary<string, string>> spawnedThisFrame = new List<Dictionary<string, string>>();

        foreach (var data in enemySpawnData)
        {
            string enemyType = data.ContainsKey("EnemyType") ? data["EnemyType"] : "";
            float positionX = data.ContainsKey("PositionX") ? float.Parse(data["PositionX"]) : transform.position.x;
            float positionY = data.ContainsKey("PositionY") ? float.Parse(data["PositionY"]) : transform.position.y;
            float positionZ = data.ContainsKey("PositionZ") ? float.Parse(data["PositionZ"]) : transform.position.z;

            Vector3 spawnPosition = new Vector3(positionX, positionY, positionZ);
            // XY平面での距離を計算
            float distanceToPlayerXY = Vector2.Distance(new Vector2(spawnPosition.x, spawnPosition.y), new Vector2(playerObject.transform.position.x, playerObject.transform.position.y));

            // スポーン済みの敵を識別するためのキーを作成 (種類 + 位置)
            string spawnKey = $"{enemyType}_{positionX}_{positionY}_{positionZ}";

            // spawnOnce が true かつ、まだこの配置の敵をスポーンさせていない場合のみスポーン判定
            if (distanceToPlayerXY <= spawnDistanceThreshold && (!spawnOnce || !spawnedEnemyKeys.Contains(spawnKey)))
            {
                enemiesToSpawn.Add(data);
                spawnedThisFrame.Add(data); // スポーン対象に追加
            }
        }

        if (enemiesToSpawn.Count > 0)
        {
            SpawnEnemies(enemiesToSpawn);
            // スポーンした敵のキーを記録
            foreach (var spawnedData in spawnedThisFrame)
            {
                string spawnedEnemyType = spawnedData.ContainsKey("EnemyType") ? spawnedData["EnemyType"] : "";
                float positionX = spawnedData.ContainsKey("PositionX") ? float.Parse(spawnedData["PositionX"]) : transform.position.x;
                float positionY = spawnedData.ContainsKey("PositionY") ? float.Parse(spawnedData["PositionY"]) : transform.position.y;
                float positionZ = spawnedData.ContainsKey("PositionZ") ? float.Parse(spawnedData["PositionZ"]) : transform.position.z;
                string spawnKey = $"{spawnedEnemyType}_{positionX}_{positionY}_{positionZ}";
                spawnedEnemyKeys.Add(spawnKey);
                // スポーン済みデータはリストから削除 (一度しかスポーンしない場合)
                enemySpawnData.Remove(spawnedData);
            }
        }
    }

    private List<Dictionary<string, string>> LoadEnemyDataFromTextAsset(TextAsset file)
    {
        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

        if (file == null)
        {
            Debug.LogError("TextAsset がアサインされていません。");
            return dataList;
        }

        using (StringReader reader = new StringReader(file.text))
        {
            string headerLine = reader.ReadLine();
            if (headerLine == null)
            {
                Debug.LogError("CSVファイルが空です。");
                return dataList;
            }
            string[] headers = headerLine.Split(',');

            while (reader.Peek() >= 0)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');
                if (values.Length == headers.Length)
                {
                    Dictionary<string, string> data = headers.Zip(values, (header, value) => new { Header = header, Value = value })
                        .ToDictionary(item => item.Header, item => item.Value);
                    dataList.Add(data);
                }
                else
                {
                    Debug.LogWarning($"CSVファイルの行の要素数がヘッダーと一致しません: {line}");
                }
            }
        }
        return dataList;
    }

    private void SpawnEnemies(List<Dictionary<string, string>> dataToSpawn)
    {
        foreach (var data in dataToSpawn)
        {
            string enemyType = data.ContainsKey("EnemyType") ? data["EnemyType"] : "";
            float positionX = data.ContainsKey("PositionX") ? float.Parse(data["PositionX"]) : transform.position.x;
            float positionY = data.ContainsKey("PositionY") ? float.Parse(data["PositionY"]) : transform.position.y;
            float positionZ = data.ContainsKey("PositionZ") ? float.Parse(data["PositionZ"]) : transform.position.z;

            GameObject prefabToSpawn = GetPrefabByType(enemyType);
            if (prefabToSpawn != null)
            {
                Vector3 spawnPosition = new Vector3(positionX, positionY, positionZ);
                GameObject spawnedEnemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

                // ターゲットを持つ敵であればターゲットを設定
                TargetingEnemy targetingEnemy = spawnedEnemy.GetComponent<TargetingEnemy>();
                if (targetingEnemy != null && playerObject != null)
                {
                    targetingEnemy.Target = playerObject;
                    Debug.Log($"{enemyType} にターゲットを設定: {playerObject.name}");
                }
                else if (targetingEnemy != null && playerObject == null)
                {
                    Debug.LogWarning($"{enemyType} はターゲットを持つ敵ですが、PlayerObject が設定されていません。");
                }

                Debug.Log($"Spawned: {enemyType} at {spawnPosition}");
            }
            else
            {
                Debug.LogError($"Prefabが見つかりません: {enemyType}");
            }
        }
    }

    private GameObject GetPrefabByType(string type)
    {
        foreach (var mapping in prefabMappings)
        {
            if (mapping.enemyType == type)
            {
                return mapping.prefab;
            }
        }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // XY平面での距離を考慮したギズモの描画は少し複雑になるため、ここではSpawnerの位置を中心に描画します
        Gizmos.DrawWireSphere(transform.position, spawnDistanceThreshold);
    }
}