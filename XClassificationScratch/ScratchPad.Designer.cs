namespace XClassificationScratch
{
    partial class ScratchPad
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
            txtAPILocation = new TextBox();
            lblAPILocation = new Label();
            lblModelName = new Label();
            txtModelName = new TextBox();
            lblImageLoc = new Label();
            openFileDialog1 = new OpenFileDialog();
            txtImageFile = new TextBox();
            btnFileSelector = new Button();
            pbImage = new PictureBox();
            btnClassify = new Button();
            txtResponse = new TextBox();
            lblResponse = new Label();
            txtQuestion = new TextBox();
            lblQuestion = new Label();
            ((System.ComponentModel.ISupportInitialize)pbImage).BeginInit();
            SuspendLayout();
            // 
            // txtAPILocation
            // 
            txtAPILocation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtAPILocation.Location = new Point(195, 26);
            txtAPILocation.Margin = new Padding(2);
            txtAPILocation.Name = "txtAPILocation";
            txtAPILocation.Size = new Size(425, 23);
            txtAPILocation.TabIndex = 0;
            txtAPILocation.Text = "http://192.168.0.117:1234";
            // 
            // lblAPILocation
            // 
            lblAPILocation.AutoSize = true;
            lblAPILocation.Location = new Point(91, 28);
            lblAPILocation.Margin = new Padding(2, 0, 2, 0);
            lblAPILocation.Name = "lblAPILocation";
            lblAPILocation.Size = new Size(74, 15);
            lblAPILocation.TabIndex = 1;
            lblAPILocation.Text = "API Location";
            // 
            // lblModelName
            // 
            lblModelName.AutoSize = true;
            lblModelName.Location = new Point(90, 55);
            lblModelName.Margin = new Padding(2, 0, 2, 0);
            lblModelName.Name = "lblModelName";
            lblModelName.Size = new Size(76, 15);
            lblModelName.TabIndex = 2;
            lblModelName.Text = "Model Name";
            // 
            // txtModelName
            // 
            txtModelName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtModelName.Location = new Point(194, 53);
            txtModelName.Margin = new Padding(2);
            txtModelName.Name = "txtModelName";
            txtModelName.Size = new Size(425, 23);
            txtModelName.TabIndex = 3;
            txtModelName.Text = "llava-1.6-mistral-7b:2";
            // 
            // lblImageLoc
            // 
            lblImageLoc.AutoSize = true;
            lblImageLoc.Location = new Point(91, 113);
            lblImageLoc.Margin = new Padding(2, 0, 2, 0);
            lblImageLoc.Name = "lblImageLoc";
            lblImageLoc.Size = new Size(40, 15);
            lblImageLoc.TabIndex = 4;
            lblImageLoc.Text = "Image";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtImageFile
            // 
            txtImageFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtImageFile.Location = new Point(195, 111);
            txtImageFile.Margin = new Padding(2);
            txtImageFile.Name = "txtImageFile";
            txtImageFile.ReadOnly = true;
            txtImageFile.Size = new Size(425, 23);
            txtImageFile.TabIndex = 5;
            // 
            // btnFileSelector
            // 
            btnFileSelector.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFileSelector.Location = new Point(623, 110);
            btnFileSelector.Margin = new Padding(2);
            btnFileSelector.Name = "btnFileSelector";
            btnFileSelector.Size = new Size(33, 20);
            btnFileSelector.TabIndex = 6;
            btnFileSelector.Text = "...";
            btnFileSelector.UseVisualStyleBackColor = true;
            btnFileSelector.Click += btnFileSelector_Click;
            // 
            // pbImage
            // 
            pbImage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pbImage.Location = new Point(195, 142);
            pbImage.Margin = new Padding(2);
            pbImage.Name = "pbImage";
            pbImage.Size = new Size(424, 162);
            pbImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbImage.TabIndex = 7;
            pbImage.TabStop = false;
            // 
            // btnClassify
            // 
            btnClassify.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClassify.Location = new Point(542, 320);
            btnClassify.Margin = new Padding(2);
            btnClassify.Name = "btnClassify";
            btnClassify.Size = new Size(77, 20);
            btnClassify.TabIndex = 8;
            btnClassify.Text = "Classify";
            btnClassify.UseVisualStyleBackColor = true;
            btnClassify.Click += btnClassify_Click;
            // 
            // txtResponse
            // 
            txtResponse.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtResponse.Enabled = false;
            txtResponse.Location = new Point(196, 353);
            txtResponse.Margin = new Padding(2);
            txtResponse.Multiline = true;
            txtResponse.Name = "txtResponse";
            txtResponse.Size = new Size(425, 75);
            txtResponse.TabIndex = 9;
            txtResponse.Text = "None";
            // 
            // lblResponse
            // 
            lblResponse.AutoSize = true;
            lblResponse.Location = new Point(91, 355);
            lblResponse.Margin = new Padding(2, 0, 2, 0);
            lblResponse.Name = "lblResponse";
            lblResponse.Size = new Size(52, 15);
            lblResponse.TabIndex = 10;
            lblResponse.Text = "Reponse";
            // 
            // txtQuestion
            // 
            txtQuestion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtQuestion.Location = new Point(194, 80);
            txtQuestion.Margin = new Padding(2);
            txtQuestion.Name = "txtQuestion";
            txtQuestion.Size = new Size(425, 23);
            txtQuestion.TabIndex = 12;
            txtQuestion.Text = "What is this image?";
            // 
            // lblQuestion
            // 
            lblQuestion.AutoSize = true;
            lblQuestion.Location = new Point(90, 82);
            lblQuestion.Margin = new Padding(2, 0, 2, 0);
            lblQuestion.Name = "lblQuestion";
            lblQuestion.Size = new Size(55, 15);
            lblQuestion.TabIndex = 11;
            lblQuestion.Text = "Question";
            // 
            // ScratchPad
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(666, 434);
            Controls.Add(txtQuestion);
            Controls.Add(lblQuestion);
            Controls.Add(lblResponse);
            Controls.Add(txtResponse);
            Controls.Add(btnClassify);
            Controls.Add(pbImage);
            Controls.Add(btnFileSelector);
            Controls.Add(txtImageFile);
            Controls.Add(lblImageLoc);
            Controls.Add(txtModelName);
            Controls.Add(lblModelName);
            Controls.Add(lblAPILocation);
            Controls.Add(txtAPILocation);
            Margin = new Padding(2);
            Name = "ScratchPad";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Scratch";
            ((System.ComponentModel.ISupportInitialize)pbImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtAPILocation;
        private Label lblAPILocation;
        private Label lblModelName;
        private TextBox txtModelName;
        private Label lblImageLoc;
        private OpenFileDialog openFileDialog1;
        private TextBox txtImageFile;
        private Button btnFileSelector;
        private PictureBox pbImage;
        private Button btnClassify;
        private TextBox txtResponse;
        private Label lblResponse;
        private TextBox txtQuestion;
        private Label lblQuestion;
    }
}
