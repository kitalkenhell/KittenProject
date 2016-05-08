using UnityEngine;
using System.Collections;

public class Wardrobe : MonoBehaviour
{
    enum Views
    {
        parachutes,
        outfits,
        hats,
        upgrades
    }

    public PlayerSkins skins;

    public GameObject parachutesContent;
    public GameObject outfitsContent;
    public GameObject hatsContent;
    public GameObject upgradesContent;

    public GameObject equipButton;
    public GameObject buyButton;

    public Animator parachutesPanel;
    public Animator outfitsPanel;
    public Animator hatsPanel;
    public Animator upgradesPanel;

    public GameObject parachuteButtonPrefab;
    public GameObject outfitButtonPrefab;
    public GameObject hatButtonPrefab;
    public GameObject upgradesButtonPrefab;

    public WardrobeSkinSetter dogePreview;

    int showAnimHash;

    Views currentView;
    PlayerBodySkin lastOutfitSkinSelected = null;
    PlayerParachuteSkin lastParachuteSkinSelected = null;
    PlayerHatSkin lastHatSkinSelected = null;

    void Awake()
    {
        showAnimHash = Animator.StringToHash("Show");

        foreach (var skin in skins.parachuteSkins)
        {
            ParachuteSkinSelectionButton button = (Instantiate(parachuteButtonPrefab) as GameObject).GetComponent<ParachuteSkinSelectionButton>();

            button.Refresh(skin);
            button.transform.SetParent(parachutesContent.transform);
            button.transform.localScale = Vector3.one;
            button.onButtonPressed = OnParachuteSelected;
        }

        foreach (var skin in skins.hatSkins)
        {
            HatSkinSelectionButton button = (Instantiate(hatButtonPrefab) as GameObject).GetComponent<HatSkinSelectionButton>();

            button.Refresh(skin);
            button.transform.SetParent(hatsContent.transform);
            button.transform.localScale = Vector3.one;
            button.onButtonPressed = OnHatSelected;
        }

        foreach (var skin in skins.bodySkins)
        {
            OutfitSkinSelectionButton button = (Instantiate(outfitButtonPrefab) as GameObject).GetComponent<OutfitSkinSelectionButton>();

            button.Refresh(skin);
            button.transform.SetParent(outfitsContent.transform);
            button.transform.localScale = Vector3.one;
            button.onButtonPressed = OnOutfitSelected;
        }
    }

    void OnEnable()
    {
        currentView = Views.hats;
        SetAnimationOfCurrentView(true);
        ResetSelection();
    }

    void RefreshView(Views newView)
    {
        if (newView == currentView)
        {
            return;
        }

        SetAnimationOfCurrentView(false);
        currentView = newView;
        SetAnimationOfCurrentView(true);
        ResetSelection();
    }

    void ResetSelection()
    {
        lastOutfitSkinSelected = null;
        lastParachuteSkinSelected = null;
        lastHatSkinSelected = null;

        buyButton.SetActive(false);
        equipButton.SetActive(false);
    }

    void SetAnimationOfCurrentView(bool show)
    {
        if (currentView == Views.parachutes)
        {
            parachutesPanel.SetBool(showAnimHash, show);
        }
        else if (currentView == Views.outfits)
        {
            outfitsPanel.SetBool(showAnimHash, show);
        }
        else if (currentView == Views.hats)
        {
            hatsPanel.SetBool(showAnimHash, show);
        }
        else if (currentView == Views.upgrades)
        {
            upgradesPanel.SetBool(showAnimHash, show);
        }
    }

    bool BuySkin(PlayerSkin skin)
    {
        if (PersistentData.Coins > skin.price)
        {
            PersistentData.Coins -= skin.price;
            return true;
        }

        return false;
    }

    void RefreshButton(bool hasItem, bool isEquiped)
    {
        buyButton.SetActive(!hasItem);

        if (hasItem)
        {
            equipButton.SetActive(!isEquiped);
        }
        else
        {
            equipButton.SetActive(false);
        }
    }

    void OnParachuteSelected(PlayerParachuteSkin skin)
    {
        lastOutfitSkinSelected = null;
        lastParachuteSkinSelected = skin;
        lastHatSkinSelected = null;

        RefreshButton(PersistentData.IsHavingParachuteSkin(skin.skinName), GameSettings.PlayerParachuteSkin == skin.skinName);
        dogePreview.SetSkin(skin);
    }

    void OnOutfitSelected(PlayerBodySkin skin)
    {
        lastOutfitSkinSelected = skin;
        lastParachuteSkinSelected = null;
        lastHatSkinSelected = null;


        RefreshButton(PersistentData.IsHavingBodySkin(skin.skinName), GameSettings.PlayerBodySkin == skin.skinName);
        dogePreview.SetSkin(skin);
    }

    void OnHatSelected(PlayerHatSkin skin)
    {
        lastOutfitSkinSelected = null;
        lastParachuteSkinSelected = null;
        lastHatSkinSelected = skin;

        RefreshButton(PersistentData.IsHavingHatSkin(skin.skinName), GameSettings.PlayerHatSkin == skin.skinName);
        dogePreview.SetSkin(skin);
    }

    public void OnParachutesButtonPressed()
    {
        RefreshView(Views.parachutes);
    }

    public void OnOutfitsButtonPressed()
    {
        RefreshView(Views.outfits);
    }

    public void OnHatsButtonPressed()
    {
        RefreshView(Views.hats);
    }

    public void OnUpgradesButtonPressed()
    {
        RefreshView(Views.upgrades);
    }

    public void OnEquipButtonPressed()
    {
        if (lastOutfitSkinSelected != null)
        {
            GameSettings.PlayerBodySkin = lastOutfitSkinSelected.skinName;
            RefreshButton(true, true);
        }
        else if (lastParachuteSkinSelected != null)
        {
            GameSettings.PlayerParachuteSkin = lastParachuteSkinSelected.skinName;
            RefreshButton(true, true);
        }
        else if (lastHatSkinSelected != null)
        {
            GameSettings.PlayerHatSkin = lastHatSkinSelected.skinName;
            RefreshButton(true, true);
        }
    }

    public void OnBuyButtonPressed()
    {
        if (lastOutfitSkinSelected != null)
        {
            if (BuySkin(lastOutfitSkinSelected))
            {
                PersistentData.IsHavingBodySkin(lastOutfitSkinSelected.skinName, true);
                RefreshButton(true, false);
            }
        }
        else if (lastParachuteSkinSelected != null)
        {
            if (BuySkin(lastParachuteSkinSelected))
            {
                PersistentData.IsHavingParachuteSkin(lastParachuteSkinSelected.skinName, true);
                RefreshButton(true, false); 
            }
        }
        else if (lastHatSkinSelected != null)
        {
            if (BuySkin(lastHatSkinSelected))
            {
                PersistentData.IsHavingHatSkin(lastHatSkinSelected.skinName, true);
                RefreshButton(true, false); 
            }
        }
    }
}
