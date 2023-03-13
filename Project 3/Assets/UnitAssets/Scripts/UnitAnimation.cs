using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    public Transform vfx;
    public Animator anim;

    // Movement
    private bool isMoving;
    private bool isAttacking;
    private Unit unit;
    private Node nodeToMoveTo;
    private Queue<Node> path = new Queue<Node>();
    private PlayerTurn player;
    private EnemyAI enemyAI;

    private void Start()
    {
        unit = GetComponent<Unit>();
        player = PlayerTurn.instance;
        enemyAI = EnemyAI.instance;
    }

    #region Movement Methods
    public void Move(List<Node> path)
    {
        ReadyMoveValues(path);
    }

    private void StopMoving()
    {
        ResetMoveValues();

        if (IsAttacking())
        {
            StartCoroutine(Attack());
            return;
        }

        EndTurn();
    }

    private void ReadyMoveValues(List<Node> path)
    {
        anim.SetBool("IsMoving", true);

        this.path = new Queue<Node>();
        for (int i = 1; i < path.Count; i++)
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
    public void MoveUnit()
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
        return unit.stats.movement - unit.stats.moved;
    }
    #endregion

    #region Attack Methods
    IEnumerator Attack()
    {
        Vector3 dir = player.GetTarget().node.GetStandingPoint() - unit.node.GetStandingPoint();
        RotateUnit(dir);
        BasicAttack();
        yield return new WaitForSeconds(0.1f);
        float length = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);
        SetIsAttacking(false);
        EndTurn();
    }
    #endregion

    #region Weapon Animations
    public void BasicAttack()
    {
        anim.Play("BasicAttack");
    }
    #endregion

    #region Utility
    private void EndTurn()
    {
        if (unit.unitType == Unit.UnitType.Player)
            player.EndTurn();
        if (unit.unitType == Unit.UnitType.Enemy)
            enemyAI.EndTurn();
    }
    #endregion

    #region Setters
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
