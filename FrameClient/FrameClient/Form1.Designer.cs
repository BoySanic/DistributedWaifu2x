namespace FrameClient
{
    partial class Form1
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
            this.listDevicesAvail = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listDevicesEnabled = new System.Windows.Forms.ListBox();
            this.btnEnable = new System.Windows.Forms.Button();
            this.btnDisable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listDevicesAvail
            // 
            this.listDevicesAvail.FormattingEnabled = true;
            this.listDevicesAvail.Location = new System.Drawing.Point(13, 55);
            this.listDevicesAvail.Name = "listDevicesAvail";
            this.listDevicesAvail.Size = new System.Drawing.Size(410, 147);
            this.listDevicesAvail.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Devices Available";
            // 
            // listDevicesEnabled
            // 
            this.listDevicesEnabled.FormattingEnabled = true;
            this.listDevicesEnabled.Location = new System.Drawing.Point(578, 55);
            this.listDevicesEnabled.Name = "listDevicesEnabled";
            this.listDevicesEnabled.Size = new System.Drawing.Size(446, 147);
            this.listDevicesEnabled.TabIndex = 2;
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(461, 82);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(75, 23);
            this.btnEnable.TabIndex = 3;
            this.btnEnable.Text = "-->";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // btnDisable
            // 
            this.btnDisable.Location = new System.Drawing.Point(461, 140);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(75, 23);
            this.btnDisable.TabIndex = 4;
            this.btnDisable.Text = "<--";
            this.btnDisable.UseVisualStyleBackColor = true;
            this.btnDisable.Click += new System.EventHandler(this.btnDisable_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1252, 450);
            this.Controls.Add(this.btnDisable);
            this.Controls.Add(this.btnEnable);
            this.Controls.Add(this.listDevicesEnabled);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listDevicesAvail);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listDevicesAvail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listDevicesEnabled;
        private System.Windows.Forms.Button btnEnable;
        private System.Windows.Forms.Button btnDisable;
    }
}

