using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �g���b�v�`��p
public class TrapRenderer : ICellRenderer
{
    [SerializeField] private GameObject trapPrefab;  // �g���b�v�̃v���n�u

    public override void RenderCell(Vector2 position)
    {
        Instantiate(trapPrefab, position, Quaternion.identity);
    }
}
