using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UpgradeSelectionButton : MonoBehaviour
{
    public Text upgradeName;
    public Text upgradeDescription;
    public Transform spritePivot;
    public Button button;
    public Color lockedColor;
    public GameObject lockedIcon;
    public GameObject boughtIcon;

    public Action<PlayerUpgrade> onButtonPressed;

    public PlayerUpgrade Upgrade
    {
        get;
        private set;
    }

    Image[] images;

    public void Awake()
    {
        PostOffice.playerUpgradeBought += OnPlayerUpgradeBought;
    }

    public void OnDestroy()
    {
        PostOffice.playerUpgradeBought -= OnPlayerUpgradeBought;
    }

    public void Refresh(PlayerUpgrade upgrade)
    {
        GameObject icon = Instantiate(Resources.Load(upgrade.icon, typeof(GameObject)) as GameObject);
        this.Upgrade = upgrade;
        upgradeName.text = upgrade.itemName;
        upgradeDescription.text = upgrade.description;

        icon.transform.SetParent(spritePivot);
        icon.transform.ResetLocal();

        Utils.ReplaceSpritesWithUiImages(gameObject);

        spritePivot.transform.localScale = Vector3.one * upgrade.wardrobeScale;
        spritePivot.transform.Translate(upgrade.wardrobePivotOffset);

        images = spritePivot.GetComponentsInChildren<Image>();

        if (upgrade.Locked)
        {
            ShowAsLocked();
        }
        else if (PersistentData.IsHavingUpgrade(upgrade.itemName))
        {
            ShowAsBought();
        }
    }

    public void SetColorOfChildren(Color color)
    {
        upgradeDescription.color = color;

        if (images != null)
        {
            foreach (var image in images)
            {
                image.color = color;
            }
        }
    }

    public void ShowAsLocked()
    {
        button.interactable = false;
        SetColorOfChildren(lockedColor);
        lockedIcon.SetActive(true);
        boughtIcon.SetActive(false);
    }

    public void ShowAsUnlocked()
    {
        button.interactable = true;
        SetColorOfChildren(Color.white);
        lockedIcon.SetActive(false);
        boughtIcon.SetActive(false);
    }

    public void ShowAsBought()
    {
        ColorBlock colors = button.colors;

        button.interactable = false;
        colors.disabledColor = Color.white;
        button.colors = colors;
        lockedIcon.SetActive(false);
        boughtIcon.SetActive(true);
    }

    public void OnPlayerUpgradeBought(PlayerUpgrade upgrade)
    {
        if (this.Upgrade.baseUpgrade != null && this.Upgrade.baseUpgrade.itemName == upgrade.itemName)
        {
            ShowAsUnlocked();
        }
        else if (this.Upgrade.itemName == upgrade.itemName)
        {
            ShowAsBought();
        }
    }

    public void OnButtonPressed()
    {
        if (onButtonPressed != null)
        {
            onButtonPressed(Upgrade);
        }
    }
}
