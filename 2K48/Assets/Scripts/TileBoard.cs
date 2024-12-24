using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class untuk mengelola logika papan permainan
public class TileBoard : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab; // Prefab untuk tile yang akan diinstansiasi
    [SerializeField] private TileState[] tileStates; // Array untuk menyimpan berbagai state tile

    private TileGrid grid; // Referensi ke grid tempat tile ditempatkan
    private List<Tile> tiles; // List untuk menyimpan semua tile aktif di papan
    private bool waiting; // Flag untuk mencegah input selama animasi berlangsung

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>(); // Mendapatkan referensi ke grid di anak objek
        tiles = new List<Tile>(16); // Menginisialisasi list tiles dengan kapasitas awal 16
    }

    // Membersihkan papan untuk memulai permainan baru
    public void ClearBoard()
    {
        foreach (var cell in grid.cells) { // Iterasi melalui setiap sel di grid
            cell.tile = null; // Mengosongkan referensi tile di sel
        }

        foreach (var tile in tiles) { // Iterasi melalui semua tile yang ada
            Destroy(tile.gameObject); // Menghancurkan objek tile
        }

        tiles.Clear(); // Menghapus semua elemen dari list tiles
    }

    // Membuat tile baru di papan
    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform); // Instansiasi tile baru
        tile.SetState(tileStates[0]); // Mengatur state awal tile
        tile.Spawn(grid.GetRandomEmptyCell()); // Menempatkan tile di sel acak yang kosong
        tiles.Add(tile); // Menambahkan tile ke list tiles
    }

    // Memproses input untuk pergerakan
    private void Update()
    {
        if (waiting) return; // Mengabaikan input jika sedang menunggu animasi selesai

        // Cek tombol input untuk masing-masing arah dan panggil fungsi Move
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            Move(Vector2Int.up, 0, 1, 1, 1); // Gerakan ke atas
        } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            Move(Vector2Int.left, 1, 1, 0, 1); // Gerakan ke kiri
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            Move(Vector2Int.down, 0, 1, grid.Height - 2, -1); // Gerakan ke bawah
        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            Move(Vector2Int.right, grid.Width - 2, -1, 0, 1); // Gerakan ke kanan
        }
    }

    // Menggerakkan tile sesuai arah yang diberikan
    private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false; // Menandai jika ada perubahan di papan

        // Iterasi melalui grid dalam urutan tertentu sesuai arah gerakan
        for (int x = startX; x >= 0 && x < grid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.Height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y); // Mendapatkan sel dari koordinat

                if (cell.Occupied) { // Jika sel memiliki tile
                    changed |= MoveTile(cell.tile, direction); // Coba gerakkan tile
                }
            }
        }

        if (changed) { // Jika ada perubahan, tunggu hingga animasi selesai
            StartCoroutine(WaitForChanges());
        }
    }

    // Logika untuk memindahkan satu tile
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null; // Menyimpan sel baru tempat tile akan dipindahkan
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction); // Mendapatkan sel yang bersebelahan

        // Loop untuk mencari posisi akhir tile
        while (adjacent != null)
        {
            if (adjacent.Occupied) // Jika sel bersebelahan ditempati
            {
                if (CanMerge(tile, adjacent.tile)) // Jika dapat digabungkan
                {
                    MergeTiles(tile, adjacent.tile); // Gabungkan tile
                    return true;
                }

                break; // Jika tidak bisa digabungkan, hentikan pencarian
            }

            newCell = adjacent; // Perbarui sel baru
            adjacent = grid.GetAdjacentCell(adjacent, direction); // Cek sel berikutnya
        }

        if (newCell != null) // Jika ada sel baru
        {
            tile.MoveTo(newCell); // Pindahkan tile ke sel baru
            return true;
        }

        return false; // Tidak ada pergerakan
    }

    // Mengecek apakah dua tile dapat digabungkan
    private bool CanMerge(Tile a, Tile b)
    {
        return a.state == b.state && !b.locked; // Bisa digabung jika state sama dan tile kedua tidak terkunci
    }

    // Menggabungkan dua tile
    private void MergeTiles(Tile a, Tile b)
    {
        tiles.Remove(a); // Hapus tile pertama dari list
        a.Merge(b.cell); // Gabungkan tile pertama dengan tile kedua

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1); // Hitung state baru
        TileState newState = tileStates[index]; // Ambil state baru

        b.SetState(newState); // Atur state baru pada tile kedua
        GameManager.Instance.IncreaseScore(newState.number); // Tambahkan skor
    }

    // Mendapatkan indeks sebuah state dalam array tileStates
    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++) // Iterasi melalui semua state
        {
            if (state == tileStates[i]) { // Jika ditemukan, kembalikan indeks
                return i;
            }
        }

        return -1; // Jika tidak ditemukan, kembalikan -1
    }

    // Coroutine untuk menunggu perubahan selesai
    private IEnumerator WaitForChanges()
    {
        waiting = true; // Tandai sedang menunggu

        yield return new WaitForSeconds(0.1f); // Tunggu durasi tertentu

        waiting = false; // Tandai selesai menunggu

        foreach (var tile in tiles) { // Iterasi melalui semua tile
            tile.locked = false; // Buka kunci tile
        }

        if (tiles.Count != grid.Size) { // Jika papan tidak penuh
            CreateTile(); // Tambahkan tile baru
        }

        if (CheckForGameOver()) { // Jika game over
            GameManager.Instance.GameOver(); // Panggil logika game over
        }
    }

    // Mengecek apakah permainan sudah berakhir
    public bool CheckForGameOver()
    {
        if (tiles.Count != grid.Size) { // Jika papan tidak penuh
            return false;
        }

        foreach (var tile in tiles) // Iterasi melalui semua tile
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up); // Cek tile di atas
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down); // Cek tile di bawah
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left); // Cek tile di kiri
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right); // Cek tile di kanan

            if (up != null && CanMerge(tile, up.tile)) { // Jika bisa digabung ke atas
                return false;
            }

            if (down != null && CanMerge(tile, down.tile)) { // Jika bisa digabung ke bawah
                return false;
            }

            if (left != null && CanMerge(tile, left.tile)) { // Jika bisa digabung ke kiri
                return false;
            }

            if (right != null && CanMerge(tile, right.tile)) { // Jika bisa digabung ke kanan
                return false;
            }
        }

        return true; // Tidak ada gerakan yang memungkinkan, game over
    }

}
