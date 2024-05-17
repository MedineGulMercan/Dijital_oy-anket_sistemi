$(document).ready(function () {
    $("#searchBox").on("input", function () {
        var searchQuery = $(this).val().trim(); // Arama sorgusunu alın ve boşlukları temizleyin

        if (searchQuery !== "") {
            $("#searchResults").removeClass("hidden");
            $.ajax({
                url: `/Group/SearchGroups`, // Sunucuya istek gönderilecek URL
                type: "GET", // HTTP GET isteği
                data: { q: searchQuery }, // Sorgu parametresi olarak 'q'
                success: function (data) { // Başarılı olursa bu fonksiyon çalışır
                    var resultsContainer = $("#searchResults");
                    resultsContainer.empty(); // Mevcut sonuçları temizleyin

                    if (data.length === 0) { // Hiç sonuç yoksa
                        resultsContainer.append("<p>No results found.</p>");
                    } else {
                        data.forEach(function (group) {
                            resultsContainer.append(`<div class="text-center"><a href="#" onclick="openPage('${group.id}')">${group.groupName}</a></div>`);
                        });
                    }
                },
                error: function (xhr, status, error) { // Hata durumunda
                    console.error("AJAX error:", status, error);
                    $("#searchResults").append("<p>Error fetching results.</p>");
                }
            });
        } else {
            $("#searchResults").addClass("hidden");
            $("#searchResults").empty(); // Eğer kutu boşsa sonuçları temizleyin
        }
    });
});
async function openPage(id) {
    console.log("Opening page with group ID:", id);
    // Örneğin, bir URL'ye yönlendirme yapabilirsiniz.
    window.location.href = `/Group/Index?id=${id}`;
}