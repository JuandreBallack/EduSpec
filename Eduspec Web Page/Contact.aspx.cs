using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Services;

public partial class Contact : System.Web.UI.Page
{
    protected static string ReCaptcha_Key = "6LcUomYUAAAAAJ5TmN8V_xPbSQp3ZHwOTdI51UQg";
    protected static string ReCaptcha_Secret = "6LcUomYUAAAAAGp1akBru-hvEZq-iQC_b6Zgjr3f";
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void SentContact(object sender, EventArgs e)
    {
        SmtpClient mailclient = new SmtpClient()
        {
            Port = 26,
            Host = "webmail.eduspec.co.za",
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Timeout = 10000
        };

        NetworkCredential mailCred = new NetworkCredential()
        {   
            UserName = "no-reply@eduspec.co.za",
            Password = "NoReply@eduspec"
        };
        mailclient.Credentials = new NetworkCredential(mailCred.UserName, mailCred.Password);

        MailMessage mail = new MailMessage()
        {
            From = new MailAddress(String.Format("{0}", Request.Form["ctl00$Content$form_email"]), Request.Form["name"]),
            Subject = "Eduspec web contact",
            Body = String.Format("<p>Institution: {0}</p><p>Contact Name: {1}</p><p>Contact Phone: {2}</p><p>Contact Email: {3}</p><p>Message: {4}</p>", Request.Form["school"], Request.Form["name"], Request.Form["phone"], Request.Form["ctl00$Content$form_email"], Request.Form["message"]),
            IsBodyHtml = true           
        };

        mail.To.Add("info@eduspec.co.za");
        mail.CC.Add("hennie@eduspec.co.za");
        mail.CC.Add("stephan@eduspec.co.za");

        mailclient.Send(mail);

    }

    [WebMethod]
    public static string VerifyCaptcha(string response)
    {
        string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + ReCaptcha_Secret + "&response=" + response;
        return (new WebClient()).DownloadString(url);
    }

}