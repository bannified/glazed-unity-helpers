public static class DataUtility {

    public static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    public static string EncodeBase64(this System.Text.Encoding encoding, string text)
    {
        if (text == null)
        {
            return null;
        }

        byte[] textAsBytes = encoding.GetBytes(text);
        return System.Convert.ToBase64String(textAsBytes);
    }

    public static string DecodeBase64(this System.Text.Encoding encoding, string encodedText)
    {
        if (encodedText == null)
        {
            return null;
        }

        byte[] textAsBytes = System.Convert.FromBase64String(encodedText);
        return encoding.GetString(textAsBytes);
    }
}
