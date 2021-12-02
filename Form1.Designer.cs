
namespace SimpleChat
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.textArea = new System.Windows.Forms.TextBox();
            this.bSent = new System.Windows.Forms.Button();
            this.messageArea = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textPort = new System.Windows.Forms.TextBox();
            this.textIP = new System.Windows.Forms.TextBox();
            this.textName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bConnect = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textArea
            // 
            this.textArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textArea.Location = new System.Drawing.Point(12, 12);
            this.textArea.Multiline = true;
            this.textArea.Name = "textArea";
            this.textArea.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textArea.Size = new System.Drawing.Size(1074, 339);
            this.textArea.TabIndex = 0;
            // 
            // bSent
            // 
            this.bSent.Enabled = false;
            this.bSent.Location = new System.Drawing.Point(620, 525);
            this.bSent.Name = "bSent";
            this.bSent.Size = new System.Drawing.Size(466, 44);
            this.bSent.TabIndex = 1;
            this.bSent.Text = "Отправить";
            this.bSent.UseVisualStyleBackColor = true;
            this.bSent.Click += new System.EventHandler(this.bSent_Click);
            // 
            // messageArea
            // 
            this.messageArea.Location = new System.Drawing.Point(620, 393);
            this.messageArea.Multiline = true;
            this.messageArea.Name = "messageArea";
            this.messageArea.Size = new System.Drawing.Size(466, 109);
            this.messageArea.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textPort);
            this.groupBox1.Controls.Add(this.textIP);
            this.groupBox1.Controls.Add(this.textName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(33, 376);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(441, 210);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры";
            // 
            // textPort
            // 
            this.textPort.Location = new System.Drawing.Point(222, 155);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(198, 29);
            this.textPort.TabIndex = 6;
            this.textPort.TextChanged += new System.EventHandler(this.OptionsChanged);
            // 
            // textIP
            // 
            this.textIP.Location = new System.Drawing.Point(222, 97);
            this.textIP.Name = "textIP";
            this.textIP.Size = new System.Drawing.Size(198, 29);
            this.textIP.TabIndex = 5;
            this.textIP.TextChanged += new System.EventHandler(this.OptionsChanged);
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(222, 41);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(198, 29);
            this.textName.TabIndex = 4;
            this.textName.TextChanged += new System.EventHandler(this.OptionsChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "Порт:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Адрес:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Имя:";
            // 
            // bConnect
            // 
            this.bConnect.Location = new System.Drawing.Point(33, 617);
            this.bConnect.Name = "bConnect";
            this.bConnect.Size = new System.Drawing.Size(441, 44);
            this.bConnect.TabIndex = 4;
            this.bConnect.Text = "Подключиться";
            this.bConnect.UseVisualStyleBackColor = true;
            this.bConnect.Click += new System.EventHandler(this.connect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 673);
            this.Controls.Add(this.bConnect);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.messageArea);
            this.Controls.Add(this.bSent);
            this.Controls.Add(this.textArea);
            this.Name = "Form1";
            this.Text = "SimpleChat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textArea;
        private System.Windows.Forms.Button bSent;
        private System.Windows.Forms.TextBox messageArea;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textPort;
        private System.Windows.Forms.TextBox textIP;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Button bConnect;
    }
}

