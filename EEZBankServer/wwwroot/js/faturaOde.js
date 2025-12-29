
$(document).ready(function () {

    // 1. Hesap Bakiyesi Gösterimi
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

    // 2. Form Gönderimi (AJAX)
    $('#billForm').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var tutar = $('#billAmount').val();
        var kurum = $('select[name="KurumAdi"]').val();

        Swal.fire({
            title: 'Ödeme Onayı',
            html: `<p><b>${kurum}</b> kurumuna ait<br/><b>${tutar} TL</b> tutarındaki faturayı ödemek istiyor musunuz?</p>`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Evet, Öde',
            cancelButtonText: 'İptal',
            confirmButtonColor: '#be123c'
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
                                title: 'Ödeme Başarılı',
                                text: response.message,
                                confirmButtonText: 'Tamam',
                                confirmButtonColor: '#be123c'
                            }).then(() => {
                                window.location.href = '/Transfer/Index';
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
                        Swal.fire('Hata', 'Sunucu hatası.', 'error');
                    }
                });
            }
        });
    });
});
function simulateQuery() {
    var aboneNo = $('input[name="AboneNo"]').val();
    if (aboneNo.length < 5) {
        Swal.fire('Uyarı', 'Lütfen geçerli bir abone no giriniz.', 'info');
        return;
    }

    EEZ.ShowLoading();
    setTimeout(() => {
        var randomAmount = (Math.random() * (900 - 100) + 100).toFixed(2);

        $('#billAmount').val(randomAmount);

        Swal.fire({
            icon: 'info',
            title: 'Fatura Bulundu',
            text: 'Son ödeme tarihi yaklaşan ' + randomAmount + ' TL borcunuz bulunmaktadır.',
            timer: 2000,
            showConfirmButton: false
        });
    }, 1000); 
}