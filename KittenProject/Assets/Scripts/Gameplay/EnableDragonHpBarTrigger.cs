using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDragonHpBarTrigger : MonoBehaviour
{
    public DragonHealthBar dragonHealthBar;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        dragonHealthBar.Show();
        Destroy(gameObject);
    }
}
