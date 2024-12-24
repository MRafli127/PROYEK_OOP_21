using System.Collections; // Library untuk bekerja dengan koleksi dan IEnumerator
using TMPro; // Library untuk menggunakan TextMeshPro
using UnityEngine; // Library utama untuk pengembangan game dengan Unity

[DefaultExecutionOrder(-1)] // Memastikan script ini dieksekusi sebelum script lainnya
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Properti singleton untuk akses global ke GameManager

    public TileBoard board; // Referensi ke objek papan game
    public CanvasGroup gameOver; // UI untuk tampilan game over
    public TextMeshProUGUI scoreText; // Komponen UI untuk menampilkan skor saat ini
    public TextMeshProUGUI hiscoreText; // Komponen UI untuk menampilkan skor tertinggi

    private int score; // Variabel untuk menyimpan skor pemain saat ini

    [SerializeField] private float fadeDuration = 0.5f; // Durasi efek fade untuk UI game over

    private void Awake()
    {
        if (Instance != null) // Jika Instance sudah ada
        {
            Destroy(gameObject); // Menghancurkan instance duplikat
        }
        else
        {
            Instance = this; // Menetapkan Instance ke script ini
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) // Jika script ini adalah Instance saat ini
        {
            Instance = null; // Reset Instance
        }
    }

    private void Start()
    {
        NewGame(); // Memulai permainan baru saat game dimulai
    }

    public void NewGame()
    {
        // Reset skor
        SetScore(0);

        // Memuat skor tertinggi dan menampilkannya
        hiscoreText.text = LoadHiscore().ToString();

        // Menyembunyikan layar game over
        gameOver.alpha = 0f;
        gameOver.interactable = false;

        // Membersihkan papan dan membuat dua tile awal
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true; // Mengaktifkan interaksi dengan papan

        // Mengaktifkan dan mereset timer
        FindObjectOfType<Timer>().enabled = true;
        FindObjectOfType<Timer>().ResetTimer();
    }

    public void GameOver()
    {
        board.enabled = false; // Menonaktifkan papan game
        gameOver.interactable = true; // Mengizinkan interaksi dengan UI game over

        FindObjectOfType<Timer>().enabled = false; // Menghentikan timer saat game over

        // Memulai efek fade-in pada layar game over
        StartCoroutine(Fade(gameOver, 1f, fadeDuration));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f)
    {
        yield return new WaitForSeconds(delay); // Menunggu waktu delay sebelum memulai efek

        float elapsed = 0f; // Waktu yang sudah berlalu
        float from = canvasGroup.alpha; // Nilai alpha awal

        // Menginterpolasi nilai alpha selama durasi fade
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration); // Linear interpolation
            elapsed += Time.deltaTime; // Tambahkan waktu yang berlalu
            yield return null; // Tunggu frame berikutnya
        }

        canvasGroup.alpha = to; // Pastikan alpha mencapai nilai akhir
    }

    public void IncreaseScore(int points)
    {
        SetScore(score + points); // Tambahkan poin ke skor saat ini
    }

    private void SetScore(int score)
    {
        this.score = score; // Menyimpan skor baru
        scoreText.text = score.ToString(); // Menampilkan skor di UI
        SaveHiscore(); // Menyimpan skor jika itu adalah skor tertinggi baru
    }

    private void SaveHiscore()
    {
        int hiscore = LoadHiscore(); // Memuat skor tertinggi

        if (score > hiscore) // Jika skor saat ini lebih tinggi dari skor tertinggi
        {
            PlayerPrefs.SetInt("hiscore", score); // Menyimpan skor baru sebagai skor tertinggi
        }
    }

    private int LoadHiscore()
    {
        return PlayerPrefs.GetInt("hiscore", 0); // Memuat skor tertinggi dari PlayerPrefs (default 0)
    }
}
