$(document).ready(function () {
    $('#btnSeedData').on('click', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Örnek Veri Eklensin mi?',
            text: "Sisteme rastgele Bireysel, Kurumsal ve Ticari kullanıcılar eklenecek.",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: 'var(--eez-primary)',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, Verileri Üret',
            cancelButtonText: 'Vazgeç'
        }).then((result) => {
            if (result.isConfirmed) {
                EEZ.ShowLoading();

                fetch('/Admin/FakeVeriUret', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            Swal.fire('Başarılı!', data.message, 'success').then(() => {
                                location.reload();
                            });
                        } else {
                            EEZ.Notify('error', 'Veritabanı hatası: ' + data.message);
                        }
                    })
                    .catch(error => {
                        EEZ.Notify('error', 'Sunucuya ulaşılamadı.');
                    });
            }
        });
    });
});