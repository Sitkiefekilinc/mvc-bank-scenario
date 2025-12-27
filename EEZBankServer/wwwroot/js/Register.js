$(document).ready(function () {
    console.log("EEZ Bank Kayıt Sistemi Yeni Standartlara Göre Hazır.");

    // 1. FORM SUBMIT İŞLEMİ (AJAX)
    $('#registerForm').on('submit', function (e) {
        e.preventDefault();

        // Merkezi 'Yükleniyor' uyarısını çağır
        EEZ.ShowLoading();

        var formData = $(this).serialize();

        $.ajax({
            url: '/Account/Register',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    // Merkezi başarılı bildirimi ve yönlendirme
                    Swal.fire({
                        title: 'Başarılı!',
                        text: response.message,
                        icon: 'success',
                        confirmButtonColor: 'var(--eez-primary)' // Değişkenimizi kullandık
                    }).then(() => {
                        window.location.href = '/Account/Login';
                    });
                } else {
                    // Merkezi hata bildirimi
                    EEZ.Notify('error', response.message);
                }
            },
            error: function () {
                EEZ.Notify('error', 'Sistem Hatası: Şu an işleminizi gerçekleştiremiyoruz.');
            }
        });
    });

    // 2. KULLANICI TİPİNE GÖRE BÖLÜMLERİ GÖSTER/GİZLE
    // Sayfa ilk yüklendiğinde mevcut seçime göre tetikle
    toggleUserSections($('#userTypeSelect').val());

    // Seçim değiştiğinde tetikle
    $('#userTypeSelect').change(function () {
        toggleUserSections($(this).val());
    });

    function toggleUserSections(value) {
        // Mevcut özel alanları kapat
        $('#kurumsalSection, #ticariSection').slideUp(300);

        if (value == "1") { // Kurumsal
            $('#kurumsalSection').delay(300).slideDown(400);
            disableInputs('#ticariSection');
            enableInputs('#kurumsalSection');
        }
        else if (value == "2") { // Ticari
            $('#ticariSection').delay(300).slideDown(400);
            disableInputs('#kurumsalSection');
            enableInputs('#ticariSection');
        }
        else { // Bireysel (0)
            disableInputs('#kurumsalSection');
            disableInputs('#ticariSection');
        }
    }

    // 3. INPUT YÖNETİMİ (Güvenlik için disabled yapma)
    function disableInputs(sectionId) {
        // Yeni 'eez-' sınıflarına sahip elemanları bulur ve devredışı bırakır
        $(sectionId).find('.eez-input, .eez-select, textarea').prop('disabled', true);
    }

    function enableInputs(sectionId) {
        // Yeni 'eez-' sınıflarına sahip elemanları bulur ve aktif eder
        $(sectionId).find('.eez-input, .eez-select, textarea').prop('disabled', false);
    }
});