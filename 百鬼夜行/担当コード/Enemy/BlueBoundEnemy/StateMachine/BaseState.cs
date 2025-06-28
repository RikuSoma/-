using UnityEngine;

// 全てのステートの設計図となる抽象クラス
public abstract class BaseState
{
    protected BlueBoundEnemyMovement enemy; // AI本体（コンテキスト）への参照
    protected float stateTimer;             // 各ステート内で使用できるタイマー

    // コンストラクタ：AI本体への参照を受け取る
    public BaseState(BlueBoundEnemyMovement enemy)
    {
        this.enemy = enemy;
    }

    // このステートに入った時に一度だけ呼ばれる処理
    public virtual void EnterState()
    {
        stateTimer = 0f;
    }

    // このステートにいる間、毎フレーム呼ばれる処理
    public abstract void UpdateState();

    // このステートから出る時に一度だけ呼ばれる処理
    public virtual void ExitState() { }
}