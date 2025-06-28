using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

public class TimerManager : MonoBehaviour
{

	// ?�C??���J�E���g����ϐ�
	private float StartTimer;

	// �o�߂���?�C??���v�Z����ϐ�
	private float ElapsedTimer;

	//	�Q??�I���𔻒f����ϐ�
	public bool VictoryGame = false;

	private GameData _gameData;
	void Start()
    {
        _gameData = GameData.Instance;
        // �Q??���X??�g������?�C?�v��
        StartTimer = Time.time;
        VictoryGame = false;
    }

    // Update is called once per frame
    void Update()
    {
		// ���t��??�o�ߎ��Ԃ��v�Z�����
		if (!VictoryGame)
		{
			CountTime();
		}
		else
		{
			StopTimer();
		}
	}

    // �@�\   �F�v���C���n�܂�����v���C���Ԃ��J�E���g�n�߂�֐�
    // ����   �F����
    // �߂�l  �F����
    private void CountTime()
	{
		float CurrentTime = Time.time - StartTimer;
	
	}

    // �@�\   �F�N���A���肪���ꂽ��^�C�}�[�̃J�E���g���~����GameData�ɑ���֐�
    // ����   �F����
    // �߂�l  �F����
    public float StopTimer()
	{
		// ?�C??��?
		ElapsedTimer = Time.time;
		float EndTimer = ElapsedTimer - StartTimer;

        if (_gameData != null)
        {
            _gameData.saveTime(EndTimer);
            Debug.Log("[TimerManager] Time saved: " + FormatTime(EndTimer));
        }

        return EndTimer;
	}

    public static string FormatTime(float timeInSeconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(timeInSeconds);
        return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }
}
