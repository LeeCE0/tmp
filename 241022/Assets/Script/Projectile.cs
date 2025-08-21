using UnityEngine;
using System.Collections;
using static Unit.UnitDataContainer;
using UnityEngine.WSA;

public class Projectile : MonoBehaviour
{
    public int atk;
    public float speed;
    public UnitBase target;
    private Vector3 targetVector;
    ObjectPoolManager.ePoolingObj poolTag;
    public float delayTime = 1f;   // 발사 대기 시간
    private bool launched;    // 실제로 나갔는지

    public void Init(int dmg, UnitBase targetUnit, ObjectPoolManager.ePoolingObj tag, bool isMine)
    {
        atk = dmg;
        target = targetUnit;
        speed = 3;
        targetVector = target.transform.position;
        //targetVector.y += 0.2f;
        poolTag = tag;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isMine ? -1 : 1);
        transform.localScale = scale;
        //StartCoroutine(ParabolaMove());
    }

    private IEnumerator ParabolaMove()
    {
        launched = true; // 바로 시작
        float t = 0f;
        Vector3 start = transform.position;
        Vector3 end = targetVector;
        float height = 0f; // 궤적 높이

        while (t < 1f)
        {
            t += Time.deltaTime * speed / Vector3.Distance(start, end);
            float heightOffset = 4 * height * (t - t * t); // 포물선 y 보정
            transform.position = Vector3.Lerp(start, end, t) + Vector3.up * heightOffset;
            yield return null;
        }

        target.TakeDMG(atk);
        ObjectPoolManager.Instance.ReturnToPool(poolTag, gameObject);
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
        //지연 처리
        if (!launched)
        {
            delayTime -= Time.deltaTime;
            if (delayTime <= 0f)
            {
                launched = true;
            }
            else return;
        }

        // 이동
        Vector3 dir = targetVector - transform.position;
        transform.position += dir.normalized * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetVector) < 0.2f)
        {
            target.TakeDMG(atk);
            ObjectPoolManager.Instance.ReturnToPool(poolTag, gameObject);
        }
    }
}
