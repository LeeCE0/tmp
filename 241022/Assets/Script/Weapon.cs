using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unit;
using UnityEngine;
using static Unit.UnitDataContainer;

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
    public UnitInfo unit;
     
    
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
    private void LaunchFireball(GameObject start, Vector2 target, int dmg, UnitInfo targetUnit)
    {
        GameObject bullet = ObjectPoolManager.Instance.SpawnFromPool(ObjectPoolManager.ePoolingObj.Skill, unit.gameObject, Quaternion.identity);
        bullet.transform.position = start.transform.position;

        Bullet bulletData = bullet.GetComponent<Bullet>();
        bulletData.Init(dmg, targetUnit);
    }

    //근거리
    private void AttackSword(UnitInfo target, int dmg)
    {
        target.TakeDMG(dmg);
    }

    //유닛 무기 정보 설정하기
    public void SetWeapon(eUnitType unitType, UnitInfo units)
    {
        unit = units;
        switch (unitType)
        {
            case eUnitType.Swordsman:
                weaponType = eWeaponType.Sword;
                break;
            case eUnitType.Bower:
                weaponType = eWeaponType.Bow;
                break;
            case eUnitType.Magician:
                weaponType = eWeaponType.Magic;
                break;
            case eUnitType.Barbarian:
                weaponType = eWeaponType.Ax;
                break;
        }
    }

    //공격 모드
    public void StartAttack(int dmg, UnitInfo target)
    {
        isAttack = true;
        switch (weaponType)
        {
            case eWeaponType.Sword:
                AttackSword(target, dmg);
                break;
            case eWeaponType.Bow:
                break;
            case eWeaponType.Magic:
                LaunchFireball(gameObject, target.transform.position, dmg, target);
                break;
            case eWeaponType.Ax:
                AttackSword(target, dmg);
                break;
        }
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
