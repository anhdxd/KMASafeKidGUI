using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace KMASafeGUI
{
    class PipeClient
    {
        public static int FlagSend { get; set; }
        public static bool bResult;
        public static string StringSend { get; set; }
        public static EventWaitHandle signal = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static readonly string PipeName = "KMASKPipe";
        public static NamedPipeClientStream pipeClient;
        public enum fText
        {
            AddHostToDB = 0,
            AddAppToDB,
            DeleteHostDB,
            DeleteAppDB,
            ChangeSetting,
            OpenGUIAdmin,
    }

        public static void OninitPipes()
        {
            Thread threadSetting = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut, PipeOptions.WriteThrough | PipeOptions.Asynchronous);
                        pipeClient.Connect();

                        SendSettingChange();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            });
            threadSetting.IsBackground = true;
            threadSetting.Start();
        }
        public static void SendSettingChange()
        {
            while (true)
            {
                signal.WaitOne();

                string sSend;
                StreamString ss = new StreamString(pipeClient);
                switch (FlagSend)
                {
                    case (int)fText.AddHostToDB:
                        sSend = string.Format("{{'flag':{0},'sDomain':'{1}'}}", (int)fText.AddHostToDB, StringSend);
                        ss.WriteString(sSend);
                        Thread.Sleep(100);
                        if (pipeClient.CanRead)
                            bResult = bool.Parse(ss.ReadString());
                        else bResult = false;
                        AddBlockWindow.signalWaitResult.Set();
                        break;

                    case (int)fText.AddAppToDB:
                        sSend = string.Format("{{'flag':{0},'sPath':'{1}'}}", (int)fText.AddAppToDB, StringSend.Replace("\\","\\\\"));
                        ss.WriteString(sSend);
                        Thread.Sleep(100);
                        if (pipeClient.CanRead)
                            bResult = bool.Parse(ss.ReadString());
                        else bResult = false;
                        AddBlockWindow.signalWaitResult.Set();
                        break;

                    case (int)fText.DeleteHostDB:
                        sSend = string.Format("{{'flag':{0},'sPath':'{1}'}}", (int)fText.DeleteHostDB, StringSend);
                        int res = ss.WriteString(sSend);
                        Thread.Sleep(100);
                        if (pipeClient.CanRead)
                            bResult = bool.Parse(ss.ReadString());
                        else bResult = false;
                        AddBlockWindow.signalWaitResult.Set();
                        break;
                    case (int)fText.DeleteAppDB:
                        sSend = string.Format("{{'flag':{0},'sPath':'{1}'}}", (int)fText.DeleteAppDB, StringSend);
                        ss.WriteString(sSend);
                        Thread.Sleep(100);
                        if (pipeClient.CanRead)
                            bResult = bool.Parse(ss.ReadString());
                        else bResult = false;
                        AddBlockWindow.signalWaitResult.Set();
                        break;
                    case (int)fText.ChangeSetting:
                        //sSend = string.Format("{{'flag':{0}}}", (int)fText.ChangeSetting);
                        ss.WriteString(StringSend);
                        break;
                    case (int)fText.OpenGUIAdmin:
                        sSend = string.Format("{{'flag':{0},'sPath':'{1}'}}", (int)fText.OpenGUIAdmin, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Replace("\\","\\\\"));
                        ss.WriteString(sSend);
                        MainWindow.signalWaitResult.Set();
                        break;
                    default:
                        break;
                }
                StringSend = "";
            }
            //Console.WriteLine(sSend);


            //// Write Registry
            //sSend = String.Format("{{'flag':{0}, 'valueName':'{1}', 'value':'{2}'}}", ((int)fText.WriteReg), "Testlai", "deobiet");
            //Console.WriteLine(sSend);
            //ss.WriteString(sSend);


            ////Console.WriteLine(ss.ReadString());

            //Console.ReadKey();
            //pipeClient.Close();
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
