using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBoss : Boss
{

    protected new enum BossSkill
    {
        Heal,

        Invincible,

        //Unidirection,

        FullScreenAttack,
    };
    protected override void Attack()
    {
        if (coolDownAttack)
            return;
        if (!isAttacking)
        {
            isAttacking = true;
            gameObject.GetComponent<OutlineEffect>().StopOutline();
            BossSkill skill = RandomEnumValue<BossSkill>();
            Debug.Log(skill);
            //skill = BossSkill.Omnidirection;
            switch (skill)
            {
                case BossSkill.Heal:
                    Debug.Log("health" + health);
                    StartCoroutine(SelfHealing());
                    break;
                case BossSkill.Invincible:
                    StartCoroutine(ShieldOpen());
                    break;
                case BossSkill.FullScreenAttack:
                    StartCoroutine(FullScreenAttack());
                    break;
            }

        }
    }

}
