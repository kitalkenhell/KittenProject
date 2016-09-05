using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ParachuteSkinSelectionButton : MonoBehaviour
{
    public Text skinName;
    public Transform spritePivot;
    public GameObject lockedIcon;
    public GameObject equippedIcon;

    public System.Action<PlayerParachuteSkin> onButtonPressed;

    PlayerParachuteSkin skin;

    void Awake()
    {
        PostOffice.playerParachuteSkinBought += OnSkinBought;
        PostOffice.playerParachuteSkinEquipped += OnSkinEquipped;
    }

    void OnDestroy()
    {
        PostOffice.playerParachuteSkinBought -= OnSkinBought;
        PostOffice.playerParachuteSkinEquipped -= OnSkinEquipped;
    }

    void OnSkinBought(PlayerParachuteSkin skin)
    {
        if (this.skin.itemName == skin.itemName)
        {
            lockedIcon.SetActive(false);
        }
    }

    void OnSkinEquipped(PlayerParachuteSkin skin)
    {
        equippedIcon.SetActive(this.skin.itemName == skin.itemName);
    }

    public void Refresh(PlayerParachuteSkin skin)
    {
        GameObject parachute = Instantiate(Resources.Load(skin.parachute, typeof(GameObject)) as GameObject);
        this.skin = skin;
        skinName.text = skin.itemName;
        
        parachute.transform.parent = spritePivot;
        parachute.transform.ResetLocal();

        Utils.ReplaceSpritesWithUiImages(gameObject, false);

        spritePivot.transform.localScale = Vector3.one * skin.wardrobeScale;
        spritePivot.transform.Translate(skin.wardrobePivotOffset);


        if (GameSettings.PlayerParachuteSkin == skin.itemName)
        {
            lockedIcon.SetActive(false);
            equippedIcon.SetActive(true);
        }
        else if (!PersistentData.IsHavingParachuteSkin(skin.itemName))
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
