using System.Formats.Asn1;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Formats.Asn1;
using System.Reflection;
class Download_Cert
{
    static void GetCertificateFromWebsite(string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        response.Close();
    }
    static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        String root_file = Directory.GetCurrentDirectory() + "\\";
        int number = 0;
        foreach (var cer in chain.ChainElements)
        {
            String file = root_file + number.ToString();
            Console.WriteLine(file);
            number++;

            using (FileStream fs = File.Create(file + ".cer"))
            {

                fs.Write(cer.Certificate.Export(X509ContentType.Cert));
            }

        }
        return true;
    }
    static void Main(string[] args)
    {
        GetCertificateFromWebsite(args[0]);
        Console.WriteLine("Download Ok");
    }
}














