using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private bool isPlayer = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayer = false;
        }
    }

    public bool IsPlayer()
    {
        return isPlayer;
    }
    public void ResetDetection()
    {
        isPlayer = false;
    }
}
