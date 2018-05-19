using Comcast.XM.ContentPublishingService.Models;
using Comcast.XM.ContentPublishingService.Publish;
using Comcast.XM.SitecoreApiMiddleware.ScApiWrapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
//test written in git
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Comcast.XM.ContentPublishingCore.Util;
using Comcast.XM.ContentPublishingService.Models.Settings;
using Comcast.XM.ContentPublishingService.Util;
using log4net.Config;

namespace TestApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            XmlConfigurator.Configure();
        }


        private void FileCopyTest()
        {
            string targetedFolderPath = "variants/default";
            //merge downloaded files to git folder
            string copyFromRootFolder =
                "C:\\Users\\Administrator\\AppData\\Local\\Temp\\f310d95b-43f2-4fb4-8859-baf6741fca2d_CMS_INT_integration-test";
            string copyToRootFolder =
                "C:\\Users\\Administrator\\AppData\\Local\\Temp\\f310d95b-43f2-4fb4-8859-baf6741fca2d_Akamai_INT_";

            if (!string.IsNullOrEmpty(copyToRootFolder) && Directory.Exists(copyToRootFolder))
            {
                string copyFromDir = Path.Combine(copyFromRootFolder, targetedFolderPath.Replace('/', '\\'));
                string copyToDir = Path.Combine(copyToRootFolder, targetedFolderPath.Replace('/', '\\'));

                 FilesUtil.CopyFiles(copyFromDir, copyToDir, false, "*.json");

            }

            richTextBox1.Text = "Done!!!";
        }

        private void PublishTest()
        {
            var targets = ContentDeliveryTargetUtil.GetSiteSettings("http://local.cds.com/cdsapi/v1/settings/getsettings", "{9FFE62F6-FAE2-46E3-A6E9-2D1B4604767A}");

            if (targets != null && targets.Any())
            {
                var intSettings = targets.FirstOrDefault(x => x.Code.Equals("INT", StringComparison.InvariantCultureIgnoreCase));

                if (intSettings != null)
                {
                    PublishEngineContext publishEngineContext = new PublishEngineContext();
                    publishEngineContext.PublishTargetSettings = intSettings;

                    PublishEngine publishEngine = new PublishEngine();

                    var results = publishEngine.Publish(publishEngineContext);




                    richTextBox1.AppendText(string.Format("MainContentTempFolder:{0}\n", publishEngineContext.Results.MainContentTempFolder));
                    richTextBox1.AppendText(string.Format("MainContentCloned:{0}\n", publishEngineContext.Results.MainContentCloned));

                    richTextBox1.AppendText(string.Format("JsonFilesTempFolder:{0}\n", publishEngineContext.Results.JsonFilesTempFolder));
                    richTextBox1.AppendText(string.Format("JsonFilesDownloaded:{0}\n", publishEngineContext.Results.JsonFilesDownloaded));

                    richTextBox1.AppendText(string.Format("MergeSecondaryAndMaster:{0}\n", publishEngineContext.PublishTargetSettings.GitHubSettings.MergeSecondaryAndMaster));

                    richTextBox1.AppendText(string.Format("SecondaryContentTempFolder:{0}\n", publishEngineContext.Results.SecondaryContentTempFolder));
                    richTextBox1.AppendText(string.Format("SecondaryContentCloned:{0}\n", publishEngineContext.Results.SecondaryContentCloned));


                    richTextBox1.AppendText(string.Format("CiContentTempFolder:{0}\n", publishEngineContext.Results.CiContentTempFolder));
                    richTextBox1.AppendText(string.Format("CiContentCloned:{0}\n", publishEngineContext.Results.CiContentCloned));


                    richTextBox1.AppendText(string.Format("IsAbort:{0}\n", publishEngineContext.Results.IsAbort));
                    richTextBox1.AppendText(string.Format("IsFinished:{0}\n", publishEngineContext.Results.IsFinished));

                    richTextBox1.AppendText(string.Format("ErrorMessage:{0}\n\n", publishEngineContext.Results.ErrorMessage));


                    foreach (var eventItem in publishEngineContext.MessageLogger.GetEvents())
                    {
                        var typeExample = new { EntryDate = eventItem.TimeStamp.ToLongDateString(), Level = eventItem.Level.Name, Message = eventItem.MessageObject };
                        JObject o = JObject.FromObject(typeExample);
                        string json = o.ToString();
                        richTextBox1.AppendText(string.Format("{0}\n\n", json));

                    }



                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //FileCopyTest();

            PublishTest();




        }

        /*
          public void TransferContent()
            {
                string path = "/454830/modesto/Test/mobile-static/content2/";
                try
                {
                    // Setup session options
                    SessionOptions sessionOptions = new SessionOptions
                    {
                        Protocol = Protocol.Sftp,
                        HostName = "ebizmicro.upload.akamai.com",
                        UserName = "sshacs",
                        SshPrivateKeyPath = @"C:\workspace\cmsdeployer\.ssh\id_rsa_AkamaiDeploy.ppk",
                        // Password = "mypassword",
                        SshHostKeyFingerprint = "ssh-dss-ee-33-bd-ac-7b-6e-bd-0b-60-6e-49-20-56-cb-00-d3"
                    };

                    using (Session session = new Session())
                    {
                        session.FileTransferred += Session_FileTransferred;
                        // Connect
                        session.Open(sessionOptions);

                        // Upload files
                        TransferOptions transferOptions = new TransferOptions();
                        transferOptions.TransferMode = TransferMode.Binary;

                        TransferOperationResult transferResult;

                        transferResult =
                            session.PutFiles(@"C:\workspace\zTest\*", path, false, transferOptions);

                        // Throw on any error
                        transferResult.Check();

                        // Print results
                        foreach (TransferEventArgs transfer in transferResult.Transfers)
                        {
                            Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                        }


                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e);

                }
            }

            private void Session_FileTransferred(object sender, TransferEventArgs e)
            {
                Console.WriteLine("Upload of {0} succeeded", e.FileName);
            }
        
        */
        private void button2_Click(object sender, EventArgs e)
        {

          
                
            /*
                
                  string proName= "coverageModule-learnMore-networkButton-networkButton2:The Network";

                 var mainObj = JsonStringPropParser.BuildObjects2(proName);
                 if (mainObj != null)
                 {             



                     var contentJson = JsonConvert.SerializeObject(mainObj, Formatting.Indented, new JsonSerializerSettings
                     {
                         NullValueHandling = NullValueHandling.Include,
                         ContractResolver = new CamelCasePropertyNamesContractResolver()
                     });

                     richTextBox1.AppendText("\n\n" + contentJson);

                 }



                 return;*/
            //string landingId = "{30092F61-BC5F-47C2-8FB5-264A49692B9E}";
            string landingId = "{891FB7E6-55FB-4F8A-9133-44C899BCF0EF}";
            SitecoreItemService sitecoreItemService = new SitecoreItemService("http://local.comcast.com/sitecore/api/ssc/item/", "master");
          string payload=  sitecoreItemService.LoadContent(landingId);

            richTextBox1.AppendText("\n\n...........................\n" + payload + "\n");
            /*  string landingId = "{891FB7E6-55FB-4F8A-9133-44C899BCF0EF}";
            //  
            dynamic landingPageInput = GetValues<ExpandoObject>(landingId);
            var landingPage = BuildObject(landingPageInput);

            var landingPageJson = JsonConvert.SerializeObject(landingPage, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

           
            */
        }


   
     

    }
}
