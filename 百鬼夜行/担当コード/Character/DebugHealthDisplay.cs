using TMPro;
using UnityEngine;

public class DebugHealthDisplay : MonoBehaviour
{
    private CharacterHealthManager healthManager;
    private TextMeshPro textMeshPro;
    private Camera mainCamera;

    [SerializeField] private float yOffset = 1.0f; // ����ɕ\������I�t�Z�b�g

#if UNITY_EDITOR
    private void OnValidate()
    {
        // TextMeshPro�����݂��Ȃ��ꍇ�͎����I�ɒǉ�����i�G�f�B�^��̂݁j
        if (GetComponentInChildren<TMPro.TextMeshPro>() == null)
        {
            GameObject textObj = new GameObject("HealthText");
            textObj.transform.SetParent(transform);
            textObj.transform.localPosition = Vector3.up * yOffset;
            textObj.transform.localRotation = Quaternion.identity;
            textObj.transform.localScale = Vector3.one * 0.1f; // �T�C�Y����

            textMeshPro = textObj.AddComponent<TMPro.TextMeshPro>();
            textMeshPro.alignment = TMPro.TextAlignmentOptions.Center;
            textMeshPro.fontSize = 36;
            textMeshPro.color = Color.green;
            textMeshPro.text = "---";
        }
        else
        {
            textMeshPro = GetComponentInChildren<TMPro.TextMeshPro>();
            textMeshPro.transform.localPosition = Vector3.up * yOffset;
        }
    }
#endif

    void Start()
    {
        healthManager = GetComponent<CharacterHealthManager>();
        if (healthManager == null)
        {
            Debug.LogError("CharacterHealthManager �R���|�[�l���g��������܂���I", this);
            enabled = false;
            return;
        }

        // TextMeshPro���q�I�u�W�F�N�g����T��
        textMeshPro = GetComponentInChildren<TMPro.TextMeshPro>();
        if (textMeshPro == null)
        {
            // TextMeshPro�����݂��Ȃ��ꍇ�͓��I�ɍ쐬
            GameObject textObj = new GameObject("HealthText");
            textObj.transform.SetParent(transform);
            textObj.transform.localPosition = Vector3.up * yOffset;
            textObj.transform.localRotation = Quaternion.identity;
            textObj.transform.localScale = Vector3.one * 0.1f; // �T�C�Y����

            textMeshPro = textObj.AddComponent<TMPro.TextMeshPro>();
            textMeshPro.alignment = TMPro.TextAlignmentOptions.Center;
            textMeshPro.fontSize = 36;
            textMeshPro.color = Color.green;
            textMeshPro.text = healthManager.GetHelth().ToString("F1"); // �����̗͕\��
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("MainCamera ���V�[���ɑ��݂��܂���I", this);
            enabled = false;
        }
    }

    void Update()
    {
        if (healthManager != null && textMeshPro != null && mainCamera != null)
        {
            textMeshPro.text = healthManager.GetHelth().ToString("F1");

            // �J�����̕����������悤�ɂ���
            textMeshPro.transform.LookAt(textMeshPro.transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up);
        }
    }
}