using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isPlayerDead = false;

    [SerializeField] private GameObject youDeadUI;
    private GameObject deadUI;

    [SerializeField] private float InvalidTime;
    private float activetime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ����d�v�I
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // UI����
        deadUI = Instantiate(youDeadUI, Vector3.zero, Quaternion.identity);

        RectTransform rect = deadUI.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = Vector2.zero;
        }

        deadUI.SetActive(false);

        PlayerHealthManager playerHealth = FindObjectOfType<PlayerHealthManager>();
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath += OnPlayerDeath;
        }
        else
        {
            Debug.LogWarning("PlayerHealthManager ��������܂���I");
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // �o�^����
    }

    void Update()
    {
        if (isPlayerDead)
        {
            activetime += Time.deltaTime;
            if (activetime >= InvalidTime)
            {
                if (Input.anyKeyDown)
                {
                    RestartScene();
                }
            }
        }
    }

    private void OnPlayerDeath()
    {
        Debug.Log("GameManager: �v���C���[�����S���܂���");
        isPlayerDead = true;

        if (youDeadUI != null)
        {
            Vector3 centerWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
            deadUI.transform.position = centerWorldPos;
            deadUI.SetActive(true);
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // UI ����蒼��
        if (deadUI == null)
        {
            deadUI = Instantiate(youDeadUI, Vector3.zero, Quaternion.identity);
            DontDestroyOnLoad(deadUI);
            deadUI.SetActive(false);
            RectTransform rect = deadUI.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero;
            }
        }
        else
        {
            deadUI.SetActive(false);
        }

            GameObject newPlayer = GameObject.FindWithTag("Player");
        if (newPlayer != null)
        {
            newPlayer.transform.position = GameData.Instance.GetPlayerInitPos();

            // �ēo�^���K�v�I
            PlayerHealthManager playerHealth = newPlayer.GetComponent<PlayerHealthManager>();
            if (playerHealth != null)
            {
                playerHealth.OnPlayerDeath += OnPlayerDeath;
            }

            isPlayerDead = false;
            activetime = 0f;
        }
    }
}
