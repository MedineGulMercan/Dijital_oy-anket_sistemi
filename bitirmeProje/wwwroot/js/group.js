document.addEventListener('DOMContentLoaded', function () {
        var data = document.getElementById("groupId");
    if (data != null) {
        const groupId = data.value;
        checkMembershipStatus(groupId);
    }
});
// Kullanıcının üyelik durumunu kontrol et

//#region Üye ol butonunun değişimi
async function checkMembershipStatus(groupId) {
    try {
        // AJAX çağrısı ile JSON verisi alıyoruz
        const response = await $.ajax({
            url: '/Group/CheckMembershipStatus',
            type: 'GET',
            data: { groupId: groupId },
            dataType: 'json'
        });

        // Üyelik durumu bilgisini alıyoruz
        const membershipStatus = response.membershipStatus;

        // Butonun metnini ve durumunu üyelik durumuna göre ayarlayın
        const button = document.getElementById('membershipButton');
        if (membershipStatus === 'approved') {
            button.innerText = 'Üyesiniz';
            button.disabled = false;
            button.onclick = function () { // Butona tıklandığında onay mesajı göster
                confirmUnsubscribe();
            };

        }

        else if (membershipStatus === 'pending') {
            button.innerText = 'İstek Gönderildi';
            button.disabled = false;
            button.onclick = function () { // Butona tıklandığında onay mesajı göster
                confirmUnsubscribe();
            };
        }
        else {
            button.innerText = 'Üye Ol';
            button.disabled = false;
            button.onclick = function () {
                MemberRequest(groupId); // İşlevi doğru şekilde bağlayın
            };
        }
    } catch (error) {
        console.error('Error checking membership status:', error);
    }
}
//#endregion 

//#region Grup üyeliğinden çıkma
function confirmUnsubscribe() {
    const groupId = document.getElementById("groupId").value;
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Üyelikten çıkmak üzeresiniz!",
        icon: 'warning', // Uyarı simgesi
        showCancelButton: true, // İptal butonunu göster
        confirmButtonColor: '#3085d6', // Onay butonu rengi
        cancelButtonColor: '#d33', // İptal butonu rengi
        confirmButtonText: 'Evet!', // Onay butonu metni
        cancelButtonText: 'Vazgeç' // İptal butonu metni
    }).then((result) => {
        if (result.isConfirmed) {
            // Kullanıcı "Yes" dediğinde işlevi çalıştırın
            unsubscribeFromGroup(groupId); // Üyelikten çıkma işlevini çağır
        }
    });
}
async function unsubscribeFromGroup(groupId) {
    console.log(groupId)
    try {
        await $.ajax({
            url: '/Group/Unsubscribe', // Üyelikten çıkma URL'si
            type: 'POST', // POST isteği
            data: { groupId: groupId },
            dataType: 'json', // Yanıt türü JSON
            success: function (response) {
                if (response.success) {
                    console.log("Üyelikten çıktınız.");
                    const button = document.getElementById('membershipButton');
                    button.innerText = 'Üye Ol';
                    button.disabled = false;
                    button.onclick = function () {
                        MemberRequest(groupId); // İşlevi doğru şekilde bağlayın
                    };
                    // İstenirse başka bir işlem yapın veya sayfayı yenileyin
                } else {
                    console.error("Error:", response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX request failed:", status, error);
            }
        });
    } catch (error) {
        console.error("An error occurred during unsubscribe:", error);
    }
}
//#endregion 

//#region Grup Ekle 
async function GroupCreate(event) {
    event.preventDefault();
    var form = document.getElementById("group-create-form");
    var formData = new FormData(form);
    await $.ajax({
        url: '/Group/GroupCreate',
        type: 'POST',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success) {
                window.location.href = "/Home/Index?success=true&message=" + data.message;
            }
            else {
                error(data.message)
            }
        }
    });
}
//#endregion 

//#region Üye isteği  
async function MemberRequest(groupId) {
    await $.ajax({
        url: '/Group/MemberRequest',
        type: 'POST',
        data: { id: groupId }, // POST isteği ile veri gönderin // JSON formatına dönüştür
        success: function (data) {
            if (data.success) {
                window.location.href = `/Group/Index?id=${groupId}&success=true&message=` + data.message;
            }
            else {
                error(data.message)
            }
        }
    });
}
//#endregion

//#region Üye ol onay/red
async function UserRequestApproval(button, userId, groupId) {
    await $.ajax({
        url: '/Group/UserRequestApproval',
        type: 'POST',
        data: { userId: userId, groupId: groupId }, // POST isteği ile veri gönderin // JSON formatına dönüştür
        success: function (data) {
            if (data.success) {
                var row = button.closest('tr');
                row.remove();
                window.location.href = `/Group/Index?id=${groupId}&success=true&message=` + data.message;
            }
            else {
                error(data.message)
            }
        }
    });
}

