﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;


namespace PickMeUpService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class AuthService : IAuthentication
    {
        // Student login() -- fetches password and compares it with the input password then returns true or false.
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "login/student/{usrn}/{pwd}")]
        public Status loginStudent(string usrn, string pwd)
        {
            string password = null;
            bool stats = false;
            try
            {
                SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["pickdb"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;

                
                cmd.CommandText = "SELECT passwd FROM student where studentid ='" + usrn + "'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;

                sqlConnection1.Open();

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        password = reader.GetString(0);
                        if (password == pwd)
                        {
                            stats = true;
                        }
                        else
                        {
                            stats = false;
                        }
                    }   
                }
                else
                {
                       Console.WriteLine("No rows found.");
                }
                reader.Close();
                sqlConnection1.Close();
                
            }
            catch (Exception e)
            {
                e.GetBaseException();

            }
            return new Status()
            {
                status = stats
            };
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "login/volunteer/{usrn}/{pwd}")]
        public Status loginVolunteer(string usrn, string pwd)
        {
            string password = null;
            bool stats = false;
            try
            {
                SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["pickdb"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;

                cmd.CommandText = "SELECT passwd FROM Volunteer where studentid ='" + usrn + "'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;

                sqlConnection1.Open();

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        password = reader.GetString(0);
                        if (password == pwd)
                        {
                            stats = true;
                        }
                        else
                        {
                            stats = false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
                sqlConnection1.Close();

            }
            catch (Exception e)
            {
                e.GetBaseException();

            }
            return new Status()
            {
                status = stats
            };
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "register/student?studentid={sid}&passwd={pwd}&firstname={fname}&lastname={lname}&email={eid}&gender={sex}&arrivaltime={atime}&airlines={airline}&flight={flight}&address={address1}")]
        public int registerStudent(string sid, string pwd,string fname, string lname,string eid,string sex, string atime,string airline,string flight,string address1)
        {
            //atime should be of this format "MM/dd/yyyy HH:mm:ss"
            int result = 39;
            try
            { 
                SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["pickdb"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                result = 45;

                string pattern = "MM/dd/yyyy HH:mm:ss";
                DateTime parsedDate;
                   if (DateTime.TryParseExact(atime, pattern, null, DateTimeStyles.None, out parsedDate))
                       Console.WriteLine("Converted to {1:d}.", atime, parsedDate);
                   else
                       Console.WriteLine("Unable to convert '{0}' to a date and time.", atime);

                   cmd.CommandText = "INSERT into student (studentid,passwd,firstname,lastname,email,gender,arrivaltime,airlines,flight,address1) VALUES ('" + sid + "','" + pwd + "','" + fname + "','" + lname + "','" + eid + "','" + sex + "','" + parsedDate + "','" + airline + "','" + flight + "','" + address1 + "')";
                
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;
                sqlConnection1.Open();
                result = cmd.ExecuteNonQuery();
                sqlConnection1.Close();
                if (result == 1) { 
                IntelligentSystem I = new IntelligentSystem();
                I.assignVolunteer(sid);
                }
            }
            catch (Exception e)
            {
                e.GetBaseException();

            }
            return result;
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "register/volunteer?studentid={sid}&passwd={pwd}&firstname={fname}&lastname={lname}&email={eid}&gender={sex}&phone={ph}&address={address1}&Mon={mon}&Tue={tue}&Wed={wed}&Thu={thu}&Fri={fri}&Sat={sat}&Sun={sun}")]
        public int registerVolunteer(string sid, string pwd, string fname, string lname, string eid, string sex, string ph, string address1, byte mon, byte tue, byte wed, byte thu, byte fri, byte sat, byte sun)
        {
            int result = 35;
            try
            {
                SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["pickdb"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                result = 34;
                cmd.CommandText = "INSERT into Volunteer(studentid,passwd,firstname,lastname,email,gender,phone,address,MonAvailability, TueAvailability, WedAvailability, ThuAvailability, FriAvailability, SatAvailability, SunAvailability) VALUES ('" + sid + "','" + pwd + "','" + fname + "','" + lname + "','" + eid + "','" + sex + "','" + ph + "','" + address1 + "','" + mon + "','" + tue + "','" + wed + "','" + thu + "','" + fri + "','" + sat + "','" + sun + "')";
                
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;

                sqlConnection1.Open();
                result = cmd.ExecuteNonQuery();
                sqlConnection1.Close();
            }
            catch (Exception e)
            {
                e.GetBaseException();

            }
            return result;
        }



        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "login/student/{usrn}")]
        public String[] getStudentDetails(string usrn)
        {
            String[] studentDetails = { "" };

            try
            {
                //connectionString = ConfigurationManager.ConnectionStrings
                System.Diagnostics.Debug.Write("testing username");
                SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["pickdb"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;


                cmd.CommandText = "SELECT firstname,lastname,email,airlines,flight,Volunteerid FROM student where studentid ='" + usrn + "' ";//and password ='" + pwd + "' ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;

                sqlConnection1.Open();

                studentDetails = new String[6];
                reader = cmd.ExecuteReader();

                // Data is accessible through the DataReader object here.
                while (reader.Read())
                {

                    studentDetails[0] = reader[0].ToString();
                    studentDetails[1] = reader[1].ToString();
                    studentDetails[2] = reader[2].ToString();
                    studentDetails[3] = reader[3].ToString();
                    studentDetails[4] = reader[4].ToString();
                    studentDetails[5] = reader[5].ToString();

                }
                reader.Close();
                sqlConnection1.Close();
            }
            catch (Exception e)
            {
                e.GetBaseException();

            }

            return studentDetails;
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "login/volunteer/{usrn}")]
        public StudentList getVolunteerDetails(string usrn)
        {
            StudentList studentRecords = new StudentList();
            studentRecords.studentList = new List<Student>();
            int count = 0;
            try
            {
                //connectionString = ConfigurationManager.ConnectionStrings
                System.Diagnostics.Debug.Write("testing username");
                SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["pickdb"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;


                cmd.CommandText = "SELECT firstname,lastname,address1 FROM student where Volunteerid ='" + usrn + "' ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;

                sqlConnection1.Open();

                SqlCommand getCount = new SqlCommand("Select count(studentid) from student", sqlConnection1);

                count = Convert.ToInt16(getCount.ExecuteScalar().ToString());

                Student[] records = new Student[count];
                reader = cmd.ExecuteReader();

                // Data is accessible through the DataReader object here.
                int i = 0;
                while (reader.Read())
                {

                    records[i] = new Student();
                    records[i].fName = reader["firstname"].ToString();
                    records[i].lName = reader["lastname"].ToString();
                    //records[i].address = Convert.ToInt32(reader["rating"].ToString());
                    records[i].address = reader["address1"].ToString();
                    studentRecords.studentList.Add(records[i]);
                    i++;
                }
                reader.Close();
                sqlConnection1.Close();
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }

            return studentRecords;
        }


        public class Status
        {
            public Status()
            {

            }
            public bool status { get; set; }
        }
       
    }
}
