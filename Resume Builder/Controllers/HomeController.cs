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
            string formData = "{\"name\":\"Sidhartha Sankar Prusty\",\"email\":\"siddharth.prusty@gmail.com\",\"phone\":\"7873391346\",\"careerObj\":\"will write anything of 400 characters. and repeat my phrases.I will write anything of 400 characters.\\nand repeat my phrases.I will write anything of 400 characters. and repeat my phrases.I will write\\nanything of 400 characters. and repeat my phrases.I will write anything of 400 characters. and repeat\\nmy phrases.I will write anything of 400 characters. and repeat my phrases.I will write anything of\",\"sex\":\"Male\",\"father\":\"Pradyumna Prusty\",\"dob\":\"1995-05-03\",\"maritalSts\":\"Unmarried\",\"nationality\":\"Indian\",\"achievements\":[[\"aadsad\",\"dsasad\",\"adasdsad\",\"asdsad\"]],\"skills\":[[\"asdada\",\"adad\",\"sadasd\",\"sadad\"]],\"academics\":[[\"10\",\"\",\"State Board\",\"Ravenshaw Collegiate School\",\"82.50\",\"2010\"],[\"12\",\"Science\",\"State Board\",\"Stewart Science College\",\"78\",\"2012\"],[\"Graduation\",\"B.Sc(IT)\",\"Ravenshaw University\",\"Ravenshaw University\",\"81.95\",\"2015\"],[\"Post Graduation\",\"MCA\",\"BPUT\",\"NIST\",\"88.50\",\"2017\"]]}";

            //string formData = "{\"name\":\"Sidhartha Sankar Prusty\",\"email\":\"siddharth.prusty@gmail.com\",\"phone\":\"7873391346\",\"careerObj\":\"will write anything of 400 characters. and repeat my phrases.I will write anything of 400 characters.\\nand repeat my phrases.I will write anything of 400 characters. and repeat my phrases.I will write\\nanything of 400 characters. and repeat my phrases.I will write anything of 400 characters. and repeat\\nmy phrases.I will write anything of 400 characters. and repeat my phrases.I will write anything of\",\"sex\":\"Male\",\"father\":\"Pradyumna Prusty\",\"dob\":\"1995-05-03\",\"maritalSts\":\"Unmarried\",\"nationality\":\"Indian\",\"achievements\":[[\"aadsad\",\"dsasad\",\"adasdsad\",\"asdsad\"]],\"skills\":[[\"asdada\",\"adad\",\"sadasd\",\"sadad\"]],\"academics\":[[\"10\",\"\",\"State Board\",\"Ravenshaw Collegiate School\",\"82.50\",\"2010\"],[\"12\",\"Science\",\"State Board\",\"Stewart Science College\",\"78\",\"2012\"],[\"Graduation\",\"B.Sc(IT)\",\"Ravenshaw University\",\"Ravenshaw University\",\"81.95\",\"2015\"]]}";

            JObject _jObject = new JObject();
            _jObject = JObject.Parse(formData);

            //Font Color
            var FontColour = new BaseColor(0, 0, 0);
            Font header = new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK);
            Font nameFont = new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.BOLD, BaseColor.BLACK);
            var headingFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 15, FontColour);
            var contentFont = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, FontColour);
            var contentFont2 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, FontColour);


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

                    LineSeparator lineSeparator = new LineSeparator();
                    lineSeparator.DrawLine(content, 50, 590, 720);

                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase("CAREER OBJECTIVE:", headingFont), 50, 700, 0);
                    ColumnText ct = new ColumnText(content);
                    ct.SetSimpleColumn(50f, 620f, 590f, 700f);
                    ct.AddElement(new Paragraph(_jObject.GetValue("careerObj").ToString()));
                    ct.Go();
                    float y = 600f;
                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase("ACADEMIC QUALIFICATION:", headingFont), 50, y, 0);
                    y -= 20;

                    PdfPTable table = new PdfPTable(6);

                    table.AddCell("Course");
                    table.AddCell("Specialization");
                    table.AddCell("Board");
                    table.AddCell("College/University");
                    table.AddCell("Marks(%/CGPA)");
                    table.AddCell("Year of Completion");
                    table.HeaderRows = 1;
                    table.SetWidths(new int[] { 17, 20, 20, 25, 25, 17 });

                    JArray academics = (JArray)_jObject.GetValue("academics");
                    float ury = y;
                    int count = 0;
                    foreach (var item in academics.Children())
                    {
                        foreach (var subItem in item.Children())
                        {
                            table.AddCell(subItem.ToString());
                            y -= 20;
                        }
                        count++;
                    }

                    //ct.SetSimpleColumn(-10f, 240f, 650f, 590f);
                    ct.SetSimpleColumn(-10f, y, 650f, ury);
                    ct.AddElement(table);
                    ct.Go();
                    y = ((count == 4) ? 420 : 440);

                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase("ACHIEVEMENTS:", headingFont), 50, y, 0);
                    y -= 20;
                    JArray achievements = (JArray)_jObject.GetValue("achievements");

                    foreach (var item in achievements.Children())
                    {
                        foreach (var subItem in item.Children())
                        {
                            ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase(" • " + subItem.ToString(), contentFont2), 60, y, 0);
                            y -= 20;
                        }
                    }

                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase("SKILLS:", headingFont), 50, y, 0);
                    y -= 20;

                    JArray skills = (JArray)_jObject.GetValue("skills");
                    foreach (var item in skills.Children())
                    {
                        foreach (var subItem in item.Children())
                        {
                            ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase(" • " + subItem.ToString(), contentFont2), 60, y, 0);
                            y -= 20;
                        }
                    }

                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase("PERSONAL INFORMATION:", headingFont), 50, y, 0);
                    y -= 20;

                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase(" • Father's Name :  " + _jObject.GetValue("father"), contentFont2), 60, y, 0);
                    y -= 20;
                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase(" • Date of Birth   :  " + _jObject.GetValue("dob"), contentFont2), 60, y, 0);
                    y -= 20;
                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase(" • Gender            :   " + _jObject.GetValue("sex"), contentFont2), 60, y, 0);
                    y -= 20;
                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase(" • Marital Status :   " + _jObject.GetValue("maritalSts"), contentFont2), 60, y, 0);
                    y -= 20;
                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase(" • Nationality      :   " + _jObject.GetValue("nationality"), contentFont2), 60, y, 0);
                    y -= 20;
                    ColumnText.ShowTextAligned(content, Element.ALIGN_JUSTIFIED, new Phrase("DECLARATION:", headingFont), 50, y, 0);
                    y -= 20;

                    ct.SetSimpleColumn(50f, y - 20, 590f, y + 20);
                    ct.AddElement(new Paragraph("I hereby declare that all the above mentioned information given by me is true and correct to the best of my knowledge and belief."));
                    ct.Go();

                    y -= 30;
                    ColumnText.ShowTextAligned(content, Element.ALIGN_LEFT, new Phrase("Place:", headingFont), 50, y, 0);
                    y -= 20;
                    ColumnText.ShowTextAligned(content, Element.ALIGN_LEFT, new Phrase("Date :", headingFont), 50, y, 0);
                    ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase("Signature", headingFont), 470, y, 0);

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