$('#btnEditMode').on('click', function () {
    // Textleri gizle, Inputları göster
    $('.view-mode').addClass('d-none');
    $('.edit-mode').removeClass('d-none');

    // Butonları değiştir
    $('.view-mode-btn').addClass('d-none');
    $('.edit-mode-btn').removeClass('d-none');
});

// 2. "İptal"e basınca eski haline dön
$('#btnCancel').on('click', function () {
    // Inputları gizle, Textleri göster
    $('.edit-mode').addClass('d-none');
    $('.view-mode').removeClass('d-none');

    // Butonları değiştir
    $('.edit-mode-btn').addClass('d-none');
    $('.view-mode-btn').removeClass('d-none');

    // Formu resetle (yazılanları sil, eskisine dön)
    $('#profileUpdateForm')[0].reset();
});

// 3. Form Gönderimi (Submit) ve AJAX
$('#profileUpdateForm').on('submit', function (e) {
    e.preventDefault(); // Sayfa yenilenmesini engelle

    var form = $(this);

    // SweetAlert ile Onay İste
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Profil bilgileriniz güncellenecektir.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#1e3a8a', // Senin temanın mavisi
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, Kaydet',
        cancelButtonText: 'İptal'
    }).then((result) => {
        if (result.isConfirmed) {

            // Loading göster
            EEZ.ShowLoading();

            // AJAX İsteği
            $.ajax({
                type: "POST",
                url: form.attr('action'), // /Account/UpdateProfile
                data: form.serialize(),   // Input verilerini paketle
                success: function (response) {

                    if (response.success) {
                        // Başarılı Mesajı
                        Swal.fire({
                            icon: 'success',
                            title: 'Başarılı!',
                            text: response.message,
                            confirmButtonColor: '#1e3a8a'
                        }).then(() => {
                            // Sayfayı yenile ki yeni bilgiler text olarak gelsin
                            location.reload();
                        });
                    } else {
                        // Backend'den gelen hata (Örn: Geçersiz email)
                        Swal.fire({
                            icon: 'error',
                            title: 'Hata',
                            text: response.message,
                            confirmButtonColor: '#e11d48'
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Sunucu Hatası',
                        text: 'İşlem sırasında bir hata oluştu.',
                        confirmButtonColor: '#e11d48'
                    });
                }
            });
        }
    });
});