namespace MultipleDesktop.Windows.Forms.View
{
    partial class BackgroundConfigurationView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPic1 = new System.Windows.Forms.TextBox();
            this.btn1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPic1
            // 
            this.txtPic1.Location = new System.Drawing.Point(3, 3);
            this.txtPic1.Name = "txtPic1";
            this.txtPic1.Size = new System.Drawing.Size(287, 20);
            this.txtPic1.TabIndex = 3;
            this.txtPic1.Leave += new System.EventHandler(this.txtPic1_Leave);
            // 
            // btn1
            // 
            this.btn1.Location = new System.Drawing.Point(296, 1);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(51, 23);
            this.btn1.TabIndex = 4;
            this.btn1.Text = "...";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // DesktopBackgroundView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.txtPic1);
            this.Name = "DesktopBackgroundView";
            this.Size = new System.Drawing.Size(350, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPic1;
        private System.Windows.Forms.Button btn1;
    }
}
