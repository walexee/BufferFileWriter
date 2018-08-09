using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FileBlockUpload.Tests
{
    public static class Helpers
    {
        public static string ComputeMd5Hash(Stream stream)
        {
            using (var sha = MD5.Create())
            {
                var hash = sha.ComputeHash(stream);

                return ToHexString(hash);
            }
        }

        public static string ComputeFileMd5Hash(string filepath)
        {
            using (var fileStream = File.OpenRead(filepath))
            {
                using (var sha = MD5.Create())
                {
                    var hash = sha.ComputeHash(fileStream);

                    return ToHexString(hash);
                }
            }
        }

        public static string ComputeMd5Hash(byte[] byteArray)
        {
            using (var sha = MD5.Create())
            {
                var hash = sha.ComputeHash(byteArray);

                return ToHexString(hash);
            }
        }

        public static string ToHexString(byte[] byteArray)
        {
            var stringBuilder = new StringBuilder();

            foreach (byte value in byteArray)
            {
                stringBuilder.Append(value.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var list = source.ToList();

            for (var i = 0; i < list.Count; i++)
            {
                action(list[i], i);
            }
        }

        public static void Shuffle<T>(this IList<T> source)
        {
            var random = new Random();

            for (var i = 0; i < source.Count; i++)
            {
                var swapIndex = random.Next(source.Count);
                var currentValue = source[i];

                source[i] = source[swapIndex];
                source[swapIndex] = currentValue;
            }
        }
    }
}
