using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RaffleScreen : MonoBehaviour
{
    public PlayerItems items;
    public WardrobeSkinSetter skinSetter;
    public Text skinNameLabel;
    public GameObject parachute;

    int stepAnimHash;

    Animator animator;

    System.Action onEquip = null;

    void Awake()
    {
        animator = GetComponent<Animator>();

        stepAnimHash = Animator.StringToHash("Step");
    }

    public void OnOpenGiftClicked()
    {
        animator.SetTrigger(stepAnimHash);
        SetRaffleSkin();
    }

    public void OnCloseButtonClicked()
    {
        animator.SetTrigger(stepAnimHash);
    }

    public void OnEquipButtonClicked()
    {
        animator.SetTrigger(stepAnimHash);

        if (onEquip != null)
        {
            onEquip();
            CoreLevelObjects.player.GetComponent<PlayerSkinSetter>().ResetSkin();
        }
    }

    public void OnClosedAnimationFinished()
    {
        gameObject.SetActive(false);
    }

    void SetRaffleSkin()
    {
        const float hatChance = 0.7f;
        const float parachuteChance = 0.5f;

        PlayerHatSkin hat = null;
        PlayerParachuteSkin parachute = null;
        PlayerBodySkin body = null;

        if (Random.value < hatChance)
        {
            hat = items.GetRandomNotUnlockedHat();
            parachute = null;

            if (hat == null)
            {
                if (Random.value < parachuteChance)
                {
                    parachute = items.GetRandomNotUnlockedParachute();

                    if (parachute == null)
                    {
                        body = items.GetRandomNotUnlockedBodySkin();
                        SetBodySkin(body);
                    }
                    else
                    {
                        SetParachute(parachute);
                    }
                }
                else
                {
                    body = items.GetRandomNotUnlockedBodySkin();

                    if (body == null)
                    {
                        parachute = items.GetRandomNotUnlockedParachute();
                        SetParachute(parachute);
                    }
                    else
                    {
                        SetBodySkin(body);
                    }
                }
            }
            else
            {
                SetHat(hat);
            }
        }
        else
        {
            if (Random.value < parachuteChance)
            {
                parachute = items.GetRandomNotUnlockedParachute();

                if (parachute == null)
                {
                    body = items.GetRandomNotUnlockedBodySkin();

                    if (body == null)
                    {
                        hat = items.GetRandomNotUnlockedHat();
                        SetHat(hat);
                    }
                    else
                    {
                        SetBodySkin(body);
                    }
                }
                else
                {
                    SetParachute(parachute);
                }
            }
            else
            {
                body = items.GetRandomNotUnlockedBodySkin();

                if (body == null)
                {
                    parachute = items.GetRandomNotUnlockedParachute();

                    if (parachute == null)
                    {
                        hat = items.GetRandomNotUnlockedHat();
                        SetHat(hat);
                    }
                    else
                    {
                        SetParachute(parachute);
                    }
                }
                else
                {
                    SetBodySkin(body);
                }
            }
        }
    }

    void SetBodySkin(PlayerBodySkin skin)
    {
        skinSetter.SetSkin(skin);
        skinNameLabel.text = skin.itemName;
        parachute.SetActive(false);
        PersistentData.IsHavingBodySkin(skin.itemName, true);

        onEquip = () =>
        {
            GameSettings.PlayerBodySkin = skin.itemName;
        };
    }

    void SetHat(PlayerHatSkin skin)
    {
        skinSetter.SetSkin(skin);
        skinNameLabel.text = skin.itemName;
        parachute.SetActive(false);
        PersistentData.IsHavingHatSkin(skin.itemName, true);

        onEquip = () =>
        {
            GameSettings.PlayerHatSkin = skin.itemName;
        };
    }

    void SetParachute(PlayerParachuteSkin skin)
    {
        skinSetter.SetSkin(skin);
        skinNameLabel.text = skin.itemName;
        parachute.SetActive(true);
        PersistentData.IsHavingParachuteSkin(skin.itemName, true);

        onEquip = () =>
        {
            GameSettings.PlayerParachuteSkin = skin.itemName;
        };
    }
}
