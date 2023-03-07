using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    [Header("Unity Reference")]
    public GameObject weaponPanel;
    public List<Button> weaponAttackButtons;

    private List<Unit> targets;

    private PlayerTurn player;
    private Unit selected;

    void Start()
    {
        player = PlayerTurn.instance;
        selected = player.GetSelected();
    }

    private void Update()
    {
        
    }

    public void SetTargets()
    {

    }

    #region Event Handlers
    public void Weapon1AttackButton()
    {
        selected.SetWeapon(selected.weapons[0]);
    }

    public void Weapon2AttackButton()
    {
        selected.SetWeapon(selected.weapons[1]);
    }

    public void Weapon3AttackButton()
    {
        selected.SetWeapon(selected.weapons[2]);
    }

    public void Weapon4AttackButton()
    {
        selected.SetWeapon(selected.weapons[3]);
    }
    #endregion
}
