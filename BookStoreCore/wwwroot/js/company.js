var dataTable; // Global variable declaration

$(document).ready(function () {
    loadDataTableCompany();
});

function loadDataTableCompany() {
    // Destroy existing DataTable if it exists
    if ($.fn.DataTable.isDataTable('#tblCompanyData')) {
        $('#tblCompanyData').DataTable().destroy();
    }
    
    dataTable = $('#tblCompanyData').DataTable({
        "ajax": {
            url: '/admin/company/getall',
            type: 'GET',
            cache: false, // Disable caching for fresh data
            dataSrc: 'data' // Properly map the JSON response
        },
        "columns": [
            { data: 'name', "width": "15%" },
            { data: 'streetAddress', "width": "15%" },
            { data: 'city', "width": "15%" },
            { data: 'state', "width": "10%" },
            { data: 'postalCode', "width": "10%" },
            { data: 'phoneNumber', "width": "10%" },            
            {
                data: 'id',
                "render": function (data, type, row) {
                    var editUrl = `/admin/company/upsertc?id=${data}`;
                    console.log('Generated Edit URL:', editUrl); // Debug logging
                    return `<div class="w-76 btn-group" role="group">
                        <a href="${editUrl}" class="btn btn-primary mx-2" onclick="console.log('Clicking edit for Company ID: ${data}');">
                            <i class="bi bi-pencil-square"></i> Edit
                        </a>
                        <a onclick="Delete('/admin/company/delete/${data}')" class="btn btn-danger mx-2">
                            <i class="bi bi-trash-fill"></i> Delete
                        </a>
                    </div>`;
                },
                "width": "25%",
                "orderable": false
            }
        ],
        "language": {
            "emptyTable": "No data found."
        },
        "processing": true,
        "serverSide": false,
        "responsive": true,
        "destroy": true, // Allow reinitialization
        "order": [[0, 'asc']] // Default sort by name
    });
}

function Delete(url) {
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
                cache: false, // Disable caching
                success: function (data) {
                    if (data.success) {
                        // Force fresh data reload
                        dataTable.ajax.reload(null, false); // false = stay on current page
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message || 'Error occurred while deleting');
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Delete error:', error);
                    toastr.error('Error occurred while deleting: ' + error);
                }
            });
        }
    });
}

// Optional: Add refresh function for manual refresh
function refreshCompanyTable() {
    if (dataTable) {
        dataTable.ajax.reload(null, false);
    }
}
