namespace _24_NumericSearch_C_sharp
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelAboutKeyAdd = new System.Windows.Forms.Label();
            this.keyAddInput = new System.Windows.Forms.TextBox();
            this.labelAboutDescAdd = new System.Windows.Forms.Label();
            this.descAddInput = new System.Windows.Forms.TextBox();
            this.listBoxResult = new System.Windows.Forms.ListBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.keyFindInput = new System.Windows.Forms.TextBox();
            this.labelAboutResult = new System.Windows.Forms.Label();
            this.labelAboutKeyFind = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelAboutKeyAdd
            // 
            this.labelAboutKeyAdd.AutoSize = true;
            this.labelAboutKeyAdd.Location = new System.Drawing.Point(13, 13);
            this.labelAboutKeyAdd.Name = "labelAboutKeyAdd";
            this.labelAboutKeyAdd.Size = new System.Drawing.Size(71, 17);
            this.labelAboutKeyAdd.TabIndex = 0;
            this.labelAboutKeyAdd.Text = "Print word";
            // 
            // keyAddInput
            // 
            this.keyAddInput.Location = new System.Drawing.Point(13, 34);
            this.keyAddInput.Name = "keyAddInput";
            this.keyAddInput.Size = new System.Drawing.Size(278, 22);
            this.keyAddInput.TabIndex = 1;
            // 
            // labelAboutDescAdd
            // 
            this.labelAboutDescAdd.AutoSize = true;
            this.labelAboutDescAdd.Location = new System.Drawing.Point(13, 63);
            this.labelAboutDescAdd.Name = "labelAboutDescAdd";
            this.labelAboutDescAdd.Size = new System.Drawing.Size(110, 17);
            this.labelAboutDescAdd.TabIndex = 2;
            this.labelAboutDescAdd.Text = "Print description";
            // 
            // descAddInput
            // 
            this.descAddInput.Location = new System.Drawing.Point(13, 84);
            this.descAddInput.Multiline = true;
            this.descAddInput.Name = "descAddInput";
            this.descAddInput.Size = new System.Drawing.Size(278, 389);
            this.descAddInput.TabIndex = 3;
            // 
            // listBoxResult
            // 
            this.listBoxResult.FormattingEnabled = true;
            this.listBoxResult.ItemHeight = 16;
            this.listBoxResult.Location = new System.Drawing.Point(297, 84);
            this.listBoxResult.Name = "listBoxResult";
            this.listBoxResult.Size = new System.Drawing.Size(282, 452);
            this.listBoxResult.TabIndex = 4;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(13, 480);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(278, 56);
            this.buttonAdd.TabIndex = 5;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // keyFindInput
            // 
            this.keyFindInput.Location = new System.Drawing.Point(297, 34);
            this.keyFindInput.Name = "keyFindInput";
            this.keyFindInput.Size = new System.Drawing.Size(281, 22);
            this.keyFindInput.TabIndex = 6;
            this.keyFindInput.TextChanged += new System.EventHandler(this.keyFindInput_TextChanged);
            // 
            // labelAboutResult
            // 
            this.labelAboutResult.AutoSize = true;
            this.labelAboutResult.Location = new System.Drawing.Point(297, 64);
            this.labelAboutResult.Name = "labelAboutResult";
            this.labelAboutResult.Size = new System.Drawing.Size(48, 17);
            this.labelAboutResult.TabIndex = 7;
            this.labelAboutResult.Text = "Result";
            // 
            // labelAboutKeyFind
            // 
            this.labelAboutKeyFind.AutoSize = true;
            this.labelAboutKeyFind.Location = new System.Drawing.Point(297, 14);
            this.labelAboutKeyFind.Name = "labelAboutKeyFind";
            this.labelAboutKeyFind.Size = new System.Drawing.Size(71, 17);
            this.labelAboutKeyFind.TabIndex = 8;
            this.labelAboutKeyFind.Text = "Print word";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 546);
            this.Controls.Add(this.labelAboutKeyFind);
            this.Controls.Add(this.labelAboutResult);
            this.Controls.Add(this.keyFindInput);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listBoxResult);
            this.Controls.Add(this.descAddInput);
            this.Controls.Add(this.labelAboutDescAdd);
            this.Controls.Add(this.keyAddInput);
            this.Controls.Add(this.labelAboutKeyAdd);
            this.Name = "MainForm";
            this.Text = "NumericSearch";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAboutKeyAdd;
        private System.Windows.Forms.TextBox keyAddInput;
        private System.Windows.Forms.Label labelAboutDescAdd;
        private System.Windows.Forms.TextBox descAddInput;
        private System.Windows.Forms.ListBox listBoxResult;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.TextBox keyFindInput;
        private System.Windows.Forms.Label labelAboutResult;
        private System.Windows.Forms.Label labelAboutKeyFind;
    }
}

