using iText.Kernel.Pdf;
using iText.Layout.Properties;
using iText.Layout;
using iText.Layout.Element;
using hallocdoc_mvc_Service.Interface;
using hallodoc_mvc_Repository.ViewModel;
using iText.IO.Image;
using iText.Kernel.Pdf.Extgstate;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout.Borders;
using System.Diagnostics.Metrics;

namespace hallocdoc_mvc_Service.Implementation
{
    public class PDFService : IPDFService
    {
        public byte[] GeneratePDF(Encounter encounter)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream)
;
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                Div div = new Div();
                Image image = new Image(ImageDataFactory.Create("D:\\Projects\\.net learning\\hallo_doc\\HalloDoc_MVC\\hallodoc mvc\\wwwroot\\SRSScreenShorts\\Fig56. Patient site 1.png"));
                image.SetTextAlignment(TextAlignment.RIGHT);
                div.SetFixedPosition(50, 400, 500).SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER).SetOpacity((float)0.2);
                div.Add(image.SetWidth(500));
                document.Add(div);
                document.Add(new Paragraph("Medical Report")
                    .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20));


                document.Add(new Paragraph($"Patient Name: \t\t {encounter.patientData.FirstName + " " + encounter.patientData.LastName}"));
                document.Add(new Paragraph($"Age: \t\t {(DateTime.Now.Year - encounter.DOB.Value.Year)}"));
                document.Add(new Paragraph($"Report Date:\t\t "));
                document.Add(new Paragraph($"PDF Generate Date:\t\t {DateTime.Now.ToShortDateString()}"));
                document.Add(new Paragraph($"Address:\t\t {encounter.patientData.Address}"));
                div.Add(new Paragraph($"Mobile Number:\t\t {encounter.phone}"));
                div.Add(new Paragraph($"Email:\t\t {encounter.email}"));


                Table mainTable = new Table(UnitValue.CreatePercentArray(new float[] { 500 }));
                Table nestedTable1 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable2 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable3 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable4 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable5 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable6 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable7 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable8 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable9 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable10 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable11 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable12 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable13 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable14 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable15 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable16 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable17 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable18 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable19 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable20 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable21 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable22 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable23 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));
                Table nestedTable24 = new Table(UnitValue.CreatePercentArray(new float[] { 100, 395 }));

                mainTable.SetWidth(500);
                nestedTable1.SetMinWidth(495);
                nestedTable2.SetMinWidth(495);
                nestedTable3.SetMinWidth(495);
                nestedTable4.SetMinWidth(495);
                nestedTable5.SetMinWidth(495);
                nestedTable6.SetMinWidth(495);
                nestedTable7.SetMinWidth(495);
                nestedTable8.SetMinWidth(495);
                nestedTable9.SetMinWidth(495);
                nestedTable10.SetMinWidth(495);
                nestedTable11.SetMinWidth(495);
                nestedTable12.SetMinWidth(495);
                nestedTable13.SetMinWidth(495);
                nestedTable14.SetMinWidth(495);
                nestedTable15.SetMinWidth(495);
                nestedTable16.SetMinWidth(495);
                nestedTable17.SetMinWidth(495);
                nestedTable18.SetMinWidth(495);
                nestedTable19.SetMinWidth(495);
                nestedTable20.SetMinWidth(495);
                nestedTable21.SetMinWidth(495);
                nestedTable22.SetMinWidth(495);
                nestedTable23.SetMinWidth(495);
                nestedTable24.SetMinWidth(495);


                nestedTable1.AddCell(new Cell().Add(new Paragraph("History of Illness:").SetBold()).SetWidth(100));
                nestedTable1.AddCell(new Cell().Add(new Paragraph(encounter.HistoryIllness ?? "")));

                nestedTable2.AddCell(new Cell().Add(new Paragraph("Medial History:").SetBold()).SetWidth(100));
                nestedTable2.AddCell(new Cell().Add(new Paragraph(encounter.MedicalHistory ?? "")));

                nestedTable3.AddCell(new Cell().Add(new Paragraph("Medications:").SetBold()).SetWidth(100));
                nestedTable3.AddCell(new Cell().Add(new Paragraph(encounter.Medications ?? "")));

                nestedTable4.AddCell(new Cell().Add(new Paragraph("Allergies:").SetBold()).SetWidth(100));
                nestedTable4.AddCell(new Cell().Add(new Paragraph(encounter.Allergies ?? "")));

                nestedTable5.AddCell(new Cell().Add(new Paragraph("Temp:").SetBold()).SetWidth(100));
                nestedTable5.AddCell(new Cell().Add(new Paragraph(encounter.Temp.ToString() ?? "")));

                nestedTable6.AddCell(new Cell().Add(new Paragraph("HR:").SetBold()).SetWidth(100));
                nestedTable6.AddCell(new Cell().Add(new Paragraph(encounter.Hr.ToString() ?? "")));

                nestedTable7.AddCell(new Cell().Add(new Paragraph("RR:").SetBold()).SetWidth(100));
                nestedTable7.AddCell(new Cell().Add(new Paragraph(encounter.Rr.ToString() ?? "")));

                nestedTable8.AddCell(new Cell().Add(new Paragraph("Blood Pressure Systolic:").SetBold()).SetWidth(100));
                nestedTable8.AddCell(new Cell().Add(new Paragraph(encounter.BpS.ToString() ?? "")));

                nestedTable9.AddCell(new Cell().Add(new Paragraph("Blood Pressure Diastolic:").SetBold()).SetWidth(100));
                nestedTable9.AddCell(new Cell().Add(new Paragraph(encounter.BpD.ToString() ?? "")));

                nestedTable10.AddCell(new Cell().Add(new Paragraph("O2:").SetBold()).SetWidth(100));
                nestedTable10.AddCell(new Cell().Add(new Paragraph(encounter.O2.ToString() ?? "")));

                nestedTable11.AddCell(new Cell().Add(new Paragraph("Pain:").SetBold()).SetWidth(100));
                nestedTable11.AddCell(new Cell().Add(new Paragraph(encounter.Pain ?? "")));

                nestedTable12.AddCell(new Cell().Add(new Paragraph("Heent:").SetBold()).SetWidth(100));
                nestedTable12.AddCell(new Cell().Add(new Paragraph(encounter.Heent ?? "")));

                nestedTable13.AddCell(new Cell().Add(new Paragraph("CV:").SetBold()).SetWidth(100));
                nestedTable13.AddCell(new Cell().Add(new Paragraph(encounter.Cv ?? "")));

                nestedTable14.AddCell(new Cell().Add(new Paragraph("Chest:").SetBold()).SetWidth(100));
                nestedTable14.AddCell(new Cell().Add(new Paragraph(encounter.Chest ?? "")));

                nestedTable15.AddCell(new Cell().Add(new Paragraph("Abd:").SetBold()).SetWidth(100));
                nestedTable15.AddCell(new Cell().Add(new Paragraph(encounter.Abd ?? "")));

                nestedTable16.AddCell(new Cell().Add(new Paragraph("Extr:").SetBold()).SetWidth(100));
                nestedTable16.AddCell(new Cell().Add(new Paragraph(encounter.Extr ?? "")));

                nestedTable17.AddCell(new Cell().Add(new Paragraph("Skin:").SetBold()).SetWidth(100));
                nestedTable17.AddCell(new Cell().Add(new Paragraph(encounter.Skin ?? "")));

                nestedTable18.AddCell(new Cell().Add(new Paragraph("Neuro:").SetBold()).SetWidth(100));
                nestedTable18.AddCell(new Cell().Add(new Paragraph(encounter.Neuro ?? "")));

                nestedTable19.AddCell(new Cell().Add(new Paragraph("Other:").SetBold()).SetWidth(100));
                nestedTable19.AddCell(new Cell().Add(new Paragraph(encounter.Other ?? "")));

                nestedTable20.AddCell(new Cell().Add(new Paragraph("Diagnosis:").SetBold()).SetWidth(100));
                nestedTable20.AddCell(new Cell().Add(new Paragraph(encounter.Diagnosis ?? "")));

                nestedTable21.AddCell(new Cell().Add(new Paragraph("Treatment:").SetBold()).SetWidth(100));
                nestedTable21.AddCell(new Cell().Add(new Paragraph(encounter.TreatmentPlan ?? "")));

                nestedTable22.AddCell(new Cell().Add(new Paragraph("Dispensed:").SetBold()).SetWidth(100));
                nestedTable22.AddCell(new Cell().Add(new Paragraph(encounter.MedicationDispensed ?? "")));

                nestedTable23.AddCell(new Cell().Add(new Paragraph("Procedures:").SetBold()).SetWidth(100));
                nestedTable23.AddCell(new Cell().Add(new Paragraph(encounter.Procedures ?? "")));

                nestedTable24.AddCell(new Cell().Add(new Paragraph("Followup:").SetBold()).SetWidth(100));
                nestedTable24.AddCell(new Cell().Add(new Paragraph(encounter.FollowUp ?? "")));

                mainTable.AddCell(nestedTable1);
                mainTable.AddCell(nestedTable2);
                mainTable.AddCell(nestedTable3);
                mainTable.AddCell(nestedTable4);
                mainTable.AddCell(nestedTable5);
                mainTable.AddCell(nestedTable6);
                mainTable.AddCell(nestedTable7);
                mainTable.AddCell(nestedTable8);
                mainTable.AddCell(nestedTable9);
                mainTable.AddCell(nestedTable10);
                mainTable.AddCell(nestedTable11);
                mainTable.AddCell(nestedTable12);
                mainTable.AddCell(nestedTable13);
                mainTable.AddCell(nestedTable14);
                mainTable.AddCell(nestedTable15);
                mainTable.AddCell(nestedTable16);
                mainTable.AddCell(nestedTable17);
                mainTable.AddCell(nestedTable18);
                mainTable.AddCell(nestedTable19);
                mainTable.AddCell(nestedTable20);
                mainTable.AddCell(nestedTable21);
                mainTable.AddCell(nestedTable22);
                mainTable.AddCell(nestedTable23);
                mainTable.AddCell(nestedTable24);
                document.Add(mainTable.SetBorder(Border.NO_BORDER).SetPadding(0));

                document.Close();

                return stream.ToArray();
            }
        }
    }
}
