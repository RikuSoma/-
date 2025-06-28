// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "EnemyBase.h"
#include "AttackBase.h"
#include "GameFramework/Character.h"
#include "FlightTypeEnemy.generated.h"

//
// �N���X���FFlightTypeEnemy
// �p�r�F��s�^�C�v�̓G�̎���
// �쐬�ҁF23cu0216���n����
//

class SearchArea; // ���G�N���X

UCLASS()
class ANTIGRAVITY_API AFlightTypeEnemy : public AEnemyBase
{
	GENERATED_BODY()

public:
	AFlightTypeEnemy();
	~AFlightTypeEnemy();

	virtual void BeginPlay();

	virtual void Tick(float DeltaTime) override;

private:
	void UpdateMove(float DeltaTime) override;

	bool PlayAttack() override;

	UFUNCTION(BlueprintCallable)
	void OnOverlapAttack(UPrimitiveComponent *OverlappedComponent, AActor *OtherActor, UPrimitiveComponent *OtherComp,
						 int32 OtherBodyIndex, bool bFromSweep, const FHitResult &SweepResult);

	UFUNCTION(BlueprintCallable)
	void AttackFunction(AActor *OtherActor, UPrimitiveComponent *OtherComp);

	UFUNCTION(BlueprintCallable)
	void OnOverlapAttackEnd(UPrimitiveComponent *OverlappedComponent, AActor *OtherActor, UPrimitiveComponent *OtherComp, int32 OtherBodyIndex);

private:
	// ��s�G�̃��b�V��
	UPROPERTY(EditAnywhere, BlueprintReadWrite, meta = (AllowPrivateAccess = "true"))
	TObjectPtr<UStaticMeshComponent> mEnemyMesh;

	TObjectPtr<UCapsuleComponent> mEnemyCapsule;

public:
	UFUNCTION(BlueprintCallable)
	void SetActiveAttack(bool isActive) { mIsActiveAttack = isActive; }
};
