using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWeaponPanel : MonoBehaviour
{
    [Header("Unity Reference")]
    public List<Button> weaponAttackButtons;

    private PlayerTurn player;
    private Unit selected;

    #region Finds Weapons
    public void Setup()
    {
        if (player == null)
            player = PlayerTurn.instance;

        selected = player.GetSelected();
        SetWeapons();
        TurnOffRemainingButtons();
    }

    private void SetWeapons()
    {
        for (int i = 0; i < selected.weapons.Count; i++)
            ConfigureButton(weaponAttackButtons[i], selected.weapons[i]);
    }

    private void ConfigureButton(Button button, Weapon weapon)
    {
        TextMeshProUGUI text = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.SetText(weapon.name);
    }

    private void TurnOffRemainingButtons()
    {
        for (int i = selected.weapons.Count; i < weaponAttackButtons.Count; i++)
            weaponAttackButtons[i].gameObject.SetActive(false);
    }
    #endregion

    #region Setters
    public void SetActiveWeaponPanel(bool value)
    {
        gameObject.SetActive(value);
    }
    #endregion

    #region Event Handlers
    public void Weapon1SelectButton()
    {
        selected.SetWeapon(selected.weapons[0]);
    }

    public void Weapon2SelectButton()
    {
        selected.SetWeapon(selected.weapons[1]);
    }

    public void Weapon3SelectButton()
    {
        selected.SetWeapon(selected.weapons[2]);
    }

    public void Weapon4SelectButton()
    {
        selected.SetWeapon(selected.weapons[3]);
    }
    #endregion
}
