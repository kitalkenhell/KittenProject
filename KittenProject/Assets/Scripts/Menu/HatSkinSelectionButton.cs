using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HatSkinSelectionButton : MonoBehaviour
{
    public Text skinName;
    public Transform spritePivot;

    public Action<PlayerHatSkin> onButtonPressed;

    PlayerHatSkin skin;

    public void Refresh(PlayerHatSkin skin)
    {
        GameObject hat = Instantiate(Resources.Load(skin.hat, typeof(GameObject)) as GameObject);
        this.skin = skin;
        skinName.text = skin.skinName;
        
        hat.transform.parent = spritePivot;
        hat.transform.ResetLocal();

        Utils.ReplaceSpritesWithUiImages(gameObject);

        spritePivot.transform.localScale = Vector3.one * skin.wardrobeScale;
        spritePivot.transform.Translate(skin.wardrobePivotOffset);
    }

    public void OnButtonPressed()
    {
        if (onButtonPressed != null)
        {
            onButtonPressed(skin);
        }
    }
}
