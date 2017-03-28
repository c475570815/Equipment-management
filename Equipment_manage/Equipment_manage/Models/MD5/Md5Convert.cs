using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Repast_Model.MD5
{
    public class Md5Convert
    {
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decryptor(string text)
        {
            string key = "12345678";
            string MD5IV = "12345678";
            try
            {
                string reust = "";
                byte[] bytetext = Convert.FromBase64String(text);
                //获取IV和Key
                byte[] iv = Encoding.UTF8.GetBytes(MD5IV);
                byte[] bytekey = Encoding.UTF8.GetBytes(key);

                //创建加密对象开始加密
                DESCryptoServiceProvider desc = new DESCryptoServiceProvider();

                //流操作
                MemoryStream memorystream = new MemoryStream();
                CryptoStream cryptostream = new CryptoStream(memorystream, desc.CreateDecryptor(bytekey, iv), CryptoStreamMode.Write);
                //加密并写入内存流
                cryptostream.Write(bytetext, 0, bytetext.Length);
                cryptostream.FlushFinalBlock();
                cryptostream.Close();
                reust = Encoding.UTF8.GetString(memorystream.ToArray());
                memorystream.Close();
                return reust;
            }
            catch
            {
                return text;
            }
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encryptor(string text)
        {
            string key = "12345678";
            string MD5IV = "12345678";
            try
            {
                string reust = "";
                byte[] bytetext = Encoding.UTF8.GetBytes(text);
                //获取IV和Key
                byte[] iv = Encoding.UTF8.GetBytes(MD5IV);
                byte[] bytekey = Encoding.UTF8.GetBytes(key);

                //创建加密对象开始加密
                DESCryptoServiceProvider desc = new DESCryptoServiceProvider();

                //流操作
                MemoryStream memorystream = new MemoryStream();
                CryptoStream cryptostream = new CryptoStream(memorystream, desc.CreateEncryptor(bytekey, iv), CryptoStreamMode.Write);
                //加密并写入内存流
                cryptostream.Write(bytetext, 0, bytetext.Length);
                cryptostream.FlushFinalBlock();
                cryptostream.Close();
                reust = Convert.ToBase64String(memorystream.ToArray());
                memorystream.Close();
                return reust;
            }
            catch
            {
                return text;
            }
        }
    }
}
