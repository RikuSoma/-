using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    [SerializeField] private Vector2 playerInitPos = new Vector2(3,-7);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector2 GetPlayerInitPos() => playerInitPos;
    public void SetPlayerInitPos(Vector2 pos) => playerInitPos = pos;
}