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

    PlayerBodySkin skin;

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

        Utils.ReplaceSpritesWithUiImages(gameObject);
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
