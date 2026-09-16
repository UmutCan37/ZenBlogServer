# ZenBlog Server

.NET ile Clean Architecture ve CQRS (MediatR) prensipleriyle geliştirilmiş blog platformu backend API'si. Angular tabanlı [ZenBlogClient](https://github.com/UmutCan37/ZenBlogClient) uygulamasına hizmet verir.

Frontend repo: [ZenBlogClient](https://github.com/UmutCan37/ZenBlogClient)

## Mimari

Proje, bağımlılıkların her zaman içeriye doğru aktığı Clean Architecture prensibiyle katmanlandırılmıştır:

```
ZenBlog.Domain          → Entity'ler, hiçbir dış bağımlılığı yok
ZenBlog.Application     → CQRS (Command/Query), MediatR handler'ları, AutoMapper profilleri, interface tanımları
ZenBlog.Persistence     → EF Core DbContext, repository implementasyonları, migration'lar
ZenBlog.API             → Minimal API endpoint tanımları, middleware, dependency injection
```

- **CQRS + MediatR:** Her işlem bir Command (yazma) ya da Query (okuma) olarak modellenmiştir, her birinin kendi handler'ı vardır
- **Generic Repository:** `IRepository<T>` ile temel CRUD operasyonları merkezi bir yerde tanımlanmıştır
- **AutoMapper:** Entity ↔ DTO dönüşümleri mapping profilleriyle yönetilir
- **Minimal API:** Controller yerine `MapGroup`/`MapGet`/`MapPost` ile endpoint tanımlama

## Özellikler / Endpoint'ler

| Modül | Endpoint | Açıklama |
|---|---|---|
| Kategoriler | `/api/categories` | CRUD |
| Bloglar | `/api/blogs` | CRUD, kategoriye göre listeleme |
| Yorumlar | `/api/comments` | CRUD |
| İletişim Bilgileri | `/api/contactInfos` | CRUD |
| Mesajlar | `/api/messages` | Oluşturma, listeleme, okundu/okunmadı filtresi, silme |
| Sosyal Ağlar | `/api/socials` | CRUD |
| Kullanıcılar | `/api/users/register`, `/api/users/login` | Kayıt ve JWT tabanlı giriş |

## Teknoloji Yığını

- **.NET** (Minimal API)
- **MediatR** — CQRS pattern implementasyonu
- **Entity Framework Core** — SQL Server ile ORM
- **AutoMapper** — Entity/DTO dönüşümleri
- **ASP.NET Core Identity** — Kullanıcı yönetimi
- **JWT (JSON Web Token)** — Stateless kimlik doğrulama

## Kimlik Doğrulama

Giriş yapan kullanıcıya, aşağıdaki claim'leri içeren bir JWT token döner:

```json
{
  "sub": "kullanıcı GUID'i",
  "email": "...",
  "fullName": "...",
  "exp": "..."
}
```

İstemci bu token'ı `Authorization: Bearer <token>` header'ı ile korumalı endpoint'lere gönderir.

## Kurulum

**1. Veritabanı bağlantısını ayarla**

`appsettings.json` içindeki connection string'i kendi SQL Server instance'ına göre düzenle:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ZenBlogDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

**2. Migration'ları uygula**

```bash
dotnet ef database update
```

**3. Projeyi çalıştır**

```bash
dotnet run
```

API varsayılan olarak `https://localhost:7000` adresinde ayakta olur.

## Proje Yapısı

```
ZenBlog.Application/Features/
├── Blogs/
│   ├── Commands/          # CreateBlogCommand, UpdateBlogCommand
│   ├── Queries/            # GetBlogsQuery, GetBlogByIdQuery
│   ├── Handlers/           # MediatR handler'ları
│   ├── Result/              # Query response DTO'ları
│   ├── Mappings/           # AutoMapper profilleri
│   └── Endpoints/          # Minimal API route tanımları
├── Categories/
├── Comments/
├── ContactInfos/
├── Messages/
├── Socials/
└── Users/
```

Her modül aynı klasör deseniyle organize edilmiştir — CQRS'in getirdiği tutarlılık sayesinde yeni bir domain eklemek, mevcut yapıyı kopyalayıp uyarlamak kadar basittir.
