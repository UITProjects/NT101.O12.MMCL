namespace ClientChat_UI
{
    partial class client_form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            chat_listbox = new ListBox();
            clients_listview = new ListBox();
            message_textbox = new TextBox();
            send_btn = new Button();
            username_textbox = new TextBox();
            register_btn = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // chat_listbox
            // 
            chat_listbox.FormattingEnabled = true;
            chat_listbox.ItemHeight = 20;
            chat_listbox.Location = new Point(14, 16);
            chat_listbox.Margin = new Padding(3, 4, 3, 4);
            chat_listbox.Name = "chat_listbox";
            chat_listbox.Size = new Size(533, 304);
            chat_listbox.TabIndex = 0;
            // 
            // clients_listview
            // 
            clients_listview.FormattingEnabled = true;
            clients_listview.ItemHeight = 20;
            clients_listview.Location = new Point(606, 36);
            clients_listview.Margin = new Padding(3, 4, 3, 4);
            clients_listview.Name = "clients_listview";
            clients_listview.Size = new Size(190, 204);
            clients_listview.TabIndex = 1;
            // 
            // message_textbox
            // 
            message_textbox.Location = new Point(82, 344);
            message_textbox.Margin = new Padding(3, 4, 3, 4);
            message_textbox.Name = "message_textbox";
            message_textbox.Size = new Size(281, 27);
            message_textbox.TabIndex = 2;
            // 
            // send_btn
            // 
            send_btn.Location = new Point(409, 344);
            send_btn.Margin = new Padding(3, 4, 3, 4);
            send_btn.Name = "send_btn";
            send_btn.Size = new Size(86, 31);
            send_btn.TabIndex = 3;
            send_btn.Text = "Send";
            send_btn.UseVisualStyleBackColor = true;
            send_btn.Click += button1_Click;
            // 
            // username_textbox
            // 
            username_textbox.Location = new Point(662, 291);
            username_textbox.Margin = new Padding(3, 4, 3, 4);
            username_textbox.Name = "username_textbox";
            username_textbox.Size = new Size(114, 27);
            username_textbox.TabIndex = 4;
            // 
            // register_btn
            // 
            register_btn.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            register_btn.Location = new Point(677, 344);
            register_btn.Margin = new Padding(3, 4, 3, 4);
            register_btn.Name = "register_btn";
            register_btn.Size = new Size(99, 48);
            register_btn.TabIndex = 5;
            register_btn.Text = "Register";
            register_btn.UseVisualStyleBackColor = true;
            register_btn.Click += register_btn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(586, 295);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 6;
            label1.Text = "Username:";
            // 
            // client_form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 451);
            Controls.Add(label1);
            Controls.Add(register_btn);
            Controls.Add(username_textbox);
            Controls.Add(send_btn);
            Controls.Add(message_textbox);
            Controls.Add(clients_listview);
            Controls.Add(chat_listbox);
            Name = "client_form";
            Text = "Client";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox chat_listbox;
        private ListBox clients_listview;
        private TextBox message_textbox;
        private Button send_btn;
        private TextBox username_textbox;
        private Button register_btn;
        private Label label1;
    }
}