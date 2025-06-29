using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AplleTrap : MonoBehaviour, ITrap
{
    [SerializeField] private int Power;

    public void Activate()
    {
        Debug.Log("ƒgƒ‰ƒbƒv’Ç‰Á");
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
