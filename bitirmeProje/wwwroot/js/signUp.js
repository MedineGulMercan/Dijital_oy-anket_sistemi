// İl seçeneklerini güncelle
document.getElementById('countryId').addEventListener('change', function () 
{
    var countryId = this.value;
    var citySelect = document.getElementById('city');
    // Ajax isteği ile il verilerini al
    $.ajax({
        url: '/SignUp/GetCities', // İl verilerini döndüren action methodunun URL'si
        type: 'GET',
        data: { countryId: countryId },
        success: function (cities) {
            // İl seçeneklerini temizle
            citySelect.innerHTML = '';

            // Yeni il seçeneklerini ekle
            cities.forEach(function (city) {
                var option = document.createElement('option');
                option.value = city.id;
                option.textContent = city.cityName;
                citySelect.appendChild(option);
            });
        }
    });
});
// İlçe seçeneklerini güncelle
document.getElementById('city').addEventListener('change', function () {
    var cityId = this.value;
    var districtSelect = document.getElementById('districtId');

    // Ajax isteği ile ilçe verilerini al
    $.ajax({
        url: '/SignUp/GetDistricts', // İlçe verilerini döndüren action methodunun URL'si
        type: 'GET',
        data: { cityId: cityId },
        success: function (districts) {
            // İlçe seçeneklerini temizle
            districtSelect.innerHTML = '';

            // Yeni ilçe seçeneklerini ekle
            districts.forEach(function (district) {
                var option = document.createElement('option');
                option.value = district.id;
                option.textContent = district.districtName;
                districtSelect.appendChild(option);
            });
        }
    });
});
async function SignUp(event) {
    debugger
    //#region Create
    event.preventDefault();
    var form = document.getElementById("sign-up-form");
    var formData = new FormData(form); // FormData nesnesini oluştur
    await $.ajax({
        url: '/SignUp/UserSignUp',
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
    //#endregion
}

