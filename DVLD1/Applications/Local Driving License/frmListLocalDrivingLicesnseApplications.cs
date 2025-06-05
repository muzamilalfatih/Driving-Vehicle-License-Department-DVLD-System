using DVLD1.Licenses;
using DVLD1.Licenses.Local_Licenses;
using DVLD1.Tests;
using DVLD1_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Applications.Local_Driving_License
{
    public partial class frmListLocalDrivingLicesnseApplications : Form
    {
        private DataTable _dtAllLocalDrivingLicenseApplications;
        public frmListLocalDrivingLicesnseApplications()
        {
            InitializeComponent();
        }
        private void RefreshDataGridView()
        {
            _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();
            dgvLocalDrivingLicenseApplications.DataSource = _dtAllLocalDrivingLicenseApplications;

            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
            if (dgvLocalDrivingLicenseApplications.Rows.Count > 0)
            {

                dgvLocalDrivingLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvLocalDrivingLicenseApplications.Columns[0].Width = 80;

                dgvLocalDrivingLicenseApplications.Columns[1].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplications.Columns[1].Width = 150;

                dgvLocalDrivingLicenseApplications.Columns[2].HeaderText = "National No.";
                dgvLocalDrivingLicenseApplications.Columns[2].Width = 70;

                dgvLocalDrivingLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplications.Columns[3].Width = 200;

                dgvLocalDrivingLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplications.Columns[4].Width = 150;

                dgvLocalDrivingLicenseApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplications.Columns[5].Width = 65;
            }
        }
        private void _ScheduleTest(clsTestType.enTestType TestType)
        {
            int LocalDrivingAppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmListTestAppointments frm = new frmListTestAppointments(LocalDrivingAppID, TestType);
            frm.DataBack += Form2_DataBack;
            frm.ShowDialog();
        }
        private void frmListLocalDrivingLicesnseApplications_Load(object sender, EventArgs e)
        {

            RefreshDataGridView();
            cbFilterBy.SelectedIndex = 0;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

            _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication();
            frm.DataBack += Form2_DataBack;
            frm.ShowDialog();
        }
        private void Form2_DataBack(object sender)
        {
            frmListLocalDrivingLicesnseApplications_Load(null, null);
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingAppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            Form frm = new frmLocalDrivingLicenseApplicationInfo(LocalDrivingAppID);
            frm.ShowDialog();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingAppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication(LocalDrivingAppID);
            frm.DataBack += Form2_DataBack;
            frm.ShowDialog();

        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingAppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            if (MessageBox.Show("Are you sure to delete this application","Confirm",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.OK)
            {
                clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
               LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingAppID);
                if (LocalDrivingLicenseApplication != null)
                {
                    if (LocalDrivingLicenseApplication.Delete())
                    {
                        MessageBox.Show("Application deleted suceesfully!","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        RefreshDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("Coudn't delete application!Other data depends on it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingAppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            if (MessageBox.Show("Are you sure to cancel this application", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {

               clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingAppID);
                if (LocalDrivingLicenseApplication != null)
                {
                    if (LocalDrivingLicenseApplication.Cancel())
                    {
                       
                        MessageBox.Show("Application Cancelled suceesfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGridView();     
                    }
                   

                }
            }
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingAppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingAppID);

            editApplicationToolStripMenuItem.Enabled = LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New && LocalDrivingLicenseApplication.GetPassedTestCount() == 0;

            deleteApplicationToolStripMenuItem.Enabled = LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New && LocalDrivingLicenseApplication.GetPassedTestCount() == 0;
            cancelToolStripMenuItem.Enabled = LocalDrivingLicenseApplication.ApplicationStatus != clsApplication.enApplicationStatus.Completed && LocalDrivingLicenseApplication.ApplicationStatus != clsApplication.enApplicationStatus.Cancelled;
            sceToolStripMenuItem.Enabled = LocalDrivingLicenseApplication.GetPassedTestCount() < 3 && LocalDrivingLicenseApplication.ApplicationStatus != clsApplication.enApplicationStatus.Cancelled;
            scheduleVisionTestToolStripMenuItem.Enabled = LocalDrivingLicenseApplication.GetPassedTestCount() == 0;
            scheduleWrittenTestToolStripMenuItem.Enabled = LocalDrivingLicenseApplication.DoesPassPreviousTest(clsTestType.enTestType.WrittenTest) && !LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            scheduleStreetTestToolStripMenuItem.Enabled = LocalDrivingLicenseApplication.DoesPassPreviousTest(clsTestType.enTestType.StreetTest) && !LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.StreetTest);
            issueDrivingLisenceFirstTimeToolStripMenuItem.Enabled = !clsLicense.IsLicenseExistByPersonID(LocalDrivingLicenseApplication.ApplicantPersonID, LocalDrivingLicenseApplication.LicenseClassID) && LocalDrivingLicenseApplication.GetPassedTestCount() == 3 && LocalDrivingLicenseApplication.ApplicationStatus != clsApplication.enApplicationStatus.Cancelled;
            showLicenseToolStripMenuItem.Enabled = clsLicense.IsLicenseExistByPersonID(LocalDrivingLicenseApplication.ApplicantPersonID,LocalDrivingLicenseApplication.LicenseClassID);
            
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.VisionTest);
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.WrittenTest);
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.StreetTest);
        }

        private void sceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void issueDrivingLisenceFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingAppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmIssueDriverLicenseFirstTime frm = new frmIssueDriverLicenseFirstTime(LocalDrivingAppID);
            frm.DataBack += Form2_DataBack;
            frm.ShowDialog();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingAppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingAppID);
            int LicenseID = LocalDrivingLicenseApplication.GetActiveLicenseID();

            Form frm = new frmShowLicenseInfo(LicenseID);
            frm.ShowDialog();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingAppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingAppID);
            Form frm = new frmShowPersonLicenseHistory(localDrivingLicenseApplication.ApplicantPersonID);
            frm.ShowDialog();
            
        }
    }
}
