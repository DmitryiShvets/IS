namespace ClipsFormsExample
{
    partial class ClipsFormsExample
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClipsFormsExample));
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.codeBox = new System.Windows.Forms.TextBox();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numeric_task = new System.Windows.Forms.NumericUpDown();
            this.numeric_hero = new System.Windows.Forms.NumericUpDown();
            this.btn_select_task = new System.Windows.Forms.Button();
            this.cb_hero = new System.Windows.Forms.ComboBox();
            this.cb_task = new System.Windows.Forms.ComboBox();
            this.add_hero = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.clipsOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.clipsSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.numeric_trash = new System.Windows.Forms.NumericUpDown();
            this.btn_res = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_task)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_hero)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_trash)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1315, 746);
            this.panel1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.codeBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.outputBox);
            this.splitContainer1.Size = new System.Drawing.Size(1315, 746);
            this.splitContainer1.SplitterDistance = 585;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 2;
            // 
            // codeBox
            // 
            this.codeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeBox.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.codeBox.Location = new System.Drawing.Point(0, 0);
            this.codeBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.codeBox.Multiline = true;
            this.codeBox.Name = "codeBox";
            this.codeBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.codeBox.Size = new System.Drawing.Size(585, 746);
            this.codeBox.TabIndex = 2;
            // 
            // outputBox
            // 
            this.outputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputBox.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.outputBox.Location = new System.Drawing.Point(0, 0);
            this.outputBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputBox.Size = new System.Drawing.Size(725, 746);
            this.outputBox.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.numeric_trash);
            this.panel2.Controls.Add(this.numeric_task);
            this.panel2.Controls.Add(this.numeric_hero);
            this.panel2.Controls.Add(this.btn_select_task);
            this.panel2.Controls.Add(this.cb_hero);
            this.panel2.Controls.Add(this.cb_task);
            this.panel2.Controls.Add(this.add_hero);
            this.panel2.Controls.Add(this.nextButton);
            this.panel2.Controls.Add(this.resetButton);
            this.panel2.Controls.Add(this.btn_res);
            this.panel2.Controls.Add(this.openButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 746);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1317, 66);
            this.panel2.TabIndex = 6;
            // 
            // numeric_task
            // 
            this.numeric_task.DecimalPlaces = 1;
            this.numeric_task.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numeric_task.Location = new System.Drawing.Point(590, 26);
            this.numeric_task.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numeric_task.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_task.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numeric_task.Name = "numeric_task";
            this.numeric_task.Size = new System.Drawing.Size(57, 22);
            this.numeric_task.TabIndex = 14;
            // 
            // numeric_hero
            // 
            this.numeric_hero.DecimalPlaces = 1;
            this.numeric_hero.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numeric_hero.Location = new System.Drawing.Point(922, 25);
            this.numeric_hero.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numeric_hero.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_hero.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numeric_hero.Name = "numeric_hero";
            this.numeric_hero.Size = new System.Drawing.Size(57, 22);
            this.numeric_hero.TabIndex = 13;
            // 
            // btn_select_task
            // 
            this.btn_select_task.Location = new System.Drawing.Point(490, 24);
            this.btn_select_task.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_select_task.Name = "btn_select_task";
            this.btn_select_task.Size = new System.Drawing.Size(93, 25);
            this.btn_select_task.TabIndex = 11;
            this.btn_select_task.Text = "Выбрать";
            this.btn_select_task.UseVisualStyleBackColor = true;
            this.btn_select_task.Click += new System.EventHandler(this.btn_select_task_Click);
            // 
            // cb_hero
            // 
            this.cb_hero.FormattingEnabled = true;
            this.cb_hero.Items.AddRange(new object[] {
            "Juggernaut",
            "Pudge",
            "Invoker",
            "Rubik",
            "Dazzle",
            "Lifestealer",
            "Lina",
            "Bristleback",
            "Bane",
            "Lich"});
            this.cb_hero.Location = new System.Drawing.Point(672, 25);
            this.cb_hero.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_hero.Name = "cb_hero";
            this.cb_hero.Size = new System.Drawing.Size(121, 24);
            this.cb_hero.TabIndex = 10;
            // 
            // cb_task
            // 
            this.cb_task.FormattingEnabled = true;
            this.cb_task.Items.AddRange(new object[] {
            "Push",
            "Burst",
            "Tank",
            "Initiator",
            "Control"});
            this.cb_task.Location = new System.Drawing.Point(363, 25);
            this.cb_task.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_task.Name = "cb_task";
            this.cb_task.Size = new System.Drawing.Size(121, 24);
            this.cb_task.TabIndex = 10;
            // 
            // add_hero
            // 
            this.add_hero.Location = new System.Drawing.Point(799, 25);
            this.add_hero.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.add_hero.Name = "add_hero";
            this.add_hero.Size = new System.Drawing.Size(116, 23);
            this.add_hero.TabIndex = 9;
            this.add_hero.Text = "Добавить";
            this.add_hero.UseVisualStyleBackColor = true;
            this.add_hero.Click += new System.EventHandler(this.add_hero_Click);
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nextButton.Location = new System.Drawing.Point(1155, 15);
            this.nextButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(160, 37);
            this.nextButton.TabIndex = 8;
            this.nextButton.Text = "Дальше";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // resetButton
            // 
            this.resetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resetButton.Location = new System.Drawing.Point(987, 17);
            this.resetButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(160, 37);
            this.resetButton.TabIndex = 7;
            this.resetButton.Text = "Рестарт";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(4, 15);
            this.openButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(132, 37);
            this.openButton.TabIndex = 5;
            this.openButton.Text = "Открыть";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openFile_Click);
            // 
            // clipsOpenFileDialog
            // 
            this.clipsOpenFileDialog.Filter = "CLIPS files|*.clp|All files|*.*";
            this.clipsOpenFileDialog.Title = "Открыть файл кода CLIPS";
            // 
            // clipsSaveFileDialog
            // 
            this.clipsSaveFileDialog.Filter = "CLIPS files|*.clp|All files|*.*";
            this.clipsSaveFileDialog.Title = "Созранить файл как...";
            // 
            // numeric_trash
            // 
            this.numeric_trash.DecimalPlaces = 1;
            this.numeric_trash.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numeric_trash.Location = new System.Drawing.Point(299, 24);
            this.numeric_trash.Margin = new System.Windows.Forms.Padding(4);
            this.numeric_trash.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_trash.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numeric_trash.Name = "numeric_trash";
            this.numeric_trash.Size = new System.Drawing.Size(57, 22);
            this.numeric_trash.TabIndex = 15;
            this.numeric_trash.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            // 
            // btn_res
            // 
            this.btn_res.Location = new System.Drawing.Point(144, 15);
            this.btn_res.Margin = new System.Windows.Forms.Padding(4);
            this.btn_res.Name = "btn_res";
            this.btn_res.Size = new System.Drawing.Size(124, 37);
            this.btn_res.TabIndex = 5;
            this.btn_res.Text = "Винрейт";
            this.btn_res.UseVisualStyleBackColor = true;
            this.btn_res.Click += new System.EventHandler(this.btn_res_Click);
            // 
            // ClipsFormsExample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1317, 812);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(874, 356);
            this.Name = "ClipsFormsExample";
            this.Text = "Экспертная система \"DOTA2\" c КУ";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numeric_task)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_hero)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_trash)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.TextBox codeBox;
    private System.Windows.Forms.TextBox outputBox;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button nextButton;
    private System.Windows.Forms.Button resetButton;
    private System.Windows.Forms.Button openButton;
    private System.Windows.Forms.OpenFileDialog clipsOpenFileDialog;
    private System.Windows.Forms.FontDialog fontDialog1;
    private System.Windows.Forms.SaveFileDialog clipsSaveFileDialog;
        private System.Windows.Forms.Button add_hero;
        private System.Windows.Forms.ComboBox cb_task;
        private System.Windows.Forms.ComboBox cb_hero;
        private System.Windows.Forms.Button btn_select_task;
        private System.Windows.Forms.NumericUpDown numeric_hero;
        private System.Windows.Forms.NumericUpDown numeric_task;
        private System.Windows.Forms.NumericUpDown numeric_trash;
        private System.Windows.Forms.Button btn_res;
    }
}

