using System;

namespace MultipleDesktop.Windows.Forms.View
{
    partial class AppView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppView));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.lblCopy = new System.Windows.Forms.Label();
            this.backgroundConfigurationFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // lblCopy
            // 
            this.lblCopy.AutoSize = true;
            this.lblCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblCopy.Location = new System.Drawing.Point(4, 183);
            this.lblCopy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCopy.Name = "lblCopy";
            this.lblCopy.Size = new System.Drawing.Size(68, 18);
            this.lblCopy.TabIndex = 10;
            this.lblCopy.Text = "copyright";
            // 
            // backgroundConfigurationFlowLayoutPanel
            // 
            this.backgroundConfigurationFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.backgroundConfigurationFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.backgroundConfigurationFlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.backgroundConfigurationFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.backgroundConfigurationFlowLayoutPanel.Name = "backgroundConfigurationFlowLayoutPanel";
            this.backgroundConfigurationFlowLayoutPanel.Size = new System.Drawing.Size(577, 179);
            this.backgroundConfigurationFlowLayoutPanel.TabIndex = 11;
            // 
            // AppView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 213);
            this.Controls.Add(this.backgroundConfigurationFlowLayoutPanel);
            this.Controls.Add(this.lblCopy);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "AppView";
            this.Text = "Virtual Desktop";
            this.Load += new System.EventHandler(this.AppView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label lblCopy;
        private System.Windows.Forms.FlowLayoutPanel backgroundConfigurationFlowLayoutPanel;
    }
}

