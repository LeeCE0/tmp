using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum eWeaponType
    {
        None = 0,
        GunBullet = 1,
        Boom = 2,
        Arrow = 3, 
        Hammer,
        Scyth,
        Sword,
    }
    [SerializeField] GameObject bulletOBJ;
    public eWeaponType weaponType = eWeaponType.None; // 무기 타입
    public Vector2 startPosition; // 시작 위치
    public Vector2 targetPosition; // 목표 위치
    public float speed = 5f; // 이동 속도
    public float arcHeight = 2f; // 포물선 높이 (필요시)
    public bool isMeeleeType = false;
    private bool isRunning = false;
    public bool isAttack = false;

    #region old
    public void Launch(Vector2 start, Vector2 target, eWeaponType weapon)
    {
        //근거리 무기 날라가면 안됨
        if (isMeeleeType)
            return;
        startPosition = start;
        targetPosition = target;
        transform.position = start;
        weaponType = weapon;
        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(MoveBullet());
        }
    }

    private IEnumerator MoveBullet()
    {
        switch (weaponType)
        {
            case eWeaponType.GunBullet:
                yield return StartCoroutine(MoveStraight());
                break;

            case eWeaponType.Boom:
                yield return StartCoroutine(MoveParabola());
                break;

            case eWeaponType.Arrow:
                yield return StartCoroutine(MoveStraightWithCurve());
                break;

            default:
                Debug.LogWarning("Invalid bullet type");
                break;
        }

        Explode();
    }

    private IEnumerator MoveStraight()
    {
        while ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator MoveParabola()
    {
        float elapsedTime = 0f;
        float duration = Vector2.Distance(startPosition, targetPosition) / speed; 
        while (elapsedTime < duration)

        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration;

            float x = Mathf.Lerp(startPosition.x, targetPosition.x, t);
            float y = Mathf.Lerp(startPosition.y, targetPosition.y, t) + arcHeight * Mathf.Sin(Mathf.PI * t);

            transform.position = new Vector2(x, y);
            yield return null;
        }
    }

    private IEnumerator MoveStraightWithCurve()
    {
        Vector2 direction = (targetPosition - startPosition).normalized;
        float curveAmount = 0.1f; // 화살의 흔들림 정도

        while ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // 회전 연출
            transform.right = direction + Vector2.up * Mathf.Sin(Time.time * 10) * curveAmount;

            yield return null;
        }
    }

    private void Explode()
    {
        Debug.Log($"{weaponType} 이동 완료: 폭발!");
        Destroy(gameObject);
    }
    #endregion

    #region new

    //유닛 무기 정보 설정하기
    public void SetWeapon(eWeaponType type)
    {
        weaponType = type;
        isMeeleeType = false;
        switch (weaponType)
        {
            case eWeaponType.GunBullet:
                break;
            case eWeaponType.Boom:
                break;
            case eWeaponType.Arrow:
                break;
            case eWeaponType.Hammer:
                isMeeleeType = true;
                break;
            case eWeaponType.Scyth:
                isMeeleeType = true;
                break;
            case eWeaponType.Sword:
                isMeeleeType = true;
                break;
        }
    }

    //공격 모드
    public void StartAttack()
    {

    }
    //공격 중지
    public void StopAttack() 
    { 
    
    
    }




    #endregion

}
