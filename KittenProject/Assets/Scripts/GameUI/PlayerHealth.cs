using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour 
{
    public RectTransform HearthIcon;
    public Vector3 rootPosition;
    public Vector3 positionOffset;
    public float iconScale;

    HeartIcon[] icons;

    void Start () 
	{
        icons = new HeartIcon[GameSettings.maxPlayerHealth];

        for (int i = 0; i < GameSettings.maxPlayerHealth; i++)
        {
            RectTransform icon = Instantiate(HearthIcon) as RectTransform;
            icon.SetParent(transform);
            icon.transform.localPosition = rootPosition + positionOffset * i;
            icon.localScale = Vector3.one * iconScale;

            icons[i] = icon.GetComponent<HeartIcon>();
        }

        PostOffice.PlayerHealthChanged += OnPlayerHealthChanged;
    }

    void OnDestroy()
    {
        PostOffice.PlayerHealthChanged -= OnPlayerHealthChanged;
    }

    void OnPlayerHealthChanged(int oldHealth, int currentHealth)
    {
        --oldHealth;
        --currentHealth;

        if (oldHealth > currentHealth)
        {
            icons[oldHealth].Hide();
        }
        else
        {
            icons[oldHealth].Show();
        }
    }
}
