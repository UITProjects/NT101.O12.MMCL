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
            chat_listbox.ItemHeight = 15;
            chat_listbox.Location = new Point(12, 12);
            chat_listbox.Name = "chat_listbox";
            chat_listbox.Size = new Size(467, 229);
            chat_listbox.TabIndex = 0;
            // 
            // clients_listview
            // 
            clients_listview.FormattingEnabled = true;
            clients_listview.ItemHeight = 15;
            clients_listview.Location = new Point(530, 27);
            clients_listview.Name = "clients_listview";
            clients_listview.Size = new Size(167, 154);
            clients_listview.TabIndex = 1;
            // 
            // message_textbox
            // 
            message_textbox.Location = new Point(72, 258);
            message_textbox.Name = "message_textbox";
            message_textbox.Size = new Size(246, 23);
            message_textbox.TabIndex = 2;
            // 
            // send_btn
            // 
            send_btn.Location = new Point(358, 258);
            send_btn.Name = "send_btn";
            send_btn.Size = new Size(75, 23);
            send_btn.TabIndex = 3;
            send_btn.Text = "Send";
            send_btn.UseVisualStyleBackColor = true;
            send_btn.Click += button1_Click;
            // 
            // username_textbox
            // 
            username_textbox.Location = new Point(579, 218);
            username_textbox.Name = "username_textbox";
            username_textbox.Size = new Size(100, 23);
            username_textbox.TabIndex = 4;
            // 
            // register_btn
            // 
            register_btn.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            register_btn.Location = new Point(592, 258);
            register_btn.Name = "register_btn";
            register_btn.Size = new Size(87, 36);
            register_btn.TabIndex = 5;
            register_btn.Text = "Register";
            register_btn.UseVisualStyleBackColor = true;
            register_btn.Click += register_btn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(513, 221);
            label1.Name = "label1";
            label1.Size = new Size(63, 15);
            label1.TabIndex = 6;
            label1.Text = "Username:";
            // 
            // client_form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(label1);
            Controls.Add(register_btn);
            Controls.Add(username_textbox);
            Controls.Add(send_btn);
            Controls.Add(message_textbox);
            Controls.Add(clients_listview);
            Controls.Add(chat_listbox);
            Margin = new Padding(3, 2, 3, 2);
            Name = "client_form";
            Text = "Form1";
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