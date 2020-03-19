using System;
using System.Drawing;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Drawing.Printing;
using System.Windows.Media;
using Microsoft.Owin;
using Owin;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Routing;

namespace ConsoleApp1
{
    
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();  
            app.UseWebApi(configuration);
            app.Run(async owincontext => {
                owincontext.Response.ContentType = "text/plain";
                await owincontext.Response.WriteAsync("Api is availble at:  /api/print"); 
            });
            //app.Run(sample =>
            //{
            //   SaleModel saleModel = new SaleModel();
            //   saleModel.SaleList = new List<SaleItem>();
            //   Debug.Write("Request Received \n");
            //   List<KeyValuePair<string,string[]>> b=  sample.Request.Query.ToList();
            //   string param = "";
            //   foreach (var item in b)
            //    {
            //        param +=  item.Key+" : " ;
            //        foreach (var item1 in item.Value)
            //        {
            //            param += item1+",";
            //        }
            //        param += "\n";
            //    }
            //    Console.WriteLine(sample.Request.QueryString);
            //    Console.WriteLine(param);
            //    foreach (KeyValuePair<string, string[]> item in b)
            //    {
            //        if (item.Key=="SaleId")
            //        {
            //            foreach (string s in item.Value)
            //            {
            //                saleModel.SaleId = s;
            //            }
            //        }
            //        if (item.Key == "Change")
            //        {
            //            foreach (string s in item.Value)
            //            {
            //                saleModel.Change = s;
            //            }
            //        }
            //        if (item.Key == "DiscountedBill")
            //        {
            //            foreach (string s in item.Value)
            //            {
            //                saleModel.DiscountedBill = s;
            //            }
            //        }
            //        if (item.Key == "Font")
            //        {
            //            foreach (string s in item.Value)
            //            {
            //                saleModel.Font = s;
            //            }
            //        }
            //        if (item.Key == "SaleList")
            //        {
            //            foreach (string s in item.Value)
            //            {
            //                try {
            //                    string[] ItemsList = s.Split('*');
            //                    foreach (string t1 in ItemsList)
            //                    {
            //                        SaleItem saleItem = new SaleItem();
            //                        var t = t1.Split('@');
            //                        try
            //                        {
            //                            saleItem.name = t[0];
            //                            saleItem.price = t[1];
            //                            saleItem.quantity = t[2];
            //                            saleItem.total = t[3];
            //                        }
            //                        catch
            //                        {
            //                            Console.WriteLine("Parameter of items not completed");
            //                        }
                                    
            //                        saleModel.SaleList.Add(saleItem);
            //                    }
            //                } catch(Exception ex)
            //                {
            //                    Console.Write("Exception in parsing sale list items:"+ ex);
            //                }
                            
