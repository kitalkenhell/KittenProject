using UnityEngine;
using System.Collections;

public class PlayerSkinSetter : MonoBehaviour
{
    public PlayerBodySkin[] bodySkins;
    public PlayerHatSkin[] hatSkins;
    public PlayerParachuteSkin[] parachuteSkins;

    public SpriteRenderer body;
    public SpriteRenderer ear;
    public SpriteRenderer eyeBig;
    public SpriteRenderer eyeSmall;
    public SpriteRenderer legFront;
    public SpriteRenderer legBack;
    public SpriteRenderer mouth;
    public SpriteRenderer tongue;
    public SpriteRenderer nose;
    public SpriteRenderer tail;
    public SpriteRenderer hat;
    public SpriteRenderer parachute;

    void Start()
    {
        PlayerBodySkin bodySkin = bodySkins[GameSettings.PlayerBodySkinIndex];
        PlayerHatSkin hatSkin = hatSkins[GameSettings.PlayerHatSkinIndex];
        PlayerParachuteSkin parachuteSkin = parachuteSkins[GameSettings.PlayerParachuteSkinIndex];

        body.sprite = Resources.Load(bodySkin.body, typeof(Sprite)) as Sprite;
        ear.sprite = Resources.Load(bodySkin.ear, typeof(Sprite)) as Sprite;
        eyeBig.sprite = Resources.Load(bodySkin.eyeBig, typeof(Sprite)) as Sprite;
        eyeSmall.sprite = Resources.Load(bodySkin.eyeSmall, typeof(Sprite)) as Sprite;
        legFront.sprite = Resources.Load(bodySkin.legBack, typeof(Sprite)) as Sprite;
        legBack.sprite = Resources.Load(bodySkin.legBack, typeof(Sprite)) as Sprite;
        mouth.sprite = Resources.Load(bodySkin.mouth, typeof(Sprite)) as Sprite;
        tongue.sprite = Resources.Load(bodySkin.tongue, typeof(Sprite)) as Sprite;
        nose.sprite = Resources.Load(bodySkin.nose, typeof(Sprite)) as Sprite;
        tail.sprite = Resources.Load(bodySkin.tail, typeof(Sprite)) as Sprite;

        hat.sprite = Resources.Load(hatSkin.hat, typeof(Sprite)) as Sprite;
        parachute.sprite = Resources.Load(parachuteSkin.parachute, typeof(Sprite)) as Sprite;
    }
}
