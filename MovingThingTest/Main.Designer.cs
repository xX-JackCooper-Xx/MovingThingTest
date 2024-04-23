namespace MovingThingTest
{
    partial class Main
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
            neMapBtn = new Button();
            playBtn = new Button();
            label1 = new Label();
            widthBox = new TextBox();
            heightBox = new TextBox();
            squadBox = new TextBox();
            SuspendLayout();
            // 
            // neMapBtn
            // 
            neMapBtn.Anchor = AnchorStyles.Top;
            neMapBtn.Location = new Point(512, 161);
            neMapBtn.Name = "neMapBtn";
            neMapBtn.Size = new Size(147, 23);
            neMapBtn.TabIndex = 1;
            neMapBtn.Text = "Create New Map";
            neMapBtn.UseVisualStyleBackColor = true;
            neMapBtn.Click += newMapBtn_Click;
            // 
            // playBtn
            // 
            playBtn.Anchor = AnchorStyles.Top;
            playBtn.Location = new Point(107, 161);
            playBtn.Name = "playBtn";
            playBtn.Size = new Size(69, 23);
            playBtn.TabIndex = 2;
            playBtn.Text = "Play Map";
            playBtn.UseVisualStyleBackColor = true;
            playBtn.Click += playBtn_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Font = new Font("Impact", 36F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(234, 46);
            label1.Name = "label1";
            label1.Size = new Size(297, 60);
            label1.TabIndex = 3;
            label1.Text = "CCF Missions\r\n";
            // 
            // widthBox
            // 
            widthBox.Anchor = AnchorStyles.Top;
            widthBox.Location = new Point(536, 206);
            widthBox.Name = "widthBox";
            widthBox.PlaceholderText = "Enter Width";
            widthBox.Size = new Size(100, 23);
            widthBox.TabIndex = 4;
            // 
            // heightBox
            // 
            heightBox.Anchor = AnchorStyles.Top;
            heightBox.Location = new Point(536, 249);
            heightBox.Name = "heightBox";
            heightBox.PlaceholderText = "Enter Height";
            heightBox.Size = new Size(100, 23);
            heightBox.TabIndex = 5;
            // 
            // squadBox
            // 
            squadBox.Anchor = AnchorStyles.Top;
            squadBox.Location = new Point(94, 218);
            squadBox.Name = "squadBox";
            squadBox.PlaceholderText = "Enter Squad Size";
            squadBox.Size = new Size(100, 23);
            squadBox.TabIndex = 6;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(squadBox);
            Controls.Add(heightBox);
            Controls.Add(widthBox);
            Controls.Add(label1);
            Controls.Add(playBtn);
            Controls.Add(neMapBtn);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button neMapBtn;
        private Button playBtn;
        private Label label1;
        private TextBox widthBox;
        private TextBox heightBox;
        private TextBox squadBox;
    }
}