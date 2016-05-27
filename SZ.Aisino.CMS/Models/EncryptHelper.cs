using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SZ.Aisino.CMS.Models
{
    /// <summary>
    /// �ӽ���
    /// </summary>
    public static class EnCryptHelper
    {
        /// <summary>
        ///  Ĭ�ϵ�ת���ʽ
        /// </summary>
        public static Encoding Encode = Encoding.Default;

        #region RSA����

        /// <summary>
        /// ��ָ����Կ�����ı�
        /// </summary>
        /// <param name="publicKey">��Կ</param>
        /// <param name="input">�ı�</param>
        /// <param name="encode">����ʱ��ת�����ֽڵı����ʽ ��Ĭ��Ϊ ANSI(Default )</param>
        /// <returns></returns>
        public static string RsaEncrypt(string publicKey, string input, Encoding encode = null)
        {
            if (null == encode)
            {
                encode = Encode;
            }
            string sFormat = string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", publicKey);
            var provider = new RSACryptoServiceProvider();
            var arrPubKey = Encoding.UTF8.GetBytes(publicKey);
            var pkB64 = Convert.ToBase64String(arrPubKey);
            var xmlStr = string.Format(sFormat, pkB64);
            var bytesInput = encode.GetBytes(input);
            provider.FromXmlString(xmlStr);
            var encryptArray = provider.Encrypt(bytesInput, false);
            var hexStr = BitConverter.ToString(encryptArray).Replace("-", "");
            return hexStr.ToLower();
        }

        #endregion

        #region ���ú���

        /// <summary>
        /// ���ֽ�ת����16��������
        /// </summary>
        /// <param name="bytes">�ֽ�����</param>
        /// <returns></returns>
        public static string Bytes2Hex(byte[] bytes)
        {
            var buffer = new StringBuilder();
            foreach (var t in bytes)
            {
                buffer.AppendFormat("{0:x2}", t);
            }
            return buffer.ToString();
        }

        /// <summary>
        /// ��16��������ת�����ֽ�
        /// </summary>
        /// <param name="input">16��������</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static byte[] Hex2Bytes(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            var offset = input.Length % 2;
            if (offset == 1) input = "0" + input;
            int i;
            var list = new List<byte>();
            for (i = 0; i < input.Length; i += 2)
            {
                var temp = input.Substring(i, 2);
                byte bv;
                var success = byte.TryParse(temp, NumberStyles.HexNumber, null, out bv);
                if (!success) throw new ArgumentOutOfRangeException();
                list.Add(bv);
            }
            return list.ToArray();
        }

        #endregion

        #region SHA1����

        /// <summary>
        /// SHA1���� ʹ��ȱʡ��Կ���ַ�������
        /// </summary>
        /// <param name="sourceString">Ҫ���ܵ��ַ�</param>
        /// <param name="encode">����ʱ��ת�����ֽڵı����ʽ ��Ĭ��Ϊ ANSI(Default )</param>
        /// <returns>shua1 ���ܺ�Ľ��</returns>
        public static string Sha1Encrypt(string sourceString, Encoding encode = null)
        {
            if (null == encode)
            {
                encode = Encode;
            }
            var data = encode.GetBytes(sourceString);
            HashAlgorithm sha = new SHA1CryptoServiceProvider();
            var bytes = sha.ComputeHash(data);
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        #endregion

        #region DES�ӽ���

        /// <summary>
        /// DES����
        /// </summary>
        /// <param name="pubKey">��Կ</param>
        /// <param name="pToEncrypt">����ת�����ַ�</param>
        /// <param name="bUseForOtherPlatform">�Ƿ�����������ƽ̨����������������ƽ̨ʱ�������㷨ʹ�õ���ECBģʽ
        /// ΢���Դ���ΪCBCģʽ</param>
        /// <returns>���ܺ������</returns>
        public static string DesEncrypt(string pubKey, string pToEncrypt, Encoding encode, bool notForOtherPlatform = true)
        {
            if (null == encode)
            {
                encode = Encode;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            if (notForOtherPlatform)
            {
                des.Mode = CipherMode.ECB;//�Խ�JAVA�����Ĭ�ϵ�CBC
                des.Padding = PaddingMode.PKCS7;//���ԶԽ�JAVA��PKCS5
            }

            byte[] inputByteArray = encode.GetBytes(pToEncrypt);

            des.Key = ASCIIEncoding.ASCII.GetBytes(pubKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(pubKey);
            //des.IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            //Write  the  byte  array  into  the  crypto  stream  
            //(It  will  end  up  in  the  memory  stream)  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get  the  data  back  from  the  memory  stream,  and  into  a  string  
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                //Format  as  hex  
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// DES����
        /// </summary>
        /// <param name="sKey">��Կ</param>
        /// <param name="pToDecrypt">����ת�����ַ�</param>
        /// <param name="encode">����</param>
        /// <param name="bUseForOtherPlatform">�Ƿ�����������ƽ̨����������������ƽ̨ʱ�������㷨ʹ�õ���ECBģʽ
        /// ΢���Դ���ΪCBCģʽ</param>
        /// <returns>���ܺ������</returns>
        public static string DesDecrypt(string sKey, string pToDecrypt, Encoding encode, bool notForOtherPlatform = true)
        {
            if (null == encode)
            {
                encode = Encode;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //Put  the  input  string  into  the  byte  array  
            int len = pToDecrypt.Length % 2;
            if (1 == len) pToDecrypt += " ";

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
           // des.IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            if (notForOtherPlatform)
            {
                des.Mode = CipherMode.ECB;//�Խ�JAVA�����Ĭ�ϵ�CBC
                des.Padding = PaddingMode.PKCS7;//���ԶԽ�JAVA��PKCS5
            }
            else
            {
                Debug.WriteLine(des.Mode);
                Debug.WriteLine(des.Padding);
            }
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
          
            //Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get  the  decrypted  data  back  from  the  memory  stream  
            //����StringBuild����CreateDecryptʹ�õ��������󣬱���ѽ��ܺ���ı����������  
            StringBuilder ret = new StringBuilder();
            return encode.GetString(ms.ToArray());
        }

    /// <summary>
        /// DES����
        /// </summary>
        /// <param name="sKey">��Կ</param>
        /// <param name="pToDecrypt">����ת�����ַ�</param>
        /// <param name="encode">����</param>
        /// <param name="bUseForOtherPlatform">�Ƿ�����������ƽ̨����������������ƽ̨ʱ�������㷨ʹ�õ���ECBģʽ
        /// ΢��Ĭ�ϵ�ΪCBCģʽ</param>
        /// <returns>���ܺ������</returns>
        public static string Base64DesDecrypt(string sKey, string pToDecrypt, Encoding encode, bool bUseForOtherPlatform = true)
        {
            if (null == encode)
            {
                encode = Encode;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //Put  the  input  string  into  the  byte  array  
            int len = pToDecrypt.Length % 2;
            if (1 == len) pToDecrypt += " ";

            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
          

            //�������ܶ������Կ��ƫ��������ֵ��Ҫ�������޸�  
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            if (bUseForOtherPlatform)
            {
                des.Mode = CipherMode.ECB;//�����������Ե�Des�����㷨
                des.Padding = PaddingMode.PKCS7;//
            }

              StringBuilder ret = new StringBuilder();

           using( MemoryStream ms = new MemoryStream())
           {
               CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

               //Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
               cs.Write(inputByteArray, 0, inputByteArray.Length);
               cs.FlushFinalBlock();
               //Get  the  decrypted  data  back  from  the  memory  stream  
               //����StringBuild����CreateDecryptʹ�õ��������󣬱���ѽ��ܺ���ı����������  
               return encode.GetString(ms.ToArray());
           }
         
        }

        #endregion

        #region MD5����

        /// <summary>
        /// MD5����
        /// </summary>
        /// <param name="input">����</param>
        /// <param name="encode">����ʱ��ת�����ֽڵı����ʽ ��Ĭ��Ϊ ANSI(Default )</param>
        /// <returns></returns>
        public static string Md5Encrypt(string input, Encoding encode = null)
        {
            if (null == encode)
            {
                encode = Encode;
            }

            MD5 md5 = new MD5CryptoServiceProvider();
            var data = encode.GetBytes(input);
            var encs = md5.ComputeHash(data);
            return BitConverter.ToString(encs).Replace("-", "");
        }

        /// <summary>
        /// MD5����
        /// </summary>
        /// <param name="input">����</param>
        /// <param name="encode">����ʱ��ת�����ֽڵı����ʽ ��Ĭ��Ϊ ANSI(Default )</param>
        /// <returns></returns>
        public static string Md5EncryptBase64(string input, Encoding encode = null)
        {
            if (null == encode)
            {
                encode = Encode;
            }

            MD5 md5 = new MD5CryptoServiceProvider();
            var data = encode.GetBytes(input);
            var encs = md5.ComputeHash(data);
            return Convert.ToBase64String(encs);
        }


        /// <summary>
        /// RSA����
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string publickey, string content)
        {
            publickey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv+SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e+BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            return BitConverter.ToString(cipherbytes).Replace("-", "");
        }

        /// <summary>
        /// RSA����
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSADecrypt(string privatekey, string content)
        {
            privatekey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv+SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e+BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=</Modulus><Exponent>AQAB</Exponent><P>/hf2dnK7rNfl3lbqghWcpFdu778hUpIEBixCDL5WiBtpkZdpSw90aERmHJYaW2RGvGRi6zSftLh00KHsPcNUMw==</P><Q>6Cn/jOLrPapDTEp1Fkq+uz++1Do0eeX7HYqi9rY29CqShzCeI7LEYOoSwYuAJ3xA/DuCdQENPSoJ9KFbO4Wsow==</Q><DP>ga1rHIJro8e/yhxjrKYo/nqc5ICQGhrpMNlPkD9n3CjZVPOISkWF7FzUHEzDANeJfkZhcZa21z24aG3rKo5Qnw==</DP><DQ>MNGsCB8rYlMsRZ2ek2pyQwO7h/sZT8y5ilO9wu08Dwnot/7UMiOEQfDWstY3w5XQQHnvC9WFyCfP4h4QBissyw==</DQ><InverseQ>EG02S7SADhH1EVT9DD0Z62Y0uY7gIYvxX/uq+IzKSCwB8M2G7Qv9xgZQaQlLpCaeKbux3Y59hHM+KpamGL19Kg==</InverseQ><D>vmaYHEbPAgOJvaEXQl+t8DQKFT1fudEysTy31LTyXjGu6XiltXXHUuZaa2IPyHgBz0Nd7znwsW/S44iql0Fen1kzKioEL3svANui63O3o5xdDeExVM6zOf1wUUh/oldovPweChyoAdMtUzgvCbJk1sYDJf++Nr0FeNW1RB1XG30=</D></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);

            return Encoding.UTF8.GetString(cipherbytes);
        }
        #endregion

        #region  AES ���ܽ���

        /// <summary>
        ///  �ֽ�����ת�ַ�����ʽ
        /// </summary>
        /// <param name="byteArray">�ֽ�����</param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] byteArray)
        {
            StringBuilder tp = new StringBuilder();

            foreach (byte by in byteArray)
            {
                //Format  as  hex  
                tp.AppendFormat("{0:X2}", by);
            }
            return tp.ToString();
        }
        /// <summary>
        ///  �ֽ�����ת�ַ�����ʽ
        /// </summary>
        /// <param name="byteStringArray">����Ϊ16���ƣ��ҳ��ȱ��뱻2����</param>
        /// <returns></returns>
        public static byte[] ByteStringArrayToByteArray(string byteStringArray)
        {
            if (byteStringArray.Length % 2 != 0)
            {
                return new byte[] { 0x00 };
            }

            string rt = string.Empty;
            byte[] rtByteArray = new byte[byteStringArray.Length / 2];

            for (int x = 0; x < byteStringArray.Length / 2; x++)
            {
                int i = (Convert.ToInt32(byteStringArray.Substring(x * 2, 2), 16));
                rtByteArray[x] = (byte)i;
            }
            return rtByteArray;
        }


        /// <summary>
        /// �������AES���� �� תΪBase64
        /// </summary>
        /// <param name="text">�����ַ�</param>
        /// <param name="password">���ܵ�����</param>
        /// <param name="iv">��Կ</param>
        /// <param name="encode">ת�ֽ�ʹ�õı���</param>
        /// <param name="bRtBase64">����Ƿ�תbase64</param>
        /// <param name="bUseForOtherPlatform">�Ƿ���������ƽ̨������ƽ̨һ��ΪECB</param>
        /// <returns></returns>
        public static string AESEncrypt(string text, string password, string iv, Encoding encode, bool bRtBase64 = false, bool bUseForOtherPlatform = true)
        {
            try
            {
                if (null == encode)
                {
                    encode = Encode;
                }

                RijndaelManaged rijndaelCipher = new RijndaelManaged();

                if (bUseForOtherPlatform)
                {
                    rijndaelCipher.Mode = CipherMode.ECB;
                    rijndaelCipher.Padding = PaddingMode.PKCS7;
                }
                else
                {
                    rijndaelCipher.Mode = CipherMode.CBC;
                    rijndaelCipher.Padding = PaddingMode.PKCS7;
                }

                rijndaelCipher.KeySize = 128;
                rijndaelCipher.BlockSize = 128;

                byte[] pwdBytes = encode.GetBytes(password);
                byte[] keyBytes = new byte[16];
                int len = pwdBytes.Length;

                if (len > keyBytes.Length) len = keyBytes.Length;

                System.Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;

                byte[] ivBytes = new byte[16];
                System.Array.Copy(encode.GetBytes(iv), ivBytes, encode.GetBytes(iv).Length);
                rijndaelCipher.IV = ivBytes;
                  byte[] cipherBytes = null;

                using (ICryptoTransform transform = rijndaelCipher.CreateEncryptor())
                {
                    byte[] plainText = encode.GetBytes(text);
                    cipherBytes  = transform.TransformFinalBlock(plainText, 0, plainText.Length);
                }


                if (bRtBase64)
                {
                    return Convert.ToBase64String(cipherBytes);
                }
                else
                {
                    return ByteArrayToString(cipherBytes);
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  AES ����
        /// </summary>
        /// <param name="encryByteArrayString">Ҫ���ܵ�ԭ�ֽ�����</param>
        /// <param name="password">��Կ</param>
        /// <param name="ivKey">����</param>
        /// <param name="encode">�ֽ����ַ���֮��ת��ʹ�õı��� </param>
        /// <param name="IsBase64">�Ƿ��ڽ��֮���ٽ���Base64����Ҫ�������紫�䣩 </param>
        /// <param name="bUseForOtherPlatform">�Ƿ�����������ƽ̨</param>
        /// <returns></returns>
        public static string AESDecrypt(string encryByteArrayString, string password, string ivKey, Encoding encode, bool IsBase64 = false, bool bUseForOtherPlatform = true)
        {
            if (null == encode)
            {
                encode = Encode;
            }

            byte[] pwdBytes = encode.GetBytes(password);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;

            if (len > keyBytes.Length) len = keyBytes.Length;
            System.Array.Copy(pwdBytes, keyBytes, len);


            byte[] ivBytes = new byte[16];
            System.Array.Copy(encode.GetBytes(ivKey), ivBytes, encode.GetBytes(ivKey).Length);

            byte[] byte_0 = null;

            if (IsBase64)
            {
                byte_0 = Convert.FromBase64String(encryByteArrayString);
            }
            else
            {
                byte_0 = ByteStringArrayToByteArray(encryByteArrayString);

            }

            byte[] buffer = null;

            Aes aes = Aes.Create();

            if (bUseForOtherPlatform)
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
            }
            else
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
            }

            try
            {
                if (byte_0 == null)
                {
                    return null;
                }

                using (ICryptoTransform transform = aes.CreateDecryptor(keyBytes, ivBytes))
                {
                    buffer = transform.TransformFinalBlock(byte_0, 0, byte_0.Length);
                }
            }
            catch (Exception exception)
            {
                buffer = null;
            }
            return encode.GetString(buffer);
        }

        /// <summary>
        ///  AES ����
        /// </summary>
        /// <param name="encryByteArrayString">Ҫ���ܵ�ԭ�ֽ�����</param>
        /// <param name="password">��Կ</param>
        /// <param name="ivKey">����</param>
        /// <param name="encode">�ֽ����ַ���֮��ת��ʹ�õı��� </param>
        /// <param name="bUseForOtherPlatform">�Ƿ�����������ƽ̨</param>
        /// <returns></returns>
        public static string AESDecrypt1(string encryByteArrayString, string password, string ivKey, Encoding encode, bool bUseForOtherPlatform = true)
        {
            if (null == encode)
            {
                encode = Encode;
            }

            byte[] pwdBytes = encode.GetBytes(password);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;

            if (len > keyBytes.Length) len = keyBytes.Length;
            System.Array.Copy(pwdBytes, keyBytes, len);


            byte[] ivBytes = new byte[16];
            System.Array.Copy(encode.GetBytes(ivKey), ivBytes, encode.GetBytes(ivKey).Length);

            byte[] byte_0 = ByteStringArrayToByteArray(encryByteArrayString);

            byte[] buffer = null;
           
            Aes aes = Aes.Create();

            if (bUseForOtherPlatform)
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
            }
            else
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
            }

            try
            {
                if (byte_0 == null)
                {
                    return null;
                }
                using (MemoryStream stream = new MemoryStream(byte_0))
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, aes.CreateDecryptor(keyBytes, ivBytes), CryptoStreamMode.Read))
                    {
                            byte[] buffer3 = new byte[0x6000];
                            int count = 0;  int nReadCount = 0;
                            while ((count = stream2.Read(buffer3, count, (buffer3.Length - count))) > 0)
                            {
                                nReadCount +=count;
                            }

                        buffer = new byte[nReadCount];
                        Array.Copy(buffer3, buffer, nReadCount);
                    }
                }
            }
            catch (Exception exception)
            {
                buffer = null;
            }
            return encode.GetString(buffer);
        }


		        /// <summary>
        ///  
        /// </summary>
        /// <param name="byte_0">Ҫ���ܵ�ԭ�ֽ�����</param>
        /// <param name="byte_1">key �ֽ�����</param>
        /// <param name="byte_2">�����ֽ�����</param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] byte_0, byte[] byte_1, byte[] byte_2)
        {
            byte[] buffer = null;
           
            Aes aes = Aes.Create();
            try
            {
                if (byte_0 == null)
                {
                    return null;
                }
                using (MemoryStream stream = new MemoryStream(byte_0))
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, aes.CreateDecryptor(byte_1, byte_2), CryptoStreamMode.Read))
                    {
                            byte[] buffer3 = new byte[0x6000];
                            int count = 0;  int nReadCount = 0;
                            while ((count = stream2.Read(buffer3, count, (buffer3.Length - count))) > 0)
                            {
                                nReadCount +=count;
                            }

                        buffer = new byte[nReadCount];
                        Array.Copy(buffer3, buffer, nReadCount);
                    }
                }
            }
            catch (Exception exception)
            {
                buffer = null;
            }
            return buffer;
        }
		


        /// <summary>
        ///  AES ����
        /// </summary>
        /// <param name="encryByteArrayString">Ҫ���ܵ�ԭ�ֽ�����</param>
        /// <param name="password">��Կ</param>
        /// <param name="ivKey">����</param>
        /// <param name="encode">�ֽ����ַ���֮��ת��ʹ�õı��� </param>
        /// <param name="bUseForOtherPlatform">�Ƿ�����������ƽ̨</param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] encryByteArray, byte[] bPassword, byte[] ivKey,  Encoding encode, bool bUseForOtherPlatform = true)
        {
            if (null == encode)
            {
                encode = Encode;
            }

            byte[] pwdBytes = bPassword;
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;

            if (len > keyBytes.Length) len = keyBytes.Length;
            System.Array.Copy(pwdBytes, keyBytes, len);


            byte[] ivBytes = new byte[16];
            System.Array.Copy(ivKey, ivBytes, ivKey.Length);

            byte[] byte_0 = encryByteArray;

            byte[] buffer = null;

            Aes aes = Aes.Create();

            if (bUseForOtherPlatform)
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
            }
            else
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
            }

            try
            {
                if (byte_0 == null)
                {
                    return null;
                }
                using (MemoryStream stream = new MemoryStream(byte_0))
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, aes.CreateDecryptor(keyBytes, ivBytes), CryptoStreamMode.Read))
                    {
                        byte[] buffer3 = new byte[0x6000];
                        int count = 0; int nReadCount = 0;
                        while ((count = stream2.Read(buffer3, count, (buffer3.Length - count))) > 0)
                        {
                            nReadCount += count;
                        }

                        buffer = new byte[nReadCount];
                        Array.Copy(buffer3, buffer, nReadCount);
                    }
                }
            }
            catch (Exception exception)
            {
                buffer = null;
            }
            return buffer;
        }



        /// <summary>
        /// ���������Կ
        /// </summary>
        /// <returns></returns>
        public static string GetIv(int n)
        {
            char[] arrChar = new char[]{
           'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
           '0','1','2','3','4','5','6','7','8','9',
           'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
          };

            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }

        /// <summary>
        ///  תΪBase64��AES����
        /// </summary>
        /// <param name="text">��������</param>
         /// <param name="password">���ܵ�����</param>
        /// <param name="iv"></param>
        /// <param name="encode">ת�ֽ�ʹ�õı���</param>
        /// <param name="bUseForOtherPlatform">�Ƿ���������ƽ̨������ƽ̨һ��ΪECB</param>
        /// <returns></returns>
        public static string AESTrans2Base64Decrypt(string text, string password, string iv, Encoding encode, bool bUseForOtherPlatform = true)
        {
            try
            {
                if (null == encode)
                {
                    encode = Encode;
                }
                RijndaelManaged rijndaelCipher = new RijndaelManaged();

                if (bUseForOtherPlatform)
                {
                    rijndaelCipher.Mode = CipherMode.ECB;
                    rijndaelCipher.Padding = PaddingMode.PKCS7;
                }
                else
                {
                    rijndaelCipher.Mode = CipherMode.CBC;
                    rijndaelCipher.Padding = PaddingMode.PKCS7;
                }

                rijndaelCipher.KeySize = 128;
                rijndaelCipher.BlockSize = 128;
                byte[] encryptedData = Convert.FromBase64String(text);
                byte[] pwdBytes = encode.GetBytes(password);
                byte[] keyBytes = new byte[16];
                int len = pwdBytes.Length;

                if (len > keyBytes.Length) len = keyBytes.Length;

                System.Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;

                byte[] ivBytes = encode.GetBytes(iv);
                rijndaelCipher.IV = ivBytes;
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
           
                return encode.GetString(plainText);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


#region  CA���� 


        /// <summary>     
        /// ���ݹ�Կ֤�飬����֤��ʵ��     
        /// </summary>     
        /// <param name="cerPath"></param>     
        public static X509Certificate2 GetCertFromCerFile(string cerPath)
        {
            try
            {
                return new X509Certificate2(cerPath);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>     
        /// ����˽Կ֤��õ�֤��ʵ�壬�õ�ʵ�����Ը����乫Կ��˽Կ���мӽ���     
        /// �ӽ��ܺ���ʹ��DEncrypt��RSACryption��     
        /// </summary>     
        /// <param name="pfxFileName"></param>     
        /// <param name="password"></param>     
        /// <returns></returns>     
        public static X509Certificate2 GetCertificateFromPfxFile(string pfxFileName,
            string password)
        {
            try
            {
                return new X509Certificate2(pfxFileName, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            }
            catch (Exception e)
            {
                return null;
            }
        }     

        public static string CAEncry(string encryContent, string caFilePath)
        {
            caFilePath = @"F:\aisino\1 Tech And Train\3 Program Code\21 ��ƱHttp �ӿ�Demo\51fapiao.cer";

            X509Certificate clientCert = new X509Certificate(caFilePath);



           string parameterString =  clientCert.GetKeyAlgorithmParametersString();
           byte[] cerByte  = clientCert.Export(X509ContentType.Cert);

           byte[] pu3k = clientCert.GetPublicKey();

           using (RSACryptoServiceProvider rsa333 = new RSACryptoServiceProvider())
           {
               
               //rsa333.(cerByte);
           };

           using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
           {
               int len = rsa.KeySize;
  
              byte[]  keyArray= new byte[((0 == pu3k.Length % 2)? pu3k.Length: pu3k.Length -1)];

               for(int i = 0; i< keyArray.Length; ++i)
               {
                   keyArray[i] = pu3k[i];
               }

               RSAParameters rsaParams = new RSAParameters()
               {
                   Exponent = new byte[] { 1, 0, 1 },
                   Modulus = keyArray,           
               };
               rsa.ImportParameters(rsaParams);

               byte[] encrypted = rsa.Encrypt(Encoding.UTF8.GetBytes("chenhong"), false);
               string encryContent2 = Convert.ToBase64String(encrypted);
               return encryContent2;
           }

            byte[] publickeyByteArray = clientCert.GetPublicKey();
            string stringpublicKey = ByteArrayToString(publickeyByteArray);

            string szhtxx = clientCert.GetPublicKeyString();

            string dfdf = "0fJGr7wccEU8r6Ciqrv4y8xGUB1PokvUtiw2mzj5oNty7tx0nezpH4xp/v2WOhZUXeHEW7mFh5eTmxPUVZAkQxwfiPCX59eHFol3me5GnhZcT74HtMS6SIM6HUDgBewdLvqUvWUove/SGIxU7Ioyi8XFcIAiMxdewGZEi5fx2P0=";

            byte[] lelekf = Convert.FromBase64String(dfdf);
            string dfdf1 = UTF8Encoding.UTF8.GetString(Convert.FromBase64String(dfdf));
            string dfdf4 = Encoding.Unicode.GetString(Convert.FromBase64String(dfdf));
            string dfdf2 = Encoding.Default.GetString(Convert.FromBase64String(dfdf));
            string dfdf23 = Encoding.ASCII.GetString(Convert.FromBase64String(dfdf));

            string dfewe45 = new UnicodeEncoding().GetString(Convert.FromBase64String(dfdf));


            string encryContentDetail = RSAEncryptWithByteKey(clientCert.GetRawCertData(), encryContent);
           string xmlPrivate = string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", "5875767687");

            string decryptContentDetail = RSADecrypt2(xmlPrivate, encryContentDetail);

           return decryptContentDetail;

        }

        /// <summary>     
        /// RSA����     
        /// </summary>     
        /// <param name="xmlPublicKey"></param>     
        /// <param name="m_strEncryptString"></param>     
        /// <returns></returns>     
        static string RSAEncryptWithByteKey(byte[] byteKey, string m_strEncryptString)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                string dfd = Convert.ToBase64String(byteKey, 0, byteKey.Length);
                string publickey = string.Format(@"<RSAKeyValue><Modulus>v6if+Ou5EI2oZoiGsA31gbgIaaay0fP2kvfBxYZuO2KKpZXtJ5C/plDU9gxQbG7eagWh/LoN1luXA8RXl3gmfnsrFZe9vVVcjBBVmRjcTp3Mq7tA+X71XGGWS+WN7+s2XG8Vqvsb9KVZNUbLXrRQFsztGl1j+tGg3slikKKxv/U=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
                provider.ImportCspBlob(byteKey);
                byte[] bytes = Encoding.UTF8.GetBytes(m_strEncryptString);
                return Convert.ToBase64String(provider.Encrypt(bytes, false));
            }
        }

        

 

        /// <summary>     
        /// RSA����     
        /// </summary>     
        /// <param name="xmlPublicKey"></param>     
        /// <param name="m_strEncryptString"></param>     
        /// <returns></returns>     
        static string RSADecryptWithByteKey(byte[] byteKey, string m_strEncryptString)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {

                provider.ImportCspBlob(byteKey);
                byte[] bytes =  Encoding.UTF8.GetBytes(m_strEncryptString);
                return Convert.ToBase64String(provider.Encrypt(bytes, false));
            }
        }

        /// <summary>     
        /// RSA����     
        /// </summary>     
        /// <param name="xmlPublicKey"></param>     
        /// <param name="m_strEncryptString"></param>     
        /// <returns></returns>     
        static string RSAEncrypt2(string xmlPublicKey, string m_strEncryptString)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlPublicKey);
            byte[] bytes = new UnicodeEncoding().GetBytes(m_strEncryptString);
            return Convert.ToBase64String(provider.Encrypt(bytes, false));
        }    


        /// <summary>
        ///  RSA���� 
        /// </summary>
        /// <param name="xmlPrivateKey"></param>
        /// <param name="m_strDecryptString"></param>
        /// <returns></returns>
        static string RSADecrypt2(string xmlPrivateKey, string m_strDecryptString)
        {
            string rt = string.Empty;

            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(xmlPrivateKey);
                byte[] rgb = Convert.FromBase64String(m_strDecryptString);
                byte[] bytes = provider.Decrypt(rgb, false);
                return new UnicodeEncoding().GetString(bytes);
            }
        }


  
#endregion

#endregion

    }
}