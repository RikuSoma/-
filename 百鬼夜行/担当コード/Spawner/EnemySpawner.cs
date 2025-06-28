using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private float spawnDistanceThreshold = 10f; // �X�|�[�����鋗����臒l (XY����)
    [SerializeField] private TextAsset csvFile; // TextAsset �Ƃ��� CSV �t�@�C�����A�T�C��
    [SerializeField] private List<EnemyPrefabMapping> prefabMappings; // �G�̎�ނ�Prefab�̑Ή�
    [SerializeField] private bool spawnOnce = true; // �e�G�̔z�u�ɂ���x�̂݃X�|�[�����邩�ǂ���

    private List<Dictionary<string, string>> enemySpawnData;
    private HashSet<string> spawnedEnemyKeys = new HashSet<string>(); // �X�|�[���ς݂̓G�̃L�[�i��� + �ʒu�j���L�^

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
            // XY���ʂł̋������v�Z
            float distanceToPlayerXY = Vector2.Distance(new Vector2(spawnPosition.x, spawnPosition.y), new Vector2(playerObject.transform.position.x, playerObject.transform.position.y));

            // �X�|�[���ς݂̓G�����ʂ��邽�߂̃L�[���쐬 (��� + �ʒu)
            string spawnKey = $"{enemyType}_{positionX}_{positionY}_{positionZ}";

            // spawnOnce �� true ���A�܂����̔z�u�̓G���X�|�[�������Ă��Ȃ��ꍇ�̂݃X�|�[������
            if (distanceToPlayerXY <= spawnDistanceThreshold && (!spawnOnce || !spawnedEnemyKeys.Contains(spawnKey)))
            {
                enemiesToSpawn.Add(data);
                spawnedThisFrame.Add(data); // �X�|�[���Ώۂɒǉ�
            }
        }

        if (enemiesToSpawn.Count > 0)
        {
            SpawnEnemies(enemiesToSpawn);
            // �X�|�[�������G�̃L�[���L�^
            foreach (var spawnedData in spawnedThisFrame)
            {
                string spawnedEnemyType = spawnedData.ContainsKey("EnemyType") ? spawnedData["EnemyType"] : "";
                float positionX = spawnedData.ContainsKey("PositionX") ? float.Parse(spawnedData["PositionX"]) : transform.position.x;
                float positionY = spawnedData.ContainsKey("PositionY") ? float.Parse(spawnedData["PositionY"]) : transform.position.y;
                float positionZ = spawnedData.ContainsKey("PositionZ") ? float.Parse(spawnedData["PositionZ"]) : transform.position.z;
                string spawnKey = $"{spawnedEnemyType}_{positionX}_{positionY}_{positionZ}";
                spawnedEnemyKeys.Add(spawnKey);
                // �X�|�[���ς݃f�[�^�̓��X�g����폜 (��x�����X�|�[�����Ȃ��ꍇ)
                enemySpawnData.Remove(spawnedData);
            }
        }
    }

    private List<Dictionary<string, string>> LoadEnemyDataFromTextAsset(TextAsset file)
    {
        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

        if (file == null)
        {
            Debug.LogError("TextAsset ���A�T�C������Ă��܂���B");
            return dataList;
        }

        using (StringReader reader = new StringReader(file.text))
        {
            string headerLine = reader.ReadLine();
            if (headerLine == null)
            {
                Debug.LogError("CSV�t�@�C������ł��B");
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
                    Debug.LogWarning($"CSV�t�@�C���̍s�̗v�f�����w�b�_�[�ƈ�v���܂���: {line}");
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

                // �^�[�Q�b�g�����G�ł���΃^�[�Q�b�g��ݒ�
                TargetingEnemy targetingEnemy = spawnedEnemy.GetComponent<TargetingEnemy>();
                if (targetingEnemy != null && playerObject != null)
                {
                    targetingEnemy.Target = playerObject;
                    Debug.Log($"{enemyType} �Ƀ^�[�Q�b�g��ݒ�: {playerObject.name}");
                }
                else if (targetingEnemy != null && playerObject == null)
                {
                    Debug.LogWarning($"{enemyType} �̓^�[�Q�b�g�����G�ł����APlayerObject ���ݒ肳��Ă��܂���B");
                }

                Debug.Log($"Spawned: {enemyType} at {spawnPosition}");
            }
            else
            {
                Debug.LogError($"Prefab��������܂���: {enemyType}");
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
        // XY���ʂł̋������l�������M�Y���̕`��͏������G�ɂȂ邽�߁A�����ł�Spawner�̈ʒu�𒆐S�ɕ`�悵�܂�
        Gizmos.DrawWireSphere(transform.position, spawnDistanceThreshold);
    }
}