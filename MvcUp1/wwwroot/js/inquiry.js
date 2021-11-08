var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/inquiry/GetInquiryList",
        },
            "columns": [
                { "data": "id", "width": "20%" },
                { "data": "fullName", "width": "20%" },
                { "data": "phoneNumber", "width": "20%" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `<div class="text-center">
                                    <a href="/Inquiry/Details/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                        <i class="fas fa-edit"></i>
                                    </a>
                                </div>`;
                    }, "width": "5%"
                }
            ]
        }
    );
}