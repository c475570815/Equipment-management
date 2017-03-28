var table_id = '#table';
var ra_id;
$(document).ready(
    function () {
        $(table_id).bootstrapTable({
            url: '/Maintenance/GetMaintenanceAppoint',
            method: 'get',
            checkboxHeader: false,//隐藏全选
            uniqueId: "ra_ID",      //每一行的唯一标识，一般为主键列
            showRefresh: true, //是否显示刷新按钮
            pagination: true,     //是否显示分页（*）
            sidePagination: "server",   //分页方式：client客户端分页，server服务端分页（*）
            pageSize: 5,      //每页的记录行数（*）
            pageList: [10, 25, 50, 100],  //可供选择的每页的行数（*）
            queryParamsType: '',// 设置为 ''  在这种情况下传给服务器的参数为：pageSize,pageNumber// queryParams: oTableInit.queryParams,//传递参数（*）
            queryParams: function (params) {
                return params;
            },//查询方法
            responseHandler: function (res) {
                for (var i = 0; i < res.rows.length; i++) {
                    if (res.rows[i].ra_Time != "")
                        res.rows[i].ra_Time = data_string(res.rows[i].ra_Time);
                }
                return res;
            },//在得到数据后对数据做处理再加载
            columns: [
                    {
                        field: 'typename',
                        title: '设备类型'
                    },
                    {
                        field: 'place',
                        title: '设备所在地'
                    },
                    {
                        field: 'ra_Resion',
                        title: '故障描述'
                    },
                    {
                        field: 'applyname',
                        title: '申请人'
                    },
                    {
                        field: 'applyphnoe',
                        title: '申请人电话'
                    },
                    {
                        sortable: true,
                        field: 'ra_Time',
                        title: '申请时间'
                    },
                    {
                        field: 'file',
                        title: '操作',
                        formatter: function (value, row, index) {
                            if (row.ra_ID == undefined)
                                return "--";
                            return '<button type="button" class="btn btn-info" id="information"  onclick="Information('+row.ra_ID+')">派遣人员</button>';
                        }
                    }
            ]
        });
    }
);
function Information(id) {
    ra_id = id;
    $('#AppointModal').modal('show');
}
function Messageshow(date) {
    var a = document.getElementById("Message");
    a.innerText = date;
    $('#myModal').modal('show');
}
$('#AppointModal').on('show.bs.modal', function () {
    $("#table_appoint").bootstrapTable({
        url: '/Maintenance/GetAppointPeople',
        method: 'get',
        checkboxHeader: false,//隐藏全选
        uniqueId: "ra_ID",      //每一行的唯一标识，一般为主键列
        pagination: true,     //是否显示分页（*）
        sidePagination: "server",   //分页方式：client客户端分页，server服务端分页（*）
        pageSize: 5,      //每页的记录行数（*）
        pageList: [10, 25, 50, 100],  //可供选择的每页的行数（*）
        queryParamsType: '',// 设置为 ''  在这种情况下传给服务器的参数为：pageSize,pageNumber// queryParams: oTableInit.queryParams,//传递参数（*）
        queryParams: function (params) {
            return params;
        },//查询方法
        columns: [
                {
                    field: 'e_ID',
                    title: '人员编号'
                },
                {
                    field: 'e_Name',
                    title: '姓名'
                },
                {
                    field: 'e_Mobile',
                    title: '电话'
                },
                {
                    field: 'work_number',
                    title: '正在维修设备数'
                },
                {
                    field: 'file',
                    title: '操作',
                    formatter: function (value, row, index) {
                        if (row.e_ID == undefined)
                            return "--";
                        return '<button type="button" class="btn btn-info" id="information"  onclick="Assign(' + row.e_ID +')">指定</button>';
                    }
                }
        ]
    });  
})
function Assign(e_id)
{
    console.log(ra_id);
    $.ajax({
        url: "/Maintenance/Assign",
        type: 'GET',
        data: { people_Id: e_id, apply_id: ra_id },
        success: function (resultData) {
                $('#AppointModal').modal('hide');//隐藏模态框
                $(table_id).bootstrapTable('refresh', { url: 'GetMaintenanceAppoint' });
                $("#table_appoint").bootstrapTable('refresh', { url: 'GetAppointPeople' });
                Messageshow(resultData);
            }
    });
}
function Messageshow(date)
{
    var a = document.getElementById("Message");
    a.innerText = date;
    $('#myModal').modal('show');

}
function data_string(str) {
    var d = eval('new ' + str.substr(1, str.length - 2));
    var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
    for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
    return ar_date.join('-');

    function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }
}