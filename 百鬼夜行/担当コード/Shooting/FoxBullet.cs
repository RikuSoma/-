using TMPro;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class FoxBullet : Bullet
{
    private Shooting shooting;

    [Header("�u�[�������ݒ�")]
    [SerializeField] private float returnSpeed = 15f;
    [SerializeField] private float maxDistance = 15f;

    private Vector3 startPosition;
    private bool isReturning = false;
    private Transform ownerTransform;

    new void Start()
    {
        shooting = GameObject.Find("Player").GetComponent<Shooting>();

        startPosition = transform.position;
        if (Owner != null)
            ownerTransform = Owner.transform;
    }

    void Update()
    {
        if (!isReturning)
        {
            // �O�����ɐi��
            transform.Translate(Vector3.forward * shooting.GetBulletSpeed() * Time.deltaTime);

            // ��苗����񂾂�߂�t���O�𗧂Ă�
            if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
            {
                isReturning = true;
            }
        }
        else
        {
            // �v���C���[�Ɍ������Ė߂�
            if (ownerTransform != null)
            {
                Vector3 direction = (ownerTransform.position - transform.position).normalized;
                transform.position += direction * returnSpeed * Time.deltaTime;

                // �v���C���[�ɖ߂�����e������
                if (Vector3.Distance(ownerTransform.position, transform.position) < 1f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        // �������X�g�ɂ���^�O�Ȃ�X���[
        if (ignoreTags.Contains(collision.tag) || collision.gameObject == Owner) return;

        // �U������
        var target = collision.GetComponent<CharacterHealthManager>();
        if (target != null && collision.gameObject != Owner)
        {
            target.ApplyDamage(power); // TakeDamage ���� ApplyDamage �ɕύX
        }

        // �����I�u�W�F�N�g����
        if (ExplosionObject != null)
        {
            GameObject obj = Instantiate(ExplosionObject, transform.position, Quaternion.identity);
            Destroy(obj, 2);
        }
    }


    public void Initialize(GameObject owner)
    {
        SetOwner(owner);
        ownerTransform = owner.transform;
    }

}
