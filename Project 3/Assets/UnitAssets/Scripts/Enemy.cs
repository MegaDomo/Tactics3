using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Which Enemy")]
    public EnemyObject enemyObject;

    private void Start()
    {
        if (enemyObject == null)
            return;

        GetComponent<Unit>().SetAsEnemy(enemyObject);   
    }
}
