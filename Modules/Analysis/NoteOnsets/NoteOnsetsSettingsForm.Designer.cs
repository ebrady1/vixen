namespace VixenModules.Analysis.NoteOnsets
{
	partial class NoteOnsetsSettingsForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.HighFrequencyRadio = new System.Windows.Forms.RadioButton();
			this.SpectralDifferenceRadio = new System.Windows.Forms.RadioButton();
			this.PhaseDeviationRadio = new System.Windows.Forms.RadioButton();
			this.ComplexDomainRadio = new System.Windows.Forms.RadioButton();
			this.BroadBandEnergyRadio = new System.Windows.Forms.RadioButton();
			this.DetectionFunctionGroupBox = new System.Windows.Forms.GroupBox();
			this.AdaptiveWhiteningCheckBox = new System.Windows.Forms.CheckBox();
			this.SensitivityTrackBar = new System.Windows.Forms.TrackBar();
			this.SensitivityGroupBox = new System.Windows.Forms.GroupBox();
			this.SensitivityValue = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.OKButton = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.GeneralPurposeButton = new System.Windows.Forms.Button();
			this.SoftOnsetsButton = new System.Windows.Forms.Button();
			this.PercussiveOnsetsButton = new System.Windows.Forms.Button();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.DetectionFunctionGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SensitivityTrackBar)).BeginInit();
			this.SensitivityGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Presets:";
			// 
			// HighFrequencyRadio
			// 
			this.HighFrequencyRadio.AutoSize = true;
			this.HighFrequencyRadio.Location = new System.Drawing.Point(16, 124);
			this.HighFrequencyRadio.Name = "HighFrequencyRadio";
			this.HighFrequencyRadio.Size = new System.Drawing.Size(140, 17);
			this.HighFrequencyRadio.TabIndex = 3;
			this.HighFrequencyRadio.TabStop = true;
			this.HighFrequencyRadio.Text = "High Frequency Content";
			this.HighFrequencyRadio.UseVisualStyleBackColor = true;
			this.HighFrequencyRadio.Click += new System.EventHandler(this.DetectFnRadio_Click);
			// 
			// SpectralDifferenceRadio
			// 
			this.SpectralDifferenceRadio.AutoSize = true;
			this.SpectralDifferenceRadio.Location = new System.Drawing.Point(16, 55);
			this.SpectralDifferenceRadio.Name = "SpectralDifferenceRadio";
			this.SpectralDifferenceRadio.Size = new System.Drawing.Size(116, 17);
			this.SpectralDifferenceRadio.TabIndex = 4;
			this.SpectralDifferenceRadio.TabStop = true;
			this.SpectralDifferenceRadio.Text = "Spectral Difference";
			this.SpectralDifferenceRadio.UseVisualStyleBackColor = true;
			this.SpectralDifferenceRadio.Click += new System.EventHandler(this.DetectFnRadio_Click);
			// 
			// PhaseDeviationRadio
			// 
			this.PhaseDeviationRadio.AutoSize = true;
			this.PhaseDeviationRadio.Location = new System.Drawing.Point(16, 78);
			this.PhaseDeviationRadio.Name = "PhaseDeviationRadio";
			this.PhaseDeviationRadio.Size = new System.Drawing.Size(103, 17);
			this.PhaseDeviationRadio.TabIndex = 5;
			this.PhaseDeviationRadio.TabStop = true;
			this.PhaseDeviationRadio.Text = "Phase Deviation";
			this.PhaseDeviationRadio.UseVisualStyleBackColor = true;
			this.PhaseDeviationRadio.Click += new System.EventHandler(this.DetectFnRadio_Click);
			// 
			// ComplexDomainRadio
			// 
			this.ComplexDomainRadio.AutoSize = true;
			this.ComplexDomainRadio.Location = new System.Drawing.Point(16, 32);
			this.ComplexDomainRadio.Name = "ComplexDomainRadio";
			this.ComplexDomainRadio.Size = new System.Drawing.Size(104, 17);
			this.ComplexDomainRadio.TabIndex = 6;
			this.ComplexDomainRadio.TabStop = true;
			this.ComplexDomainRadio.Text = "Complex Domain";
			this.ComplexDomainRadio.UseVisualStyleBackColor = true;
			this.ComplexDomainRadio.Click += new System.EventHandler(this.DetectFnRadio_Click);
			// 
			// BroadBandEnergyRadio
			// 
			this.BroadBandEnergyRadio.AutoSize = true;
			this.BroadBandEnergyRadio.Location = new System.Drawing.Point(16, 101);
			this.BroadBandEnergyRadio.Name = "BroadBandEnergyRadio";
			this.BroadBandEnergyRadio.Size = new System.Drawing.Size(137, 17);
			this.BroadBandEnergyRadio.TabIndex = 7;
			this.BroadBandEnergyRadio.TabStop = true;
			this.BroadBandEnergyRadio.Text = "Broadband Energy Rise";
			this.BroadBandEnergyRadio.UseVisualStyleBackColor = true;
			this.BroadBandEnergyRadio.Click += new System.EventHandler(this.DetectFnRadio_Click);
			// 
			// DetectionFunctionGroupBox
			// 
			this.DetectionFunctionGroupBox.Controls.Add(this.AdaptiveWhiteningCheckBox);
			this.DetectionFunctionGroupBox.Controls.Add(this.HighFrequencyRadio);
			this.DetectionFunctionGroupBox.Controls.Add(this.BroadBandEnergyRadio);
			this.DetectionFunctionGroupBox.Controls.Add(this.SpectralDifferenceRadio);
			this.DetectionFunctionGroupBox.Controls.Add(this.ComplexDomainRadio);
			this.DetectionFunctionGroupBox.Controls.Add(this.PhaseDeviationRadio);
			this.DetectionFunctionGroupBox.Location = new System.Drawing.Point(13, 78);
			this.DetectionFunctionGroupBox.Name = "DetectionFunctionGroupBox";
			this.DetectionFunctionGroupBox.Size = new System.Drawing.Size(171, 194);
			this.DetectionFunctionGroupBox.TabIndex = 8;
			this.DetectionFunctionGroupBox.TabStop = false;
			this.DetectionFunctionGroupBox.Text = "Detection Function";
			// 
			// AdaptiveWhiteningCheckBox
			// 
			this.AdaptiveWhiteningCheckBox.AutoSize = true;
			this.AdaptiveWhiteningCheckBox.Location = new System.Drawing.Point(16, 163);
			this.AdaptiveWhiteningCheckBox.Name = "AdaptiveWhiteningCheckBox";
			this.AdaptiveWhiteningCheckBox.Size = new System.Drawing.Size(119, 17);
			this.AdaptiveWhiteningCheckBox.TabIndex = 0;
			this.AdaptiveWhiteningCheckBox.Text = "Adaptive Whitening";
			this.AdaptiveWhiteningCheckBox.UseVisualStyleBackColor = true;
			this.AdaptiveWhiteningCheckBox.CheckedChanged += new System.EventHandler(this.AdaptiveWhiteningCheckBox_CheckedChanged);
			// 
			// SensitivityTrackBar
			// 
			this.SensitivityTrackBar.Location = new System.Drawing.Point(21, 42);
			this.SensitivityTrackBar.Maximum = 100;
			this.SensitivityTrackBar.Name = "SensitivityTrackBar";
			this.SensitivityTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.SensitivityTrackBar.Size = new System.Drawing.Size(45, 122);
			this.SensitivityTrackBar.TabIndex = 9;
			this.SensitivityTrackBar.TickFrequency = 5;
			this.SensitivityTrackBar.Scroll += new System.EventHandler(this.SensitivityTrackBar_Scroll);
			this.SensitivityTrackBar.ValueChanged += new System.EventHandler(this.SensitivityTrackBar_ValueChanged);
			// 
			// SensitivityGroupBox
			// 
			this.SensitivityGroupBox.Controls.Add(this.SensitivityValue);
			this.SensitivityGroupBox.Controls.Add(this.label3);
			this.SensitivityGroupBox.Controls.Add(this.label2);
			this.SensitivityGroupBox.Controls.Add(this.SensitivityTrackBar);
			this.SensitivityGroupBox.Location = new System.Drawing.Point(196, 78);
			this.SensitivityGroupBox.Name = "SensitivityGroupBox";
			this.SensitivityGroupBox.Size = new System.Drawing.Size(118, 194);
			this.SensitivityGroupBox.TabIndex = 10;
			this.SensitivityGroupBox.TabStop = false;
			this.SensitivityGroupBox.Text = "Sensitivity";
			// 
			// SensitivityValue
			// 
			this.SensitivityValue.AutoSize = true;
			this.SensitivityValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SensitivityValue.Location = new System.Drawing.Point(55, 89);
			this.SensitivityValue.Name = "SensitivityValue";
			this.SensitivityValue.Size = new System.Drawing.Size(38, 25);
			this.SensitivityValue.TabIndex = 12;
			this.SensitivityValue.Text = "00";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(18, 26);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(25, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "100";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(27, 167);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(13, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "0";
			// 
			// OKButton
			// 
			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKButton.Location = new System.Drawing.Point(158, 287);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(75, 23);
			this.OKButton.TabIndex = 13;
			this.OKButton.Text = "OK";
			this.OKButton.UseVisualStyleBackColor = true;
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Location = new System.Drawing.Point(239, 287);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 23);
			this.CancelBtn.TabIndex = 14;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			// 
			// GeneralPurposeButton
			// 
			this.GeneralPurposeButton.Location = new System.Drawing.Point(80, 15);
			this.GeneralPurposeButton.Name = "GeneralPurposeButton";
			this.GeneralPurposeButton.Size = new System.Drawing.Size(68, 38);
			this.GeneralPurposeButton.TabIndex = 15;
			this.GeneralPurposeButton.Text = "General Purpose";
			this.GeneralPurposeButton.UseVisualStyleBackColor = true;
			this.GeneralPurposeButton.Click += new System.EventHandler(this.PresetButton_Click);
			// 
			// SoftOnsetsButton
			// 
			this.SoftOnsetsButton.Location = new System.Drawing.Point(154, 15);
			this.SoftOnsetsButton.Name = "SoftOnsetsButton";
			this.SoftOnsetsButton.Size = new System.Drawing.Size(68, 38);
			this.SoftOnsetsButton.TabIndex = 16;
			this.SoftOnsetsButton.Text = "Soft Onsets";
			this.SoftOnsetsButton.UseVisualStyleBackColor = true;
			this.SoftOnsetsButton.Click += new System.EventHandler(this.PresetButton_Click);
			// 
			// PercussiveOnsetsButton
			// 
			this.PercussiveOnsetsButton.Location = new System.Drawing.Point(228, 15);
			this.PercussiveOnsetsButton.Name = "PercussiveOnsetsButton";
			this.PercussiveOnsetsButton.Size = new System.Drawing.Size(68, 38);
			this.PercussiveOnsetsButton.TabIndex = 17;
			this.PercussiveOnsetsButton.Text = "Percussive Onsets";
			this.PercussiveOnsetsButton.UseVisualStyleBackColor = true;
			this.PercussiveOnsetsButton.Click += new System.EventHandler(this.PresetButton_Click);
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(12, 292);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(29, 13);
			this.linkLabel1.TabIndex = 8;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Help";
			// 
			// NoteOnsetsSettingsForm
			// 
			this.AcceptButton = this.OKButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(326, 324);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.PercussiveOnsetsButton);
			this.Controls.Add(this.SoftOnsetsButton);
			this.Controls.Add(this.GeneralPurposeButton);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.SensitivityGroupBox);
			this.Controls.Add(this.DetectionFunctionGroupBox);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "NoteOnsetsSettingsForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Note Onset Settings";
			this.Load += new System.EventHandler(this.NoteOnsetsSettings_Load);
			this.DetectionFunctionGroupBox.ResumeLayout(false);
			this.DetectionFunctionGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SensitivityTrackBar)).EndInit();
			this.SensitivityGroupBox.ResumeLayout(false);
			this.SensitivityGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton HighFrequencyRadio;
		private System.Windows.Forms.RadioButton SpectralDifferenceRadio;
		private System.Windows.Forms.RadioButton PhaseDeviationRadio;
		private System.Windows.Forms.RadioButton ComplexDomainRadio;
		private System.Windows.Forms.RadioButton BroadBandEnergyRadio;
		private System.Windows.Forms.GroupBox DetectionFunctionGroupBox;
		private System.Windows.Forms.TrackBar SensitivityTrackBar;
		private System.Windows.Forms.GroupBox SensitivityGroupBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label SensitivityValue;
		private System.Windows.Forms.CheckBox AdaptiveWhiteningCheckBox;
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.Button GeneralPurposeButton;
		private System.Windows.Forms.Button SoftOnsetsButton;
		private System.Windows.Forms.Button PercussiveOnsetsButton;
		private System.Windows.Forms.LinkLabel linkLabel1;
	}
}