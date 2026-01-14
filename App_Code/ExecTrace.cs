using System;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for ExecTrace
/// </summary>
[Serializable()]
public class ExecTrace
{
    private static readonly ExecTrace instance = new ExecTrace();
    public DateTime callReceivedTime;
    public DateTime callProcessReceivedTime;
    public string serverElapsTime = string.Empty;

    public DateTime callReceivedTimeMethod;
    public string serverElapsTimeMethod = string.Empty;
    public static ExecTrace Instance
    {
        get
        {
            return instance;
        }
    }

    public ExecTrace()
    {

    }

    public string RequestProcessTime(string ProcessType, string strResponse = "")
    {
        string ExecutionLogText = string.Empty;
        try
        {
            DateTime currentTime = DateTime.Now;
            string rptTime = currentTime.Subtract(callProcessReceivedTime).TotalMilliseconds.ToString();
            callProcessReceivedTime = currentTime;
            ExecutionLogText += ProcessType + " processed at server " + string.Format("{0:F4}", rptTime) + " ms ";
            if (strResponse != "")
            {
                double responseSize = Math.Round(Encoding.UTF8.GetByteCount(strResponse) / 1024f, 3);
                ExecutionLogText += ", Response size " + responseSize + " KB ";
            }
            ExecutionLogText += " ♦ ";
        }
        catch (Exception ex) { }
        return ExecutionLogText;
    }

    public string WireElapsTime(string elapsTime, string st = "", string strWire = "")
    {
        string ExecutionLogText = string.Empty;
        try
        {
            if (st == "")
                st = HttpContext.Current.Session["etServerTime"].ToString();
            if (strWire != "")
            {
                double wireSize = Math.Round(Encoding.UTF8.GetByteCount(strWire) / 1024f, 3);
                ExecutionLogText = "Request size, " + wireSize + " KB ♦ ";
            }

            DateTime ct = DateTime.Now;
            callReceivedTime = ct;
            callProcessReceivedTime = ct;

            string wireElapsTime = ct.Subtract(DateTime.Parse(st)).TotalMilliseconds.ToString();
            float wireElTime = (float.Parse(wireElapsTime) - float.Parse(elapsTime));

            serverElapsTime = wireElTime.ToString();
            ExecutionLogText += "Request received at server, wire transfer time " + string.Format("{0:F4}", wireElTime) + " ms ♦ ";
        }
        catch (Exception ex) { }
        return ExecutionLogText;
    }

    public string TotalServerElapsTime()
    {
        string totElapsTime = string.Empty;
        try
        {
            DateTime currentTime = DateTime.Now;
            totElapsTime = currentTime.Subtract(callReceivedTime).TotalMilliseconds.ToString();
            totElapsTime = (float.Parse(totElapsTime) + float.Parse(serverElapsTime)).ToString();
        }
        catch (Exception ex) { }
        return totElapsTime;
    }

    public string KernelProcessTime(DateTime kst, string processName, string strInp = "", string strOtp = "")
    {
        string ExecutionLogText = string.Empty;
        try
        {
            string inputSize = string.Empty, outputSize = string.Empty;
            if (strInp != "")
            {
                double inpSize = Math.Round(Encoding.UTF8.GetByteCount(strInp) / 1024f, 3);
                inputSize = "input data size " + inpSize + " KB, ";
            }
            if (strOtp != "")
            {
                double otpSize = Math.Round(Encoding.UTF8.GetByteCount(strOtp) / 1024f, 3);
                outputSize = ", result data size " + otpSize + " KB ";
            }

            DateTime currentTime = DateTime.Now;
            callProcessReceivedTime = currentTime;
            ExecutionLogText = processName + " kernel " + inputSize + "process time " + string.Format("{0:F4}", currentTime.Subtract(kst).TotalMilliseconds.ToString()) + " ms" + outputSize + " ♦ ";
        }
        catch (Exception ex) { }
        return ExecutionLogText;
    }

    public void SetCurrentTime()
    {
        DateTime ct = DateTime.Now;
        callReceivedTime = ct;
        callProcessReceivedTime = ct;
        serverElapsTime = "0";
    }

    public string ResponseErrorMsg(string strError)
    {
        string ErrorLogText = string.Empty;
        ErrorLogText = strError + " ♦ ";
        return ErrorLogText;
    }

    public void SetCurrentTimeMethod()
    {
        DateTime ct = DateTime.Now;
        callReceivedTimeMethod = ct;
        serverElapsTimeMethod = "0";
    }

    public string TotalServerElapsTimeMethod()
    {
        string totElapsTime = string.Empty;
        try
        {
            DateTime currentTime = DateTime.Now;
            totElapsTime = currentTime.Subtract(callReceivedTimeMethod).TotalMilliseconds.ToString();
            totElapsTime = (float.Parse(totElapsTime) + float.Parse(serverElapsTimeMethod)).ToString();
        }
        catch (Exception ex) { }
        return totElapsTime;
    }

    //public long GetRequestSize(object TestObject)
    //{
    //    //BinaryFormatter bf = new BinaryFormatter();
    //    //MemoryStream ms = new MemoryStream();
    //    //byte[] Array;
    //    //bf.Serialize(ms, TestObject);
    //    //Array = ms.ToArray();
    //    //return Array.Length;


    //    long size = 0;
    //    //object o = new object();
    //    using (Stream s = new MemoryStream())
    //    {
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        formatter.Serialize(s, TestObject);
    //        size = s.Length;
    //    }
    //    return size;


    //    //object Value = null;
    //    //int size = 0;
    //    //Type type = TestObject.GetType();
    //    //PropertyInfo[] info = type.GetProperties();
    //    //foreach (PropertyInfo property in info)
    //    //{
    //    //    Value = property.GetValue(TestObject, null);
    //    //    unsafe
    //    //    {
    //    //        size += sizeof(Value);
    //    //    }
    //    //}
    //    //return size;
    //}

    //public static int GetSizeOfObject(object obj)
    //{
    //    object Value = null;
    //    int size = 0;
    //    Type type = obj.GetType();
    //    PropertyInfo[] info = type.GetProperties();
    //    foreach (PropertyInfo property in info)
    //    {
    //        Value = property.GetValue(obj, null);
    //        unsafe
    //        {
    //            size += sizeof(Value);
    //        }
    //    }
    //    return size;
    //}

    //public void GetRequestSize(object TestObject)
    //{
    //    int isize = Marshal.SizeOf(TestObject);
    //}
}

