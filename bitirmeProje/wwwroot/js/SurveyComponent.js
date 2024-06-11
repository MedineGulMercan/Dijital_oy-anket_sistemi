async function openReport(questionId) {
    // Anket raporu modal penceresini gösterir
    $('#viewSurveyReport').modal('show');
    var formData = new FormData();
    // Form verilerine questionId ekler
    formData.append("questionId", questionId)
    await $.ajax({
        url: '/Survey/GetVoteReport',
        type: 'POST',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            // Veritabanı tablosunu oluştur
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Option');// Birinci kolon: Seçenek
            data.addColumn('number', 'Votes'); // İkinci kolon: Oy sayısı
            // Sonuçlardan dinamik olarak satır ekle
            var rows = result.map(function (item) {
                return [item.name, item.count];// Her seçenek ve oy sayısını ekler
            });
            data.addRows(rows);// Satırları veritabanına ekler
            // Grafik seçeneklerini ayarla
            var options = {
                'title': 'ANKET SONUÇLARI GRAFİĞİ',
                'width': 400,
                'height': 300
            };
            // Grafik nesnesini oluştur ve çiz
            var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
            chart.draw(data, options);// Verilerle birlikte grafiği çiz
        }
    });

}

async function deleteSurvey(surveyId) {
    var formData = new FormData();
    formData.append("surveyId", surveyId)
    await $.ajax({
        url: '/Survey/DeleteSurvey',
        type: 'POST',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            document.getElementById("survey-" + surveyId).remove();
        }
    });
}
