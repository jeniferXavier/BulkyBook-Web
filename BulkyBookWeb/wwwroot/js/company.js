$(document).ready(function () {
    loadDataTable();
});
var dataTable;
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/company/getall' },
        "columns": [
            { data: 'name', "width": "25%" },
            { data: 'streetAddress', "width": "15%" },
            { data: 'city', "width": "20%" },
            { data: 'state', "width": "10%" },
            { data: 'postalCode', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                         <a href="/admin/company/upsert?id=${data}" class="btn btn-primary"><i class="bi bi-pencil-square"></i> &nbsp; Edit</a>
                         &nbsp;&nbsp;
                         <a onClick=deleteCompany("/admin/company/delete/${data}") class="btn btn-danger"><i class="bi bi-trash3"></i> &nbsp; Delete</a>
                     </div>`
                },
                "width": "15%",

            }
        ]
    });
}

function deleteCompany(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}