﻿namespace client_csharp
{
    partial class MainWindow
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
            this.listBoxSessions = new System.Windows.Forms.ListBox();
            this.textBoxSessionName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPlayerName = new System.Windows.Forms.TextBox();
            this.buttonUpdateSessionList = new System.Windows.Forms.Button();
            this.buttonNewSession = new System.Windows.Forms.Button();
            this.buttonJoinSession = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxRole = new System.Windows.Forms.ComboBox();
            this.labelPaused = new System.Windows.Forms.Label();
            this.timerGameUpdate = new System.Windows.Forms.Timer(this.components);
            this.buttonPause = new System.Windows.Forms.Button();
            this.pictureBoxMap = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxResources = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.listBoxPuppies = new System.Windows.Forms.ListBox();
            this.textBoxPuppyData = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxSessions
            // 
            this.listBoxSessions.FormattingEnabled = true;
            this.listBoxSessions.Location = new System.Drawing.Point(12, 117);
            this.listBoxSessions.Name = "listBoxSessions";
            this.listBoxSessions.Size = new System.Drawing.Size(288, 69);
            this.listBoxSessions.TabIndex = 0;
            this.listBoxSessions.SelectedIndexChanged += new System.EventHandler(this.listBoxSessions_SelectedIndexChanged);
            // 
            // textBoxSessionName
            // 
            this.textBoxSessionName.Location = new System.Drawing.Point(121, 30);
            this.textBoxSessionName.Name = "textBoxSessionName";
            this.textBoxSessionName.Size = new System.Drawing.Size(179, 20);
            this.textBoxSessionName.TabIndex = 1;
            this.textBoxSessionName.Text = "Eden";
            this.textBoxSessionName.TextChanged += new System.EventHandler(this.textBoxSessionName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "New session name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Player name:";
            // 
            // textBoxPlayerName
            // 
            this.textBoxPlayerName.Location = new System.Drawing.Point(121, 6);
            this.textBoxPlayerName.Name = "textBoxPlayerName";
            this.textBoxPlayerName.Size = new System.Drawing.Size(179, 20);
            this.textBoxPlayerName.TabIndex = 1;
            this.textBoxPlayerName.Text = "Matt";
            this.textBoxPlayerName.TextChanged += new System.EventHandler(this.textBoxPlayerName_TextChanged);
            // 
            // buttonUpdateSessionList
            // 
            this.buttonUpdateSessionList.Location = new System.Drawing.Point(12, 84);
            this.buttonUpdateSessionList.Name = "buttonUpdateSessionList";
            this.buttonUpdateSessionList.Size = new System.Drawing.Size(103, 27);
            this.buttonUpdateSessionList.TabIndex = 3;
            this.buttonUpdateSessionList.Text = "Update Sessions";
            this.buttonUpdateSessionList.UseVisualStyleBackColor = true;
            this.buttonUpdateSessionList.Click += new System.EventHandler(this.buttonUpdateSessionList_Click);
            // 
            // buttonNewSession
            // 
            this.buttonNewSession.Location = new System.Drawing.Point(121, 84);
            this.buttonNewSession.Name = "buttonNewSession";
            this.buttonNewSession.Size = new System.Drawing.Size(86, 27);
            this.buttonNewSession.TabIndex = 3;
            this.buttonNewSession.Text = "New Session";
            this.buttonNewSession.UseVisualStyleBackColor = true;
            this.buttonNewSession.Click += new System.EventHandler(this.buttonNewSession_Click);
            // 
            // buttonJoinSession
            // 
            this.buttonJoinSession.Location = new System.Drawing.Point(213, 84);
            this.buttonJoinSession.Name = "buttonJoinSession";
            this.buttonJoinSession.Size = new System.Drawing.Size(86, 27);
            this.buttonJoinSession.TabIndex = 3;
            this.buttonJoinSession.Text = "Join Session";
            this.buttonJoinSession.UseVisualStyleBackColor = true;
            this.buttonJoinSession.Click += new System.EventHandler(this.buttonJoinSession_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Player role:";
            // 
            // comboBoxRole
            // 
            this.comboBoxRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRole.FormattingEnabled = true;
            this.comboBoxRole.Items.AddRange(new object[] {
            "Builder",
            "Culture",
            "Intrigue",
            "Military"});
            this.comboBoxRole.Location = new System.Drawing.Point(121, 57);
            this.comboBoxRole.Name = "comboBoxRole";
            this.comboBoxRole.Size = new System.Drawing.Size(178, 21);
            this.comboBoxRole.TabIndex = 4;
            // 
            // labelPaused
            // 
            this.labelPaused.AutoSize = true;
            this.labelPaused.Location = new System.Drawing.Point(411, 13);
            this.labelPaused.Name = "labelPaused";
            this.labelPaused.Size = new System.Drawing.Size(73, 13);
            this.labelPaused.TabIndex = 5;
            this.labelPaused.Text = "Game paused";
            // 
            // timerGameUpdate
            // 
            this.timerGameUpdate.Enabled = true;
            this.timerGameUpdate.Interval = 1000;
            this.timerGameUpdate.Tick += new System.EventHandler(this.timerGameUpdate_Tick);
            // 
            // buttonPause
            // 
            this.buttonPause.Location = new System.Drawing.Point(321, 6);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(84, 27);
            this.buttonPause.TabIndex = 6;
            this.buttonPause.Text = "Toggle Pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // pictureBoxMap
            // 
            this.pictureBoxMap.Location = new System.Drawing.Point(321, 57);
            this.pictureBoxMap.Name = "pictureBoxMap";
            this.pictureBoxMap.Size = new System.Drawing.Size(683, 760);
            this.pictureBoxMap.TabIndex = 7;
            this.pictureBoxMap.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Resources:";
            // 
            // textBoxResources
            // 
            this.textBoxResources.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxResources.Enabled = false;
            this.textBoxResources.Location = new System.Drawing.Point(12, 219);
            this.textBoxResources.Multiline = true;
            this.textBoxResources.Name = "textBoxResources";
            this.textBoxResources.Size = new System.Drawing.Size(287, 587);
            this.textBoxResources.TabIndex = 9;
            this.textBoxResources.Text = "Line 1\r\nLine 2\r\n";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1010, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Puppies:";
            // 
            // listBoxPuppies
            // 
            this.listBoxPuppies.FormattingEnabled = true;
            this.listBoxPuppies.Location = new System.Drawing.Point(1011, 44);
            this.listBoxPuppies.Name = "listBoxPuppies";
            this.listBoxPuppies.Size = new System.Drawing.Size(300, 303);
            this.listBoxPuppies.TabIndex = 10;
            // 
            // textBoxPuppyData
            // 
            this.textBoxPuppyData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPuppyData.Enabled = false;
            this.textBoxPuppyData.Location = new System.Drawing.Point(1317, 44);
            this.textBoxPuppyData.Multiline = true;
            this.textBoxPuppyData.Name = "textBoxPuppyData";
            this.textBoxPuppyData.Size = new System.Drawing.Size(287, 303);
            this.textBoxPuppyData.TabIndex = 9;
            this.textBoxPuppyData.Text = "Line 1\r\nLine 2\r\n";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1314, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "Puppy data:";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1739, 878);
            this.Controls.Add(this.listBoxPuppies);
            this.Controls.Add(this.textBoxPuppyData);
            this.Controls.Add(this.textBoxResources);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pictureBoxMap);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.labelPaused);
            this.Controls.Add(this.comboBoxRole);
            this.Controls.Add(this.buttonJoinSession);
            this.Controls.Add(this.buttonNewSession);
            this.Controls.Add(this.buttonUpdateSessionList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPlayerName);
            this.Controls.Add(this.textBoxSessionName);
            this.Controls.Add(this.listBoxSessions);
            this.Name = "MainWindow";
            this.Text = "Puppies Client";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxSessions;
        private System.Windows.Forms.TextBox textBoxSessionName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPlayerName;
        private System.Windows.Forms.Button buttonUpdateSessionList;
        private System.Windows.Forms.Button buttonNewSession;
        private System.Windows.Forms.Button buttonJoinSession;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxRole;
        private System.Windows.Forms.Label labelPaused;
        private System.Windows.Forms.Timer timerGameUpdate;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.PictureBox pictureBoxMap;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxResources;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox listBoxPuppies;
        private System.Windows.Forms.TextBox textBoxPuppyData;
        private System.Windows.Forms.Label label6;
    }
}

