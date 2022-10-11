namespace DesktopApp
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
            this.components = new System.ComponentModel.Container();
            this.testButton = new System.Windows.Forms.Button();
            this.testTextBox1 = new System.Windows.Forms.TextBox();
            this.addressBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.addressBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(291, 239);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(234, 83);
            this.testButton.TabIndex = 0;
            this.testButton.Text = "Test Button";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // testTextBox1
            // 
            this.testTextBox1.Enabled = false;
            this.testTextBox1.Location = new System.Drawing.Point(247, 120);
            this.testTextBox1.Name = "testTextBox1";
            this.testTextBox1.Size = new System.Drawing.Size(307, 31);
            this.testTextBox1.TabIndex = 1;
            // 
            // addressBindingSource
            // 
            this.addressBindingSource.DataSource = typeof(DTO.Address);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(371, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Output";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.testTextBox1);
            this.Controls.Add(this.testButton);
            this.Name = "Form1";
            this.Text = "UPS Application";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.addressBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button testButton;
        private TextBox testTextBox1;
        private BindingSource addressBindingSource;
        private Label label1;
    }
}