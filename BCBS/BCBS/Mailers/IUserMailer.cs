using BCBS.Models;
using Mvc.Mailer;
using System.Web;

namespace BCBS.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage Welcome();
			MvcMailMessage GoodBye();
            MvcMailMessage SBF(SBFEmailViewModel sbfemail, HttpPostedFileBase file,string subject);
	}
}