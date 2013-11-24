using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BCD.Utility
{
    class StreamAndByteTransfer
    {
        public byte[] ReadFile(string fileName)
        {

            FileStream pFileStream = null;

            byte[] pReadByte = new byte[0];

            try
            {

                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                BinaryReader r = new BinaryReader(pFileStream);

                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开

                pReadByte = r.ReadBytes((int)r.BaseStream.Length);

                return pReadByte;

            }

            catch
            {

                return pReadByte;

            }

            finally
            {

                if (pFileStream != null)

                    pFileStream.Close();

            }

        }

        //写byte[]到fileName

        private bool writeFile(byte[] pReadByte, string fileName)
        {

            FileStream pFileStream = null;



            try
            {

                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);

                pFileStream.Write(pReadByte, 0, pReadByte.Length);



            }

            catch
            {

                return false;

            }

            finally
            {

                if (pFileStream != null)

                    pFileStream.Close();

            }

            return true;

        }

    }
}
