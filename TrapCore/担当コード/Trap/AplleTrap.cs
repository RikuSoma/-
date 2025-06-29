using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AplleTrap : MonoBehaviour, ITrap
{
    [SerializeField] private int Power;

    public void Activate()
    {
        Debug.Log("�g���b�v�ǉ�");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealthManager playerhealth = collision.GetComponent<PlayerHealthManager>();

        if(playerhealth)
        {
            playerhealth.TakeDamage(Power);
        }
    }
}
