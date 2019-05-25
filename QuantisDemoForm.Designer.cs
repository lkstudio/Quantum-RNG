namespace QuantisDemo
{
  partial class QuantisDemoForm
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
      this.groupBoxInformation = new System.Windows.Forms.GroupBox();
      this.textBoxInfo = new System.Windows.Forms.TextBox();
      this.groupBoxData = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
      this.buttonGenerate = new System.Windows.Forms.Button();
      this.textBoxBuffer = new System.Windows.Forms.TextBox();
      this.groupBoxInformation.SuspendLayout();
      this.groupBoxData.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBoxInformation
      // 
      this.groupBoxInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxInformation.AutoSize = true;
      this.groupBoxInformation.Controls.Add(this.textBoxInfo);
      this.groupBoxInformation.Location = new System.Drawing.Point(12, 207);
      this.groupBoxInformation.Name = "groupBoxInformation";
      this.groupBoxInformation.Size = new System.Drawing.Size(601, 141);
      this.groupBoxInformation.TabIndex = 1;
      this.groupBoxInformation.TabStop = false;
      this.groupBoxInformation.Text = "Information";
      // 
      // textBoxInfo
      // 
      this.textBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxInfo.Location = new System.Drawing.Point(11, 19);
      this.textBoxInfo.Multiline = true;
      this.textBoxInfo.Name = "textBoxInfo";
      this.textBoxInfo.ReadOnly = true;
      this.textBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxInfo.Size = new System.Drawing.Size(576, 106);
      this.textBoxInfo.TabIndex = 0;
      // 
      // groupBoxData
      // 
      this.groupBoxData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxData.Controls.Add(this.label1);
      this.groupBoxData.Controls.Add(this.numericUpDown1);
      this.groupBoxData.Controls.Add(this.buttonGenerate);
      this.groupBoxData.Controls.Add(this.textBoxBuffer);
      this.groupBoxData.Location = new System.Drawing.Point(14, 10);
      this.groupBoxData.Name = "groupBoxData";
      this.groupBoxData.Size = new System.Drawing.Size(597, 189);
      this.groupBoxData.TabIndex = 2;
      this.groupBoxData.TabStop = false;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(14, 155);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(144, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Number of bytes to generate:";
      // 
      // numericUpDown1
      // 
      this.numericUpDown1.Location = new System.Drawing.Point(164, 153);
      this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDown1.Name = "numericUpDown1";
      this.numericUpDown1.Size = new System.Drawing.Size(61, 20);
      this.numericUpDown1.TabIndex = 2;
      this.numericUpDown1.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
      // 
      // buttonGenerate
      // 
      this.buttonGenerate.Location = new System.Drawing.Point(231, 150);
      this.buttonGenerate.Name = "buttonGenerate";
      this.buttonGenerate.Size = new System.Drawing.Size(75, 23);
      this.buttonGenerate.TabIndex = 1;
      this.buttonGenerate.Text = "Generate";
      this.buttonGenerate.UseVisualStyleBackColor = true;
      this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
      // 
      // textBoxBuffer
      // 
      this.textBoxBuffer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxBuffer.Location = new System.Drawing.Point(17, 22);
      this.textBoxBuffer.Multiline = true;
      this.textBoxBuffer.Name = "textBoxBuffer";
      this.textBoxBuffer.ReadOnly = true;
      this.textBoxBuffer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxBuffer.Size = new System.Drawing.Size(566, 124);
      this.textBoxBuffer.TabIndex = 0;
      // 
      // QuantisDemoForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(624, 359);
      this.Controls.Add(this.groupBoxData);
      this.Controls.Add(this.groupBoxInformation);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "QuantisDemoForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Quantis Demo for C#";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.groupBoxInformation.ResumeLayout(false);
      this.groupBoxInformation.PerformLayout();
      this.groupBoxData.ResumeLayout(false);
      this.groupBoxData.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxInformation;
    private System.Windows.Forms.TextBox textBoxInfo;
    private System.Windows.Forms.GroupBox groupBoxData;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown numericUpDown1;
    private System.Windows.Forms.Button buttonGenerate;
    private System.Windows.Forms.TextBox textBoxBuffer;

  }
}

