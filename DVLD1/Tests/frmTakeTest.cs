using DVLD1.Global_Classes;
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

namespace DVLD1.Tests
{
    public partial class frmTakeTest : Form
    {
        public delegate void DataBackEventHandler(object sender);
        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        private int _TestAppointmentID;
        private clsTest _Test;
        private clsTestAppointment _TestAppoinment;
        private clsTestType.enTestType _TestTypeID;
        public frmTakeTest(int TestAppointmentID, clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestAppointmentID = TestAppointmentID;
            _TestTypeID = TestTypeID;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlSecheduledTest1.TestTypeID = _TestTypeID;
            ctrlSecheduledTest1.LoadData(_TestAppointmentID);
            _TestAppoinment = clsTestAppointment.Find(_TestAppointmentID);
            if (_TestAppoinment.IsLocked)
            {
                _Test = clsTest.Find(_TestAppoinment.TestID);
                
                lblUserMessage.Visible = true;
                rbFail.Checked = !_Test.TestResult;
                rbPass.Checked = _Test.TestResult;
                txtNotes.Text = _Test.Notes;
                rbFail.Enabled = false;
                rbPass.Enabled = false;
                txtNotes.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save? After that you can't change the Pass/Fail results","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_TestAppoinment.IsLocked)
                {
                    
                    _Test.Notes = txtNotes.Text;
                    if (!_Test.Save())
                    {
                        MessageBox.Show("Failed to take test","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    _Test = new clsTest();
                    _Test.TestAppointmentID = _TestAppointmentID;
                    _Test.TestResult = rbPass.Checked;
                    _Test.Notes = txtNotes.Text;
                    _Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;
                    if (!_Test.Save())
                    {
                        MessageBox.Show("Failed to take test", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    _TestAppoinment.Lock();
                }
                MessageBox.Show("Data saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void frmTakeTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this);
        }
    }
}
