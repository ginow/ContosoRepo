using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ExportOrDeleteRecords
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConnectToD365();
            }
            catch (Exception ex)
            {
                errorText(ex.Message + "\nStacktrace: " + ex.StackTrace);                
            }
            Console.Read();
        }

        private static void ConnectToD365()
        {
            ClientCredentials clientCredentials = new ClientCredentials();
            clientCredentials.UserName.UserName = ConfigurationManager.AppSettings["userName"].ToString();
            clientCredentials.UserName.Password = ConfigurationManager.AppSettings["password"];
            normalText("User name: " + ConfigurationManager.AppSettings["userName"].ToString());
            //Below security protocol wasn't present in earlier versions of CRM
            // For Dynamics 365 Customer Engagement V9.X, set Security Protocol as TLS12
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Get the URL from CRM, Navigate to Settings -> Customizations -> Developer Resources
            // Copy and Paste Organization Service Endpoint Address URL              
            //string environment = "https://fellow.crm8.dynamics.com/main.aspx";
            string uri = ConfigurationManager.AppSettings["uri"].ToString();
            normalText("uri: " + ConfigurationManager.AppSettings["uri"].ToString());

            normalText("Creating organizationService...");
            IOrganizationService organizationService = (IOrganizationService)new OrganizationServiceProxy(new Uri(uri), null, clientCredentials, null);
            normalText("Executing whoamirequest...");
            Guid userid = ((WhoAmIResponse)organizationService.Execute(new WhoAmIRequest())).UserId;

            if (userid != Guid.Empty)
            {
                normalText("Who am i?: " + userid);
                successText("Connection Established Successfully!");             
            }
        }
        #region Colourful Console Writeline Methods
        private static void successText(string v)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(v);
        }

        private static void normalText(string v)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(v);
        }

        private static void errorText(string v)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(v);
        }

        private static void warningText(string v)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(v);
        }
        #endregion
    }
}
