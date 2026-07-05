# Ticketora — Project 12

Bu proje, **M&Y Yazılım Eğitim Akademi Danışmanlık** bünyesinde düzenlenen ve **Murat Yücedağ** tarafından verilen **.NET Core Full Stack Bootcamp** eğitiminin **12. Case çalışması** olarak geliştirilmiştir.

Projenin temel amacı; **Onion Architecture'ın tüm kurallarına eksiksiz uyulmuş**, katmanlı sorumluluğun titizlikle korunduğu, okunabilir ve sürdürülebilir bir .NET uygulaması ortaya koymaktır. Mimari saflık bu projenin en öncelikli hedefidir.

---

## 🌐 Canlı Demo (Live Demo)

Projeyi canlı ortamda test etmek için aşağıdaki bağlantıyı ziyaret edebilirsiniz:
🔗 **[https://ticketora.egeasp.net/](https://ticketora.egeasp.net/)**

**Giriş Bilgileri:**
* **E-posta:** `demo@ticketora.com`
* **Şifre:** `Demo123`

---

## 🎯 Proje Özeti

**Ticketora**, etkinlik biletleme ve rezervasyon yönetimi üzerine kurulu tam kapsamlı bir web uygulamasıdır. Kullanıcılar kategorilere göre etkinlikleri keşfedebilir, bilet satın alabilir ve kendi panellerinden geçmiş ile aktif biletlerini takip edebilir.

---

## 🚀 Özellikler

### 🌐 Kullanıcı Arayüzü
- Kategorilere göre etkinlik listeleme
- Etkinlik detay sayfası (açıklama, organizatör, konum, fiyat, tarih/saat)
- **Book Now** akışı → Checkout sayfası → Başarı ekranı
- Rezervasyon tamamlandığında otomatik dijital bilet oluşturma

### 🎫 Dijital Bilet
Oluşturulan her biletin içeriği:
- Etkinlik adı, tarihi ve saati
- Etkinlik konumu
- Bilet sahibinin adı ve e-postası
- Koltuk bilgisi
- Benzersiz seri numarası (rastgele üretim)

### 👤 Kullanıcı Paneli
- Profil bilgilerini görüntüleme
- Ad soyad ve e-posta güncelleme
- **Şifre değiştirme** (mevcut şifre doğrulaması ile)
- Aktif biletleri görüntüleme ve bilet detayına erişim
- Geçmişte katıldığı etkinlikleri görüntüleme

### 🔐 Kimlik Doğrulama
- ASP.NET Core Identity ile Giriş (Login) ve Kayıt (Register)
- Tüm kullanıcıya özel sayfalarda `[Authorize]` koruması
- Anonim erişimde `/Auth` sayfasına yönlendirme

---

## 🏛️ Mimari: Onion Architecture

Bu projenin **en kritik başarısı**, Onion Architecture kurallarının katı biçimde uygulanmasıdır. Her katman yalnızca içteki katmana bağımlıdır; dışa doğru hiçbir bağımlılık yoktur.

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation                         │
│          Controllers · Views · ViewModels               │
├─────────────────────────────────────────────────────────┤
│                   Infrastructure                        │
│   EF Core · Identity · Repositories · ProfileService   │
├─────────────────────────────────────────────────────────┤
│                    Application                          │
│    Interfaces (IProfileService, IBookingService…)       │
│    DTOs · Application-layer Service Implementations     │
├─────────────────────────────────────────────────────────┤
│                      Domain                             │
│         Entities · BaseEntity (bağımlılık yok)          │
└─────────────────────────────────────────────────────────┘
```

### Katman Detayları

| Katman | Sorumluluk |
|---|---|
| **Domain** | `Event`, `Ticket`, `EventRegistration`, `Category` entity'leri. Hiçbir dış bağımlılık yok. |
| **Application** | `IProfileService`, `IBookingService`, `IDashboardService`, `IEventService` interface'leri. DTO'lar. Domain'e bağımlı, Infrastructure'ı tanımıyor. |
| **Infrastructure** | EF Core `TicketoraContext`, Repository implementasyonları, `ProfileService` (Identity impl.), `ServiceRegistration`. |
| **Presentation** | MVC Controller'lar, Razor View'lar, ViewModels. Yalnızca Application interface'lerini görür. |

### Mimari Kararların Öne Çıkanları

- **`IProfileService`** interface'i Application katmanında tanımlandı; Identity bağımlı implementasyonu (`ProfileService`) Infrastructure'da tutuldu. Controller asla `UserManager<AppUser>` görmez.
- **`[Authorize]`** `DashboardController` ve `BookingController` sınıf düzeyinde tanımlandı; `UserId = 0` riski sıfırlandı.
- **Cookie login path** Identity'nin default `/Account/Login` yerine projeye ait `/Auth` olarak yapılandırıldı.
- **Generic Repository Pattern** `IRepository<T>` ile kurgulandı; entity'ye özgü sorgular ayrı `IEventRepository`, `ITicketRepository` vb. interface'lerine taşındı.

---

## 🛠️ Kullanılan Teknolojiler

| Teknoloji | Kullanım Alanı |
|---|---|
| ASP.NET Core MVC (.NET 8) | Web framework |
| Entity Framework Core | ORM, Code-First migration |
| MSSQL (SQL Server) | Veritabanı |
| ASP.NET Core Identity | Kimlik doğrulama ve yetkilendirme |
| Onion Architecture | Mimari yaklaşım |
| Repository Pattern | Veri erişim soyutlaması |
| Dependency Injection | Servis yönetimi |
| Razor View Engine | Şablon motoru |
| HTML / CSS / JavaScript | UI katmanı |

---

## 📁 Proje Yapısı

```
Ticketora-Project-12/
│
├── Domain/
│   └── Entities/           → Event, Ticket, EventRegistration, Category, BaseEntity
│
├── Application/
│   ├── Abstract/
│   │   ├── Repository/     → IRepository<T>, IEventRepository, ITicketRepository…
│   │   └── Service/        → IProfileService, IBookingService, IDashboardService, IEventService
│   ├── DTOs/               → Profile, Booking, Dashboard, Events DTO'ları
│   └── Services/           → BookingService, DashboardService, EventService
│
├── Infrastructure/
│   ├── Identity/           → AppUser
│   ├── Persistence/        → TicketoraContext, Migrations, DataSeeder
│   ├── Repositories/       → Generic + özel repository implementasyonları
│   ├── Services/           → ProfileService (Identity impl.)
│   └── ServiceRegistration.cs
│
└── Presentation/
    ├── Controllers/        → AuthController, DashboardController, BookingController…
    ├── Models/ViewModels/  → Auth, Dashboard, Booking, Events ViewModel'leri
    └── Views/              → Razor sayfaları
```

---

## ⚙️ Kurulum ve Çalıştırma

### 1. Repoyu klonlayın
```bash
git clone https://github.com/nullablege/Ticketora-Project-12.git
cd Ticketora-Project-12
```

### 2. Bağlantı dizesini güncelleyin
`Presentation/appsettings.json` içindeki `DefaultConnection` değerini kendi SQL Server bilgilerinize göre ayarlayın:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=TicketoraDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### 3. Migration'ı uygulayın
```bash
dotnet ef database update --project Infrastructure --startup-project Presentation
```

### 4. Uygulamayı çalıştırın
```bash
dotnet run --project Presentation
```

> Uygulama ilk çalıştığında `DataSeeder` otomatik devreye girer ve demo kullanıcı, kategoriler, gelecek + geçmiş etkinlikler ile biletleri veritabanına ekler.

### 🐋 Docker ve Docker Compose ile Çalıştırma (Alternatif)

Projede Docker desteği bulunmaktadır. Yerel bilgisayarınızda SQL Server kurulumu veya yapılandırması ile uğraşmadan uygulamayı hızlıca ayağa kaldırabilirsiniz:

1. Projeyi klonlayın ve klasöre gidin.
2. Klasörün kök dizininde aşağıdaki komutu çalıştırın:
   ```bash
   docker compose up -d --build
   ```
3. Uygulama `http://localhost:8080` adresinden yayına girecektir. Veritabanı migrasyonları ve seed verileri ilk çalıştırmada otomatik olarak uygulanacaktır.

### 🔑 Demo Kullanıcı Bilgileri
| Alan | Değer |
|---|---|
| E-posta | `demo@ticketora.com` |
| Şifre | `Demo123` |

---

## Geliştirici

Bu proje, bootcamp sürecindeki **12. case çalışmam** olarak geliştirilmiştir.

**Ege AYTAÇ**

## Proje Görselleri : 

### Login Ekranı
<img width="1264" height="596" alt="image" src="https://github.com/user-attachments/assets/2ebd0965-7d57-4a5c-917e-e825d9923f64" />

### Anasayfa
<img width="1267" height="602" alt="image" src="https://github.com/user-attachments/assets/eec3ea00-0482-4fdc-9ace-3b6e9fad29ed" />
<img width="1264" height="599" alt="image" src="https://github.com/user-attachments/assets/5e581627-330f-4cec-ac2a-cf1b76d0303d" />
<img width="1266" height="598" alt="image" src="https://github.com/user-attachments/assets/921cb4b7-7828-4325-97d8-d0be50a85dc5" />

### Etkinlik Detay Sayfası
<img width="1258" height="591" alt="image" src="https://github.com/user-attachments/assets/aede54af-2ec7-45a9-9e28-5c1693115e74" />
<img width="1257" height="594" alt="image" src="https://github.com/user-attachments/assets/e832ebca-fb61-4969-a903-cc7bb227965f" />

### Dashboard
<img width="1261" height="594" alt="image" src="https://github.com/user-attachments/assets/d49a0eab-5ff8-4695-9646-aa299c06e499" />

### Biletlerim Sayfası
<img width="1265" height="597" alt="image" src="https://github.com/user-attachments/assets/8e9fe868-678a-43eb-9953-065fe6d9711c" />

### Ayarlar Sayfası
<img width="1263" height="594" alt="image" src="https://github.com/user-attachments/assets/11921e5a-19f6-4d03-90e8-5545fb07f319" />

### Ödeme Sayfası
<img width="1261" height="596" alt="image" src="https://github.com/user-attachments/assets/1e0862f3-4500-4de4-9da9-662efa324855" />

### Ödeme Başarılı Sayfası
<img width="1254" height="596" alt="image" src="https://github.com/user-attachments/assets/56395fbd-d82d-42f0-9286-3ae872e92c41" />

### Bilet Geçiş Kartı Detayı
<img width="1259" height="591" alt="image" src="https://github.com/user-attachments/assets/1286cfb3-cc96-4a34-8b98-9c055469d4d4" />
