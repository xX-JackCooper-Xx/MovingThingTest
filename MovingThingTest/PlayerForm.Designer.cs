﻿namespace MovingThingTest
{
    partial class PlayerForm : Form
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
            components = new System.ComponentModel.Container();
            mapPanel = new Panel();
            exitBtn = new Button();
            LoadBtn = new Button();
            controlsPanel = new Panel();
            timer = new System.Windows.Forms.Timer(components);
            mapPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mapPanel
            // 
            mapPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mapPanel.AutoSize = true;
            mapPanel.BackColor = Color.Green;
            mapPanel.Controls.Add(exitBtn);
            mapPanel.Controls.Add(LoadBtn);
            mapPanel.Location = new Point(0, 0);
            mapPanel.Margin = new Padding(0);
            mapPanel.Name = "mapPanel";
            mapPanel.Size = new Size(500, 450);
            mapPanel.TabIndex = 3;
            // 
            // exitBtn
            // 
            exitBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            exitBtn.Location = new Point(86, 416);
            exitBtn.Name = "exitBtn";
            exitBtn.Size = new Size(75, 23);
            exitBtn.TabIndex = 16;
            exitBtn.Text = "Exit";
            exitBtn.UseVisualStyleBackColor = true;
            exitBtn.Click += exitBtn_Click;
            // 
            // LoadBtn
            // 
            LoadBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LoadBtn.Location = new Point(12, 416);
            LoadBtn.Name = "LoadBtn";
            LoadBtn.Size = new Size(68, 23);
            LoadBtn.TabIndex = 15;
            LoadBtn.Text = "Load";
            LoadBtn.UseVisualStyleBackColor = true;
            LoadBtn.Click += LoadBtn_Click;
            // 
            // controlsPanel
            // 
            controlsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            controlsPanel.Location = new Point(500, 0);
            controlsPanel.Margin = new Padding(0);
            controlsPanel.Name = "controlsPanel";
            controlsPanel.Size = new Size(300, 450);
            controlsPanel.TabIndex = 5;
            // 
            // timer
            // 
            timer.Enabled = true;
            timer.Interval = 50;
            timer.Tick += timer_Tick;
            // 
            // PlayerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 451);
            Controls.Add(controlsPanel);
            Controls.Add(mapPanel);
            IsMdiContainer = true;
            Name = "PlayerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Load += MapMaker_Load;
            mapPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel mapPanel;
        private Panel controlsPanel;
        private System.Windows.Forms.Timer timer;
        private Button LoadBtn;
        private Button exitBtn;
    }
}