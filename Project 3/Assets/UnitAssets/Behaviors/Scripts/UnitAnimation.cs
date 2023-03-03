using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{

    [Header("Unity References")]
    public Transform vfx;
    public Animator anim;

    // Movement
    private Queue<Node> path = new Queue<Node>();
    private Unit unit;
    private Node nodeToMoveTo;
    private bool isMoving;
    private bool doneMoving;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    private void Update()
    {
        MoveUnit();
    }

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

        unit.stats.moved += pathCost;

        anim.SetBool("IsMoving", false);
    }

    // Update Method
    private void MoveUnit()
    {
        if (!isMoving)
            return;

        Vector3 dir = nodeToMoveTo.GetStandingPoint() - unit.node.GetStandingPoint();
        //Debug.Log(node.x + ", " + node.z);
        Debug.Log(dir);
        RotateUnit(dir);
        transform.Translate(dir.normalized * unit.stats.pathingSpeed * Time.deltaTime);
        if (IsUnitWithinNode(nodeToMoveTo))
        {
            //Debug.Log("Loading Next Node");
            if (path.Count == 0)
            {
                //Debug.Log("Here");
                doneMoving = true;
                unit.node = nodeToMoveTo;
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
        return unit.stats.movement - unit.stats.moved;
    }
    #endregion


}
