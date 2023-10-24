using Newtonsoft.Json;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ClientChat_UI
{
    public partial class client_form : Form
    {
        Socket client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        String[] clients;
        String client_name;
        Dictionary<String, String> clients_publickey = new Dictionary<String, String>();
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        Thread listen_Thread;
        Thread get_client_Thread;
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
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            client_socket.Shutdown(SocketShutdown.Both);
            client_socket.Close();
        }
        void send(Dictionary<String, String> message_dict)

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
                if (message_dict["status"].ToString() == "OK")
                {
                    MessageBox.Show("Đăng ký thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                while (true)
                {
                    try
                    {
                        client_socket.Receive(header_bytes);
                    }
                    catch (SocketException ex)
                    {
                        break;
                    }
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
                        String decypted_message_String = Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String((string)message_dict["message"]), true));
                        chat_listbox.Invoke(new Action(() =>
                        {
                            chat_listbox.Items.Add(message_dict["from"].ToString() + ": " + decypted_message_String);
                        }));
                        Console.WriteLine("Message from " + message_dict["from"].ToString() + ": " + decypted_message_String);
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
                        Dictionary<String,String> clients_publickey_temp = new Dictionary<String,String>();
                        for (int i = 0; i < clients.Length; i++)
                        {
                            String client_username = clients[i].Split(':')[0].Trim();
                            String client_publickey = clients[i].Split(":")[1].Trim();   
                            clients_publickey_temp.Add(client_username, client_publickey);

                        }
                        
                    
                        if (clients_publickey.Count==clients_publickey_temp.Count)
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
                                    if (clients_username[i] == "null" || clients_username[i]==client_name)
                                        continue;
                                    clients_listview.Items.Add(clients_username[i]);
                                }

                            }));
                        }
                        

                    }
                }
            }
        }
        void register(String username)
        {
            Dictionary<String, String> message_dict = new Dictionary<String, String>();
            message_dict.Add("username", username);
            message_dict.Add("type", "register");
            message_dict.Add("public_key", Convert.ToBase64String(rsa.ExportRSAPublicKey()));
            send(message_dict);
            client_name = username;
            listen(true);
        }
        void get_client()
        {
            while (true)
            {
                Thread.Sleep(500);
                Dictionary<String, String> message_dict = new Dictionary<String, String>();
                message_dict.Add("type", "get_clients");
                send(message_dict);
            }
        }

        void chat(String message_String, String to_recipient)
        {
            RSACryptoServiceProvider rsa_recipient = new RSACryptoServiceProvider();
            rsa_recipient.ImportRSAPublicKey(Convert.FromBase64String(clients_publickey[to_recipient]), out _);
            Dictionary<String, String> message_dict = new Dictionary<String, String>();
            message_dict.Add("type", "forward");
            message_dict.Add("from", client_name);
            message_dict.Add("message", Convert.ToBase64String(rsa_recipient.Encrypt(Encoding.UTF8.GetBytes(message_String), true)));
            message_dict.Add("to_recipient", to_recipient);
            send(message_dict);
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
            listen_Thread.Start();
            username_textbox.Enabled = false;
            get_client_Thread.Start();
        }
    }
}