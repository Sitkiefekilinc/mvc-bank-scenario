$(document).ready(function () {

    // 1. Hesap Seçilince Bakiyeyi Göster
    $('#accountSelector').on('change', function () {
        var selectedOption = $(this).find('option:selected');
        var balance = selectedOption.data('balance');

        if (balance !== undefined) {
            var formatted = parseFloat(balance.toString().replace(',', '.')).toLocaleString('tr-TR', { minimumFractionDigits: 2 });
            $('#currentBalanceText').text(formatted + " TL");
            $('#currentBalanceDiv').removeClass('d-none');
        } else {
            $('#currentBalanceDiv').addClass('d-none');
        }
    });

    // 2. IBAN Formatlama (Görsel olarak boşluk koyma)
    $('#ibanInput').on('input', function () {
        var value = $(this).val().replace(/\s+/g, '').toUpperCase();
        // Basit bir gruplama mantığı (TR12 3456 ...)
        // Tam maskeleme için 'InputMask' kütüphanesi kullanılabilir, burada manuel yapıyoruz:
        if (!value.startsWith('TR')) {
            // Kullanıcı TR yazmadıysa biz ekleyebiliriz veya uyarı veririz
        }
    });

    // 3. Form Gönderimi (AJAX)
    $('#eftForm').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);

        // Validasyon kontrolü (Boş alan var mı?)
        // Not: HTML5 'required' attribute'u zaten basit kontrolü yapar.

        Swal.fire({
            title: 'Onaylıyor musunuz?',
            html: `<b>${$('#amountInput').val()} TL</b> tutarındaki transfer işlemi gerçekleştirilecektir.`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Evet, Gönder',
            cancelButtonText: 'İptal',
            confirmButtonColor: '#2563eb'
        }).then((result) => {
            if (result.isConfirmed) {
                EEZ.ShowLoading();

                $.ajax({
                    type: "POST",
                    url: form.attr('action'),
                    data: form.serialize(),
                    success: function (response) {
                        if (response.success) {
                            Swal.fire({
                                icon: 'success',
                                title: 'İşlem Başarılı',
                                text: response.message,
                                confirmButtonText: 'Tamam',
                                confirmButtonColor: '#2563eb'
                            }).then(() => {
                                window.location.href = '/Transfer/Index'; // Merkeze dön
                            });
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Hata',
                                text: response.message,
                                confirmButtonColor: '#ef4444'
                            });
                        }
                    },
                    error: function () {
                        Swal.fire('Hata', 'Sunucu ile iletişim kurulamadı.', 'error');
                    }
                });
            }
        });
    });
});

function pasteIban() {
    navigator.clipboard.readText().then(text => {
        $('#ibanInput').val(text);
    });
}