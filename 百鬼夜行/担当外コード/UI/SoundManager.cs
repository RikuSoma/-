using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Main CameraのAudioSource
    public AudioSource audioSource;

    // サウンドスライダー
    public Slider soundSlider;

    // ボリューム表示テキスト
    public TextMeshProUGUI volumeText;

    // 
    public float adjustSpeed = 0.0000000000000000000001f;

    
    void Start()
    {
        // 
        float initialVolume = VolumeSettings.Instance != null ? VolumeSettings.Instance.Volume : 0.5f;

        soundSlider.value = initialVolume;
        audioSource.volume = initialVolume;
        UpdateVolumeText(initialVolume);

        soundSlider.onValueChanged.AddListener(SetVolume);
    }

    void Update()
    {
        if (soundSlider.gameObject.activeInHierarchy)
        {
            // キーボードまたはコントローラーで対応
            float horizontal = Input.GetAxis("Horizontal");

            float vertical = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(horizontal) > 0.1f)
            {
                soundSlider.value = Mathf.Clamp(soundSlider.value + horizontal * 0.001f, 0f, 1f);
            }
        }
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
        VolumeSettings.Instance?.SetVolume(value);
        UpdateVolumeText(value);
    }

    //private void OnSliderChanged(float value)
    //{
    //    audioSource.volume = value;

    //    // 
    //    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Title")
    //    {
    //        VolumeSettings.SaveVolume(value);
    //    }

    //    UpdateVolumeText(value);
    //}

    // テキスト更新(設定値の100倍して表示)
    private void UpdateVolumeText(float value)
    {
        int percent = Mathf.RoundToInt(value * 100);
        volumeText.text = "ボリューム: " + percent.ToString();
    }
}
