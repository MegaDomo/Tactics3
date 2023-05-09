using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [Header("Scriptable Object References")]
    public PlayerTurn playerTurn;
    public EnemyAI enemyAI;

    [Header("Unity References")]
    public Transform ground;
    public Transform weaponPoint;
    public Transform EffectPoint;
    public Transform vfx;
    public Animator anim;

    [HideInInspector] public Vector3 offset;

    private Unit unit;
    private bool isMoving;
    private bool isAttacking;
    private GameObject weaponPrefab;
    private Node nodeToMoveTo;
    private Queue<Node> path = new Queue<Node>();

    private void Update()
    {
        MoveUnitUpdate();
    }

    public void Setup(Unit unit)
    {
        this.unit = unit;
        SetWeapon(unit.weapons[0]);
        offset = transform.position - ground.position;
    }

    private void EndTurn()
    {
        if (unit.unitType == Unit.UnitType.Player)
            playerTurn.EndTurn();
        if (unit.unitType == Unit.UnitType.Enemy)
            enemyAI.EndTurn();
    }

    #region Animation
    public void Move(Node destination)
    {
        List<Node> path = Pathfinding.AStar(unit.GetMap(), unit.node, destination);

        Move(path);

        unit.node.OnUnitExit();
        destination.OnUnitEnter(unit);
    }

    private void Move(List<Node> path)
    {
        if (path.Count == 0)
        {
            CheckAttack();
            return;
        }
        ReadyMoveValues(path);
    }

    private void StopMoving()
    {
        ResetMoveValues();
        CheckAttack();
    }

    private void ReadyMoveValues(List<Node> path)
    {
        anim.SetBool("IsMoving", true);

        this.path = new Queue<Node>();
        for (int i = 0; i < path.Count; i++)
            this.path.Enqueue(path[i]);
        nodeToMoveTo = this.path.Dequeue();

        SetIsMoving(true);
    }

    private void ResetMoveValues()
    {
        SetIsMoving(false);
        anim.SetBool("IsMoving", false);

        nodeToMoveTo = null;
        path = new Queue<Node>();
    }

    // Update Method
    private void MoveUnitUpdate()
    {
        if (!IsMoving())
            return;
        if (nodeToMoveTo == null)
            return;

        Vector3 dir = nodeToMoveTo.GetStandingPoint() - unit.node.GetStandingPoint();
        LerpRotateUnit(dir);
        transform.Translate(dir.normalized * unit.stats.pathingSpeed * Time.deltaTime);
        if (IsUnitWithinNode(nodeToMoveTo))
        {
            if (path.Count == 0)
            {
                unit.node = nodeToMoveTo;
                StopMoving();
                return;
            }
            unit.node = nodeToMoveTo;
            nodeToMoveTo = path.Dequeue();
        }
    }

    #region Animation Utility
    private bool IsUnitWithinNode(Node targetNode)
    {
        float ux = transform.position.x;
        float uz = transform.position.z;

        float epsilon = 1f;
        Vector3 des = unit.node.grid.GetWorldPosition(targetNode);
        float xFloor = des.x - epsilon;
        float xCeil = des.x + epsilon;
        float zFloor = des.z - epsilon;
        float zCeil = des.z + epsilon;
        if (ux >= xFloor && ux <= xCeil && uz >= zFloor && uz <= zCeil)
            return true;
        return false;
    }

    private void LerpRotateUnit(Vector3 dir)
    {
        Quaternion startValue = vfx.rotation;

        float angle = (Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg) + 180;
        Quaternion endValue = Quaternion.Euler(0, angle, 0);

        vfx.rotation = Quaternion.Lerp(startValue, endValue, 3f * Time.deltaTime);
    }

    private void RotateUnit(Vector3 dir)
    {
        float angle = (Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg) + 180;
        Quaternion endValue = Quaternion.Euler(0, angle, 0);

        vfx.rotation = endValue;
    }

    public int MovementLeft()
    {
        return unit.stats.movement;
    }
    #endregion
    #endregion

    #region Attack Methods
    IEnumerator AttackAnimation()
    {
        Unit target;
        if (unit.unitType == Unit.UnitType.Player)
            target = playerTurn.GetTarget();
        else
            target = enemyAI.GetTarget();

        Vector3 dir = target.node.GetStandingPoint() - unit.node.GetStandingPoint();
        RotateUnit(dir);
        BasicAttack();
        yield return new WaitForSeconds(0.1f);
        float length = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);
        SetIsAttacking(false);
        EndTurn();
    }

    private void CheckAttack()
    {
        if (IsAttacking())
        {
            Attack();
            return;
        }
        EndTurn();
    }

    public void Attack()
    {
        StartCoroutine(AttackAnimation());
    }

    public void BasicAttack()
    {
        anim.Play("BasicAttack");
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
        isMoving = value;
    }

    public bool IsMoving()
    {
        return isMoving;
    }
    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }
    public bool IsAttacking()
    {
        return isAttacking;
    }
    #endregion
}
