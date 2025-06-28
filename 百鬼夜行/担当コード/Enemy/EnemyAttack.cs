using UnityEngine;

// �U���R���|�[�l���g�͕K��Shooting�R���|�[�l���g��K�v�Ƃ���
[RequireComponent(typeof(Shooting))]
public abstract class EnemyAttack : MonoBehaviour
{
    protected Shooting shooting;

    protected virtual void Awake()
    {
        shooting = GetComponent<Shooting>();
    }

    /// <summary>
    /// �O����Config�A�Z�b�g���g���čU���̏����ݒ���s���܂��B
    /// </summary>
    /// <param name="config">�U���ݒ�p��ScriptableObject</param>
    public abstract void Initialize(ScriptableObject config);

    /// <summary>
    /// �U�������s���܂��B
    /// </summary>
    public abstract void PerformAttack();

    public abstract void SetTarget(GameObject target);
}