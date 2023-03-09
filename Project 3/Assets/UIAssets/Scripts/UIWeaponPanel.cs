using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWeaponPanel : MonoBehaviour
{
    [Header("Unity Reference")]
    public UITargetPanel targetPanel;
    public List<Button> weaponAttackButtons;

    private PlayerTurn player;
    private Unit selected;

    private void Awake()
    {
        player = PlayerTurn.instance;
    }

    void Start()
    {
        SetActiveWeaponPanel(false);
    }

    private void OnEnable()
    {
        if (player == null)
            return;
        selected = player.GetSelected();
        SetWeapons();
        TurnOffRemainingButtons();
    }

    private void SetWeapons()
    {
        for (int i = 0; i < selected.weapons.Count; i++)
            ConfigureButton(i, weaponAttackButtons[i], selected.weapons[i]);
    }

    private void ConfigureButton(int index, Button button, Weapon weapon)
    {
        TextMeshProUGUI text = button.transform.GetChild(index).GetComponent<TextMeshProUGUI>();
        text.SetText(weapon.name);
    }
    private void TurnOffRemainingButtons()
    {
        for (int i = selected.weapons.Count - 1; i < weaponAttackButtons.Count; i++)
            weaponAttackButtons[i].gameObject.SetActive(false);
    }

    private void ChangeToTargetPanel()
    {
        targetPanel.SetActiveTargetPanel(true);
        gameObject.SetActive(false);
    }

    public void SetActiveWeaponPanel(bool value)
    {
        gameObject.SetActive(value);
    }

    #region Event Handlers
    public void Weapon1SelectButton()
    {
        selected.SetWeapon(selected.weapons[0]);
        ChangeToTargetPanel();
    }

    public void Weapon2SelectButton()
    {
        selected.SetWeapon(selected.weapons[1]);
        ChangeToTargetPanel();
    }

    public void Weapon3SelectButton()
    {
        selected.SetWeapon(selected.weapons[2]);
        ChangeToTargetPanel();
    }

    public void Weapon4SelectButton()
    {
        selected.SetWeapon(selected.weapons[3]);
        ChangeToTargetPanel();
    }
    #endregion
}
