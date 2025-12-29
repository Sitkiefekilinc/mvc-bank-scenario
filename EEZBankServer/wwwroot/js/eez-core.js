const EEZ = {
    Notify: function (type, message) {
        Swal.fire({
            icon: type,
            text: message,
            confirmButtonColor: '#1e3a8a',
            timer: 3000
        });
    },
    ShowLoading: function () {
        Swal.fire({
            title: 'Lütfen Bekleyin...',
            text: 'İşleminiz gerçekleştiriliyor.',
            allowOutsideClick: false,
            didOpen: () => { Swal.showLoading(); }
        });
    },

    CopyIban: function (iban) {
        navigator.clipboard.writeText(iban).then(() => {
            Swal.fire({
                toast: true,
                position: 'top-end',
                icon: 'success',
                title: 'IBAN Kopyalandı',
                showConfirmButton: false,
                timer: 2000
            });
        });
    }
};

function copyIban(ibanText) {
    EEZ.CopyIban(ibanText);
}

$(document).ready(function () {
    // 1. Mobil Menü Toggle
    const $navToggle = $('#navToggle');
    const $navMenu = $('#navMenu');

    if ($navToggle.length) {
        $navToggle.on('click', function () {
            $navMenu.toggleClass('active');
            $(this).toggleClass('active');
        });
    }

    // 2. Sayfa Dışına Tıklayınca Menüyü Kapat
    $(document).on('click', function (event) {
        if (!$navMenu.is(event.target) && $navMenu.has(event.target).length === 0 &&
            !$navToggle.is(event.target) && $navMenu.hasClass('active')) {
            $navMenu.removeClass('active');
            $navToggle.removeClass('active');
        }
    });

    // 3. Navbar Scroll Efekti (Standardize edildi)
    $(window).on('scroll', function () {
        const $navbar = $('.eez-navbar'); // Yeni class ismimiz
        if ($(window).scrollTop() > 50) {
            $navbar.addClass('eez-navbar-scrolled');
        } else {
            $navbar.removeClass('eez-navbar-scrolled');
        }
    });
});

function pasteIban() {
    navigator.clipboard.readText().then(text => {
        $('#ibanInput').val(text);
    });
}