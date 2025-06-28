using UnityEngine;

// �S�ẴX�e�[�g�̐݌v�}�ƂȂ钊�ۃN���X
public abstract class BaseState
{
    protected BlueBoundEnemyMovement enemy; // AI�{�́i�R���e�L�X�g�j�ւ̎Q��
    protected float stateTimer;             // �e�X�e�[�g���Ŏg�p�ł���^�C�}�[

    // �R���X�g���N�^�FAI�{�̂ւ̎Q�Ƃ��󂯎��
    public BaseState(BlueBoundEnemyMovement enemy)
    {
        this.enemy = enemy;
    }

    // ���̃X�e�[�g�ɓ��������Ɉ�x�����Ă΂�鏈��
    public virtual void EnterState()
    {
        stateTimer = 0f;
    }

    // ���̃X�e�[�g�ɂ���ԁA���t���[���Ă΂�鏈��
    public abstract void UpdateState();

    // ���̃X�e�[�g����o�鎞�Ɉ�x�����Ă΂�鏈��
    public virtual void ExitState() { }
}