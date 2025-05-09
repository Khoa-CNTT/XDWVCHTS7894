var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/blog/getall'
        },
        "columns": [
            { data: 'tilte', "width": "20%" },
            { data: 'content', "width": "35%" },
            { data: 'ngayTao', "width": "15%" },
            { data: 'typeBlog.name', "width": "10%" },
            {
                data: 'id_Blog',
                render: function (data) {
                    return `
                        <div class=" text-center" role="group">
                            <a href="/Admin/Blog/Upsert?id=${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onclick=Delete('/Admin/Blog/Delete/${data}') class="btn btn-danger text-white" style="cursor:pointer; width:120px">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>
                    `;
                }, "width": "20%"
            }
        ]
    });
};
function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                },
            });
        }
    });
};