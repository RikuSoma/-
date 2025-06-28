using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Playables;
using System;


#if UNITY_EDITOR
using static UnityEditor.Experimental.GraphView.GraphView;
#endif

public class PlayerHP : MonoBehaviour
{
	private Player _player;

	private GameData _gameData;

	[SerializeField]
	private CharacterHealthManager healthManager;

	[SerializeField] 
	private TextMeshProUGUI HPText;

	private GameManager _gameManager;

	[SerializeField] 
	private Image hpBackgroundImage;

	private Coroutine RedCoroutine;

	private bool isBlinking = false;

	private bool isDeadSceneLoaded = false;

	private float _playerHP = 0.0f;

    private float MaxHP = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		_player = FindObjectOfType<Player>();

		_gameData = FindAnyObjectByType<GameData>();

        _gameManager = FindAnyObjectByType<GameManager>();


        if (_player != null)
		{
			healthManager = _player.GetComponent<CharacterHealthManager>();
			if (healthManager != null)
			{
				
				healthManager.OnDeath += OnPlayerDeath;
			}
			
		}
		
	}

    // Update is called once per frame
    void Update()
    {
		if (healthManager != null && HPText != null)
		{
			HPText.text = "HP. " + Mathf.Max(0, (int)healthManager.GetHelth());
		}

		if (healthManager != null && HPText != null)
		{
			float currentHP = healthManager.GetHelth();

            _playerHP = currentHP;

			//Debug.Log("プレイヤー体力：" + _playerHP);

            HPText.text = "HP: " + Mathf.Max(0, (int)currentHP);

			if (currentHP <= 1)
			{
				if (!isBlinking && RedCoroutine == null)
				{
					RedCoroutine = StartCoroutine(BlinkBackground());
					isBlinking = true;
				}
			}
			else
			{
				if (isBlinking)
				{
					StopCoroutine(RedCoroutine);
					RedCoroutine = null;
					hpBackgroundImage.color = Color.white;
					isBlinking = false;
				}
			}
		}

	}

	private IEnumerator BlinkBackground()
	{
		Color blinkColor = Color.red;
		Color normalColor = Color.white;

		while (true)
		{
			hpBackgroundImage.color = blinkColor;
			yield return new WaitForSeconds(0.5f);

			hpBackgroundImage.color = normalColor;
			yield return new WaitForSeconds(0.5f);
		}
	}

	private void OnPlayerDeath()
	{

		if (!isDeadSceneLoaded)
		{
			isDeadSceneLoaded = true;

            _gameManager.ChangeOverScene();
		}
	}

	public void StopCountHP()
	{
		if(_gameData != null)
		{
			_gameData.savePlayerHP(_playerHP);
		}
	}

    public static string FormatHP(float currentHP, float MaxHP)
    {
        
        return string.Format("{0:D1}/{1:D1}", (int)currentHP, (int)MaxHP);
    }
}

