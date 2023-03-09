using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    public Transform vfx;
    public Animator anim;

    // Movement
    private Unit unit;
    private Node nodeToMoveTo;
    private Queue<Node> path = new Queue<Node>();
    private PlayerTurn player;

    // Attacking
    private bool isAttacking;

    private void Start()
    {
        unit = GetComponent<Unit>();
        player = PlayerTurn.instance;
    }

    #region Movement Methods
    public void Move(List<Node> path)
    {
        ReadyMoveValues(path);
        anim.SetBool("IsMoving", true);
    }

    private void StopMoving()
    {
        ResetMoveValues();
        anim.SetBool("IsMoving", false);

        if (!isAttacking)
            return;
        StartCoroutine(Attack());
    }

    // Update Method
    public void MoveUnit()
    {
        if (!player.IsMoving())
            return;

        Vector3 dir = nodeToMoveTo.GetStandingPoint() - unit.node.GetStandingPoint();

        RotateUnit(dir);
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

    private void RotateUnit(Vector3 dir)
    {
        Quaternion startValue = vfx.rotation;

        float angle = (Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg) + 180;
        Quaternion endValue = Quaternion.Euler(0, angle, 0);

        vfx.rotation = Quaternion.Lerp(startValue, endValue, 3f * Time.deltaTime);
    }

    private void ResetMoveValues()
    {
        player.SetIsMoving(false);

        nodeToMoveTo = null;
        path = new Queue<Node>();
    }

    private void ReadyMoveValues(List<Node> path)
    {
        player.SetIsMoving(true);

        this.path = new Queue<Node>();
        for (int i = 1; i < path.Count; i++)
            this.path.Enqueue(path[i]);
        nodeToMoveTo = this.path.Dequeue();
    }

    public int MovementLeft()
    {
        return unit.stats.movement - unit.stats.moved;
    }
    #endregion

    #region Attack Methods
    IEnumerator Attack()
    {
        BasicAttack();
        float length = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length + 0.7f);
        player.SetIsAttacking(false);
        player.EndTurn();
    }
    #endregion

    #region Weapon Animations
    public void WeaponStrike(Weapon weapon)
    {
        anim.Play(weapon.weaponType);
    }

    public void BasicAttack()
    {
        anim.Play("BasicAttack");
    }
    #endregion
}
