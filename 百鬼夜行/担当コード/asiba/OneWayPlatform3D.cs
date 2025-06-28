using System.Collections;
using UnityEngine;

/// <summary>
/// �v���C���[�Ȃǂ̃I�u�W�F�N�g��������ʂ蔲������悤�ɂ��A
/// �O������̖��߂ŏՓ˔����ON/OFF�������ʍs�̑���B
/// </summary>
public class OneWayPlatform3D : MonoBehaviour
{
    [Tooltip("��ɏ�邽�߂́A���������R���C�_�[�iIsTrigger��OFF�̂��́j")]
    [SerializeField]
    private Collider platformCollider;

    [Tooltip("�I�u�W�F�N�g�̏o��������m���邽�߂́A�傫�ȃg���K�[�iIsTrigger��ON�̂��́j")]
    [SerializeField]
    private Collider platformTrigger;

    private const string DEBUG_KEY = "[OneWayPlatformDebug]";

    private void Start()
    {
        if (platformCollider == null)
            Debug.LogError($"{DEBUG_KEY} ({gameObject.name}): 'Platform Collider'���C���X�y�N�^�[�Őݒ肳��Ă��܂���I", this);
        if (platformTrigger == null)
            Debug.LogError($"{DEBUG_KEY} ({gameObject.name}): 'Platform Trigger'���C���X�y�N�^�[�Őݒ肳��Ă��܂���I", this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.y < platformTrigger.bounds.center.y)
            {
                IgnoreCollision(other);
            }
        }
    }

    // ������ ���̃��\�b�h���C������܂��� ������
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �v���C���[�̒��S�_���A����̃R���C�_�[�̒��S�_����ɏo���ꍇ�̂݁A�Փ˂����ɖ߂�
            // ����ɂ��A������W�����v�œ˂������Ă���r���ň���������Ȃ��Ȃ�
            if (other.transform.position.y > platformCollider.bounds.center.y)
            {
                StartCoroutine(RestoreCollisionAfterFrame(other));
                Debug.Log($"[OneWayPlatformDebug] ({gameObject.name}): Player is above platform, restoring collision.");
            }
        }
    }

    public void IgnoreCollision(Collider objectToIgnore)
    {
        if (this.enabled && platformCollider != null && objectToIgnore != null)
        {
            Physics.IgnoreCollision(objectToIgnore, platformCollider, true);
            Debug.Log($"{DEBUG_KEY} ({gameObject.name}): IGNORE collision with '{objectToIgnore.name}'.");
        }
    }

    public void RestoreCollision(Collider objectToRestore)
    {
        if (this.enabled && platformCollider != null && objectToRestore != null)
        {
            Physics.IgnoreCollision(objectToRestore, platformCollider, false);
            Debug.Log($"{DEBUG_KEY} ({gameObject.name}): RESTORE collision with '{objectToRestore.name}'.");
        }
    }

    private IEnumerator RestoreCollisionAfterFrame(Collider objectToRestore)
    {
        yield return new WaitForEndOfFrame();
        RestoreCollision(objectToRestore);
    }
}