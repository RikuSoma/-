using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("弾設定")]
    [SerializeField] protected float power = 1.0f;
    [SerializeField] protected float lifeTime = 5f;
    [SerializeField] protected GameObject ExplosionObject;
    protected GameObject Owner;
    [SerializeField, TagSelector] protected List<string> ignoreTags = null;

    protected void Start()
    {
        Destroy(gameObject, lifeTime); // 一定時間後に削除
    }

    virtual protected void OnTriggerEnter(Collider collision)
    {
        // 無視リストにあるタグならスルー
        if (ignoreTags.Contains(collision.tag) || collision.gameObject == Owner) return;

        // 攻撃処理
        var target = collision.GetComponent<CharacterHealthManager>();
        if (target != null && collision.gameObject != Owner)
        {
            target.ApplyDamage(power);
        }

        // 爆発オブジェクト生成
        if (ExplosionObject != null)
        {
            GameObject obj = Instantiate(ExplosionObject, transform.position, Quaternion.identity);
            Destroy(obj, 2);
        }

        // 弾を削除
        Destroy(gameObject);
    }

    public void SetOwner(GameObject owner) => Owner = owner;

    public void SetIgnoreTags(List<string> tags) => ignoreTags = tags;

    public float GetPower() => power;

    public void SetPower(float Power) { power = Power; }

    public float GetLifeTime() => lifeTime;

    public void SetLifeTime(float lifetime) { lifeTime = lifetime; }
}