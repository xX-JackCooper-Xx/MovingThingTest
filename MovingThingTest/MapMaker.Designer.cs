namespace MovingThingTest
{
    partial class MapMaker
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
            controlsPanel = new Panel();
            timer1 = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // mapPanel
            // 
            mapPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mapPanel.AutoSize = true;
            mapPanel.BackColor = Color.Green;
            mapPanel.Location = new Point(0, 0);
            mapPanel.Margin = new Padding(0);
            mapPanel.Name = "mapPanel";
            mapPanel.Size = new Size(500, 450);
            mapPanel.TabIndex = 3;
            mapPanel.Paint += panel1_Paint;
            // 
            // controlsPanel
            // 
            controlsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            controlsPanel.Location = new Point(500, 0);
            controlsPanel.Margin = new Padding(0);
            controlsPanel.Name = "controlsPanel";
            controlsPanel.Size = new Size(300, 450);
            controlsPanel.TabIndex = 5;
            controlsPanel.Paint += controlsPanel_Paint;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 50;
            timer1.Tick += timer1_Tick;
            // 
            // MapMaker
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 451);
            Controls.Add(controlsPanel);
            Controls.Add(mapPanel);
            IsMdiContainer = true;
            Name = "MapMaker";
            Load += MapMaker_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel mapPanel;
        private Panel controlsPanel;
        private System.Windows.Forms.Timer timer1;
    }
}