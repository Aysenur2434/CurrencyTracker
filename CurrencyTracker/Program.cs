using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CurrencyTracker.Models;

namespace CurrencyTracker
{
    internal class Program
    {
        private static readonly HttpClient _http = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<Currency> currencies;
            try
            {
                currencies = await FetchCurrenciesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("API'den veri alınamadı: " + ex.Message);
                Console.WriteLine("Çıkmak için bir tuşa bas...");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                PrintMenu();

                Console.Write("Seçiminiz: ");
                var input = Console.ReadLine()?.Trim();

                switch (input)
                {
                    case "1":
                        ListAll(currencies);
                        break;
                    case "2":
                        SearchByCode(currencies);
                        break;
                    case "3":
                        ListGreaterThan(currencies);
                        break;
                    case "4":
                        SortByRate(currencies);
                        break;
                    case "5":
                        ShowStats(currencies);
                        break;
                    case "0":
                        Console.WriteLine("Çıkılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim!");
                        Pause();
                        break;
                }
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("===== CurrencyTracker =====");
            Console.WriteLine("1. Tüm dövizleri listele");
            Console.WriteLine("2. Koda göre döviz ara");
            Console.WriteLine("3. Belirli bir değerden büyük dövizleri listele");
            Console.WriteLine("4. Dövizleri değere göre sırala");
            Console.WriteLine("5. İstatistiksel özet göster");
            Console.WriteLine("0. Çıkış");
            Console.WriteLine();
        }

        // ZORUNLU API: https://api.frankfurter.app/latest?from=TRY
        private static async Task<List<Currency>> FetchCurrenciesAsync()
        {
            var url = "https://api.frankfurter.app/latest?from=TRY";

            using var resp = await _http.GetAsync(url);
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<CurrencyResponse>(json, options);
            if (data == null || data.Rates == null)
                throw new Exception("API yanıtı boş/uygunsuz.");

            // Verileri hafızada tutma: List<Currency>
            // LINQ Select (ZORUNLU 1. madde için de kullanılacak)
            var list = data.Rates
                .Select(kv => new Currency { Code = kv.Key, Rate = kv.Value })
                .ToList();

            return list;
        }

        // 1) Tüm dövizleri listele (LINQ Select)
        private static void ListAll(List<Currency> currencies)
        {
            Console.Clear();
            Console.WriteLine("=== Tüm Dövizler (Base: TRY) ===\n");

            // LINQ Select (şart)
            var lines = currencies
                .Select(c => $"{c.Code,-6} : {c.Rate}")
                .ToList();

            foreach (var line in lines)
                Console.WriteLine(line);

            Pause();
        }

        // 2) Koda göre döviz ara (LINQ Where, case-insensitive)
        private static void SearchByCode(List<Currency> currencies)
        {
            Console.Clear();
            Console.Write("Aranacak döviz kodu (örn: USD): ");
            var code = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(code))
            {
                Console.WriteLine("Kod boş olamaz.");
                Pause();
                return;
            }

            // Büyük/küçük harf duyarsız LINQ Where
            var results = currencies
                .Where(c => c.Code != null &&
                            c.Code.Equals(code, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Console.WriteLine();
            if (results.Count == 0)
            {
                Console.WriteLine("Sonuç bulunamadı.");
            }
            else
            {
                foreach (var c in results)
                    Console.WriteLine($"{c.Code,-6} : {c.Rate}");
            }

            Pause();
        }

        // 3) Belirli bir değerden büyük dövizler (LINQ Where)
        private static void ListGreaterThan(List<Currency> currencies)
        {
            Console.Clear();
            Console.Write("Eşik değer gir (örn: 0.030): ");
            var text = (Console.ReadLine() ?? "").Trim();

            if (!decimal.TryParse(text, out var threshold))
            {
                Console.WriteLine("Geçersiz sayı!");
                Pause();
                return;
            }

            // LINQ Where
            var results = currencies
                .Where(c => c.Rate > threshold)
                .OrderByDescending(c => c.Rate)
                .ToList();

            Console.WriteLine($"\nRate > {threshold} olan dövizler:\n");
            if (results.Count == 0)
            {
                Console.WriteLine("Sonuç yok.");
            }
            else
            {
                foreach (var c in results)
                    Console.WriteLine($"{c.Code,-6} : {c.Rate}");
            }

            Pause();
        }

        // 4) Dövizleri değere göre sırala (OrderBy / OrderByDescending)
        private static void SortByRate(List<Currency> currencies)
        {
            Console.Clear();
            Console.WriteLine("1) Artan (OrderBy)");
            Console.WriteLine("2) Azalan (OrderByDescending)");
            Console.Write("\nSeçim: ");
            var choice = (Console.ReadLine() ?? "").Trim();

            List<Currency> sorted;

            if (choice == "1")
                sorted = currencies.OrderBy(c => c.Rate).ToList(); // LINQ OrderBy
            else if (choice == "2")
                sorted = currencies.OrderByDescending(c => c.Rate).ToList(); // LINQ OrderByDescending
            else
            {
                Console.WriteLine("Geçersiz seçim!");
                Pause();
                return;
            }

            Console.WriteLine("\n=== Sıralı Liste ===\n");
            foreach (var c in sorted)
                Console.WriteLine($"{c.Code,-6} : {c.Rate}");

            Pause();
        }

        // 5) İstatistiksel özet (Count, Max, Min, Average)
        private static void ShowStats(List<Currency> currencies)
        {
            Console.Clear();

            // LINQ Count, Max, Min, Average (ZORUNLU)
            var count = currencies.Count(); // Count
            var maxRate = currencies.Max(c => c.Rate); // Max
            var minRate = currencies.Min(c => c.Rate); // Min
            var avgRate = currencies.Average(c => c.Rate); // Average

            var maxCurrency = currencies.First(c => c.Rate == maxRate);
            var minCurrency = currencies.First(c => c.Rate == minRate);

            Console.WriteLine("=== İstatistiksel Özet ===\n");
            Console.WriteLine($"Toplam döviz sayısı : {count}");
            Console.WriteLine($"En yüksek kur       : {maxCurrency.Code} = {maxRate}");
            Console.WriteLine($"En düşük kur        : {minCurrency.Code} = {minRate}");
            Console.WriteLine($"Ortalama kur        : {avgRate}");
            Console.WriteLine("\nNot: Base TRY'dir. Rate = 1 TRY'nin ilgili döviz cinsinden değeri.");

            Pause();
        }

        private static void Pause()
        {
            Console.WriteLine("\nDevam etmek için bir tuşa bas...");
            Console.ReadKey();
        }
    }
}
