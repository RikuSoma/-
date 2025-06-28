using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Dont Destoroy クラス
/// 用途：シーン間の破棄したくないものを登録する
/// 作成者：23Cu0216相馬理玖
/// 作成日：2024/10/07 作成開始
/// </summary>


public class DontDestoroyGameObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_prefabs;

    public Scene Destoroy { get; private set; }

    void Start()
    {
        // 自信を消さないようにする
        DontDestroyOnLoad(this);

        // 要素数取得
        int Index = m_prefabs.Count;

        // DontDestroyOnLoadに登録
        for (int i = 0; i < Index; ++i)
        {
            DontDestroyOnLoad(m_prefabs[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // リストの追加
    public void AddList(GameObject gameobject)
    {
        int num = m_prefabs.Count;
        bool ret = true;
        for (int i = 0; i < num; ++i)
        {
            // 追加しようとしているGameObjectに同じものがあるか検索
            if (m_prefabs[i] == gameobject) ret = false;
        }
        // 同じものがなければ
        if (!ret)
        {
            // リストに追加
            m_prefabs.Add(gameObject);
        }

        // DontDestroyOnLoadに登録
        DontDestroyOnLoad(gameobject);
    }

    // リストの削除
    public void RemoveList(GameObject gameobject)
    {
        int num = m_prefabs.Count;
        bool ret = false;
        for (int i = 0; i < num; ++i)
        {
            // 削除しようとしているGameObjectに同じものがあるか検索
            if (m_prefabs[i] == gameobject) ret = true;
        }
        // 同じものがあれば
        if (ret)
        {
            // Dontdestoroyから削除する
            SceneManager.MoveGameObjectToScene(gameobject,Destoroy);
        }

        // リストから削除する
        int Index = m_prefabs.Count;

        for (int i = 0; i < Index; ++i)
        {
            // 検索をかけ同じものが出れば
            if (m_prefabs[i] == gameobject)
            {
                //　削除する
                m_prefabs.Remove(gameobject);
            }
        }
    }
}
