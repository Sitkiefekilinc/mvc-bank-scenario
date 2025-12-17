document.addEventListener('DOMContentLoaded', function () {
    const navToggle = document.getElementById('navToggle');
    const navMenu = document.getElementById('navMenu');

    if (navToggle) {
        navToggle.addEventListener('click', function () {
            navMenu.classList.toggle('active');

            // Hamburger menü animasyonu
            this.classList.toggle('active');
        });
    }

    // Sayfa scroll olduğunda navbar'a shadow ekle
    window.addEventListener('scroll', function () {
        const navbar = document.querySelector('.navbar');
        if (window.scrollY > 50) {
            navbar.style.boxShadow = '0 4px 20px rgba(0, 0, 0, 0.15)';
        } else {
            navbar.style.boxShadow = '0 2px 10px rgba(0, 0, 0, 0.1)';
        }
    });

    document.addEventListener('click', function (event) {
        const isClickInsideNav = navMenu.contains(event.target);
        const isClickOnToggle = navToggle.contains(event.target);

        if (!isClickInsideNav && !isClickOnToggle && navMenu.classList.contains('active')) {
            navMenu.classList.remove('active');
            navToggle.classList.remove('active');
        }
    });
});

document.getElementById('btnSeedData').addEventListener('click', function (e) {
    e.preventDefault();

    Swal.fire({
        title: 'Örnek Veri Eklensin mi?',
        text: "Sisteme rastgele Bireysel, Kurumsal ve Ticari kullanıcılar eklenecek.",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#1e3a8a', 
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, Verileri Üret',
        cancelButtonText: 'Vazgeç'
    }).then((result) => {
        if (result.isConfirmed) {

            Swal.fire({
                title: 'İşlem Yapılıyor...',
                html: 'Bogus kütüphanesi verileri hazırlıyor, lütfen bekleyin.',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading()
                }
            });

            fetch('/Admin/FakeVeriUret', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        Swal.fire({
                            title: 'İşlem Başarılı!',
                            text: data.message,
                            icon: 'success',
                            confirmButtonColor: '#1e3a8a'
                        }).then(() => {
                            location.reload();
                        });
                    } else {
                        Swal.fire({
                            title: 'Hata!',
                            text: 'Veritabanına kayıt sırasında bir sorun oluştu: ' + data.message,
                            icon: 'error',
                            confirmButtonColor: '#1e3a8a'
                        });
                    }
                })
                .catch(error => {
                    Swal.fire('Bağlantı Hatası!', 'Sunucuya ulaşılamadı.', 'error');
                });
        }
    });
});