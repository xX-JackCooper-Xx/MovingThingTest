namespace MovingThingTest
{
    partial class PlayerControl : UserControl
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
            timer = new System.Windows.Forms.Timer(components);
            modeButton = new Button();
            pictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // timer
            // 
            timer.Enabled = true;
            timer.Interval = 10;
            timer.Tick += timer1_Tick;
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
            // pictureBox
            // 
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Location = new Point(0, 0);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(800, 450);
            pictureBox.TabIndex = 12;
            pictureBox.TabStop = false;
            pictureBox.Click += pictureBox_Click;
            pictureBox.Paint += pictureBox_Paint;
            pictureBox.MouseDown += pictureBox_MouseDown;
            pictureBox.MouseUp += pictureBox_MouseUp;
            pictureBox.MouseWheel += pictureBox_MouseWheel;
            // 
            // PlayerControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.Green;
            Controls.Add(modeButton);
            Controls.Add(pictureBox);
            Name = "PlayerControl";
            Size = new Size(800, 450);
            KeyDown += PlayerControl_KeyDown;
            KeyPress += PlayerControl_KeyPress;
            PreviewKeyDown += PlayerControl_PreviewKeyDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private Button modeButton;
        private PictureBox pictureBox;
    }
}
