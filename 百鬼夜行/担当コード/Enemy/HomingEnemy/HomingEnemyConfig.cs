using UnityEngine;

[CreateAssetMenu(fileName = "HomingEnemyConfig", menuName = "Enemy/HomingEnemyConfig", order = 1)]
public class HomingEnemyConfig : ScriptableObject
{
    [Header("�U���֌W")]
    public GameObject BulletObject;
    public float BulletSpeed;
    public float LifeTime;

    [Header("�ړ��֌W")]
    public float JumpPower;
}
