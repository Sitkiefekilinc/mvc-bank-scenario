$(document).ready(function () {
    $('#tblUsers').DataTable({
        "processing": true,
        "serverSide": true,
        "responsive": true,
        "order": [[6, "desc"]],
        "ajax": {
            "url": "/Admin/GetKullaniciListesi",
            "type": "POST",
            "datatype": "json"
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.6/i18n/tr.json"
        },
        "columns": [
            { "data": "adSoyad", "name": "UserName" },
            { "data": "email", "name": "UserEmail" },
            { "data": "telefon", "name": "UserPhoneNumber" },
            { "data": "tcNo", "name": "TcKimlikNo"},
            {
                "data": "tip", "name": "UserType",
                "render": function (data) {
                    var color = "secondary";
                    if (data === "Kurumsal") color = "primary";
                    if (data === "Admin") color = "danger";
                    return `<span class="badge bg-${color}">${data}</span>`;
                }
            },
            {
                "data": "durum", "name": "IsActive",
                "render": function (data) {
                    if (data)
                        return '<span class="badge bg-success bg-opacity-10 text-success"><i class="fas fa-check-circle"></i> Aktif</span>';
                    else
                        return '<span class="badge bg-danger bg-opacity-10 text-danger"><i class="fas fa-ban"></i> Pasif</span>';
                }
            },
            { "data": "tarih", "name": "UserCreatedAt" },
            {
                "data": "id",
                "orderable": false,
                "className": "text-end",
                "render": function (data) {
                    return `
                                <div class="btn-group">
                                    <button onclick="deleteUser('${data}')" class="btn btn-sm btn-outline-danger" title="Sil"><i class="fas fa-trash"></i></button>
                                </div>
                            `;
                }
            }
        ]
    });
});

function deleteUser(id) {
    Swal.fire({
        title: 'Kullanıcıyı Silmek İstiyor musunuz?',
        text: "Bu işlem geri alınamaz! Eğer kullanıcının hesap hareketleri varsa silinemez.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',      
        cancelButtonColor: '#3085d6',    
        confirmButtonText: 'Evet, Sil!',
        cancelButtonText: 'Vazgeç'
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                type: "POST",
                url: "/Admin/KullaniciSil",
                data: { id: id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire(
                            'Silindi!',
                            response.message,
                            'success'
                        );

             
                        $('#tblUsers').DataTable().ajax.reload();
                    } else {
                        Swal.fire(
                            'Hata!',
                            response.message,
                            'error'
                        );
                    }
                },
                error: function () {
                    Swal.fire('Hata', 'Sunucu ile iletişim kurulamadı.', 'error');
                }
            });
        }
    });
}