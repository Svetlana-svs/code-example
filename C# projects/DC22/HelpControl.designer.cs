using DC22.Shell.Controls.Common;
namespace DC22.Shell.Help
{
    partial class HelpControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpControl));
            this.m_txtHelp = new DC22.Shell.Controls.Common.CTextBox();
            this.SuspendLayout();
            // 
            // m_txtHelp
            // 
            resources.ApplyResources(this.m_txtHelp, "m_txtHelp");
            this.m_txtHelp.Name = "m_txtHelp";
            this.m_txtHelp.ReadOnly = true;
            this.m_txtHelp.TabStop = false;
            // 
            // HelpControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.m_txtHelp);
            this.Name = "HelpControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

       private CTextBox m_txtHelp;
       private System.Windows.Forms.Panel m_pnlHarmonic;
       private System.Windows.Forms.Panel m_pnlResults;
       private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
       private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
       private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
       private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn4;
       private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn5;
       private System.Windows.Forms.Panel m_pnlSignal;
       private DC22.Shell.Controls.Measures.ChartControl m_chartSignal;
       private Resco.Controls.AdvancedList.RowTemplate m_rowTemplateSmallText;
       private Resco.Controls.AdvancedList.RowTemplate m_rowTemplateSmallTextSelected;
    }
}
