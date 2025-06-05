﻿using DVLD1.Global_Classes;
using DVLD1.Properties;
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

namespace DVLD1.Tests.Controls
{
    
    public partial class ctrlScheduleTest : UserControl
    {

        public enum enMode { AddNew, Update };
        private enMode _Mode = enMode.AddNew;
        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 };
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;


        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsTestAppointment _TestAppointment;
        private int _AppointmentID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;
        
        public clsTestType.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value;
                switch (_TestTypeID )
                {
                    case clsTestType.enTestType.VisionTest:
                        gbTestType.Text = "Vision Test";
                        pbTestTypeImage.Image = Resources.Vision_512; 
                        break;
                    case clsTestType.enTestType.StreetTest:
                        gbTestType.Text = "Street Test";
                        pbTestTypeImage.Image = Resources.Schedule_Test_512;
                        break;
                    case clsTestType.enTestType.WrittenTest:
                        gbTestType.Text = "Written Test";
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;

                }
            }
        }
        public void LoadTestAppointmentData (int LocalDrivingLicenseApplicationID, int AppointmentID = -1)
        {
            _LocalDrivingLicenseApplicationID= LocalDrivingLicenseApplicationID;
            _AppointmentID = AppointmentID;
            if (AppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
            
           _LocalDrivingLicenseApplication =  clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show($"Error: No local Driving license Application With ID = {_LocalDrivingLicenseApplication}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }
            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblFullName.Text = _LocalDrivingLicenseApplication.ApplicantFullName.ToString();
            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTrialsPerTest(_TestTypeID).ToString();

            if ( _LocalDrivingLicenseApplication.DoesAttendTestType(_TestTypeID))
            {
                _CreationMode = enCreationMode.RetakeTestSchedule;
            }
            if (_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                lblRetakeAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeTestAppID.Text = "0";
            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schedule Test";
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }
            if (_Mode == enMode.AddNew)
            {
        
                lblFees.Text = clsTestType.Find(_TestTypeID).Fees.ToString();
                dtpTestDate.MinDate = DateTime.Now;
                lblRetakeTestAppID.Text = "N/A";

                _TestAppointment = new clsTestAppointment();
            }

            else
            {

                if (!_LoadTestAppointmentData())
                    return;
            }

            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRetakeAppFees.Text)).ToString();

            if (!_HandleActiveTestAppointmentConstraint())
                return;

            if (!_HandleAppointmentLockedConstraint())
                return;

            if (!_HandlePrviousTestConstraint())
                return;

        }
        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_AppointmentID);
            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _AppointmentID.ToString(),
               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text = _TestAppointment.PaidFees.ToString();

            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
            {
                dtpTestDate.MinDate = DateTime.Now;
            }
            else
            {
                dtpTestDate.MinDate= _TestAppointment.AppointmentDate;
            }
            dtpTestDate.Value = _TestAppointment.AppointmentDate;
            if (_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";

            }
            else
            {
                gbRetakeTestInfo.Enabled = true;
                lblRetakeTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
                lblRetakeAppFees.Text = _TestAppointment.RetakeTestAppInfo.PaidFees.ToString();
            }
            return true;
        }

        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.AddNew && clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID,TestTypeID))
            {
                lblUserMessage.Text = "Person Already have an active appointment for this test";
                lblUserMessage.Visible = true;
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }
            return true;
        }
        private bool _HandleAppointmentLockedConstraint()
        {
            if (_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person already sat for the test, appointment loacked.";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                return false;

            }
            else
                lblUserMessage.Visible = false;

            return true;
        }
        private bool _HandlePrviousTestConstraint()
        {
            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    
                    lblUserMessage.Visible = false;

                    return true;

                case clsTestType.enTestType.WrittenTest:
                   
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Vision Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                    }


                    return true;

                case clsTestType.enTestType.StreetTest:

                    
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Written Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                    }


                    return true;

            }
            return true;

        }
        private bool _HandleRetakeApplication()
        {
            //this will decide to create a seperate application for retake test or not.
            // and will create it if needed , then it will linkit to the appoinment.
            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                //incase the mode is add new and creation mode is retake test we should create a seperate application for it.
                //then we linke it with the appointment.

                //First Create Applicaiton 
                clsApplication Application = new clsApplication();

                Application.ApplicantPersonID = _LocalDrivingLicenseApplication.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees;
                Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if (!Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                _TestAppointment.RetakeTestApplicationID = Application.ApplicationID;
                   
            }
            return true;
        }
        public ctrlScheduleTest()
        {
            InitializeComponent();  
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
