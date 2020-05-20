namespace IMG2PDF {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.sourcePathTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.browseButton = new System.Windows.Forms.Button();
            this.sizeComboBox = new System.Windows.Forms.ComboBox();
            this.marginBoxLeft = new System.Windows.Forms.TextBox();
            this.marginBoxTop = new System.Windows.Forms.TextBox();
            this.marginBoxRight = new System.Windows.Forms.TextBox();
            this.marginBoxBottom = new System.Windows.Forms.TextBox();
            this.rotateCheckBox = new System.Windows.Forms.CheckBox();
            this.sortComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.outputLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sourcePathTextBox
            // 
            this.sourcePathTextBox.Location = new System.Drawing.Point(112, 16);
            this.sourcePathTextBox.Name = "sourcePathTextBox";
            this.sourcePathTextBox.Size = new System.Drawing.Size(120, 20);
            this.sourcePathTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Source Image Folder";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(104, 260);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Save";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(236, 13);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(28, 25);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // sizeComboBox
            // 
            this.sizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeComboBox.FormattingEnabled = true;
            this.sizeComboBox.Items.AddRange(new object[] {
            "A0",
            "A1",
            "A2",
            "A3",
            "A4",
            "A5",
            "A6",
            "A7",
            "A8"});
            this.sizeComboBox.Location = new System.Drawing.Point(123, 45);
            this.sizeComboBox.Name = "sizeComboBox";
            this.sizeComboBox.Size = new System.Drawing.Size(63, 21);
            this.sizeComboBox.TabIndex = 3;
            // 
            // marginBoxLeft
            // 
            this.marginBoxLeft.Location = new System.Drawing.Point(128, 127);
            this.marginBoxLeft.Name = "marginBoxLeft";
            this.marginBoxLeft.Size = new System.Drawing.Size(27, 20);
            this.marginBoxLeft.TabIndex = 6;
            // 
            // marginBoxTop
            // 
            this.marginBoxTop.Location = new System.Drawing.Point(152, 101);
            this.marginBoxTop.Name = "marginBoxTop";
            this.marginBoxTop.Size = new System.Drawing.Size(27, 20);
            this.marginBoxTop.TabIndex = 5;
            // 
            // marginBoxRight
            // 
            this.marginBoxRight.Location = new System.Drawing.Point(175, 127);
            this.marginBoxRight.Name = "marginBoxRight";
            this.marginBoxRight.Size = new System.Drawing.Size(27, 20);
            this.marginBoxRight.TabIndex = 7;
            // 
            // marginBoxBottom
            // 
            this.marginBoxBottom.Location = new System.Drawing.Point(152, 153);
            this.marginBoxBottom.Name = "marginBoxBottom";
            this.marginBoxBottom.Size = new System.Drawing.Size(27, 20);
            this.marginBoxBottom.TabIndex = 8;
            // 
            // rotateCheckBox
            // 
            this.rotateCheckBox.AutoSize = true;
            this.rotateCheckBox.Location = new System.Drawing.Point(47, 181);
            this.rotateCheckBox.Name = "rotateCheckBox";
            this.rotateCheckBox.Size = new System.Drawing.Size(209, 17);
            this.rotateCheckBox.TabIndex = 9;
            this.rotateCheckBox.Text = "Landscape page for landscape images";
            this.rotateCheckBox.UseVisualStyleBackColor = true;
            // 
            // sortComboBox
            // 
            this.sortComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sortComboBox.FormattingEnabled = true;
            this.sortComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sortComboBox.Items.AddRange(new object[] {
            "Leading Number",
            "Alphabetical"});
            this.sortComboBox.Location = new System.Drawing.Point(123, 69);
            this.sortComboBox.Name = "sortComboBox";
            this.sortComboBox.Size = new System.Drawing.Size(118, 21);
            this.sortComboBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Size";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(70, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Sort by";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(78, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Margins";
            // 
            // outputLabel
            // 
            this.outputLabel.Location = new System.Drawing.Point(12, 201);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(252, 56);
            this.outputLabel.TabIndex = 18;
            this.outputLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 295);
            this.Controls.Add(this.outputLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sortComboBox);
            this.Controls.Add(this.rotateCheckBox);
            this.Controls.Add(this.marginBoxBottom);
            this.Controls.Add(this.marginBoxRight);
            this.Controls.Add(this.marginBoxTop);
            this.Controls.Add(this.marginBoxLeft);
            this.Controls.Add(this.sizeComboBox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sourcePathTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "IMG2PDF";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sourcePathTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.ComboBox sizeComboBox;
        private System.Windows.Forms.TextBox marginBoxLeft;
        private System.Windows.Forms.TextBox marginBoxTop;
        private System.Windows.Forms.TextBox marginBoxRight;
        private System.Windows.Forms.TextBox marginBoxBottom;
        private System.Windows.Forms.CheckBox rotateCheckBox;
        private System.Windows.Forms.ComboBox sortComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label outputLabel;
    }
}

