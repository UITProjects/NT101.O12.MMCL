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
bool test_boolean = true;
byte[] encrypt_bytes = new byte[1];
encrypt_bytes[0] = Convert.ToByte(test_boolean);
Dictionary<String, String> header_message = new Dictionary<string, string>();
header_message.Add("type", "forward");
header_message.Add("from", "test");
header_message.Add("to_recipient", "test2");
String Header_message_json_String = JsonConvert.SerializeObject(header_message);
byte[] header_message_bytes = Encoding.UTF8.GetBytes(Header_message_json_String);
byte[] header_message_length = new byte[4];
header_message_length = BitConverter.GetBytes(header_message_bytes.Length);
client_socket.Send(encrypt_bytes);
client_socket.Send(header_message_length); 
Console.WriteLine(header_message_bytes.Length);
Console.ReadKey();

