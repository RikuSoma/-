using UnityEngine;
using UnityEngine.UI;

public class GaugeUIManager : MonoBehaviour
{
    [SerializeField] private Image[] maskImages; // ���ɔz�u���ꂽ�C���[�W

    private BulletSkill bulletSkill;
    private const float gaugePerMask = 1f;  // 1���P�Q�[�W

    private void Start()
    {
        bulletSkill = new BulletSkill();
    }

    private void Update()
    {
        // bulletskill��null�̏ꍇ���s���Ȃ�
        if (bulletSkill == null)
        {
            Debug.Log("null��");
            return;
        }

        // �Q�[�W�̕ϐ��錾��bulletskill�̌��݃Q�[�W���������Ă���
        float gauge = bulletSkill.GetGauge();

        // ��̏������ƂɃQ�[�W�̏󋵂���ʂɕ\������
        for (int i = 0; i < maskImages.Length; i++)
        {
            float value = gauge - i * gaugePerMask;
            if (value <= 0f)
                maskImages[i].fillAmount = 0f;
            else if (value >= 1f)
                maskImages[i].fillAmount = 1f;
            else
                maskImages[i].fillAmount = value;
        }
    }


}
