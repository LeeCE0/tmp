using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int atk;
    public float speed;
    public UnitBase target;
    private Vector3 targetVector;


    public void Init(int dmg, UnitBase targetUnit)
    {
        atk = dmg;
        target = targetUnit;
        speed = 3;
        targetVector = target.transform.position;
        targetVector.y += 0.2f;
    }
    void Update()
    {
        if (target == null)
        {
            ObjectPoolManager.Instance.ReturnToPool(ObjectPoolManager.ePoolingObj.Skill, gameObject);
            return;
        }

        Vector3 dir = targetVector - transform.position;

        transform.position += dir * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetVector) < 0.2f)
        {
            target.TakeDMG(atk);
            ObjectPoolManager.Instance.ReturnToPool(ObjectPoolManager.ePoolingObj.Skill, gameObject);
        }
    }
}
