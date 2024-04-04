
async function UserLogin(event) {
    event.preventDefault();
    var form = document.getElementById("login-form");
    var formData = new FormData(form);
    console.log(formData.get("username"));

    await $.ajax({
        url: '/Login/UserLogin',
        type: 'POST',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success) {
                window.location.href = "/Login/Index?success=true&message=" + data.message;
            }
            else {
                error(data.message)
            }
        }
    });
}