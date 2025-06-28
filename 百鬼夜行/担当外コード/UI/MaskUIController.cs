using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// 仮面UIの表示を個別に管理するコントローラー
public class MaskUIController : MonoBehaviour
{
    [SerializeField] private Shooting shooting; // 現在の弾情報を取得するため
    [SerializeField] private GameObject[] centerImages; // 中央の仮面UI 3個
    [SerializeField] private GameObject[] leftImages;   // 左側の仮面UI 3個
    [SerializeField] private GameObject[] rightImages;  // 右側の仮面UI 3個

    private List<string> bulletNames = new() { "NohMaskBullet", "DemonBullet", "FoxBullet" };
    private GameObject lastBullet;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        var current = shooting.GetBulletObject();
        if (current != lastBullet)
        {
            lastBullet = current;
            UpdateUI();
        }
    }

    // 仮面表示UIを更新する処理
    void UpdateUI()
    {
        if (shooting.GetBulletObject() == null) return;

        // 現在の弾名を取得（Clone削除）
        string currentBulletName = shooting.GetBulletObject().name.Replace("(Clone)", "");

        // 該当するインデックスを取得
        int currentIndex = bulletNames.IndexOf(currentBulletName);
        if (currentIndex == -1)
        {
            Debug.LogWarning($"未登録の弾: {currentBulletName}");
            return;
        }

        // 全UIを一旦非表示
        foreach (var img in centerImages) img.SetActive(false);
        foreach (var img in leftImages) img.SetActive(false);
        foreach (var img in rightImages) img.SetActive(false);

        // 中央に現在の仮面
        centerImages[currentIndex].SetActive(true);

        // 左に次の仮面（順回り）
        int nextIndex = (currentIndex + 1) % bulletNames.Count;
        leftImages[nextIndex].SetActive(true);

        // 右に次の次の仮面（順回り）
        int afterNextIndex = (currentIndex + 2) % bulletNames.Count;
        rightImages[afterNextIndex].SetActive(true);
    }
}
