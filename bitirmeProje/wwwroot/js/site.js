function info(message) {
    var nFrom = 'top';
    var nAlign = 'right';
    var nIcons = '';
    var nType = 'info';
    var nAnimIn = 'animated fadeInRight';
    var nAnimOut = 'animated fadeOutRight';
    var title = '';
    notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, message, title);
}

function error(message) {
    var nFrom = 'top';
    var nAlign = 'right';
    var nIcons = '';
    var nType = 'danger';
    var nAnimIn = 'animated fadeInRight';
    var nAnimOut = 'animated fadeOutRight';
    var title = '';
    notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, message, title);
}
function primary(message) {
    var nFrom = 'top';
    var nAlign = 'right';
    var nIcons = '';
    var nType = 'primary';
    var nAnimIn = 'animated fadeInRight';
    var nAnimOut = 'animated fadeOutRight';
    var title = '';
    notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, message, title);
}
function success(message) {
    console.log("Error notification");
    var nFrom = 'top';
    var nAlign = 'right';
    var nIcons = '';
    var nType = 'success';
    var nAnimIn = 'animated fadeInRight';
    var nAnimOut = 'animated fadeOutRight';
    var title = '';
    notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, message, title);
}
function warning(message) {
    console.log("Error notification");
    var nFrom = 'top';
    var nAlign = 'right';
    var nIcons = '';
    var nType = 'warning';
    var nAnimIn = 'animated fadeInRight';
    var nAnimOut = 'animated fadeOutRight';
    var title = '';
    notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, message, title);
}

function notify(from, align, icon, type, animIn, animOut, message, title) {
    $.notify({
        icon: icon,
        title: title,
        message: message,
        url: ''
    }, {
        element: 'body',
        type: type,
        allow_dismiss: true,
        placement: {
            from: from,
            align: align
        },
        offset: {
            x: 30,
            y: 30
        },
        spacing: 10,
        z_index: 999999,
        delay: 2500,
        timer: 1000,
        url_target: '_blank',
        mouse_over: false,
        animate: {
            enter: animIn,
            exit: animOut
        },
        icon_type: 'class',
        template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
            '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
            '<span data-notify="icon"></span> ' +
            '<span data-notify="title">{1}</span> ' +
            '<span data-notify="message">{2}</span>' +
            '<div class="progress" data-notify="progressbar">' +
            '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
            '</div>' +
            '<a href="{3}" target="{4}" data-notify="url"></a>' +
            '</div>'
    });
};
//url search parametresini alıyorum
const urlDocReadyQueryString = window.location.search;
const urlDocReadyParams = new URLSearchParams(urlDocReadyQueryString);
//success ve message paramlarını urlden siliyoruz
urlDocReadyParams.delete('success');
urlDocReadyParams.delete('message');

//parametre kaldımı diye kontrol ediyoruz kaldıysa url soru işareti (?) ile oluşturuluyor.
let newDocReadySiteUrl = window.location.origin + window.location.pathname;
if (urlDocReadyParams.toString()) {
    newDocReadySiteUrl += '?' + urlDocReadyParams.toString();
}

//url güncelleniyor
history.replaceState({}, document.title, newDocReadySiteUrl);

function logout() {
    // Ajax isteği ile ilçe verilerini al
    $.ajax({
        url: '/Login/UserLogout', // İlçe verilerini döndüren action methodunun URL'si
        type: 'POST',
        success: function () {
            window.location.href = "/Login/Index";
        }
    });
}

function deleteAddGroupPhoto() {
    debugger
    $('#preview').empty()
    $('#image-id-group').remove();
    $('#preview').append('<span>Resim Yükle</span>')
    $('#groupImageLayout').val("");
}

function previewImageLayout() {
    var file = document.getElementById('groupImageLayout').files[0];  /* Seçilen dosya */
    var reader = new FileReader();

    reader.onload = function (e) {
        var preview = document.getElementById('preview');
        preview.innerHTML = '';  /* Mevcut içeriği temizle */
        var img = document.createElement('img');
        img.src = e.target.result;  /* Resim içeriği */
        img.id = "image-id-group"
        preview.appendChild(img);  /* Resmi ekle */
    };

    if (file) {
        reader.readAsDataURL(file);  /* Resmi oku ve veri URL'sine dönüştür */
    }
}