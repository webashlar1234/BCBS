using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBCBS" in both code and config file together.
    [ServiceContract]
    public interface IBCBS
    {
        #region Project
        [OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertProject/?name={name}&charge_code={charge_code}&high_level_budget={high_level_budget}&status={status}&description={description}")]
        long InsertProject(string name, string charge_code, string high_level_budget, string status, string description, string rc, string glaccount);

        [OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetProjectList")]
        string GetProjectList();

        [OperationContract]
        string GetProjectById(long id);

        [OperationContract]
        long UpdateProjectById(long Id, string name, string charge_code, string high_level_budget, string status, string description, string rc, string glaccount);

        [OperationContract]
        bool DeleteProjectById(string id);
        [OperationContract]
        string GetProjectAvailableBalance(long id);

        [OperationContract]
        bool CheckIsChargeCodeExist(string chargecode, string tablename);
        #endregion

        #region Service
        //Service Types
        [OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "InsertProject/?name={name}&charge_code={charge_code}&high_level_budget={high_level_budget}&status={status}&description={description}")]
        long InsertServiceType(string name, long? projectid, string status,string feestype,double budget, string notes);

        [OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetProjectList")]
        string GetServiceTypeList();

        [OperationContract]
        string GetServiceTypeById(long id);
        [OperationContract]
        string GetServiceTypesByProjectId(long projectid);
        [OperationContract]
        long UpdateServiceTypeById(long Id, string name, long? projectid, string status, string notes);

        [OperationContract]
        bool DeleteServiceTypeById(string id);

        //[OperationContract]
        //long InsertServiceFeesType(long serviceid, string feestype,double amount);
        //[OperationContract]
        //string GetServiceFeesTypeByServiceId(long serviceid);
        //[OperationContract]
        //string GetServiceFeesTypeById(long id);
        //[OperationContract]
        //bool DeleteServiceFeesTypeByServiceId(string id);
        #endregion

        #region Customer
        [OperationContract]
        string GetcustomerList();
        [OperationContract]
        string GetCustomerListForSBF();
        [OperationContract]
        string GetCustomerListForPlanCustomer();
        [OperationContract]
        long Insertcustomer(string name, string charge_code, string customertype, string address, string city, string postalcode, string state, string country, string firstname, string lastname, string phone, string fax, string occupation, string email, string status);
        [OperationContract]
        long UpdatecustomerById(long Id, string name, string charge_code, string customertype, string address, string city, string postalcode, string state, string country, string firstname, string lastname, string phone, string fax, string occupation, string email, string status);
        [OperationContract]
        string GetcustomerById(long id);
        [OperationContract]
        string GetcustomerByContractId(long id);
        [OperationContract]
        bool DeletecustomerById(string id);
        #region Invoice
        [OperationContract]
        long InsertCustomerInvoice(string invoicenumber, long customerid, DateTime invoicedate, string prepareby, string preparebyext, string authorizedby, string authorizedbyext, string division, bool isdeffered, string defferedaccount, DateTime fromdate, DateTime todate, string specialinstruction, string supportingdocuments, double totalamount);
        [OperationContract]
        string GetContractByCustomerId(long id);
        [OperationContract]
        string GetContractbyCustomerIdHaveActivity(long id);
        [OperationContract]
        string GetActivitiesByCustomerId(long id);
        [OperationContract]
        long InsertSBFActivity(long sbfid, long activityid);
        [OperationContract]
        bool SetActivityBilled(string activityids);
        #endregion
        #endregion

        #region Acknowlegement
        [OperationContract]
        string GetAcknowledgementList();
        [OperationContract]
        string GetProjectServiceList(long projectId, string serviceIds);
        [OperationContract]
        string GetAcknowledgementProjectServiceList(long projectId, string serviceIds,string ackid);
        [OperationContract]
        long InsertCustomerAcknoeledgement(long customerId);
        [OperationContract]
        long InsertAcknowledgementServices(long acknowledgementid, long projectid, long serviceid, double total, string volume, DateTime fromdate, DateTime todate, string feestype);
        [OperationContract]
        string GetAcknowledgementServicesbyAcknowledgemetnId(long ackid);
        [OperationContract]
        long AcknowledgementApprove(long ackid);
        [OperationContract]
        bool DeleteAcknowledgementById(string ids);
        #endregion

        #region contract
        [OperationContract]
        string GetcontractList();
        [OperationContract]
        string GetcontractListByProjectId(long projectid);
        [OperationContract]
        long Insertcontract(long customer, long service_type, DateTime fromdate, DateTime enddate, bool dirrection, bool estimate, string status, string volume, double amount, long projectid, string description, string contractcode, string filename,string feestype);
        [OperationContract]
        long UpdatecontractById(long Id, DateTime enddate, string status, string description);
        [OperationContract]
        string GetcontractById(long id);
        [OperationContract]
        bool DeleteContractById(string id);
        [OperationContract]
        string GetContractAvailableBalance(long contractid);
        [OperationContract]
        bool CheckStausIsActive(long id, string tablename);
        [OperationContract]
        bool CheckIsContractCodeExist(string contractCode);
        [OperationContract]
        long InsertContractActivity(long contractid, DateTime fromdate, DateTime enddate, double volume, double amount, bool charges, bool estimate, string description, string status, string filename, string activitycode);
        [OperationContract]
        long UpdateContractActivity(long id, long contractid, DateTime fromdate, DateTime enddate, double volume, double amount, bool charges, bool estimate, string description, string status);
        [OperationContract]
        string GetLastContractActivity(long contractid);
        [OperationContract]
        string GetActivitiesByContractIds(string contractid);
        [OperationContract]
        string GetActivitiesByActivityIds(string activityids);
        [OperationContract]
        string GetAllActivities();
        [OperationContract]
        string GetActivityById(long activityid);
        [OperationContract]
        bool DeleteActivityById(string id);
        [OperationContract]
        bool CheckIsActivityCodeExist(string activityCode);
        //string GetFeesTypeByServiceID(long serviceId);
        [OperationContract]
        string GetContractFeesTypeByServiceId(long id);
        #endregion

        [OperationContract]
        string GetAccuralReportMonthYear();

        [OperationContract]
        string GetAccuralReportByMonthYear(string month, string year);

        [OperationContract]
        string GetAccuralReportByDate(string fromdate, string todate);

        [OperationContract]
        string GetServiceByProjectId(long projectId);
        [OperationContract]
        string GetCustomerByServiceAndProjectId(long serviceId, long projectId);
        [OperationContract]
        string GetContractDetailForActivity(long projectId, long serviceId, long customerId, string fromMonth, string toMonth, string year);
        #region Chart
        [OperationContract]
        string GetProjectRevenueExpenseMonthYear(string month, string year);

        [OperationContract]
        string GetProjectRevenueExpenseByDate(string fromdate, string todate);

        [OperationContract]
        string GetServiceRevenueExpenseMonthYear(string month, string year);

        [OperationContract]
        string GetServiceRevenueExpenseByDate(string fromdate, string todate);
        [OperationContract]
        string GetPlanCustomerRevenueExpenseMonthYear(string month, string year);
        [OperationContract]
        string GetPlanCustomerRevenueExpenseByDate(string fromdate, string todate);
        #endregion
    }
}
