async function UserUpdate(event,id) {
    event.preventDefault();
    var form = document.getElementById("user-update-form");
    var formData = new FormData(form);
    await $.ajax({
        url: 'User/UserUpdate',
        type: 'POST',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success) {
                window.location.href = `/User/Indext?id=${id}&success=true&message=${data.message}`;
            }
            else {
                error(data.message)
            }
        }
    });
}