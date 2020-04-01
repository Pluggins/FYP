$(document).ready(function () {
    $('#orderList').DataTable({
        'order': [[1, "desc"]],
        "columnDefs": [
            { "width": "200px", "targets": 0 },
            { "width": "100px", "targets": 1 },
            { "width": "50px", "targets": 2 }
        ],
    });
});