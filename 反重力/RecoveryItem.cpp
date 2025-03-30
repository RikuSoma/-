// Fill out your copyright notice in the Description page of Project Settings.


#include "RecoveryItem.h"
#include "Async/Async.h"

// Sets default values
ARecoveryItem::ARecoveryItem()
	:mItemRotationSpeed(1.f)
	, mItemAddHP(0)
	, mItemAddMP(0)
	, mTilt(15.f)
	,mUpDownSpeed(3.f)
	,mAmplitude(10.f)
	,mBaseHeight(20.f)
	,isHit(false)
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	//���[�g�ɒu���x�[�X�I�u�W�F�N�g������
	mpBase = CreateDefaultSubobject<USceneComponent>(TEXT("SceceComp"));
	RootComponent = mpBase;

	//�R���W��������p�{�b�N�X�R���|�[�l���g����
	mpBoxComp = CreateDefaultSubobject<UBoxComponent>(TEXT("CollitionComp"));
	mpBoxComp->SetMobility(EComponentMobility::Movable);
	mpBoxComp->SetupAttachment(RootComponent);

	//���b�V���R���|�[�l���g(StatickMesh)����
	mpItemMesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("ItemMesh"));
	
	mpItemMesh->SetupAttachment(RootComponent);
}

// Called when the game starts or when spawned
void ARecoveryItem::BeginPlay()
{
	Super::BeginPlay();
	
	mBaseHeight = GetActorLocation().Z;
}

// Called every frame
void ARecoveryItem::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

	mpBoxComp->SetWorldLocation(GetActorLocation());
}

//�A�C�e������]������
void ARecoveryItem::RotateItem(float rotateSpeed)
{
	//���݂�FPS�𑪒�
	float fps = 1.0 / (GetWorld()->DeltaTimeSeconds);

	//�A�C�e����]����
	float rotateCorrection = 60.f / fps;
	FRotator newRot = mpItemMesh->GetRelativeRotation() + FRotator(0, rotateSpeed, 0);
	mpItemMesh->SetRelativeRotation(newRot);
}

//�A�C�e���̊p�x���X����
void ARecoveryItem::TiltItem(float tilt)
{
	FRotator newRot = mpItemMesh->GetRelativeRotation() + FRotator(0, 0, tilt);
	mpItemMesh->SetRelativeRotation(newRot);
}

//�A�C�e���̏㉺�ړ�
void ARecoveryItem::UpdownAmplitude() 
{
	FVector CurrentLocation = GetActorLocation();

	//�����ԓ���(sin�g���g�p)
	float Time = GetWorld()->GetTimeSeconds();
	float Offset = FMath::Sin(Time * mUpDownSpeed) * mAmplitude;

	//�V�����ʒu������
	CurrentLocation.Z = mBaseHeight + Offset;

	SetActorLocation(CurrentLocation);
}

void ARecoveryItem::GravityInitialize()
{
	PrimaryActorTick.bCanEverTick = true;

	//�����l�ݒ�
	Velocity = FVector::ZeroVector;
	GravityForce = 0.0f;
}

void ARecoveryItem::GravityScale(float DeltaTime)
{
	//�d�͂𑬓x�ɉ��Z
	Velocity.Z += GravityForce * DeltaTime;

	//�A�N�^�[�̈ʒu���X�V
	AddActorWorldOffset(Velocity * DeltaTime, true);

	//�Փˌ��o�̂��߂̏���
	FHitResult Hit;

	AddActorWorldOffset(Velocity * DeltaTime, true, &Hit);
	if (Hit.IsValidBlockingHit())
	{
		//�Փ˂��Ă�����Z�����̑��x�����Z�b�g
		Velocity.Z = 0.0f;
	}
}

//UE���N���b�V����������AsyncTask�𗘗p���ăQ�[���X���b�h�ŏ�������悤�ɕύX
void ARecoveryItem::SetMesh(UStaticMesh* NewMesh)
{
	if (!NewMesh)
	{
		UE_LOG(LogTemp, Warning, TEXT("NewMesh is nullptr."));
		return;
	}

	// this �����S�ɃL���v�`�����邽�߂� TWeakObjectPtr ���g�p
	TWeakObjectPtr<ARecoveryItem> WeakThis(this);

	AsyncTask(ENamedThreads::GameThread, [WeakThis, NewMesh]()
		{
			if (WeakThis.IsValid() && WeakThis->mpItemMesh)
			{
				WeakThis->mpItemMesh->SetStaticMesh(NewMesh);
			}
			else
			{
				UE_LOG(LogTemp, Warning, TEXT("ARecoveryItem or mpItemMesh is nullptr in AsyncTask."));
			}
		});
}

void ARecoveryItem::OnOverlapRecovery(UPrimitiveComponent* OverlappedComponent, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
}

void ARecoveryItem::OnOverlapRecoveryEnd(UPrimitiveComponent* OverlappedComponent, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex)
{
}
