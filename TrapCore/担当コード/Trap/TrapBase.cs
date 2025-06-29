using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour ,ITrap
{
    [SerializeField] protected int Power = 100;
    virtual public void Activate()
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealthManager playerhealth = collision.GetComponent<PlayerHealthManager>();

        if (playerhealth)
        {
            playerhealth.TakeDamage(Power);
        }
    }
}
