using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class KonamiCommand : MonoBehaviour
{
    private List<KeyCode> konamiCommandSequence = new List<KeyCode> {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.B,
        KeyCode.A
    };

    [SerializeField]
    private bool[] konamiCommandProgress;

    private bool skipThisFrame = false; // ✅ 1フレームスキップ用フラグ

    public bool[] KonamiCommandProgress => konamiCommandProgress;

    void Awake()
    {
        konamiCommandProgress = new bool[konamiCommandSequence.Count];
    }

    void Update()
    {
        // ✅ コマンド発動後の1フレームだけ入力処理をスキップ
        if (skipThisFrame)
        {
            skipThisFrame = false;
            return;
        }

        CheckKonamiCommandInput();
    }

    private void CheckKonamiCommandInput()
    {
        if (Input.anyKeyDown)
        {
            KeyCode currentKey = GetCurrentKeyDown();
            if (currentKey != KeyCode.None)
            {
                int nextInputIndex = konamiCommandProgress.TakeWhile(b => b).Count();

                if (nextInputIndex >= konamiCommandSequence.Count)
                    return;

                KeyCode expectedKey = konamiCommandSequence[nextInputIndex];

                if (currentKey == expectedKey)
                {
                    konamiCommandProgress[nextInputIndex] = true;

                    if (konamiCommandProgress.All(b => b))
                    {
                        ExecuteKonamiCommandAction();
                        ResetKonamiCommandProgress();

                        skipThisFrame = true; // ✅ 次のUpdateはスキップ
                    }
                }
                else
                {
                    ResetKonamiCommandProgress(); // ❌誤入力ならリセット
                }
            }
        }
    }

    KeyCode GetCurrentKeyDown()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
                return key;
        }
        return KeyCode.None;
    }

    void ExecuteKonamiCommandAction()
    {
        Debug.Log("コナミコマンド（キーボード）が入力されました！");

        CharacterHealthManager healthManager = GetComponent<CharacterHealthManager>();
        if (healthManager != null)
        {
            // HP全回復（既存処理）
            float hp = 10000 - healthManager.GetHelth();
            healthManager.Recovery(hp);

            // ✅ 無敵状態をONにする
            typeof(CharacterHealthManager)
                .GetField("isInvincible", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(healthManager, true);

            Debug.Log("無敵モードになりました！");
        }

        // 弾の強化（既存処理）
        Shooting shooting = GetComponent<Shooting>();
        if (shooting != null)
        {
            Bullet bullet = shooting.GetBulletObject().GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetPower(bullet.GetPower() * 10);
                bullet.SetLifeTime(bullet.GetLifeTime() * 8);
            }

            float speed = shooting.GetBulletSpeed() * 2;
            shooting.SetBulletSpeed(speed);
        }

        PlayerShooting playerShooting = GetComponent<PlayerShooting>();
        if (playerShooting != null)
        {
            float interval = playerShooting.GetShootInterval() * 4;
            playerShooting.SetShootInterval(interval);
        }
    }

    void ResetKonamiCommandProgress()
    {
        for (int i = 0; i < konamiCommandProgress.Length; i++)
        {
            konamiCommandProgress[i] = false;
        }
    }
}
