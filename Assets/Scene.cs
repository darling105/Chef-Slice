using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void StartGame()
    {
        SoundManager.Instance.PlayKnifeChopSFX();
        int lastPlayedLevel = PlayerPrefs.GetInt("LastPlayedLevel", 1);
        SceneManager.LoadScene(lastPlayedLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
