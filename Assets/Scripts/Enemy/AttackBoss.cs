using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackBoss : Boss
{
    protected new enum BossSkill
    {
        Omnidirection,

        //Unidirection,

        SingleShoot,
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
                case BossSkill.Omnidirection:
                    StartCoroutine(OmnidirectionShoot());
                    break;
                /*
            case BossSkill.Unidirection:
                StartCoroutine(UnidirectionShoot());
                break;
                */
                case BossSkill.SingleShoot:
                    StartCoroutine(SingleShoot());
                    break;
            }

        }
    }

}
