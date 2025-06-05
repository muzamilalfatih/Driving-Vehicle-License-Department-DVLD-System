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
    public partial class frmScheduleTest : Form
    {
        public delegate void DataBackEventHandler(object sender);
        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        private clsTestType.enTestType _TestType;
        private int _LocalDrivingLicenseApplicationID;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _TestAppointID;
        public frmScheduleTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType, int TestAppointment = -1)
        {
            InitializeComponent();
            _TestType = TestType;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointID = TestAppointment;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DataBack?.Invoke(this);
            this.Close();
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestTypeID = _TestType;
            ctrlScheduleTest1.LoadTestAppointmentData(_LocalDrivingLicenseApplicationID,_TestAppointID);
        }

        private void frmScheduleTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this);
        }
    }
}
