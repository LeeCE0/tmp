using UnityEngine;
using System.Collections;
using static Unit.UnitDataContainer;

public class Projectile : MonoBehaviour
{
    public int atk;
    public float speed;
    public UnitBase target;
    ObjectPoolManager.ePoolingObj poolTag;
    public float delayTime = 1f;   // 발사 대기 시간

    public void Init(int dmg, UnitBase targetUnit, ObjectPoolManager.ePoolingObj tag, bool isMine)
    {
        atk = dmg;
        target = targetUnit;
        poolTag = tag;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isMine ? -1 : 1);
        transform.localScale = scale;
    }

    public void SetProjectile(eUnitType type)
    {
        switch(type)
        {
            case eUnitType.Magician:
                break;
            case eUnitType.Archer:
                break;
            case eUnitType.Bishop:
                break;
        }

    }
    void Update() 
    {
        if (target == null)
        {
            ObjectPoolManager.Instance.ReturnToPool(poolTag, gameObject);
            return;
        }

        // 이동
        Vector3 dir = target.transform.position - transform.position;
        transform.position += dir.normalized * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            if (target != null)
                target.TakeDMG(atk);

            target = null;
            transform.position = Vector3.zero; 
            ObjectPoolManager.Instance.ReturnToPool(poolTag, gameObject);
        }
    }
}
