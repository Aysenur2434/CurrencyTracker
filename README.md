# CurrencyTracker – Döviz Takip Konsol Uygulaması

## Proje Açıklaması
CurrencyTracker, Frankfurter FREE API kullanarak Türk Lirası (TRY) bazlı güncel döviz kurlarını
alan, bu verileri hafızada tutan ve LINQ sorguları ile analiz eden bir C# konsol uygulamasıdır.

Uygulama, bir finans firmasının temel döviz takip ihtiyacını karşılamak amacıyla geliştirilmiştir.

---

## Kullanılan Teknolojiler
- C# Konsol Uygulaması
- HttpClient
- async / await
- System.Text.Json
- List<Currency>
- LINQ (Where, Select, OrderBy, OrderByDescending, Count, Max, Min, Average)

---

## Kullanılan API
Zorunlu API:
https://api.frankfurter.app/latest?from=TRY

Bu API üzerinden TRY bazlı döviz kurları çekilmiştir.

---

## Uygulama Özellikleri

1. Tüm Dövizleri Listele  
   - Hafızada tutulan tüm dövizler listelenir  
   - LINQ Select kullanılmıştır  

2. Koda Göre Döviz Ara  
   - Kullanıcının girdiği döviz koduna göre arama yapılır  
   - Büyük/küçük harf duyarsızdır  
   - LINQ Where kullanılmıştır  

3. Belirli Bir Değerden Büyük Dövizler  
   - Kullanıcının girdiği eşik değerden büyük kurlar listelenir  
   - LINQ Where kullanılmıştır  

4. Dövizleri Değere Göre Sırala  
   - Dövizler artan veya azalan şekilde sıralanır  
   - LINQ OrderBy ve OrderByDescending kullanılmıştır  

5. İstatistiksel Özet  
   - Toplam döviz sayısı  
   - En yüksek kur  
   - En düşük kur  
   - Ortalama kur  
   - LINQ Count, Max, Min, Average kullanılmıştır  

---

## Kurulum ve Çalıştırma
1. Projeyi klonlayın:
git clone https://github.com/Aysenur2434/CurrencyTracker.git

2. Proje klasörüne girin:
cd CurrencyTracker

3. Uygulamayı çalıştırın:
dotnet run

---

## Notlar
- Hard-coded veri kullanılmamıştır.
- Veriler API üzerinden dinamik olarak alınmaktadır.
- Veriler uygulama çalıştığı sürece hafızada tutulmaktadır.
- GUI kullanılmamış, yalnızca konsol uygulaması geliştirilmiştir.
