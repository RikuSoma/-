using UnityEngine;

public class LiftManager : MonoBehaviour
{
    [Header("移動範囲（X座標）")]
    public float leftX = -5f;     // 左端座標
    public float rightX = 5f;     // 右端座標
    public float speed = 2f;      // 移動速度

    private bool movingRight = true;

    void Update()
    {
        // 左右に移動処理
        Vector3 pos = transform.position;

        if (movingRight)
        {
            pos.x += speed * Time.deltaTime;
            if (pos.x >= rightX)
                movingRight = false;
        }
        else
        {
            pos.x -= speed * Time.deltaTime;
            if (pos.x <= leftX)
                movingRight = true;
        }

        transform.position = pos;
    }

    // プレイヤーが乗ったら子オブジェクトにして一緒に移動
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    // プレイヤーが降りたら親子関係を解除
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
