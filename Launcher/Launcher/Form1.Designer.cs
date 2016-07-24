namespace Launcher
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.VK = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.start = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelServer = new System.Windows.Forms.Label();
            this.Settings = new System.Windows.Forms.Button();
            this.SettingsBox = new System.Windows.Forms.GroupBox();
            this.ExitSettingsCancel = new System.Windows.Forms.Button();
            this.ExitSettings = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.StartParams = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SettingsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // VK
            // 
            this.VK.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.VK.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.VK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VK.ForeColor = System.Drawing.Color.White;
            this.VK.Location = new System.Drawing.Point(12, 12);
            this.VK.Name = "VK";
            this.VK.Size = new System.Drawing.Size(142, 41);
            this.VK.TabIndex = 0;
            this.VK.Text = "Мы ВКонтакте";
            this.VK.UseVisualStyleBackColor = false;
            this.VK.Click += new System.EventHandler(this.VK_Click);
            // 
            // exit
            // 
            this.exit.BackColor = System.Drawing.Color.Transparent;
            this.exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.exit.ForeColor = System.Drawing.Color.Red;
            this.exit.Location = new System.Drawing.Point(552, 3);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(75, 37);
            this.exit.TabIndex = 1;
            this.exit.Text = "Выход";
            this.exit.UseVisualStyleBackColor = false;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // start
            // 
            this.start.BackColor = System.Drawing.Color.Transparent;
            this.start.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.start.FlatAppearance.BorderSize = 2;
            this.start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.start.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.start.ForeColor = System.Drawing.Color.White;
            this.start.Location = new System.Drawing.Point(116, 290);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(387, 75);
            this.start.TabIndex = 2;
            this.start.Text = "Запустить игру";
            this.start.UseVisualStyleBackColor = false;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(187, 255);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "Сервер:";
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.BackColor = System.Drawing.Color.Transparent;
            this.labelServer.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelServer.ForeColor = System.Drawing.Color.White;
            this.labelServer.Location = new System.Drawing.Point(285, 255);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(102, 32);
            this.labelServer.TabIndex = 6;
            this.labelServer.Text = "SERVER";
            // 
            // Settings
            // 
            this.Settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Settings.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Settings.Location = new System.Drawing.Point(5, 320);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(105, 42);
            this.Settings.TabIndex = 7;
            this.Settings.Text = "Настройки";
            this.Settings.UseVisualStyleBackColor = true;
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            // 
            // SettingsBox
            // 
            this.SettingsBox.BackColor = System.Drawing.Color.LightGray;
            this.SettingsBox.Controls.Add(this.ExitSettingsCancel);
            this.SettingsBox.Controls.Add(this.ExitSettings);
            this.SettingsBox.Controls.Add(this.listBox1);
            this.SettingsBox.Controls.Add(this.StartParams);
            this.SettingsBox.Controls.Add(this.label3);
            this.SettingsBox.Location = new System.Drawing.Point(116, 101);
            this.SettingsBox.Name = "SettingsBox";
            this.SettingsBox.Size = new System.Drawing.Size(430, 264);
            this.SettingsBox.TabIndex = 8;
            this.SettingsBox.TabStop = false;
            this.SettingsBox.Text = "Настройки";
            this.SettingsBox.Visible = false;
            // 
            // ExitSettingsCancel
            // 
            this.ExitSettingsCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitSettingsCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ExitSettingsCancel.Location = new System.Drawing.Point(287, 210);
            this.ExitSettingsCancel.Name = "ExitSettingsCancel";
            this.ExitSettingsCancel.Size = new System.Drawing.Size(137, 46);
            this.ExitSettingsCancel.TabIndex = 9;
            this.ExitSettingsCancel.Text = "Отмена";
            this.ExitSettingsCancel.UseVisualStyleBackColor = true;
            this.ExitSettingsCancel.Click += new System.EventHandler(this.ExitSettingsCancel_Click);
            // 
            // ExitSettings
            // 
            this.ExitSettings.BackColor = System.Drawing.Color.LightGray;
            this.ExitSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ExitSettings.Location = new System.Drawing.Point(12, 210);
            this.ExitSettings.Name = "ExitSettings";
            this.ExitSettings.Size = new System.Drawing.Size(137, 46);
            this.ExitSettings.TabIndex = 8;
            this.ExitSettings.Text = "ОК!";
            this.ExitSettings.UseVisualStyleBackColor = false;
            this.ExitSettings.Click += new System.EventHandler(this.ExitSettings_Click);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.DarkGray;
            this.listBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "Справка:",
            "-cpuCount=2 - количество ядер процессора.",
            "-maxmem=1024 - максимальный объём оперативной памяти (в Мб)",
            "выделяемой для игры (2047 - максимально возможное значение,",
            "всё что выше будет сброшено на 2047).",
            "-nosplash - запуск игры без заставки.",
            "-world=none - отключает загрузку карты при запуске.",
            "-winxp - используется в Vista/Windows 7, чтобы включить поддержку ",
            "multi-GPU."});
            this.listBox1.Location = new System.Drawing.Point(11, 83);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(413, 121);
            this.listBox1.TabIndex = 7;
            // 
            // StartParams
            // 
            this.StartParams.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartParams.Location = new System.Drawing.Point(12, 48);
            this.StartParams.Name = "StartParams";
            this.StartParams.Size = new System.Drawing.Size(412, 29);
            this.StartParams.TabIndex = 6;
            this.StartParams.Text = "-nosplash -world=empty";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(7, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(332, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "Параметры запуска игры:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Launcher.Properties.Resources.main;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(629, 369);
            this.Controls.Add(this.SettingsBox);
            this.Controls.Add(this.Settings);
            this.Controls.Add(this.labelServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.start);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.VK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Launcher Name";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SettingsBox.ResumeLayout(false);
            this.SettingsBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button VK;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.Button Settings;
        private System.Windows.Forms.GroupBox SettingsBox;
        private System.Windows.Forms.TextBox StartParams;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ExitSettingsCancel;
        private System.Windows.Forms.Button ExitSettings;
        private System.Windows.Forms.ListBox listBox1;

    }
}

