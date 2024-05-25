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
                            resultsContainer.append(`<div class="cart">
                        <div>
                            <div  class="img">
                                <img src="/images/profile_img.jpg" alt="">
                            </div>
                            <div class="info">
                                <p class="name" style="cursor: pointer;"  onclick="openPage('${group.id}')">${group.groupName}</p>
                            </div>
                        </div>
                    </div>`);
                        });
                        /*
                        <div b-cg3yhkq320="" class="cart">
                        <div b-cg3yhkq320="">
                            <div b-cg3yhkq320="" class="img">
                                <img b-cg3yhkq320="" src="/images/profile_img.jpg" alt="">
                            </div>
                            <div b-cg3yhkq320="" class="info">
                                <p b-cg3yhkq320="" class="name">Zineb_essoussi</p>
                                <p b-cg3yhkq320="" class="second_name">Zim Ess</p>
                            </div>
                        </div>
                        <div b-cg3yhkq320="" class="clear">
                            <a b-cg3yhkq320="" href="#">X</a>
                        </div>
                    </div>
                        */
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