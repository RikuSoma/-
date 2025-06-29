using System;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int Health;

    [SerializeField] private GameObject piecesPrefab;

    // HP変化時のイベント
    public event Action<int, int> OnHealthChanged; // (現在HP, 最大HP)
    public event Action OnPlayerDeath;

    void Start()
    {
        Health = maxHealth;
        OnHealthChanged?.Invoke(Health, maxHealth);
    }

    private void Update()
    {
        if(IsDead())
        {
            for (int i = 0; i < 100; i++)
            {
                GameObject gomi = Instantiate(piecesPrefab, transform.position, Quaternion.identity);
                gomi.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(10f, 50f)));
            }

            Destroy(gameObject);
        }
    }

    public bool IsDead() { return Health <= 0; }

    // ダメージを受ける
    public void TakeDamage(int damage)
    {
        if (damage < 0) return;

        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
        }

        Debug.Log($"ダメージを受けた: -{damage} 現在HP: {Health}/{maxHealth}");
        OnHealthChanged?.Invoke(Health, maxHealth);

        if (Health == 0)
        {
            Debug.Log("プレイヤー死亡！");
            OnPlayerDeath?.Invoke();
        }
    }

    // 回復する
    public void Heal(int healAmount)
    {
        if (healAmount < 0) return;

        Health += healAmount;
        if (Health > maxHealth)
        {
            Health = maxHealth;
        }

        Debug.Log($"回復した: +{healAmount} 現在HP: {Health}/{maxHealth}");
        OnHealthChanged?.Invoke(Health, maxHealth);
    }

    // HPを取得
    public int GetCurrentHealth() => Health;
    public int GetMaxHealth() => maxHealth;
}
