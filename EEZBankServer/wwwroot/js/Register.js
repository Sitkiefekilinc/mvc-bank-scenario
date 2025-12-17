$('#registerForm').on('submit', function (e) {
    e.preventDefault();

  
    Swal.fire({
        title: 'Lütfen Bekleyin...',
        text: 'Bilgileriniz EEZ Bank güvenliğine kaydediliyor.',
        allowOutsideClick: false,
        didOpen: () => { Swal.showLoading(); }
    });

    var formData = $(this).serialize();

    $.ajax({
        url: '/Account/Register',
        type: 'POST',
        data: formData,
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    title: 'Başarılı!',
                    text: response.message,
                    icon: 'success',
                    confirmButtonColor: '#1e3a8a'
                }).then(() => {
                    window.location.href = '/Account/Login';
                });
            } else {
                Swal.fire('Hata!', response.message, 'error');
            }
        },
        error: function () {
            Swal.fire('Sistem Hatası', 'Şu an işleminizi gerçekleştiremiyoruz.', 'error');
        }
    });
});

$(document).ready(function () {
   
    toggleUserSections($('#userTypeSelect').val());

    $('#userTypeSelect').change(function () {
        var selectedValue = $(this).val();
        toggleUserSections(selectedValue);
    });

    function toggleUserSections(value) {
        $('#kurumsalSection, #ticariSection').slideUp(300);

        if (value == "1") {
            $('#kurumsalSection').delay(300).slideDown(400);
            disableInputs('#ticariSection');
            enableInputs('#kurumsalSection');
        }
        else if (value == "2") {
            $('#ticariSection').delay(300).slideDown(400);
            disableInputs('#kurumsalSection');
            enableInputs('#ticariSection');
        }
        else {
            disableInputs('#kurumsalSection');
            disableInputs('#ticariSection');
        }
    }

    function disableInputs(sectionId) {
        $(sectionId).find('input, select, textarea').prop('disabled', true);
    }
    function enableInputs(sectionId) {
        $(sectionId).find('input, select, textarea').prop('disabled', false);
    }
});