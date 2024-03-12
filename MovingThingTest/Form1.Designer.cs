namespace MovingThingTest
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            timer1 = new System.Windows.Forms.Timer(components);
            pictureBox1 = new PictureBox();
            tyleButton = new Button();
            modeButton = new Button();
            zoomInButton = new Button();
            zoomOutButton = new Button();
            button1 = new Button();
            Save = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 10;
            timer1.Tick += timer1_Tick;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.None;
            pictureBox1.BackColor = Color.Green;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(771, 450);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            pictureBox1.MouseWheel += pictureBox1_MouseWheel;
            // 
            // tyleButton
            // 
            tyleButton.AutoSize = true;
            tyleButton.BackColor = Color.Gray;
            tyleButton.Location = new Point(0, 41);
            tyleButton.Name = "tyleButton";
            tyleButton.Size = new Size(68, 25);
            tyleButton.TabIndex = 1;
            tyleButton.Text = "Tyle";
            tyleButton.UseVisualStyleBackColor = false;
            tyleButton.Click += tyleButton_Click;
            // 
            // modeButton
            // 
            modeButton.Location = new Point(0, 12);
            modeButton.Name = "modeButton";
            modeButton.Size = new Size(68, 23);
            modeButton.TabIndex = 2;
            modeButton.Text = "Move";
            modeButton.UseVisualStyleBackColor = true;
            modeButton.Click += modeButton_Click;
            // 
            // zoomInButton
            // 
            zoomInButton.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            zoomInButton.Location = new Point(0, 100);
            zoomInButton.Name = "zoomInButton";
            zoomInButton.Size = new Size(31, 63);
            zoomInButton.TabIndex = 3;
            zoomInButton.Text = "+\r\n";
            zoomInButton.UseVisualStyleBackColor = true;
            zoomInButton.Click += zoomInButton_Click;
            // 
            // zoomOutButton
            // 
            zoomOutButton.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            zoomOutButton.Location = new Point(37, 100);
            zoomOutButton.Name = "zoomOutButton";
            zoomOutButton.Size = new Size(31, 63);
            zoomOutButton.TabIndex = 4;
            zoomOutButton.Text = "-";
            zoomOutButton.UseVisualStyleBackColor = true;
            zoomOutButton.Click += zoomOutButton_Click;
            // 
            // button1
            // 
            button1.Location = new Point(0, 169);
            button1.Name = "button1";
            button1.Size = new Size(68, 39);
            button1.TabIndex = 9;
            button1.Text = "Lock Camera";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Save
            // 
            Save.Location = new Point(0, 392);
            Save.Name = "Save";
            Save.Size = new Size(68, 23);
            Save.TabIndex = 10;
            Save.Text = "Save";
            Save.UseVisualStyleBackColor = true;
            Save.Click += Save_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(771, 450);
            Controls.Add(Save);
            Controls.Add(button1);
            Controls.Add(zoomOutButton);
            Controls.Add(zoomInButton);
            Controls.Add(modeButton);
            Controls.Add(tyleButton);
            Controls.Add(pictureBox1);
            DoubleBuffered = true;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            SizeChanged += Form1_SizeChanged;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private PictureBox pictureBox1;
        private Button tyleButton;
        private Button modeButton;
        private Button zoomInButton;
        private Button zoomOutButton;
        private Button button1;
        private Button Save;
    }
}