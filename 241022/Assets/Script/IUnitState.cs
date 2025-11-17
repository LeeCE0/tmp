using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit.UnitDataContainer;

public interface IUnitState
{
    eUnitState StateType { get; }
    void Enter();
    void Update();
    void Exit();
}

public class WalkState : IUnitState
{
    private UnitBase unit;
    public eUnitState StateType => eUnitState.Walk;

    public WalkState(UnitBase unit) =>  this.unit = unit;

    public void Enter()
    {
        unit.attackDelay = 0f;
        unit.isAttack = false;
    }

    public void Update() 
    {       
        if (unit.curTarget == null)
        {
            unit.anim.SetBool("isMoving", true);
            float dir = unit.isMyUnit ? 1f : -1f;
            unit.transform.Translate(Vector2.right * dir * unit.moveSpeed * Time.deltaTime);
             
            unit.curTarget = unit.FindNearestEnemy();

            NexusInfo nexus = unit.FindTargetNexus();
            if (nexus != null && Vector2.Distance(unit.transform.position, nexus.transform.position) < unit.attackDistance)
            {
                unit.ChangeState(new NexusAttackState(unit, nexus));
                return;
            }
        }

        if (unit.curTarget != null)
        {
            if(unit.curTarget.curHP <= 0)
            {
                unit.curTarget = null;
                return;
            }
            unit.ChangeState(new AttackState(unit));
        }
    }

    public void Exit()
    {
        unit.anim.SetBool("isMoving", false);
    }
}

public class AttackState : IUnitState
{
    private UnitBase unit;
    public eUnitState StateType => eUnitState.Attack;

    public AttackState(UnitBase unit) => this.unit = unit;

    public void Enter()
    {
        unit.isAttack = true; 
        unit.attackDelay = unit.attackCT;
    }

    public void Update()
    {
        if (unit.curTarget.curHP <= 0)
            return;

        if (unit.curTarget == null || unit.DistanceToTarget() > unit.attackDistance)
        {
            unit.curTarget = null;
            unit.ChangeState(new WalkState(unit));
            return;
        }

        unit.attackDelay += Time.deltaTime;
        if (unit.attackDelay >= unit.attackCT)
        {
            unit.attackDelay = 0f;
            PerformAttack();
        }
    }

    public void Exit()
    {
        unit.isAttack = false;
    }

    private void PerformAttack()
    {
        if (unit.curTarget == null) return;

        unit.anim.SetTrigger("Attack");
        if(unit.unitType != eUnitType.Archer)
            unit.weapon.StartAttack(unit.ATK, unit.curTarget);
        unit.anim.SetBool("isMoving", false);
    }
}

public class StunState : IUnitState
{
    public eUnitState StateType => eUnitState.Debuff;
    private UnitBase unit;
    public StunState(UnitBase unit) => this.unit = unit;
    public void Enter()
    {

    }
    public void Update()
    {

    }
    public void Exit()
    {

    }

}


public class DeadState : IUnitState
{
    public eUnitState StateType => eUnitState.Dead;
    private UnitBase unit;
     public DeadState(UnitBase unit) => this.unit = unit;
    public void Enter()
    {
        unit.anim.SetTrigger("Die");

        unit.curTarget = null;
        unit.isAttack = false;
        unit.StartCoroutine(HandleDeath());
    }
    public void Update()
    {

    }
    public void Exit()
    {
       
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1f);

        yield return unit.StartCoroutine(FadeOut(1f));

        unit.Deactivate();
    }

    private IEnumerator FadeOut(float duration)
    {
        float elapsed = 0f;
        Color color = unit.image.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            unit.image.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        unit.image.color = new Color(color.r, color.g, color.b, 0f);
    }
}

public class NexusAttackState : IUnitState
{
    private UnitBase unit;
    private NexusInfo nexus;

    public NexusAttackState(UnitBase unit, NexusInfo nexus)
    {
        this.unit = unit;
        this.nexus = nexus;
    }

    public eUnitState StateType => eUnitState.Attack;

    public void Enter()
    {
        unit.attackDelay = 0f;
        unit.isAttack = true;
        unit.anim.SetTrigger("Attack");
    }

    public void Update()
    {
        if (nexus == null)
        {
            unit.ChangeState(new WalkState(unit));
            return;
        }

        unit.attackDelay += Time.deltaTime;
        if (unit.attackDelay >= unit.attackCT)
        {
            unit.attackDelay = 0f;
            nexus.TakeDMG(unit.ATK);
        }
    }

    public void Exit()
    {
        unit.isAttack = false;
    }
}