            //            }
            //        }
            //    }
            //    if (saleModel.SaleId != null)
            //    {
            //        new PrintingUtils().printSaleReceipt(saleModel);
            //    }
            //    else {
            //        Console.WriteLine("Sale Id is null, returning back");
            //    }
            //    sample.Response.ContentType = "text/plain";
            //    return sample.Response.WriteAsync("Printer Name: \n"+new PrinterSettings().PrinterName+"\n Params: \n"+param); 
            //});
        }
    }
    public class SaleModel
    {
        public string Font { get; set; } // to be used in printing
        public string SaleId { get; set; } // to be used in printing
        public string DiscountedBill { get; set; }
        public string Change { get; set; }
        public List<SaleItem> SaleList { get; set; }
    }
    public class SaleItem
    {
        public string name { get; set; }
        public string price { get; set; }
        public string quantity { get; set; }
        public string total { get; set; }
    }
    public class PrintingUtils
    {
        public void printSaleReceipt(SaleModel saleModel)
        {
            PrintDialog pd = new PrintDialog();
            var doc = ((IDocumentPaginatorSource)getFlowDocument(saleModel)).DocumentPaginator;
            pd.PrintQueue = new PrintQueue(new PrintServer(), new PrinterSettings().PrinterName);
            pd.PrintDocument(doc, "Print Document");
        }
         FlowDocument getFlowDocument(SaleModel saleModel)
        {
            FlowDocument fd = new FlowDocument();
            fd.PageWidth = 280;
            if (saleModel.Font != null)
            {
                fd.FontFamily = new System.Windows.Media.FontFamily(saleModel.Font);
            }
            else
            {
                fd.FontFamily = new System.Windows.Media.FontFamily("Arial");
            }

            fd.PagePadding = new Thickness(0, 0, 0, 0);
            fd.TextAlignment = TextAlignment.Center;
            Section header = new Section();
            Paragraph header1 = new Paragraph(new Bold(new Run("Usman General Store")));
            Paragraph header2 = new Paragraph(new Run("Noori Darbar Road, Muslim Abad,Pull Fateh Garh, Lahore. 0300-4396491"));


            string date = DateTime.Now.ToLongDateString() + " "+DateTime.Now.ToShortTimeString();
            Paragraph header3 = new Paragraph(new Run("Sales Id: " + saleModel.SaleId));
            Paragraph header4 = new Paragraph(new Run("Date: " + date));
            Paragraph header5 = new Paragraph(new Run("______________________________________"));
            header1.FontSize = 17;// old was 14
            header2.FontSize = 14;// old was 10
            header3.FontSize = 12;// old was 9
            header4.FontSize = 12;
            header5.FontSize = 10;
            header.Blocks.Add(header1);
            header.Blocks.Add(header2);
            header.Blocks.Add(header3);
            header.Blocks.Add(header4);
            header.Blocks.Add(header5);


            Section middle = new Section();
            middle.FontSize = 11; // old size was 9
            Table table = new Table();
            table.TextAlignment = TextAlignment.Left;
            TableColumn tb1 = new TableColumn();
            tb1.Width = new GridLength(140);
            TableColumn tb2 = new TableColumn();
            TableColumn tb3 = new TableColumn();
            TableColumn tb4 = new TableColumn();
            table.Columns.Add(tb1);
            table.Columns.Add(tb2);
            table.Columns.Add(tb3);
            table.Columns.Add(tb4);
            table.RowGroups.Add(new TableRowGroup());
            TableRow trHeader = new TableRow();
            table.RowGroups[0].Rows.Add(trHeader);
            trHeader.Cells.Add(new TableCell(new Paragraph(new Run("Name"))));
            trHeader.Cells.Add(new TableCell(new Paragraph(new Run("Rs"))));
            trHeader.Cells.Add(new TableCell(new Paragraph(new Run("Qty"))));
            trHeader.Cells.Add(new TableCell(new Paragraph(new Run("Ttl"))));

            foreach (SaleItem item in saleModel.SaleList)
            {
                TableRow tr = new TableRow();
                table.RowGroups[0].Rows.Add(tr);
                //tr.Cells.Add(new TableCell(new Paragraph(new Run("Item 1"))));
                //tr.Cells.Add(new TableCell(new Paragraph(new Run("100"))));
                //tr.Cells.Add(new TableCell(new Paragraph(new Run("100"))));
                //tr.Cells.Add(new TableCell(new Paragraph(new Run("1000"))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run(item.name))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run(item.price))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run(item.quantity))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run(item.total))));
            }
            middle.Blocks.Add(table);
            double Payment = Convert.ToDouble(saleModel.DiscountedBill) + Convert.ToDouble(saleModel.Change);
            middle.Blocks.Add(new Paragraph(new Run("______________________________________")));
            middle.Blocks.Add(new Paragraph(new Bold(new Run("           Total Items                      " + saleModel.SaleList.Count))));
            middle.Blocks.Add(new Paragraph(new Bold(new Run("           Bill                                   " + saleModel.DiscountedBill))));
            middle.Blocks.Add(new Paragraph(new Bold(new Run("           Payment                          " + Payment))));
            middle.Blocks.Add(new Paragraph(new Bold(new Run("           Balance                            " + saleModel.Change))));
            middle.TextAlignment = TextAlignment.Left;

            Section footer = new Section();
            Paragraph footer1 = new Paragraph(new Run("Thankyou for Purchasing."));
            Paragraph footer2 = new Paragraph(new Run("Software Developed By QuickLinqs."));
            Paragraph footer3 = new Paragraph(new Bold(new Run("QuickLinqs Phone: 03024759550  www.quicklinqs.com")));
            Paragraph footer4 = new Paragraph(new Run("                "));
            footer1.FontSize = 14;
            footer2.FontSize = 10;
            footer3.FontSize = 9;
            footer.Blocks.Add(footer1);
            footer.Blocks.Add(footer2);
            footer.Blocks.Add(footer3);
            footer.Blocks.Add(footer4);


            fd.Blocks.Add(header);
            fd.Blocks.Add(middle);
            fd.Blocks.Add(footer);
            return fd;
        }
    }
    /// <summary>
    /// Owin Request extensions.
    /// </summary>
    public static class OwinRequestExtensions
    {
        /// <summary>
        /// Gets the combined request parameters from the form body, query string, and request headers.
        /// </summary>
        /// <param name="request">Owin request.</param>
        /// <returns>Dictionary of combined form body, query string, and request headers.</returns>
        public static Dictionary<string, string> GetRequestParameters(this IOwinRequest request)
        {
            var bodyParameters = request.GetBodyParameters();

            var queryParameters = request.GetQueryParameters();

            var headerParameters = request.GetHeaderParameters();

            bodyParameters.Merge(queryParameters);

            bodyParameters.Merge(headerParameters);

            return bodyParameters;
        }

        /// <summary>
        /// Gets the query string request parameters.
        /// </summary>
        /// <param name="request">Owin Request.</param>
        /// <returns>Dictionary of query string parameters.</returns>
        public static Dictionary<string, string> GetQueryParameters(this IOwinRequest request)
        {
            var dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            foreach (var pair in request.Query)
            {
                var value = GetJoinedValue(pair.Value);

                dictionary.Add(pair.Key, value);
            }

            return dictionary;
        }

        /// <summary>
        /// Gets the form body request parameters.
        /// </summary>
        /// <param name="request">Owin Request.</param>
        /// <returns>Dictionary of form body parameters.</returns>
        public static Dictionary<string, string> GetBodyParameters(this IOwinRequest request)
        {
            var dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            var formCollectionTask = request.ReadFormAsync();

            formCollectionTask.Wait();

            foreach (var pair in formCollectionTask.Result)
            {
                var value = GetJoinedValue(pair.Value);

                dictionary.Add(pair.Key, value);
            }

            return dictionary;
        }

        /// <summary>
        /// Gets the header request parameters.
        /// </summary>
        /// <param name="request">Owin Request.</param>
        /// <returns>Dictionary of header parameters.</returns>
        public static Dictionary<string, string> GetHeaderParameters(this IOwinRequest request)
        {
            var dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            foreach (var pair in request.Headers)
            {
                var value = GetJoinedValue(pair.Value);

                dictionary.Add(pair.Key, value);
            }

            return dictionary;
        }

        private static string GetJoinedValue(string[] value)
        {
            if (value != null)
                return string.Join(",", value);

            return null;
        }
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> to, IDictionary<TKey, TValue> data)
        {
            foreach (var item in data)
            {
                if (to.ContainsKey(item.Key) == false)
                {
                    to.Add(item.Key, item.Value);
                }
            }
        }
    }
}
