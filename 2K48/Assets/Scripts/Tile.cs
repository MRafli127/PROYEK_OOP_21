using System.Collections; // Library untuk IEnumerator dan fungsi korutin
using TMPro; // Library untuk penggunaan TextMeshPro
using UnityEngine; // Library utama untuk pengembangan game dengan Unity
using UnityEngine.UI; // Library untuk penggunaan komponen UI seperti Image

public class Tile : MonoBehaviour
{
    public TileState state { get; private set; } // Properti untuk menyimpan status tile (angka, warna, dll.)
    public TileCell cell { get; private set; } // Properti untuk menyimpan referensi ke sel tempat tile berada
    public bool locked { get; set; } // Properti untuk menentukan apakah tile terkunci (tidak dapat bergerak)

    private Image background; // Referensi ke komponen background (Image) dari tile
    private TextMeshProUGUI text; // Referensi ke komponen teks pada tile

    private void Awake()
    {
        // Mendapatkan referensi ke komponen UI pada tile
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Mengatur status (angka, warna, teks) tile
    public void SetState(TileState state)
    {
        this.state = state; // Menyimpan status baru

        // Mengatur warna background dan teks berdasarkan status
        background.color = state.backgroundColor;
        text.color = state.textColor;

        // Mengatur teks angka sesuai dengan nomor dalam status
        text.text = state.number.ToString();
    }

    // Memunculkan tile baru di papan
    public void Spawn(TileCell cell)
    {
        if (this.cell != null) // Jika tile sudah berada di sel tertentu
        {
            this.cell.tile = null; // Lepaskan tile dari sel sebelumnya
        }

        this.cell = cell; // Tetapkan tile ke sel baru
        this.cell.tile = this; // Tetapkan tile ini ke sel baru

        // Pindahkan posisi tile ke posisi sel baru
        transform.position = cell.transform.position;
    }

    // Menggerakkan tile ke sel lain
    public void MoveTo(TileCell cell)
    {
        if (this.cell != null) // Jika tile sudah berada di sel tertentu
        {
            this.cell.tile = null; // Lepaskan tile dari sel sebelumnya
        }

        this.cell = cell; // Tetapkan tile ke sel baru
        this.cell.tile = this; // Tetapkan tile ini ke sel baru

        // Jalankan animasi pergerakan ke posisi sel baru
        StartCoroutine(Animate(cell.transform.position, false));
    }

    // Menggabungkan tile dengan tile lain di sel tujuan
    public void Merge(TileCell cell)
    {
        if (this.cell != null) // Jika tile sudah berada di sel tertentu
        {
            this.cell.tile = null; // Lepaskan tile dari sel sebelumnya
        }

        this.cell = null; // Hapus referensi ke sel saat ini
        cell.tile.locked = true; // Kunci tile di sel tujuan

        // Jalankan animasi penggabungan ke posisi sel tujuan
        StartCoroutine(Animate(cell.transform.position, true));
    }

    // Fungsi animasi untuk pergerakan tile
    private IEnumerator Animate(Vector3 to, bool merging)
    {
        float elapsed = 0f; // Waktu yang sudah berlalu selama animasi
        float duration = 0.1f; // Durasi animasi

        Vector3 from = transform.position; // Posisi awal tile

        // Menginterpolasi posisi tile dari awal ke tujuan selama durasi animasi
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration); // Lerp (linear interpolation) untuk pergerakan
            elapsed += Time.deltaTime; // Tambahkan waktu yang berlalu
            yield return null; // Tunggu frame berikutnya
        }

        transform.position = to; // Tetapkan posisi akhir tile

        // Jika tile sedang digabungkan
        if (merging)
        {
            Destroy(gameObject); // Hancurkan tile setelah penggabungan
        }
    }
}
