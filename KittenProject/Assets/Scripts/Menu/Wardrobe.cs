using UnityEngine;
using UnityEngine.UI;
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

    public PlayerItems items;

    public GameObject parachutesContent;
    public GameObject outfitsContent;
    public GameObject hatsContent;
    public GameObject upgradesContent;

    public GameObject equipButton;
    public GameObject buyButton;
    public Text priceLabel;

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
    PlayerUpgrade lastUpgradeSelected = null;

    void Awake()
    {
        showAnimHash = Animator.StringToHash("Show");

        StartCoroutine(LoadData());
    }

    IEnumerator LoadData()
    {
        foreach (var skin in items.parachuteSkins)
        {
            ParachuteSkinSelectionButton button = (Instantiate(parachuteButtonPrefab) as GameObject).GetComponent<ParachuteSkinSelectionButton>();

            button.Refresh(skin);
            button.transform.SetParent(parachutesContent.transform);
            button.transform.localScale = Vector3.one;
            button.onButtonPressed = OnParachuteSelected;
            yield return null;
        }

        foreach (var skin in items.hatSkins)
        {
            HatSkinSelectionButton button = (Instantiate(hatButtonPrefab) as GameObject).GetComponent<HatSkinSelectionButton>();

            button.Refresh(skin);
            button.transform.SetParent(hatsContent.transform);
            button.transform.localScale = Vector3.one;
            button.onButtonPressed = OnHatSelected;
            yield return null;
        }

        foreach (var skin in items.bodySkins)
        {
            OutfitSkinSelectionButton button = (Instantiate(outfitButtonPrefab) as GameObject).GetComponent<OutfitSkinSelectionButton>();

            button.Refresh(skin);
            button.transform.SetParent(outfitsContent.transform);
            button.transform.localScale = Vector3.one;
            button.onButtonPressed = OnOutfitSelected;
            yield return null;
        }

        foreach (var upgrade in items.upgrades)
        {
            UpgradeSelectionButton button = (Instantiate(upgradesButtonPrefab) as GameObject).GetComponent<UpgradeSelectionButton>();

            button.Refresh(upgrade);
            button.transform.SetParent(upgradesContent.transform);
            button.transform.localScale = Vector3.one;
            button.onButtonPressed = OnUpgradeSelected;
            yield return null;
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
        dogePreview.ResetSkin();
    }

    void ResetSelection()
    {
        lastOutfitSkinSelected = null;
        lastParachuteSkinSelected = null;
        lastHatSkinSelected = null;
        lastUpgradeSelected = null;

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

    bool BuyItem(PlayerItem skin)
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

        if (currentView == Views.upgrades || !hasItem)
        {
            equipButton.SetActive(false);
        }
        else
        {
            equipButton.SetActive(!isEquiped);
        }
    }

    void OnParachuteSelected(PlayerParachuteSkin skin)
    {
        lastOutfitSkinSelected = null;
        lastParachuteSkinSelected = skin;
        lastHatSkinSelected = null;
        lastUpgradeSelected = null;
        priceLabel.text = skin.price.ToString();

        RefreshButton(PersistentData.IsHavingParachuteSkin(skin.itemName), GameSettings.PlayerParachuteSkin == skin.itemName);
        dogePreview.SetSkin(skin);
    }

    void OnOutfitSelected(PlayerBodySkin skin)
    {
        lastOutfitSkinSelected = skin;
        lastParachuteSkinSelected = null;
        lastHatSkinSelected = null;
        lastUpgradeSelected = null;
        priceLabel.text = skin.price.ToString();

        RefreshButton(PersistentData.IsHavingBodySkin(skin.itemName), GameSettings.PlayerBodySkin == skin.itemName);
        dogePreview.SetSkin(skin);
    }

    void OnHatSelected(PlayerHatSkin skin)
    {
        lastOutfitSkinSelected = null;
        lastParachuteSkinSelected = null;
        lastHatSkinSelected = skin;
        lastUpgradeSelected = null;
        priceLabel.text = skin.price.ToString();

        RefreshButton(PersistentData.IsHavingHatSkin(skin.itemName), GameSettings.PlayerHatSkin == skin.itemName);
        dogePreview.SetSkin(skin);
    }

    void OnUpgradeSelected(PlayerUpgrade upgrade)
    {
        lastOutfitSkinSelected = null;
        lastParachuteSkinSelected = null;
        lastHatSkinSelected = null;
        lastUpgradeSelected = upgrade;
        priceLabel.text = upgrade.price.ToString();

        RefreshButton(PersistentData.IsHavingUpgrade(upgrade.itemName), false);
    }

    public void OnParachutesButtonPressed()
    {
        if (currentView != Views.parachutes)
        {
            RefreshView(Views.parachutes); 
        }
    }

    public void OnOutfitsButtonPressed()
    {
        if (currentView != Views.outfits)
        {
            RefreshView(Views.outfits); 
        }
    }

    public void OnHatsButtonPressed()
    {
        if (currentView != Views.hats)
        {
            RefreshView(Views.hats); 
        }
    }

    public void OnUpgradesButtonPressed()
    {
        if (currentView != Views.upgrades)
        {
            RefreshView(Views.upgrades); 
        }
    }

    public void OnEquipButtonPressed()
    {
        if (lastOutfitSkinSelected != null)
        {
            GameSettings.PlayerBodySkin = lastOutfitSkinSelected.itemName;
            RefreshButton(true, true);
            PostOffice.PostPlayerBodySkinEquipped(lastOutfitSkinSelected);
        }
        else if (lastParachuteSkinSelected != null)
        {
            GameSettings.PlayerParachuteSkin = lastParachuteSkinSelected.itemName;
            RefreshButton(true, true);
            PostOffice.PostPlayerParachuteSkinEquipped(lastParachuteSkinSelected);
        }
        else if (lastHatSkinSelected != null)
        {
            GameSettings.PlayerHatSkin = lastHatSkinSelected.itemName;
            RefreshButton(true, true);
            PostOffice.PostPlayerHatSkinEquipped(lastHatSkinSelected);
        }
    }

    public void OnBuyButtonPressed()
    {
        if (lastOutfitSkinSelected != null && BuyItem(lastOutfitSkinSelected))
        {
            AnalyticsManager.OnOutfitSkinBought(lastOutfitSkinSelected.itemName);
            PersistentData.IsHavingBodySkin(lastOutfitSkinSelected.itemName, true);
            PostOffice.PostPlayerBodySkinBought(lastOutfitSkinSelected);
            RefreshButton(true, false);
        }
        else if (lastParachuteSkinSelected != null && BuyItem(lastParachuteSkinSelected))
        {
            AnalyticsManager.OnParachuteSkinBought(lastParachuteSkinSelected.itemName);
            PersistentData.IsHavingParachuteSkin(lastParachuteSkinSelected.itemName, true);
            PostOffice.PostPlayerParachuteSkinBought(lastParachuteSkinSelected);
            RefreshButton(true, false);
        }
        else if (lastHatSkinSelected != null && BuyItem(lastHatSkinSelected))
        {
            AnalyticsManager.OnHatSkinBought(lastHatSkinSelected.itemName);
            SocialManager.IncrementAchievement(SocialManager.Achievements.hatsUnlocked);
            PersistentData.IsHavingHatSkin(lastHatSkinSelected.itemName, true);
            PostOffice.PostPlayerHatSkinBought(lastHatSkinSelected);
            RefreshButton(true, false);
        }
        else if (lastUpgradeSelected != null && BuyItem(lastUpgradeSelected))
        {
            AnalyticsManager.OnUpgradeBought(lastUpgradeSelected.itemName);
            PersistentData.IsHavingUpgrade(lastUpgradeSelected.itemName, true);
            lastUpgradeSelected.OnBought();
            PostOffice.PostPlayerUpgradeBought(lastUpgradeSelected);
            lastUpgradeSelected = null;
            RefreshButton(true, false);
        }
    }
}
