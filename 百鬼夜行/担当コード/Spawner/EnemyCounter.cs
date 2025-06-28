using UnityEngine;
using System.Collections.Generic;

public class EnemyCounter : MonoBehaviour
{
    public static EnemyCounter Instance { get; private set; }

    private HashSet<GameObject> enemiesInView = new HashSet<GameObject>();
    [SerializeField] private int enemyCountInView; // �V���A���C�Y���ăC���X�y�N�^�[�ɕ\��
    public int EnemyCountInView => enemyCountInView;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // DontDestroyOnLoad(gameObject); // �K�v�ł����
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null && !enemiesInView.Contains(other.gameObject))
        {
            enemiesInView.Add(other.gameObject);
            UpdateEnemyCountInInspector();
            Debug.Log($"�J����������ɓ������G: {other.gameObject.name}, ���݂̓G��: {EnemyCountInView}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemiesInView.Contains(other.gameObject))
        {
            enemiesInView.Remove(other.gameObject);
            UpdateEnemyCountInInspector();
            Debug.Log($"�J��������O�ɏo���G: {other.gameObject.name}, ���݂̓G��: {EnemyCountInView}");
        }
    }

    // Destroy���ꂽ�G���J�E���g����폜���邽�߂̃��\�b�h
    public void RemoveEnemy(GameObject enemy)
    {
        if (enemiesInView.Contains(enemy))
        {
            enemiesInView.Remove(enemy);
            UpdateEnemyCountInInspector();
            Debug.Log($"�G���폜���܂��� (Destroy): {enemy.name}, ���݂̓G��: {EnemyCountInView}");
        }
    }

    private void UpdateEnemyCountInInspector()
    {
        enemyCountInView = enemiesInView.Count;
    }

    public int GetEnemyCountInView() => EnemyCountInView;
    public HashSet<GameObject> GetEnemiesInView() => enemiesInView;
}