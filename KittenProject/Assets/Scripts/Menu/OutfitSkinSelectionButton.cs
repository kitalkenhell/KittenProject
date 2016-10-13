using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class OutfitSkinSelectionButton : MonoBehaviour
{
    public Text skinName;

    public PlayerHatSkin hatSkin;
    
    public Transform bodyPivot;
    public Transform earPivot;
    public Transform eyeBigPivot;
    public Transform eyeSmallPivot;
    public Transform legFrontPivot;
    public Transform legBackPivot;
    public Transform mouthPivot;
    public Transform tonguePivot;
    public Transform nosePivot;
    public Transform tailPivot;
    public Transform hatPivot;

    public Action<PlayerBodySkin> onButtonPressed;

    public GameObject lockedIcon;
    public GameObject equippedIcon;

    PlayerBodySkin skin;

    void Awake()
    {
        PostOffice.playerBodySkinBought += OnSkinBought;
        PostOffice.playerBodySkinEquipped += OnSkinEquipped;
    }

    void OnDestroy()
    {
        PostOffice.playerBodySkinBought -= OnSkinBought;
        PostOffice.playerBodySkinEquipped -= OnSkinEquipped;
    }

    void OnSkinBought(PlayerBodySkin skin)
    {
        if (this.skin.itemName == skin.itemName)
        {
            lockedIcon.SetActive(false);
        }
    }

    void OnSkinEquipped(PlayerBodySkin skin)
    {
        equippedIcon.SetActive(this.skin.itemName == skin.itemName);
    }


    public void Refresh(PlayerBodySkin skin)
    {
        this.skin = skin;
        skinName.text = skin.itemName;

        InstantiateSkinPart(skin.legFront, legFrontPivot);
        InstantiateSkinPart(skin.legBack, legBackPivot);
        InstantiateSkinPart(skin.tail, tailPivot);
        InstantiateSkinPart(skin.body, bodyPivot);
        InstantiateSkinPart(skin.ear, earPivot);
        InstantiateSkinPart(skin.eyeBig, eyeBigPivot);
        InstantiateSkinPart(skin.eyeSmall, eyeSmallPivot);
        InstantiateSkinPart(skin.mouth, mouthPivot);
        InstantiateSkinPart(skin.nose, nosePivot);
        InstantiateSkinPart(skin.tongue, tonguePivot);
        InstantiateSkinPart(hatSkin.hat, hatPivot);

        Utils.ReplaceSpritesWithUiImages(gameObject, false);

        bodyPivot.parent.localScale = Vector3.one * skin.wardrobeScale;
        bodyPivot.parent.Translate(skin.wardrobePivotOffset);

        if (GameSettings.PlayerBodySkin == skin.itemName)
        {
            lockedIcon.SetActive(false);
            equippedIcon.SetActive(true);
        }
        else if (!PersistentData.IsHavingBodySkin(skin.itemName))
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

    void InstantiateSkinPart(string prefabName, Transform pivot)
    {
        GameObject skinPart = Instantiate(Resources.Load(prefabName, typeof(GameObject)) as GameObject);
        skinPart.transform.parent = pivot;
        skinPart.transform.ResetLocal();
    }
}
