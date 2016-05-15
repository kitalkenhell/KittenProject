using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UpgradeSelectionButton : MonoBehaviour
{
    public Text upgradeName;
    public Transform spritePivot;

    public Action<PlayerUpgrade> onButtonPressed;

    PlayerUpgrade upgrade;

    public void Refresh(PlayerUpgrade upgrade)
    {
        GameObject icon = Instantiate(Resources.Load(upgrade.icon, typeof(GameObject)) as GameObject);
        this.upgrade = upgrade;
        upgradeName.text = upgrade.itemName;
        
        icon.transform.SetParent(spritePivot);
        icon.transform.ResetLocal();

        Utils.ReplaceSpritesWithUiImages(gameObject);

        spritePivot.transform.localScale = Vector3.one * upgrade.wardrobeScale;
        spritePivot.transform.Translate(upgrade.wardrobePivotOffset);
    }

    public void OnButtonPressed()
    {
        if (onButtonPressed != null)
        {
            onButtonPressed(upgrade);
        }
    }
}
