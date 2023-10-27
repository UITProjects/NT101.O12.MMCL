using Newtonsoft.Json;
using System.Buffers.Text;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ClientChat_UI
{
    public partial class client_form : Form
    {
        Socket client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        String[] clients;
        String client_name;
        Dictionary<String, String> clients_publickey = new Dictionary<String, String>();
        RSACryptoServiceProvider rsa_client = new RSACryptoServiceProvider(4096);
        Thread listen_Thread;
        Thread get_client_Thread;
        RSACryptoServiceProvider rsa_server = new RSACryptoServiceProvider();
        Boolean server_encrypt = false;
        AesCryptoServiceProvider aes_client = new AesCryptoServiceProvider();
        public client_form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client_socket.Connect("localhost", 2509);
            listen_Thread = new Thread(() => listen());
            listen_Thread.IsBackground = true;
            get_client_Thread = new Thread(() => get_client());
            get_client_Thread.IsBackground = true;
            exchange_publickey();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            client_socket.Shutdown(SocketShutdown.Both);
            client_socket.Close();
        }
        void send(Dictionary<String, String> header_dict,String message)

        {
            String header_String = JsonConvert.SerializeObject(header_dict);
            byte[] header_content_bytes = Encoding.UTF8.GetBytes(header_String);
            byte[] message_bytes = Encoding.UTF8.GetBytes(message);
            byte[] encrypt_bytes = new byte[1];

            if (server_encrypt)
            {
                header_content_bytes = rsa_server.Encrypt(header_content_bytes, RSAEncryptionPadding.Pkcs1);
                message_bytes = rsa_server.Encrypt(message_bytes, RSAEncryptionPadding.Pkcs1);
                encrypt_bytes[0] = Convert.ToByte(server_encrypt);
            }
            else
                encrypt_bytes[0] = Convert.ToByte(server_encrypt);

            byte[] header_length_bytes = BitConverter.GetBytes(header_content_bytes.Length);
            byte[] message_length_bytes = BitConverter.GetBytes(message_bytes.Length);

            client_socket.Send(encrypt_bytes);
            client_socket.Send(header_length_bytes);
            client_socket.Send(header_content_bytes);
            client_socket.Send(message_length_bytes);
            client_socket.Send(message_bytes);
        }
        void listen(bool onetime = false)
        {
            byte[] encrypt_bytes = new byte[1];


            byte[] header_length_bytes = new byte[4];
            byte[] header_content_bytes;
            String header_content_String;


            byte[] message_length_bytes = new byte[4];
            byte[] message_bytes;
            String message_String;



            while (true)
                {
                    try
                    {
                        client_socket.Receive(encrypt_bytes);
                        client_socket.Receive(header_length_bytes);
                        header_content_bytes = new byte[BitConverter.ToInt32(header_length_bytes)];
                        client_socket.Receive(header_content_bytes);
                        client_socket.Receive(message_length_bytes);
                        message_bytes = new byte[BitConverter.ToInt32(message_length_bytes)] ;
                        client_socket.Receive(message_bytes);
                     
                    }
                    catch (SocketException ex)
                    {
                        break;
                    }


                if (Convert.ToBoolean(encrypt_bytes[0])) {
                    MessageBox.Show("true");
                    continue;
                }
                else
                {
                    MessageBox.Show("false");
                    message_String = Encoding.UTF8.GetString(message_bytes);
                    header_content_String = Encoding.UTF8.GetString(header_content_bytes);

                }

                    Dictionary<string, Object> header_dict = JsonConvert.DeserializeObject<Dictionary<String, Object>>(header_content_String);
                    if (header_dict["type"].ToString() == "forwarded")
                    {
                        String decypted_message_String = Encoding.UTF8.GetString(rsa_client.Decrypt(Convert.FromBase64String((string)header_dict["message"]), true));
                        chat_listbox.Invoke(new Action(() =>
                        {
                            chat_listbox.Items.Add(header_dict["from"].ToString() + ": " + decypted_message_String);
                        }));
                        Console.WriteLine("Message from " + header_dict["from"].ToString() + ": " + decypted_message_String);
                    }
                    else if (header_dict["type"].ToString() == "recipients_publickey")
                    {


                        String clients_object = header_dict["clients"].ToString();
                        clients_object = clients_object.Replace('{', ' ');
                        clients_object = clients_object.Replace('}', ' ');
                        clients_object = clients_object.Replace('"', ' ');
                        clients_object = clients_object.Trim();
                        char[] chars = { ',' };
                        clients = clients_object.Split(chars);
                        Dictionary<String, String> clients_publickey_temp = new Dictionary<String, String>();
                        for (int i = 0; i < clients.Length; i++)
                        {
                            String client_username = clients[i].Split(':')[0].Trim();
                            String client_publickey = clients[i].Split(":")[1].Trim();
                            clients_publickey_temp.Add(client_username, client_publickey);

                        }


                        if (clients_publickey.Count == clients_publickey_temp.Count)
                        {
                            continue;
                        }
                        else
                        {
                            clients_publickey = clients_publickey_temp;
                            clients_listview.Invoke(new Action(() =>
                            {
                                clients_listview.Items.Clear();

                                List<String> clients_username = new List<String>(clients_publickey.Keys);
                                for (int i = 0; i < clients_username.Count; i++)
                                {
                                  //  if (clients_username[i] == "null" || clients_username[i] == client_name)
                                       // continue;
                                    clients_listview.Items.Add(clients_username[i]);
                                }

                            }));
                        }


                    }else if(header_dict["type"].ToString() == "exchange_publickey_response")
                    {
                        String server_publickey = header_dict["server_publickey"].ToString();
                        rsa_server = new RSACryptoServiceProvider();
                        rsa_server.ImportRSAPublicKey(Convert.FromBase64String(server_publickey),out _);
                        server_encrypt = true;
                    MessageBox.Show("init connection: Ok");
                    }
                    else if (header_dict["type"].ToString()== "register")
                    {
                        if (header_dict["status"].ToString() == "OK")
                        {
                            MessageBox.Show("Đăng ký thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                
            }
        }
        void register(String username)
        {
            Dictionary<String, String> message_dict = new Dictionary<String, String>();
            message_dict.Add("username", username);
            message_dict.Add("type", "register");
            
            send(message_dict,"");
            client_name = username;
        }
        

        void get_client()
        {
            while (true)
            {
                Thread.Sleep(500);
                Dictionary<String, String> message_dict = new Dictionary<String, String>();
                message_dict.Add("type", "get_clients");
                send(message_dict, "hello");
                break;
            }
        }

        void chat(String message_String, String to_recipient)
        {
            RSACryptoServiceProvider rsa_recipient = new RSACryptoServiceProvider();
            rsa_recipient.ImportRSAPublicKey(Convert.FromBase64String(clients_publickey[to_recipient]),out _);
            Dictionary<String, String> message_dict = new Dictionary<String, String>();
            message_dict.Add("type", "forward");
            message_dict.Add("from", client_name);
            message_dict.Add("message", Convert.ToBase64String(rsa_recipient.Encrypt(Encoding.UTF8.GetBytes(message_String), true)));
            message_dict.Add("to_recipient", to_recipient);
            MessageBox.Show("length of message: " + message_String.Length.ToString());
            send(message_dict, "hello" );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String recipient_String = clients_listview.SelectedItem.ToString();
            chat(message_textbox.Text, recipient_String);

        }

        private void register_btn_Click(object sender, EventArgs e)
        {
            register(username_textbox.Text);
            register_btn.Enabled = false;
            username_textbox.Enabled = false;
            get_client_Thread.Start();
        }
        void exchange_publickey()
        {
            listen_Thread.Start();
            Dictionary<String, String> message_dict = new Dictionary<String, String>();
            message_dict.Add("machine_name", Environment.MachineName);
            message_dict.Add("type", "exchange_publickey");
            message_dict.Add("client_publickey",Convert.ToBase64String(rsa_client.ExportRSAPublicKey()));
            send(message_dict,"hello");
        }
    }
}