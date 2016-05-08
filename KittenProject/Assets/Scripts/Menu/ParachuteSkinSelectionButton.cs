using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ParachuteSkinSelectionButton : MonoBehaviour
{
    public Text skinName;
    public Transform spritePivot;

    public System.Action<PlayerParachuteSkin> onButtonPressed;

    PlayerParachuteSkin skin;

    public void Refresh(PlayerParachuteSkin skin)
    {
        GameObject parachute = Instantiate(Resources.Load(skin.parachute, typeof(GameObject)) as GameObject);
        this.skin = skin;
        skinName.text = skin.skinName;
        
        parachute.transform.parent = spritePivot;
        parachute.transform.ResetLocal();

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
