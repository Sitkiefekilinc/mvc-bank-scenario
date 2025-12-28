$(document).ready(function () {
    $('#createAccountForm').on('submit', function (e) {
        // 1. Sayfanın yenilenmesini engelle (Çünkü JSON bekliyoruz)
        e.preventDefault();

        var form = $(this);

        // Validasyon kontrolü (Boş alan var mı?)
        if (!form.valid()) return;

        // Loading ekranını aç
        EEZ.ShowLoading();

        // 2. Veriyi AJAX ile gönder
        $.ajax({
            type: "POST",
            url: form.attr('action'), // /Account/HesapOlustur
            data: form.serialize(),
            success: function (response) {
                // Controller'dan gelen "return Json(...)" buraya düşer

                if (response.success) {
                    // --- BAŞARILI ---
                    Swal.fire({
                        icon: 'success',
                        title: 'Harika!',
                        text: response.message, // "Vadesiz hesap başarıyla..."
                        confirmButtonColor: '#1e3a8a',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        // Kullanıcı Tamam'a basınca Ana Sayfaya gitsin
                        if (result.isConfirmed) {
                            window.location.href = '/Home/Index';
                        }
                    });
                } else {
                    // --- HATA ---
                    Swal.fire({
                        icon: 'error',
                        title: 'Hata',
                        text: response.message, // "Lütfen tüm alanları..."
                        confirmButtonColor: '#e11d48'
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Sistem Hatası',
                    text: 'Sunucu ile bağlantı kurulamadı.',
                    confirmButtonColor: '#e11d48'
                });
            }
        });
    });
});