using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Framework_Lib;
using System.Data.Entity;
using System.Data.SqlClient;

namespace UI
{
    public partial class Form1 : Form
    {
        string serviceUri = @"http://localhost:51000/";
        //string serviceUri = @"http://dbservicew0272050.azurewebsites.net/";
        Default.Container service;
        public List<Applicant> AllApplicants = new List<Applicant>();
        public List<Framework_Lib.Application> AllApplications = new List<Framework_Lib.Application>();
        public Framework_Lib.Application selectedApplication;
        public Framework_Lib.Applicant selectedApplicant;
        public Citizenship selectedAppCitizenship;
        public List<ProgramChoice> selectedAppChoices = new List<ProgramChoice>();
        public Campus selectedCampus;
        public UI.Framework_Lib.Program selectedProgram;

        public Form1()
        {
            InitializeComponent();
            service = new Default.Container(new Uri(serviceUri));
            cbxGender.SelectedIndex = 2;

        }

        public void AltLoadStuff()
        {
            selectedApplication = (Framework_Lib.Application)lbApplications.SelectedItem;
            lblDate.Text = selectedApplication.ApplicationDate;
            lblFee.Text = selectedApplication.ApplicationFee.ToString();
            if (selectedApplication.Paid)
            {
                lblPaid.Text = "True";//idk
            }
            else
            {
                lblPaid.Text = "False";//idk
            }
            selectedApplicant = (from a in service.Applicants
                                where a.ApplicantId == selectedApplication.ApplicantId
                                select a).FirstOrDefault();
            tbApplicantName.Text = selectedApplicant.FirstName + " " + selectedApplicant.LastName;
            lblCity.Text = selectedApplicant.City;
            selectedAppCitizenship = (from c in service.Citizenships
                                      where c.CitizenshipId == selectedApplicant.Citizenship
                                      select c).FirstOrDefault();
            lblCitizenship.Text = selectedAppCitizenship.Description;
            lblCity.Text = selectedApplicant.City;
            lblDOB.Text = selectedApplicant.DOB.ToString();
            lblHPhone.Text = selectedApplicant.HomePhone;
            lblStreet.Text = selectedApplicant.StreetAddress1;
            lblMName.Text = selectedApplicant.MiddleName;
            lblPassword.Text = selectedApplicant.Password;
            lblFName.Text = selectedApplicant.FirstName;
            lblLName.Text = selectedApplicant.LastName;
            lblGender.Text = selectedApplicant.Gender;
            lblCountry.Text = selectedApplicant.CountryCode;

            var progchoice = (from pc in service.ProgramChoices
                              where pc.ApplicationId == selectedApplication.ApplicationId
                              select pc);
            foreach(var choice in progchoice)
            {
                selectedAppChoices.Add(choice);    
            }
            selectedProgram = (from p in service.Programs
                               where p.ProgramID == selectedAppChoices[0].ProgramId
                               select p).FirstOrDefault();
            selectedCampus = (from c in service.Campuses
                              where c.CampusId == selectedAppChoices[0].CampusId
                              select c).FirstOrDefault();
            lblProgram1.Text = selectedProgram.Name;
            lblCampus1.Text = selectedCampus.Name;

            selectedProgram = (from p in service.Programs
                               where p.ProgramID == selectedAppChoices[1].ProgramId
                               select p).FirstOrDefault();
            selectedCampus = (from c in service.Campuses
                              where c.CampusId == selectedAppChoices[1].CampusId
                              select c).FirstOrDefault();
            lblProgram2.Text = selectedProgram.Name;
            lblCampus2.Text = selectedCampus.Name;

        }

