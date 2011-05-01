namespace YTech.Ltr.Sms.WinForms
{
    partial class FormServerMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.lbl_phone_status = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(59, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 24);
            this.label1.TabIndex = 59;
            this.label1.Text = "Received SMS :";
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(59, 113);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(472, 272);
            this.txtOutput.TabIndex = 58;
            // 
            // lbl_phone_status
            // 
            this.lbl_phone_status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_phone_status.BackColor = System.Drawing.Color.White;
            this.lbl_phone_status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_phone_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_phone_status.ForeColor = System.Drawing.Color.Red;
            this.lbl_phone_status.Location = new System.Drawing.Point(335, 407);
            this.lbl_phone_status.Name = "lbl_phone_status";
            this.lbl_phone_status.Size = new System.Drawing.Size(196, 23);
            this.lbl_phone_status.TabIndex = 57;
            this.lbl_phone_status.Text = "NO PHONE CONNECTED";
            this.lbl_phone_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_phone_status.UseMnemonic = false;
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(62, 23);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(91, 44);
            this.btnStart.TabIndex = 60;
            this.btnStart.Text = "Mulai SMS";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // btnEnd
            // 
            this.btnEnd.Enabled = false;
            this.btnEnd.Location = new System.Drawing.Point(254, 23);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(104, 44);
            this.btnEnd.TabIndex = 61;
            this.btnEnd.Text = "Berhenti SMS";
            this.btnEnd.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(441, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 62;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormServerMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 439);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.lbl_phone_status);
            this.Name = "FormServerMain";
            this.Text = "FormServerMain";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label lbl_phone_status;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
    }
}