async function UserRequestRejection(button,userId, groupId) {
    await $.ajax({
        url: '/Group/UserRequestRejection',
        type: 'POST',
        data: { userId: userId, groupId: groupId }, // POST isteği ile veri gönderin // JSON formatına dönüştür
        success: function (data) {
            if (data.success) {
                var row = button.closest('tr');
                row.remove();
                window.location.href = `/Group/Index?id=${groupId}&success=true&message=` + data.message;
            }
            else {
                error(data.message)
            }
        }
    });
}
//#endregion 

//#region Yönetici yap/çıkart
async function MakeGroupAdmin(button,userId, groupId) {
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Grup yöneticisi yapmak üzeresiniz!",
        icon: 'warning', // Uyarı simgesi
        showCancelButton: true, // İptal butonunu göster
        confirmButtonColor: '#3085d6', // Onay butonu rengi
        cancelButtonColor: '#d33', // İptal butonu rengi
        confirmButtonText: 'Evet!', // Onay butonu metni
        cancelButtonText: 'Vazgeç' // İptal butonu metni
    }).then(async (result) => {
        if (result.isConfirmed) {
            // Kullanıcı "Yes" dediğinde işlevi çalıştırın
            try {
                const response = await $.ajax({
                    url: '/Group/MakeGroupAdmin',
                    type: 'POST',
                    data: { userId: userId, groupId: groupId }, // Veri gönderme
                });

                if (response.success) {
                    // Başarı durumunda işlemler
                    const button = document.querySelector("[name='MakeGroupAdminButton']");
                    if (button) {
                        button.innerText = 'Grup Yöneticisi';
                    }
                    window.location.href = `/Group/Index?id=${groupId}&success=true&message=${response.message}`;
                } else {
                    console.error("Hata:", response.message);
                }
            } catch (error) {
                console.error("AJAX çağrısı sırasında hata:", error);
            }
        }
    });
   
}

async function TakeGroupAdmin(button,userId, groupId) {
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Grup yöneticiliğinden çıkarmak üzeresiniz!",
        icon: 'warning', // Uyarı simgesi
        showCancelButton: true, // İptal butonunu göster
        confirmButtonColor: '#3085d6', // Onay butonu rengi
        cancelButtonColor: '#d33', // İptal butonu rengi
        confirmButtonText: 'Evet!', // Onay butonu metni
        cancelButtonText: 'Vazgeç' // İptal butonu metni
    }).then(async (result) => {
        if (result.isConfirmed) {
            // Kullanıcı "Yes" dediğinde işlevi çalıştırın
            try {
                const response = await $.ajax({
                    url: '/Group/TakeGroupAdmin',
                    type: 'POST',
                    data: { userId: userId, groupId: groupId }, // Veri gönderme
                });

                if (response.success) {
                    // Başarı durumunda işlemler
                    const button = document.querySelector("[name='TakeGroupAdminButton']");
                    if (button) {
                        button.innerText = 'Grup Yöneticisi Yap';
                    }
                    window.location.href = `/Group/Index?id=${groupId}&success=true&message=${response.message}`;
                } else {
                    console.error("Hata:", response.message);
                }
            } catch (error) {
                console.error("AJAX çağrısı sırasında hata:", error);
            }
        }
    });
}
//#endregion

function previewImage() {
    var file = document.getElementById('groupImage').files[0];  /* Seçilen dosya */
    var reader = new FileReader();

    reader.onload = function (e) {
        var preview = document.getElementById('preview');
        preview.innerHTML = '';  /* Mevcut içeriği temizle */
        var img = document.createElement('img');
        img.src = e.target.result;  /* Resim içeriği */
        preview.appendChild(img);  /* Resmi ekle */
    };

    if (file) {
        reader.readAsDataURL(file);  /* Resmi oku ve veri URL'sine dönüştür */
    }
}

$(window).on("load", function () {
    //var isHome = document.getElementById("homePage");
    //console.log(isHome)
    //if (isHome == null || isHome == "") {
    //    // JavaScript
    //    // Sidebar sınıfına sahip div'i seç
    //    const sidebar = document.querySelector('.sidebar');

    //    // Eğer div bulunduysa, onu sil
    //    if (sidebar) {
    //        sidebar.remove(); // div'i DOM'dan kaldırır
    //        // veya
    //        // sidebar.parentNode.removeChild(sidebar); // farklı bir yöntemle div'i siler
    //    }
    //}
});



