using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fake : MonoBehaviour
{
    [SerializeField] private float colorChangeSpeed = 1f; // 色の変化速度
    private Renderer objRenderer;
    private float hue;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        hue += Time.deltaTime * colorChangeSpeed;
        if (hue > 1f) hue -= 1f; // 0〜1の範囲に収める

        Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);
        objRenderer.material.color = rainbowColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if(playerController != null)
        {
            Destroy(gameObject);
        }
    }
}
