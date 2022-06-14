using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;
namespace KMASafeKidGUI
{
    class PipeClient
    {
        private enum fText
        {
            CloseApp = 0,
            WriteReg,
            GetUser,
        }
        static String PipeName = "KMASKPipe";
        public static bool SendSettingChange()
        {
            //string sSend;
            //var pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut, PipeOptions.Asynchronous | PipeOptions.WriteThrough);
            //pipeClient.Connect();

            //var ss = new StreamString(pipeClient);

            //sSend = String.Format("{{'flag':{0}, 'idproc':{1}}}", ((int)fText.CloseApp), idProc);//"{'flag':" + fText.CloseApp + ",'idproc':" + idProc +"}";
            //Console.WriteLine(sSend);

            //ss.WriteString(sSend);
            //// Write Registry
            //sSend = String.Format("{{'flag':{0}, 'valueName':'{1}', 'value':'{2}'}}", ((int)fText.WriteReg), "Testlai", "deobiet");
            //Console.WriteLine(sSend);
            //ss.WriteString(sSend);


            ////Console.WriteLine(ss.ReadString());

            //Console.ReadKey();
            //pipeClient.Close();
            return true;
        }
 
    }
    public class StreamString
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public string ReadString()
        {
            int len;
            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            var inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();

            return outBuffer.Length + 2;
        }
    }
}
