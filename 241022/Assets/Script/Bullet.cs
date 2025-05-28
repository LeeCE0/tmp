using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int atk;
    public float speed;
    public UnitInfo target;
    private Vector3 targetVector;


    public void Init(int dmg, UnitInfo targetUnit)
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
            ObjectPoolManager.Instance.ReturnToPool("fireball", gameObject);
            return;
        }

        Vector3 dir = targetVector - transform.position;

        transform.position += dir * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetVector) < 0.2f)
        {
            target.TakeDMG(atk);
            ObjectPoolManager.Instance.ReturnToPool("fireball", gameObject);
        }
    }
}
