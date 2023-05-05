using UnityEngine;
using UnityEngine.UI;
public class AbilitySlot : MonoBehaviour
{
    [Header("Scriptable Object References")]
    public PlayerTurn playerTurn;

    [Header("UI References")]
    public Image abilityIcon;

    private Ability ability;

    public void ActiveAbilityButton()
    {
        playerTurn.ChooseAbility(ability);
    }

    public void SetAbilityImage(Ability ability)
    {
        this.ability = ability;
        abilityIcon.sprite = ability.iconSprite;
    }

    public void SetAbilityImage(Sprite sprite)
    {
        abilityIcon.sprite = sprite;
    }

    public Ability GetAbility()
    {
        return ability;
    }
}
