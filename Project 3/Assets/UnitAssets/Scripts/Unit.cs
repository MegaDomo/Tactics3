using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitType { Player, Enemy, Neutral, Ally, Civilian }

    [Header("Unity References")]
    public Transform vfx;
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

    // Movement
    private Queue<Node> path = new Queue<Node>();
    private Node nodeToMoveTo;
    private bool isMoving;
    private bool doneMoving;

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


    #region Turn Methods
    public void StartTurn()
    {
        // Resets how far he moved
        stats.moved = 0;




        // TODO : Handle Status Effects / Tile Effects

    }

    public void EndTurn()
    {

    }
    #endregion

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
        for (int i = 1; i < path.Count; i++)
            this.path.Enqueue(path[i]);
        nodeToMoveTo = this.path.Dequeue();

        isMoving = true;
        anim.SetBool("IsMoving", true);

        doneMoving = false;
        yield return new WaitUntil(() => doneMoving);
        ResetMoveValues();

        stats.moved += pathCost;
        
        anim.SetBool("IsMoving", false);
    }

    // Update Method
    private void MoveUnit()
    {
        if (!isMoving)
            return;

        Vector3 dir = nodeToMoveTo.GetStandingPoint() - node.GetStandingPoint();
        //Debug.Log(node.x + ", " + node.z);
        Debug.Log(dir);
        RotateUnit(dir);
        transform.Translate(dir.normalized * stats.pathingSpeed * Time.deltaTime);
        if (IsUnitWithinNode(nodeToMoveTo))
        {
            //Debug.Log("Loading Next Node");
            if (path.Count == 0)
            {
                //Debug.Log("Here");
                doneMoving = true;
                node = nodeToMoveTo;
                return;
            }
            node = nodeToMoveTo;
            nodeToMoveTo = path.Dequeue();
        }
    }

    private bool IsUnitWithinNode(Node targetNode)
    {
        float ux = transform.position.x;
        float uz = transform.position.z;

        float epsilon = 1f;
        Vector3 des = node.grid.GetWorldPosition(targetNode);
        float xFloor = des.x - epsilon;
        float xCeil = des.x + epsilon;
        float zFloor = des.z - epsilon;
        float zCeil = des.z + epsilon;
        if (ux >= xFloor && ux <= xCeil && uz >= zFloor && uz <= zCeil)
            return true;
        return false;
    }

    private void RotateUnit(Vector3 dir)
    {
        Quaternion startValue = vfx.rotation;

        float angle = (Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg) + 180;
        Debug.Log(angle);
        Quaternion endValue = Quaternion.Euler(0, angle, 0);

        vfx.rotation = Quaternion.Lerp(startValue, endValue, 100f * Time.deltaTime);
    }
    private void ResetMoveValues()
    {
        isMoving = false;
        doneMoving = false;

        nodeToMoveTo = null;
        path = new Queue<Node>();
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
