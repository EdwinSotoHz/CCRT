using System;
using System.Collections.Generic;
using System.IO;
using Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.Pdf
{
    public class GraduatePdfGenerator
    {
        private readonly List<GraduatesVM> _students;
        private readonly string _headerText = "ACUSE DE EGRESADOS";
        private readonly string _footerText = "Centro Cultural Regional Tlahuelilpan • Hidalgo, México";

        // Datos del encabezado institucional
        private readonly string _institutionName = "Centro Cultural Regional Tlahuelilpan";
        private readonly string _institutionAddress = "Av. Universidad s/n, Centro, CP 42780, Tlahuelilpan, Hidalgo";
        private readonly string _institutionContact = "Tels.: (763) 786 00 10, cel. (773) 124 69 53";
        private readonly string _institutionEmail = "vicrohe09@hotmail.com";
        private readonly string _institutionExtra = "Imparte talleres de danza, música, artes plásticas y escénicas.";

        public GraduatePdfGenerator(List<GraduatesVM> students)
        {
            _students = students;
        }

        public byte[] GeneratePdf()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                // Pie de página personalizado
                writer.PageEvent = new PdfFooter(_footerText);

                document.Open();

                // Encabezado institucional
                AddInstitutionHeader(document);

                // Título del documento
                AddTitle(document);

                // Tabla de datos
                AddStudentTable(document);

                document.Close();
                return ms.ToArray();
            }
        }

        private void AddInstitutionHeader(Document document)
        {
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.Black);
            Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.DarkGray);

            PdfPTable headerTable = new PdfPTable(1)
            {
                WidthPercentage = 100,
                DefaultCell = { Border = Rectangle.NO_BORDER }
            };

            headerTable.AddCell(new PdfPCell(new Phrase(_institutionName, boldFont)) { Border = Rectangle.NO_BORDER });
            headerTable.AddCell(new PdfPCell(new Phrase("Ayuntamiento de Tlahuelipan / Dirección de Educación y Cultura", normalFont)) { Border = Rectangle.NO_BORDER });
            headerTable.AddCell(new PdfPCell(new Phrase(_institutionAddress, normalFont)) { Border = Rectangle.NO_BORDER });
            headerTable.AddCell(new PdfPCell(new Phrase(_institutionContact, normalFont)) { Border = Rectangle.NO_BORDER });
            headerTable.AddCell(new PdfPCell(new Phrase(_institutionEmail, normalFont)) { Border = Rectangle.NO_BORDER });
            headerTable.AddCell(new PdfPCell(new Phrase("facebook 1 | abrir en Google Maps", normalFont)) { Border = Rectangle.NO_BORDER });
            headerTable.AddCell(new PdfPCell(new Phrase("\nDatos generales:\nEl edificio fue construido exprofeso para convertirse en el Centro Regional de Cultura, lugar que dará cabida a personas no solo del municipio sino de toda la región.", normalFont)) { Border = Rectangle.NO_BORDER });

            document.Add(headerTable);
        }

        private void AddTitle(Document document)
        {
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.Black);
            Paragraph title = new Paragraph(_headerText, titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 15f
            };
            document.Add(title);

            Paragraph dateLine = new Paragraph($"Fecha: {DateTime.Now.ToString("dd/MM/yyyy")}", FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.Gray))
            {
                Alignment = Element.ALIGN_RIGHT,
                SpacingAfter = 10f
            };
            document.Add(dateLine);
        }

        private void AddStudentTable(Document document)
        {
            PdfPTable table = new PdfPTable(5)
            {
                WidthPercentage = 100,
                DefaultCell = { MinimumHeight = 12f },
                HorizontalAlignment = Element.ALIGN_LEFT
            };

            // Anchuras de columna
            table.SetWidths(new float[] { 2, 2, 3, 1, 1 });

            // Fuentes
            Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.White);
            BaseColor headerBg = new BaseColor(83, 109, 254); // Azul principal

            // Encabezados
            table.AddCell(new PdfPCell(new Phrase("Taller", headerFont)) { BackgroundColor = headerBg, Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase("Grupo", headerFont)) { BackgroundColor = headerBg, Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase("Nombre Completo", headerFont)) { BackgroundColor = headerBg, Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase("Calificación", headerFont)) { BackgroundColor = headerBg, Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase("Asistencia (%)", headerFont)) { BackgroundColor = headerBg, Padding = 5 });

            // Contenido
            Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.Black);

            foreach (var item in _students)
            {
                table.AddCell(new PdfPCell(new Phrase(item.Taller, contentFont)));
                table.AddCell(new PdfPCell(new Phrase(item.Grupo, contentFont)));
                table.AddCell(new PdfPCell(new Phrase(item.NombreCompleto, contentFont)));
                table.AddCell(new PdfPCell(new Phrase(item.Calificacion?.ToString("F2") ?? "-", contentFont)));
                table.AddCell(new PdfPCell(new Phrase(item.Asistencia?.ToString("F2") ?? "-", contentFont)));
            }

            document.Add(table);
        }

        // Clase interna para pie de página
        private class PdfFooter : PdfPageEventHelper
        {
            private readonly string _footerText;

            public PdfFooter(string footerText)
            {
                _footerText = footerText;
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                PdfPTable footerTable = new PdfPTable(1)
                {
                    TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin,
                    LockedWidth = true
                };

                PdfPCell cell = new PdfPCell(new Phrase(_footerText, FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8, BaseColor.Gray)))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 10f
                };

                footerTable.AddCell(cell);
                footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin - 10, writer.DirectContent);
            }
        }
    }
}