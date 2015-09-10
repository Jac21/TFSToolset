using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Office.Interop.Excel;

namespace TFSToolset
{
    public class ExcelHelperFunctions
    {
        // class fields
        private Application _xlApp = new Application();
        public Worksheet[] _worksheetCollection;
        public Workbook _workbook;

        /// <summary>
        /// Constructor that intakes Excel application object
        /// and instantiates the associated workbook
        /// </summary>
        /// <param name="xlApp"></param>
        public ExcelHelperFunctions(Application xlApp)
        {
            // workbook instantiation
            Workbook workbook = _xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            this._xlApp = xlApp;
            this._workbook = workbook;

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
            System.Threading.Thread.CurrentThread.CurrentCulture =
                new System.Globalization.CultureInfo("en-US");

            // make a collection of worksheets, name them appropriately 
            var worksheetCollections = new Worksheet[sheetNames.Length];
            for (var i = sheetNames.Length - 1; i >= 0; --i)
            {
                worksheetCollections[i] = this._workbook.Worksheets.Add();
                worksheetCollections[i].Name = sheetNames[i];
            }

            Worksheet ws = (Worksheet)_workbook.Worksheets[1];

            // tie object for later use in main class
            this._worksheetCollection = worksheetCollections;
        }

        // method stub
        public void AddWorksheetObjects()
        {
            //
        }
    }
}
