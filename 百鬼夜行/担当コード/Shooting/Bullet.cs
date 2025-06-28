using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("�e�ݒ�")]
    [SerializeField] protected float power = 1.0f;
    [SerializeField] protected float lifeTime = 5f;
    [SerializeField] protected GameObject ExplosionObject;
    protected GameObject Owner;
    [SerializeField, TagSelector] protected List<string> ignoreTags = null;

    protected void Start()
    {
        Destroy(gameObject, lifeTime); // ��莞�Ԍ�ɍ폜
    }

    virtual protected void OnTriggerEnter(Collider collision)
    {
        // �������X�g�ɂ���^�O�Ȃ�X���[
        if (ignoreTags.Contains(collision.tag) || collision.gameObject == Owner) return;

        // �U������
        var target = collision.GetComponent<CharacterHealthManager>();
        if (target != null && collision.gameObject != Owner)
        {
            target.ApplyDamage(power);
        }

        // �����I�u�W�F�N�g����
        if (ExplosionObject != null)
        {
            GameObject obj = Instantiate(ExplosionObject, transform.position, Quaternion.identity);
            Destroy(obj, 2);
        }

        // �e���폜
        Destroy(gameObject);
    }

    public void SetOwner(GameObject owner) => Owner = owner;

    public void SetIgnoreTags(List<string> tags) => ignoreTags = tags;

    public float GetPower() => power;

    public void SetPower(float Power) { power = Power; }

    public float GetLifeTime() => lifeTime;

    public void SetLifeTime(float lifetime) { lifeTime = lifetime; }
}