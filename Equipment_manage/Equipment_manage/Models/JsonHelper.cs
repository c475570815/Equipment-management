using System;
/// <summary>
/// FileName: JSONHelper.cs
/// CLRVersion: 2.0.50727.3623
/// Author: Mikel
/// Corporation:
/// Description:JSON��ʽ����ת��������
/// 1.��List<T>���͵�����ת��ΪJSON��ʽ
/// 2.��T���Ͷ���ת��ΪJSON��ʽ����
/// 3.��JSON��ʽ����ת��ΪT���Ͷ���
/// DateTime: 2011-09-13 14:11:34
/// </summary>
using System.Collections.Generic;
using System.Web.Mvc;
public static class JSONHelper 
{
    /// <summary>
    /// ת������ΪJSON��ʽ����
    /// </summary>
    /// <typeparam name="T">��</typeparam>
    /// <param name="obj">����</param>
    /// <returns>�ַ���ʽ��JSON����</returns>
    public static string GetJSON<T>(object obj)
    {
        string result = String.Empty;
        try
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
            new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                result = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }
    /// <summary>
    /// ת��List<T>������ΪJSON��ʽ
    /// </summary>
    /// <typeparam name="T">��</typeparam>
    /// <param name="vals">�б�ֵ</param>
    /// <returns>JSON��ʽ����</returns>
    public static string JSON<T>(List<T> vals)
    {
        System.Text.StringBuilder st = new System.Text.StringBuilder();
        try
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer s = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));

            foreach (T city in vals)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    s.WriteObject(ms, city);
                    st.Append(System.Text.Encoding.UTF8.GetString(ms.ToArray()));
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return st.ToString();
    }
    /// <summary>
    /// JSON��ʽ�ַ�ת��ΪT���͵Ķ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonStr"></param>
    /// <returns></returns>
    public static T ParseFormByJson<T>(string jsonStr)
    {
        T obj = Activator.CreateInstance<T>();
        using (System.IO.MemoryStream ms =
        new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonStr)))
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
            new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(ms);
        }
    }


}