namespace MovingThingTest
{
    partial class MapMakerControl
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
            components = new System.ComponentModel.Container();
            timer1 = new System.Windows.Forms.Timer(components);
            modeButton = new Button();
            tyleButton = new Button();
            button1 = new Button();
            Save = new Button();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 10;
            timer1.Tick += timer1_Tick;
            // 
            // modeButton
            // 
            modeButton.Location = new Point(12, 14);
            modeButton.Name = "modeButton";
            modeButton.Size = new Size(68, 23);
            modeButton.TabIndex = 3;
            modeButton.Text = "Move";
            modeButton.UseVisualStyleBackColor = true;
            modeButton.Click += modeButton_Click;
            // 
            // tyleButton
            // 
            tyleButton.AutoSize = true;
            tyleButton.BackColor = Color.Gray;
            tyleButton.Location = new Point(12, 43);
            tyleButton.Name = "tyleButton";
            tyleButton.Size = new Size(68, 25);
            tyleButton.TabIndex = 4;
            tyleButton.Text = "Tyle";
            tyleButton.UseVisualStyleBackColor = false;
            tyleButton.Click += tyleButton_Click;
            // 
            // button1
            // 
            button1.Location = new Point(12, 110);
            button1.Name = "button1";
            button1.Size = new Size(68, 39);
            button1.TabIndex = 10;
            button1.Text = "Lock Camera";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Save
            // 
            Save.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Save.Location = new Point(12, 409);
            Save.Name = "Save";
            Save.Size = new Size(68, 23);
            Save.TabIndex = 11;
            Save.Text = "Save";
            Save.UseVisualStyleBackColor = true;
            Save.Click += Save_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(800, 450);
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            pictureBox1.MouseWheel += pictureBox1_MouseWheel;
            // 
            // UserControl1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.Green;
            Controls.Add(Save);
            Controls.Add(button1);
            Controls.Add(tyleButton);
            Controls.Add(modeButton);
            Controls.Add(pictureBox1);
            Name = "UserControl1";
            Size = new Size(800, 450);
            Load += UserControl1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private Button modeButton;
        private Button tyleButton;
        private Button button1;
        private Button Save;
        private PictureBox pictureBox1;
    }
}
