using UnityEngine;
using System.Collections;

public class PlayerSkinSetter : MonoBehaviour
{
    public PlayerBodySkin[] bodySkins;
    public PlayerHatSkin[] hatSkins;
    public PlayerParachuteSkin[] parachuteSkins;

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

    void Start()
    {
        PlayerBodySkin bodySkin = bodySkins[GameSettings.PlayerBodySkinIndex];
        PlayerHatSkin hatSkin = hatSkins[GameSettings.PlayerHatSkinIndex];
        PlayerParachuteSkin parachuteSkin = parachuteSkins[GameSettings.PlayerParachuteSkinIndex];

        InstantiateSkinPart(bodySkin.body, bodyPivot);
        InstantiateSkinPart(bodySkin.ear, earPivot);
        InstantiateSkinPart(bodySkin.eyeBig, eyeBigPivot);
        InstantiateSkinPart(bodySkin.eyeSmall, eyeSmallPivot);
        InstantiateSkinPart(bodySkin.legFront, legFrontPivot);
        InstantiateSkinPart(bodySkin.legBack, legBackPivot);
        InstantiateSkinPart(bodySkin.mouth, mouthPivot);
        InstantiateSkinPart(bodySkin.tongue, tonguePivot);
        InstantiateSkinPart(bodySkin.nose, nosePivot);
        InstantiateSkinPart(bodySkin.tail, tailPivot);
        InstantiateSkinPart(hatSkin.hat, hatPivot);
        InstantiateSkinPart(parachuteSkin.parachute, parachutePivot);
    }

    void InstantiateSkinPart(string prefabName, Transform pivot)
    {
        GameObject skinPart = Instantiate(Resources.Load(prefabName, typeof(GameObject)) as GameObject);
        skinPart.transform.parent = pivot;
        skinPart.transform.ResetLocal();
    }
}

