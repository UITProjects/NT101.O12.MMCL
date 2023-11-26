using System.Formats.Asn1;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Formats.Asn1;
using System.Reflection;

static void GetCertificateFromWebsite(string url)
{
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    request.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

    response.Close();
}
static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
{
    String root_file = Directory.GetCurrentDirectory()+"\\";
    int number = 0;
    foreach (var cer in chain.ChainElements)
    {
        String file = root_file+ number.ToString();
        Console.WriteLine(file);
        number++;

        using(FileStream fs = File.Create(file+".cer"))
        {
           
            fs.Write(cer.Certificate.Export(X509ContentType.Cert));
        }

    }
    return true;
}






String[] current_directory = Directory.GetFiles(Directory.GetCurrentDirectory(),"*.cer");
Console.WriteLine(String.Format("\nThis url have {0} cer in chains, continue to verify from bot to root?: ",(current_directory.Length)));

for(int i = 0; i< current_directory.Length - 1; i++)
{
    byte[] current_cert_bytes;
    byte[] previous_cert_bytes;
    using (FileStream fs = File.OpenRead(current_directory[i]))
    {
        current_cert_bytes = new byte[fs.Length];
        int byte_read =   fs.Read(current_cert_bytes, 0, current_cert_bytes.Length);
    }
    using (FileStream fs = File.OpenRead(current_directory[i + 1]))
    {
        previous_cert_bytes = new byte[fs.Length];
        fs.Read(previous_cert_bytes, 0, previous_cert_bytes.Length);
    }

    X509Certificate current_cert = new X509Certificate(current_cert_bytes);
    byte[] current_pubkey_bytes = current_cert.GetPublicKey();
    byte[] current_cert_only_bytes  = current_cert.Export(X509ContentType.Cert);
    Console.ReadKey ();

}


