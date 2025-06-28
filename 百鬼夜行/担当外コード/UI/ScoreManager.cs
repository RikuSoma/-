using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TimeText;

    [SerializeField]
    TextMeshProUGUI HPBounusText;

    [SerializeField]
    TextMeshProUGUI SpecialSkillText;

    [SerializeField]
    TextMeshProUGUI TechnicalPointText;

    [SerializeField]
    TextMeshProUGUI Rank;

    private GameManager gameManager;

    private TimerManager timerManager;

    private GameData _gameData;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        timerManager = FindObjectOfType<TimerManager>();

        _gameData = FindAnyObjectByType<GameData>();

        if (GameData.Instance != null)
        {
            ShowPlayTimer();
            ShowHPBonus();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShowPlayTimer();

        ShowHPBonus();
    }

    private void ShowPlayTimer()
    {
        if (timerManager != null)
        {
            float time = GameData.Instance.PlayTime;
            TimeText.text = TimerManager.FormatTime(time);
        }
        
    }

    private void ShowHPBonus()
    {
        float current = GameData.Instance.PlayerHP;
        Debug.Log("hoge" + current);
        float max = GameData.Instance.PlayerMaxHP;

        HPBounusText.text = PlayerHP.FormatHP(current, max);
    }
    
}
