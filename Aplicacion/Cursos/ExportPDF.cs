using System.IO;
using System.Threading;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ExportPDF
    {
        public class Consulta : IRequest<Stream>
        {

        }

        public class Handler : IRequestHandler<Consulta, Stream>
        {
            private readonly CursosOnlineContext _context;

            public Handler(CursosOnlineContext context)
            {
                this._context = context;
            }
            public async Task<Stream> Handle(Consulta request, CancellationToken cancellationToken)
            {
                Font fuenteTitulo = new Font(Font.HELVETICA, 14f, Font.BOLD, BaseColor.Black);
                Font fuenteNombreEmpresa = new Font(Font.HELVETICA, 16f, Font.BOLD, BaseColor.Black);
                Font fuenteHeader = new Font(Font.HELVETICA, 9f, Font.BOLD, BaseColor.Black);
                Font fuenteDescripcion = new Font(Font.HELVETICA, 10f, Font.NORMAL, BaseColor.Black);
                Font fuenteDescripcionVenta = new Font(Font.HELVETICA, 10f, Font.BOLD, BaseColor.Black);
                Font fuenteData = new Font(Font.HELVETICA, 10f, Font.NORMAL, BaseColor.Black);

                var cursos = await _context.Curso
                    .Include(x => x.InstructorLink).ThenInclude(x => x.Instructor)
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                    .ToListAsync();

                MemoryStream workStream = new MemoryStream();
                Rectangle rect = new Rectangle(PageSize.A4);

                Document document = new Document(rect, 0, 0, 50, 100);

                PdfWriter writer = PdfWriter.GetInstance(document, workStream);

                writer.CloseStream = false;

                document.Open();
                document.AddTitle("Boleta de Ventas");

                PdfPTable tablaEspacio = new PdfPTable(1);
                tablaEspacio.WidthPercentage = 20;

                // ESpacio para el documento
                PdfPCell celdaEspacio = new PdfPCell(new Phrase("\n", fuenteTitulo));
                celdaEspacio.Border = Rectangle.NO_BORDER;

                tablaEspacio.AddCell(celdaEspacio);
                document.Add(tablaEspacio);


                // Descripcion de la organizacion
                PdfPTable tablaDescripcionEmpresa = new PdfPTable(2);
                float[] widthstablaDescripcionEmpresa = new float[] { 60f, 40f };
                tablaDescripcionEmpresa.SetWidthPercentage(widthstablaDescripcionEmpresa, rect);

                //Descripcion Empresa
                Phrase phraseDescripcionEmpresa = new Phrase();
                phraseDescripcionEmpresa.Add(new Phrase("S.A.C EMPRESA\n\n", fuenteNombreEmpresa));
                phraseDescripcionEmpresa.Add(new Phrase("Ubicacion\n\n", fuenteDescripcion));
                phraseDescripcionEmpresa.Add(new Phrase("Telefono\n\n", fuenteDescripcion));

                PdfPCell celdaDescripcionEmpresa = new PdfPCell(phraseDescripcionEmpresa);
                celdaDescripcionEmpresa.Border = Rectangle.NO_BORDER;
                tablaDescripcionEmpresa.AddCell(celdaDescripcionEmpresa);


                //Descripcion Boletea
                Phrase phraseDescripcionBoleta = new Phrase();
                phraseDescripcionBoleta.Add(new Phrase("BOLETA DE VENTA ELECTRONICA\n\n", fuenteNombreEmpresa));
                phraseDescripcionBoleta.Add(new Phrase("RUC : 12345678910\n\n", fuenteDescripcion));
                phraseDescripcionBoleta.Add(new Phrase("Nro: 123124515\n\n", fuenteDescripcion));

                PdfPCell celdaDescripcionBoleta = new PdfPCell(phraseDescripcionBoleta);
                tablaDescripcionEmpresa.AddCell(celdaDescripcionBoleta);

                tablaDescripcionEmpresa.WidthPercentage = 90;
                document.Add(tablaDescripcionEmpresa);

                document.Add(tablaEspacio);

                // Descripcion de la venta
                PdfPTable tablaDescripcionVenta = new PdfPTable(4);
                float[] widthstablaDescripcionVenta = new float[] { 10f, 50f, 20f, 20f };
                tablaDescripcionVenta.SetWidthPercentage(widthstablaDescripcionVenta, rect);

                // Primera Celda
                PdfPCell primeraCelda_column1 = new PdfPCell(new Phrase("Cliente", fuenteDescripcionVenta));
                // primeraCelda_column1.Border = Rectangle.NO_BORDER;
                tablaDescripcionVenta.AddCell(primeraCelda_column1);

                PdfPCell primeraCelda_column2 = new PdfPCell(new Phrase(":", fuenteDescripcionVenta));
                // primeraCelda_column2.Border = Rectangle.NO_BORDER;
                tablaDescripcionVenta.AddCell(primeraCelda_column2);

                PdfPCell primeraCelda_column3 = new PdfPCell(new Phrase("Fecha Emisión", fuenteDescripcionVenta));
                // primeraCelda_column3.Border = Rectangle.NO_BORDER;
                tablaDescripcionVenta.AddCell(primeraCelda_column3);

                PdfPCell primeracelda_column4 = new PdfPCell(new Phrase(":", fuenteDescripcionVenta));
                // primeracelda_column4.Border = Rectangle.NO_BORDER;
                tablaDescripcionVenta.AddCell(primeracelda_column4);

                // Segunda Celda

                PdfPCell segundaCelda_column1 = new PdfPCell(new Phrase("DNI", fuenteDescripcionVenta));
                // segundaCelda_column1.Border = Rectangle.NO_BORDER;
                tablaDescripcionVenta.AddCell(segundaCelda_column1);

                PdfPCell segundaCelda_column2 = new PdfPCell(new Phrase(":", fuenteDescripcionVenta));
                // segundaCelda_column2.Border = Rectangle.NO_BORDER;
                tablaDescripcionVenta.AddCell(segundaCelda_column2);

                PdfPCell segundaCelda_column3 = new PdfPCell(new Phrase("Moneda", fuenteDescripcionVenta));
                // segundaCelda_column3.Border = Rectangle.NO_BORDER;
                tablaDescripcionVenta.AddCell(segundaCelda_column3);

                PdfPCell segundaCelda_column4 = new PdfPCell(new Phrase(":", fuenteDescripcionVenta));
                // segundaCelda_column4.Border = Rectangle.NO_BORDER;
                tablaDescripcionVenta.AddCell(segundaCelda_column4);

                // Tercera Celda

                PdfPCell terceraCelda_column1 = new PdfPCell(new Phrase("Direccion", fuenteDescripcionVenta));
                // terceraCelda_column1.Border = Rectangle.NO_BORDER;
                tablaDescripcionVenta.AddCell(terceraCelda_column1);

                PdfPCell terceraCelda_column2 = new PdfPCell(new Phrase(":", fuenteDescripcionVenta));
                // terceraCelda_column2.Border = Rectangle.NO_BORDER;
                tablaDescripcionVenta.AddCell(terceraCelda_column2);

                PdfPCell terceraCelda_column3 = new PdfPCell(new Phrase("Observaciones", fuenteDescripcionVenta));
                // terceraCelda_column3.Border = Rectangle.NO_BORDER;
                tablaDescripcionVenta.AddCell(terceraCelda_column3);

                PdfPCell terceraCelda_column4 = new PdfPCell(new Phrase(":", fuenteDescripcionVenta));
                // terceraCelda_column4.Border = Rectangle.NO_BORDER;
                tablaDescripcionVenta.AddCell(terceraCelda_column4);

                tablaDescripcionVenta.WidthPercentage = 90;
                document.Add(tablaDescripcionVenta);

                document.Add(tablaEspacio);



                PdfPTable tablaCurso = new PdfPTable(6);
                float[] widths = new float[] { 5f, 45f, 10f, 10f, 15f, 15f };
                tablaCurso.SetWidthPercentage(widths, rect);

                PdfPCell celdaHeaderNumero = new PdfPCell(new Phrase("ITEM", fuenteHeader));
                tablaCurso.AddCell(celdaHeaderNumero);

                PdfPCell celdaHeaderDescripcion = new PdfPCell(new Phrase("DESCRIPCIÓN", fuenteHeader));
                tablaCurso.AddCell(celdaHeaderDescripcion);

                PdfPCell celdaHeaderUnid = new PdfPCell(new Phrase("UNID.", fuenteHeader));
                tablaCurso.AddCell(celdaHeaderUnid);

                PdfPCell celdaHeaderCantidad = new PdfPCell(new Phrase("CANTIDAD", fuenteHeader));
                tablaCurso.AddCell(celdaHeaderCantidad);

                PdfPCell celdaHeaderPrecioUnitario = new PdfPCell(new Phrase("P. UNITARIO", fuenteHeader));
                tablaCurso.AddCell(celdaHeaderPrecioUnitario);

                PdfPCell celdaHeaderTotal = new PdfPCell(new Phrase("TOTAL", fuenteHeader));
                tablaCurso.AddCell(celdaHeaderTotal);

                tablaCurso.WidthPercentage = 90;

                decimal numero = 0;
                decimal total = 0;
                decimal precio = 0;

                foreach (var cursoElmento in cursos)
                {
                    numero += 1;
                    PdfPCell celdaDataNumero = new PdfPCell(new Phrase(numero.ToString(), fuenteData));
                    tablaCurso.AddCell(celdaDataNumero);

                    PdfPCell celdaDataDescripcion = new PdfPCell(new Phrase(cursoElmento.Descripcion, fuenteData));
                    tablaCurso.AddCell(celdaDataDescripcion);

                    PdfPCell celdaDataUnidad = new PdfPCell(new Phrase("UNID", fuenteData));
                    tablaCurso.AddCell(celdaDataUnidad);

                    PdfPCell celdaDataCantidad = new PdfPCell(new Phrase(numero.ToString(), fuenteData));
                    tablaCurso.AddCell(celdaDataCantidad);

                    precio = decimal.Round(cursoElmento.PrecioPromocion.PrecioActual,2);

                    PdfPCell celdaDataPrecioUnitario = new PdfPCell(new Phrase(precio.ToString(), fuenteData));
                    tablaCurso.AddCell(celdaDataPrecioUnitario);

                    total = decimal.Round(cursoElmento.PrecioPromocion.PrecioActual * numero, 2);

                    PdfPCell celdaDataTotal = new PdfPCell(new Phrase(total.ToString(), fuenteData));
                    tablaCurso.AddCell(celdaDataTotal);

                }


                document.Add(tablaCurso);

                document.Add(tablaEspacio);

                // Tabla importe total
                PdfPTable tablaImporteTotal = new PdfPTable(3);
                float[] widthsImporteTotal  = new float[] {40f, 40f, 20f};
                tablaImporteTotal.SetWidthPercentage(widthsImporteTotal, rect);

                // Primera celda
                PdfPCell celdaImporteTotalEspacio = new PdfPCell(new Phrase("", fuenteHeader));
                celdaImporteTotalEspacio.Border = Rectangle.NO_BORDER;
                tablaImporteTotal.AddCell(celdaImporteTotalEspacio);

                PdfPCell celdaSubTotal = new PdfPCell(new Phrase("Total Valor Venta - Operaciones Grabadas", fuenteHeader));
                tablaImporteTotal.AddCell(celdaSubTotal);

                PdfPCell celdaDataSubTotal = new PdfPCell(new Phrase("", fuenteData));
                tablaImporteTotal.AddCell(celdaDataSubTotal);

                // Segunda celda
                tablaImporteTotal.AddCell(celdaImporteTotalEspacio);
                
                PdfPCell celdaIGV = new PdfPCell(new Phrase("IGV", fuenteHeader));
                tablaImporteTotal.AddCell(celdaIGV);

                PdfPCell celdaDataIGV = new PdfPCell(new Phrase("", fuenteData));
                tablaImporteTotal.AddCell(celdaDataIGV);

                // Tercera celda
                tablaImporteTotal.AddCell(celdaImporteTotalEspacio);

                PdfPCell celdaDescuento = new PdfPCell(new Phrase("Descuento", fuenteHeader));
                tablaImporteTotal.AddCell(celdaDescuento);

                PdfPCell celdaDataDescuento = new PdfPCell(new Phrase("", fuenteData));
                tablaImporteTotal.AddCell(celdaDataDescuento);

                // Cuarta celda
                tablaImporteTotal.AddCell(celdaImporteTotalEspacio);

                PdfPCell celdaTotalImporte = new PdfPCell(new Phrase("Importe Total", fuenteHeader));
                tablaImporteTotal.AddCell(celdaTotalImporte);

                PdfPCell celdaDataImporte = new PdfPCell(new Phrase("", fuenteData));
                tablaImporteTotal.AddCell(celdaDataImporte);

                tablaImporteTotal.WidthPercentage = 90;
                document.Add(tablaImporteTotal);


                
                
                document.Close();

                byte[] byteData = workStream.ToArray();

                workStream.Write(byteData, 0, byteData.Length);
                workStream.Position = 0;

                return workStream;
            }
        }
    }
}