using UnityEngine;
using UnityEngine.UI;

public class GaugeUIManager : MonoBehaviour
{
    [SerializeField] private Image[] maskImages; // 既に配置されたイメージ

    private BulletSkill bulletSkill;
    private const float gaugePerMask = 1f;  // 1個つき１ゲージ

    private void Start()
    {
        bulletSkill = new BulletSkill();
    }

    private void Update()
    {
        // bulletskillがnullの場合実行しない
        if (bulletSkill == null)
        {
            Debug.Log("nullだ");
            return;
        }

        // ゲージの変数宣言後bulletskillの現在ゲージ情報を持ってくる
        float gauge = bulletSkill.GetGauge();

        // 上の情報をもとにゲージの状況を画面に表示する
        for (int i = 0; i < maskImages.Length; i++)
        {
            float value = gauge - i * gaugePerMask;
            if (value <= 0f)
                maskImages[i].fillAmount = 0f;
            else if (value >= 1f)
                maskImages[i].fillAmount = 1f;
            else
                maskImages[i].fillAmount = value;
        }
    }


}
