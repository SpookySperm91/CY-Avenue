namespace CarlosYulo;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        grpbxClientMembership = new GroupBox();
        lblMembershipStatus = new Label();
        lblMembershipEnd = new Label();
        lblMemberShipStart = new Label();
        lblBirthDay = new Label();
        lblAge = new Label();
        lblGender = new Label();
        lblPhoneNumber = new Label();
        lblEmail = new Label();
        lblName = new Label();
        txtbxFullName = new TextBox();
        lblFullName = new Label();
        lblMemberShipId = new Label();
        txtbxMemberShipId = new TextBox();
        btnSearchFullName = new Button();
        btnSearchMemberShipId = new Button();
        table1 = new DataGridView();
        pictureBox1 = new PictureBox();
        grpbxClientMembership.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)table1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // grpbxClientMembership
        // 
        grpbxClientMembership.Controls.Add(lblMembershipStatus);
        grpbxClientMembership.Controls.Add(lblMembershipEnd);
        grpbxClientMembership.Controls.Add(lblMemberShipStart);
        grpbxClientMembership.Controls.Add(lblBirthDay);
        grpbxClientMembership.Controls.Add(lblAge);
        grpbxClientMembership.Controls.Add(lblGender);
        grpbxClientMembership.Controls.Add(lblPhoneNumber);
        grpbxClientMembership.Controls.Add(lblEmail);
        grpbxClientMembership.Controls.Add(lblName);
        grpbxClientMembership.Location = new Point(441, 12);
        grpbxClientMembership.Name = "grpbxClientMembership";
        grpbxClientMembership.Size = new Size(358, 604);
        grpbxClientMembership.TabIndex = 0;
        grpbxClientMembership.TabStop = false;
        grpbxClientMembership.Text = "Client";
        // 
        // lblMembershipStatus
        // 
        lblMembershipStatus.AutoSize = true;
        lblMembershipStatus.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblMembershipStatus.Location = new Point(17, 534);
        lblMembershipStatus.Name = "lblMembershipStatus";
        lblMembershipStatus.Size = new Size(208, 28);
        lblMembershipStatus.TabIndex = 8;
        lblMembershipStatus.Text = "MEMBERSHIP STATUS:";
        // 
        // lblMembershipEnd
        // 
        lblMembershipEnd.AutoSize = true;
        lblMembershipEnd.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblMembershipEnd.Location = new Point(17, 475);
        lblMembershipEnd.Name = "lblMembershipEnd";
        lblMembershipEnd.Size = new Size(180, 28);
        lblMembershipEnd.TabIndex = 7;
        lblMembershipEnd.Text = "MEMBERSHIP END:";
        // 
        // lblMemberShipStart
        // 
        lblMemberShipStart.AutoSize = true;
        lblMemberShipStart.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblMemberShipStart.Location = new Point(17, 407);
        lblMemberShipStart.Name = "lblMemberShipStart";
        lblMemberShipStart.Size = new Size(195, 28);
        lblMemberShipStart.TabIndex = 6;
        lblMemberShipStart.Text = "MEMBERSHIP START:";
        // 
        // lblBirthDay
        // 
        lblBirthDay.AutoSize = true;
        lblBirthDay.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblBirthDay.Location = new Point(17, 344);
        lblBirthDay.Name = "lblBirthDay";
        lblBirthDay.Size = new Size(109, 28);
        lblBirthDay.TabIndex = 5;
        lblBirthDay.Text = "BIRTH DAY:";
        // 
        // lblAge
        // 
        lblAge.AutoSize = true;
        lblAge.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblAge.Location = new Point(17, 287);
        lblAge.Name = "lblAge";
        lblAge.Size = new Size(53, 28);
        lblAge.TabIndex = 4;
        lblAge.Text = "AGE:";
        // 
        // lblGender
        // 
        lblGender.AutoSize = true;
        lblGender.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblGender.Location = new Point(17, 233);
        lblGender.Name = "lblGender";
        lblGender.Size = new Size(91, 28);
        lblGender.TabIndex = 3;
        lblGender.Text = "GENDER:";
        // 
        // lblPhoneNumber
        // 
        lblPhoneNumber.AutoSize = true;
        lblPhoneNumber.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblPhoneNumber.Location = new Point(17, 179);
        lblPhoneNumber.Name = "lblPhoneNumber";
        lblPhoneNumber.Size = new Size(166, 28);
        lblPhoneNumber.TabIndex = 2;
        lblPhoneNumber.Text = "PHONE NUMBER:";
        // 
        // lblEmail
        // 
        lblEmail.AutoSize = true;
        lblEmail.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblEmail.Location = new Point(17, 125);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(71, 28);
        lblEmail.TabIndex = 1;
        lblEmail.Text = "EMAIL:";
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblName.Location = new Point(17, 69);
        lblName.Name = "lblName";
        lblName.Size = new Size(72, 28);
        lblName.TabIndex = 0;
        lblName.Text = "NAME:";
        // 
        // txtbxFullName
        // 
        txtbxFullName.Font = new Font("Segoe UI", 20F);
        txtbxFullName.Location = new Point(26, 106);
        txtbxFullName.Name = "txtbxFullName";
        txtbxFullName.Size = new Size(316, 52);
        txtbxFullName.TabIndex = 1;
        txtbxFullName.TextChanged += txtbxFullName_TextChanged;
        // 
        // lblFullName
        // 
        lblFullName.AutoSize = true;
        lblFullName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblFullName.Location = new Point(26, 75);
        lblFullName.Name = "lblFullName";
        lblFullName.Size = new Size(119, 28);
        lblFullName.TabIndex = 2;
        lblFullName.Text = "FULL NAME:";
        // 
        // lblMemberShipId
        // 
        lblMemberShipId.AutoSize = true;
        lblMemberShipId.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblMemberShipId.Location = new Point(26, 260);
        lblMemberShipId.Name = "lblMemberShipId";
        lblMemberShipId.Size = new Size(160, 28);
        lblMemberShipId.TabIndex = 4;
        lblMemberShipId.Text = "MEMBERSHIP ID:";
        // 
        // txtbxMemberShipId
        // 
        txtbxMemberShipId.Font = new Font("Segoe UI", 20F);
        txtbxMemberShipId.Location = new Point(26, 291);
        txtbxMemberShipId.Name = "txtbxMemberShipId";
        txtbxMemberShipId.Size = new Size(316, 52);
        txtbxMemberShipId.TabIndex = 3;
        // 
        // btnSearchFullName
        // 
        btnSearchFullName.Location = new Point(26, 172);
        btnSearchFullName.Name = "btnSearchFullName";
        btnSearchFullName.Size = new Size(206, 47);
        btnSearchFullName.TabIndex = 5;
        btnSearchFullName.Text = "SEARCH BY FULLNAME";
        btnSearchFullName.UseVisualStyleBackColor = true;
        btnSearchFullName.Click += btnSearchFullName_Click;
        // 
        // btnSearchMemberShipId
        // 
        btnSearchMemberShipId.Location = new Point(26, 369);
        btnSearchMemberShipId.Name = "btnSearchMemberShipId";
        btnSearchMemberShipId.Size = new Size(206, 56);
        btnSearchMemberShipId.TabIndex = 6;
        btnSearchMemberShipId.Text = "SEARCH BY MEMBERSHIP-ID";
        btnSearchMemberShipId.UseVisualStyleBackColor = true;
        btnSearchMemberShipId.Click += btnSearchMemberShipId_Click;
        // 
        // table1
        // 
        dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
        table1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        table1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        table1.Location = new Point(805, 12);
        table1.Name = "table1";
        table1.RowHeadersWidth = 51;
        table1.Size = new Size(937, 604);
        table1.TabIndex = 7;
        table1.CellContentClick += table1_CellContentClick;
        // 
        // pictureBox1
        // 
        pictureBox1.Location = new Point(65, 487);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(277, 212);
        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        pictureBox1.TabIndex = 8;
        pictureBox1.TabStop = false;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1754, 786);
        Controls.Add(pictureBox1);
        Controls.Add(table1);
        Controls.Add(btnSearchMemberShipId);
        Controls.Add(btnSearchFullName);
        Controls.Add(lblMemberShipId);
        Controls.Add(txtbxMemberShipId);
        Controls.Add(lblFullName);
        Controls.Add(txtbxFullName);
        Controls.Add(grpbxClientMembership);
        Name = "Form1";
        Text = "Form1";
        grpbxClientMembership.ResumeLayout(false);
        grpbxClientMembership.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)table1).EndInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox grpbxClientMembership;
    private Label lblMembershipEnd;
    private Label lblMemberShipStart;
    private Label lblBirthDay;
    private Label lblAge;
    private Label lblGender;
    private Label lblPhoneNumber;
    private Label lblEmail;
    private Label lblName;
    private TextBox txtbxFullName;
    private Label lblFullName;
    private Label lblMemberShipId;
    private TextBox txtbxMemberShipId;
    private Button btnSearchFullName;
    private Button btnSearchMemberShipId;
    private Label lblMembershipStatus;
    private DataGridView table1;
    private PictureBox pictureBox1;
}