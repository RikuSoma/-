using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// �v���C���[�����ʒu�ɓ��B������㩂��쓮������
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
        Debug.Log($"�C�x���g�����n�_ {triggerPosition} �ɓ��B�I");
        onTrigger?.Invoke();
    }
}