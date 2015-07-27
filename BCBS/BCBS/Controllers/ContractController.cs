using BCBS.bsbcserviceref;
using BCBS.Models;
using BCBS.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCBS.Controllers
{
    public class ContractController : Controller
    {
        //
        // GET: /Contract/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult New()
        {
            BCBSClient client = new BCBSClient();
            string customerlist = client.GetcustomerList();
           
            ContractModel contract = new ContractModel();
            contract.FromDate = DateTime.Now;
            contract.EndDate = DateTime.Now;
            contract.ContractCode = GenerateUniqueContractCode().ToUpper();
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
            return View(contract);
        }

        [HttpPost]
        public ActionResult New(ContractModel contract, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                BCBSClient client = new BCBSClient();
                long Id = 0;
                string fname = string.Empty;
                if (file != null && file.ContentLength > 0)
                {
                    byte[] imgarray;
                    using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                    {
                        imgarray = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                        string logodata = Convert.ToBase64String(imgarray);
                        string lg = "data:image/*;base64," + logodata;
                    }
                    try
                    {
                        // extract only the fielname
                        var fileName = Path.GetFileName(file.FileName);
                        string ext = Path.GetExtension(file.FileName);
                        Guid g = Guid.NewGuid();

                        fname = g.ToString() + ext;
                        // store the file inside ~/UploadDocuments/uploads folder
                        var path = Path.Combine(Server.MapPath("~/UploadDocuments/Contracts"), fname);
                        file.SaveAs(path);
                        contract.FileName = fname;
                    }
                    catch (Exception ex)
                    { }
                }
                Id = client.Insertcontract(contract.CustomerId, contract.ServiceId, contract.FromDate, contract.EndDate, contract.Dirrection, contract.Estimate, contract.Status, contract.Volume, contract.Amount, contract.ProjectId, contract.Description, contract.ContractCode, contract.FileName,contract.FeesType);
                if (Id > 0)
                {
                    TempData["Message"] = "Contract Added successfully..!";
                }
                else
                {
                    TempData["Error"] = "Contract Adding failed..!";
                }
                ModelState.Clear();
                return RedirectToAction("Index", "Contract");
            }
            else
            {
                return View(contract);
            }
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            BCBSClient client = new BCBSClient();
            string customerlist = client.GetcustomerList();
            ContractModel contract = new ContractModel();
            contract.FromDate = DateTime.Now;
            contract.EndDate = DateTime.Now;
            if (!string.IsNullOrEmpty(customerlist))
            {
                ViewBag.Customers = JsonConvert.DeserializeObject<List<CustomerModel>>(customerlist).Select(x => new { x.Id, x.Name });
            }
            else
            {
                ViewBag.Customers = "";
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
            if (id > 0)
            {
                string ContractData = client.GetcontractById(id);
                if (!string.IsNullOrEmpty(ContractData))
                {
                    contract = JsonConvert.DeserializeObject<ContractModel>(ContractData);
                    string servicelist = client.GetServiceTypesByProjectId(contract.ProjectId);
                    if (!string.IsNullOrEmpty(servicelist))
                    {
                        ViewBag.Services = JsonConvert.DeserializeObject<List<ServiceModel>>(servicelist).Select(x => new { x.Id, x.Name });
                    }
                    else
                    {
                        ViewBag.Services = "";
                    }
                }
                else
                {
                    TempData["Error"] = "Requested contract not available!!";
                    return RedirectToAction("Index", "Contract");
                }
            }

            return View(contract);
        }

        [HttpPost]
        public ActionResult Edit(ContractModel contract, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                BCBSClient client = new BCBSClient();
                long Id = 0;
                string oldDescription = fc["OldDescription"].ToString();
                contract.Description += System.Environment.NewLine + System.Environment.NewLine + oldDescription;
                contract.Description = contract.Description.Replace("'", "''");
                Id = client.UpdatecontractById(contract.Id, contract.EndDate, contract.Status, contract.Description);
                if (Id > 0)
                {
                    TempData["Message"] = "Contract Updated successfully..!";
                }
                else
                {
                    TempData["Error"] = "Contract Update failed..!";
                }
                ModelState.Clear();
                return RedirectToAction("Index", "Contract");
            }
            else
            {
                return View(contract);
            }
        }

        [HttpGet]
        public ActionResult NewActivity(string Id)
        {
            ActivityModel activity = new ActivityModel();
            activity.FromDate = DateTime.Now;
            activity.EndDate = DateTime.Now;
            activity.ActivityCode = GenerateUniqueActivityCode();
            if (!string.IsNullOrEmpty(Id))
            {
                BCBSClient client = new BCBSClient();
                ContractModel contract = new ContractModel();
                string result = client.GetcontractById(Convert.ToInt64(Id));
                if (!string.IsNullOrEmpty(result))
                {
                    contract = JsonConvert.DeserializeObject<ContractModel>(result);
                    activity.ContractCode = contract.ContractCode;
                }
                activity.ContractId = Convert.ToInt64(Id);
                ViewBag.AvailableBalance = client.GetContractAvailableBalance(Convert.ToInt64(Id));
            }
            return View(activity);
        }
        [HttpPost]
        public ActionResult NewActivity(ActivityModel activity, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                BCBSClient client = new BCBSClient();
                string fname = string.Empty;
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the fielname
                    var fileName = Path.GetFileName(file.FileName);
                    string ext = Path.GetExtension(file.FileName);
                    Guid g = Guid.NewGuid();

                    fname = g.ToString() + ext;
                    // store the file inside ~/UploadDocuments/uploads folder
                    var path = Path.Combine(Server.MapPath("~/UploadDocuments/Activity"), fname);
                    file.SaveAs(path);
                    activity.FileName = fname;
                }
                long id = client.InsertContractActivity(activity.ContractId, activity.FromDate, activity.EndDate, activity.Volume, activity.Amount, activity.Charges, activity.Estimate, activity.Description, activity.Status, activity.FileName, activity.ContractCode + "-" + activity.ActivityCode);
                if (id > 0)
                {
                    TempData["Message"] = "Activity Added successfully..!";
                    ModelState.Clear();
                    return RedirectToAction("Activities", "Contract");
                }
                else
                {
                    TempData["Error"] = "Activity Adding failed..!";
                }
            }
            return View(activity);
        }

        [HttpGet]
        public ActionResult EditActivity(string Id)
        {
            BCBSClient client = new BCBSClient();
            ActivityModel activity = new ActivityModel();
            if (!string.IsNullOrEmpty(Id))
            {
                if (Convert.ToInt64(Id) > 0)
                {
                    string ActivityData = client.GetActivityById(Convert.ToInt64(Id));
                    if (!string.IsNullOrEmpty(ActivityData))
                    {
                        activity = JsonConvert.DeserializeObject<ActivityModel>(ActivityData);
                        ViewBag.AvailableBalance = client.GetContractAvailableBalance(activity.ContractId);
                    }
                    else
                    {
                        TempData["Error"] = "Requested Activity not available!!";
                        return RedirectToAction("Index", "Contract");
                    }
                }
                else
                {
                    TempData["Error"] = "Requested Activity not available!!";
                    return RedirectToAction("Index", "Contract");

                }
            }
            else
            {
                TempData["Error"] = "Requested Activity not available!!";
                return RedirectToAction("Index", "Contract");
            }
            return View(activity);
        }

        [HttpPost]
        public ActionResult EditActivity(ActivityModel activity)
        {
            if (ModelState.IsValid)
            {
                BCBSClient client = new BCBSClient();
                long Id = 0;
                Id = client.UpdateContractActivity(activity.Id, activity.ContractId, activity.FromDate, activity.EndDate, activity.Volume, activity.Amount, activity.Charges, activity.Estimate, activity.Description, activity.Status);
                if (Id > 0)
                {
                    TempData["Message"] = "Contract Updated successfully..!";
                }
                else
                {
                    TempData["Error"] = "Contract Update failed..!";
                }
                ModelState.Clear();
                return RedirectToAction("Activities", "Contract");
            }
            else
            {
                return View(activity);
            }
        }

        public JsonResult IsActivityCodeExist(string ActivityCode)
        {
            bool isexist = false;
            if (!string.IsNullOrEmpty(ActivityCode))
            {
                BCBSClient client = new BCBSClient();
                isexist = client.CheckIsActivityCodeExist(ActivityCode);
            }
            return Json(!isexist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Activities(long? id)
        {
            if (id > 0)
            {
                BCBSClient client = new BCBSClient();
                ViewBag.ContractId = id;
            }
            return View();
        }

        public JsonResult GetContractList()
        {
            try
            {
                BCBSClient client = new BCBSClient();
                string result = client.GetcontractList();
                //List<ContractModel> ContractList = JsonConvert.DeserializeObject<List<ContractModel>>(result);
                List<ContractListModel> ContractList = JsonConvert.DeserializeObject<List<ContractListModel>>(result);
                return Json(ContractList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteContractById(string id)
        {
            bool isdeleted = false;
            if (!string.IsNullOrEmpty(id))
            {
                BCBSClient client = new BCBSClient();
                isdeleted = client.DeleteContractById(id);
            }
            return Json(isdeleted, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContractDataById(string contractId)
        {
            try
            {
                BCBSClient client = new BCBSClient();
                string result = client.GetcontractById(Convert.ToInt64(contractId));
                //List<ContractModel> ContractList = JsonConvert.DeserializeObject<List<ContractModel>>(result);
                ContractModel Contract = JsonConvert.DeserializeObject<ContractModel>(result);
                return Json(Contract, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetServiceTypeByServiceID(string serviceId)
        {
            string feestype = string.Empty;
            if (!string.IsNullOrEmpty(serviceId))
            {
                BCBSClient client = new BCBSClient();
                feestype = client.GetServiceTypeById(Convert.ToInt64(serviceId));
            }
            return Json(feestype, JsonRequestBehavior.AllowGet);
        }
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
         public JsonResult GetServiceTypeByProjectID(string projectId)
        {
            string servicetype = string.Empty;
            if (!string.IsNullOrEmpty(projectId))
            {
                BCBSClient client = new BCBSClient();
                servicetype = client.GetServiceTypesByProjectId(Convert.ToInt64(projectId));
            }
            return Json(servicetype, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBalanceForProject(string projectId)
        {
            string balance = string.Empty;
            if (!string.IsNullOrEmpty(projectId))
            {
                BCBSClient client = new BCBSClient();
                balance = client.GetProjectAvailableBalance(Convert.ToInt64(projectId));
            }
            return Json(balance, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckStatusIsActive(string tableName, string Id)
        {
            bool isAvtive = false;
            if ((!string.IsNullOrEmpty(Id)) && (!string.IsNullOrEmpty(tableName)))
            {
                BCBSClient client = new BCBSClient();
                isAvtive = client.CheckStausIsActive(Convert.ToInt64(Id), tableName);
            }
            return Json(isAvtive, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsContractCodeExist(string ContractCode)
        {

            bool isexist = false;
            if (!string.IsNullOrEmpty(ContractCode))
            {
                //if (!string.IsNullOrEmpty(existCode))
                //{
                //if (ChargeCode.ToLower() != existCode.ToLower())
                //{
                BCBSClient client = new BCBSClient();
                isexist = client.CheckIsContractCodeExist(ContractCode);
                //    }
                //}
            }
            return Json(!isexist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLastContractActivity(string contractId)
        {
            try
            {
                BCBSClient client = new BCBSClient();
                string result = client.GetLastContractActivity(Convert.ToInt64(contractId));
                //List<ContractModel> ContractList = JsonConvert.DeserializeObject<List<ContractModel>>(result);
                ActivityModel activity = JsonConvert.DeserializeObject<ActivityModel>(result);
                return Json(activity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetActivityById(string ActivityId)
        {
            try
            {
                BCBSClient client = new BCBSClient();
                string result = client.GetActivityById(Convert.ToInt64(ActivityId));
                //List<ContractModel> ContractList = JsonConvert.DeserializeObject<List<ContractModel>>(result);
                ActivityModel activity = JsonConvert.DeserializeObject<ActivityModel>(result);
                return Json(activity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetAllActivities()
        {
            try
            {
                BCBSClient client = new BCBSClient();
                string result = client.GetAllActivities();
                //List<ContractModel> ContractList = JsonConvert.DeserializeObject<List<ContractModel>>(result);
                List<ActivityListModel> activities = JsonConvert.DeserializeObject<List<ActivityListModel>>(result);
                return Json(activities, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetActivitiesByContractId(string contractId)
        {
            try
            {
                BCBSClient client = new BCBSClient();
                string result = client.GetActivitiesByContractIds(contractId);
                //List<ContractModel> ContractList = JsonConvert.DeserializeObject<List<ContractModel>>(result);
                List<ActivityListModel> activity = JsonConvert.DeserializeObject<List<ActivityListModel>>(result);
                return Json(activity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteActivityById(string id)
        {
            bool isdeleted = false;
            if (!string.IsNullOrEmpty(id))
            {
                BCBSClient client = new BCBSClient();
                isdeleted = client.DeleteActivityById(id);
            }
            return Json(isdeleted, JsonRequestBehavior.AllowGet);
        }
        public FileResult DownloadContractDocument(string fileName)
        {
            string filepath = Path.Combine(Server.MapPath("~/UploadDocuments/Contracts"), fileName);
            return File(filepath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(filepath));
        }
        public FileResult DownloadActivityDocument(string fileName)
        {
            string filepath = Path.Combine(Server.MapPath("~/UploadDocuments/Activity"), fileName);
            return File(filepath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(filepath));
        }
        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
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
        public string GenerateUniqueActivityCode()
        {
            string Result = string.Empty;
            Guid g = Guid.NewGuid();
            Result = g.ToString().Substring(0, g.ToString().IndexOf('-'));
            BCBSClient client = new BCBSClient();
            bool isexist = client.CheckIsActivityCodeExist(Result);
            if (isexist)
            {
                GenerateUniqueActivityCode();
            }
            return Result.ToUpper();
        }
    }
}
