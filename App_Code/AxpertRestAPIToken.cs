
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public class AxpertRestAPIToken
{
    public string seed = string.Empty;
    public string token = string.Empty;
    public string userAuthKey = string.Empty;
    private const string KeyForAxpertToken = "202308231803";

    public AxpertRestAPIToken(string userName)
    {
        GenerateAxpertToken();
        userAuthKey = AxpertEncryptString(userName);
    }

    public bool ValidateAxpertToken(string validateToken, string validateSeed)
    {
        string secretKey = KeyForAxpertToken;
        string strPlain = validateSeed + secretKey + validateSeed;
        return MD5Hash(strPlain) == validateToken;
    }


    private void GenerateAxpertToken()
    {
        string secretKey = KeyForAxpertToken;
        Random random = new Random();
        seed = random.Next(100000, 1000000).ToString("D6");
        string strPlain = seed + secretKey + seed;
        token = MD5Hash(strPlain);
    }

    private string MD5Hash(string inputString)
    {
        string hashString = "";
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                stringBuilder.Append(hashBytes[i].ToString("x2"));
            }

            hashString = stringBuilder.ToString();
        }
        return hashString;
    }

    public static string AxpertEncryptString(string str)
    {
        string enstr = string.Empty;
        string insid = str;
        string dtid = GetTimeId();

        int i = dtid.Length;
        string s = dtid.Substring(0, dtid.Length - 4);

        insid = AxpertEncodeString(s, insid);
        enstr = insid + dtid;
        return enstr;
    }

    private static string GetTimeId()
    {
        string res = string.Empty;
        string s = string.Empty, s1 = string.Empty;
        int i;
        string dtime = "01020345060708";
        i = int.Parse(dtime.Substring(0, 2));
        s = (i + 31).ToString();
        s = s + (int.Parse(dtime.Substring(2, 2)) + i + 13);
        s = s + (int.Parse(dtime.Substring(4, 4)) * i);
        s = s + dtime.Substring(8, 2) + dtime.Substring(10, 2) + dtime.Substring(12, 2);
        i = s.Length;
        s1 = "00" + i;
        res = s + s1;
        return res;
    }

    private static string AxpertEncodeString(string dtid, string dbid)
    {
        string Result = string.Empty;
        int len = dbid.Length;
        int len1 = dtid.Length;
        string s = string.Empty, s1 = string.Empty;
        if (len1 < len)
        {
            for (int i = len1; i < len; i++)
            {
                dtid = dtid + '0';
            }
        }
        ArrayList arr = new ArrayList();
        for (int i = 0; i < len; i++)
        {
            arr.Add((Encoding.ASCII.GetBytes(dbid[i].ToString())[0] + dtid[i]));
        }

        for (int i = 0; i < arr.Count; i++)
        {
            if (arr[i].ToString().Length == 4)
                s1 = s1 + arr[i];
            else if (arr[i].ToString().Length == 3)
                s1 = s1 + ("0" + arr[i]);
            else if (arr[i].ToString().Length == 2)
                s1 = s1 + ("00" + arr[i]);
        }
        int i1 = dbid.Length;
        if (i1.ToString().Length == 1)
            s = "000" + i1;
        else if (i1.ToString().Length == 2)
            s = "00" + i1;
        else if (i1.ToString().Length == 3)
            s = "0" + i1;
        else if (i1.ToString().Length == 4)
            s = i1.ToString();
        Result = s + s1;
        return Result;
    }
}
