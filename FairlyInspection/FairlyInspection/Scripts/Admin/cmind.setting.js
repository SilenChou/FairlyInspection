﻿var dataTableConfig = {
    "paging": true,
    "lengthChange": false,
    "searching": false,
    "ordering": true,
    "info": true,
    "autoWidth": true,
    "stateSave": true,
    "language": {
        "info": "第 _START_ 筆到 _END_ 筆，總共 _TOTAL_ 筆",
        "infoEmpty": "",
        "emptyTable": "沒有任何資料！",
        "paginate": {
            "first": "首頁",
            "next": "下一頁",
            "previous": "上一頁",
            "last": "最後一頁"
        }
    },
    "responsive": true,
    "pageLength": 10
}

$(function () {
    $('#storedata').DataTable(dataTableConfig);
});