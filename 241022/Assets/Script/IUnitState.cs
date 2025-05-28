using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;
public interface IUnitState
{
    MyInfo.eUnitState StateType { get; }
    void Enter();
    void Update();
    void Exit();
}

public class WalkState : IUnitState
{
    private UnitInfo unit;
    public MyInfo.eUnitState StateType => MyInfo.eUnitState.Walk;

    public WalkState(UnitInfo unit) => this.unit = unit;

    public void Enter()
    {
        unit.attackDelay = 0f;
        unit.isAttack = false;
    }

    public void Update() 
    {
        if (unit.curTarget == null)
        {
            unit.anim.SetFloat("RunState", 0.5f);
            float dir = unit.isMyUnit ? 1f : -1f;
            unit.transform.Translate(Vector2.right * dir * unit.moveSpeed * Time.deltaTime);
             
            unit.curTarget = unit.FindNearestEnemy();

            NexusInfo nexus = unit.FindNexus();
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
        unit.anim.SetFloat("RunState", 0f);
    }
}

public class AttackState : IUnitState
{
    private UnitInfo unit;
    public MyInfo.eUnitState StateType => MyInfo.eUnitState.Attack;

    public AttackState(UnitInfo unit) => this.unit = unit;

    public void Enter()
    {
        unit.isAttack = true; 
        unit.attackDelay = unit.attackCT;
    }

    public void Update()
    {
        if (unit.curTarget == null || unit.curTarget.curHP <= 0 || unit.DistanceToTarget() > unit.attackDistance)
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
        unit.anim.SetFloat("AttackState", 0f);

        unit.weapon.StartAttack(unit.ATK, unit.curTarget);

        switch (unit.unitType)
        {
            case MyInfo.eUnitType.Swordsman:
                unit.anim.SetFloat("NormalState", 0f);
                unit.anim.SetFloat("RunState", 0f);
                break;
            case MyInfo.eUnitType.Bower:
                unit.anim.SetFloat("NormalState", 0.5f);
                unit.anim.SetFloat("RunState", 0f);
                break;
            case MyInfo.eUnitType.Magician:
                unit.anim.SetFloat("NormalState", 1.0f);
                unit.anim.SetFloat("RunState", 0f);
                break;
        }
    }
}
public class DeadState : IUnitState
{
    public MyInfo.eUnitState StateType => MyInfo.eUnitState.Dead;
    private UnitInfo unit;
     public DeadState(UnitInfo unit) => this.unit = unit;
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
        yield return new WaitForSeconds(2f);

        unit.Deactivate();
    }
}

public class NexusAttackState : IUnitState
{
    private UnitInfo unit;
    private NexusInfo nexus;

    public NexusAttackState(UnitInfo unit, NexusInfo nexus)
    {
        this.unit = unit;
        this.nexus = nexus;
    }

    public MyInfo.eUnitState StateType => MyInfo.eUnitState.Attack;

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