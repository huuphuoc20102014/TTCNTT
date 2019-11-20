$("#btn-save").on("click", function (e) {

    var arrayId = $("#selected option");
    var idRoles;
    var array = [];

    for (var i = 0; i < arrayId.length; i++) {
        idRoles = $(arrayId[i]).val();
        array.push({ IDroles: idRoles });
    }
    console.log(array);
    if (arrayId.length == 0) {
        alert("Không có quyền nào được chọn");
    }
    else {
        $.ajax({
            url: _urlUserRoles,
            type: 'POST',
            data: JSON.stringify(array),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            crossDomain: true,
            success: function (data) {
                if (data.errorMessage) {
                    console.log(data.errorMessage);
                    alert('Lỗi');
                }
                else {
                    //Thực thi xong, chuyển sang View khác
                    window.location.href = url;
                }
            },
        });
    }
});