using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected CharacterHealthManager healthManager;
    [SerializeField] protected GameObject piecesPrefab;
    [SerializeField] protected int GomiNum;
    [SerializeField] protected float GomiLifeTime;

    // ���݂̑��x�{����ێ�����ϐ�
    protected float currentSpeedModifier = 1.0f;

    protected void Start()
    {
        healthManager = GetComponent<CharacterHealthManager>();
    }

    virtual protected void Explode()
    {
        for (int i = 0; i < GomiNum; i++)
        {
            GameObject gomi = Instantiate(piecesPrefab, transform.position, Quaternion.identity);
            gomi.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-3f, 3f), Random.Range(2f, 5f), Random.Range(-3f, 3f)));
            Destroy(gomi, GomiLifeTime);
        }
    }

    public virtual void ApplySpeedModifier(float modifier)
    {
        currentSpeedModifier = modifier;
    }

    protected virtual void OnDestroy()
    {
        if (EnemyCounter.Instance != null)
        {
            EnemyCounter.Instance.RemoveEnemy(gameObject);
            Debug.Log($"{gameObject.name} ���j�����ꂽ���߁A�G�J�E���^�[����폜���܂����B���݂̓G��: {EnemyCounter.Instance.EnemyCountInView}");
        }
    }
}
