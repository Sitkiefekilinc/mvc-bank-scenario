$(document).ready(function () {
    $('#tblHistory').DataTable({
        "processing": true,
        "serverSide": true,
        "responsive": true,
        "order": [[0, "desc"]],
        "ajax": {
            "url": "/Transfer/GetIslemGecmisi",
            "type": "POST",
            "datatype": "json"
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.6/i18n/tr.json"
        },
        "columns": [
            {
                "data": "tarih",
                "name": "IslemTarihi",
                "render": function (data) {
                    var parts = data.split(' ');
                    return `<div class="d-flex flex-column">
                                        <span class="fw-bold text-dark">${parts[0]}</span>
                                        <span class="text-muted small">${parts[1]}</span>
                                    </div>`;
                }
            },
            {
                "data": "karsiTaraf",
                "name": "KarsiTaraf",
                "orderable": false,
                "render": function (data, type, row) {
                    var icon = row.tur === "Fatura" ? '<i class="fas fa-file-invoice me-2 text-secondary"></i>' : '<i class="fas fa-user me-2 text-primary"></i>';
                    return `<div class="d-flex align-items-center">
                                        ${icon} <span class="fw-bold text-dark">${data}</span>
                                    </div>`;
                }
            },
            {
                "data": "aciklama",
                "name": "Aciklama",
                "orderable": false
            },
            {
                "data": "tutar",
                "name": "IslemMiktari",
                "className": "text-end",
                "render": function (data, type, row) {
                    if (row.yon === 'out') {
                        return `<span class="fw-bold text-danger" style="font-family:'Consolas', monospace; font-size:1.1rem;">- ${data} ₺</span>`;
                    } else {
                        return `<span class="fw-bold text-success" style="font-family:'Consolas', monospace; font-size:1.1rem;">+ ${data} ₺</span>`;
                    }
                }
            }
        ]
    });
});