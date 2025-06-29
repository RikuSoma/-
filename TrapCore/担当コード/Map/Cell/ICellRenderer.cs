using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICellRenderer : MonoBehaviour
{
    public abstract void RenderCell(Vector2 position);
}
