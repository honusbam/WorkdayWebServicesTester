using System.ServiceModel;
using WorkdayWebServices.Human_ResourcesService;

namespace WWS_tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Human_ResourcesPortClient hr = new Human_ResourcesPortClient();
            hr.Endpoint.Address = new EndpointAddress("https://wd2-impl-services1.workday.com/ccx/service/{TENANT_NAME_HERE}/Human_Resources/v28.1");

            //Specify the username and password for WS-Security UsernameToken Header
            hr.ClientCredentials.UserName.UserName = "lmcneil@{TENANT_NAME_HERE}";   //put a working username with credentials here.  include the @tenant, replace {TENANT_NAME_HERE} with tenant, no curly brackets
            hr.ClientCredentials.UserName.Password = "validPassword";         //put a working password here

            //Instantiate Header for the request
            Workday_Common_HeaderType header = new Workday_Common_HeaderType();
            header.Include_Reference_Descriptors_In_Response = false;
            header.Include_Reference_Descriptors_In_ResponseSpecified = false;

            //Setting up request criteria to use Country
            Get_Workers_RequestType request = new Get_Workers_RequestType();
            Worker_Request_CriteriaType requestCriteria = new Worker_Request_CriteriaType();
            CountryObjectIDType[] us = new CountryObjectIDType[1];
            us[0] = new CountryObjectIDType() { type = "ISO_3166-1_Alpha-2_Code", Value = "US" };
            CountryObjectType[] country = new CountryObjectType[1];
            country[0] = new CountryObjectType() { ID = us };
            requestCriteria.Country_Reference = country;

            //Setting up filter to get data for 5 workers
            Response_FilterType responseFilter = new Response_FilterType();
            responseFilter.Count = 5;
            responseFilter.CountSpecified = true;

            // Formating response to contain Personal information, Employment information and Compensation
            Worker_Response_GroupType responseGroup = new Worker_Response_GroupType();
            responseGroup.Include_Reference = true;
            responseGroup.Include_Personal_Information = true;
            responseGroup.Include_Compensation = true;
            responseGroup.Include_Employment_Information = true;
            responseGroup.Include_ReferenceSpecified = true;
            responseGroup.Include_Personal_InformationSpecified = true;
            responseGroup.Include_CompensationSpecified = true;
            responseGroup.Include_Employment_InformationSpecified = true;

            //updating preferences for the request
            request.Request_Criteria = requestCriteria;
            request.Response_Filter = responseFilter;
            request.Response_Group = responseGroup;
            request.version = "v28.1";      //should match the version from the URL

            //Invoke HR getworker api via Proxy
            var workers = hr.Get_Workers(header, request);
        }
    }
}
