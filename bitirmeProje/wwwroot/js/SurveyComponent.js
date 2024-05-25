async function openReport(questionId) {
    $('#viewSurveyReport').modal('show');
    console.log(questionId)
    var formData = new FormData();
    formData.append("questionId", questionId)
    await $.ajax({
        url: '/Survey/GetVoteReport',
        type: 'POST',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            console.log(result)

            // Create the data table.
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Option');
            data.addColumn('number', 'Votes');

            // Add rows dynamically from the result
            var rows = result.map(function (item) {
                return [item.name, item.count];
            });
            data.addRows(rows);

            // Set chart options
            var options = {
                'title': 'ANKET SONUÇLARI GRAFİĞİ',
                'width': 400,
                'height': 300
            };

            // Instantiate and draw our chart, passing in some options.
            var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
            chart.draw(data, options);
        }
    });
    
}