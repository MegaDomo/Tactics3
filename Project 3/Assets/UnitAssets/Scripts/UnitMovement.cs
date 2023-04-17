using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [Header("Debugging Scriptable Object")]
    public Unit unit;

    [Header("Unity References")]
    public Transform ground;
    public Transform weaponPoint;
    public Transform EffectPoint;
    public Transform vfx;
    public Animator anim;

    [HideInInspector] public Vector3 offset;
    [HideInInspector] public UnitAnimation unitAnim;

    private GameObject weaponPrefab;

    private void Update()
    {
        unitAnim.MoveUnit();
    }

    public void SetupUnit(Unit unit)
    {
        this.unit = unit;
        unit.unitMovement = this;
        offset = transform.position - ground.position;
        unitAnim = GetComponent<UnitAnimation>();
        unitAnim.SetUnit(unit);
    }

    #region Movement
    public void Move(Node destination)
    {
        List<Node> path = Pathfinding.AStar(unit.GetMap(), unit.node, destination);

        unitAnim.Move(path);

        unit.node.OnUnitExit();
        destination.OnUnitEnter(unit);
    }

    public int MovementLeft()
    {
        return unit.stats.movement - unit.stats.moved;
    }
    #endregion

    #region Getters & Setters
    public void SetWeapon(Weapon _weapon)
    {
        if (_weapon == null)
            return;

        if (weaponPrefab != null)
            Destroy(weaponPrefab);
        weaponPrefab = Instantiate(_weapon.prefab, weaponPoint.position, weaponPoint.rotation);
        weaponPrefab.transform.SetParent(weaponPoint.transform);
    }

    public void SetIsMoving(bool value)
    {
        unitAnim.SetIsMoving(value);
    }

    public bool IsMoving()
    {
        return unitAnim.IsMoving();
    }
    public void SetIsAttacking(bool value)
    {
        unitAnim.SetIsAttacking(value);
    }
    public bool IsAttacking()
    {
        return unitAnim.IsAttacking();
    }
    #endregion
}
