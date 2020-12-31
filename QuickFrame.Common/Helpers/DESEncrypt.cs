using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuickFrame.Common
{
    /// <summary>
    /// Des加解密
    /// </summary>
    public class DesEncrypt
    {
        private const string Key = "keyk#eyde$sc!";

        /// <summary>
        /// DES+Base64加密
        /// <para>采用ECB、PKCS7</para>
        /// </summary>
        /// <param name="encryptString">加密字符串</param>
        /// <param name="key">秘钥</param>
        /// <returns></returns>
        public static Task<string?> EncryptAsync(string encryptString, string? key = default)
        {
            return EncryptAsync(encryptString, key, false, true);
        }
        /// <summary>
        /// DES+Base64解密
        /// <para>采用ECB、PKCS7</para>
        /// </summary>
        /// <param name="decryptString">解密字符串</param>
        /// <param name="key">秘钥</param>
        /// <returns></returns>
        public static Task<string?> DecryptAsync(string decryptString, string? key = default)
        {
            return DecryptAsync(decryptString, key, false);
        }
        /// <summary>
        /// DES+16进制加密
        /// <para>采用ECB、PKCS7</para>
        /// </summary>
        /// <param name="encryptString">加密字符串</param>
        /// <param name="key">秘钥</param>
        /// <param name="lowerCase">是否小写</param>
        /// <returns></returns>
        public static Task<string?> Encrypt4HexAsync(string encryptString, string? key = default, bool lowerCase = false)
        {
            return EncryptAsync(encryptString, key, true, lowerCase);
        }
        /// <summary>
        /// DES+16进制解密
        /// <para>采用ECB、PKCS7</para>
        /// </summary>
        /// <param name="decryptString">解密字符串</param>
        /// <param name="key">秘钥</param>
        /// <returns></returns>
        public static Task<string?> Decrypt4HexAsync(string? decryptString, string? key = default)
        {
            return DecryptAsync(decryptString, key, true);
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="key"></param>
        /// <param name="hex"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        private async static Task<string?> EncryptAsync(string? encryptString, string? key, bool hex, bool lowerCase = false)
        {
            if (encryptString!.IsNull()) return default;
            if (key!.IsNull()) key = Key;
            if (key!.Length < 8) throw new ArgumentException("秘钥长度为8位", nameof(key));

            var keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            var inputByteArray = Encoding.UTF8.GetBytes(encryptString!);
            var provider = new DESCryptoServiceProvider
            {
                Mode = CipherMode.ECB,
                Key = keyBytes,
                Padding = PaddingMode.PKCS7
            };

            await using var stream = new MemoryStream();
            var cStream = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            await cStream.WriteAsync(inputByteArray.AsMemory(0, inputByteArray.Length));
            cStream.FlushFinalBlock();

            var bytes = stream.ToArray();
            return hex ? bytes.ToHex(lowerCase) : bytes.ToBase64();
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="key"></param>
        /// <param name="hex"></param>
        /// <returns></returns>
        private async static Task<string?> DecryptAsync(string? decryptString, string? key, bool hex)
        {
            if (decryptString!.IsNull()) return default;
            if (key!.IsNull()) key = Key;
            if (key!.Length < 8) throw new ArgumentException("秘钥长度最少为8位", nameof(key));

            var keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            var inputByteArray = hex ? decryptString!.HexToBytes() : Convert.FromBase64String(decryptString!);
            inputByteArray ??= Array.Empty<byte>();
            var provider = new DESCryptoServiceProvider
            {
                Mode = CipherMode.ECB,
                Key = keyBytes,
                Padding = PaddingMode.PKCS7
            };

            await using var mStream = new MemoryStream();
            var cStream = new CryptoStream(mStream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            await cStream.WriteAsync(inputByteArray.AsMemory(0, inputByteArray.Length));
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
    }
}
