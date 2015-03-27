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
            this.SuspendLayout();
            // 
            // listBoxSessions
            // 
            this.listBoxSessions.FormattingEnabled = true;
            this.listBoxSessions.Location = new System.Drawing.Point(12, 117);
            this.listBoxSessions.Name = "listBoxSessions";
            this.listBoxSessions.Size = new System.Drawing.Size(288, 173);
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
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 695);
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
    }
}
