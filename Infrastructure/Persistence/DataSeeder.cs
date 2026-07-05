using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class DataSeeder
{
    // Demo kullanıcı bilgileri — sunumda kullanabilirsiniz
    private const string DemoEmail    = "demo@ticketora.com";
    private const string DemoPassword = "Demo123";
    private const string DemoFullName = "Ege Yılmaz";

    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context     = scope.ServiceProvider.GetRequiredService<TicketoraContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        // ── 0. Migrate ──────────────────────────────────────────────────
        await context.Database.MigrateAsync();

        // ── 1. Kategoriler ──────────────────────────────────────────────
        await SeedCategoriesAsync(context);

        // ── 2. Aktif etkinlikler (mevcut listeye ek olarak, yoksa ekle) ─
        await SeedUpcomingEventsAsync(context);

        // ── 3. Geçmiş etkinlikler ───────────────────────────────────────
        await SeedPastEventsAsync(context);

        // ── 4. Demo kullanıcı ───────────────────────────────────────────
        var demoUser = await SeedDemoUserAsync(userManager);

        // ── 5. Geçmiş etkinliklere ait registration + ticket ───────────
        await SeedPastRegistrationsAsync(context, demoUser.Id);
    }

    // ────────────────────────────────────────────────────────────────────
    private static async Task SeedCategoriesAsync(TicketoraContext context)
    {
        if (await context.Categories.AnyAsync()) return;

        var categories = new List<Category>
        {
            new() { CategoryName = "Müzik",      CreatedAt = DateTime.UtcNow },
            new() { CategoryName = "Teknoloji",  CreatedAt = DateTime.UtcNow },
            new() { CategoryName = "Sanat",      CreatedAt = DateTime.UtcNow },
            new() { CategoryName = "Spor",       CreatedAt = DateTime.UtcNow },
            new() { CategoryName = "Eğitim",     CreatedAt = DateTime.UtcNow },
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    // ────────────────────────────────────────────────────────────────────
    private static async Task SeedUpcomingEventsAsync(TicketoraContext context)
    {
        // Gelecek etkinlik zaten varsa atla
        if (await context.Events.AnyAsync(e => e.Date > DateTime.UtcNow)) return;

        var musicCatId = await GetCategoryIdAsync(context, "Müzik");
        var techCatId  = await GetCategoryIdAsync(context, "Teknoloji");
        var artCatId   = await GetCategoryIdAsync(context, "Sanat");
        var sportCatId = await GetCategoryIdAsync(context, "Spor");

        var upcoming = new List<Event>
        {
            new()
            {
                CategoryId  = musicCatId,
                Title       = "İstanbul Jazz Festival 2026",
                HeroImage   = "https://images.unsplash.com/photo-1511192336575-5a79af67a629?auto=format&fit=crop&w=1200&q=80",
                Organizer   = "İKSV",
                Description = "Dünyanın dört bir yanından ünlü caz sanatçıları İstanbul'da buluşuyor. Açık hava konserleri ve atölyelerle dolu 10 günlük müzik şöleni.",
                Badges      = "Öne Çıkan,Açık Hava",
                Date        = DateTime.UtcNow.AddDays(20),
                TimeRange   = "19:00 – 23:00",
                Location    = "Harbiye Açık Hava Tiyatrosu, İstanbul",
                Price       = 350m,
                IsFeatured  = true,
                CreatedAt   = DateTime.UtcNow,
            },
            new()
            {
                CategoryId  = techCatId,
                Title       = "TechSummit Ankara 2026",
                HeroImage   = "https://images.unsplash.com/photo-1540575467063-178a50c2df87?auto=format&fit=crop&w=1200&q=80",
                Organizer   = "TechTR Derneği",
                Description = "Yapay zeka, bulut bilişim ve siber güvenlik alanlarında 80+ konuşmacıyla Türkiye'nin en büyük teknoloji zirvesi.",
                Badges      = "Konferans,Networking",
                Date        = DateTime.UtcNow.AddDays(35),
                TimeRange   = "09:00 – 18:00",
                Location    = "Congresium, Ankara",
                Price       = 499m,
                IsFeatured  = true,
                CreatedAt   = DateTime.UtcNow,
            },
            new()
            {
                CategoryId  = artCatId,
                Title       = "Çağdaş Sanat Bienali",
                HeroImage   = "https://images.unsplash.com/photo-1460661419201-fd4cecdf8a8b?auto=format&fit=crop&w=1200&q=80",
                Organizer   = "İstanbul Modern",
                Description = "40 ülkeden 200'den fazla sanatçının eserlerini bir arada sunan uluslararası çağdaş sanat bienali.",
                Badges      = "Sergi,Uluslararası",
                Date        = DateTime.UtcNow.AddDays(45),
                TimeRange   = "10:00 – 20:00",
                Location    = "İstanbul Modern, Karaköy",
                Price       = 150m,
                IsFeatured  = false,
                CreatedAt   = DateTime.UtcNow,
            },
            new()
            {
                CategoryId  = sportCatId,
                Title       = "Vodafone İstanbul Maratonu",
                HeroImage   = "https://images.unsplash.com/photo-1561214115-f2f134cc4912?auto=format&fit=crop&w=1200&q=80",
                Organizer   = "Atletizm Federasyonu",
                Description = "İki kıtayı birleştiren eşsiz koşu deneyimi. 42 km tam maraton, 10 km yarı maraton ve 8 km aile koşusu kategorileri.",
                Badges      = "Spor,Açık Hava",
                Date        = DateTime.UtcNow.AddDays(60),
                TimeRange   = "07:00 – 14:00",
                Location    = "Boğaz Köprüsü Başlangıç Noktası, İstanbul",
                Price       = 250m,
                IsFeatured  = true,
                CreatedAt   = DateTime.UtcNow,
            },
        };

        await context.Events.AddRangeAsync(upcoming);
        await context.SaveChangesAsync();
    }

    // ────────────────────────────────────────────────────────────────────
    private static async Task SeedPastEventsAsync(TicketoraContext context)
    {
        // Geçmiş etkinlik zaten varsa atla
        if (await context.Events.AnyAsync(e => e.Date < DateTime.UtcNow)) return;

        var musicCatId = await GetCategoryIdAsync(context, "Müzik");
        var techCatId  = await GetCategoryIdAsync(context, "Teknoloji");
        var eduCatId   = await GetCategoryIdAsync(context, "Eğitim");

        var past = new List<Event>
        {
            new()
            {
                CategoryId  = musicCatId,
                Title       = "Rock'n Coke Festival 2025",
                HeroImage   = "https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?auto=format&fit=crop&w=1200&q=80",
                Organizer   = "Doğan Müzik",
                Description = "Türkiye'nin en büyük rock festivali. 3 gün boyunca 30'dan fazla yerli ve yabancı grup.",
                Badges      = "Festival,Rock",
                Date        = DateTime.UtcNow.AddDays(-45),
                TimeRange   = "16:00 – 02:00",
                Location    = "Hezarfen Havalimanı, İstanbul",
                Price       = 450m,
                IsFeatured  = false,
                CreatedAt   = DateTime.UtcNow.AddDays(-100),
            },
            new()
            {
                CategoryId  = techCatId,
                Title       = "Web Summit Istanbul 2025",
                HeroImage   = "https://images.unsplash.com/photo-1591115765373-5207764f72e7?auto=format&fit=crop&w=1200&q=80",
                Organizer   = "Web Summit",
                Description = "Avrupa'nın en büyük teknoloji konferansının İstanbul ayağı. Startup'lar, yatırımcılar ve sektör liderleri.",
                Badges      = "Konferans,Startup",
                Date        = DateTime.UtcNow.AddDays(-90),
                TimeRange   = "09:00 – 20:00",
                Location    = "İstanbul Kongre Merkezi, Harbiye",
                Price       = 799m,
                IsFeatured  = false,
                CreatedAt   = DateTime.UtcNow.AddDays(-150),
            },
            new()
            {
                CategoryId  = eduCatId,
                Title       = "Clean Architecture Workshop 2025",
                HeroImage   = "https://images.unsplash.com/photo-1517245386807-bb43f82c33c4?auto=format&fit=crop&w=1200&q=80",
                Organizer   = "DevTR Community",
                Description = ".NET ile Onion Architecture, CQRS ve Domain-Driven Design konularında yoğun 2 günlük atölye.",
                Badges      = "Atölye,.NET",
                Date        = DateTime.UtcNow.AddDays(-30),
                TimeRange   = "10:00 – 18:00",
                Location    = "Teknopark İstanbul, Pendik",
                Price       = 299m,
                IsFeatured  = false,
                CreatedAt   = DateTime.UtcNow.AddDays(-60),
            },
        };

        await context.Events.AddRangeAsync(past);
        await context.SaveChangesAsync();
    }

    // ────────────────────────────────────────────────────────────────────
    private static async Task<AppUser> SeedDemoUserAsync(UserManager<AppUser> userManager)
    {
        var existing = await userManager.FindByEmailAsync(DemoEmail);
        if (existing != null) return existing;

        var user = new AppUser
        {
            FullName  = DemoFullName,
            Email     = DemoEmail,
            UserName  = DemoEmail,
            CreatedAt = DateTime.UtcNow,
        };

        await userManager.CreateAsync(user, DemoPassword);

        return (await userManager.FindByEmailAsync(DemoEmail))!;
    }

    // ────────────────────────────────────────────────────────────────────
    private static async Task SeedPastRegistrationsAsync(TicketoraContext context, int userId)
    {
        // Bu kullanıcıya ait registration zaten varsa tekrar ekleme
        if (await context.EventRegistrations.AnyAsync(r => r.UserId == userId)) return;

        var pastEvents = await context.Events
            .Where(e => !e.IsDeleted && e.Date < DateTime.UtcNow)
            .OrderBy(e => e.Date)
            .ToListAsync();

        if (!pastEvents.Any()) return;

        foreach (var evt in pastEvents)
        {
            var registration = new EventRegistration
            {
                UserId     = userId,
                EventId    = evt.Id,
                TotalPrice = evt.Price,
                CreatedAt  = evt.Date.AddDays(-7),
            };

            await context.EventRegistrations.AddAsync(registration);
            await context.SaveChangesAsync();

            var ticket = new Ticket
            {
                EventRegistrationId = registration.Id,
                PassengerFullName   = DemoFullName,
                PassengerEmail      = DemoEmail,
                SerialNo            = GenerateSerial(),
                Seat                = GenerateSeat(),
                CreatedAt           = evt.Date.AddDays(-7),
            };

            await context.Tickets.AddAsync(ticket);
            await context.SaveChangesAsync();
        }
    }

    // ────────────────────────────────────────────────────────────────────
    private static async Task<int> GetCategoryIdAsync(TicketoraContext context, string name)
    {
        var cat = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName == name);
        return cat?.Id ?? 1;
    }

    private static string GenerateSerial()
    {
        var rng = Random.Shared;
        return $"TKT-{rng.Next(1000, 9999)}-{rng.Next(100000, 999999)}";
    }

    private static string GenerateSeat()
    {
        var rows  = new[] { "A", "B", "C", "D", "E" };
        var row   = rows[Random.Shared.Next(rows.Length)];
        var num   = Random.Shared.Next(1, 50);
        return $"{row}{num}";
    }
}
