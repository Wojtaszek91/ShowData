//var dataTable;

//$(document).ready(function () {
//    var id = document.getElementById("projectIdHolder");
//    alert(id);
//    loadDataTable(id.value);
//});

//function loadDataTable(id) {
//    var getData = JSON.stringify({
//        projectId : id
//    })
//    dataTable = $('#tblData').DataTable({
//        "ajax": {
//            "url": "/task/GetAllTaskFromProject",
//            "type": "POST",
//            "data": getData,
//            "datatype": "json"
//        },
//        "columns": [
//            { "data": "project.title", "width": "20%" },
//            { "data": "displayName", "width": "20%" },
//            { "data": "isAvailsable", "width": "10%" },
//            { "data": "displayDate", "width": "20%" },
//            {
//                "data": "id",
//                "render": function (data) {
//                    return `<div class="text-center">
//                                <a href="/task/Upserttask/${data}" class='btn btn-success text-white'
//                                    style='cursor:pointer;'> <i class='far fa-edit'></i></a>
//                                    &nbsp;
//                                <a onclick=Delete("/task/Deletetask/${data}") class='btn btn-danger text-white'
//                                    style='cursor:pointer;'> <i class='far fa-trash-alt'></i></a>
//                                </div>
//                            `;
//                }, "width": "30%"
//            }
//        ]
//    });
//}

function Delete(id) {
    swal({
        title: "Are you sure you want to Delete this task ?",
        text: "You will not be able to restore the data !",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: '/task/Deletetask/' + id,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}