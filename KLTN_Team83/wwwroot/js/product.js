

$(document).ready(function () {
    loadDataTable();
});

//function loadDataTable() {
//    dataTable = $('#tblData').DataTable({
//        "ajax": { url:'/admin/product/getall' },
//        "columns": [
//            { data: 'nameProduct', "width": "10%" },
//            { data: 'description', "width": "10%" },
//            { data: 'ncc', "width": "10%" },
//            { data: 'price', "width": "10%" },
//            { data: 'category.name', "width": "10%" }
//        ]
//    });
//}
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
                        <div class="text-center w-75 btn-group" role="group">
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