using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unit;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum eWeaponType
    {
        None = 0,
        Sword = 1,
        Bow = 2,
        Magic = 3, 
        Ax,
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
     
    
    //원거리 (화살)
    public void Launch(Vector2 start, Vector2 target)
    {
        startPosition = start;
        targetPosition = target;
        transform.position = start;
        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(MoveStraightWithCurve());
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


        Explode();
    }

    private void Explode()
    {
        Debug.Log($"{weaponType} 이동 완료: 폭발!");
        Destroy(gameObject);
    }

    //원거리 (마법)


    //근거리
    private void AttackSword(int dmg, UnitInfo target)
    {
        target.TakeDMG(dmg);
    }

    //유닛 무기 정보 설정하기
    public void SetWeapon(MyInfo.eUnitType unitType)
    {
        switch (unitType)
        {
            case MyInfo.eUnitType.Swordsman:
                weaponType = eWeaponType.Sword;
                break;
            case MyInfo.eUnitType.Bower:
                weaponType = eWeaponType.Bow;
                break;
            case MyInfo.eUnitType.Magician:
                weaponType = eWeaponType.Magic;
                break;
            case MyInfo.eUnitType.Barbarian:
                weaponType = eWeaponType.Ax;
                break;
        }
    }

    //공격 모드
    public void StartAttack(int dmg, UnitInfo target)
    {
        isAttack = true;
        AttackSword(dmg, target);
    }
    //공격 중지
    public void StopAttack() 
    {
        isAttack = false;
    }

    public void PoolingBullet()
    {

    }
}
