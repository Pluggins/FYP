$(document).ready(function () {
    $('#orderList').DataTable({
        'order': [[2, "asc"]],
        "columnDefs": [
            { "width": "200px", "targets": 0 },
            { "width": "100px", "targets": 1 },
            { "width": "50px", "targets": 2 }
        ],
    });
});