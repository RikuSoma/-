using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// プレイヤーが一定位置に到達したら罠を作動させる
public class PositionEventTrigger : IEventTrigger
{
    private Vector2 triggerPosition;
    private Action onTrigger;

    public PositionEventTrigger(Vector2 position, Action action)
    {
        triggerPosition = position;
        onTrigger = action;
    }

    public void Trigger()
    {
        Debug.Log($"イベント発動地点 {triggerPosition} に到達！");
        onTrigger?.Invoke();
    }
}