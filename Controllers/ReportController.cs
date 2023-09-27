using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;
using System.Reflection;
using System.Text;

namespace Proyecto.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;


        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("GenerateReport/{reportName}")]
        public ActionResult GenerateReport(string reportName)
        {

            List<Alumno> alumnosList = _context.Alumnos?.Include(a => a.Carrera).OrderBy(a => a.Carrera.Nombre).ThenBy(a => a.Nombre).ToList();
            List<Profesor> profesoresList = _context.Profesores?.Include(a => a.Carrera).OrderBy(a => a.Nombre).ToList();
            List<Carrera> carrerasList = _context.Carreras.OrderBy(c => c.Nombre).ThenBy(c => c.Duracion).ToList();

            var returnString = GenerateReportAsync(reportName, profesoresList, carrerasList, alumnosList);

            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".pdf");
        }

        public byte[] GenerateReportAsync(string reportName, List<Profesor> profesoresList, List<Carrera> carrerasList, List<Alumno> alumnosList)
        {
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("ReportAPI.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}ReportFiles\\{1}.rdlc", fileDirPath, reportName);
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);
            report.AddDataSource("DataSetProfesor", profesoresList);
            report.AddDataSource("DataSetCarreras", carrerasList);
            report.AddDataSource("DataSetAlumnos", alumnosList);

            //var result = report.Execute(GetRenderType("pdf"), 0, parameters);
            //var result = report.Execute(RenderType.Pdf, 1);
            var result = report.Execute(RenderType.Pdf);
            return result.MainStream;
        }

        private RenderType GetRenderType(string reportType)
        {
            var renderType = RenderType.Pdf;
            switch (reportType.ToLower())
            {
                default:
                case "pdf":
                    renderType = RenderType.Pdf;
                    break;
                case "word":
                    renderType = RenderType.Word;
                    break;
                case "excel":
                    renderType = RenderType.Excel;
                    break;
            }

            return renderType;
        }
    }
}

