//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Equipment_manage.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RepairApply
    {
        public int ra_ID { get; set; }
        public int e_ID { get; set; }
        public string di_EqId { get; set; }
        public Nullable<System.DateTime> ra_Time { get; set; }
        public string ra_Resion { get; set; }
        public Nullable<int> ra_Approval { get; set; }
        public Nullable<int> ra_AllocationPerson { get; set; }
        public string ra_Status { get; set; }
        public Nullable<System.DateTime> ra_ApprovalTime { get; set; }
        public int d_ID { get; set; }

        public string place;//设备的存放处
        public string typename;//设备类型名
        public string applyname;//申请人姓名
        public string applyphnoe; //申请人的电话
    }
}