using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public Vector3 viewportOffset; // Viewport ���W�̃I�t�Z�b�g (0-1�͈̔͐���)
        [Range(0f, 1f)] public float spawnProbability;
    }

    [SerializeField] private List<EnemySpawnInfo> enemySpawnInfos;
    [SerializeField] private int maxEnemiesInView = 5;
    [SerializeField] private float spawnInterval = 3f;
    private float nextSpawnTime;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera ���V�[���ɑ��݂��܂���I");
        }
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (EnemyCounter.Instance == null || mainCamera == null) return;

        if (Time.time >= nextSpawnTime && EnemyCounter.Instance.EnemyCountInView < maxEnemiesInView)
        {
            SpawnRandomEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnRandomEnemy()
    {
        if (enemySpawnInfos.Count == 0)
        {
            Debug.LogWarning("��������G�̏�񂪐ݒ肳��Ă��܂���B");
            return;
        }

        float randomIndex = Random.Range(0f, CalculateTotalProbability());
        EnemySpawnInfo selectedEnemyInfo = GetEnemyToSpawn(randomIndex);

        if (selectedEnemyInfo.enemyPrefab == null)
        {
            Debug.LogWarning($"�GPrefab���ݒ肳��Ă��܂���: {selectedEnemyInfo.enemyPrefab?.name}");
            return;
        }

        Vector3 spawnViewportPosition = new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane + 10f) + selectedEnemyInfo.viewportOffset;
        Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(spawnViewportPosition);
        Quaternion spawnRotation = Quaternion.identity;

        // Z�����Œ肷��ꍇ�ȂǁA�K�v�ɉ����Ē���
        spawnPosition.z = 0f;

        GameObject spawnedEnemy = Instantiate(selectedEnemyInfo.enemyPrefab, spawnPosition, spawnRotation);

        // �^�[�Q�b�g�ݒ�
        TargetingEnemy targetingComponent = spawnedEnemy.GetComponent<TargetingEnemy>();
        if (targetingComponent != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                targetingComponent.Target = player;
                Debug.Log($"{spawnedEnemy.name} �Ƀ^�[�Q�b�g (Player) ��ݒ肵�܂����B");
            }
            else
            {
                Debug.LogWarning($"{spawnedEnemy.name} �� TargetingEnemy �R���|�[�l���g�������Ă��܂����APlayer �^�O�̃I�u�W�F�N�g��������܂���B");
            }
        }
    }

    float CalculateTotalProbability()
    {
        float totalProbability = 0f;
        foreach (var enemyInfo in enemySpawnInfos)
        {
            totalProbability += enemyInfo.spawnProbability;
        }
        return totalProbability;
    }

    EnemySpawnInfo GetEnemyToSpawn(float randomValue)
    {
        float cumulativeProbability = 0f;
        foreach (var enemyInfo in enemySpawnInfos)
        {
            cumulativeProbability += enemyInfo.spawnProbability;
            if (randomValue <= cumulativeProbability)
            {
                return enemyInfo;
            }
        }
        return enemySpawnInfos[0]; // �O�̂��� (�ʏ�͂����ɂ͗��Ȃ��͂�)
    }
}