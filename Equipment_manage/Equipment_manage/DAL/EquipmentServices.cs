using Equipment_manage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Equipment_manage.DAL
{
    public class DecivesInfoServices
    {
        private static Equipment DB = new Equipment();
        private static DbSet<DecivesInfo> table=DB.DecivesInfo;
        private static IQueryable<DecivesInfo> InitialDate = null;
        /// <summary>
        /// 根据提交的数据在数据库中做筛选
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="id">查询的设备编号</param>
        /// <returns></returns>
        public static IQueryable<DecivesInfo> GetWhere(string keyword, string id)
        {
            var a = table.Where(q => q.di_Name.Contains(keyword) ? true : //名
                 q.di_Model.Contains(keyword)//型号
                || q.di_Status == keyword //状态
                || q.di_ManuFacturers.Contains(keyword)//出品商
                || q.di_User.Contains(keyword)//使用者
                || q.di_Position.Contains(keyword))//存放位置
                .Where(q => id != "" ? id == q.di_EqId : true);//制定设备编号
            return a;
        }
        /// <summary>
        /// 为数据排序
        /// </summary>
        /// <param name="date">数据</param>
        /// <param name="sortOrder">指定排序方式</param>
        /// <param name="sortName">指定排序的字段</param>
        /// <returns></returns>
        public static IQueryable<DecivesInfo> Sort(IQueryable<DecivesInfo> date, string sortOrder, string sortName)
        {
                InitialDate = date.OrderBy(i => i.di_EqId);
            if (sortName != "" && InitialDate != null)
            { 
                if (sortOrder != "desc")
                    switch (sortName)
                    {
                        case "di_EqId": InitialDate = InitialDate.OrderBy(P => P.di_EqId); break;
                        case "di_Status": InitialDate = InitialDate.OrderBy(P => P.di_Status); break;
                        case "di_ButTime": InitialDate = InitialDate.OrderBy(P => P.di_ButTime); break;
                        default:  break;
                    }
                else
                    switch (sortName)
                    {
                        case "di_EqId": InitialDate = InitialDate.OrderByDescending(P => P.di_EqId); break;
                        case "di_Status": InitialDate = InitialDate.OrderByDescending(P => P.di_Status); break;
                        case "di_ButTime": InitialDate = InitialDate.OrderByDescending(P => P.di_ButTime); break;
                        default:break;
                    }            
            }
            return InitialDate;
        }
        /// <summary>
        /// 添加条记录
        /// </summary>
        /// <param name="recorde">被添加的记录</param>
        public static void AddRecorde(DecivesInfo recorde)
        {
            table.Add(recorde);
            DB.SaveChanges();
        }
        /// <summary>
        /// 查询di_EqId是否已经存在
        /// </summary>
        /// <param name="id">di_EqId</param>
        /// <returns>false 不存在 true存在</returns>
        public static bool Repetition(string id)
        {
          return  table.Where(p => p.di_EqId == id).Count()==0?false:true;
        }
        /// <summary>
        /// 根据id 查询记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DecivesInfo IdGetRecorde(string id)
        {
            return table.FirstOrDefault(p => p.di_EqId == id);
        }
        public static void clone(DecivesInfo a, DecivesInfo b)
        {
            b.di_ButTime = a.di_ButTime;
            b.di_EqId = a.di_EqId;
            b.di_ManuFacturers = a.di_ManuFacturers;
            b.di_Model = a.di_Model;
            b.di_Name = a.di_Name;
            b.di_Position = a.di_Position;
            b.di_Price = a.di_Price;
            b.di_Status = a.di_Status;
            b.di_User = a.di_User;
            b.di_Warranty = a.di_Warranty;
            b.dt_ID = a.dt_ID;
        }
        public static string Alert(DecivesInfo a, string id)
        {
            var record = table.FirstOrDefault(p => p.di_EqId == id);
            if (a.Equals(record))
            {
                return "您没有做修改";
            }
            DecivesInfoServices.clone(a, record);
            return DB.SaveChanges() == 1 ? "修改成功" : "修改异常";     
        }
        public static string Delete(string id)
        {
            var recorde = table.FirstOrDefault(p => p.di_EqId == id);
            if (recorde != null)
            {
                DB.DecivesInfo.Remove(recorde);
                var i = DB.SaveChanges();
                return "成功删除" + i + "条记录";
            }
            return "删除失败";
        }
        public static bool ToUseStatus(string id)
        {
            table.FirstOrDefault(p => p.di_EqId == id).di_Status = "可用";
            return  DB.SaveChanges() == 1 ? true : false;
        }
    }
}