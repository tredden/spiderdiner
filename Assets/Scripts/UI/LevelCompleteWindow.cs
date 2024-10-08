using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteWindow : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Image background;
    [SerializeField]
    Color successColor;
    [SerializeField]
    Color failColor;
    [SerializeField]
    TMPro.TMP_Text successText;
    [SerializeField]
    TMPro.TMP_Text failureText;
    [SerializeField]
    TMPro.TMP_Text lastLevelText;
    [SerializeField]
    UnityEngine.UI.Button nextLevelButton;
    [SerializeField]
    UnityEngine.UI.Button retryLevelButton;
    [SerializeField]
    UnityEngine.UI.Button mainMenuButton;

    bool success = false;

    public void Show(bool success)
    {
        this.success = success;
        bool lastLevel = !SceneController.GetInstance().HasNextLevel();

        background.color = success ? successColor : failColor;
        successText.gameObject.SetActive(success && !lastLevel);
        lastLevelText.gameObject.SetActive(success && lastLevel);
        failureText.gameObject.SetActive(!success);
        nextLevelButton.gameObject.SetActive(success && !lastLevel);
        retryLevelButton.gameObject.SetActive(!success);

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

    public void RetryLevel()
    {
        SceneController.GetInstance().RetryLevel();
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
