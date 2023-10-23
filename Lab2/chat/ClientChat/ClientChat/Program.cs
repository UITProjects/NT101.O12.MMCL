// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;

Socket client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
client_socket.Connect("localhost", 2509);
String[] clients;
String client_name;
Dictionary<String,String> clients_publickey = new Dictionary<String,String>();
RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
void send(Dictionary<String,String> message_dict)

{
    String message_json_String = JsonConvert.SerializeObject(message_dict);
    byte[] message_bytes = Encoding.UTF8.GetBytes(message_json_String);
    int header_message_int = message_bytes.Length;
    byte[] header_message_bytes = BitConverter.GetBytes(header_message_int);
    client_socket.Send(header_message_bytes);
    client_socket.Send(message_bytes);
}
void listen(bool onetime = false)
{
    byte[] header_bytes = new byte[4];
    if (onetime)
    {
        client_socket.Receive(header_bytes);
        int header_int = BitConverter.ToInt32(header_bytes, 0);
        byte[] message_bytes = new byte[header_int];
        int byte_read = 0;
        while (byte_read < message_bytes.Length)
        {
            byte_read += client_socket.Receive(message_bytes);
        }
        String message_String = Encoding.UTF8.GetString(message_bytes);
        Dictionary<string, Object> message_dict = JsonConvert.DeserializeObject<Dictionary<String, Object>>(message_String);
        Console.WriteLine("Server response: " + message_dict["status"]);
        Console.WriteLine(message_String);
    }
    else
    {
        while (true)
        {
            client_socket.Receive(header_bytes);
            int header_int = BitConverter.ToInt32(header_bytes, 0);
            byte[] message_bytes = new byte[header_int];
            int byte_read = 0;
            while (byte_read < message_bytes.Length)
            {
                byte_read += client_socket.Receive(message_bytes);
            }
            String message_String = Encoding.UTF8.GetString(message_bytes);
            Dictionary<string, Object> message_dict = JsonConvert.DeserializeObject<Dictionary<String, Object>>(message_String);
            if (message_dict["type"].ToString() == "forwarded")
            {
               String decypted_message_String= Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String((string)message_dict["message"]),true));
                Console.WriteLine("Message from "+ message_dict["from"].ToString() + ": "+decypted_message_String);
            }
             else if (message_dict["type"].ToString() == "recipients_publickey")
            {
                String clients_object = message_dict["clients"].ToString();
                clients_object = clients_object.Replace('{', ' ');
                clients_object = clients_object.Replace('}', ' ');
                clients_object = clients_object.Replace('"', ' ');
                clients_object = clients_object.Trim();
                char[] chars = { ',' };
                clients = clients_object.Split(chars);
                for (int i = 0; i < clients.Length; i++)
                {
                    String client_username = clients[i].Split(':')[0].Trim();
                    String client_publickey = clients[i].Split(":")[1].Trim();
                    try
                    {
                        clients_publickey.Add(client_username, client_publickey);
                    }catch (ArgumentException) {
                        continue;
                    }
                }

            }
        }
    }
}
void register(String username)
{
    Dictionary<String,String> message_dict = new Dictionary<String,String>();
    message_dict.Add("username", username);
    message_dict.Add("type", "register");
    message_dict.Add("public_key", Convert.ToBase64String(rsa.ExportRSAPublicKey()));
    send(message_dict);
    client_name = username;
    listen(true);
}
void get_client()
{

    Dictionary<String, String> message_dict = new Dictionary<String, String>();
    message_dict.Add("type", "get_clients");
    send(message_dict);
}

void chat(String message_String,String to_recipient)
{
    RSACryptoServiceProvider rsa_recipient = new RSACryptoServiceProvider();
    rsa_recipient.ImportRSAPublicKey(Convert.FromBase64String(clients_publickey[to_recipient]),out _);
    Dictionary<String ,String> message_dict = new Dictionary<String ,String>();
    message_dict.Add("type", "forward");
    message_dict.Add ("from", client_name);
    message_dict.Add("message", Convert.ToBase64String(rsa_recipient.Encrypt(Encoding.UTF8.GetBytes(message_String),true)));
    message_dict.Add("to_recipient", to_recipient);
    send(message_dict);
}
Console.WriteLine("register username:");
register(Console.ReadLine());
Thread listen_Thread = new Thread(() => listen());

void test()
{
    listen_Thread.IsBackground = true;
    listen_Thread.Start();
    Console.WriteLine("1: get_clients");
    Console.WriteLine("2: chat");
    while (true)
    {
        String func = Console.ReadLine();
        if (func == "1")
        {
            get_client();
            Thread.Sleep(300);
            List<String> clients_username = new List<String>(clients_publickey.Keys);
            for (int i = 0; i < clients_username.Count; i++)
                Console.WriteLine(clients_username[i]);
        }
        else if (func == "2")
        {
            Console.WriteLine("chat, nhap theo thu tu: message,to");
            chat(Console.ReadLine(), Console.ReadLine());
        }
        else
            break;
    }
    
}
test();

Console.ReadKey();

