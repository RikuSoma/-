// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"


// �ǐՏ��
enum class ChaseStatus {
	None,	// ��
	Idle,	// �ҋ@
	Chase,	// �ǐՒ�
	TargetLost,	// �����������
};

// �����_����]�ɕK�v�ȏ��
struct RandomRotationInfo {
	// ���݂̉�]�p�x
	float mCurrentLookAroundYaw = 0.0f;

	// �ڕW�Ƃ����]�p�x
	float mTargetLookAroundYaw = 30.0f;

	// ��]�����i1: ���v���, -1: �����v���j
	int32 mLookAroundDirection = 1;

	// ��]�����ɐ؂�ւ��܂ł̃^�C�}�[
	float mLookAroundChangeTimer = 0.0f;

	// ���̕����؂�ւ��܂ł̊Ԋu
	float mLookAroundChangeInterval = 1.5f; // �b
};

// �����_���ړ��ɕK�v�ȏ��
struct RandomMoveInfo {
	FVector RandomPos;	// �����_�����W�p
	FVector PreviousPos;	// �O��̍��W
	bool IsEndMove;	// �ړ�������������
	float MoveRange;	// �ړ��͈�
	float MinDistance;	// �O��̖ڕW�n�_�Ƃ̍ŏ�����
	float ElapsedTime;	// �o�ߎ���
};

// ���E���
struct VisibilityInfo
{
	float Centerangle;	// ���S�p
	float Radius;	// ���a

	float GetArcLength()
	{
		return Radius * 2 * 3.14f * (Centerangle / 360.0f);
	}
};

struct RandomRotateInfo {
	float ElapsedTime;	// �o�ߎ���
	float DeltaTime;	// 1�t���[�����x
	float RotateTime;	// ��]����
};