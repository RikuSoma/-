// Fill out your copyright notice in the Description page of Project Settings.

#pragma once
#include "Components/BoxComponent.h"
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "RecoveryItem.generated.h"

UCLASS()
class ANTIGRAVITY_API ARecoveryItem : public AActor
{
	GENERATED_BODY()
	
public:	
	//�R���X�g���N�^
	ARecoveryItem();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	//���t���[���̍X�V����
	virtual void Tick(float DeltaTime) override;

protected:
	//�A�C�e������]
	virtual void RotateItem(float rotateSpeed);

	//�A�C�e�����X����
	virtual void TiltItem(float tilt);

	//�A�C�e���̏㉺�^��
	virtual void UpdownAmplitude();

	//�d�͂̐ݒ�
	virtual void GravityScale(float DeltaTime);

	//�d�͏������֐�
	virtual void GravityInitialize();

	//�A�C�e�����o�����𔻒f����֐�
	//virtual bool discharge();

	//�G�f�B�^�[��ŕҏW�ł��郁���o�[�ϐ�
public:
	//�A�C�e���ڐG�p�R���W����
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	UBoxComponent* mpBoxComp;

	//�A�C�e�����b�V��
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	UStaticMeshComponent* mpItemMesh;

	//Hp�񕜃A�C�e��
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Meshes")
	UStaticMesh* Mesh1;

	//Mp�񕜃A�C�e��
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Meshes")
	UStaticMesh* Mesh2;

	//���b�V����ݒ肷��֐�
	UFUNCTION(BlueprintCallable, Category = "Mesh Control")
	void SetMesh(UStaticMesh* NewMesh);
	
protected:
	//�x�[�X�R���|�[�l���g
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	USceneComponent* mpBase;

	//��]���x
	UPROPERTY(EditAnywhere)
	float mItemRotationSpeed;	//��]�X�s�[�h

	UPROPERTY(EditAnywhere)
	float mUpDownSpeed;			//�㉺����X�s�[�h

	UPROPERTY(EditAnywhere)
	float mAmplitude;			//�U��(�㉺�͈̔�)

	UPROPERTY(EditAnywhere)
	float mBaseHeight;			//��̍���

	UPROPERTY(EditAnywhere)
	float mTilt;				//�X��

	UPROPERTY(EditAnywhere)
	float mItemAddHP;			//HP�{

	UPROPERTY(EditAnywhere)
	float mItemAddMP;			//MP�{

	UPROPERTY(EditAnywhere)
	bool isHit;

	UFUNCTION(BlueprintCallable)
	bool IsHit() { return isHit; }

private:
	FVector Velocity;
	float GravityForce;

protected:
	UFUNCTION(BlueprintCallable)
	virtual void OnOverlapRecovery(UPrimitiveComponent* OverlappedComponent, AActor* OtherActor, 
		UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult);

	UFUNCTION(BlueprintCallable)
	virtual void OnOverlapRecoveryEnd(UPrimitiveComponent* OverlappedComponent, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex);
};
