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
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            widthBox = new TextBox();
            heightBox = new TextBox();
            squadBox = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top;
            button1.Location = new Point(512, 161);
            button1.Name = "button1";
            button1.Size = new Size(147, 23);
            button1.TabIndex = 1;
            button1.Text = "Create New Map";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top;
            button2.Location = new Point(107, 161);
            button2.Name = "button2";
            button2.Size = new Size(69, 23);
            button2.TabIndex = 2;
            button2.Text = "Play Map";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
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
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main";
            Load += Main_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Label label1;
        private TextBox widthBox;
        private TextBox heightBox;
        private TextBox squadBox;
    }
}