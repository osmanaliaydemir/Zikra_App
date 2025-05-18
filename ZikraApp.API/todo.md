# ZikraApp.API Eksik/TODO Listesi

2. **Validation & Error Handling**
   - Global exception handling (ör. UseExceptionHandler veya custom middleware) eksik.
   - Model validation hataları için standart response yapısı yok.

3. **API Documentation**
   - Swagger açıklamaları (summary, parametre açıklamaları) eksik.
   - Response örnekleri ve hata kodları Swagger'da detaylandırılmalı.

5. **Email/Notification**
   - Email ve push notification servisleri mock, gerçek entegrasyon yok.
   - SMTP/Firebase/OneSignal gibi servislerle gerçek bildirim entegrasyonu yapılmalı.

6. **Background Jobs**
   - Zikir hatırlatma servisi sadece saat/dakika bazlı, haftalık/gün seçimi desteği yok.
   - Kullanıcıya özel gün/saat seçimi ve esnek tekrar ayarları eklenmeli.

7. **Logging & Monitoring**
   - Serilog temel kurulu, ancak request/response loglama, hata logları ve performans metrikleri için ek middleware eklenebilir.

8. **Rate Limiting & Security**
   - API rate limiting, CORS, XSS/CSRF korumaları eklenmeli.

9. **Deployment/CI-CD**
   - Otomatik test, build ve deploy için GitHub Actions veya başka bir CI/CD pipeline'ı eklenmeli.

10. **Diğer Eksikler**
    - Kullanıcı şifre sıfırlama, email doğrulama gibi ek endpointler.
    - Kullanıcıya özel zikir geçmişi ve istatistik endpointleri.