$("#btnSubmit").click(function () {
    $.ajax({
        url: "/Apply/AddApply",
        type: "post",
        data: { di_EqId: $("#di_EqId").val(), a_Resion: $("#a_Resion").val()},
        async: false,
        success: function (data) {
            if (data.state == "error") {
                $("#adderror>a").text(data.msg);
                $("#adderror").show();
                setTimeout(function () { $("#adderror").hide() }, 4000)
            }
            if (data.state == "success") {
                $("#addsuccess>a").text(data.msg);
                $("#addsuccess").show();
                $("#di_EqId").val("");
                $("#a_Resion").val("");
                setTimeout(function () { $("#addsuccess").hide() }, 4000)
            }
        },
    });
});
$(document).ready(function () {
    $("#adderror").hide();
    $("#addsuccess").hide();
})
$("clean").click(function () {
    $("#di_EqId").val("");
    $("#a_Resion").val();
});