        public void LoadStuff()
        {
            AllApplicants = service.Applicants.ToList();//Get all applicants
            AllApplications = service.Applications.ToList();//Get all applications

            lbApplications.DataSource = service.Applications.ToList();
            lbApplications.DisplayMember = "ApplicationDate";

            cbxAcademicYear.DataSource = service.AcademicYears.ToList();
            cbxAcademicYear.DisplayMember = "Description";
            cbxAcademicYear.SelectedIndex = 0;

            cbxCitizenship.DataSource = service.Citizenships.ToList();
            cbxCitizenship.DisplayMember = "Description";
            cbxCitizenship.SelectedIndex = 0;

            cbxCountry.DataSource = service.Countries.ToList();
            cbxCountry.DisplayMember = "CountryName";
            cbxCountry.SelectedIndex = 0;

            cbxCitizenshipOther.DataSource = service.Countries.ToList();
            cbxCitizenshipOther.DisplayMember = "CountryName";
            cbxCitizenshipOther.SelectedIndex = 0;

            cbxProgSel1.DataSource = service.Programs.ToList();
            cbxProgSel1.DisplayMember = "Name";
            cbxProgSel1.SelectedIndex = 0;

            cbxProgSel2.DataSource = service.Programs.ToList();
            cbxProgSel2.DisplayMember = "Name";
            cbxProgSel2.SelectedIndex = 0;

            cbxCampSel1.DataSource = service.Campuses.ToList();
            cbxCampSel1.DisplayMember = "Name";
            cbxCampSel1.SelectedIndex = 0;

            cbxCampSel2.DataSource = service.Campuses.ToList();
            cbxCampSel2.DisplayMember = "Name";
            cbxCampSel2.SelectedIndex = 0;

            cbxProvState.DataSource = service.ProvinceStates.ToList();
            cbxProvState.DisplayMember = "ProvinceStateCode";
            cbxProvState.SelectedIndex = 0;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbCity.Text.Length > 0 && tbFirstName.Text.Length > 0 &&
                tbLastName.Text.Length > 0 && tbStreetAdr.Text.Length > 0 &&
                tbEmail.Text.Length > 0 && tbMiddleName.Text.Length > 0              
                )
            {
                ////New Applicant
                Applicant newApp = new Applicant();
                Country country = (Country)cbxCountry.SelectedItem;
                ProvinceState provstate = (ProvinceState)cbxProvState.SelectedItem;
                Citizenship citizenship = (Citizenship)cbxCitizenship.SelectedItem;
                Country countrycitizen = (Country)cbxCountry.SelectedItem;


                newApp.FirstName = tbFirstName.Text.ToString();
                newApp.MiddleName = tbMiddleName.Text.ToString();
                newApp.LastName = tbLastName.Text.ToString();

                newApp.DOB = DateTime.Now;

                newApp.Gender = cbxGender.SelectedItem.ToString();
                newApp.StreetAddress1 = tbStreetAdr.Text.ToString();
                newApp.City = tbCity.Text.ToString();
                newApp.CellPhone = "FuckPhones";
                newApp.HomePhone = "FuckPhones";
                newApp.WorkPhone = "FuckPhones";
                newApp.Email = tbEmail.Text.ToString();
                newApp.CountryCode = country.CountryCode;
                newApp.Password = "FuckPasswordsToo";
                //Check boxes
                if (chbxAfrican.Checked) { newApp.IsAfricanCanadian = true; }
                else { newApp.IsAfricanCanadian = false; }
                if (chbxAPL.Checked) { newApp.IsCurrentALP = true; }
                else { newApp.IsCurrentALP = false; }
                if (chbxCAR.Checked) { newApp.OnChildAbuseRegistry = true; }
                else { newApp.OnChildAbuseRegistry = false; }
                if (chbxCriminal.Checked) { newApp.HasCriminalConviction = true; }
                else { newApp.HasCriminalConviction = false; }
                if (chbxDisability.Checked) { newApp.HasDisability = true; }
                else { newApp.HasDisability = false; }
                if (chbxDiscip.Checked) { newApp.HasDisciplinaryAction = true; }
                else { newApp.HasDisciplinaryAction = false; }
                if (chbxFirstNations.Checked) { newApp.IsFirstNations = true; }
                else { newApp.IsFirstNations = false; }

                if (country.CountryName == "Canada" || country.CountryName == "United States")
                {
                    newApp.ProvinceStateCode = provstate.ProvinceStateCode;
                    newApp.ProvinceStateOther = null;
                }
                else
                {
                    newApp.ProvinceStateCode = null;
                    newApp.ProvinceStateOther = tbProvStateOther.Text.ToString();
                }

                if (cbxCitizenship.SelectedIndex != 4)
                {
                    newApp.Citizenship = citizenship.CitizenshipId;
                    newApp.CitizenshipOther = countrycitizen.CountryCode;
                }
                else
                {
                    newApp.Citizenship = citizenship.CitizenshipId;
                    newApp.CitizenshipOther = countrycitizen.CountryCode;
                }

                Framework_Lib.Application application = new Framework_Lib.Application();
                application.Paid = false;
                application.ApplicationDate = DateTime.Now.ToString();
                application.ApplicationFee = 5000000;

                //Programs
                Framework_Lib.Program program1 = (Framework_Lib.Program)cbxProgSel1.SelectedItem;
                Framework_Lib.Program program2 = (Framework_Lib.Program)cbxProgSel2.SelectedItem;

                //Campuses
                Campus campus1 = (Campus)cbxCampSel1.SelectedItem;
                Campus campus2 = (Campus)cbxCampSel2.SelectedItem;



                ProgramChoice progchoice1 = new ProgramChoice();
                progchoice1.CampusId = campus1.CampusId;
                progchoice1.ProgramId = program1.ProgramID;
                ProgramChoice progchoice2 = new ProgramChoice();
                progchoice2.CampusId = campus2.CampusId;
                progchoice2.ProgramId = program2.ProgramID;


                try
                {
                    service.AddToApplicants(newApp);
                    LoadStuff();//Reload for getting the new ID for applicant
                    service.SaveChanges();

                    Applicant lastApp = AllApplicants.LastOrDefault();
                    application.ApplicantId = lastApp.ApplicantId+1;
                    service.AddToApplications(application);
                    LoadStuff();//same thing as above but for application
                    service.SaveChanges();


                    Framework_Lib.Application lastApplication = AllApplications.LastOrDefault();
                    progchoice1.ApplicationId = lastApplication.ApplicationId + 1;
                    progchoice2.ApplicationId = lastApplication.ApplicationId + 1;

                    service.AddToProgramChoices(progchoice1);
                    service.AddToProgramChoices(progchoice2);


                    service.SaveChanges();
                    tbCity.Text = "";
                    tbEmail.Text = "";
                    tbFirstName.Text = "";
                    tbLastName.Text = "";
                    tbMiddleName.Text = "";
                    tbProvStateOther.Text = "";
                    tbStreetAdr.Text = "";
                    MessageBox.Show("Your application & information was submitted!", "Success!");
                    LoadStuff();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields to send your application for reviewal.", "Information Error");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStuff();
        }

        private void cbxProgSel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Framework_Lib.Program program = (Framework_Lib.Program)cbxProgSel1.SelectedItem;

            service.LoadProperty(program, "Campuses");
            cbxCampSel1.DataSource = program.Campuses;


        }

        private void cbxProgSel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UI.Framework_Lib.Program program = (UI.Framework_Lib.Program)cbxProgSel2.SelectedItem;

            service.LoadProperty(program, "Campuses");
            cbxCampSel2.DataSource = program.Campuses;

        }

        private void cbxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            UI.Framework_Lib.Country country = (UI.Framework_Lib.Country)cbxCountry.SelectedItem;
            if (country.CountryName == "Canada" || country.CountryName == "United States")
            {
                cbxProvState.Enabled = true;
                cbxProvState.DataSource = service.ProvinceStates.Where(c => c.CountryCode == country.CountryCode).ToList();
                tbProvStateOther.Enabled = false;
            }
            else
            {
                tbProvStateOther.Enabled = true;
                cbxProvState.Enabled = false;
            }
        }

        private void cbxCitizenship_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCitizenship.SelectedIndex != 3)
            {
                cbxCitizenshipOther.Enabled = false;
            }
            else
            {
                cbxCitizenshipOther.Enabled = true;
            }
        }

        private void lbApplications_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            AltLoadStuff();
        }

        private void lblPaid_Click(object sender, EventArgs e)
        {

        }
    }
}
