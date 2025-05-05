var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/product/getall'
        },
        "columns": [
            { data: 'nameProduct', "width": "20%" },
            { data: 'description', "width": "30%" },
            { data: 'ncc', "width": "10%" },
            { data: 'price', "width": "8%" },
            { data: 'category.name', "width": "12%" },
            {
                data: 'id_Product',
                render: function (data) {
                    return `
                        <div class=" w-75 btn-group" role="group">
                            <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-success text-white mx-2" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i>Edit
                            </a>
                            <a onclick=Delete('/Admin/Product/Delete/${data}') class="btn btn-danger text-white mx-2" style="cursor:pointer">
                                <i class="bi bi-trash-fill"></i>Delete
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
                //title: "Deleted!",
                //text: "Your file has been deleted.",
                //icon: "success"
            });
        }
    });
};