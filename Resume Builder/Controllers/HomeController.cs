using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json.Linq;
using Resume_Builder.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Resume_Builder.Controllers
{
    public class HomeController : Controller
    {
        public string resumeFormat;
        Resume_BuilderEntities db = new Resume_BuilderEntities();

        public ActionResult Index()
        {
            var resumes = db.Resumes.ToList();
            return View(resumes);
        }

        [HttpPost]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FillDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FillDetails(int id)
        {
            return View();
        }
        //[HttpPost]
        public ActionResult BuildResume()
        {

            //for testing
            int id = 1;
            string formData = "{\"name\":\"Sidhartha Sankar Prusty\",\"email\":\"siddharth.prusty@gmail.com\",\"phone\":\"7873391346\",\"careerObj\":\"bsadb\",\"sex\":\"Male\",\"father\":\"Sid\",\"dob\":\"2018-12-20\",\"maritalSts\":\"Unmarried\",\"nationality\":\"Indian\",\"achievements\":[[\"jsdf\"]],\"skills\":[[\"asd\"]],\"academics10\":[\"State Board\",\"RCS\",\"2010\",\"82.50\"],\"academics12\":[\"State Board\",\"Stewart\",\"Science\",\"2012\",\"63\"],\"academicsGr\":[\"Ravenshaw\",\"Ravenshaw\",\"B.Sc.(IT)\",\"2015\",\"81.95\"],\"academicsPG\":[\"BPUT\",\"NISt\",\"MCA\",\"2017\",\"88.50\"]}";
            JObject _jObject = new JObject();
            _jObject = JObject.Parse(formData);

            //Font
            var FontColour = new BaseColor(0, 0, 0);

            var userNameFont = FontFactory.GetFont("AvenirLTStd-Heavy", 14, FontColour);
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/pdf"));
            resumeFormat = dirInfo.FullName + "\\" + "blank.pdf";
            string resumeName = dirInfo.FullName + "\\" + _jObject.GetValue("name") + ".pdf";

            try
            {
                //Reads the Pdf we want to fetch
                PdfReader reader = new PdfReader(resumeFormat);

                //Creates the new PDF instance refering to the exising PDF format
                PdfStamper stamper = new PdfStamper(reader, new FileStream(resumeName, FileMode.Create));

                //Get the exact location where the file is to be modified like page number
                PdfContentByte content = stamper.GetUnderContent(1);

                if (id == 1)
                {
                    System.IO.DirectoryInfo DownloadsPDF = new System.IO.DirectoryInfo(@"D:\\resumeBuilder\Downloads\");
                    ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase(_jObject.GetValue("name").ToString(), userNameFont), 300, 750, 0);
                }

                stamper.Close();
                reader.Close();
            }
            catch (Exception e)
            {
                try
                {
                    using (StreamWriter w = System.IO.File.AppendText(@"D:\resumeBuilder\log.txt"))
                    {
                        w.WriteLine("\r\nError Log Entry : ");
                        w.WriteLine("Error occured in BuildResume method of Home controller  on {0}", DateTime.Now);
                        w.WriteLine("Error Information : ");
                        w.WriteLine("Message: {0} \n, InnerException: {1} , StackTrace {2}, Data {3} ", e.Message, e.InnerException, e.StackTrace, e.Data);
                        w.WriteLine("-----------------------------------------------------------------------------------------------");
                        w.WriteLine();
                        w.WriteLine();
                    }
                }
                catch (Exception)
                {
                }
            }

            return RedirectToAction("FillDetails");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //PDF Generation

    }
}