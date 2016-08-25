using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkinSetter : MonoBehaviour
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

    List<GameObject> allSkinParts;

    void Start()
    {
        allSkinParts = new List<GameObject>();
        SetSkin();
    }

    void InstantiateSkinPart(string prefabName, Transform pivot, bool isActive = true)
    {
        if (isActive)
        {
            GameObject skinPart = Instantiate(Resources.Load(prefabName, typeof(GameObject)) as GameObject);
            allSkinParts.Add(skinPart);
            skinPart.transform.parent = pivot;
            skinPart.transform.ResetLocal();
        }
    }

    public void ResetSkin()
    {
        foreach (var part in allSkinParts)
        {
            Destroy(part);
        }

        allSkinParts.Clear();
        SetSkin();
    }

    void SetSkin()
    {
        PlayerBodySkin bodySkin = skins.GetEquipedBodySkin();
        PlayerHatSkin hatSkin = skins.GetEquipedHatSkin();
        PlayerParachuteSkin parachuteSkin = skins.GetEquipedParachuteSkin();

        InstantiateSkinPart(bodySkin.body, bodyPivot);
        InstantiateSkinPart(bodySkin.ear, earPivot, !hatSkin.disableEar);
        InstantiateSkinPart(bodySkin.eyeBig, eyeBigPivot);
        InstantiateSkinPart(bodySkin.eyeSmall, eyeSmallPivot);
        InstantiateSkinPart(bodySkin.legFront, legFrontPivot);
        InstantiateSkinPart(bodySkin.legBack, legBackPivot);
        InstantiateSkinPart(bodySkin.mouth, mouthPivot);
        InstantiateSkinPart(bodySkin.tongue, tonguePivot, !hatSkin.disableTongue);
        InstantiateSkinPart(bodySkin.nose, nosePivot, !hatSkin.disableNose);
        InstantiateSkinPart(bodySkin.tail, tailPivot);
        InstantiateSkinPart(hatSkin.hat, hatPivot);
        InstantiateSkinPart(parachuteSkin.parachute, parachutePivot);
    }
}

