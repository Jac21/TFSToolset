using System;
using System.Globalization;
using System.Threading;
using Microsoft.Office.Interop.Excel;

namespace TFSToolset.UI.Views.Helpers
{
    public class ExcelHelperFunctions
    {
        // class fields
        private Application _xlApp = new Application();
        public Worksheet[] WorksheetCollection;
        public Workbook Workbook;

        /// <summary>
        /// Constructor that intakes Excel application object
        /// and instantiates the associated workbook
        /// </summary>
        /// <param name="xlApp"></param>
        public ExcelHelperFunctions(Application xlApp)
        {
            // workbook instantiation
            Workbook workbook = _xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            _xlApp = xlApp;
            Workbook = workbook;

            // sanity checks
            if (xlApp == null)
            {
                Console.WriteLine("EXCEL could not be started, please check" +
                                  " that it is installed in your computer.");
            }

            if (xlApp != null)
            {
                xlApp.Visible = true;
            }
        }

        /// <summary>
        /// Method to instantiate worksheets needed using supplied
        /// worksheet names
        /// </summary>
        /// <param name="sheetNames"></param>
        public void CreateWorksheets(string[] sheetNames)
        {
            // specify culture to avoid exceptions regarding adding workbooks
            // on non-english machines
            Thread.CurrentThread.CurrentCulture =
                new CultureInfo("en-US");

            // make a collection of worksheets, name them appropriately 
            var worksheetCollections = new Worksheet[sheetNames.Length];
            for (var i = sheetNames.Length - 1; i >= 0; --i)
            {
                worksheetCollections[i] = Workbook.Worksheets.Add();
                worksheetCollections[i].Name = sheetNames[i];
            }

            Worksheet ws = (Worksheet)Workbook.Worksheets[1];

            // tie object for later use in main class
            WorksheetCollection = worksheetCollections;
        }

        // method stub
        public void AddWorksheetObjects()
        {
            //
        }
    }
}
