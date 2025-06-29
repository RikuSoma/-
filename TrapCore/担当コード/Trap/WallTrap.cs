using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 壁が出現する罠クラス
public class WallTrap : ITrap
{
    public void Activate()
    {
        Debug.Log("壁が出現！道が塞がれる！");
    }
}
