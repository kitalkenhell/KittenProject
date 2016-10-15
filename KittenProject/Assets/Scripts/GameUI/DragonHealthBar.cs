using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonHealthBar : MonoBehaviour
{
    public Dragon dragon;
    public RectTransform hpBar;
    public float updateSmoothTime;

    float hpBarMaxWidth;
    float dragonMaxHp;
    float currentHpBarWidth;
    float lastDragonHp;
    float hpBarSpeed;

    Animator animator;

    int showAnimHash = Animator.StringToHash("Show");
    int hitAnimHash = Animator.StringToHash("Hit");

    void Start()
    {
        animator = GetComponent<Animator>();

        hpBarMaxWidth = currentHpBarWidth = hpBar.GetWidth();
        dragonMaxHp = lastDragonHp = dragon.hp;
        hpBarSpeed = 0;
    }

    void Update()
    {
        currentHpBarWidth = Mathf.SmoothDamp(currentHpBarWidth, hpBarMaxWidth * (dragon.hp / dragonMaxHp), ref hpBarSpeed, updateSmoothTime);
        hpBar.SetWidth(currentHpBarWidth);

        if (lastDragonHp != dragon.hp)
        {
            lastDragonHp = dragon.hp;
            animator.SetTrigger(hitAnimHash);
        }

        if (dragon.hp <= 0)
        {
            animator.SetTrigger(showAnimHash);
            enabled = false;
        }
    }

    public void Show()
    {
        animator.SetTrigger(showAnimHash);
    }
}
