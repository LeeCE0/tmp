using System.Collections;
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
        Heal = 4,
        Ax,
    }
    [SerializeField] GameObject projectileOBJ;
    public eWeaponType weaponType = eWeaponType.None; // 무기 타입
    public Vector2 startPosition; // 시작 위치
    public Vector2 targetPosition; // 목표 위치
    public float speed = 5f; // 이동 속도
    public float arcHeight = 2f; // 포물선 높이 (필요시)
    public bool isMeeleeType = false;
    public bool isAttack = false;
    public UnitBase unit;
    public Sprite obImg;
    private bool isMine;
     
    
    //원거리 (화살)
    public void LaunchArrow(GameObject start, Vector2 target, int dmg, UnitBase targetUnit)
    {
        startPosition = start.transform.position;
        targetPosition = target;
        transform.position = start.transform.position;

        GameObject projectile = ObjectPoolManager.Instance.SpawnFromPool(ObjectPoolManager.ePoolingObj.Projectile_Arrow, unit.gameObject, Quaternion.identity);
        projectile.transform.position = start.transform.position;

        Projectile projectileData = projectile.GetComponent<Projectile>();
        projectileData.Init(dmg, targetUnit, ObjectPoolManager.ePoolingObj.Projectile_fireBall, isMine);
    }

    //원거리 (마법)
    private void LaunchFireball(GameObject start, Vector2 target, int dmg, UnitBase targetUnit)
    {
        GameObject projectile = ObjectPoolManager.Instance.SpawnFromPool(ObjectPoolManager.ePoolingObj.Projectile_fireBall, unit.gameObject, Quaternion.identity);
        projectile.transform.position = start.transform.position;

        Projectile projectileData = projectile.GetComponent<Projectile>();
        projectileData.Init(dmg, targetUnit, ObjectPoolManager.ePoolingObj.Projectile_fireBall, isMine);
    }

    //힐
    private void GetHeal(int amount, UnitBase targetUnit)
    {
        targetUnit.HealHP(amount);
    }

    //근거리
    private void AttackSword(UnitBase target, int dmg)
    {
        target.TakeDMG(dmg);
    }

    //유닛 무기 정보 설정하기
    public void SetWeapon(eUnitType unitType, UnitBase units)
    {
        unit = units;
        isMine = unit.isMyUnit;
        switch (unitType)
        {
            case eUnitType.Swordmaster:
                weaponType = eWeaponType.Sword;
                break;
            case eUnitType.Archer:
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
    public void StartAttack(int dmg, UnitBase target)
    {
        isAttack = true;
        switch (weaponType)
        {
            case eWeaponType.Sword:
                AttackSword(target, dmg);
                break;            
            case eWeaponType.Magic:
                LaunchFireball(gameObject, target.transform.position, dmg, target);
                break;
            case eWeaponType.Heal:
                GetHeal(dmg, target);
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
