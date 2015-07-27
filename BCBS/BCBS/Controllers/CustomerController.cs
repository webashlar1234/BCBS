using BCBS.bsbcserviceref;
using BCBS.Mailers;
using BCBS.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCBS.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string id,string password)
        {
            return RedirectToAction("Index", "Project");
        }



        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult New()
        {
            CustomerModel customer = new CustomerModel();
            return View(customer);
        }

        [HttpPost]
        public ActionResult New(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                BCBSClient client = new BCBSClient();
                long Id = 0;
                Id = client.Insertcustomer(customer.Name, customer.ChargeCode, customer.CustomerType, customer.CustomerAddress, customer.City,
                    customer.PostalCode, customer.State, customer.Country, customer.FirstName, customer.LastName,
                    customer.Phone, customer.Fax, customer.Occupation, customer.Email, customer.Status);
                if (Id > 0)
                {
                    TempData["Message"] = "Customer Added successfully..!";
                }
                else
                {
                    TempData["Error"] = "Customer Adding failed..!";
                }
                ModelState.Clear();
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                return View(customer);
            }
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            CustomerModel Customer = new CustomerModel();
            if (id > 0)
            {
                BCBSClient client = new BCBSClient();
                string CustomerData = client.GetcustomerById(id);
                if (!string.IsNullOrEmpty(CustomerData))
                {
                    Customer = JsonConvert.DeserializeObject<CustomerModel>(CustomerData);
                }
                else
                {
                    TempData["Error"] = "Requested customer not available!!";
                    return RedirectToAction("Index", "Customer");
                }
            }
            return View(Customer);
        }

        [HttpPost]
        public ActionResult Edit(CustomerModel Customer)
        {
            if (ModelState.IsValid)
            {
                BCBSClient client = new BCBSClient();
                long Id = 0;
                Id = client.UpdatecustomerById(Customer.Id, Customer.Name, Customer.ChargeCode, Customer.CustomerType, Customer.CustomerAddress, Customer.City,
                    Customer.PostalCode, Customer.State, Customer.Country, Customer.FirstName, Customer.LastName, Customer.Phone, Customer.Fax,
                    Customer.Occupation, Customer.Email, Customer.Status);
                if (Id > 0)
                {
                    TempData["Message"] = "Customer Updated successfully..!";
                }
                else
                {
                    TempData["Error"] = "Customer Update failed..!";
                }
                ModelState.Clear();
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                return View(Customer);
            }
        }

        public JsonResult GetCustomerList()
        {
            try
            {
                BCBSClient client = new BCBSClient();
                string result = client.GetcustomerList();
                List<CustomerModel> CustomerList = JsonConvert.DeserializeObject<List<CustomerModel>>(result);
                return Json(CustomerList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteCustomerById(string id)
        {
            bool isdeleted = false;
            if (!string.IsNullOrEmpty(id))
            {
                BCBSClient client = new BCBSClient();
                isdeleted = client.DeletecustomerById(id);
            }
            return Json(isdeleted, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsChargeCodeExist(string chargeCode)
        {
            bool isexist = false;
            if (!string.IsNullOrEmpty(chargeCode))
            {
                BCBSClient client = new BCBSClient();
                isexist = client.CheckIsChargeCodeExist(chargeCode, "customer");
            }
            return Json(!isexist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Invoice()
        {
            BCBSClient client = new BCBSClient();
            string customerlist = client.GetCustomerListForSBF();
            if (!string.IsNullOrEmpty(customerlist))
            {
                ViewBag.Customers = JsonConvert.DeserializeObject<List<CustomerModel>>(customerlist).Select(x => new { x.Id, x.Name });
            }
            else
            {
                ViewBag.Customers = "";
            }
            return View();
        }
        [HttpPost]
        public ActionResult Invoice(InvoiceViewModel invoiceModel, HttpPostedFileBase file, FormCollection fc)
        {
            BCBSClient client = new BCBSClient();
            //ModelState.Where(m => m.Key == "CustomerCode").FirstOrDefault().Value.Errors.Clear();
            //if (ModelState.IsValid)
            //{
            long Id = 0;
            string fname = string.Empty;
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(file.FileName);
                Guid g = Guid.NewGuid();

                fname = g.ToString() + ext;
                // store the file inside ~/UploadDocuments/uploads folder
                var path = Path.Combine(Server.MapPath("~/UploadDocuments/uploads"), fname);
                file.SaveAs(path);
                invoiceModel.SupportingDocuments = fname;
            }

            //string csv = string.Empty;
            Id = client.InsertCustomerInvoice(invoiceModel.InvoiceNumber, invoiceModel.CustomerId, invoiceModel.InvoiceDate, invoiceModel.PrepareBy, invoiceModel.PrepareByExt, invoiceModel.AuthorizedBy, invoiceModel.AuthorizedByExt, invoiceModel.Division, invoiceModel.IsDeffered, invoiceModel.DefferedAccount, invoiceModel.FromDate, invoiceModel.ToDate, invoiceModel.SpecialInstuction, invoiceModel.SupportingDocuments, invoiceModel.TotalAmount);
            if (Id > 0)
            {
                //csv += "Invoice Number," + invoiceModel.InvoiceNumber + "\n";
                //csv += "Invoice Date," + invoiceModel.InvoiceDate.ToString("MM/dd/yyyy") + "\n";
                //csv += "Prepared by," + invoiceModel.PrepareBy + ", Authorized By," + invoiceModel.AuthorizedBy + "\n";
                //csv += "Devision," + invoiceModel.Division + "\n\n";

                //csv += "Customer Name," + invoiceModel.Customer.Name + "\n";
                //csv += "Address\n";
                //csv += invoiceModel.Customer.CustomerAddress + " " + invoiceModel.Customer.City + " " + invoiceModel.Customer.State + " " + invoiceModel.Customer.PostalCode + "\n";
                //csv += "Contact Name," + invoiceModel.Customer.FirstName + " " + invoiceModel.Customer.LastName + "\n";
                //csv += "Phone," + invoiceModel.Customer.Phone + "\n\n";

                SBFEmailViewModel sbfemail = new SBFEmailViewModel();
                //mailer.SBF();
                string fcActivities = fc["Activities"].ToString();
                List<string> activities = new List<string>();
                if (!string.IsNullOrEmpty(fcActivities))
                {


                    bool isbilled = client.SetActivityBilled(fcActivities);
                    if (isbilled)
                    {
                        var activityIds = fcActivities.Split(',');
                        foreach (var x in activityIds)
                        {
                            if (!string.IsNullOrEmpty(x))
                            {
                                long activityId = Convert.ToInt64(x);
                                long sbfActivityId = client.InsertSBFActivity(Id, activityId);
                                if (sbfActivityId > 0)
                                {
                                    activities.Add(activityId.ToString());

                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                if (activities != null && activities.Count > 0)
                {
                    string ids = string.Join(",", activities.ToArray());
                    string result = client.GetActivitiesByActivityIds(ids);
                    List<ActivityListModel> activity = JsonConvert.DeserializeObject<List<ActivityListModel>>(result);
                    if (sbfemail.ActivityList == null)
                    {
                        //csv += "Contract -Activity\n\n";
                        //csv += "Project Name,Service Type,From Date,End Date,Charges,Value,Amount\n";
                        sbfemail.ActivityList = new List<ActivityListModel>();
                    }
                    if (activity != null)
                    {
                        foreach (ActivityListModel x in activity)
                        {
                            string charge = string.Empty;
                            if (x.Charges == true) { charge = "Expense"; } else { charge = "Revenue"; }
                            string estimate = string.Empty;

                            if (x.Estimate == true) { estimate = "Real"; } else { estimate = "Estimate"; }

                            //csv += x.ProjectName + "," + x.Service + "," + x.FromDate.ToString("MM/dd/yyyy") + "," + x.EndDate.ToString("MM/dd/yyyy") + "," + charge + "," + estimate + "," + x.Amount + "\n";
                        }
                        //csv += ",,,,,Total," + invoiceModel.TotalAmount + "\n\n";
                        //if (invoiceModel.IsDeffered == true)
                        //{
                        //csv += "Deffered Account," + invoiceModel.DefferedAccount + "\n";
                        //}
                        //csv += "From Date," + invoiceModel.FromDate.ToString("MM/dd/yyyy") + ",,End Date," + invoiceModel.ToDate.ToString("MM/dd/yyyy") + "\n";

                        sbfemail.ActivityList = activity;
                    }
                }

                IUserMailer mailer = new UserMailer();

                sbfemail.Invoice = invoiceModel;
                string CustomerData = client.GetcustomerById(invoiceModel.CustomerId);
                if (!string.IsNullOrEmpty(CustomerData))
                {
                    sbfemail.Customer = JsonConvert.DeserializeObject<CustomerModel>(CustomerData);
                }
                string ContractData = client.GetcontractById(invoiceModel.ContractId);
                ContractModel contract = new ContractModel();
                if (!string.IsNullOrEmpty(ContractData))
                {
                    contract = JsonConvert.DeserializeObject<ContractModel>(ContractData);
                    sbfemail.Contract = contract;
                }
                string fileName = "SBF_" + sbfemail.Invoice.InvoiceNumber + "_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss");
                string excelFileName = CreatExcel(sbfemail, fileName);
                mailer.SBF(sbfemail, file, "SBF Invoice - BCBS Acc Sys").Send();
                if (!string.IsNullOrEmpty(excelFileName))
                {
                    string path = Path.Combine(Server.MapPath("~/UploadDocuments/uploads/xlsx"), excelFileName);
                    return File(path, "text/csv", excelFileName);
                }
                else
                {

                    TempData["Message"] = "Customer Invoice generated successfully..!";

                    //string fileName = "SBF_" + invoiceModel.InvoiceNumber + "_" + DateTime.Now.ToString("MM-dd-yyyy");
                    //
                    return RedirectToAction("Index", "Customer");
                }
            }
            else
            {
                string fullPath = Request.MapPath("~/UploadDocuments/uploads/" + fname);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                TempData["Error"] = "Customer Invoice generation failed..!";
                return View(invoiceModel);
            }
            ModelState.Clear();

            //}
            //else
            //{
            //    string customerlist = client.GetcustomerList();
            //    if (!string.IsNullOrEmpty(customerlist))
            //    {
            //        ViewBag.Customers = JsonConvert.DeserializeObject<List<CustomerModel>>(customerlist).Select(x => new { x.Id, x.Name });
            //    }
            //    else
            //    {
            //        ViewBag.Customers = "";
            //    }
            //    return View(invoiceModel);
            //}
        }

        public ActionResult GenerateInvoice(string Id)
        {
            InvoiceViewModel invoiceModel = new InvoiceViewModel();
            invoiceModel.Customer = new CustomerModel();
            invoiceModel.FromDate = DateTime.Now;
            invoiceModel.ToDate = DateTime.Now;
            invoiceModel.InvoiceDate = DateTime.Now;
            if (!string.IsNullOrEmpty(Id))
            {
                BCBSClient client = new BCBSClient();
                string CustomerData = client.GetcustomerById(Convert.ToInt64(Id));
                if (!string.IsNullOrEmpty(CustomerData))
                {
                    CustomerModel Customer = new CustomerModel();
                    Customer = JsonConvert.DeserializeObject<CustomerModel>(CustomerData);
                    if (Customer != null)
                    {
                        invoiceModel.Customer = Customer;
                    }
                }
            }
            return PartialView(invoiceModel);
        }

        public JsonResult GetActivitiesByContactId(string Id)
        {
            BCBSClient client = new BCBSClient();
            string result = client.GetActivitiesByContractIds(Id);
            List<ActivityListModel> contractActivityList = JsonConvert.DeserializeObject<List<ActivityListModel>>(result).Where(x => x.IsBilled != true).ToList();
            return Json(contractActivityList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPlanActivitiesByContactId(string Id)
        {
            BCBSClient client = new BCBSClient();
            string result = client.GetActivitiesByContractIds(Id);
            List<ActivityListModel> contractActivityList = JsonConvert.DeserializeObject<List<ActivityListModel>>(result).Where(x => x.IsBilled != true && x.Estimate == false).ToList();
            return Json(contractActivityList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContractsByCustomerIdHaveActivity(string Id)
        {
            BCBSClient client = new BCBSClient();
            string result = client.GetContractbyCustomerIdHaveActivity(Convert.ToInt64(Id));
            List<CustomerContractModel> customerContractList = JsonConvert.DeserializeObject<List<CustomerContractModel>>(result);
            return Json(customerContractList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetContractByCustomerIdHaveProjectedActivity(string Id)
        {
            BCBSClient client = new BCBSClient();
            string result = client.GetContractbyCustomerIdHaveActivity(Convert.ToInt64(Id));
            List<CustomerContractModel> customerContractList = JsonConvert.DeserializeObject<List<CustomerContractModel>>(result).Where(x => x.Estimate == "Projected").ToList();
            return Json(customerContractList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SBFEmailView(SBFEmailViewModel sbf)
        {
            BCBSClient client = new BCBSClient();
            sbf.ActivityList = new List<ActivityListModel>();
            string result = client.GetActivitiesByContractIds("23,24");
            List<ActivityListModel> activity = JsonConvert.DeserializeObject<List<ActivityListModel>>(result);
            sbf.ActivityList = activity;
            string CustomerData = client.GetcustomerById(4);
            if (!string.IsNullOrEmpty(CustomerData))
            {
                sbf.Customer = JsonConvert.DeserializeObject<CustomerModel>(CustomerData);
            }
            InvoiceViewModel ivm = new InvoiceViewModel();
            ivm.AuthorizedBy = "Paatrick Egan";
            ivm.AuthorizedByExt = "5497";
            ivm.ContractId = 12;
            ivm.CustomerId = 4;
            ivm.FromDate = DateTime.Now;
            ivm.ToDate = DateTime.Now.AddDays(2);
            ivm.TotalAmount = 350.00;
            sbf.Invoice = ivm;
            string ContractData = client.GetcontractById(24);
            ContractModel contract = new ContractModel();
            if (!string.IsNullOrEmpty(ContractData))
            {
                contract = JsonConvert.DeserializeObject<ContractModel>(ContractData);
                sbf.Contract = contract;
            }
            return View(sbf);
        }

        [HttpGet]
        public ActionResult GrandBillingForm()
        {
            BCBSClient client = new BCBSClient();
            string customerlist = client.GetCustomerListForPlanCustomer();
            if (!string.IsNullOrEmpty(customerlist))
            {
                ViewBag.Customers = JsonConvert.DeserializeObject<List<CustomerModel>>(customerlist).Select(x => new { x.Id, x.Name });
            }
            else
            {
                ViewBag.Customers = "";
            }
            return View();
        }
        [HttpPost]
        public ActionResult GrandBillingForm(InvoiceViewModel invoiceModel, HttpPostedFileBase file, FormCollection fc)
        {
            BCBSClient client = new BCBSClient();
            //ModelState.Where(m => m.Key == "CustomerCode").FirstOrDefault().Value.Errors.Clear();
            //if (ModelState.IsValid)
            //{
            long Id = 0;
            string fname = string.Empty;
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                string ext = Path.GetExtension(file.FileName);
                Guid g = Guid.NewGuid();

                fname = g.ToString() + ext;
                // store the file inside ~/UploadDocuments/uploads folder
                var path = Path.Combine(Server.MapPath("~/UploadDocuments/uploads"), fname);
                file.SaveAs(path);
                invoiceModel.SupportingDocuments = fname;
            }
            //string csv = string.Empty;
            Id = client.InsertCustomerInvoice(invoiceModel.InvoiceNumber, invoiceModel.CustomerId, invoiceModel.InvoiceDate, invoiceModel.PrepareBy, invoiceModel.PrepareByExt, invoiceModel.AuthorizedBy, invoiceModel.AuthorizedByExt, invoiceModel.Division, invoiceModel.IsDeffered, invoiceModel.DefferedAccount, invoiceModel.FromDate, invoiceModel.ToDate, invoiceModel.SpecialInstuction, invoiceModel.SupportingDocuments, invoiceModel.TotalAmount);
            if (Id > 0)
            {
                //csv += "Invoice Number," + invoiceModel.InvoiceNumber + "\n";
                //csv += "Invoice Date," + invoiceModel.InvoiceDate.ToString("MM/dd/yyyy") + "\n";
                //csv += "Prepared by," + invoiceModel.PrepareBy + ", Authorized By," + invoiceModel.AuthorizedBy + "\n";
                //csv += "Devision," + invoiceModel.Division + "\n\n";

                //csv += "Customer Name," + invoiceModel.Customer.Name + "\n";
                //csv += "Address\n";
                //csv += invoiceModel.Customer.CustomerAddress + " " + invoiceModel.Customer.City + " " + invoiceModel.Customer.State + " " + invoiceModel.Customer.PostalCode + "\n";
                //csv += "Contact Name," + invoiceModel.Customer.FirstName + " " + invoiceModel.Customer.LastName + "\n";
                //csv += "Phone," + invoiceModel.Customer.Phone + "\n\n";

                SBFEmailViewModel sbfemail = new SBFEmailViewModel();
                //mailer.SBF();
                string fcActivities = fc["Activities"].ToString();
                List<string> activities = new List<string>();
                if (!string.IsNullOrEmpty(fcActivities))
                {
                    bool isbilled = client.SetActivityBilled(fcActivities);
                    if (isbilled)
                    {
                        var activityIds = fcActivities.Split(',');
                        foreach (var x in activityIds)
                        {
                            if (!string.IsNullOrEmpty(x))
                            {
                                long activityId = Convert.ToInt64(x);
                                long sbfActivityId = client.InsertSBFActivity(Id, activityId);
                                if (sbfActivityId > 0)
                                {
                                    activities.Add(activityId.ToString());

                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                if (activities != null && activities.Count > 0)
                {
                    string ids = string.Join(",", activities.ToArray());
                    string result = client.GetActivitiesByActivityIds(ids);
                    List<ActivityListModel> activity = JsonConvert.DeserializeObject<List<ActivityListModel>>(result);
                    if (sbfemail.ActivityList == null)
                    {
                        //csv += "Contract -Activity\n\n";
                        //csv += "Project Name,Service Type,From Date,End Date,Charges,Value,Amount\n";
                        sbfemail.ActivityList = new List<ActivityListModel>();
                    }
                    if (activity != null)
                    {
                        //foreach (ActivityListModel x in activity)
                        //{
                        //    string charge = string.Empty;
                        //    if (x.Charges == true) { charge = "Expense"; } else { charge = "Revenue"; }
                        //    string estimate = string.Empty;

                        //    if (x.Estimate == true) { estimate = "Real"; } else { estimate = "Estimate"; }

                        //    csv += x.ProjectName + "," + x.Service + "," + x.FromDate.ToString("MM/dd/yyyy") + "," + x.EndDate.ToString("MM/dd/yyyy") + "," + charge + "," + estimate + "," + x.Amount + "\n";
                        //}
                        //csv += ",,,,,Total," + invoiceModel.TotalAmount + "\n\n";
                        //if (invoiceModel.IsDeffered == true)
                        //{
                        //    csv += "Deffered Account," + invoiceModel.DefferedAccount + "\n";
                        //}
                        //csv += "From Date," + invoiceModel.FromDate.ToString("MM/dd/yyyy") + ",,End Date," + invoiceModel.ToDate.ToString("MM/dd/yyyy") + "\n";
                        sbfemail.ActivityList = activity;
                    }
                }

                IUserMailer mailer = new UserMailer();

                sbfemail.Invoice = invoiceModel;
                string CustomerData = client.GetcustomerById(invoiceModel.CustomerId);
                if (!string.IsNullOrEmpty(CustomerData))
                {
                    sbfemail.Customer = JsonConvert.DeserializeObject<CustomerModel>(CustomerData);
                }
                string ContractData = client.GetcontractById(invoiceModel.ContractId);
                ContractModel contract = new ContractModel();
                if (!string.IsNullOrEmpty(ContractData))
                {
                    contract = JsonConvert.DeserializeObject<ContractModel>(ContractData);
                    sbfemail.Contract = contract;
                }
                mailer.SBF(sbfemail, file, "Grand Bill - BCBS Acc Sys").Send();
                string fileName = "GBF_" + sbfemail.Invoice.InvoiceNumber + "_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss");
                string excelFileName = CreatExcel(sbfemail, fileName);
                if (!string.IsNullOrEmpty(excelFileName))
                {
                    string path = Path.Combine(Server.MapPath("~/UploadDocuments/uploads/xlsx"), excelFileName);
                    return File(path, "text/csv", excelFileName);
                }
                else
                {

                    TempData["Message"] = "Customer grand bill generated successfully..!";

                    //string fileName = "SBF_" + invoiceModel.InvoiceNumber + "_" + DateTime.Now.ToString("MM-dd-yyyy");
                    //
                    return RedirectToAction("Index", "Customer");
                }
                //TempData["Message"] = "Customer grand bill generated successfully..!";
                //string fileName = "GBF_" + invoiceModel.InvoiceNumber + "_" + DateTime.Now.ToString("MM-dd-yyyy");
                //return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", fileName + ".csv");
                //return RedirectToAction("Index", "Customer");
            }
            else
            {
                string fullPath = Request.MapPath("~/UploadDocuments/uploads/" + fname);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                TempData["Error"] = "Customer grand bill generation failed..!";
                return View(invoiceModel);
            }
            ModelState.Clear();

            //}
            //else
            //{
            //    string customerlist = client.GetcustomerList();
            //    if (!string.IsNullOrEmpty(customerlist))
            //    {
            //        ViewBag.Customers = JsonConvert.DeserializeObject<List<CustomerModel>>(customerlist).Select(x => new { x.Id, x.Name });
            //    }
            //    else
            //    {
            //        ViewBag.Customers = "";
            //    }
            //    return View(invoiceModel);
            //}
        }

        [HttpGet]
        public ActionResult AcknowledgementForm()
        {
            BCBSClient client = new BCBSClient();
            string customerlist = client.GetcustomerList();
            if (!string.IsNullOrEmpty(customerlist))
            {
                ViewBag.Customers = JsonConvert.DeserializeObject<List<CustomerModel>>(customerlist).Select(x => new { x.Id, x.Name });
            }
            else
            {
                ViewBag.Customers = "";
            }
            //string servicelist = client.GetServiceTypeList();
            //if (!string.IsNullOrEmpty(servicelist))
            //{
            //    ViewBag.Services = JsonConvert.DeserializeObject<List<ServiceModel>>(servicelist).Select(x => new { x.Id, x.Name });
            //}
            //else
            //{
            //    ViewBag.Services = "";
            //}
            string projectList = client.GetProjectList();
            if (!string.IsNullOrEmpty(projectList))
            {
                ViewBag.Projects = JsonConvert.DeserializeObject<List<ProjectModel>>(projectList).Select(x => new { x.Id, x.Name });
            }
            else
            {
                ViewBag.Projects = "";
            }
            return View();
        }
        [HttpPost]
        public ActionResult AcknowledgementForm(AcknowledgementModel avm, FormCollection fc)
        {
            BCBSClient client = new BCBSClient();
            string removedProjects = fc["RemovedProjects"].ToString();
            string removedServices = fc["RemovedServices"].ToString();
            List<string> removedProjectList = new List<string>();
            if (!string.IsNullOrEmpty(removedProjects))
            {
                removedProjectList = removedProjects.Split(',').ToList();
            }

            List<string> removedServiceList = new List<string>();
            if (!string.IsNullOrEmpty(removedServices))
            {
                removedServiceList = removedServices.Split(',').ToList();
            }
            long ackId = client.InsertCustomerAcknoeledgement(avm.CustomerId);
            if (ackId > 0)
            {
                if (avm.Projects != null)
                {
                    for (int i = 0; i < avm.Projects.Count(); i++)
                    {
                        if (removedProjectList.Count > 0)
                        {
                            var r = removedProjectList.Where(x => x.Equals(avm.Projects[i].Project.Id + "@" + i));
                            if (r != null)
                            {
                                if (r.Count() > 0)
                                {
                                    continue;
                                }
                            }
                        }
                        if (avm.Projects[i].Services != null)
                        {
                            for (int j = 0; j < avm.Projects[i].Services.Count; j++)
                            {
                                var r = removedServiceList.Where(x => x.Equals(i + "_" + j));
                                if (r != null)
                                {
                                    if (r.Count() > 0)
                                    {
                                        continue;
                                    }
                                }
                                long ackServiceId = client.InsertAcknowledgementServices(ackId, avm.Projects[i].Project.Id, avm.Projects[i].Services[j].Id, avm.Projects[i].Services[j].Total, avm.Projects[i].Services[j].NewVolume, avm.Projects[i].FromDate, avm.Projects[i].EndDate, avm.Projects[i].Services[j].FeesType);
                            }
                        }
                    }
                    string fileName = "Ack_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss");
                    string retunfileName = CreateAcknowledgementExcel(avm, fileName, removedProjects, removedServices);
                    if (!string.IsNullOrEmpty(retunfileName))
                    {
                        string path = Path.Combine(Server.MapPath("~/UploadDocuments/Acknowledgements"), retunfileName);
                        return File(path, "text/csv", retunfileName);
                    }
                    else
                    {

                        TempData["Message"] = "Customer grand bill generated successfully..!";

                        //string fileName = "SBF_" + invoiceModel.InvoiceNumber + "_" + DateTime.Now.ToString("MM-dd-yyyy");
                        //
                        return RedirectToAction("acknowledgements", "customer");
                    }
                    //TempData["Message"] = "Acknowledgement Bill generated successfully..";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult ViewAcknowledgeForm(string Id)
        {
            BCBSClient client = new BCBSClient();
            AcknowledgementModel avm = new AcknowledgementModel();
            string customerlist = client.GetcustomerList();
            if (!string.IsNullOrEmpty(customerlist))
            {
                ViewBag.Customers = JsonConvert.DeserializeObject<List<CustomerModel>>(customerlist).Select(x => new { x.Id, x.Name });
            }
            else
            {
                ViewBag.Customers = "";
            }
            string servicelist = client.GetServiceTypeList();
            if (!string.IsNullOrEmpty(servicelist))
            {
                ViewBag.Services = JsonConvert.DeserializeObject<List<ServiceModel>>(servicelist).Select(x => new { x.Id, x.Name });
            }
            else
            {
                ViewBag.Services = "";
            }
            string projectList = client.GetProjectList();
            if (!string.IsNullOrEmpty(projectList))
            {
                ViewBag.Projects = JsonConvert.DeserializeObject<List<ProjectModel>>(projectList).Select(x => new { x.Id, x.Name });
            }
            else
            {
                ViewBag.Projects = "";
            }
            if (!string.IsNullOrEmpty(Id))
            {

                List<CustomerAcknowledgementServicesModel> ackService = new List<CustomerAcknowledgementServicesModel>();
                string dataResult = client.GetAcknowledgementServicesbyAcknowledgemetnId(Convert.ToInt64(Id));

                if (!string.IsNullOrEmpty(dataResult))
                {
                    ackService = JsonConvert.DeserializeObject<List<CustomerAcknowledgementServicesModel>>(dataResult);
                    if (ackService.Count > 0)
                    {
                        avm.Id = ackService[0].AcknowledementId;
                        avm.CustomerId = ackService[0].CustomerId;
                        avm.Status = ackService[0].Status;
                    }
                }
                if (ackService.Count > 0)
                {
                    List<long> Projects = ackService.Select(x => x.ProjectId).Distinct().ToList();
                    var ServicebyProjects = ackService.ToList();

                    ViewBag.SelectedProjects = JsonConvert.SerializeObject(Projects);
                    ViewBag.ProjectServices = dataResult;
                }
            }
            return View(avm);
        }

        [HttpPost]
        public ActionResult ViewAcknowledgeForm(AcknowledgementModel avm, FormCollection fc)
        {
            bool isResult = false;
            if (avm.Id > 0)
            {

                BCBSClient client = new BCBSClient();
                List<CustomerAcknowledgementServicesModel> ackService = new List<CustomerAcknowledgementServicesModel>();
                string dataResult = client.GetAcknowledgementServicesbyAcknowledgemetnId(Convert.ToInt64(avm.Id));
                if (!string.IsNullOrEmpty(dataResult))
                {
                    ackService = JsonConvert.DeserializeObject<List<CustomerAcknowledgementServicesModel>>(dataResult);
                }
                if (ackService.Count > 0)
                {

                    foreach (CustomerAcknowledgementServicesModel ack in ackService)
                    {
                        string ContractCode = GenerateUniqueContractCode().ToUpper();
                        long id = client.Insertcontract(ack.CustomerId, ack.ServiceId, ack.FromDate, ack.EndDate, true, false, "Active", ack.Volume, ack.Total, ack.ProjectId, "", ContractCode, "", ack.FeesType);
                        //long id = client.Insertcontract(ack.CustomerId, ack.ServiceId, ack.FromDate, ack.EndDate, true, false, "Active", "", ack.Total, ack.ProjectId, "", ContractCode, "", "");
                        if (id > 0)
                        {
                            isResult = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (isResult == true)
                    {
                        long id = client.AcknowledgementApprove(Convert.ToInt64(avm.Id));
                        TempData["Message"] = "Acknowledgement Approved ! Contract Added Successfully!!";
                        return RedirectToAction("acknowledgements", "customer");
                    }
                    else
                    {
                        string customerlist = client.GetcustomerList();
                        if (!string.IsNullOrEmpty(customerlist))
                        {
                            ViewBag.Customers = JsonConvert.DeserializeObject<List<CustomerModel>>(customerlist).Select(x => new { x.Id, x.Name });
                        }
                        else
                        {
                            ViewBag.Customers = "";
                        }
                        string servicelist = client.GetServiceTypeList();
                        if (!string.IsNullOrEmpty(servicelist))
                        {
                            ViewBag.Services = JsonConvert.DeserializeObject<List<ServiceModel>>(servicelist).Select(x => new { x.Id, x.Name });
                        }
                        else
                        {
                            ViewBag.Services = "";
                        }
                        string projectList = client.GetProjectList();
                        if (!string.IsNullOrEmpty(projectList))
                        {
                            ViewBag.Projects = JsonConvert.DeserializeObject<List<ProjectModel>>(projectList).Select(x => new { x.Id, x.Name });
                        }
                        else
                        {
                            ViewBag.Projects = "";
                        }

                    }
                }
            }
            //BCBSClient client = new BCBSClient();
            //string removedProjects = fc["RemovedProjects"].ToString();
            //string removedServices = fc["RemovedServices"].ToString();
            //List<string> removedProjectList = new List<string>();
            //if (!string.IsNullOrEmpty(removedProjects))
            //{
            //    removedProjectList = removedProjects.Split(',').ToList();
            //}

            //List<string> removedServiceList = new List<string>();
            //if (!string.IsNullOrEmpty(removedServices))
            //{
            //    removedServiceList = removedServices.Split(',').ToList();
            //}
            //long ackid = avm.Id;
            ////long ackId = client.InsertCustomerAcknoeledgement(avm.CustomerId);
            //if (ackid > 0)
            //{
            //    if (avm.Projects != null)
            //    {
            //        for (int i = 0; i < avm.Projects.Count(); i++)
            //        {
            //            if (removedProjectList.Count > 0)
            //            {
            //                var r = removedProjectList.Where(x => x.Equals(avm.Projects[i].Project.Id + "@" + i));
            //                if (r != null)
            //                {
            //                    if (r.Count() > 0)
            //                    {
            //                        continue;
            //                    }
            //                }
            //            }
            //            if (avm.Projects[i].Services != null)
            //            {
            //                for (int j = 0; j < avm.Projects[i].Services.Count; j++)
            //                {
            //                    var r = removedServiceList.Where(x => x.Equals(i + "_" + j));
            //                    if (r != null)
            //                    {
            //                        if (r.Count() > 0)
            //                        {
            //                            continue;
            //                        }
            //                    }
            //                    long ackserviceId = avm.Projects[i].Services[j].Id;
            //                    //long ackServiceId = client.InsertAcknowledgementServices(ackid, avm.Projects[i].Project.Id, avm.Projects[i].Services[j].Id, avm.Projects[i].Services[j].Total, avm.Projects[i].FromDate, avm.Projects[i].EndDate);
            //                }
            //            }
            //        }
            //        string fileName = "Ack_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss");
            //        string retunfileName = CreateAcknowledgementExcel(avm, fileName, removedProjects, removedServices);
            //        if (!string.IsNullOrEmpty(retunfileName))
            //        {
            //            string path = Path.Combine(Server.MapPath("~/UploadDocuments/Acknowledgements"), retunfileName);
            //            //return File(path, "text/csv", retunfileName);
            //        }
            //        else
            //        {

            //            TempData["Message"] = "Customer grand bill generated successfully..!";

            //            //string fileName = "SBF_" + invoiceModel.InvoiceNumber + "_" + DateTime.Now.ToString("MM-dd-yyyy");
            //            //
            //            return RedirectToAction("acknowledgements", "customer");
            //        }
            //        //TempData["Message"] = "Acknowledgement Bill generated successfully..";
            //    }
            //}
            return View(avm);
        }

        [HttpGet]
        public JsonResult ApproveAcknowledgement(string ackId)
        {
            bool isResult = false;
            if (!string.IsNullOrEmpty(ackId))
            {

                BCBSClient client = new BCBSClient();
                List<CustomerAcknowledgementServicesModel> ackService = new List<CustomerAcknowledgementServicesModel>();
                string dataResult = client.GetAcknowledgementServicesbyAcknowledgemetnId(Convert.ToInt64(ackId));
                if (!string.IsNullOrEmpty(dataResult))
                {
                    ackService = JsonConvert.DeserializeObject<List<CustomerAcknowledgementServicesModel>>(dataResult);
                }
                if (ackService.Count > 0)
                {

                    foreach (CustomerAcknowledgementServicesModel ack in ackService)
                    {
                        string ContractCode = GenerateUniqueContractCode();
                        long id = client.Insertcontract(ack.CustomerId, ack.ServiceId, ack.FromDate, ack.EndDate, true, false, "Active",ack.Volume, ack.Total, ack.ProjectId, "", ContractCode, "", ack.FeesType);
                        if (id > 0)
                        {
                            isResult = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (isResult == true)
                    {
                        long id = client.AcknowledgementApprove(Convert.ToInt64(ackId));
                    }
                }
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GenerateProjectServiceList(string projectId, string serviceIds)
        {
            BCBSClient client = new BCBSClient();
            string dataResult = client.GetProjectServiceList(Convert.ToInt64(projectId), serviceIds);
            List<ProjectServiceListModel> projectServiceList = new List<ProjectServiceListModel>();
            projectServiceList = JsonConvert.DeserializeObject<List<ProjectServiceListModel>>(dataResult);
            return Json(projectServiceList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAcknowledgementProjectServiceList(string projectId, string serviceIds, string ackId)
        {
            BCBSClient client = new BCBSClient();
            string dataResult = client.GetAcknowledgementProjectServiceList(Convert.ToInt64(projectId), serviceIds, ackId);
            List<ProjectServiceListModel> projectServiceList = new List<ProjectServiceListModel>();
            projectServiceList = JsonConvert.DeserializeObject<List<ProjectServiceListModel>>(dataResult);
            return Json(projectServiceList, JsonRequestBehavior.AllowGet);
        }


        //[HttpGet]
        //public JsonResult GetFeesTypeByServiceID(string serviceId)
        //{
        //    string feestype = string.Empty;
        //    if (!string.IsNullOrEmpty(serviceId))
        //    {
        //        BCBSClient client = new BCBSClient();
        //        feestype = client.GetServiceFeesTypeByServiceId(Convert.ToInt64(serviceId));
        //    }
        //    return Json(feestype, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult Acknowledgements()
        {
            return View();
        }

        public JsonResult GetAcknowledgementList()
        {
            try
            {
                BCBSClient client = new BCBSClient();
                string result = client.GetAcknowledgementList();
                List<AcknowledgementListModel> CustomerList = JsonConvert.DeserializeObject<List<AcknowledgementListModel>>(result);
                return Json(CustomerList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteAcknowledgementById(string Ids)
        {
            bool isdeleted = false;
            if (!string.IsNullOrEmpty(Ids))
            {
                BCBSClient client = new BCBSClient();
                isdeleted = client.DeleteAcknowledgementById(Ids);
            }
            return Json(isdeleted, JsonRequestBehavior.AllowGet);
        }
        public FileContentResult DownloadCSV()
        {
            string csv = string.Empty;
            csv += "Invoice Number,123\n";
            csv += "Invoice Date,7/14/2015\n";
            csv += "Prepare by,Patrick egan, Authorized By,Laurie Condon\n";
            csv += "Devision,operation\n";
            csv += "Devision,operation\n\n";
            csv += "Customer Name,PX Conflict of Interest\n";
            csv += "Address\n";
            csv += "high Charleston West Virgi 55545\n";
            csv += "Contact Name,test_f test_l\n";
            csv += "Phone,98989898\n";
            csv += "Contract -Activity\n";
            csv += "Project Name,Service Type,From Date,End Date,Charges,Value,Amount\n";
            csv += "PlanConnexion Shared Services,Transaction ST,07/02/2015,07/04/2015,Expense,Estimate,$500\n";
            csv += "'','','','','',Total,$500\n";
            csv += "deffered,no\n";
            csv += "From Date,07/02/2015,EndDate,07/02/2015\n";

            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Report123.csv");
        }
        public string CreatExcel(SBFEmailViewModel sbfemail, string fileName)
        {
            string path = Path.Combine(Server.MapPath("~/UploadDocuments/uploads/xlsx"));
            DirectoryInfo d = new DirectoryInfo(@path);
            if (d.Exists)
            {

                FileInfo newFile = new FileInfo(d.FullName + @"\" + fileName + ".xlsx");
                if (newFile.Exists)
                {
                    newFile.Delete();  // ensures we create a new workbook
                    newFile = new FileInfo(d.FullName + @"\" + fileName + ".xlsx");
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {

                    // add a new worksheet to the empty workbook
                    string titleHeader = string.Empty;
                    if (fileName.IndexOf("SBF") > -1)
                    {
                        titleHeader = "SINGLE-BILLING REQUEST FORM";
                    }
                    else if (fileName.IndexOf("GBF") > -1)
                    {
                        titleHeader = "GRAND-BILLING FORM";
                    }
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(titleHeader);
                    string imagepath = Path.Combine(Server.MapPath("~/Images/"), "bcbs-excel.png");
                    Image logo = Image.FromFile(imagepath);
                    int a = 1;
                    //worksheet.Row(a * 5).Height = 39.00D;
                    var picture = worksheet.Drawings.AddPicture(a.ToString(), logo);
                    picture.From.Column = 6;
                    picture.From.Row = a;
                    picture.SetSize(130, 100);
                    //Add the headers
                    worksheet.Column(1).Width = 5;
                    worksheet.Column(2).Width = 10;
                    worksheet.Column(3).Width = 10;
                    worksheet.Column(4).Width = 10;
                    worksheet.Column(5).Width = 10;
                    worksheet.Column(6).Width = 10;
                    worksheet.Cells["B1"].Value = titleHeader;
                    worksheet.Cells["B3"].Value = "Invoice Nuber";
                    worksheet.Cells["c3"].Value = sbfemail.Invoice.InvoiceNumber;
                    worksheet.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    //worksheet.Cells["F4"].Value = "(will be provided by Finance)";

                    worksheet.Cells["B5"].Value = "Invoice Date";
                    worksheet.Cells["C5"].Value = sbfemail.Invoice.InvoiceDate.ToString("MM/dd/yyyy");
                    worksheet.Cells["C5"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    worksheet.Cells["B7"].Value = "Prepared By";
                    worksheet.Cells["C7"].Value = sbfemail.Invoice.PrepareBy;
                    worksheet.Cells["C7"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    worksheet.Cells["D7"].Value = "EXT";
                    worksheet.Cells["E7"].Value = sbfemail.Invoice.PrepareByExt;
                    worksheet.Cells["E7"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    worksheet.Cells["B8"].Value = "Authorized By";
                    worksheet.Cells["C8"].Value = sbfemail.Invoice.AuthorizedBy;
                    worksheet.Cells["C8"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    worksheet.Cells["D8"].Value = "EXT";
                    worksheet.Cells["E8"].Value = sbfemail.Invoice.AuthorizedByExt;
                    worksheet.Cells["E8"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    worksheet.Cells["B9"].Value = "Division";
                    worksheet.Cells["C9"].Value = sbfemail.Invoice.Division;
                    worksheet.Cells["C9"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    worksheet.Cells["B11"].Value = "Billning Information";
                    worksheet.Cells["B11"].Style.Font.Bold = true;


                    worksheet.Cells["B12"].Value = sbfemail.Customer.Name;
                    worksheet.Cells["B12"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    worksheet.Cells["D12"].Value = sbfemail.Customer.ChargeCode;

                    worksheet.Cells["B13"].Value = "Plan/Customer Name";
                    worksheet.Cells["D13"].Value = "Plan/Customer #";

                    worksheet.Cells["B14"].Value = sbfemail.Customer.CustomerAddress; ;
                    worksheet.Cells["B14"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    worksheet.Cells["D14"].Value = sbfemail.Customer.City; ;
                    worksheet.Cells["D14"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    worksheet.Cells["B15"].Value = "Address";
                    worksheet.Cells["D15"].Value = "City";

                    worksheet.Cells["B16"].Value = sbfemail.Customer.State;
                    worksheet.Cells["B16"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    worksheet.Cells["d16"].Value = sbfemail.Customer.PostalCode;
                    worksheet.Cells["d16"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    worksheet.Cells["f16"].Value = "";
                    worksheet.Cells["f16"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);


                    worksheet.Cells["B17"].Value = "State";
                    worksheet.Cells["D17"].Value = "Zip Code";
                    worksheet.Cells["f17"].Value = "Customer PO#";

                    worksheet.Cells["B18"].Value = sbfemail.Customer.FirstName + " " + sbfemail.Customer.LastName;
                    worksheet.Cells["B18"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    worksheet.Cells["d18"].Value = sbfemail.Customer.Phone;
                    worksheet.Cells["d18"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);


                    worksheet.Cells["B19"].Value = "Customer Contact Name";
                    worksheet.Cells["d19"].Value = "Telephone #";

                    worksheet.Cells["B20"].Value = "When multiple products/services are billed and/or different charge codes are to be used, please list each.";

                    worksheet.Cells["B21"].Value = "Product/Service";
                    worksheet.Cells["C21"].Value = "GL Acct";
                    worksheet.Cells["D21"].Value = "RC";
                    worksheet.Cells["E21"].Value = "Project";
                    worksheet.Cells["F21"].Value = "Amount";

                    int last = 21;

                    if (sbfemail.ActivityList != null)
                    {
                        int tableinit = 21;
                        int tableend = tableinit + sbfemail.ActivityList.Count();
                        for (int i = 0; i < sbfemail.ActivityList.Count(); i++)
                        {
                            last++;
                            if (sbfemail.ActivityList[i].FeesType == "Transaction")
                            {
                                worksheet.Cells["B" + last].Value = sbfemail.ActivityList[i].ProjectName + " (" + sbfemail.ActivityList[i].RateVolume + " " + sbfemail.ActivityList[i].Service + " @ $" + sbfemail.ActivityList[i].Rate + " )";
                            }
                            else if (sbfemail.ActivityList[i].FeesType == "Hourly")
                            {
                                worksheet.Cells["B" + last].Value = sbfemail.ActivityList[i].ProjectName + "-" + sbfemail.ActivityList[i].Service + " (" + sbfemail.ActivityList[i].RateVolume + " hours @ $" + sbfemail.ActivityList[i].Rate + " )";
                            }
                            else
                            {
                                worksheet.Cells["B" + last].Value = sbfemail.ActivityList[i].ProjectName + "-" + sbfemail.ActivityList[i].Service;
                            }
                            worksheet.Cells["C" + last].Value = sbfemail.ActivityList[i].GLAccount;
                            worksheet.Cells["D" + last].Value = sbfemail.ActivityList[i].RC;
                            worksheet.Cells["E" + last].Value = sbfemail.ActivityList[i].ProjectCode;
                            worksheet.Cells["F" + last].Value = sbfemail.ActivityList[i].Amount.ToString("C");
                        }
                        using (var range = worksheet.Cells[tableinit, 2, tableend, 6])
                        {
                            range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                        }
                    }
                    last++;
                    worksheet.Cells["E" + last].Value = "Total";
                    worksheet.Cells["F" + last].Value = sbfemail.Invoice.TotalAmount.ToString("C");
                    last++;
                    worksheet.Cells["B" + last].Value = "Deffered?";
                    worksheet.Cells["C" + last].Value = sbfemail.Invoice.IsDeffered ? "Yes" : "No";
                    worksheet.Cells["C" + last].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    last++;

                    worksheet.Cells["B" + last].Value = "Deffered Acct";
                    worksheet.Cells["C" + last].Value = sbfemail.Invoice.DefferedAccount;
                    worksheet.Cells["C" + last].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    worksheet.Cells["e" + last].Value = "From date";
                    worksheet.Cells["f" + last].Value = sbfemail.Invoice.FromDate.ToString("MM/dd/yyyy");
                    worksheet.Cells["f" + last].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    worksheet.Cells["h" + last].Value = "End date";
                    worksheet.Cells["i" + last].Value = sbfemail.Invoice.ToDate.ToString("MM/dd/yyyy");
                    worksheet.Cells["i" + last].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    last++;
                    worksheet.Cells["B" + last].Value = "Special Instruction";
                    worksheet.Cells["C" + last].Value = sbfemail.Invoice.SpecialInstuction;
                    worksheet.Cells["C" + last].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells["C" + last].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    last++;
                    worksheet.Cells["B" + last].Value = "Supporting document";
                    worksheet.Cells["C" + last].Value = "";
                    //worksheet.Cells["C29"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    last++;
                    worksheet.Cells["b" + last].Value = "Please send the completed form and all supporting documents to Finance via email at billing.ar@bcbsa.com or via ";
                    worksheet.Cells["b" + last].Style.Font.Size = 10;
                    //worksheet.Cells["b30"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    ////Add a formula for the value-column
                    //worksheet.Cells["E2:E4"].Formula = "C2*D2";

                    ////Ok now format the table range ;

                    worksheet.Cells["B11:F11"].Merge = true;
                    worksheet.Cells["B20:f20"].Merge = true;

                    worksheet.Cells["B" + last + ":h" + last].Merge = true;
                    worksheet.Cells["B" + last + ":h" + last].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    last--;
                    worksheet.Cells["B" + last + ":f" + last].Merge = true;
                    worksheet.Cells["B" + last + ":f" + last].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    package.Save();
                }
                return newFile.Name;
            }
            else
            {
                return null;
            }
        }

        public string CreateAcknowledgementExcel(AcknowledgementModel avm, string fileName, string removedProjects, string removedServices)
        {
            string path = Path.Combine(Server.MapPath("~/UploadDocuments/Acknowledgements"));
            DirectoryInfo d = new DirectoryInfo(@path);
            if (d.Exists)
            {
                FileInfo newFile = new FileInfo(d.FullName + @"\" + fileName + ".xlsx");
                if (newFile.Exists)
                {
                    newFile.Delete();  // ensures we create a new workbook
                    newFile = new FileInfo(d.FullName + @"\" + fileName + ".xlsx");
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    // add a new worksheet to the empty workbook
                    string titleHeader = string.Empty;
                    titleHeader = "BILLING ACKNOWLEDGEMENT FORM";

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(titleHeader);
                    string imagepath = Path.Combine(Server.MapPath("~/Images/"), "bcbs-excel.png");
                    Image logo = Image.FromFile(imagepath);
                    int a = 1;
                    //worksheet.Row(a * 5).Height = 39.00D;
                    var picture = worksheet.Drawings.AddPicture(a.ToString(), logo);
                    picture.From.Column = 6;
                    picture.From.Row = a;
                    picture.SetSize(130, 100);
                    //Add the headers
                    worksheet.Column(1).Width = 5;
                    worksheet.Column(2).Width = 10;
                    worksheet.Column(3).Width = 10;
                    worksheet.Column(4).Width = 10;
                    worksheet.Column(5).Width = 10;
                    worksheet.Column(6).Width = 10;
                    CustomerModel customer = new CustomerModel();
                    BCBSClient client = new BCBSClient();
                    customer = JsonConvert.DeserializeObject<CustomerModel>(client.GetcustomerById(avm.CustomerId));
                    worksheet.Cells["B1"].Value = titleHeader;
                    worksheet.Cells["B3"].Value = "PLAN/ORGANIZATION:";
                    worksheet.Cells["c3"].Value = customer.Name;
                    worksheet.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    //worksheet.Cells["F4"].Value = "(will be provided by Finance)";

                    worksheet.Cells["B5"].Value = "ATTENTION:";
                    worksheet.Cells["C5"].Value = customer.FirstName + " " + customer.LastName;
                    worksheet.Cells["C5"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    worksheet.Cells["B7"].Value = "ADDRESS:";
                    worksheet.Cells["C7"].Value = customer.CustomerAddress;
                    //worksheet.Cells["C7"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                    worksheet.Cells["C8"].Value = customer.City;
                    //worksheet.Cells["C8"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    worksheet.Cells["C9"].Value = customer.State + " " + customer.Country + " " + customer.PostalCode;
                    //worksheet.Cells["C9"].Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);

                    int last = 11;
                    int tableinit = 11;
                    int tableend = 0;

                    for (int i = 0; i < avm.Projects.Count(); i++)
                    {
                        List<string> removedProjectList = new List<string>();
                        if (!string.IsNullOrEmpty(removedProjects))
                        {
                            removedProjectList = removedProjects.Split(',').ToList();
                        }
                        if (removedProjectList.Count > 0)
                        {
                            var r = removedProjectList.Where(x => x.Equals(avm.Projects[i].Project.Id + "@" + i));
                            if (r != null)
                            {
                                if (r.Count() > 0)
                                {
                                    continue;
                                }
                            }
                        }
                        last++;
                        worksheet.Cells["B" + last].Value = avm.Projects[i].Project.Name;
                        tableinit = last;
                        int totalinit = 0;
                        int totalend = 0;
                        for (int j = 0; j < avm.Projects[i].Services.Count; j++)
                        {
                            last++;
                            if (j == 0)
                            {
                                worksheet.Cells["C" + last].Value = "Service Name";
                                worksheet.Cells["D" + last].Value = "Volume";
                                worksheet.Cells["E" + last].Value = "Rate";
                                worksheet.Cells["F" + last].Value = "Total";
                                last++;
                                totalinit = last;
                            }
                            List<string> removedServiceList = new List<string>();
                            if (!string.IsNullOrEmpty(removedServices))
                            {
                                removedServiceList = removedServices.Split(',').ToList();
                            }
                            var r = removedServiceList.Where(x => x.Equals(i + "_" + j));
                            if (r != null)
                            {
                                if (r.Count() > 0)
                                {
                                    last--;
                                    continue;
                                }
                            }
                            worksheet.Cells["C" + last].Value = avm.Projects[i].Services[j].Name;
                            worksheet.Cells["D" + last].Value = avm.Projects[i].Services[j].NewVolume;
                            if (avm.Projects[i].Services[j].FeesType == "Transaction")
                            {
                                worksheet.Cells["E" + last].Value = avm.Projects[i].Services[j].Total.ToString("C") + " / " + avm.Projects[i].Services[j].NewVolume + " " + avm.Projects[i].Services[j].FeesType;//avm.Projects[i].Services[j].Volume;
                            }
                            else
                            {
                                worksheet.Cells["E" + last].Value = avm.Projects[i].Services[j].Total.ToString("C") + "/ " + avm.Projects[i].Services[j].FeesType;
                            }
                            worksheet.Cells["F" + last].Value = avm.Projects[i].Services[j].Total;
                            worksheet.Cells["F" + last].Style.Numberformat.Format = "$#,###0.00";
                        }
                        totalend = last;
                        tableend = last;
                        last++;
                        worksheet.Cells["F" + last].Formula = "=SUM(F" + totalinit + ":F" + totalend + ")";
                        worksheet.Cells["F" + last].Calculate();
                        worksheet.Cells["F" + last].Style.Numberformat.Format = "$#,###0.00";
                        last++;
                        using (var range = worksheet.Cells[tableinit, 2, tableend, 6])
                        {
                            range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.Black);
                        }
                    }
                    package.Save();
                }
                return newFile.Name;
            }
            else
            {
                return null;
            }
        }

        public string GenerateUniqueContractCode()
        {
            string Result = string.Empty;
            Guid g = Guid.NewGuid();
            Result = g.ToString().Substring(0, g.ToString().IndexOf('-'));
            BCBSClient client = new BCBSClient();
            bool isexist = client.CheckIsContractCodeExist(Result);
            if (isexist)
            {
                GenerateUniqueContractCode();
            }
            return Result;
        }
    }
}
