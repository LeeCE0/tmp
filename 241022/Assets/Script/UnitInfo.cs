using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
using UnityEditor.Animations;

public class UnitInfo : MonoBehaviour
{

    public float moveSpeed = 2.0f;
    [SerializeField] SpriteRenderer image;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] AnimatorController animController;




    [SerializeField] int ID;
    [SerializeField] string Name;
    [SerializeField] int HP;
    [SerializeField] int cost;

    MyInfo.eUnitState unitState = MyInfo.eUnitState.Idle;
    bool isMyUnit = true;

    void Update()
    {
        // 적 스폰지점으로 이동
        //if (isMyUnit)
        //    transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        //else
        //    transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

    }

    public void SetData(MyInfo.UnitData data)
    {
        ID = data.UnitID;
        Name = data.UnitName;
        HP = data.HP;
        cost = data.Cost;
        moveSpeed = data.UnitSpeed;
    }

    public void SetState(MyInfo.eUnitState state)
    {
        unitState = state;
        switch (state)
        {
            case MyInfo.eUnitState.Idle:
                break;
            case MyInfo.eUnitState.Walk:
                break;
            case MyInfo.eUnitState.Attack:
                break;
            case MyInfo.eUnitState.Dead:
                break;
        }
    }

}
