using TMPro;
using UnityEngine;

/// <summary>
/// EnemyCounter�̒l��\������TextMeshPro�I�u�W�F�N�g�������I�ɐ����E�Ǘ����܂��B
/// ���̃X�N���v�g���A�^�b�`�����I�u�W�F�N�g�̎q�Ƃ��āA�e�L�X�g����������܂��B
/// </summary>
public class AutoCounterDisplay : MonoBehaviour
{
    // --- Inspector�Œ����\�Ȑݒ� ---
    [Header("�\���ݒ�")]
    [Tooltip("�e�L�X�g��\������A���̃I�u�W�F�N�g����̑��ΓI�Ȉʒu")]
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);

    [Tooltip("�e�L�X�g�I�u�W�F�N�g�̏����X�P�[��")]
    [SerializeField] private float initialScale = 0.1f;

    [Tooltip("�t�H���g�T�C�Y")]
    [SerializeField] private float fontSize = 36;

    [Tooltip("�e�L�X�g�̐F")]
    [SerializeField] private Color textColor = Color.cyan;

    [Tooltip("�\������e�L�X�g�̏����B{0}���G�̐��ɒu���������܂��B")]
    [SerializeField] private string displayTextFormat = "Enemies In View\n<size=150%>{0}</size>";

    // --- �����Q�� ---
    private TextMeshPro textMeshPro;
    private Camera mainCamera;

    // OnValidate�̓G�f�B�^��Œl���ύX���ꂽ�Ƃ��ɌĂяo����܂��B
    // ����ɂ��AInspector�ŃI�t�Z�b�g�Ȃǂ�ύX�����ۂɑ����Ƀv���r���[�ł��܂��B
#if UNITY_EDITOR
    private void OnValidate()
    {
        // ����TextMeshPro�����邩�T��
        textMeshPro = GetComponentInChildren<TextMeshPro>();

        if (textMeshPro == null)
        {
            // �Ȃ���΃Z�b�g�A�b�v�������Ă�
            SetupTextMesh();
        }
        else
        {
            // ����ΐݒ�l�𔽉f����
            ApplySettings();
        }
    }
#endif

    void Start()
    {
        // �Q�[�����s����TextMeshPro���Ȃ���΃Z�b�g�A�b�v����
        if (textMeshPro == null)
        {
            textMeshPro = GetComponentInChildren<TextMeshPro>();
            if (textMeshPro == null)
            {
                SetupTextMesh();
            }
        }

        // ���C���J������T���ăL���b�V������
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera ��������܂���I", this);
            enabled = false; // �G���[���̓X�N���v�g�𖳌���
        }
    }

    void Update()
    {
        // �K�v�ȃR���|�[�l���g���S�đ����Ă��邩�m�F
        if (textMeshPro != null && mainCamera != null && EnemyCounter.Instance != null)
        {
            // EnemyCounter����l���擾���ăe�L�X�g���X�V
            int count = EnemyCounter.Instance.EnemyCountInView;
            textMeshPro.text = string.Format(displayTextFormat, count);

            // �e�L�X�g����ɃJ�����̕��������悤�ɂ���
            textMeshPro.transform.rotation = mainCamera.transform.rotation;
        }
    }

    /// <summary>
    /// TextMeshPro�I�u�W�F�N�g�𐶐����A�����ݒ���s���܂��B
    /// </summary>
    private void SetupTextMesh()
    {
        // �q�I�u�W�F�N�g�Ƃ���TextMeshPro������GameObject�𐶐�
        GameObject textObj = new GameObject("CounterText");
        textObj.transform.SetParent(this.transform);

        // TextMeshPro�R���|�[�l���g��ǉ�
        textMeshPro = textObj.AddComponent<TextMeshPro>();

        // �ݒ��K�p
        ApplySettings();
    }

    /// <summary>
    /// Inspector�Őݒ肳�ꂽ�l��TextMeshPro�R���|�[�l���g�ɓK�p���܂��B
    /// </summary>
    private void ApplySettings()
    {
        if (textMeshPro == null) return;

        Transform textTransform = textMeshPro.transform;
        textTransform.localPosition = offset;
        textTransform.localRotation = Quaternion.identity;
        textTransform.localScale = Vector3.one * initialScale;

        textMeshPro.fontSize = fontSize;
        textMeshPro.color = textColor;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.text = "---"; // �����e�L�X�g
    }
}