$("postData").click(function () {
    var aName = $("#a_Name").val();
    var dName = $("#d_Name").val();
    var eqName = $("#eq_Name").val();
    var aResion = $("#a_Resion").val();
    if (aName == "") {
        ShowFailTip('申请人不能为空!');
        return;
    }
    $.ajax({
        url: "",
        type: 'POST',
        async: false,
        dataType: 'html',
        data: { a_Name: aName, d_Name: dName, eq_Name: eqName, a_Resion: aResion },
        success: function (resultData) {
            $(".dialogDiv").hide();
            if (resultData == "No") {
                ShowFailTip("提交失败!");
            } else {
                var data = $(resultData);
            }
        }
    });
});