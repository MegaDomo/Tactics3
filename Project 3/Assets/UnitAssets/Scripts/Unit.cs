using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitType { Player, Enemy, Neutral, Ally, Civilian }

    [Header("Unity References")]
    public Transform ground;
    public Transform weaponPoint;
    public Animator anim;

    [Header("Attributes")]
    public UnitType type;
    public UnitStats stats;
    public Weapon equippedWeapon;
    public List<Weapon> weapons;
    public List<Ability> abilities;

    [Header("Debugging")]
    public Behavior behavior;

    [HideInInspector] public Node node;
    [HideInInspector] public Vector3 offset;

    private GameObject weaponPrefab;

    private Node nodeToMoveTo;
    private bool isUnitInRange;
    private bool isMoving;

    void Awake()
    {
        offset = transform.position - ground.position;
    }

    private void Start()
    {
        SetSelf();
        SetWeapon(weapons[0]);
    }

    private void Update()
    {
        MoveUnit();
    }

    public void StartTurn()
    {
        // Resets how far he moved
        stats.moved = 0;




        // TODO : Handle Status Effects / Tile Effects

    }

    public void EndTurn()
    {

    }

    #region Damage Methods
    public void TakePhysicalDamage(int damage)
    {
        // TODO : If health < 0 Death()
        stats.curHealth -= damage - stats.defense < 0 ? 0 : damage - stats.defense;
    }

    public void TakeMagicalDamage(int damage)
    {
        stats.curHealth -= damage - stats.spDefense < 0 ? 0 : damage - stats.spDefense;
    }

    public int ForecastPhysicalDamage(int damage)
    {
        return damage - stats.defense < 0 ? 0 : damage - stats.defense;
    }

    public int ForecastMagicalDamage(int damage)
    {
        return damage - stats.spDefense < 0 ? 0 : damage - stats.spDefense;
    }
    #endregion

    #region Movement Methods
    public IEnumerator Move(int pathCost, List<Node> path)
    {
        isMoving = true;
        anim.SetBool("IsMoving", true);

        // Some Loop to move through
        for (int i = 1; i < path.Count; i++)
        {
            nodeToMoveTo = path[i];
            yield return new WaitUntil(() => isUnitInRange);
        }

        stats.moved += pathCost;
        isMoving = false;
        anim.SetBool("IsMoving", false);
    }

    // This is an Update Method
    private void MoveUnit()
    {
        if (!isMoving)
            return;
        Vector3 dir = nodeToMoveTo.GetStandingPoint() - node.GetStandingPoint();
        transform.Translate(dir * 10f * Time.deltaTime);
        isUnitInRange = IsUnitWithinNode(nodeToMoveTo);
    }

    public bool IsUnitWithinNode(Node node)
    {
        float ux = transform.position.x;
        float uz = transform.position.z;

        float epsilon = 1f;

        float xFloor = node.x - epsilon;
        float xCeil = node.x + epsilon;
        float zFloor = node.z - epsilon;
        float zCeil = node.z + epsilon;

        if (ux < xFloor && ux > xCeil)
            return false;
        if (uz < zFloor && uz > zCeil)
            return false;
        return true;
    }
    public int MovementLeft()
    {
        return stats.movement - stats.moved;
    }
    #endregion

    #region Setters
    public void SetWeapon(Weapon _weapon)
    {
        if (_weapon == null)
            return;
        weaponPrefab = Instantiate(_weapon.prefab, weaponPoint.position, weaponPoint.rotation);
        weaponPrefab.transform.SetParent(weaponPoint.transform);
        equippedWeapon = _weapon;
    }

    private void SetSelf()
    {
        if (type == Unit.UnitType.Enemy)
        {
            gameObject.tag = "Enemy";
            behavior.self = this;
        }

        if (type == Unit.UnitType.Player)
        {
            gameObject.tag = "Player";
        }

    }
    #endregion
}
