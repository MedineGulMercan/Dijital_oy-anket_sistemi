
$(window).on("load", async function () {
    await GetAllGroup();
});
async function GetAllGroup(event) {
    var groupSelect = document.getElementById('group_name_select');
    await $.ajax({
        url: '/Survey/GetAllGroup',
        type: 'POST',
        success: function (groups) {
            groupSelect.innerHTML = '';

            groups.forEach(function (group) {
                var option = document.createElement('option');
                option.value = group.group.id;
                option.textContent = group.group.groupName;
                groupSelect.appendChild(option);
            });
        }
    });
}
async function addOption() {
    var optionInput = document.getElementById('survey_option');
    var optionValue = optionInput.value;

    // Seçenek değeri boşsa ekleme işlemi yapma
    if (optionValue.trim() === '') {
        return;
    }

    // Yeni bir satır oluştur
    var newRow = document.createElement('tr');
    // Yeni bir hücre oluştur ve içine seçeneği yerleştir
    var newCell = document.createElement('td');
    newCell.textContent = optionValue;
    // Yeni bir hücre oluştur ve içine silme butonunu yerleştir
    var deleteButtonCell = document.createElement('td');
    var deleteButton = document.createElement('button');
    deleteButton.className = 'btn btn-danger'; // Bootstrap ile silme butonu stili

    deleteButton.innerHTML = '<i class="fa fa-trash"></i>'; // FontAwesome ikonu
    deleteButton.onclick = function () {
        newRow.remove(); // Satırı sil
    };

    // Hücreleri satıra ekle
    newRow.appendChild(newCell);
    newRow.appendChild(deleteButtonCell);
    deleteButtonCell.appendChild(deleteButton);

    // Tabloya satırı ekle
    var optionTableBody = document.getElementById('option_table_body');
    optionTableBody.appendChild(newRow);

    // Seçenek girdisini temizle
    optionInput.value = '';
}
async function SurveyCreate(event) {
    event.preventDefault();
    var form = document.getElementById("survey-create-form");
    var formData = new FormData(form);
    // Şıkların bulunduğu tablo gövdesini al
    var optionTableBody = document.getElementById('option_table_body');
    var optionRows = optionTableBody.getElementsByTagName('tr');

    // Şıkları bir liste olarak topla
    var surveyOptions = [];
    for (var i = 0; i < optionRows.length; i++) {
        var cell = optionRows[i].getElementsByTagName('td')[0]; // İlk hücredeki metni al
        formData.append(`SurveyOptions[${i}].SurveyOption`, cell.textContent.trim()); // iç içe list şeklinde tutulan classlarda formdatanın anlmaası için index vererek
        // tek tek hangi prop'a geliceğini belirtiyoruz
    }
    console.log(formData.get("SurveyOptions"));
    // Şıkları form verilerine ekle

    var selectedGroup = $('#group_name_select').val(); // Seçili olanı al
    formData.append('GroupId', selectedGroup); // FormData'ya ekle
    await $.ajax({
        url: '/Survey/SurveyCreate',
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

async function SetVote() {
    event.preventDefault();
    const form = event.target;
    const questionId = form.querySelector('input[name="QuestionId"]').value;
    const selectedOption = form.querySelector('input[type="radio"]:checked');
    if (!selectedOption) {
        alert("Lütfen bir seçenek seçin.");
        return;
    }
    var formData = new FormData();
    const optionId = selectedOption.id;
    formData.append('optionId', optionId);
    formData.append('questionId', questionId);
    await $.ajax({
        url: '/Survey/SetVote',
        type: 'POST',
        dataType: 'json',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success) {
                success(data.message);
            }
            else {
                error(data.message)
            }
        }
    });
}