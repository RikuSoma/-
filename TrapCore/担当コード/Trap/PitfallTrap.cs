using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 落とし穴クラス
public class PitfallTrap : ITrap
{
    public void Activate()
    {
        Debug.Log("落とし穴発動！プレイヤーが落ちる！");
    }
}