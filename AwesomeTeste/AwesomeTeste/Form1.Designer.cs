namespace AwesomeTeste
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            Awesomium.Core.WebPreferences webPreferences1 = new Awesomium.Core.WebPreferences(true);
            this.webControl1 = new Awesomium.Windows.Forms.WebControl(this.components);
            this.webSessionProvider1 = new Awesomium.Windows.Forms.WebSessionProvider(this.components);
            this.addressBox1 = new Awesomium.Windows.Forms.AddressBox();
            this.SuspendLayout();
            // 
            // webControl1
            // 
            this.webControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.webControl1.Location = new System.Drawing.Point(12, 38);
            this.webControl1.Size = new System.Drawing.Size(927, 291);
            this.webControl1.Source = new System.Uri("about:blank", System.UriKind.Absolute);
            this.webControl1.TabIndex = 0;
            // 
            // webSessionProvider1
            // 
            webPreferences1.CanScriptsAccessClipboard = true;
            webPreferences1.Databases = true;
            webPreferences1.EnableGPUAcceleration = true;
            webPreferences1.JavascriptViewChangeSource = false;
            webPreferences1.JavascriptViewEvents = false;
            webPreferences1.JavascriptViewExecute = false;
            webPreferences1.SmoothScrolling = true;
            webPreferences1.WebGL = true;
            this.webSessionProvider1.Preferences = webPreferences1;
            this.webSessionProvider1.Views.Add(this.webControl1);
            // 
            // addressBox1
            // 
            this.addressBox1.AcceptsReturn = true;
            this.addressBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.addressBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.addressBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.addressBox1.Location = new System.Drawing.Point(12, 12);
            this.addressBox1.Name = "addressBox1";
            this.addressBox1.Size = new System.Drawing.Size(927, 20);
            this.addressBox1.TabIndex = 1;
            this.addressBox1.URL = null;
            this.addressBox1.WebControl = this.webControl1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 341);
            this.Controls.Add(this.addressBox1);
            this.Controls.Add(this.webControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Awesomium.Windows.Forms.WebControl webControl1;
        private Awesomium.Windows.Forms.WebSessionProvider webSessionProvider1;
        private Awesomium.Windows.Forms.AddressBox addressBox1;
    }
}

