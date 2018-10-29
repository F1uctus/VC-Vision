namespace Vision
{
    partial class CtlMain
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
            this.components = new System.ComponentModel.Container();
            this.BtnSnapshot = new System.Windows.Forms.Button();
            this.DetectAndTrain = new System.Windows.Forms.Button();
            this.FaceName = new System.Windows.Forms.TextBox();
            this.Tab = new System.Windows.Forms.TabControl();
            this.TabSettings = new System.Windows.Forms.TabPage();
            this.UseColorCorrection = new System.Windows.Forms.CheckBox();
            this.NumCamDelay = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.TabFaceReco = new System.Windows.Forms.TabPage();
            this.TestImage = new Emgu.CV.UI.ImageBox();
            this.LabelFacesList = new System.Windows.Forms.Label();
            this.DetectedGrayFace = new Emgu.CV.UI.ImageBox();
            this.btSave = new System.Windows.Forms.Button();
            this.ElementToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.Tab.SuspendLayout();
            this.TabSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumCamDelay)).BeginInit();
            this.TabFaceReco.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TestImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetectedGrayFace)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnSnapshot
            // 
            this.BtnSnapshot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.BtnSnapshot.FlatAppearance.BorderSize = 0;
            this.BtnSnapshot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSnapshot.Location = new System.Drawing.Point(3, 124);
            this.BtnSnapshot.Name = "BtnSnapshot";
            this.BtnSnapshot.Size = new System.Drawing.Size(100, 25);
            this.BtnSnapshot.TabIndex = 12;
            this.BtnSnapshot.Text = "Распознать";
            this.BtnSnapshot.UseVisualStyleBackColor = false;
            this.BtnSnapshot.Click += new System.EventHandler(this.BtnSnapshot_Click);
            // 
            // DetectAndTrain
            // 
            this.DetectAndTrain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.DetectAndTrain.FlatAppearance.BorderSize = 0;
            this.DetectAndTrain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DetectAndTrain.Location = new System.Drawing.Point(109, 124);
            this.DetectAndTrain.Name = "DetectAndTrain";
            this.DetectAndTrain.Size = new System.Drawing.Size(100, 25);
            this.DetectAndTrain.TabIndex = 17;
            this.DetectAndTrain.Text = "Обучить";
            this.DetectAndTrain.UseVisualStyleBackColor = false;
            this.DetectAndTrain.Click += new System.EventHandler(this.DetectAndTrain_Click);
            // 
            // FaceName
            // 
            this.FaceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FaceName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.FaceName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FaceName.ForeColor = System.Drawing.SystemColors.Control;
            this.FaceName.Location = new System.Drawing.Point(127, 3);
            this.FaceName.Name = "FaceName";
            this.FaceName.Size = new System.Drawing.Size(316, 23);
            this.FaceName.TabIndex = 18;
            this.FaceName.WordWrap = false;
            // 
            // Tab
            // 
            this.Tab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tab.Controls.Add(this.TabSettings);
            this.Tab.Controls.Add(this.TabFaceReco);
            this.Tab.Location = new System.Drawing.Point(-4, 36);
            this.Tab.Name = "Tab";
            this.Tab.SelectedIndex = 0;
            this.Tab.Size = new System.Drawing.Size(458, 418);
            this.Tab.TabIndex = 21;
            // 
            // TabSettings
            // 
            this.TabSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.TabSettings.Controls.Add(this.UseColorCorrection);
            this.TabSettings.Controls.Add(this.NumCamDelay);
            this.TabSettings.Controls.Add(this.label1);
            this.TabSettings.Location = new System.Drawing.Point(4, 24);
            this.TabSettings.Name = "TabSettings";
            this.TabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.TabSettings.Size = new System.Drawing.Size(450, 390);
            this.TabSettings.TabIndex = 0;
            this.TabSettings.Text = "Настройки";
            // 
            // UseColorCorrection
            // 
            this.UseColorCorrection.AutoSize = true;
            this.UseColorCorrection.Location = new System.Drawing.Point(11, 11);
            this.UseColorCorrection.Name = "UseColorCorrection";
            this.UseColorCorrection.Size = new System.Drawing.Size(335, 34);
            this.UseColorCorrection.TabIndex = 18;
            this.UseColorCorrection.Text = "Корректировать яркость и контрастность  изображений\r\nдля улучшения точности распо" +
    "знавания";
            this.UseColorCorrection.UseVisualStyleBackColor = true;
            // 
            // NumCamDelay
            // 
            this.NumCamDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.NumCamDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumCamDelay.ForeColor = System.Drawing.SystemColors.Control;
            this.NumCamDelay.Location = new System.Drawing.Point(240, 67);
            this.NumCamDelay.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumCamDelay.Name = "NumCamDelay";
            this.NumCamDelay.Size = new System.Drawing.Size(116, 23);
            this.NumCamDelay.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 15);
            this.label1.TabIndex = 19;
            this.label1.Text = "Задержка камеры перед снимком (мс):";
            // 
            // TabFaceReco
            // 
            this.TabFaceReco.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.TabFaceReco.Controls.Add(this.TestImage);
            this.TabFaceReco.Controls.Add(this.LabelFacesList);
            this.TabFaceReco.Controls.Add(this.DetectAndTrain);
            this.TabFaceReco.Controls.Add(this.FaceName);
            this.TabFaceReco.Controls.Add(this.BtnSnapshot);
            this.TabFaceReco.Controls.Add(this.DetectedGrayFace);
            this.TabFaceReco.Location = new System.Drawing.Point(4, 24);
            this.TabFaceReco.Name = "TabFaceReco";
            this.TabFaceReco.Padding = new System.Windows.Forms.Padding(3);
            this.TabFaceReco.Size = new System.Drawing.Size(450, 390);
            this.TabFaceReco.TabIndex = 1;
            this.TabFaceReco.Text = "Обучение распознавателя";
            // 
            // TestImage
            // 
            this.TestImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TestImage.Location = new System.Drawing.Point(3, 155);
            this.TestImage.Name = "TestImage";
            this.TestImage.Size = new System.Drawing.Size(440, 231);
            this.TestImage.TabIndex = 2;
            this.TestImage.TabStop = false;
            // 
            // LabelFacesList
            // 
            this.LabelFacesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelFacesList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelFacesList.Location = new System.Drawing.Point(127, 29);
            this.LabelFacesList.Name = "LabelFacesList";
            this.LabelFacesList.Size = new System.Drawing.Size(316, 89);
            this.LabelFacesList.TabIndex = 21;
            // 
            // DetectedGrayFace
            // 
            this.DetectedGrayFace.Location = new System.Drawing.Point(3, 3);
            this.DetectedGrayFace.Name = "DetectedGrayFace";
            this.DetectedGrayFace.Size = new System.Drawing.Size(117, 115);
            this.DetectedGrayFace.TabIndex = 20;
            this.DetectedGrayFace.TabStop = false;
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btSave.FlatAppearance.BorderSize = 0;
            this.btSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSave.ForeColor = System.Drawing.Color.White;
            this.btSave.Location = new System.Drawing.Point(0, 0);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(450, 30);
            this.btSave.TabIndex = 5;
            this.btSave.Text = "Save options";
            this.btSave.UseVisualStyleBackColor = false;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // ElementToolTip
            // 
            this.ElementToolTip.AutoPopDelay = 5000;
            this.ElementToolTip.InitialDelay = 100;
            this.ElementToolTip.ReshowDelay = 100;
            // 
            // CtlMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.Tab);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.MinimumSize = new System.Drawing.Size(450, 450);
            this.Name = "CtlMain";
            this.Size = new System.Drawing.Size(450, 450);
            this.Load += new System.EventHandler(this.CtlMain_Load);
            this.Tab.ResumeLayout(false);
            this.TabSettings.ResumeLayout(false);
            this.TabSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumCamDelay)).EndInit();
            this.TabFaceReco.ResumeLayout(false);
            this.TabFaceReco.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TestImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetectedGrayFace)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BtnSnapshot;
        public Emgu.CV.UI.ImageBox TestImage;
        private System.Windows.Forms.Button DetectAndTrain;
        private System.Windows.Forms.TextBox FaceName;
        public Emgu.CV.UI.ImageBox DetectedGrayFace;
        private System.Windows.Forms.TabControl Tab;
        private System.Windows.Forms.TabPage TabSettings;
        private System.Windows.Forms.TabPage TabFaceReco;
        private System.Windows.Forms.CheckBox UseColorCorrection;
        private System.Windows.Forms.NumericUpDown NumCamDelay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelFacesList;
		private System.Windows.Forms.Button btSave;
		private System.Windows.Forms.ToolTip ElementToolTip;
	}
}
