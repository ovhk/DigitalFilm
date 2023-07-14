﻿using System;
using System.Security.Cryptography;

namespace DigitalFilm.Tools
{
    internal class Checksum
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string CalculateMD5(string filename)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (System.IO.FileStream stream = System.IO.File.OpenRead(filename))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
