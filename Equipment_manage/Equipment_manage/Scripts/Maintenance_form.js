var table_id = '#table';
var pass_Id = "";
var refuse_Id = "";
$(document).ready(
    function () {
        $(table_id).bootstrapTable({
            url: '/Maintenance/GetMaintenanceApply',
            method: 'get',
            checkboxHeader: false,//隐藏全选
            uniqueId: "ra_ID",      //每一行的唯一标识，一般为主键列
            showRefresh: true, //是否显示刷新按钮
            pagination: true,     //是否显示分页（*）
            sidePagination: "server",   //分页方式：client客户端分页，server服务端分页（*）
            pageSize: 5,      //每页的记录行数（*）
            pageList: [10, 25, 50, 100],  //可供选择的每页的行数（*）
            queryParamsType: '',// 设置为 ''  在这种情况下传给服务器的参数为：pageSize,pageNumber// queryParams: oTableInit.queryParams,//传递参数（*）
            //rel:drevil,
            queryParams: function (params) {
                return params;
            },//查询方法
            onCheck: function (row, $element) {
                pass_Id = pass_Id.replace(row.ra_ID + ",", "");
                pass_Id += row.ra_ID + ",";//replace
                console.log(pass_Id);
            },//选中事件
            onUncheck: function (row, $element) {
                pass_Id = pass_Id.replace(row.ra_ID + ",", "");
                console.log(pass_Id);
            },//取消选中事件
            onClickRow: function (row,$element)
            {
                $element.find("input").click();
            },//行点击事件
            onLoadSuccess: function(date)
            {
                for (var i = 0; i < date.rows.length; i++) {
                    popovr($("tr").eq(i + 1), date.rows[i].ra_ID);
                }
            },//加载成功事件
            responseHandler: function (res) {
                for (var i = 0; i < res.rows.length; i++) {
                    if (res.rows[i].ra_Time != "")
                        res.rows[i].ra_Time = data_string(res.rows[i].ra_Time);

                }
                return res;
            },//在得到数据后对数据做处理再加载
            columns: [
                    {
                        field: 'pass',
                        checkbox: true,
                        title: "通过"
                    },
                    {
                        field: 'ra_ID',
                        title: 'number'
                    },
                    {
                        field: 'di_EqId',
                        title: '设备编号',
                    }, {
                        field: 'ra_Time',
                        title: '报修时间',
                        sortable: true,
                    }, {
                        field: 'ra_Resion',
                        title: '故障'
                    }, {
                        field: 'file',
                        title: '操作',
                        formatter: function (value, row, index) {
                            if (row.ra_ID == undefined) {
                                return "--";
                            } else {
                                return '<button type="button" class="btn btn-info" id="refuse"  onclick="refuse(' + row.ra_ID + ')">拒绝</button>';
                            }
                        }
                    }
            ]
        });
        $(table_id).bootstrapTable('hideColumn', 'ra_ID');//隐藏单号  
    }
    );

function pass() {
    if (pass_Id == "") {
        Messageshow('请勾选相应报修申请记录后再做提交');
        return;
    }
    $.ajax({
        url: "/Maintenance/Pass",
        type: 'GET',
        data: { pass_Id: pass_Id },
        success: function (resultData) {
            if (resultData) {
                Messageshow(resultData);
                pass_Id = "";
                $(table_id).bootstrapTable('refresh', { url: '/Maintenance/GetMaintenanceApply' });
            } else {
                Messageshow(resultData);
            }
        }
    });
};
function refuse(id) {
    $.ajax({
        url: "/Maintenance/Refuse",
        type: 'GET',
        data: { refuse_id: id },
        success: function (resultData) {
            if (resultData) {
                Messageshow(resultData);
                check_Id = "";
                $(table_id).bootstrapTable('refresh', { url: '/Maintenance/GetMaintenanceApply' });
            } else {
                Messageshow(resultData);
            }
        }
    });
};
function Messageshow(date) {
    var a = document.getElementById("Message");
    a.innerText = date;
    $('#myModal').modal('show');
}
function popovr(tr, id) {
    $.ajax({
        url: "/Maintenance/Getinformation",
        type: 'GET',
        data: { id: id },
        success: function (resultData) {
            if (resultData != null) {
                tr.popover
             ({
                 trigger: 'manual',
                 placement: 'right', //显示的位置
                 title: ' <center><h5> 详细信息</h5></center>',
                 html: 'true',
                 content: '<table><tr><td>申请者姓名:' + resultData.apply_name + '</td></tr>' +
                                     '<tr><td>设备所属部门名称:' + resultData.department_name + '</td></tr>' +
                                     '<tr><td>设备类型:' + resultData.Dinf_type + '</td></tr>' +
                                     '<tr><td>设备名称:' + resultData.Dinf_name + '</td></tr></table>'
             }).on("mouseenter", function () {
                 $(this).popover("show");
                 $(this).siblings(".popover").css("width", "22%");
                 $(this).siblings(".popover").on("mouseleave", function () {//siblings被选元素的所有同胞元素popover当鼠标从这个标签移走时隐藏提醒框 
                    $(_this).popover('hide');
                 });
             }).on("mouseleave", function () {
                 var _this = this;
                 setTimeout(function () {
                     if (!$(".popover:hover").length) {
                         $(_this).popover("hide")
                     }
                 }, 200);
             });
            }
        }
    });

}
function data_string(str) {
    var d = eval('new ' + str.substr(1, str.length - 2));
    var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
    for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
    return ar_date.join('-');
    function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }
}