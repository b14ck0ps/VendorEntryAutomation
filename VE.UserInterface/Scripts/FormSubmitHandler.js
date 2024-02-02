$(document).ready(function () {
    $('.add-material-btn').on('click', function () {
        var valueString = $(this).data('value');
        var materialCode = valueString.split('|')[0];
        var materialName = valueString.split('|')[1];

        $('#myModal').modal('hide');

        var row = $('<tr>').attr('id', materialCode)
            .append($('<td>').text(materialCode))
            .append($('<td>').text(materialName))
            .append(
                $('<input>').attr({
                    type: 'hidden',
                    id: materialCode,
                    name: materialCode,
                    class: 'form-control material-input',
                    value: materialCode + '|' + materialName
                })
            )
            .append($('<td>').append(
                $('<button>').addClass('btn btn-danger remove-material-btn').attr('data-code', materialCode).text('Remove')
            ));
        $('#materials tbody').append(row);

        $('.remove-material-btn').on('click', function () {
            var codeToRemove = $(this).data('code');
            $('#' + codeToRemove).remove();
        });
    });

    var table = $('#materialTable').DataTable({
        "paging": true,
        "info": false
    });

    $('#materialSearch').keyup(function () {
        table.search($(this).val()).draw();
    });

    function getAllMaterialInputs() {
        var materialInputs = [];
        $('.material-input').each(function () {
            materialInputs.push($(this).val());
        });
        return materialInputs;
    }

    $('form').submit(function (event) {
        event.preventDefault();
        $(".loader-container").show();
        var selectedMaterials = getAllMaterialInputs();

        if (selectedMaterials.length === 0) {
            // Show an error message for missing materials
            alert('Please select at least one material.');
            $(".loader-container").hide(); // Hide loader
            console.log($('#ExisitngSupplierCount').val());
            return;
        }


        var formData = new FormData(this);
        formData.append('VendorName', $('#VendorName').val());
        formData.append('VendorEmail', $('#VendorEmail').val());
        formData.append('ServiceDescription', $('#ServiceDescription').val());
        formData.append('RequirementGeneral', $('#RequirementGeneral').val());
        formData.append('RequirementOther', $('#RequirementOther').val());
        formData.append('TypeOfSupplierId', $('#TypeOfSupplierId').val());
        formData.append('ExisitngSupplierCount', $('#ExisitngSupplierCount').val());
        formData.append('ExisitngSupplierProblem', $('#ExisitngSupplierProblem').val());
        formData.append('NewSupplierAdditionReason', $('#NewSupplierAdditionReason').val());
        formData.append('Comment', $('#Comment').val());
        formData.append('SelectedMaterials', JSON.stringify(selectedMaterials));

        var clickedButtonId = $(document.activeElement).attr('id');
        let endpoint;
        if (clickedButtonId === 'change') {
            endpoint = '/Home/SubmitFormForChangeRequest';
            formData.append('Code', $('#AppProspectiveVendorId').val());
        } else if (clickedButtonId === 'submit') {
            endpoint = '/Home/SubmitForm';
        }

        $.ajax({
            url: endpoint,
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response && response.code) {
                    var redirectUrl = response.code ? '/Home/Details/' + response.code : 'Home/Index';
                    window.location.href = redirectUrl;
                }
            },
            error: function (error) {
                $(".loader-container").show();
                alert('Error occured while submitting the form. Please try again later.');
                console.log(error);
            }
        });
    });
    if (materials !== undefined && materials !== null) {
        if (materials.length > 0) {
            materials.forEach(function (material) {
                var materialCode = material.MaterialCode;
                var materialName = material.MaterialName;

                var row = $('<tr>').attr('id', materialCode)
                    .append($('<td>').text(materialCode))
                    .append($('<td>').text(materialName))
                    .append(
                        $('<input>').attr({
                            type: 'hidden',
                            id: materialCode,
                            name: materialCode,
                            class: 'form-control material-input',
                            value: materialCode + '|' + materialName
                        })
                    )
                    .append($('<td>').append(
                        $('<button>').addClass('btn btn-danger remove-material-btn').attr('data-code', materialCode).text('Remove')
                    ));
                $('#materials tbody').append(row);

                $('.remove-material-btn').on('click', function () {
                    var codeToRemove = $(this).data('code');
                    $('#' + codeToRemove).remove();
                });
            });
        }
    }
});

//@Author: Azran