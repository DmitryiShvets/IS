namespace Laba5
{
    partial class Form1
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
            this.btn_load_db = new System.Windows.Forms.Button();
            this.lst_facts = new System.Windows.Forms.ListBox();
            this.lst_rules = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lst_resolve = new System.Windows.Forms.ListView();
            this.btn_forvard_resolve = new System.Windows.Forms.Button();
            this.btn_back_resolve = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_load_db
            // 
            this.btn_load_db.Location = new System.Drawing.Point(453, 49);
            this.btn_load_db.Name = "btn_load_db";
            this.btn_load_db.Size = new System.Drawing.Size(75, 39);
            this.btn_load_db.TabIndex = 1;
            this.btn_load_db.Text = "Загрузить данные";
            this.btn_load_db.UseVisualStyleBackColor = true;
            this.btn_load_db.Click += new System.EventHandler(this.btn_load_db_Click);
            // 
            // lst_facts
            // 
            this.lst_facts.FormattingEnabled = true;
            this.lst_facts.Location = new System.Drawing.Point(6, 19);
            this.lst_facts.Name = "lst_facts";
            this.lst_facts.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lst_facts.Size = new System.Drawing.Size(324, 277);
            this.lst_facts.TabIndex = 3;
            // 
            // lst_rules
            // 
            this.lst_rules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lst_rules.HideSelection = false;
            this.lst_rules.Location = new System.Drawing.Point(6, 19);
            this.lst_rules.Name = "lst_rules";
            this.lst_rules.Size = new System.Drawing.Size(370, 278);
            this.lst_rules.TabIndex = 4;
            this.lst_rules.TileSize = new System.Drawing.Size(236, 30);
            this.lst_rules.UseCompatibleStateImageBehavior = false;
            this.lst_rules.View = System.Windows.Forms.View.List;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lst_rules);
            this.groupBox1.Location = new System.Drawing.Point(889, 113);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(382, 297);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Список продукций";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lst_facts);
            this.groupBox2.Location = new System.Drawing.Point(12, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(336, 297);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Список фактов";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lst_resolve);
            this.groupBox3.Location = new System.Drawing.Point(373, 113);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(489, 305);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Вывод";
            // 
            // lst_resolve
            // 
            this.lst_resolve.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lst_resolve.HideSelection = false;
            this.lst_resolve.Location = new System.Drawing.Point(6, 19);
            this.lst_resolve.Name = "lst_resolve";
            this.lst_resolve.Size = new System.Drawing.Size(477, 278);
            this.lst_resolve.TabIndex = 4;
            this.lst_resolve.TileSize = new System.Drawing.Size(477, 15);
            this.lst_resolve.UseCompatibleStateImageBehavior = false;
            this.lst_resolve.View = System.Windows.Forms.View.Tile;
            // 
            // btn_forvard_resolve
            // 
            this.btn_forvard_resolve.Location = new System.Drawing.Point(603, 49);
            this.btn_forvard_resolve.Name = "btn_forvard_resolve";
            this.btn_forvard_resolve.Size = new System.Drawing.Size(63, 39);
            this.btn_forvard_resolve.TabIndex = 1;
            this.btn_forvard_resolve.Text = "Прямой вывод";
            this.btn_forvard_resolve.UseVisualStyleBackColor = true;
            this.btn_forvard_resolve.Click += new System.EventHandler(this.btn_forvard_resolve_Click);
            // 
            // btn_back_resolve
            // 
            this.btn_back_resolve.Location = new System.Drawing.Point(672, 49);
            this.btn_back_resolve.Name = "btn_back_resolve";
            this.btn_back_resolve.Size = new System.Drawing.Size(66, 39);
            this.btn_back_resolve.TabIndex = 1;
            this.btn_back_resolve.Text = "Обратный вывод";
            this.btn_back_resolve.UseVisualStyleBackColor = true;
            this.btn_back_resolve.Click += new System.EventHandler(this.btn_back_resolve_Click);
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(534, 49);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(63, 39);
            this.btn_clear.TabIndex = 1;
            this.btn_clear.Text = "Очистить";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1283, 550);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_back_resolve);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.btn_forvard_resolve);
            this.Controls.Add(this.btn_load_db);
            this.Name = "Form1";
            this.Text = "Лабараторная работа №5 \"Продукционная модель\"";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_load_db;
        private System.Windows.Forms.ListBox lst_facts;
        private System.Windows.Forms.ListView lst_rules;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView lst_resolve;
        private System.Windows.Forms.Button btn_forvard_resolve;
        private System.Windows.Forms.Button btn_back_resolve;
        private System.Windows.Forms.Button btn_clear;
    }
}

