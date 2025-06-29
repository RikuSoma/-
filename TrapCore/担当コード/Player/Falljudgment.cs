using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falljudgment : MonoBehaviour
{
    private int Power = 9999;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealthManager playerhealth = collision.GetComponent<PlayerHealthManager>();

        if (playerhealth != null)
        {
            playerhealth.TakeDamage(Power);
        }
    }
}
