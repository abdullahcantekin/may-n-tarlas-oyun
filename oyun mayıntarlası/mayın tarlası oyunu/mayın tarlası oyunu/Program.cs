using System;

class Program
{
    static void Main(string[] args)
    {
        // Oyun tahtası boyutları
        int rows = 20; // Satır sayısı
        int cols = 20; // Sütun sayısı
        int mines = 40; // Mayın sayısı

        // Oyun tahtası ve oyuncuya gösterilen tahtayı tanımla
        char[,] board = new char[rows, cols]; // Gerçek oyun tahtası
        char[,] displayBoard = new char[rows, cols]; // Oyuncunun gördüğü tahta
        Random random = new Random(); // Rastgele mayın yerleştirme için rastgele sayı üreteci

        // Tahtaları başlat
        InitializeBoard(board, displayBoard, rows, cols, mines, random);

        bool gameOver = false; // Oyun durumu

        // Oyun döngüsü
        while (!gameOver)
        {
            // Oyuncuya tahtayı göster
            PrintBoard(displayBoard, rows, cols);

            // Oyuncudan satır ve sütun seçimi al
            Console.Write("Satır girin (0-{0}): ", rows - 1);
            int row = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Sütun girin (0-{0}): ", cols - 1);
            int col = int.Parse(Console.ReadLine() ?? "0");

            // Eğer seçilen hücrede mayın varsa, oyun biter
            if (board[row, col] == '*')
            {
                Console.WriteLine("Mayına bastınız! Oyun bitti.");
                gameOver = true;

                // Tüm tahtayı açarak oyunu sonlandır
                RevealBoard(board, displayBoard, rows, cols);
            }
            else
            {
                // Eğer mayına basılmadıysa, hücreyi aç
                RevealCell(board, displayBoard, row, col, rows, cols);

                // Kazanma durumu kontrolü
                if (CheckWin(displayBoard, rows, cols, mines))
                {
                    Console.WriteLine("Tüm mayınları başarıyla buldunuz! Tebrikler!");
                    gameOver = true;
                }
            }
        }
    }

    // Tahtayı başlatma fonksiyonu
    static void InitializeBoard(char[,] board, char[,] displayBoard, int rows, int cols, int mines, Random random)
    {
        // Tüm hücreleri sıfırla
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                board[i, j] = '0'; // Başlangıçta tüm hücreler 0
                displayBoard[i, j] = '#'; // Oyuncuya gizli hücreler #
            }
        }

        // Mayınları rastgele yerleştir
        for (int i = 0; i < mines; i++)
        {
            int r, c;
            do
            {
                r = random.Next(rows); // Rastgele bir satır seç
                c = random.Next(cols); // Rastgele bir sütun seç
            } while (board[r, c] == '*'); // Eğer burada zaten mayın varsa, başka yer seç

            board[r, c] = '*'; // Mayını yerleştir

            // Çevresindeki hücreleri güncelle
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    int nr = r + dr, nc = c + dc;
                    if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && board[nr, nc] != '*')
                    {
                        board[nr, nc]++; // Mayının çevresindeki hücrelerin değerini artır
                    }
                }
            }
        }
    }

    // Oyuncuya tahtayı yazdırma fonksiyonu
    static void PrintBoard(char[,] board, int rows, int cols)
    {
        Console.Clear(); // Konsolu temizle

        // Sütun başlıklarını yazdır
        Console.Write("   ");
        for (int i = 0; i < cols; i++)
        {
            Console.Write(i % 10 + " "); // Çok geniş başlıklar için sadece son rakamları göster
        }
        Console.WriteLine();

        // Satırları ve hücreleri yazdır
        for (int i = 0; i < rows; i++)
        {
            Console.Write(i % 10 + "  "); // Çok geniş başlıklar için sadece son rakamları göster
            for (int j = 0; j < cols; j++)
            {
                Console.Write(board[i, j] + " "); // Hücre değerini yazdır
            }
            Console.WriteLine();
        }
    }

    // Hücreyi açma fonksiyonu
    static void RevealCell(char[,] board, char[,] displayBoard, int row, int col, int rows, int cols)
    {
        // Geçersiz ya da zaten açılmış hücreler için çık
        if (row < 0 || row >= rows || col < 0 || col >= cols || displayBoard[row, col] != '#')
            return;

        displayBoard[row, col] = board[row, col]; // Hücreyi aç

        // Eğer hücre boşsa, çevresindeki hücreleri de aç
        if (board[row, col] == '0')
        {
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    RevealCell(board, displayBoard, row + dr, col + dc, rows, cols);
                }
            }
        }
    }

    // Tüm tahtayı açma fonksiyonu (oyun bittiğinde)
    static void RevealBoard(char[,] board, char[,] displayBoard, int rows, int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                displayBoard[i, j] = board[i, j]; // Gerçek tahtayı oyuncuya göster
            }
        }
        PrintBoard(displayBoard, rows, cols); // Tam tahtayı yazdır
    }

    // Kazanma kontrolü
    static bool CheckWin(char[,] displayBoard, int rows, int cols, int mines)
    {
        int coveredCells = 0; // Kapatılmış hücrelerin sayısı
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (displayBoard[i, j] == '#') // Kapatılmış hücreleri say
                {
                    coveredCells++;
                }
            }
        }
        return coveredCells == mines; // Kapatılmış hücreler mayın sayısına eşitse, oyun kazanıldı
    }
}
