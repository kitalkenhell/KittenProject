using UnityEngine;
using System.Collections;

public class WardrobeSkinSetter : MonoBehaviour
{
    public PlayerItems skins;

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
    public Transform parachutePivot;

    PlayerBodySkin currentBodySkin;
    PlayerHatSkin currentHatSkin;
    PlayerParachuteSkin currentParachuteSkin;

    void Start()
    {
        ResetSkin();
    }

    public void ResetSkin()
    {
        currentBodySkin = skins.GetEquipedBodySkin();
        currentHatSkin = skins.GetEquipedHatSkin();
        currentParachuteSkin = skins.GetEquipedParachuteSkin();

        Refresh();
    }

    public void SetSkin(PlayerBodySkin skin)
    {
        currentBodySkin = skin;
        Refresh();
    }

    public void SetSkin(PlayerHatSkin skin)
    {
        currentHatSkin = skin;
        Refresh();
    }

    public void SetSkin(PlayerParachuteSkin skin)
    {
        currentParachuteSkin = skin;
        Refresh();
    }

    void Refresh()
    {
        RemoveBodySkin();
        RemoveHatSkin();
        RemoveParachuteSkin();

        SetBodySkin(currentBodySkin, currentHatSkin);
        SetHatSkin(currentHatSkin);
        SetParachuteSkin(currentParachuteSkin);

        Utils.ReplaceSpritesWithUiImages(gameObject, true);
    }

    void SetBodySkin(PlayerBodySkin skin, PlayerHatSkin hatSkin)
    {
        InstantiateSkinPart(skin.body, bodyPivot);
        InstantiateSkinPart(skin.ear, earPivot, !hatSkin.disableEar);
        InstantiateSkinPart(skin.eyeBig, eyeBigPivot);
        InstantiateSkinPart(skin.eyeSmall, eyeSmallPivot);
        InstantiateSkinPart(skin.legFront, legFrontPivot);
        InstantiateSkinPart(skin.legBack, legBackPivot);
        InstantiateSkinPart(skin.mouth, mouthPivot);
        InstantiateSkinPart(skin.tongue, tonguePivot, !hatSkin.disableTongue);
        InstantiateSkinPart(skin.nose, nosePivot, !hatSkin.disableNose);
        InstantiateSkinPart(skin.tail, tailPivot);
    }

    void SetHatSkin(PlayerHatSkin skin)
    {
        InstantiateSkinPart(skin.hat, hatPivot);
    }

    void SetParachuteSkin(PlayerParachuteSkin skin)
    {
        InstantiateSkinPart(skin.parachute, parachutePivot);
    }

    public void RemoveBodySkin()
    {
        bodyPivot.DestroyAllChildren();
        earPivot.DestroyAllChildren();
        eyeBigPivot.DestroyAllChildren();
        eyeSmallPivot.DestroyAllChildren();
        legFrontPivot.DestroyAllChildren();
        legBackPivot.DestroyAllChildren();
        mouthPivot.DestroyAllChildren();
        tonguePivot.DestroyAllChildren();
        nosePivot.DestroyAllChildren();
        tailPivot.DestroyAllChildren();
    }

    public void RemoveHatSkin()
    {
        hatPivot.DestroyAllChildren();
    }

    public void RemoveParachuteSkin()
    {
        parachutePivot.DestroyAllChildren();
    }

    void InstantiateSkinPart(string prefabName, Transform pivot, bool isActive = true)
    {
        if (isActive)
        {
            GameObject skinPart = Instantiate(Resources.Load(prefabName, typeof(GameObject)) as GameObject);
            PlayerSkinPartDisabler partDisabler = skinPart.GetComponent<PlayerSkinPartDisabler>();

            skinPart.transform.parent = pivot;
            skinPart.transform.ResetLocal();

            if (partDisabler != null && partDisabler.enableOnlyInGame != null)
            {
                for (int i = 0; i < partDisabler.enableOnlyInGame.Length; i++)
                {
                    partDisabler.enableOnlyInGame[i].SetActive(false);
                }
            }
        }
    }
}

