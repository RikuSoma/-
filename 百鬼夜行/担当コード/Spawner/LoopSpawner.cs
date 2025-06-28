using UnityEngine;

// 
// �O��X�|�[�������G���|����Ă���A��莞�Ԍ�Ɏ��̓G���X�|�[������
// 

public class LoopSpawner : MonoBehaviour
{
    [Header("�X�|�[���ݒ�")]
    [SerializeField] private bool CanSpawn = true;
    [SerializeField] private GameObject SpawnObject;
    [SerializeField] private float SpawnInterval = 2.0f;
    [SerializeField] private GameObject TargetObject;

    private float spawnTimer;
    private bool isTargetNeeded = false;
    private GameObject lastSpawnedObject;

    void Start()
    {
        // ... Start���\�b�h�̒��g�͕ύX�Ȃ� ...
        if (SpawnObject == null)
        {
            Debug.LogError("�X�|�[������I�u�W�F�N�g(SpawnObject)���ݒ肳��Ă��܂���B", this);
            CanSpawn = false;
            return;
        }
        if (SpawnObject.GetComponent<EnemyBase>() == null)
        {
            Debug.LogError("�X�|�[������I�u�W�F�N�g���G(EnemyBase)�ł͂���܂���B", this);
            CanSpawn = false;
            return;
        }
        if (SpawnObject.GetComponent<TargetingEnemy>() != null)
        {
            isTargetNeeded = true;
            if (TargetObject == null)
            {
                Debug.LogError("�^�[�Q�b�g���K�v�ȓG�ł����A�^�[�Q�b�g�I�u�W�F�N�g(TargetObject)���ݒ肳��Ă��܂���B", this);
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

        // ������ �������烍�W�b�N���C�� ������

        // 1. �O��X�|�[�������G���܂����݂��Ă���ꍇ�́A�������Ȃ�
        if (lastSpawnedObject != null)
        {
            return; // �G���|�����܂őҋ@
        }

        // 2. �G�����Ȃ��ꍇ�̂݁A���̃X�|�[���܂ł̃^�C�}�[��i�߂�
        spawnTimer += Time.deltaTime;

        // 3. �^�C�}�[���ݒ莞�Ԃ𒴂�����A�V�����G���X�|�[������
        if (spawnTimer >= SpawnInterval)
        {
            // �I�u�W�F�N�g���X�|�[��
            GameObject spawnedObject = Instantiate(SpawnObject, transform.position, Quaternion.identity);

            // �X�|�[�������I�u�W�F�N�g���L��
            lastSpawnedObject = spawnedObject;

            if (isTargetNeeded)
            {
                TargetingEnemy targetingEnemy = spawnedObject.GetComponent<TargetingEnemy>();
                if (targetingEnemy != null)
                {
                    targetingEnemy.Target = TargetObject;
                }
            }

            // �^�C�}�[�����Z�b�g���āA���́u�G���|���ꂽ��v�̃J�E���g�_�E���ɔ�����
            spawnTimer = 0.0f;
        }
    }
}