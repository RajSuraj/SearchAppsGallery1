using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SearchAppsGallery1
{
    [Binding]
    public class SearchAppsGallery1Steps
    {
        private string _theUrl;
        private string _theResponse;
        private string _theapiresponse;

        [Given(@"the alteryx service is running at ""(.*)""")]
        public void GivenTheAlteryxServiceIsRunningAt(string alteryxUrl)
        {
            _theUrl = alteryxUrl;
        }
        
        [When(@"I invoke GET at application details at ""(.*)"" for ""(.*)""")]
        public void WhenIInvokeGETAtApplicationDetailsAtFor(string apiurl, string expectedapp)
        {
            string Url = _theUrl + "/" + apiurl;
            WebRequest webRequest = System.Net.WebRequest.Create(Url);
            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new System.IO.StreamReader(responseStream);
            string responseFromServer = reader.ReadToEnd();
            _theResponse = responseFromServer;


            var dict = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<dynamic>(_theResponse);
            int count = dict["recordCount"];
            int i = 0;
            for (i = 0; i <= count - 1; i++)
            {
                if (dict["records"][i]["primaryApplication"]["metaInfo"]["name"] == expectedapp)
                {
                    break;
                }
            }
            string id = dict["records"][i]["id"];

            string apiUrl = " http://gallery.alteryx.com/api/apps/" + id;
            WebRequest apirequest = WebRequest.Create(apiUrl);
            WebResponse apiresponse = apirequest.GetResponse();
            Stream apiresponseStream = apiresponse.GetResponseStream();
            StreamReader newreader = new System.IO.StreamReader(apiresponseStream);
            string apiresponseFromServer = newreader.ReadToEnd();
            _theapiresponse = apiresponseFromServer;


        }
        
        [Then(@"I see run count > (.*)")]
        public void ThenISeeRunCount(int appruncount)
        {
            var expected = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(_theapiresponse);
            int runcount = expected["runCount"];
            Console.Write("No of times the app has run" + runcount);
            Assert.IsTrue(runcount > appruncount);

        }
        
        [Then(@"I see description contains ""(.*)""")]
        public void ThenISeeDescriptionContains(string expecteddesc)
        {
            var expected = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(_theapiresponse);
            string description = expected["primaryApplication"]["metaInfo"]["description"];
            StringAssert.Contains(expecteddesc, description);

        }
    }
}
