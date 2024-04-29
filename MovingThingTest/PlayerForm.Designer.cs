namespace MovingThingTest
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
            timer = new System.Windows.Forms.Timer(components);
            mapPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mapPanel
            // 
            mapPanel.AutoSize = true;
            mapPanel.BackColor = Color.Green;
            mapPanel.Controls.Add(exitBtn);
            mapPanel.Controls.Add(LoadBtn);
            mapPanel.Dock = DockStyle.Fill;
            mapPanel.Location = new Point(0, 0);
            mapPanel.Margin = new Padding(0);
            mapPanel.Name = "mapPanel";
            mapPanel.Size = new Size(851, 506);
            mapPanel.TabIndex = 3;
            mapPanel.Paint += mapPanel_Paint;
            // 
            // exitBtn
            // 
            exitBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            exitBtn.Location = new Point(86, 472);
            exitBtn.Name = "exitBtn";
            exitBtn.Size = new Size(75, 23);
            exitBtn.TabIndex = 16;
            exitBtn.TabStop = false;
            exitBtn.Text = "Exit";
            exitBtn.UseVisualStyleBackColor = true;
            exitBtn.Click += exitBtn_Click;
            // 
            // LoadBtn
            // 
            LoadBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LoadBtn.Location = new Point(12, 472);
            LoadBtn.Name = "LoadBtn";
            LoadBtn.Size = new Size(68, 23);
            LoadBtn.TabIndex = 15;
            LoadBtn.TabStop = false;
            LoadBtn.Text = "Load";
            LoadBtn.UseVisualStyleBackColor = true;
            LoadBtn.Click += LoadBtn_Click;
            // 
            // timer
            // 
            timer.Enabled = true;
            timer.Interval = 10;
            timer.Tick += timer_Tick;
            // 
            // PlayerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(851, 506);
            Controls.Add(mapPanel);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "PlayerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Load += MapMaker_Load;
            KeyDown += PlayerForm_KeyDown;
            KeyPress += PlayerForm_KeyPress;
            KeyUp += PlayerForm_KeyUp;
            PreviewKeyDown += PlayerForm_PreviewKeyDown;
            mapPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel mapPanel;
        private System.Windows.Forms.Timer timer;
        private Button LoadBtn;
        private Button exitBtn;
    }
}