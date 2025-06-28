using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemonSkill : BulletSkill
{
    // ダメージ量（ここを変更）
    private float damageAmount = 50f;
    protected override SkillType Type => SkillType.Instant;


    public override void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        base.Init(player, playerStateMachine);
        Debug.Log("DemonSkill 初期化完了");
    }

    protected override void Skill()
    {
        var targets = GetRandomEnemiesInView(3);

        foreach (var enemy in targets)
        {
            var damageable = enemy.GetComponent<CharacterHealthManager>();

            if (damageable != null)
            {
                damageable.ApplyDamage(damageAmount);
            }
            else
            {
                Debug.LogWarning($"{enemy.name} に IDamageable が見つかりませんでした。");
            }
        }
    }

    protected override void SubSkill()
    {
        // 必要に応じて実装
    }

    public override void Remove()
    {
        base.Remove();
        Debug.Log("[DemonSkill] イベント解除");
    }

    // 敵をランダムに取得（汎用）
    protected virtual List<GameObject> GetRandomEnemiesInView(int count)
    {
        var enemiesInView = EnemyCounter.Instance.GetEnemiesInView();
        if (enemiesInView == null || enemiesInView.Count == 0)
            return new List<GameObject>();

        List<GameObject> enemyList = enemiesInView.ToList();

        if (count >= enemyList.Count)
            return new List<GameObject>(enemyList);

        for (int i = enemyList.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (enemyList[i], enemyList[j]) = (enemyList[j], enemyList[i]);
        }

        return enemyList.GetRange(0, count);
    }


}
