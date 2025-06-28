// Fill out your copyright notice in the Description page of Project Settings.

#include "RecoveryMp.h"
#include "PlayerCharacter.h"

ARecoveryMp::ARecoveryMp()
{
	// Mesh�̐ݒ�
	static ConstructorHelpers::FObjectFinder<UStaticMesh> MeshAsset(TEXT("/Script/Engine.StaticMesh'/Game/Characters/Object/Gravite_Item/Gravity_Item.Gravity_Item'"));
	if (MeshAsset.Succeeded())
	{
		Mesh2 = MeshAsset.Object;
		SetMesh(Mesh2);
	}

	else
	{
		UE_LOG(LogTemp, Warning, TEXT("Mesh2 is not set for RecoveryMp."));
	}
}

void ARecoveryMp::BeginPlay()
{
	Super::BeginPlay();

	GravityInitialize();

	// �A�C�e���̌X��
	TiltItem(mTilt);

	mpBoxComp->OnComponentBeginOverlap.AddDynamic(this, &ARecoveryMp::OnOverlapRecovery);
	mpBoxComp->OnComponentEndOverlap.AddDynamic(this, &ARecoveryMp::OnOverlapRecoveryEnd);
	mpBoxComp->SetupAttachment(RootComponent);
	mpBoxComp->SetWorldLocation(GetActorLocation());
}

void ARecoveryMp::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

	// �d�͕t�^
	GravityScale(DeltaTime);

	// �A�C�e���̉�]
	RotateItem(mItemRotationSpeed);

	// �㉺�ړ�
	UpdownAmplitude();

	mpBoxComp->SetWorldLocation(GetActorLocation());
}

void ARecoveryMp::OnOverlapRecovery(UPrimitiveComponent *OverlappedComponent, AActor *OtherActor, UPrimitiveComponent *OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult &SweepResult)
{
	UE_LOG(LogTemp, Log, TEXT("Item is Hit"));

	// ������������Player���m�F
	APlayerCharacter *PlayerActor = Cast<APlayerCharacter>(OtherActor);
	if (PlayerActor == nullptr)
	{
		return;
	}
	else
	{
		isHit = true;

		// ����d�̓Q�[�W���m�F
		if (PlayerActor->GetMaxGravityGauge() <= 300.0f)
		{
			// Player�̏d�͕ύX�Q�[�W�̏�����グ��
			PlayerActor->IncreaseMaxGravityGauge(mItemAddMP);
		}
	}
}

void ARecoveryMp::OnOverlapRecoveryEnd(UPrimitiveComponent *OverlappedComponent, AActor *OtherActor, UPrimitiveComponent *OtherComp, int32 OtherBodyIndex)
{
}
