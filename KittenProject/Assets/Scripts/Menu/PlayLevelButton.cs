using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayLevelButton : MonoBehaviour 
{
    public string sceneName;

	public void OnClicked() 
	{
        SceneManager.LoadScene(sceneName);
    }
}
