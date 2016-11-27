using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HatSkinSelectionButton : MonoBehaviour
{
    public Text skinName;
    public Transform spritePivot;
    public GameObject lockedIcon;
    public GameObject equippedIcon;

    public Action<PlayerHatSkin> onButtonPressed;

    PlayerHatSkin skin;

    void Awake()
    {
        PostOffice.playerHatSkinBought += OnSkinBought;
        PostOffice.playerHatSkinEquipped += OnSkinEquipped;
    }

    void OnDestroy()
    {
        PostOffice.playerHatSkinBought -= OnSkinBought;
        PostOffice.playerHatSkinEquipped -= OnSkinEquipped;
    }

    void OnSkinBought(PlayerHatSkin skin)
    {
        if (this.skin.itemName == skin.itemName)
        {
            lockedIcon.SetActive(false);
        }
    }

    void OnSkinEquipped(PlayerHatSkin skin)
    {
        equippedIcon.SetActive(this.skin.itemName == skin.itemName);
    }

    public void Refresh(PlayerHatSkin skin)
    {
        GameObject hat = Instantiate(Resources.Load(skin.hat, typeof(GameObject)) as GameObject);
        this.skin = skin;
        skinName.text = skin.itemName;
        
        hat.transform.parent = spritePivot;
        hat.transform.ResetLocal();

        Utils.ReplaceSpritesWithUiImages(gameObject, false);

        spritePivot.transform.localScale = Vector3.one * skin.wardrobeScale;
        spritePivot.transform.Translate(skin.wardrobePivotOffset);

        if (PersistentData.PlayerHatSkin == skin.itemName)
        {
            lockedIcon.SetActive(false);
            equippedIcon.SetActive(true);
        }
        else if (!PersistentData.IsHavingHatSkin(skin.itemName))
        {
            lockedIcon.SetActive(true);
            equippedIcon.SetActive(false);
        }
        else
        {
            lockedIcon.SetActive(false);
            equippedIcon.SetActive(false);
        }
    }

    public void OnButtonPressed()
    {
        if (onButtonPressed != null)
        {
            onButtonPressed(skin);
        }
    }
}
