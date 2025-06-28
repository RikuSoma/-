using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static PlayerController;

/// <summary>
/// PlayerAnimationクラス
/// Playerのアニメーションを行う
/// 作成者：23cu0216 相馬理玖
/// 作成日：2024/10/21 作成開始
/// </summary>

public class PlayerAnimation : MonoBehaviour
{
    PlayerStatus m_PlayerStatus;    // プレイヤーの状態

    SpriteRenderer m_sprite;    // Sprite格納用
    [SerializeField] private bool m_Istransparent;  // 透明化しているか？
    [SerializeField] private float m_transparentTime; // 透明化間隔
    [SerializeField] private float m_Time;

    [SerializeField] private GameObject m_exObj;    // 爆発用

	// Start is called before the first frame update
	void Start()
    {
        // PlayerControllerからPlayer情報を取得
        m_PlayerStatus = (PlayerStatus)GetComponent<PlayerController>().GetPlayerStatus();
        m_sprite = GetComponent<SpriteRenderer>();
        m_Istransparent = false;
        m_Time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // PlayerControllerからPlayer情報を取得
        m_PlayerStatus = (PlayerStatus)GetComponent<PlayerController>().GetPlayerStatus();

        switch (m_PlayerStatus)
        {
            // 待機状態
            case PlayerStatus.IDLE:
                break;
            // 通常状態
            case PlayerStatus.FINE:
                // ダメージを受けていた場合
                // Playerを点滅させる
                if(GetComponent<PlayerHitPoints>().IsDamage())
                {
					m_Time += Time.deltaTime;

					if (!m_Istransparent)
                    {
                        // 自身を透明化
                        m_sprite.color = new Vector4(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0.0f);
                        if (m_Time > m_transparentTime)
                        {
                            m_Istransparent = true;
                            m_Time = 0.0f;
                        }
					}
					else
                    {
						// 自身の透明化を解除
						m_sprite.color = new Vector4(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 1.0f);
						if (m_Time > m_transparentTime)
						{
							m_Istransparent = false;
							m_Time = 0.0f;
						}
					}
				}
                else
                {
					// 自身の透明化を解除
					m_sprite.color = new Vector4(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 1.0f);
				}
				break;
            // 死亡状態
            case PlayerStatus.DEAD:
                // 自身の場所に爆発オブジェクトをインスタンス化し
                Instantiate(m_exObj, transform.position, Quaternion.identity);

                // 自身は削除する
                Destroy(gameObject);
                break;
            // リスポーン状態
            case PlayerStatus.RESPAWN:
                break;
            // クリア状態
            case PlayerStatus.CLEAR:
                break;
            default:
                break;
        }
    }
}
