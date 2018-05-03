using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_ItemWeaponMelee : Tapestry_ItemEquippable {

    public bool
        useStandingAsUniversalBehavior = true;
    public Tapestry_Effect
        effectForward,
        effectLateral,
        effectReverse,
        effectAerial,
        effectStanding;
    public float attackSpeed = .36f;

    private float time;
    private bool isAttacking;

    private void Update()
    {
        if(isAttacking)
        {
            time -= Time.deltaTime;
            float prog = (1 - (time / attackSpeed)) * Mathf.PI;
            if (prog >= Mathf.PI)
            {
                prog = Mathf.PI;
                isAttacking = false;
                GetComponent<Collider>().enabled = false;
            }
            transform.localRotation = Quaternion.Euler(Mathf.Sin(prog) * 50, 0, 0);
        }
    }

    public void AttackStanding()
    {
        if(!isAttacking)
        {
            isAttacking = true;
            time = attackSpeed;
            GetComponent<Collider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Tapestry_Actor actor = other.gameObject.GetComponentInParent<Tapestry_Actor>();
        if (!ReferenceEquals(actor, null))
        {
            actor.AddEffect(effectStanding);
        }
    }
}

public enum Tapestry_MeleeWeaponBehavior
{
    Swing
}