namespace RMU1
{
    partial class Launch
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxOverlapZone = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxVolltage = new System.Windows.Forms.TextBox();
            this.tbxCurrent = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbxPipeWall = new System.Windows.Forms.ComboBox();
            this.cbxPipeDiameter = new System.Windows.Forms.ComboBox();
            this.btnDefault = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbxExpositionTime = new System.Windows.Forms.TextBox();
            this.tbFramesCount = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxTargetingTime = new System.Windows.Forms.TextBox();
            this.tbxExpositionTimeAbut = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chbxReverse = new System.Windows.Forms.CheckBox();
            this.tbxComment = new System.Windows.Forms.RichTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbxCropZone = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Диаметр трубы (мм)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Толщина стенки (мм)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(259, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Зона перекрытия кадров (pix)";
            // 
            // tbxOverlapZone
            // 
            this.tbxOverlapZone.Location = new System.Drawing.Point(455, 164);
            this.tbxOverlapZone.Name = "tbxOverlapZone";
            this.tbxOverlapZone.Size = new System.Drawing.Size(48, 20);
            this.tbxOverlapZone.TabIndex = 5;
            this.tbxOverlapZone.Text = "2";
            this.tbxOverlapZone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxOverlapZone.TextChanged += new System.EventHandler(this.tbxOverlapZone_TextChanged);
            this.tbxOverlapZone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbx_KeyDown);
            this.tbxOverlapZone.Leave += new System.EventHandler(this.tbx_Leave);
            this.tbxOverlapZone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbxOverlapZone.Validating += new System.ComponentModel.CancelEventHandler(this.tbx_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(259, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Напряжение (КВ)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(259, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Ток (мА)";
            // 
            // tbxVolltage
            // 
            this.tbxVolltage.Location = new System.Drawing.Point(455, 82);
            this.tbxVolltage.MaxLength = 5;
            this.tbxVolltage.Name = "tbxVolltage";
            this.tbxVolltage.Size = new System.Drawing.Size(48, 20);
            this.tbxVolltage.TabIndex = 3;
            this.tbxVolltage.Text = "135";
            this.tbxVolltage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxVolltage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbx_KeyDown);
            this.tbxVolltage.Leave += new System.EventHandler(this.tbx_Leave);
            this.tbxVolltage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbxVolltage.Validating += new System.ComponentModel.CancelEventHandler(this.tbx_Validating);
            // 
            // tbxCurrent
            // 
            this.tbxCurrent.Location = new System.Drawing.Point(455, 115);
            this.tbxCurrent.Name = "tbxCurrent";
            this.tbxCurrent.Size = new System.Drawing.Size(48, 20);
            this.tbxCurrent.TabIndex = 4;
            this.tbxCurrent.Text = "0.3";
            this.tbxCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxCurrent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbx_KeyDown);
            this.tbxCurrent.Leave += new System.EventHandler(this.tbx_Leave);
            this.tbxCurrent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbxCurrent.Validating += new System.ComponentModel.CancelEventHandler(this.tbx_Validating);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(318, 429);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(154, 39);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Сканирование";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbxPipeWall
            // 
            this.cbxPipeWall.FormattingEnabled = true;
            this.cbxPipeWall.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "25"});
            this.cbxPipeWall.Location = new System.Drawing.Point(151, 118);
            this.cbxPipeWall.Name = "cbxPipeWall";
            this.cbxPipeWall.Size = new System.Drawing.Size(48, 21);
            this.cbxPipeWall.TabIndex = 2;
            this.cbxPipeWall.SelectedIndexChanged += new System.EventHandler(this.pipeParameter_SelectedIndexChanged);
            // 
            // cbxPipeDiameter
            // 
            this.cbxPipeDiameter.FormattingEnabled = true;
            this.cbxPipeDiameter.Items.AddRange(new object[] {
            "104",
            "159",
            "168",
            "219",
            "325",
            "426",
            "530"});
            this.cbxPipeDiameter.Location = new System.Drawing.Point(151, 85);
            this.cbxPipeDiameter.Name = "cbxPipeDiameter";
            this.cbxPipeDiameter.Size = new System.Drawing.Size(48, 21);
            this.cbxPipeDiameter.TabIndex = 1;
            this.cbxPipeDiameter.TextChanged += new System.EventHandler(this.pipeParameter_SelectedIndexChanged);
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(19, 344);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(147, 45);
            this.btnDefault.TabIndex = 13;
            this.btnDefault.Text = "Установить значения по умолчанию";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(49, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(117, 18);
            this.label9.TabIndex = 18;
            this.label9.Text = "Выбор объекта";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(308, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 18);
            this.label11.TabIndex = 20;
            this.label11.Text = "Параметры съёмки";
            // 
            // tbxExpositionTime
            // 
            this.tbxExpositionTime.Location = new System.Drawing.Point(455, 261);
            this.tbxExpositionTime.Name = "tbxExpositionTime";
            this.tbxExpositionTime.Size = new System.Drawing.Size(48, 20);
            this.tbxExpositionTime.TabIndex = 8;
            this.tbxExpositionTime.Text = "5";
            this.tbxExpositionTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxExpositionTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbx_KeyDown);
            this.tbxExpositionTime.Leave += new System.EventHandler(this.tbx_Leave);
            this.tbxExpositionTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbxExpositionTime.Validating += new System.ComponentModel.CancelEventHandler(this.tbx_Validating);
            // 
            // tbFramesCount
            // 
            this.tbFramesCount.Location = new System.Drawing.Point(455, 226);
            this.tbFramesCount.Name = "tbFramesCount";
            this.tbFramesCount.Size = new System.Drawing.Size(48, 20);
            this.tbFramesCount.TabIndex = 7;
            this.tbFramesCount.Text = "12";
            this.tbFramesCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbFramesCount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbx_KeyDown);
            this.tbFramesCount.Leave += new System.EventHandler(this.tbx_Leave);
            this.tbFramesCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbFramesCount.Validating += new System.ComponentModel.CancelEventHandler(this.tbx_Validating);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(259, 264);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(151, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Время экспозиции кадра (с)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(259, 233);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(105, 13);
            this.label12.TabIndex = 21;
            this.label12.Text = "Количество кадров";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(259, 299);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(187, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Время позиционирования кадра (с)";
            // 
            // tbxTargetingTime
            // 
            this.tbxTargetingTime.Location = new System.Drawing.Point(455, 292);
            this.tbxTargetingTime.Name = "tbxTargetingTime";
            this.tbxTargetingTime.Size = new System.Drawing.Size(48, 20);
            this.tbxTargetingTime.TabIndex = 9;
            this.tbxTargetingTime.Text = "5";
            this.tbxTargetingTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxTargetingTime.TextChanged += new System.EventHandler(this.expositionTime_TextChanged);
            this.tbxTargetingTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbx_KeyDown);
            this.tbxTargetingTime.Leave += new System.EventHandler(this.tbx_Leave);
            this.tbxTargetingTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbxTargetingTime.Validating += new System.ComponentModel.CancelEventHandler(this.tbx_Validating);
            // 
            // tbxExpositionTimeAbut
            // 
            this.tbxExpositionTimeAbut.Location = new System.Drawing.Point(455, 329);
            this.tbxExpositionTimeAbut.Name = "tbxExpositionTimeAbut";
            this.tbxExpositionTimeAbut.ReadOnly = true;
            this.tbxExpositionTimeAbut.Size = new System.Drawing.Size(48, 20);
            this.tbxExpositionTimeAbut.TabIndex = 10;
            this.tbxExpositionTimeAbut.Text = "120";
            this.tbxExpositionTimeAbut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxExpositionTimeAbut.Validating += new System.ComponentModel.CancelEventHandler(this.tbx_Validating);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(259, 332);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(152, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Время экспозиции стыка (с)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(315, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 15);
            this.label7.TabIndex = 25;
            this.label7.Text = "Источник напряжения";
            // 
            // chbxReverse
            // 
            this.chbxReverse.AutoSize = true;
            this.chbxReverse.Location = new System.Drawing.Point(262, 372);
            this.chbxReverse.Name = "chbxReverse";
            this.chbxReverse.Size = new System.Drawing.Size(68, 17);
            this.chbxReverse.TabIndex = 11;
            this.chbxReverse.Text = "Возврат";
            this.chbxReverse.UseVisualStyleBackColor = true;
            // 
            // tbxComment
            // 
            this.tbxComment.Location = new System.Drawing.Point(570, 115);
            this.tbxComment.Name = "tbxComment";
            this.tbxComment.Size = new System.Drawing.Size(202, 274);
            this.tbxComment.TabIndex = 12;
            this.tbxComment.Text = "";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(567, 85);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 13);
            this.label13.TabIndex = 29;
            this.label13.Text = "Комментарий";
            // 
            // tbxCropZone
            // 
            this.tbxCropZone.Location = new System.Drawing.Point(455, 189);
            this.tbxCropZone.Name = "tbxCropZone";
            this.tbxCropZone.Size = new System.Drawing.Size(48, 20);
            this.tbxCropZone.TabIndex = 6;
            this.tbxCropZone.Text = "2";
            this.tbxCropZone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxCropZone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbx_KeyDown);
            this.tbxCropZone.Leave += new System.EventHandler(this.tbx_Leave);
            this.tbxCropZone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbxCropZone.Validating += new System.ComponentModel.CancelEventHandler(this.tbx_Validating);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(259, 192);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(153, 13);
            this.label14.TabIndex = 30;
            this.label14.Text = "Зона обрезания кадров (mm)";
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // Launch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 487);
            this.Controls.Add(this.tbxCropZone);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tbxComment);
            this.Controls.Add(this.chbxReverse);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbxExpositionTime);
            this.Controls.Add(this.tbxExpositionTimeAbut);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbxTargetingTime);
            this.Controls.Add(this.tbFramesCount);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.cbxPipeDiameter);
            this.Controls.Add(this.cbxPipeWall);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbxCurrent);
            this.Controls.Add(this.tbxVolltage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbxOverlapZone);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Launch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сканирование";
            this.Load += new System.EventHandler(this.Launch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxOverlapZone;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxVolltage;
        private System.Windows.Forms.TextBox tbxCurrent;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cbxPipeWall;
        private System.Windows.Forms.ComboBox cbxPipeDiameter;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbxExpositionTime;
        private System.Windows.Forms.TextBox tbFramesCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbxTargetingTime;
        private System.Windows.Forms.TextBox tbxExpositionTimeAbut;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chbxReverse;
        private System.Windows.Forms.RichTextBox tbxComment;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbxCropZone;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ErrorProvider errorProvider2;
    }
}