using TMPro;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class FoxBullet : Bullet
{
    private Shooting shooting;

    [Header("ブーメラン設定")]
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
            // 前方向に進む
            transform.Translate(Vector3.forward * shooting.GetBulletSpeed() * Time.deltaTime);

            // 一定距離飛んだら戻るフラグを立てる
            if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
            {
                isReturning = true;
            }
        }
        else
        {
            // プレイヤーに向かって戻る
            if (ownerTransform != null)
            {
                Vector3 direction = (ownerTransform.position - transform.position).normalized;
                transform.position += direction * returnSpeed * Time.deltaTime;

                // プレイヤーに戻ったら弾を消す
                if (Vector3.Distance(ownerTransform.position, transform.position) < 1f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        // 無視リストにあるタグならスルー
        if (ignoreTags.Contains(collision.tag) || collision.gameObject == Owner) return;

        // 攻撃処理
        var target = collision.GetComponent<CharacterHealthManager>();
        if (target != null && collision.gameObject != Owner)
        {
            target.ApplyDamage(power); // TakeDamage から ApplyDamage に変更
        }

        // 爆発オブジェクト生成
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
