using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LibrarySystemForWeb.Tools
{
    /// <summary>
    /// 字符串加密解密类
    /// </summary>
    public static class StringSecurity
    {
        #region MD5 加密
        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="sourceString">原字符串</param>
        /// <param name="codeName">编码名称</param>
        /// <returns></returns>
        public static string MD5Encrypt(string sourceString, string codeName = "UTF-8")
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] source = md5.ComputeHash(Encoding.GetEncoding(codeName).GetBytes(sourceString));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sBuilder.Append(source[i].ToString("X"));//x小写 X大写  x2填充对齐
            }
            return sBuilder.ToString();
        }
        #endregion
    }
}