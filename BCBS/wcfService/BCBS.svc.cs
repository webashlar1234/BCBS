using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using wcfService.Model;

namespace wcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BCBS" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BCBS.svc or BCBS.svc.cs at the Solution Explorer and start debugging.
    public class BCBS : IBCBS
    {
        #region Project
        //PROJECT
        public string GetProjectList()
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM project", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Project> dataResult = new List<Project>();
                dataResult = (from DataRow dr in dt.Rows
                              select new Project()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  ChargeCode = dr["CHARGE_CODE"].ToString(),
                                  Description = dr["DESCRIPTION"].ToString(),
                                  HighLevelBudget = dr["HIGH_LEVEL_BUDGET"].ToString(),
                                  Status = dr["STATUS"].ToString(),
                                  Name = dr["NAME"].ToString(),
                                  RC = dr["RC"].ToString()
                              }).ToList();
                con.Close();
                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return result;
        }
        public long InsertProject(string name, string charge_code, string high_level_budget, string status, string description, string rc, string glaccount)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO project (NAME,CHARGE_CODE,HIGH_LEVEL_BUDGET,STATUS,DESCRIPTION,RC,GLACCOUNT) VALUES (@name,@charge_code,@high_level_budget,@status,@description,@rc,@glaccount)";
                //cmd.Parameters.AddWithValue("@id", project.Id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@charge_code", charge_code);
                cmd.Parameters.AddWithValue("@high_level_budget", Convert.ToDouble(high_level_budget));
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@rc", rc);
                cmd.Parameters.AddWithValue("@glaccount", glaccount);
                cmd.ExecuteNonQuery();
                resultId = cmd.LastInsertedId;
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return resultId;
        }
        public long UpdateProjectById(long Id, string name, string charge_code, string high_level_budget, string status, string description, string rc, string glaccount)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "Update project set NAME='" + name + "',CHARGE_CODE='" + charge_code + "',HIGH_LEVEL_BUDGET='" + Convert.ToDouble(high_level_budget) + "',STATUS='" + status + "',DESCRIPTION='" + description + "',RC='" + rc + "',GLACCOUNT='" + glaccount + "' where ID=" + Id;
                resultId = cmd.ExecuteNonQuery();
                if (resultId > 0)
                {
                    resultId = Id;
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return resultId;
        }
        public string GetProjectById(long id)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM project where ID='" + id + "'", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                Project dataResult = new Project();
                dataResult = (from DataRow dr in dt.Rows
                              select new Project()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  ChargeCode = dr["CHARGE_CODE"].ToString(),
                                  Description = dr["DESCRIPTION"].ToString(),
                                  HighLevelBudget = dr["HIGH_LEVEL_BUDGET"].ToString(),
                                  Status = dr["STATUS"].ToString(),
                                  Name = dr["NAME"].ToString(),
                                  RC = dr["RC"].ToString(),
                                  GLAccount = dr["GLACCOUNT"].ToString()
                              }).FirstOrDefault();
                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
                if (result == "null")
                {
                    result = "";
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return result;
        }
        public bool DeleteProjectById(string id)
        {
            bool isDeleted = false;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "DELETE FROM project where ID in (" + id + ")";
                //cmd.Parameters.AddWithValue("@id", project.Id);
                int isexecute = cmd.ExecuteNonQuery();
                if (isexecute > 0)
                {
                    isDeleted = true;
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return isDeleted;
        }
        #endregion

        #region Service
        //SERVICE TYPES
        public string GetServiceTypeList()
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM servicetype", con);
                MySqlCommand cmd = new MySqlCommand("SELECT st.*,if(p.name !='',p.name,'N/A') as PROJECTNAME FROM servicetype st left join project p on st.projectid = p.id", con);

                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<ServiceTypeList> dataResult = new List<ServiceTypeList>();
                dataResult = (from DataRow dr in dt.Rows
                              select new ServiceTypeList()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  Name = dr["NAME"].ToString(),
                                  //ProjectId = Convert.ToDouble(dr["PROJECTID"].ToString()),
                                  ProjectName = dr["PROJECTNAME"].ToString(),
                                  Budget = Convert.ToDouble((dr["BUDGET"] != DBNull.Value ? dr["BUDGET"] : 0)),
                                  FeesType = dr["FEESTYPE"].ToString(),
                                  Status = dr["STATUS"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return result;
        }
        public long InsertServiceType(string name, long? projectid, string status, string feestype, double budget, string notes)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO servicetype (NAME,PROJECTID,STATUS,FEESTYPE,BUDGET,NOTES) VALUES (@name,@projectid,@status,@feestype,@budget,@notes)";
                //cmd.Parameters.AddWithValue("@id", project.Id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@projectid", projectid);
                cmd.Parameters.AddWithValue("@budget", budget);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@feestype", feestype);
                //cmd.Parameters.AddWithValue("@volume", volume);
                cmd.Parameters.AddWithValue("@notes", notes);
                //cmd.Parameters.AddWithValue("@projectid", projectid);
                cmd.ExecuteNonQuery();
                resultId = cmd.LastInsertedId;
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return resultId;
        }
        public long UpdateServiceTypeById(long Id, string name, long? projectid, string status, string notes)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                if (projectid == null)
                {
                    cmd.CommandText = "Update servicetype set NAME='" + name + "',PROJECTID=null,STATUS='" + status + "',NOTES='" + notes + "' where ID = " + Id;

                }
                else
                {
                    cmd.CommandText = "Update servicetype set NAME='" + name + "',PROJECTID='" + projectid.ToString() + "',STATUS='" + status + "',NOTES='" + notes + "' where ID = " + Id;
                }
                resultId = cmd.ExecuteNonQuery();
                if (resultId > 0)
                {
                    resultId = Id;
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return resultId;
        }
        public string GetServiceTypeById(long id)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM servicetype where ID='" + id + "'", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                ServiceType dataResult = new ServiceType();
                dataResult = (from DataRow dr in dt.Rows
                              select new ServiceType()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  ProjectId = Convert.ToInt64((dr["PROJECTID"] != DBNull.Value ? dr["PROJECTID"] : null)),
                                  Name = dr["NAME"].ToString(),
                                  Budget = Convert.ToDouble((dr["BUDGET"] != DBNull.Value ? dr["BUDGET"] : 0)),
                                  FeesType = dr["FEESTYPE"].ToString(),
                                  Status = dr["STATUS"].ToString(),
                                  //Volume = dr["VOLUME"].ToString(),
                                  Notes = dr["NOTES"].ToString()
                              }).FirstOrDefault();
                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
                if (result == "null")
                {
                    result = "";
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return result;
        }
        public string GetServiceTypesByProjectId(long projectid)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM servicetype where PROJECTID='" + projectid + "'", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<ServiceType> dataResult = new List<ServiceType>();
                dataResult = (from DataRow dr in dt.Rows
                              select new ServiceType()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  ProjectId = Convert.ToInt64((dr["PROJECTID"] != DBNull.Value ? dr["PROJECTID"] : null)),
                                  Name = dr["NAME"].ToString(),
                                  //Budget = Convert.ToDouble(dr["BUDGET"].ToString()),
                                  //FeesType = dr["FEESTYPE"].ToString(),
                                  Status = dr["STATUS"].ToString(),
                                  //Volume = dr["VOLUME"].ToString(),
                                  Notes = dr["NOTES"].ToString()
                              }).ToList();
                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
                if (result == "null")
                {
                    result = "";
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return result;
        }
        public bool DeleteServiceTypeById(string id)
        {
            bool isDeleted = false;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                cmd = con.CreateCommand();
                cmd.CommandText = "DELETE FROM servicetype where ID in (" + id + ")";

                //cmd.Parameters.AddWithValue("@id", project.Id);
                int isexecute = cmd.ExecuteNonQuery();
                if (isexecute > 0)
                {
                    isDeleted = true;
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return isDeleted;
        }
        //public long InsertServiceFeesType(long serviceid, string feestype, double amount)
        //{
        //    long resultId = 0;
        //    MySqlConnection con = new MySqlConnection(GetConnectionString());
        //    MySqlCommand cmd = new MySqlCommand();
        //    try
        //    {
        //        if (con.State != ConnectionState.Open)
        //        {
        //            con.Open();
        //        }
        //        cmd = con.CreateCommand();
        //        cmd.CommandText = "INSERT INTO servicefeestype (SERVICEID,FEESTYPE,AMOUNT) VALUES (@serviceid,@feestype,@amount)";
        //        //cmd.Parameters.AddWithValue("@id", project.Id);
        //        cmd.Parameters.AddWithValue("@serviceid", serviceid);
        //        cmd.Parameters.AddWithValue("@feestype", feestype);
        //        cmd.Parameters.AddWithValue("@amount", amount);
        //        //cmd.Parameters.AddWithValue("@projectid", projectid);
        //        cmd.ExecuteNonQuery();
        //        resultId = cmd.LastInsertedId;
        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    //finally
        //    //{

        //    //}
        //    return resultId;
        //}
        //public string GetServiceFeesTypeByServiceId(long serviceid)
        //{
        //    //
        //    string result = string.Empty;
        //    MySqlConnection con = new MySqlConnection(GetConnectionString());
        //    try
        //    {
        //        if (con.State != ConnectionState.Open)
        //        {
        //            con.Open();
        //        }
        //        MySqlCommand cmd = new MySqlCommand("select * from servicefeestype where serviceid = '" + serviceid + "'", con);
        //        //cmd.CommandText = "SELECT * FROM project";
        //        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //        da.SelectCommand = cmd;
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        List<ServiceFeesType> dataResult = new List<ServiceFeesType>();
        //        dataResult = (from DataRow dr in dt.Rows
        //                      select new ServiceFeesType()
        //                      {
        //                          Id = Convert.ToInt64(dr["ID"]),
        //                          FeesType = dr["FEESTYPE"].ToString(),
        //                          Amount = Convert.ToDouble(dr["AMOUNT"].ToString()),
        //                          //FeesType = dr["FEESTYPE"].ToString(),
        //                          ServiceId = Convert.ToInt64(dr["SERVICEID"].ToString()),
        //                      }).ToList();
        //        //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
        //        result = JsonConvert.SerializeObject(dataResult);
        //        if (result == "null")
        //        {
        //            result = "";
        //        }
        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    //finally
        //    //{

        //    //}
        //    return result;
        //}
        //public string GetServiceFeesTypeById(long id)
        //{
        //    string result = string.Empty;
        //    MySqlConnection con = new MySqlConnection(GetConnectionString());
        //    try
        //    {
        //        if (con.State != ConnectionState.Open)
        //        {
        //            con.Open();
        //        }
        //        MySqlCommand cmd = new MySqlCommand("select * from servicefeestype where serviceid = '" + id + "'", con);
        //        //cmd.CommandText = "SELECT * FROM project";
        //        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //        da.SelectCommand = cmd;
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        ServiceFeesType dataResult = new ServiceFeesType();
        //        dataResult = (from DataRow dr in dt.Rows
        //                      select new ServiceFeesType()
        //                      {
        //                          Id = Convert.ToInt64(dr["ID"]),
        //                          FeesType = dr["FEESTYPE"].ToString(),
        //                          Amount = Convert.ToDouble(dr["AMOUNT"].ToString()),
        //                          //FeesType = dr["FEESTYPE"].ToString(),
        //                          ServiceId = Convert.ToInt64(dr["SERVICEID"].ToString()),
        //                      }).FirstOrDefault();
        //        //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
        //        result = JsonConvert.SerializeObject(dataResult);
        //        if (result == "null")
        //        {
        //            result = "";
        //        }
        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    //finally
        //    //{

        //    //}
        //    return result;
        //}
        //public bool DeleteServiceFeesTypeByServiceId(string id)
        //{
        //    bool isDeleted = false;
        //    MySqlConnection con = new MySqlConnection(GetConnectionString());
        //    MySqlCommand cmd = new MySqlCommand();
        //    try
        //    {
        //        if (con.State != ConnectionState.Open)
        //        {
        //            con.Open();
        //        }

        //        cmd = con.CreateCommand();
        //        cmd.CommandText = "DELETE FROM servicefeestype where SERVICEID in (" + id + ")";
        //        //cmd.Parameters.AddWithValue("@id", project.Id);
        //        int isexecute = cmd.ExecuteNonQuery();
        //        if (isexecute > 0)
        //        {
        //            isDeleted = true;
        //        }
        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    //finally
        //    //{

        //    //}
        //    return isDeleted;
        //}
        #endregion

        #region Customer
        //Customer
        public string GetcustomerList()
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM customer", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Customer> dataResult = new List<Customer>();
                dataResult = (from DataRow dr in dt.Rows
                              select new Customer()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  Name = dr["NAME"].ToString(),
                                  ChargeCode = dr["CHARGE_CODE"].ToString(),
                                  CustomerType = dr["CUSTOMER_TYPE"].ToString(),
                                  CustomerAddress = dr["ADDRESS"].ToString(),
                                  City = dr["CITY"].ToString(),
                                  PostalCode = dr["POSTAL_CODE"].ToString(),
                                  State = dr["STATE"].ToString(),
                                  Country = dr["COUNTRY"].ToString(),
                                  FirstName = dr["FIRST_NAME"].ToString(),
                                  LastName = dr["LAST_NAME"].ToString(),
                                  Phone = dr["PHONE"].ToString(),
                                  Fax = dr["FAX"].ToString(),
                                  Occupation = dr["OCCUPATION"].ToString(),
                                  Email = dr["EMAIL"].ToString(),
                                  Status = dr["STATUS"].ToString(),

                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetCustomerListForSBF()
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select cust.* from activity a join contract c on a.contractid =c.id join customer cust on c.customerid = cust.id join  project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where isbilled = false group by cust.name having cust.customer_type = 'ExternalVendor'", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Customer> dataResult = new List<Customer>();
                dataResult = (from DataRow dr in dt.Rows
                              select new Customer()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  Name = dr["NAME"].ToString(),
                                  ChargeCode = dr["CHARGE_CODE"].ToString(),
                                  CustomerType = dr["CUSTOMER_TYPE"].ToString(),
                                  CustomerAddress = dr["ADDRESS"].ToString(),
                                  City = dr["CITY"].ToString(),
                                  PostalCode = dr["POSTAL_CODE"].ToString(),
                                  State = dr["STATE"].ToString(),
                                  Country = dr["COUNTRY"].ToString(),
                                  FirstName = dr["FIRST_NAME"].ToString(),
                                  LastName = dr["LAST_NAME"].ToString(),
                                  Phone = dr["PHONE"].ToString(),
                                  Fax = dr["FAX"].ToString(),
                                  Occupation = dr["OCCUPATION"].ToString(),
                                  Email = dr["EMAIL"].ToString(),
                                  Status = dr["STATUS"].ToString(),

                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetCustomerListForPlanCustomer()
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select cust.* from activity a join contract c on a.contractid =c.id join customer cust on c.customerid = cust.id join  project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where isbilled = false and a.estimate = false  group by cust.name having cust.customer_type = 'Plan'", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Customer> dataResult = new List<Customer>();
                dataResult = (from DataRow dr in dt.Rows
                              select new Customer()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  Name = dr["NAME"].ToString(),
                                  ChargeCode = dr["CHARGE_CODE"].ToString(),
                                  CustomerType = dr["CUSTOMER_TYPE"].ToString(),
                                  CustomerAddress = dr["ADDRESS"].ToString(),
                                  City = dr["CITY"].ToString(),
                                  PostalCode = dr["POSTAL_CODE"].ToString(),
                                  State = dr["STATE"].ToString(),
                                  Country = dr["COUNTRY"].ToString(),
                                  FirstName = dr["FIRST_NAME"].ToString(),
                                  LastName = dr["LAST_NAME"].ToString(),
                                  Phone = dr["PHONE"].ToString(),
                                  Fax = dr["FAX"].ToString(),
                                  Occupation = dr["OCCUPATION"].ToString(),
                                  Email = dr["EMAIL"].ToString(),
                                  Status = dr["STATUS"].ToString(),

                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public long Insertcustomer(string name, string charge_code, string customertype, string address, string city, string postalcode, string state, string country, string firstname, string lastname, string phone, string fax, string occupation, string email, string status)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO customer (NAME,CHARGE_CODE,CUSTOMER_TYPE,ADDRESS,CITY,POSTAL_CODE,STATE,COUNTRY,FIRST_NAME,LAST_NAME,PHONE,FAX,OCCUPATION,EMAIL,STATUS) VALUES" +
                                  "(@name,@charge_code,@customer_type,@address,@city,@postalcode,@state,@country,@firstname,@lastname,@phone,@fax,@occupation,@email,@status)";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@charge_code", charge_code);
                cmd.Parameters.AddWithValue("@customer_type", customertype);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@postalcode", postalcode);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@country", country);
                cmd.Parameters.AddWithValue("@firstname", firstname);
                cmd.Parameters.AddWithValue("@lastname", lastname);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@fax", fax);
                cmd.Parameters.AddWithValue("@occupation", occupation);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.ExecuteNonQuery();
                resultId = cmd.LastInsertedId;
            }
            catch (Exception ex)
            {

            }
            return resultId;
        }
        public long UpdatecustomerById(long Id, string name, string charge_code, string customertype, string address, string city, string postalcode, string state, string country, string firstname, string lastname, string phone, string fax, string occupation, string email, string status)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "Update customer set NAME='" + name + "',CHARGE_CODE='" + charge_code + "',CUSTOMER_TYPE='" + customertype + "',ADDRESS='" + address +
                    "',CITY='" + city + "',POSTAL_CODE='" + postalcode + "',STATE='" + state + "',COUNTRY='" + country + "',FIRST_NAME='" + firstname + "',LAST_NAME='" + lastname +
                    "',PHONE='" + phone + "',FAX='" + fax + "',OCCUPATION='" + occupation + "',EMAIL='" + email + "',STATUS='" + status + "' where ID=" + Id;
                resultId = cmd.ExecuteNonQuery();
                if (resultId > 0)
                {
                    resultId = Id;
                }
            }
            catch (Exception ex)
            {

            }
            return resultId;
        }
        public string GetcustomerById(long id)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM customer where ID='" + id + "'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                Customer dataResult = new Customer();
                dataResult = (from DataRow dr in dt.Rows
                              select new Customer()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  Name = dr["NAME"].ToString(),
                                  ChargeCode = dr["CHARGE_CODE"].ToString(),
                                  CustomerType = dr["CUSTOMER_TYPE"].ToString(),
                                  CustomerAddress = dr["ADDRESS"].ToString(),
                                  City = dr["CITY"].ToString(),
                                  PostalCode = dr["POSTAL_CODE"].ToString(),
                                  State = dr["STATE"].ToString(),
                                  Country = dr["COUNTRY"].ToString(),
                                  FirstName = dr["FIRST_NAME"].ToString(),
                                  LastName = dr["LAST_NAME"].ToString(),
                                  Phone = dr["PHONE"].ToString(),
                                  Fax = dr["FAX"].ToString(),
                                  Occupation = dr["OCCUPATION"].ToString(),
                                  Email = dr["EMAIL"].ToString(),
                                  Status = dr["STATUS"].ToString()
                              }).FirstOrDefault();
                result = JsonConvert.SerializeObject(dataResult);
                if (result == "null")
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetcustomerByContractId(long id)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select cust.* from customer cust join contract c on cust.id= c.customerId where c.id='" + id + "'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                Customer dataResult = new Customer();
                dataResult = (from DataRow dr in dt.Rows
                              select new Customer()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  Name = dr["NAME"].ToString(),
                                  ChargeCode = dr["CHARGE_CODE"].ToString(),
                                  CustomerType = dr["CUSTOMER_TYPE"].ToString(),
                                  CustomerAddress = dr["ADDRESS"].ToString(),
                                  City = dr["CITY"].ToString(),
                                  PostalCode = dr["POSTAL_CODE"].ToString(),
                                  State = dr["STATE"].ToString(),
                                  Country = dr["COUNTRY"].ToString(),
                                  FirstName = dr["FIRST_NAME"].ToString(),
                                  LastName = dr["LAST_NAME"].ToString(),
                                  Phone = dr["PHONE"].ToString(),
                                  Fax = dr["FAX"].ToString(),
                                  Occupation = dr["OCCUPATION"].ToString(),
                                  Email = dr["EMAIL"].ToString(),
                                  Status = dr["STATUS"].ToString()
                              }).FirstOrDefault();
                result = JsonConvert.SerializeObject(dataResult);
                if (result == "null")
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public bool DeletecustomerById(string id)
        {
            bool isDeleted = false;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "DELETE FROM customer where ID in (" + id + ")";
                //cmd.Parameters.AddWithValue("@id", project.Id);
                int isexecute = cmd.ExecuteNonQuery();
                if (isexecute > 0)
                {
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return isDeleted;
        }
        #region Invoice

        public string GetContractByCustomerId(long id)
        {
            //select p.name,st.name,p.rc,p.charge_code,c.amount from contract c join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where customerid = 9
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("select p.name as PROJECTNAME,st.name as SERVICENAME,p.rc as RC,p.charge_code as PROJECTCODE,c.amount as AMOUNT from contract c join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where customerid = '" + id + "'", con);
                MySqlCommand cmd = new MySqlCommand("select c.id as ID,c.contractcode as CONTRACTCODE, p.name as PROJECTNAME,st.name as SERVICENAME,c.amount as AMOUNT,IF(c.DIRRECTION = 1,'Revenue','Expense') as CHARGES,c.FromDate as FROMDATE,c.EndDate as ENDDATE from contract c join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where c.customerid = '" + id + "'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<CustomerContract> dataResult = new List<CustomerContract>();
                dataResult = (from DataRow dr in dt.Rows
                              select new CustomerContract()
                              {
                                  ContractId = Convert.ToInt64(dr["ID"].ToString()),
                                  ContractCode = dr["CONTRACTCODE"].ToString(),
                                  ProjectName = dr["PROJECTNAME"].ToString(),
                                  ServiceName = dr["SERVICENAME"].ToString(),
                                  Amount = Convert.ToDouble(dr["AMOUNT"].ToString()),
                                  Charges = dr["CHARGES"].ToString(),
                                  FromDate = Convert.ToDateTime(dr["FROMDATE"].ToString()),
                                  EndDate = Convert.ToDateTime(dr["ENDDATE"].ToString())
                              }).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetContractbyCustomerIdHaveActivity(long id)
        {
            //select c.id as ID,c.contractcode as CONTRACTCODE, p.name as PROJECTNAME,st.name as SERVICENAME,c.amount as AMOUNT,IF(c.DIRRECTION = 1,'Revenue','Expense') as CHARGES,c.FromDate as FROMDATE,c.EndDate as ENDDATE from activity a join contract c on a.contractid = c.id join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where c.customerid = 4 
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("select p.name as PROJECTNAME,st.name as SERVICENAME,p.rc as RC,p.charge_code as PROJECTCODE,c.amount as AMOUNT from contract c join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where customerid = '" + id + "'", con);
                //MySqlCommand cmd = new MySqlCommand("select c.id as ID,c.contractcode as CONTRACTCODE, p.name as PROJECTNAME,st.name as SERVICENAME,c.amount as AMOUNT,IF(a.DIRRECTION = 1,'Revenue','Expense') as CHARGES,IF(a.Estimate = 1,'Actual','Projected') as ESTIMATE,c.FromDate as FROMDATE,c.EndDate as ENDDATE from activity a join contract c on a.contractid = c.id join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where c.customerid =" + id + " and a.isbilled = false group by c.id", con);
                MySqlCommand cmd = new MySqlCommand("select c.id as ID,c.contractcode as CONTRACTCODE, p.name as PROJECTNAME,st.name as SERVICENAME,c.amount as AMOUNT,IF(a.Charges = 1,'Revenue','Expense') as CHARGES,IF(a.Estimate = 1,'Actual','Projected') as ESTIMATE,c.FromDate as FROMDATE,c.EndDate as ENDDATE from activity a join contract c on a.contractid = c.id join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where c.customerid =" + id + " and a.isbilled = false group by c.id", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<CustomerContract> dataResult = new List<CustomerContract>();
                dataResult = (from DataRow dr in dt.Rows
                              select new CustomerContract()
                              {
                                  ContractId = Convert.ToInt64(dr["ID"].ToString()),
                                  ContractCode = dr["CONTRACTCODE"].ToString(),
                                  ProjectName = dr["PROJECTNAME"].ToString(),
                                  ServiceName = dr["SERVICENAME"].ToString(),
                                  Amount = Convert.ToDouble(dr["AMOUNT"].ToString()),
                                  Charges = dr["CHARGES"].ToString(),
                                  FromDate = Convert.ToDateTime(dr["FROMDATE"].ToString()),
                                  EndDate = Convert.ToDateTime(dr["ENDDATE"].ToString()),
                                  Estimate = dr["ESTIMATE"].ToString()
                              }).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetActivitiesByCustomerId(long id)
        {
            // 
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("select p.name as PROJECTNAME,st.name as SERVICENAME,p.rc as RC,p.charge_code as PROJECTCODE,IF(c.DIRRECTION = 1, c.amount,-c.amount) as AMOUNT,IF(c.DIRRECTION = 1,'Revenue','Expense') as CHARGES from contract c join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where customerid = '" + id + "'", con);
                MySqlCommand cmd = new MySqlCommand("select a.id as ACTIVITYID, p.name as PROJECTNAME,st.name as SERVICENAME,p.rc as RC,a.FROMDATE as FROMDATE,a.ENDDATE as ENDDATE,p.charge_code as PROJECTCODE,IF(c.DIRRECTION = 1, c.amount,-c.amount) as AMOUNT,IF(c.DIRRECTION = 1,'Revenue','Expense') as CHARGES  from activity a join contract c on a.contractid = c.id join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where c.customerid = '" + id + "'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<ContractInvoice> dataResult = new List<ContractInvoice>();
                dataResult = (from DataRow dr in dt.Rows
                              select new ContractInvoice()
                              {
                                  ActivityId = Convert.ToInt64(dr["ACTIVITYID"].ToString()),
                                  ProjectName = dr["PROJECTNAME"].ToString(),
                                  ServiceName = dr["SERVICENAME"].ToString(),
                                  RC = dr["RC"].ToString(),
                                  ProjectCode = dr["PROJECTCODE"].ToString(),
                                  Amount = Convert.ToDouble(dr["AMOUNT"].ToString()),
                                  Charges = dr["CHARGES"].ToString(),
                                  FromDate = Convert.ToDateTime(dr["FROMDATE"].ToString()),
                                  EndDate = Convert.ToDateTime(dr["ENDDATE"].ToString())
                              }).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public long InsertCustomerInvoice(string invoicenumber, long customerid, DateTime invoicedate, string prepareby, string preparebyext, string authorizedby, string authorizedbyext, string division, bool isdeffered, string defferedaccount, DateTime fromdate, DateTime todate, string specialinstruction, string supportingdocuments, double totalamount)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "insert into invoice(INVOICENUMBER, CUSTOMERID, INVOICEDATE, PREPAREBY, PREPAREBYEXT, AUTHORIZEDBY, AUTHORIZEDBYEXT, DIVISION, ISDEFFERED, DEFFEREDACCOUNT, FROMDATE, TODATE, SPECIALINSTRUCTION, SUPPORTINGDOCUMENTS,TOTALAMOUNT) VALUES" +
                                  "(@invoicenumber,@customerid,@invoicedate,@prepareby,@preparebyext,@authorizedby,@authorizedbyext,@division,@isdeffered,@defferedaccount,@fromdate,@todate,@specialinstruction,@supportingdocuments,@totalamount)";
                cmd.Parameters.AddWithValue("@invoicenumber", invoicenumber);
                cmd.Parameters.AddWithValue("@customerid", customerid);
                cmd.Parameters.AddWithValue("@invoicedate", invoicedate);
                cmd.Parameters.AddWithValue("@prepareby", prepareby);
                cmd.Parameters.AddWithValue("@preparebyext", preparebyext);
                cmd.Parameters.AddWithValue("@authorizedby", authorizedby);
                cmd.Parameters.AddWithValue("@authorizedbyext", authorizedbyext);
                cmd.Parameters.AddWithValue("@division", division);
                cmd.Parameters.AddWithValue("@isdeffered", isdeffered);
                cmd.Parameters.AddWithValue("@defferedaccount", defferedaccount);
                cmd.Parameters.AddWithValue("@fromdate", fromdate);
                cmd.Parameters.AddWithValue("@todate", todate);
                cmd.Parameters.AddWithValue("@specialinstruction", specialinstruction);
                cmd.Parameters.AddWithValue("@supportingdocuments", supportingdocuments);
                cmd.Parameters.AddWithValue("@totalamount", totalamount);
                cmd.ExecuteNonQuery();
                resultId = cmd.LastInsertedId;
            }
            catch (Exception ex)
            {

            }
            return resultId;
        }
        public long InsertSBFActivity(long sbfid, long activityid)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "insert into sbfactivity(SBFINVOICEID,ACTIVITYID) VALUES" +
                                  "(@sbfid,@activityid)";
                cmd.Parameters.AddWithValue("@sbfid", sbfid);
                cmd.Parameters.AddWithValue("@activityid", activityid);
                cmd.ExecuteNonQuery();
                resultId = cmd.LastInsertedId;
            }
            catch (Exception ex)
            {

            }
            return resultId;
        }
        public bool SetActivityBilled(string activityids)
        {
            int resultId = 0;
            bool result = false;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "Update activity set ISBILLED = TRUE where ID IN(" + activityids + ")";
                resultId = cmd.ExecuteNonQuery();
                if (resultId > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        #endregion

        #endregion

        #region Acknowlegement
        public string GetAcknowledgementList()
        {
            //
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select ca.id as ID,cust.name CUSTOMERNAME,p.name as PROJECTNAME, ca.STATUS from customeraknowledgement ca join aknowledgementservice a  on  ca.id = a.ACKNOWLEDGEMENTID join customer cust on ca.customerid = cust.id join project p on a.projectid = p.id group by ca.id", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<AcknowledgementList> dataResult = new List<AcknowledgementList>();
                dataResult = (from DataRow dr in dt.Rows
                              select new AcknowledgementList()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  CustomerName = dr["CUSTOMERNAME"].ToString(),
                                  ProjectName = dr["PROJECTNAME"].ToString(),
                                  Status = dr["STATUS"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetProjectServiceList(long projectId, string serviceIds)
        {
            //
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("select p.name as PROJECTNAME,st.name as SERVICENAME,p.rc as RC,p.charge_code as PROJECTCODE,IF(c.DIRRECTION = 1, c.amount,-c.amount) as AMOUNT,IF(c.DIRRECTION = 1,'Revenue','Expense') as CHARGES from contract c join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where customerid = '" + id + "'", con);
                MySqlCommand cmd = new MySqlCommand("select p.id as PROJECTID,p.charge_code PROJECTCODE,p.name as PROJECTNAME,st.id as SERVICEID,st.NAME as SERVICENAME,st.budget as BUDGET,st.volume as VOLUME,st.feestype as FEESTYPE from project p , servicetype st where p.id =" + projectId + " and st.id in(" + serviceIds + ")", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<ProjectServiceList> dataResult = new List<ProjectServiceList>();
                dataResult = (from DataRow dr in dt.Rows
                              select new ProjectServiceList()
                              {
                                  ProjectId = Convert.ToInt64(dr["PROJECTID"].ToString()),
                                  ProjectCode = dr["PROJECTCODE"].ToString(),
                                  ProjectName = dr["PROJECTNAME"].ToString(),
                                  ServiceId = Convert.ToInt64(dr["SERVICEID"].ToString()),
                                  ServiceName = dr["SERVICENAME"].ToString(),
                                  FeesType = dr["FEESTYPE"].ToString(),
                                  Budget = Convert.ToDouble((dr["BUDGET"] != DBNull.Value ? dr["BUDGET"] : 0)),
                                  Volume = dr["VOLUME"].ToString(),
                              }).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetAcknowledgementProjectServiceList(long projectId, string serviceIds, string ackid)
        {
            //
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("select p.name as PROJECTNAME,st.name as SERVICENAME,p.rc as RC,p.charge_code as PROJECTCODE,IF(c.DIRRECTION = 1, c.amount,-c.amount) as AMOUNT,IF(c.DIRRECTION = 1,'Revenue','Expense') as CHARGES from contract c join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where customerid = '" + id + "'", con);
                MySqlCommand cmd = new MySqlCommand("select p.id as PROJECTID,p.charge_code PROJECTCODE,p.name as PROJECTNAME,st.id as SERVICEID,st.NAME as SERVICENAME,aks.total as BUDGET,aks.volume as VOLUME,aks.feestype as FEESTYPE from servicetype st join aknowledgementservice aks on aks.serviceId = st.id, project p where p.id =" + projectId + " and st.id in(" + serviceIds + ") and aks.acknowledgementid =" + ackid, con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<ProjectServiceList> dataResult = new List<ProjectServiceList>();
                dataResult = (from DataRow dr in dt.Rows
                              select new ProjectServiceList()
                              {
                                  ProjectId = Convert.ToInt64(dr["PROJECTID"].ToString()),
                                  ProjectCode = dr["PROJECTCODE"].ToString(),
                                  ProjectName = dr["PROJECTNAME"].ToString(),
                                  ServiceId = Convert.ToInt64(dr["SERVICEID"].ToString()),
                                  ServiceName = dr["SERVICENAME"].ToString(),
                                  FeesType = dr["FEESTYPE"].ToString(),
                                  Budget = Convert.ToDouble((dr["BUDGET"] != DBNull.Value ? dr["BUDGET"] : 0)),
                                  Volume = dr["VOLUME"].ToString(),
                              }).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public long InsertCustomerAcknoeledgement(long customerId)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO customeraknowledgement (customerid) VALUES" +
                                  "(@customerid)";
                cmd.Parameters.AddWithValue("@customerid", customerId);
                cmd.ExecuteNonQuery();
                resultId = cmd.LastInsertedId;
            }
            catch (Exception ex)
            {

            }
            return resultId;
        }

        public long InsertAcknowledgementServices(long acknowledgementid, long projectid, long serviceid, double total, string volume, DateTime fromdate, DateTime enddate, string feestype)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO aknowledgementservice (ACKNOWLEDGEMENTID,PROJECTID,SERVICEID,TOTAL,VOLUME,FROMDATE,ENDDATE,FEESTYPE) VALUES" +
                                  "(@acknowledgementid,@projectid,@serviceid,@total,@volume,@fromdate,@enddate,@feestype)";
                cmd.Parameters.AddWithValue("@acknowledgementid", acknowledgementid);
                cmd.Parameters.AddWithValue("@projectid", projectid);
                cmd.Parameters.AddWithValue("@serviceid", serviceid);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@volume", volume);
                cmd.Parameters.AddWithValue("@fromdate", fromdate);
                cmd.Parameters.AddWithValue("@enddate", enddate);
                cmd.Parameters.AddWithValue("@feestype", feestype);
                cmd.ExecuteNonQuery();
                resultId = cmd.LastInsertedId;
            }
            catch (Exception ex)
            {

            }
            return resultId;
        }
        public string GetAcknowledgementServicesbyAcknowledgemetnId(long ackid)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select ca.CUSTOMERID,ca.STATUS,a.* from customeraknowledgement ca join aknowledgementservice a on ca.ID =  a.acknowledgementid where ca.id  ='" + ackid + "'", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<CustomerAcknowledementService> dataResult = new List<CustomerAcknowledementService>();
                dataResult = (from DataRow dr in dt.Rows
                              select new CustomerAcknowledementService()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  CustomerId = Convert.ToInt64(dr["CUSTOMERID"]),
                                  //ProjectId = Convert.ToInt64(dr["PROJECTID"]),
                                  AcknowledementId = Convert.ToInt64(dr["ACKNOWLEDGEMENTID"].ToString()),
                                  ProjectId = Convert.ToInt64(dr["PROJECTID"].ToString()),
                                  ServiceId = Convert.ToInt64(dr["SERVICEID"].ToString()),
                                  Total = Convert.ToDouble(dr["TOTAL"].ToString()),
                                  FromDate = Convert.ToDateTime(dr["FROMDATE"].ToString()),
                                  EndDate = Convert.ToDateTime(dr["ENDDATE"].ToString()),
                                  Volume = dr["VOLUME"].ToString(),
                                  Status = dr["STATUS"].ToString(),
                                  FeesType = dr["FEESTYPE"].ToString(),
                              }).ToList();
                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
                if (result == "null")
                {
                    result = "";
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return result;
        }

        public long AcknowledgementApprove(long ackid)
        {
            //update customeraknowledgement set Status = 'Approved' where ID =11
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "update customeraknowledgement set Status = 'Approved' where ID=" + ackid;
                resultId = cmd.ExecuteNonQuery();
                if (resultId > 0)
                {
                    resultId = ackid;
                }
            }
            catch (Exception ex)
            {

            }
            return resultId;
        }
        #endregion
        #region Contract
        //public string GetcontractList()
        //{
        //    string result = string.Empty;
        //    MySqlConnection con = new MySqlConnection(GetConnectionString());
        //    try
        //    {
        //        if (con.State != ConnectionState.Open)
        //        {
        //            con.Open();
        //        }
        //        MySqlCommand cmd = new MySqlCommand("SELECT * FROM contract", con);
        //        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //        da.SelectCommand = cmd;
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        List<Contract> dataResult = new List<Contract>();
        //        dataResult = (from DataRow dr in dt.Rows
        //                      select new Contract()
        //                      {
        //                          Id = Convert.ToInt64(dr["ID"]),
        //                          CustomerId = Convert.ToInt64(dr["CUSTOMERID"]),
        //                          ServiceId = Convert.ToInt64(dr["SERVICEID"]),
        //                          ProjectId = Convert.ToInt64(dr["PROJECTID"]),
        //                          FromDate = Convert.ToDateTime(dr["FROMDATE"]),
        //                          EndDate = Convert.ToDateTime(dr["ENDDATE"]),
        //                          Dirrection = Convert.ToBoolean(dr["DIRRECTION"]),
        //                          Estimate = Convert.ToBoolean(dr["ESTIMATE"]),
        //                          Status = dr["STATUS"].ToString(),
        //                          Amount = Convert.ToDouble(dr["AMOUNT"]),
        //                          Description = dr["DESCRIPTION"].ToString(),
        //                      }).ToList();
        //        result = JsonConvert.SerializeObject(dataResult);
        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return result;
        //}

        public string GetcontractList()
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM contract", con);
                MySqlCommand cmd = new MySqlCommand("select c.ID,c.CONTRACTCODE,p.Name as PROJECT,cust.name as CUSTOMER,st.name as SERVICE,c.FROMDATE,c.ENDDATE,c.STATUS,c.DIRRECTION,c.ESTIMATE,c.AMOUNT from contract c" +
                " join customer cust on c.customerid = cust.id " +
                " join  project p on c.projectid = p.id" +
                " join servicetype st on c.serviceid = st.id order by c.ID desc", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<ContractList> dataResult = new List<ContractList>();
                dataResult = (from DataRow dr in dt.Rows
                              select new ContractList()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  Customer = dr["CUSTOMER"].ToString(),
                                  Service = dr["SERVICE"].ToString(),
                                  Project = dr["PROJECT"].ToString(),
                                  FromDate = Convert.ToDateTime(dr["FROMDATE"]),
                                  EndDate = Convert.ToDateTime(dr["ENDDATE"]),
                                  Dirrection = Convert.ToBoolean(dr["DIRRECTION"]),
                                  Estimate = Convert.ToBoolean(dr["ESTIMATE"]),
                                  Status = dr["STATUS"].ToString(),
                                  Amount = Convert.ToDouble(dr["AMOUNT"]),
                                  ContractCode = dr["CONTRACTCODE"].ToString()
                                  //Description = dr["DESCRIPTION"].ToString(),
                              }).ToList();
                result = JsonConvert.SerializeObject(dataResult);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public string GetcontractListByProjectId(long projectid)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM contract", con);
                MySqlCommand cmd = new MySqlCommand("select c.ID,c.CONTRACTCODE,p.Name as PROJECT,cust.name as CUSTOMER,st.name as SERVICE,c.FROMDATE,c.ENDDATE,c.STATUS,c.DIRRECTION,c.ESTIMATE,c.AMOUNT from contract c" +
                " join customer cust on c.customerid = cust.id " +
                " join  project p on c.projectid = p.id" +
                " join servicetype st on c.serviceid = st.id where p.id=" + projectid + " order by c.ID desc", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<ContractList> dataResult = new List<ContractList>();
                dataResult = (from DataRow dr in dt.Rows
                              select new ContractList()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  Customer = dr["CUSTOMER"].ToString(),
                                  Service = dr["SERVICE"].ToString(),
                                  Project = dr["PROJECT"].ToString(),
                                  FromDate = Convert.ToDateTime(dr["FROMDATE"]),
                                  EndDate = Convert.ToDateTime(dr["ENDDATE"]),
                                  Dirrection = Convert.ToBoolean(dr["DIRRECTION"]),
                                  Estimate = Convert.ToBoolean(dr["ESTIMATE"]),
                                  Status = dr["STATUS"].ToString(),
                                  Amount = Convert.ToDouble(dr["AMOUNT"]),
                                  ContractCode = dr["CONTRACTCODE"].ToString()
                                  //Description = dr["DESCRIPTION"].ToString(),
                              }).ToList();
                result = JsonConvert.SerializeObject(dataResult);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public long Insertcontract(long customer, long service_type, DateTime fromdate, DateTime enddate, bool dirrection, bool estimate, string status, string volume, double amount, long projectid, string description, string contractcode, string filename, string feestype)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO contract (CUSTOMERID,SERVICEID,FROMDATE,ENDDATE,DIRRECTION,ESTIMATE,STATUS,VOLUME,AMOUNT,PROJECTID,DESCRIPTION,CONTRACTCODE,FILENAME,FEESTYPE) VALUES" +
                                  "(@customer,@service_type,@fromdate,@enddate,@dirrection,@estimate,@status,@volume,@amount,@projectid,@description,@contractcode,@filename,@feestype)";
                cmd.Parameters.AddWithValue("@customer", customer);
                cmd.Parameters.AddWithValue("@service_type", service_type);
                cmd.Parameters.AddWithValue("@fromdate", fromdate);
                cmd.Parameters.AddWithValue("@enddate", enddate);
                cmd.Parameters.AddWithValue("@dirrection", dirrection);
                cmd.Parameters.AddWithValue("@estimate", estimate);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@projectid", projectid);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@volume", volume);
                cmd.Parameters.AddWithValue("@contractcode", contractcode);
                cmd.Parameters.AddWithValue("@filename", filename);
                cmd.Parameters.AddWithValue("@feestype", feestype);
                cmd.ExecuteNonQuery();
                resultId = cmd.LastInsertedId;
            }
            catch (Exception ex)
            {

            }
            return resultId;
        }
        public long UpdatecontractById(long Id, DateTime enddate, string status, string description)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "Update contract set ENDDATE='" + enddate.ToString("yyyy/MM/dd") + "',STATUS='" + status + "',DESCRIPTION='" + description + "' where ID=" + Id;
                resultId = cmd.ExecuteNonQuery();
                if (resultId > 0)
                {
                    resultId = Id;
                }
            }
            catch (Exception ex)
            {

            }
            return resultId;
        }
        public string GetcontractById(long id)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM contract where ID='" + id + "'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                Contract dataResult = new Contract();
                dataResult = (from DataRow dr in dt.Rows
                              select new Contract()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  CustomerId = Convert.ToInt64(dr["CUSTOMERID"]),
                                  ServiceId = Convert.ToInt64(dr["SERVICEID"]),
                                  ProjectId = Convert.ToInt64(dr["PROJECTID"]),
                                  FromDate = Convert.ToDateTime(dr["FROMDATE"]),
                                  EndDate = Convert.ToDateTime(dr["ENDDATE"]),
                                  Dirrection = Convert.ToBoolean(dr["DIRRECTION"]),
                                  Estimate = Convert.ToBoolean(dr["ESTIMATE"]),
                                  Status = dr["STATUS"].ToString(),
                                  Amount = Convert.ToDouble(dr["AMOUNT"]),
                                  Description = dr["DESCRIPTION"].ToString(),
                                  Volume = dr["VOLUME"].ToString(),
                                  ContractCode = dr["CONTRACTCODE"].ToString(),
                                  FileName = dr["FILENAME"].ToString(),
                                  FeesType = dr["FEESTYPE"].ToString(),
                              }).FirstOrDefault();
                result = JsonConvert.SerializeObject(dataResult);
                if (result == "null")
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public bool DeleteContractById(string id)
        {
            bool isDeleted = false;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "DELETE FROM contract where ID in (" + id + ")";
                int isexecute = cmd.ExecuteNonQuery();
                if (isexecute > 0)
                {
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return isDeleted;
        }

        public string GetProjectAvailableBalance(long id)
        {
            //
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select (p.HIGH_LEVEL_BUDGET -sum(c.Amount)) AS EstimateAvailableBalance from contract c join project p on c.PROJECTID = p.ID where c.projectId =" + id, con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                double? balance = (from r in dt.AsEnumerable()
                                   select r.Field<double?>("EstimateAvailableBalance")).First<double?>();

                if (balance == null)
                {
                    dt.Clear();
                    cmd = new MySqlCommand("select HIGH_LEVEL_BUDGET from project where ID=" + id, con);
                    da = new MySqlDataAdapter(cmd);
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    balance = (from r in dt.AsEnumerable()
                               select r.Field<double?>("HIGH_LEVEL_BUDGET")).First<double?>();
                }
                //dataResult = (from DataRow dr in dt.Rows
                //              select new Balance()
                //              {
                //                  Avialablebalance = dr["EstimateAvailableBalance"].ToString()
                //              }).FirstOrDefault();
                result = balance.ToString();//JsonConvert.SerializeObject(name);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public string GetContractAvailableBalance(long contractid)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select  if(sum(a.amount)>0,c.amount -sum(a.amount),c.amount) as BALANCE from activity a join contract c on a.contractid = c.id where c.id = " + contractid + " and a.estimate = false", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                double? balance = (from r in dt.AsEnumerable()
                                   select r.Field<double?>("BALANCE")).First<double?>();
                result = balance.ToString();//JsonConvert.SerializeObject(name);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public bool CheckStausIsActive(long id, string tablename)
        {
            bool result = false;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select id from " + tablename + "  where Lower(status) = Lower('Active') and ID='" + id + "'", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return result;
            //
        }

        public bool CheckIsContractCodeExist(string contractCode)
        {
            bool result = false;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM contract where Lower(CONTRACTCODE)= Lower('" + contractCode + "')", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return result;
        }

        public long InsertContractActivity(long contractid, DateTime fromdate, DateTime enddate, double volume, double amount, bool charges, bool estimate, string description, string status, string filename, string activitycode)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "insert into activity (CONTRACTID, FROMDATE, ENDDATE, VOLUME,AMOUNT, CHARGES, ESTIMATE,DESCRIPTION,STATUS,FILENAME,ACTIVITYCODE) values (@contractid, @fromdate, @enddate, @volume,@amount, @charges, @estimate, @description,@status,@filename,@activitycode)";
                cmd.Parameters.AddWithValue("@contractid", contractid);
                cmd.Parameters.AddWithValue("@fromdate", fromdate);
                cmd.Parameters.AddWithValue("@enddate", enddate);
                cmd.Parameters.AddWithValue("@volume", volume);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@charges", charges);
                cmd.Parameters.AddWithValue("@estimate", estimate);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@filename", filename);
                cmd.Parameters.AddWithValue("@activitycode", activitycode);
                cmd.ExecuteNonQuery();
                resultId = cmd.LastInsertedId;
            }
            catch (Exception ex)
            {

            }
            return resultId;
        }
        public long UpdateContractActivity(long id, long contractid, DateTime fromdate, DateTime enddate, double volume, double amount, bool charges, bool estimate, string description, string status)
        {
            long resultId = 0;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "Update activity set FROMDATE = '" + fromdate.ToString("yyyy/MM/dd") + "' ,ENDDATE='" + enddate.ToString("yyyy/MM/dd") + "',VOLUME='" + volume + "',AMOUNT='" + amount + "',CHARGES=" + charges + ",ESTIMATE=" + estimate + ",DESCRIPTION='" + description + "',STATUS='" + status + "' where ID=" + id;
                resultId = cmd.ExecuteNonQuery();
                if (resultId > 0)
                {
                    resultId = id;
                }
            }
            catch (Exception ex)
            {

            }
            return resultId;
        }
        public string GetLastContractActivity(long contractid)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select * from activity where contractid = " + contractid + " Order by enddate desc limit 1", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                Activity dataResult = new Activity();
                dataResult = (from DataRow dr in dt.Rows
                              select new Activity()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  ContractId = Convert.ToInt64(dr["CONTRACTID"]),
                                  FromDate = Convert.ToDateTime(dr["FROMDATE"]),
                                  EndDate = Convert.ToDateTime(dr["ENDDATE"]),
                                  Charges = Convert.ToBoolean(dr["CHARGES"]),
                                  Estimate = Convert.ToBoolean(dr["ESTIMATE"]),
                                  Amount = Convert.ToDouble(dr["AMOUNT"]),
                                  Description = dr["DESCRIPTION"].ToString(),
                                  Volume = Convert.ToDouble(dr["VOLUME"].ToString()),
                                  Status = dr["STATUS"].ToString(),
                              }).FirstOrDefault();
                result = JsonConvert.SerializeObject(dataResult);
                if (result == "null")
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetActivityById(long activityid)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select * from activity where id = " + activityid, con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                Activity dataResult = new Activity();
                dataResult = (from DataRow dr in dt.Rows
                              select new Activity()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  ContractId = Convert.ToInt64(dr["CONTRACTID"]),
                                  FromDate = Convert.ToDateTime(dr["FROMDATE"]),
                                  EndDate = Convert.ToDateTime(dr["ENDDATE"]),
                                  Charges = Convert.ToBoolean(dr["CHARGES"]),
                                  Estimate = Convert.ToBoolean(dr["ESTIMATE"]),
                                  Amount = Convert.ToDouble(dr["AMOUNT"]),
                                  Description = dr["DESCRIPTION"].ToString(),
                                  Volume = Convert.ToDouble(dr["VOLUME"].ToString()),
                                  Status = dr["STATUS"].ToString(),
                                  IsBilled = Convert.ToBoolean(dr["ISBILLED"]),
                                  FileName = dr["FILENAME"].ToString(),
                                  ActivityCode = dr["ACTIVITYCODE"].ToString()
                              }).FirstOrDefault();
                result = JsonConvert.SerializeObject(dataResult);
                if (result == "null")
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public string GetActivitiesByContractIds(string contractid)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("select p.name as PROJECT,st.name as SERVICE,a.*,c.CONTRACTCODE from activity  a join contract c on a.contractid = c.id join project p  on c.projectid = p.id join servicetype st on c.serviceid = st.id WHERE a.Contractid in(" + contractid + ") Order by enddate asc", con);
                MySqlCommand cmd = new MySqlCommand("select p.name as PROJECT,p.charge_code as PROJECTCODE,p.GLACCOUNT,st.name as SERVICE,a.id as ID,a.contractid as CONTRACTID,a.fromdate as FROMDATE, a.ENDDATE as ENDDATE,a.charges as CHARGES,a.estimate as ESTIMATE,a.description as DESCRIPTION,a.volume as VOLUME,st.budget as RATE,st.Feestype as FEESTYPE,p.RC as RC,if(a.charges = 0,a.amount,-a.amount) as AMOUNT,a.status as STATUS,a.isbilled as ISBILLED,a.ACTIVITYCODE,a.filename as FILENAME,c.CONTRACTCODE from activity  a join contract c on a.contractid = c.id join project p  on c.projectid = p.id join servicetype st on c.serviceid = st.id WHERE a.Contractid in(" + contractid + ") Order by enddate asc", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<ActivityList> dataResult = new List<ActivityList>();
                dataResult = (from DataRow dr in dt.Rows
                              select new ActivityList()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  ContractId = Convert.ToInt64(dr["CONTRACTID"]),
                                  FromDate = Convert.ToDateTime(dr["FROMDATE"]),
                                  EndDate = Convert.ToDateTime(dr["ENDDATE"]),
                                  Charges = Convert.ToBoolean(dr["CHARGES"]),
                                  Estimate = Convert.ToBoolean(dr["ESTIMATE"]),
                                  Amount = Convert.ToDouble(dr["AMOUNT"]),
                                  Description = dr["DESCRIPTION"].ToString(),
                                  Volume = Convert.ToDouble(dr["VOLUME"].ToString()),
                                  ContractCode = dr["CONTRACTCODE"].ToString(),
                                  Status = dr["STATUS"].ToString(),
                                  IsBilled = Convert.ToBoolean(dr["ISBILLED"]),
                                  Service = dr["SERVICE"].ToString(),
                                  ProjectName = dr["PROJECT"].ToString(),
                                  GLAccount = dr["GLACCOUNT"].ToString(),
                                  ActivityCode = dr["ACTIVITYCODE"].ToString(),
                                  Rate = Convert.ToDouble((dr["RATE"] != DBNull.Value ? dr["RATE"] : 0)),
                                  FeesType = dr["FEESTYPE"].ToString(),
                                  RC = dr["RC"].ToString(),
                                  ProjectCode = dr["PROJECTCODE"].ToString(),
                              }).ToList();
                result = JsonConvert.SerializeObject(dataResult);
                if (result == "null")
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetActivitiesByActivityIds(string activityids)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select p.name as PROJECT,p.charge_code as PROJECTCODE,st.name as SERVICE,a.*,c.volume as RATEVOLUME,c.amount as RATE,c.Feestype as FEESTYPE,p.RC as RC,c.CONTRACTCODE from activity  a join contract c on a.contractid = c.id join project p  on c.projectid = p.id join servicetype st on c.serviceid = st.id WHERE a.id in(" + activityids + ") Order by enddate asc", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<ActivityList> dataResult = new List<ActivityList>();
                dataResult = (from DataRow dr in dt.Rows
                              select new ActivityList()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  ContractId = Convert.ToInt64(dr["CONTRACTID"]),
                                  FromDate = Convert.ToDateTime(dr["FROMDATE"]),
                                  EndDate = Convert.ToDateTime(dr["ENDDATE"]),
                                  Charges = Convert.ToBoolean(dr["CHARGES"]),
                                  Estimate = Convert.ToBoolean(dr["ESTIMATE"]),
                                  Amount = Convert.ToDouble((dr["AMOUNT"] != DBNull.Value ? dr["AMOUNT"] : 0)),
                                  Description = dr["DESCRIPTION"].ToString(),
                                  RateVolume = Convert.ToDouble((dr["RATEVOLUME"] != DBNull.Value ? dr["RATEVOLUME"] : 0)),
                                  Volume = Convert.ToDouble((dr["VOLUME"] != DBNull.Value ? dr["VOLUME"] : 0)),
                                  ContractCode = dr["CONTRACTCODE"].ToString(),
                                  Status = dr["STATUS"].ToString(),
                                  IsBilled = Convert.ToBoolean(dr["ISBILLED"]),
                                  Service = dr["SERVICE"].ToString(),
                                  ProjectName = dr["PROJECT"].ToString(),
                                  FeesType = dr["FEESTYPE"].ToString(),
                                  Rate = Convert.ToDouble((dr["RATE"] != DBNull.Value ? dr["RATE"] : 0)),
                                  RC = dr["RC"].ToString(),
                                  ProjectCode = dr["PROJECTCODE"].ToString(),
                              }).ToList();
                result = JsonConvert.SerializeObject(dataResult);
                if (result == "null")
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public string GetAllActivities()
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select a.ID,a.CONTRACTID,a.FROMDATE,a.ENDDATE,a.VOLUME,a.CHARGES,a.ESTIMATE,a.DESCRIPTION,if(a.charges = 1,- a.amount,a.amount) as AMOUNT,a.STATUS,a.ISBILLED,a.ACTIVITYCODE,c.CONTRACTCODE from activity  a join contract c on a.contractid = c.id  Order by a.ID desc", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<ActivityList> dataResult = new List<ActivityList>();
                dataResult = (from DataRow dr in dt.Rows
                              select new ActivityList()
                              {
                                  Id = Convert.ToInt64(dr["ID"]),
                                  ContractId = Convert.ToInt64(dr["CONTRACTID"]),
                                  FromDate = Convert.ToDateTime(dr["FROMDATE"]),
                                  EndDate = Convert.ToDateTime(dr["ENDDATE"]),
                                  Charges = Convert.ToBoolean(dr["CHARGES"]),
                                  Estimate = Convert.ToBoolean(dr["ESTIMATE"]),
                                  Amount = Convert.ToDouble(dr["AMOUNT"]),
                                  Description = dr["DESCRIPTION"].ToString(),
                                  Volume = Convert.ToDouble(dr["VOLUME"].ToString()),
                                  ContractCode = dr["CONTRACTCODE"].ToString(),
                                  ActivityCode = dr["ACTIVITYCODE"].ToString(),
                                  Status = dr["STATUS"].ToString(),
                                  IsBilled = Convert.ToBoolean(dr["ISBILLED"])
                              }).ToList();
                result = JsonConvert.SerializeObject(dataResult);
                if (result == "null")
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public bool CheckIsActivityCodeExist(string activityCode)
        {
            bool result = false;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM activity where Lower(activitycode)=Lower('" + activityCode + "')", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return result;
        }

        public bool DeleteAcknowledgementById(string ids)
        {
            bool isDeleted = false;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "DELETE FROM customeraknowledgement where ID in (" + ids + ")";
                int isexecute = cmd.ExecuteNonQuery();
                if (isexecute > 0)
                {
                    cmd = con.CreateCommand();
                    cmd.CommandText = "DELETE FROM aknowledgementservice where acknowledgementid in (" + ids + ")";
                    isexecute = cmd.ExecuteNonQuery();
                    if (isexecute > 0)
                    {
                        isDeleted = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return isDeleted;
        }

        public string GetContractFeesTypeByServiceId(long id)
        {
            //
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select FEESTYPE from contract where serviceid =" + id, con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<string> dataResult = dt.AsEnumerable()
                           .Select(r => r.Field<string>("FEESTYPE"))
                           .ToList();

                //Dictionary<string, string> dataResult = Dictionary<string, string>();

                //string dataResult = (from DataRow dr in dt.Rows select
                //                         new Dictionary<string,string>{ Keys = dr["FEESTYPE"].ToString(),Values=dr["FEESTYPE"].ToString()}
                //                         ).ToDictionary();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        #endregion

        public bool CheckIsChargeCodeExist(string chargecode, string tablename)
        {
            bool result = false;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + tablename + " where  Lower(CHARGE_CODE) = Lower('" + chargecode + "')", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
                con.Close();
            }
            catch (Exception ex)
            {

            }
            //finally
            //{

            //}
            return result;
        }
        public string GetAccuralReportMonthYear()
        {
            //SELECT DISTINCT YEAR(FROMDATE), MONTH(FROMDATE) FROM contract
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT YEAR(FROMDATE) as YEAR, MONTH(FROMDATE) as MONTH  FROM contract", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<MonthYear> dataResult = new List<MonthYear>();
                dataResult = (from DataRow dr in dt.Rows
                              select new MonthYear()
                              {
                                  Month = dr["MONTH"].ToString(),
                                  Year = dr["YEAR"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetAccuralReportByMonthYear(string month, string year)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("select p.CHARGE_CODE,cust.CUSTOMER_TYPE,st.NAME,IF(a.charges = 1, a.amount,0)as REVENUE,IF(a.charges = 0, a.amount,0)as EXPENSE ,IF(c.Estimate= 0, 'Real','Estimate')as ESTIMATE,c.FROMDATE,cust.NAME as CUSTOMER_NAME from activity a join contract c on a.contractid = c.id join customer cust on c.CUSTOMERID = cust.ID join servicetype st on c.SERVICEID = st.ID join project p  on c.PROJECTID=p.ID  where Year(c.FROMDATE) = " + Convert.ToUInt64(year) + " AND MONTH(c.FROMDATE) = " + Convert.ToUInt64(month)+"", con);
                MySqlCommand cmd = new MySqlCommand("select p.CHARGE_CODE,cust.CUSTOMER_TYPE,st.NAME,sum(IF(a.charges = 1, a.amount,0))as REVENUE,sum(IF(a.charges = 0, a.amount,0))as EXPENSE ,IF(c.Estimate= 0, 'Real','Estimate')as ESTIMATE,c.FROMDATE,cust.NAME as CUSTOMER_NAME from activity a join contract c on a.contractid = c.id join customer cust on c.CUSTOMERID = cust.ID join servicetype st on c.SERVICEID = st.ID join project p  on c.PROJECTID=p.ID  where Year(c.FROMDATE) = " + Convert.ToUInt64(year) + " AND MONTH(c.FROMDATE) = " + Convert.ToUInt64(month) + " group by p.charge_code", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<AccuralReport> dataResult = new List<AccuralReport>();
                dataResult = (from DataRow dr in dt.Rows
                              select new AccuralReport()
                              {
                                  ProjectCode = dr["CHARGE_CODE"].ToString(),
                                  ServiceName = dr["NAME"].ToString(),
                                  CustomerType = dr["CUSTOMER_TYPE"].ToString(),
                                  Revenue = dr["REVENUE"].ToString(),
                                  Expense = dr["EXPENSE"].ToString(),
                                  Estimate = dr["ESTIMATE"].ToString(),
                                  FromDate = dr["FROMDATE"].ToString(),
                                  CustomerName = dr["CUSTOMER_NAME"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
            //            
        }
        public string GetAccuralReportByDate(string fromdate, string todate)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("select p.CHARGE_CODE,cust.CUSTOMER_TYPE,st.NAME,IF(c.DIRRECTION = 1, c.amount,0)as REVENUE,IF(c.DIRRECTION = 0, c.amount,0)as EXPENSE ,IF(c.Estimate= 0, 'Real','Estimate')as ESTIMATE,c.FROMDATE,cust.NAME as CUSTOMER_NAME from contract c join customer cust on c.CUSTOMERID = cust.ID join servicetype st on c.SERVICEID = st.ID join project p on c.PROJECTID=p.ID where (c.FROMDATE BETWEEN '" + fromdate + "' AND '" + todate + "')", con);
                MySqlCommand cmd = new MySqlCommand("select p.CHARGE_CODE,cust.CUSTOMER_TYPE,st.NAME,sum(IF(a.charges = 1, a.amount,0))as REVENUE,sum(IF(a.charges = 0, a.amount,0))as EXPENSE ,IF(c.Estimate= 0, 'Real','Estimate')as ESTIMATE,c.FROMDATE,cust.NAME as CUSTOMER_NAME from activity a join contract c on a.contractid = c.id join customer cust on c.CUSTOMERID = cust.ID join servicetype st on c.SERVICEID = st.ID join project p  on c.PROJECTID=p.ID  where (c.FROMDATE BETWEEN '" + fromdate + "' AND '" + todate + "') group by p.charge_code", con);

                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<AccuralReport> dataResult = new List<AccuralReport>();
                dataResult = (from DataRow dr in dt.Rows
                              select new AccuralReport()
                              {
                                  ProjectCode = dr["CHARGE_CODE"].ToString(),
                                  ServiceName = dr["NAME"].ToString(),
                                  CustomerType = dr["CUSTOMER_TYPE"].ToString(),
                                  Revenue = dr["REVENUE"].ToString(),
                                  Expense = dr["EXPENSE"].ToString(),
                                  Estimate = dr["ESTIMATE"].ToString(),
                                  FromDate = dr["FROMDATE"].ToString(),
                                  CustomerName = dr["CUSTOMER_NAME"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
            //            
        }

        public bool DeleteActivityById(string id)
        {
            bool isDeleted = false;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd = con.CreateCommand();
                cmd.CommandText = "DELETE FROM activity where ID in (" + id + ")";
                int isexecute = cmd.ExecuteNonQuery();
                if (isexecute > 0)
                {
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return isDeleted;
        }

        public string GetConnectionString()
        {
            string cs = ConfigurationManager.ConnectionStrings["LocalMySqlServer"].ConnectionString;
            return cs;
        }

        //Reports
        public string GetProjectRevenueExpenseMonthYear(string month, string year)
        {
            //
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select p.CHARGE_CODE,sum(IF(a.charges = 1, a.amount,0))as REVENUE,sum(IF(a.charges = 0, a.amount,0))as EXPENSE from activity a join contract c on a.contractid = c.id  join project p on c.PROJECTID=p.ID where  Year(a.FROMDATE) = " + Convert.ToUInt64(year) + " AND MONTH(a.FROMDATE) = " + Convert.ToUInt64(month) + " group by p.charge_code ", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<PlanRevenueExpense> dataResult = new List<PlanRevenueExpense>();
                dataResult = (from DataRow dr in dt.Rows
                              select new PlanRevenueExpense()
                              {
                                  ProjectCode = dr["CHARGE_CODE"].ToString(),
                                  Revenue = dr["REVENUE"].ToString(),
                                  Expense = dr["EXPENSE"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetProjectRevenueExpenseByDate(string fromdate, string todate)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("select p.CHARGE_CODE,cust.CUSTOMER_TYPE,st.NAME,IF(c.DIRRECTION = 1, c.amount,0)as REVENUE,IF(c.DIRRECTION = 0, c.amount,0)as EXPENSE ,IF(c.Estimate= 0, 'Real','Estimate')as ESTIMATE,c.FROMDATE,cust.NAME as CUSTOMER_NAME from contract c join customer cust on c.CUSTOMERID = cust.ID join servicetype st on c.SERVICEID = st.ID join project p on c.PROJECTID=p.ID where (c.FROMDATE BETWEEN '" + fromdate + "' AND '" + todate + "')", con);
                MySqlCommand cmd = new MySqlCommand("select p.CHARGE_CODE,sum(IF(a.charges = 1, a.amount,0))as REVENUE,sum(IF(a.charges = 0, a.amount,0))as EXPENSE from activity a join contract c on a.contractid = c.id   join project p on c.PROJECTID=p.ID where  (a.FROMDATE BETWEEN '" + fromdate + "' AND '" + todate + "') group by p.charge_code ", con);

                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<PlanRevenueExpense> dataResult = new List<PlanRevenueExpense>();
                dataResult = (from DataRow dr in dt.Rows
                              select new PlanRevenueExpense()
                              {
                                  ProjectCode = dr["CHARGE_CODE"].ToString(),
                                  Revenue = dr["REVENUE"].ToString(),
                                  Expense = dr["EXPENSE"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
            //            
        }

        public string GetServiceRevenueExpenseMonthYear(string month, string year)
        {
            //
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select st.name as NAME,sum(IF(a.charges = 1, a.amount,0))as REVENUE,sum(IF(a.charges = 0, a.amount,0))as EXPENSE from activity a join contract c on a.contractid = c.id   join customer cust on c.customerid = cust.id  join  project p on c.projectid = p.id  join servicetype st on c.serviceid = st.id where  Year(a.FROMDATE) = '" + year + "' AND MONTH(a.FROMDATE) = '" + month + "' group by st.name", con);
                //MySqlCommand cmd = new MySqlCommand("select p.CHARGE_CODE,sum(IF(c.DIRRECTION = 1, c.amount,0))as REVENUE,sum(IF(c.DIRRECTION = 0, c.amount,0))as EXPENSE from contract c  join project p on c.PROJECTID=p.ID where  Year(c.FROMDATE) = " + Convert.ToUInt64(year) + " AND MONTH(c.FROMDATE) = " + Convert.ToUInt64(month) + " group by p.charge_code ", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<PlanRevenueExpense> dataResult = new List<PlanRevenueExpense>();
                dataResult = (from DataRow dr in dt.Rows
                              select new PlanRevenueExpense()
                              {
                                  ProjectCode = dr["NAME"].ToString(),
                                  Revenue = dr["REVENUE"].ToString(),
                                  Expense = dr["EXPENSE"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetServiceRevenueExpenseByDate(string fromdate, string todate)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select st.name as NAME,sum(IF(a.charges = 1, a.amount,0))as REVENUE,sum(IF(a.charges = 0, a.amount,0))as EXPENSE from activity a join contract c on a.contractid = c.id join customer cust on c.customerid = cust.id  join  project p on c.projectid = p.id  join servicetype st on c.serviceid = st.id where  (a.FROMDATE BETWEEN '" + fromdate + "' AND '" + todate + "') group by st.name", con);
                //MySqlCommand cmd = new MySqlCommand("select p.CHARGE_CODE,sum(IF(c.DIRRECTION = 1, c.amount,0))as REVENUE,sum(IF(c.DIRRECTION = 0, c.amount,0))as EXPENSE from contract c  join project p on c.PROJECTID=p.ID where  (c.FROMDATE BETWEEN '" + fromdate + "' AND '" + todate + "') group by p.charge_code ", con);

                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<PlanRevenueExpense> dataResult = new List<PlanRevenueExpense>();
                dataResult = (from DataRow dr in dt.Rows
                              select new PlanRevenueExpense()
                              {
                                  ProjectCode = dr["NAME"].ToString(),
                                  Revenue = dr["REVENUE"].ToString(),
                                  Expense = dr["EXPENSE"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
            //            
        }

        public string GetPlanCustomerRevenueExpenseMonthYear(string month, string year)
        {
            //
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select cust.name as NAME,sum(IF(a.charges = 1, a.amount,0))as REVENUE,sum(IF(a.charges = 0, a.amount,0))as EXPENSE from activity a join contract c on a.contractid = c.id join customer cust on c.customerid = cust.id  join  project p on c.projectid = p.id  join servicetype st on c.serviceid = st.id where  Year(a.FROMDATE) = " + Convert.ToUInt64(year) + " AND MONTH(a.FROMDATE) = " + Convert.ToUInt64(month) + " group by cust.name", con);
                //MySqlCommand cmd = new MySqlCommand("select p.CHARGE_CODE,sum(IF(c.DIRRECTION = 1, c.amount,0))as REVENUE,sum(IF(c.DIRRECTION = 0, c.amount,0))as EXPENSE from contract c  join project p on c.PROJECTID=p.ID where  Year(c.FROMDATE) = " + Convert.ToUInt64(year) + " AND MONTH(c.FROMDATE) = " + Convert.ToUInt64(month) + " group by p.charge_code ", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<PlanRevenueExpense> dataResult = new List<PlanRevenueExpense>();
                dataResult = (from DataRow dr in dt.Rows
                              select new PlanRevenueExpense()
                              {
                                  ProjectCode = dr["NAME"].ToString(),
                                  Revenue = dr["REVENUE"].ToString(),
                                  Expense = dr["EXPENSE"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetPlanCustomerRevenueExpenseByDate(string fromdate, string todate)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select cust.name as NAME,sum(IF(a.charges = 1, a.amount,0))as REVENUE,sum(IF(a.charges = 0, a.amount,0))as EXPENSE from activity a join contract c on a.contractid = c.id  join customer cust on c.customerid = cust.id  join  project p on c.projectid = p.id  join servicetype st on c.serviceid = st.id where  (a.FROMDATE BETWEEN '" + fromdate + "' AND '" + todate + "') group by cust.name", con);
                //MySqlCommand cmd = new MySqlCommand("select p.CHARGE_CODE,sum(IF(c.DIRRECTION = 1, c.amount,0))as REVENUE,sum(IF(c.DIRRECTION = 0, c.amount,0))as EXPENSE from contract c  join project p on c.PROJECTID=p.ID where  (c.FROMDATE BETWEEN '" + fromdate + "' AND '" + todate + "') group by p.charge_code ", con);

                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<PlanRevenueExpense> dataResult = new List<PlanRevenueExpense>();
                dataResult = (from DataRow dr in dt.Rows
                              select new PlanRevenueExpense()
                              {
                                  ProjectCode = dr["NAME"].ToString(),
                                  Revenue = dr["REVENUE"].ToString(),
                                  Expense = dr["EXPENSE"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
            //            
        }

        public string GetServiceByProjectId(long projectId)
        {
            //select st.id as ID,st.name As NAME from servicetype st join contract c on c.serviceid = st.id where c.projectid = 20 group by st.name
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select st.id as ID,st.name As NAME from servicetype st join contract c on c.serviceid = st.id where c.projectid = " + projectId + " group by st.name", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<ServiceType> dataResult = new List<ServiceType>();
                dataResult = (from DataRow dr in dt.Rows
                              select new ServiceType()
                              {
                                  Id = Convert.ToInt64(dr["ID"].ToString()),
                                  Name = dr["NAME"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string GetCustomerByServiceAndProjectId(long serviceId, long projectId)
        {
            //select c.CustomerID,cust.Name from contract c join customer cust on c.customerId = cust.id  join servicetype st on c.serviceid = st.id where c.serviceid=15 and c.projectid=32 group by c.customerid
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmd = new MySqlCommand("select c.CustomerID as ID,cust.Name as NAME from contract c join customer cust on c.customerId = cust.id  join servicetype st on c.serviceid = st.id where c.serviceid=" + serviceId + " and c.projectid=" + projectId + " group by c.customerid", con);
                //cmd.CommandText = "SELECT * FROM project";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Customer> dataResult = new List<Customer>();
                dataResult = (from DataRow dr in dt.Rows
                              select new Customer()
                              {
                                  Id = Convert.ToInt64(dr["ID"].ToString()),
                                  Name = dr["NAME"].ToString(),
                              }).ToList();

                //List<Project> projectList = dt.AsEnumerable().ToList().Select(x=>new Project(){Id=Convert.ToInt64(x.ItemArray[0])}).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public string GetContractDetailForActivity(long projectId, long serviceId, long customerId, string fromMonth, string toMonth, string year)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                //MySqlCommand cmd = new MySqlCommand("select p.name as PROJECTNAME,st.name as SERVICENAME,p.rc as RC,p.charge_code as PROJECTCODE,c.amount as AMOUNT from contract c join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where customerid = '" + id + "'", con);
                MySqlCommand cmd = new MySqlCommand("select p.name as PROJECTNAME,st.name as SERVICENAME,p.rc as RC,p.charge_code as PROJECTCODE,IF(c.DIRRECTION = 1, c.amount,-c.amount) as AMOUNT,IF(c.DIRRECTION = 1,'Revenue','Expense') as CHARGES, c.FromDate as FROMDATE,c.EndDate as ENDDATE  from contract c join project p on c.projectid = p.id join servicetype st on c.serviceid = st.id where customerid = " + customerId + " and c.projectid=" + projectId + " and c.serviceid=" + serviceId + " and MONTH(c.FROMDATE) >= " + fromMonth + " and MONTH(c.enddate) <= " + toMonth + " and Year(c.enddate) = " + year + "", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<ContractInvoice> dataResult = new List<ContractInvoice>();
                dataResult = (from DataRow dr in dt.Rows
                              select new ContractInvoice()
                              {
                                  ProjectName = dr["PROJECTNAME"].ToString(),
                                  ServiceName = dr["SERVICENAME"].ToString(),
                                  RC = dr["RC"].ToString(),
                                  ProjectCode = dr["PROJECTCODE"].ToString(),
                                  Amount = Convert.ToDouble(dr["AMOUNT"].ToString()),
                                  Charges = dr["CHARGES"].ToString(),
                              }).ToList();
                result = JsonConvert.SerializeObject(dataResult);
            }
            catch (Exception ex)
            {

            }
            return result;

        }
    }
}
