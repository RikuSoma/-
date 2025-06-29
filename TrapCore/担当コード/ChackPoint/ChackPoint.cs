using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �`�F�b�N�|�C���g�N���X
public class ChackPoint : MonoBehaviour
{
    private bool StayPlayer;
    private ChackPointManager manager;

    public void Initialize(ChackPointManager _manager)
    {
        manager = _manager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();

        if (playerController != null)
        {
            StayPlayer = true;
            manager?.SetActiveCheckpoint(this);
            // �ז��Ȃ̂œ�����
            gameObject.SetActive(false);
        }
    }

    public bool GetFlag() => StayPlayer;
    public void SetFlag(bool flag) => StayPlayer = flag;
}