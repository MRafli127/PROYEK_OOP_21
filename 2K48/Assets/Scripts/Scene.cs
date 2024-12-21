using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu; // Reference to the Settings Menu
    [SerializeField] private GameObject mainMenu;     // Reference to the Main Menu (Optional)

    // Automatically hide the Settings Menu when the game starts
    private void Awake()
    {
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(false); // Hide the Settings Menu
        }

        if (mainMenu != null)
        {
            mainMenu.SetActive(true); // Ensure the Main Menu is visible
        }
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
