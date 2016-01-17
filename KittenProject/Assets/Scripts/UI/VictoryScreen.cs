using UnityEngine;
using System.Collections;

public class VictoryScreen : MonoBehaviour
{
    bool locked = false;

    public void RestartButton()
    {
        if (!locked)
        {
            PostOffice.PostRestartLevel();
            locked = true;
        }
    }
}

