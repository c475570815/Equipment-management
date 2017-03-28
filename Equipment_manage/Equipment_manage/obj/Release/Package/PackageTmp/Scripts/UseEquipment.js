var table_id = '#table';
var button_name = '#btn_query';
var alert = null;
var del_id = "";
$(document).ready(function () {
    $(table_id).bootstrapTable({
        url: '/UseEquipment/GetPersonEquipment',
        method: 'get',
        striped: true,
        showRefresh: true, //是否显示刷新按钮
        pagination: true,     //是否显示分页（*）
        sidePagination: "server",   //分页方式：client客户端分页，server服务端分页（*）
        pageSize: 10,      //每页的记录行数（*）
        pageList: [30],  //可供选择的每页的行数（*）
        toolbar: "#toolbar",//指定工具栏
        queryParamsType: '',// 设置为 ''  在这种情况下传给服务器的参数为：pageSize,pageNumber// queryParams: oTableInit.queryParams,//传递参数（*）
        onClickRow: function (row, $element) {
            $element.find("input")[0].click();
        },
        onCheck: function (row, $element) {
            del_id = del_id.replace(row.e_ID + ",", "");
            del_id += row.e_ID + ",";//replace
        },
        onUncheck: function (row, $element) {
            del_id = del_id.replace(row.e_ID + ",", "");
        },//取消选中事件
        columns: [{
                        checkbox: true
                        },
                        {
                            field: 'di_EqId',
                            title: '设备编号'
                        }, {
                            field: 'di_Name',
                            title: '设备名称'
                        }, {
                            field: 'di_Position',
                            title: '存放地点'
                        }, {
                            field: 'di_Status',
                            title: '状态'
                        }]
    });
});