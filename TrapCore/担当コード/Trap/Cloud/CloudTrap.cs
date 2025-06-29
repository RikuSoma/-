using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudTrap : TrapBase
{
    private CloudAnimation cloudAnimation;

    void Awake()
    {
        cloudAnimation = GetComponent<CloudAnimation>();
    }

    override public void Activate()
    {
        Debug.Log("�N���g���b�v�����I");
        cloudAnimation?.PlayActivatedAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealthManager playerhealth = collision.GetComponent<PlayerHealthManager>();

        if (playerhealth)
        {
            playerhealth.TakeDamage(Power);
            Activate();
        }
    }
}
