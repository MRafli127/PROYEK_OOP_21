using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu; // Reference to the Settings Menu
    [SerializeField] private GameObject mainMenu;     // Reference to the Main Menu (Optional)

    // Automatically hide the Settings Menu when the game starts
    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // Start the game button
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Quit the application
    public void Quit()
    {
        Application.Quit();
    }
}
