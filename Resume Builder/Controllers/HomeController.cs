using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
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
            string formData = "{\"name\":\"Sidhartha Sankar Prusty\",\"email\":\"siddharth.prusty@gmail.com\",\"phone\":\"7873391346\",\"careerObj\":\"I'd like to know how I can stop the text overflowing a given region when writing. If possible it would be great if iText could also place an ellipsis character where the text does not fit. I can't find any method on ColumnText that will help either. I do not wish the content to wrap when writing. I'd like to know how I can stop the text overflowing a given region when writing. If possible it would\",\"sex\":\"Male\",\"father\":\"Pradyumna Prusty\",\"dob\":\"1995-05-03\",\"maritalSts\":\"Unmarried\",\"nationality\":\"Indian\",\"achievements\":[[\"asd\"]],\"skills\":[[\"asdsad\"]],\"academics\":[[\"10\",\"State Board\",\"Ravenshaw Collegiate School \",\"\",\"2010\",\"82.50\"],[\"12\",\"State Board\",\"Stewart Science College\",\"Science\",\"2012\",\"63\"],[\"B.Sc(IT)\",\"Ravenshaw University\",\"Ravenshaw University\",\"B.Sc.(IT)\",\"2015\",\"81.95\"],[\"MCA\",\"BPUT\",\"NIST\",\"MCA\",\"2017\",\"88.50\"]]}";
            JObject _jObject = new JObject();
            _jObject = JObject.Parse(formData);

            //Font Color
            var FontColour = new BaseColor(0, 0, 0);
            Font header = new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK);
            Font nameFont = new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD, BaseColor.BLACK);
            //var resumeFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10, FontColour);
            var contentFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10, FontColour);


            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/pdf"));
            resumeFormat = dirInfo.FullName + "\\" + "blank.pdf";
            string resumeName = dirInfo.FullName + "\\" + _jObject.GetValue("email") + ".pdf";

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
                    //Write into the PDF
                    //ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase("RESUME", header), 300, 730, 0);                     
                    ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase(_jObject.GetValue("name").ToString(), nameFont), 300, 750, 0);
                    ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase(_jObject.GetValue("email").ToString() + " • " + _jObject.GetValue("phone"), contentFont), 300, 730, 0);
                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase("CAREER OBJECTIVE:", contentFont), 50, 700, 0);
                    LineSeparator lineSeparator = new LineSeparator();
                    lineSeparator.DrawLine(content, 50, 590, 690);

                    ColumnText ct = new ColumnText(content);
                    ct.SetSimpleColumn(50f, 610f, 590f, 690f);
                    ct.AddElement(new Paragraph(_jObject.GetValue("careerObj").ToString()));
                    ct.Go();
                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase("ACADEMIC QUALIFICATION:", contentFont), 50, 590, 0);
                    lineSeparator.DrawLine(content, 50, 590, 580);

                    PdfPTable table = new PdfPTable(6);
                    ColumnText ct2 = new ColumnText(content);
                    table.AddCell("Course");
                    table.AddCell("Specialization");
                    table.AddCell("Board");
                    table.AddCell("College/University");
                    table.AddCell("Marks(%/CGPA)");
                    table.AddCell("Year of Completion");
                    table.HeaderRows = 1;
                    table.SetWidths(new int[] { 15, 20, 20, 25, 25, 20 });

                    JArray academics = (JArray)_jObject.GetValue("academics");

                    foreach (var item in academics.Children())
                    {
                        foreach (var subItem in item.Children())
                        {
                            table.AddCell(subItem.ToString());
                        }
                    }
                    Rectangle rectPage1 = new Rectangle(0, 200, 630, 550);
                    ct2.SetSimpleColumn(rectPage1);
                    ct2.AddElement(table);
                    ct2.Go();

                }
                stamper.Close();
                reader.Close();

                //Delete previous pdf if existing
                System.IO.DirectoryInfo DownloadsPDF = new System.IO.DirectoryInfo(@"D:\\resumeBuilder\Downloads\");
                foreach (System.IO.FileInfo file in DownloadsPDF.GetFiles(_jObject.GetValue("email") + ".pdf"))
                {
                    file.Delete();
                }

                //move to downloads folder
                foreach (System.IO.FileInfo file in dirInfo.GetFiles(_jObject.GetValue("email") + ".pdf"))
                {
                    file.MoveTo(@"D:\resumeBuilder\Downloads\" + _jObject.GetValue("email") + ".pdf");
                }
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

    }
}