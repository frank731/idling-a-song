using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsScreen;
    public Animator transitionAnimator;

    public void OpenSettings()
    {
        if (settingsScreen.activeInHierarchy) settingsScreen.SetActive(false);
        else settingsScreen.SetActive(true);
    }

    public void OnCancel()
    {
        OpenSettings();
    }

    public void PlayGame()
    {
        transitionAnimator.enabled = true;
        Invoke("ChangeScene", 1f);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
    public void SetFullscreen(bool isFull)
    {
        if (isFull) Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else Screen.fullScreenMode = FullScreenMode.Windowed;
    }
}
