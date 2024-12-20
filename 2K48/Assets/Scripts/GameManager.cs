using System.Collections;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TileBoard board;
    public CanvasGroup gameOver;

    [SerializeField] private float fadeDuration = 0.5f; // Configurable fade duration

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // Changed from DestroyImmediate
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        // Reset score (if applicable)
        // Hide game over screen
        gameOver.alpha = 0f;
        gameOver.interactable = false;

        // Update board state
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    public void GameOver()
    {
        board.enabled = false;
        gameOver.interactable = true;

        StartCoroutine(Fade(gameOver, 1f, fadeDuration));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float from = canvasGroup.alpha;

        while (elapsed < fadeDuration) // Use configurable duration
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}
