namespace HiveTools
{
    partial class MainForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.contextMenuStripMainForm = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.consoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageDeliveredEnergy = new System.Windows.Forms.TabPage();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonProcessSelectedFast = new System.Windows.Forms.Button();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.buttonProcessSelected = new System.Windows.Forms.Button();
            this.buttonDeselectAll = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.LocationSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActiveEnergy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReactiveEnergy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripDeliveredEnergy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deliveredEnergyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonGetLocations = new System.Windows.Forms.Button();
            this.tabPageDataIntegrity = new System.Windows.Forms.TabPage();
            this.buttonProcess = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SummaryValid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SummaryDuplicate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripDataIntegrity = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToCSVToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataIntegrityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activeFlagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.missingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zeroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nullToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.repeatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFlagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateTimePickerEndDataIntegrity = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStartDataIntegrity = new System.Windows.Forms.DateTimePicker();
            this.buttonDeselectAllDataIntegrity = new System.Windows.Forms.Button();
            this.buttonSelectAllDataIntegrity = new System.Windows.Forms.Button();
            this.buttonGetLocationsDataIntegrity = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabControl.SuspendLayout();
            this.contextMenuStripMainForm.SuspendLayout();
            this.tabPageDeliveredEnergy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStripDeliveredEnergy.SuspendLayout();
            this.tabPageDataIntegrity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.contextMenuStripDataIntegrity.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.ContextMenuStrip = this.contextMenuStripMainForm;
            this.tabControl.Controls.Add(this.tabPageDeliveredEnergy);
            this.tabControl.Controls.Add(this.tabPageDataIntegrity);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(868, 344);
            this.tabControl.TabIndex = 0;
            // 
            // contextMenuStripMainForm
            // 
            this.contextMenuStripMainForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consoleToolStripMenuItem});
            this.contextMenuStripMainForm.Name = "contextMenuStripMainForm";
            this.contextMenuStripMainForm.Size = new System.Drawing.Size(118, 26);
            // 
            // consoleToolStripMenuItem
            // 
            this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            this.consoleToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.consoleToolStripMenuItem.Text = "Console";
            this.consoleToolStripMenuItem.Click += new System.EventHandler(this.consoleToolStripMenuItem_Click);
            // 
            // tabPageDeliveredEnergy
            // 
            this.tabPageDeliveredEnergy.Controls.Add(this.buttonUpdate);
            this.tabPageDeliveredEnergy.Controls.Add(this.buttonProcessSelectedFast);
            this.tabPageDeliveredEnergy.Controls.Add(this.dateTimePickerEnd);
            this.tabPageDeliveredEnergy.Controls.Add(this.dateTimePickerStart);
            this.tabPageDeliveredEnergy.Controls.Add(this.buttonProcessSelected);
            this.tabPageDeliveredEnergy.Controls.Add(this.buttonDeselectAll);
            this.tabPageDeliveredEnergy.Controls.Add(this.buttonSelectAll);
            this.tabPageDeliveredEnergy.Controls.Add(this.dataGridView1);
            this.tabPageDeliveredEnergy.Controls.Add(this.buttonGetLocations);
            this.tabPageDeliveredEnergy.Location = new System.Drawing.Point(4, 22);
            this.tabPageDeliveredEnergy.Name = "tabPageDeliveredEnergy";
            this.tabPageDeliveredEnergy.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDeliveredEnergy.Size = new System.Drawing.Size(860, 318);
            this.tabPageDeliveredEnergy.TabIndex = 0;
            this.tabPageDeliveredEnergy.Text = "DeliveredEnergy";
            this.tabPageDeliveredEnergy.UseVisualStyleBackColor = true;
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdate.Location = new System.Drawing.Point(779, 10);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 19;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonProcessSelectedFast
            // 
            this.buttonProcessSelectedFast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonProcessSelectedFast.Location = new System.Drawing.Point(698, 10);
            this.buttonProcessSelectedFast.Name = "buttonProcessSelectedFast";
            this.buttonProcessSelectedFast.Size = new System.Drawing.Size(75, 23);
            this.buttonProcessSelectedFast.TabIndex = 17;
            this.buttonProcessSelectedFast.Text = "Get Fast";
            this.buttonProcessSelectedFast.UseVisualStyleBackColor = true;
            this.buttonProcessSelectedFast.Click += new System.EventHandler(this.buttonProcessSelectedFast_Click);
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.CustomFormat = "MM:yyyy";
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(308, 12);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.ShowUpDown = true;
            this.dateTimePickerEnd.Size = new System.Drawing.Size(87, 20);
            this.dateTimePickerEnd.TabIndex = 16;
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Checked = false;
            this.dateTimePickerStart.CustomFormat = "MM:yyyy";
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerStart.Location = new System.Drawing.Point(204, 12);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.ShowUpDown = true;
            this.dateTimePickerStart.Size = new System.Drawing.Size(83, 20);
            this.dateTimePickerStart.TabIndex = 15;
            this.dateTimePickerStart.Value = new System.DateTime(2017, 6, 1, 0, 0, 0, 0);
            // 
            // buttonProcessSelected
            // 
            this.buttonProcessSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonProcessSelected.Location = new System.Drawing.Point(617, 10);
            this.buttonProcessSelected.Name = "buttonProcessSelected";
            this.buttonProcessSelected.Size = new System.Drawing.Size(75, 23);
            this.buttonProcessSelected.TabIndex = 14;
            this.buttonProcessSelected.Text = "Get Slow";
            this.buttonProcessSelected.UseVisualStyleBackColor = true;
            this.buttonProcessSelected.Click += new System.EventHandler(this.buttonProcessSelected_Click);
            // 
            // buttonDeselectAll
            // 
            this.buttonDeselectAll.Location = new System.Drawing.Point(59, 9);
            this.buttonDeselectAll.Name = "buttonDeselectAll";
            this.buttonDeselectAll.Size = new System.Drawing.Size(45, 23);
            this.buttonDeselectAll.TabIndex = 13;
            this.buttonDeselectAll.Text = "None";
            this.buttonDeselectAll.UseVisualStyleBackColor = true;
            this.buttonDeselectAll.Click += new System.EventHandler(this.buttonDeselectAll_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(6, 9);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(47, 23);
            this.buttonSelectAll.TabIndex = 12;
            this.buttonSelectAll.Text = "All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LocationSelect,
            this.ID,
            this.LocName,
            this.LocationDesc,
            this.ActiveEnergy,
            this.ReactiveEnergy,
            this.StartTime,
            this.EndTime});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStripDeliveredEnergy;
            this.dataGridView1.Location = new System.Drawing.Point(6, 39);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(848, 273);
            this.dataGridView1.TabIndex = 11;
            // 
            // LocationSelect
            // 
            this.LocationSelect.HeaderText = "Select";
            this.LocationSelect.MinimumWidth = 50;
            this.LocationSelect.Name = "LocationSelect";
            this.LocationSelect.Width = 50;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Width = 50;
            // 
            // LocName
            // 
            this.LocName.HeaderText = "LocName";
            this.LocName.Name = "LocName";
            // 
            // LocationDesc
            // 
            this.LocationDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LocationDesc.HeaderText = "LocationDesc";
            this.LocationDesc.Name = "LocationDesc";
            // 
            // ActiveEnergy
            // 
            this.ActiveEnergy.HeaderText = "Active Energy [kW]";
            this.ActiveEnergy.Name = "ActiveEnergy";
            this.ActiveEnergy.Width = 140;
            // 
            // ReactiveEnergy
            // 
            this.ReactiveEnergy.HeaderText = "Reactive Energy [kVAr]";
            this.ReactiveEnergy.Name = "ReactiveEnergy";
            this.ReactiveEnergy.Width = 145;
            // 
            // StartTime
            // 
            this.StartTime.HeaderText = "StartTime";
            this.StartTime.Name = "StartTime";
            this.StartTime.Width = 120;
            // 
            // EndTime
            // 
            this.EndTime.HeaderText = "EndTime";
            this.EndTime.Name = "EndTime";
            this.EndTime.Width = 120;
            // 
            // contextMenuStripDeliveredEnergy
            // 
            this.contextMenuStripDeliveredEnergy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToCSVToolStripMenuItem});
            this.contextMenuStripDeliveredEnergy.Name = "contextMenuStripDeliveredEnergy";
            this.contextMenuStripDeliveredEnergy.Size = new System.Drawing.Size(139, 26);
            // 
            // saveToCSVToolStripMenuItem
            // 
            this.saveToCSVToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deliveredEnergyToolStripMenuItem});
            this.saveToCSVToolStripMenuItem.Name = "saveToCSVToolStripMenuItem";
            this.saveToCSVToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.saveToCSVToolStripMenuItem.Text = "Save To CSV";
            // 
            // deliveredEnergyToolStripMenuItem
            // 
            this.deliveredEnergyToolStripMenuItem.Name = "deliveredEnergyToolStripMenuItem";
            this.deliveredEnergyToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.deliveredEnergyToolStripMenuItem.Text = "Delivered Energy";
            this.deliveredEnergyToolStripMenuItem.Click += new System.EventHandler(this.deliveredEnergyToolStripMenuItem_Click);
            // 
            // buttonGetLocations
            // 
            this.buttonGetLocations.Location = new System.Drawing.Point(110, 9);
            this.buttonGetLocations.Name = "buttonGetLocations";
            this.buttonGetLocations.Size = new System.Drawing.Size(75, 23);
            this.buttonGetLocations.TabIndex = 10;
            this.buttonGetLocations.Text = "Get Devices";
            this.buttonGetLocations.UseVisualStyleBackColor = true;
            this.buttonGetLocations.Click += new System.EventHandler(this.buttonGetLocations_Click);
            // 
            // tabPageDataIntegrity
            // 
            this.tabPageDataIntegrity.Controls.Add(this.buttonProcess);
            this.tabPageDataIntegrity.Controls.Add(this.dataGridView2);
            this.tabPageDataIntegrity.Controls.Add(this.dateTimePickerEndDataIntegrity);
            this.tabPageDataIntegrity.Controls.Add(this.dateTimePickerStartDataIntegrity);
            this.tabPageDataIntegrity.Controls.Add(this.buttonDeselectAllDataIntegrity);
            this.tabPageDataIntegrity.Controls.Add(this.buttonSelectAllDataIntegrity);
            this.tabPageDataIntegrity.Controls.Add(this.buttonGetLocationsDataIntegrity);
            this.tabPageDataIntegrity.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataIntegrity.Name = "tabPageDataIntegrity";
            this.tabPageDataIntegrity.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataIntegrity.Size = new System.Drawing.Size(860, 318);
            this.tabPageDataIntegrity.TabIndex = 1;
            this.tabPageDataIntegrity.Text = "Data Integrity";
            this.tabPageDataIntegrity.UseVisualStyleBackColor = true;
            // 
            // buttonProcess
            // 
            this.buttonProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonProcess.Location = new System.Drawing.Point(778, 8);
            this.buttonProcess.Name = "buttonProcess";
            this.buttonProcess.Size = new System.Drawing.Size(75, 23);
            this.buttonProcess.TabIndex = 23;
            this.buttonProcess.Text = "Process";
            this.buttonProcess.UseVisualStyleBackColor = true;
            this.buttonProcess.Click += new System.EventHandler(this.buttonProcess_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.SummaryValid,
            this.SummaryDuplicate,
            this.DateStart,
            this.DateEnd,
            this.dataGridViewTextBoxColumn4});
            this.dataGridView2.ContextMenuStrip = this.contextMenuStripDataIntegrity;
            this.dataGridView2.Location = new System.Drawing.Point(6, 39);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(848, 273);
            this.dataGridView2.TabIndex = 22;
            this.dataGridView2.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellDoubleClick);
            this.dataGridView2.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView2_CellMouseDown);
            this.dataGridView2.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellMouseEnter);
            this.dataGridView2.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellMouseLeave);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "Select";
            this.dataGridViewCheckBoxColumn1.MinimumWidth = 50;
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "LocName";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "LocationDesc";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // SummaryValid
            // 
            this.SummaryValid.HeaderText = "Valid + Duplicate %";
            this.SummaryValid.Name = "SummaryValid";
            this.SummaryValid.Width = 90;
            // 
            // SummaryDuplicate
            // 
            this.SummaryDuplicate.HeaderText = "Flags";
            this.SummaryDuplicate.Name = "SummaryDuplicate";
            this.SummaryDuplicate.Width = 80;
            // 
            // DateStart
            // 
            this.DateStart.HeaderText = "Date start";
            this.DateStart.Name = "DateStart";
            // 
            // DateEnd
            // 
            this.DateEnd.HeaderText = "Date end";
            this.DateEnd.Name = "DateEnd";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Status";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 80;
            // 
            // contextMenuStripDataIntegrity
            // 
            this.contextMenuStripDataIntegrity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToCSVToolStripMenuItem1,
            this.showFlagsToolStripMenuItem});
            this.contextMenuStripDataIntegrity.Name = "contextMenuStripDataIntegrity";
            this.contextMenuStripDataIntegrity.Size = new System.Drawing.Size(139, 48);
            // 
            // saveToCSVToolStripMenuItem1
            // 
            this.saveToCSVToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataIntegrityToolStripMenuItem,
            this.activeFlagsToolStripMenuItem});
            this.saveToCSVToolStripMenuItem1.Name = "saveToCSVToolStripMenuItem1";
            this.saveToCSVToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
            this.saveToCSVToolStripMenuItem1.Text = "Save To CSV";
            // 
            // dataIntegrityToolStripMenuItem
            // 
            this.dataIntegrityToolStripMenuItem.Name = "dataIntegrityToolStripMenuItem";
            this.dataIntegrityToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.dataIntegrityToolStripMenuItem.Text = "Data Integrity";
            this.dataIntegrityToolStripMenuItem.Click += new System.EventHandler(this.dataIntegrityToolStripMenuItem_Click);
            // 
            // activeFlagsToolStripMenuItem
            // 
            this.activeFlagsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToolStripMenuItem,
            this.missingToolStripMenuItem,
            this.duplicateToolStripMenuItem,
            this.zeroToolStripMenuItem,
            this.nullToolStripMenuItem,
            this.repeatToolStripMenuItem});
            this.activeFlagsToolStripMenuItem.Name = "activeFlagsToolStripMenuItem";
            this.activeFlagsToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.activeFlagsToolStripMenuItem.Text = "Active Flags";
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // missingToolStripMenuItem
            // 
            this.missingToolStripMenuItem.Name = "missingToolStripMenuItem";
            this.missingToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.missingToolStripMenuItem.Text = "Missing";
            this.missingToolStripMenuItem.Click += new System.EventHandler(this.missingToolStripMenuItem_Click);
            // 
            // duplicateToolStripMenuItem
            // 
            this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.duplicateToolStripMenuItem.Text = "Duplicate";
            this.duplicateToolStripMenuItem.Click += new System.EventHandler(this.duplicateToolStripMenuItem_Click);
            // 
            // zeroToolStripMenuItem
            // 
            this.zeroToolStripMenuItem.Name = "zeroToolStripMenuItem";
            this.zeroToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.zeroToolStripMenuItem.Text = "Zero";
            this.zeroToolStripMenuItem.Click += new System.EventHandler(this.zeroToolStripMenuItem_Click);
            // 
            // nullToolStripMenuItem
            // 
            this.nullToolStripMenuItem.Name = "nullToolStripMenuItem";
            this.nullToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.nullToolStripMenuItem.Text = "Null";
            this.nullToolStripMenuItem.Click += new System.EventHandler(this.nullToolStripMenuItem_Click);
            // 
            // repeatToolStripMenuItem
            // 
            this.repeatToolStripMenuItem.Name = "repeatToolStripMenuItem";
            this.repeatToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.repeatToolStripMenuItem.Text = "Repeat";
            this.repeatToolStripMenuItem.Click += new System.EventHandler(this.repeatToolStripMenuItem_Click);
            // 
            // showFlagsToolStripMenuItem
            // 
            this.showFlagsToolStripMenuItem.Name = "showFlagsToolStripMenuItem";
            this.showFlagsToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.showFlagsToolStripMenuItem.Text = "Show Flags";
            this.showFlagsToolStripMenuItem.Click += new System.EventHandler(this.showFlagsToolStripMenuItem_Click);
            // 
            // dateTimePickerEndDataIntegrity
            // 
            this.dateTimePickerEndDataIntegrity.CustomFormat = "dd:MM:yyyy";
            this.dateTimePickerEndDataIntegrity.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEndDataIntegrity.Location = new System.Drawing.Point(292, 12);
            this.dateTimePickerEndDataIntegrity.Name = "dateTimePickerEndDataIntegrity";
            this.dateTimePickerEndDataIntegrity.Size = new System.Drawing.Size(82, 20);
            this.dateTimePickerEndDataIntegrity.TabIndex = 21;
            // 
            // dateTimePickerStartDataIntegrity
            // 
            this.dateTimePickerStartDataIntegrity.Checked = false;
            this.dateTimePickerStartDataIntegrity.CustomFormat = "dd:MM:yyyy";
            this.dateTimePickerStartDataIntegrity.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStartDataIntegrity.Location = new System.Drawing.Point(204, 12);
            this.dateTimePickerStartDataIntegrity.Name = "dateTimePickerStartDataIntegrity";
            this.dateTimePickerStartDataIntegrity.Size = new System.Drawing.Size(82, 20);
            this.dateTimePickerStartDataIntegrity.TabIndex = 20;
            this.dateTimePickerStartDataIntegrity.Value = new System.DateTime(2017, 6, 1, 0, 0, 0, 0);
            // 
            // buttonDeselectAllDataIntegrity
            // 
            this.buttonDeselectAllDataIntegrity.Location = new System.Drawing.Point(59, 9);
            this.buttonDeselectAllDataIntegrity.Name = "buttonDeselectAllDataIntegrity";
            this.buttonDeselectAllDataIntegrity.Size = new System.Drawing.Size(45, 23);
            this.buttonDeselectAllDataIntegrity.TabIndex = 19;
            this.buttonDeselectAllDataIntegrity.Text = "None";
            this.buttonDeselectAllDataIntegrity.UseVisualStyleBackColor = true;
            this.buttonDeselectAllDataIntegrity.Click += new System.EventHandler(this.buttonDeselectAllDataIntegrity_Click);
            // 
            // buttonSelectAllDataIntegrity
            // 
            this.buttonSelectAllDataIntegrity.Location = new System.Drawing.Point(6, 9);
            this.buttonSelectAllDataIntegrity.Name = "buttonSelectAllDataIntegrity";
            this.buttonSelectAllDataIntegrity.Size = new System.Drawing.Size(47, 23);
            this.buttonSelectAllDataIntegrity.TabIndex = 18;
            this.buttonSelectAllDataIntegrity.Text = "All";
            this.buttonSelectAllDataIntegrity.UseVisualStyleBackColor = true;
            this.buttonSelectAllDataIntegrity.Click += new System.EventHandler(this.buttonSelectAllDataIntegrity_Click);
            // 
            // buttonGetLocationsDataIntegrity
            // 
            this.buttonGetLocationsDataIntegrity.Location = new System.Drawing.Point(110, 9);
            this.buttonGetLocationsDataIntegrity.Name = "buttonGetLocationsDataIntegrity";
            this.buttonGetLocationsDataIntegrity.Size = new System.Drawing.Size(75, 23);
            this.buttonGetLocationsDataIntegrity.TabIndex = 17;
            this.buttonGetLocationsDataIntegrity.Text = "Get Devices";
            this.buttonGetLocationsDataIntegrity.UseVisualStyleBackColor = true;
            this.buttonGetLocationsDataIntegrity.Click += new System.EventHandler(this.buttonGetLocationsDataIntegrity_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 368);
            this.Controls.Add(this.tabControl);
            this.MinimumSize = new System.Drawing.Size(908, 407);
            this.Name = "MainForm";
            this.Text = "HiveTools 1.6.0.171102";
            this.tabControl.ResumeLayout(false);
            this.contextMenuStripMainForm.ResumeLayout(false);
            this.tabPageDeliveredEnergy.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStripDeliveredEnergy.ResumeLayout(false);
            this.tabPageDataIntegrity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.contextMenuStripDataIntegrity.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageDeliveredEnergy;
        private System.Windows.Forms.TabPage tabPageDataIntegrity;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonProcessSelectedFast;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.Button buttonProcessSelected;
        private System.Windows.Forms.Button buttonDeselectAll;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LocationSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActiveEnergy;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReactiveEnergy;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndTime;
        private System.Windows.Forms.Button buttonGetLocations;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndDataIntegrity;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartDataIntegrity;
        private System.Windows.Forms.Button buttonDeselectAllDataIntegrity;
        private System.Windows.Forms.Button buttonSelectAllDataIntegrity;
        private System.Windows.Forms.Button buttonGetLocationsDataIntegrity;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button buttonProcess;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn SummaryValid;
        private System.Windows.Forms.DataGridViewTextBoxColumn SummaryDuplicate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMainForm;
        private System.Windows.Forms.ToolStripMenuItem consoleToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDeliveredEnergy;
        private System.Windows.Forms.ToolStripMenuItem saveToCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deliveredEnergyToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDataIntegrity;
        private System.Windows.Forms.ToolStripMenuItem saveToCSVToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dataIntegrityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activeFlagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFlagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem missingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zeroToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nullToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem repeatToolStripMenuItem;
    }
}