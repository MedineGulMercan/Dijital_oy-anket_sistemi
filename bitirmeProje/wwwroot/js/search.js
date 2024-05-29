$(document).ready(function () {
    $("#searchInput").on("input", function () {
        var searchQuery = $(this).val().trim(); // Arama sorgusunu alın ve boşlukları temizleyin

        if (searchQuery !== "") {
            $.ajax({
                url: `/Group/SearchGroups`, // Sunucuya istek gönderilecek URL
                type: "GET", // HTTP GET isteği
                data: { q: searchQuery }, // Sorgu parametresi olarak 'q'
                success: function (data) { // Başarılı olursa bu fonksiyon çalışır
                    var resultsContainer = $("#searchAccount");
                    resultsContainer.empty(); // Mevcut sonuçları temizleyin
                    if (data.length === 0) { // Hiç sonuç yoksa
                        resultsContainer.append("<p>Bu isime sahip bir grup bulunamadı.</p>");
                    } else {
                        data.forEach(function (group) {
                            var image1 = ' <img class="fixed-size-image-40" src="/groupImage/group-photo.jpg" alt="">'
                            var image2 = `<img class="fixed-size-image-40" src="${group.imageUrl}" alt="">` 
                            resultsContainer.append(`<div class="cart">
                        <div>
                       
                            <div  class="img">
                             ${group.imageUrl ? image2 :image1   }
                            </div>
                            <div class="info">
                                <p class="name" style="cursor: pointer;"  onclick="openPage('${group.id}')">${group.groupName}</p>
                            </div>
                        </div>
                    </div>`);
                        });
                    }
                },
                error: function (xhr, status, error) { // Hata durumunda
                    console.error("AJAX error:", status, error);
                    $("#searchResults").append("<p>Error fetching results.</p>");
                }
            });
        } else {
            $("#searchAccount").empty(); // Eğer kutu boşsa sonuçları temizleyin
        }
    });
});
async function openPage(id) {
    console.log("Opening page with group ID:", id);
    // Örneğin, bir URL'ye yönlendirme yapabilirsiniz.
    window.location.href = `/Group/Index?id=${id}`;
}