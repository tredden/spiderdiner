using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteWindow : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text successText;
    [SerializeField]
    TMPro.TMP_Text failureText;
    [SerializeField]
    UnityEngine.UI.Button nextLevelButton;
    [SerializeField]
    UnityEngine.UI.Button mainMenuButton;

    bool success = false;

    public void Show(bool success)
    {
        this.success = success;
        successText.gameObject.SetActive(success);
        failureText.gameObject.SetActive(!success);
        nextLevelButton.gameObject.SetActive(SceneController.GetInstance().HasNextLevel());
        this.gameObject.SetActive(true);
        if (success) {
            SceneController.GetInstance().MarkLevelComplete();
        }
        // TODO: disable player controller mouse actions
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void NextLevel()
    {
        SceneController.GetInstance().LoadNextLevel();
    }

    public void MainMenu()
    {
        SceneController.GetInstance().LoadMainMenu();
    }
}
