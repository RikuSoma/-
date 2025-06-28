using UnityEngine;
using System.Collections;

public class BlueEnemy : EnemyBase
{
    private Collider enemyCollider;
    private Rigidbody rb;
    private BlueEnemyMoving moving;

    [SerializeField] private float reviveTime = 5f;
    [SerializeField] private Vector3 squashedScale = new Vector3(1f, 0.3f, 1f); // �ׂꂽ�T�C�Y
    [SerializeField] private string enemyLayer = "Enemy";
    [SerializeField] private string invincibleLayer = "InvincibleEnemy";

    private Vector3 originalScale;
    private int defaultLayer;

    new void Start()
    {
        healthManager = GetComponent<CharacterHealthManager>();
        enemyCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        moving = GetComponent<BlueEnemyMoving>();

        originalScale = transform.localScale;
        defaultLayer = LayerMask.NameToLayer(enemyLayer);

        if (healthManager != null)
        {
            healthManager.OnDeath += OnEnemyDeath;
        }
    }

    private void OnEnemyDeath()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        float originalHeight = transform.localScale.y;
        float squashedHeight = squashedScale.y;
        float heightDifference = (originalHeight - squashedHeight) * 0.5f;

        // �ׂ��
        transform.localScale = squashedScale;
        transform.position -= new Vector3(0f, heightDifference, 0f);

        // ���C���[�ύX���ďՓ˖�����
        gameObject.layer = LayerMask.NameToLayer(invincibleLayer);

        // ���͉�
        enemyCollider.enabled = false;
        if (rb != null) rb.isKinematic = true;
        if (moving != null) moving.enabled = false;

        // �k����O�܂ő҂�
        float shakeDuration = reviveTime / 2.5f;
        float waitBeforeShake = reviveTime - shakeDuration;
        if (waitBeforeShake > 0f)
            yield return new WaitForSeconds(waitBeforeShake);

        // �k���鉉�o
        float elapsed = 0f;
        float shakeStrength = 0.05f;
        Vector3 basePosition = transform.position;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-shakeStrength, shakeStrength);
            float offsetY = Random.Range(-shakeStrength, shakeStrength);
            transform.position = basePosition + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // ���̈ʒu�E�T�C�Y�ɖ߂�
        transform.position = basePosition + new Vector3(0f, heightDifference, 0f);
        transform.localScale = originalScale;

        // ���C���[�߂�
        gameObject.layer = defaultLayer;

        // ����
        enemyCollider.enabled = true;
        if (rb != null) rb.isKinematic = false;
        if (moving != null) moving.enabled = true;

        healthManager.Recovery(1f); // HP��
    }

    private void OnDestroy()
    {
        // �������[���[�N���
        if (healthManager != null)
        {
            healthManager.OnDeath -= OnEnemyDeath;
        }
    }
}
