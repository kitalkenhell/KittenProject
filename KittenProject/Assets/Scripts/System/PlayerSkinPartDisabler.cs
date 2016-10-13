using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinPartDisabler : MonoBehaviour
{
    public GameObject[] enableOnlyInGame;

    public void Enable(bool enable = true)
    {
        for (int i = 0; i < enableOnlyInGame.Length; i++)
        {
            enableOnlyInGame[i].SetActive(enable);
        }
    }
}
