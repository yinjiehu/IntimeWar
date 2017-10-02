using System;
using System.Collections.Generic;
using System.Xml;

namespace Kit.Utility.OpenXml
{
    public class ExcelRow : List<ExcelCell>
	{
		public ExcelSheet excelSheet{ private set; get; }
		int _rowIndex;
		public int RowCount { get { return this._rowIndex + 1; } }

		Dictionary<string, ExcelCell> _dataMap = new Dictionary<string, ExcelCell>();
		public Dictionary<string, ExcelCell> CellsWithHeader { get; private set; }

		public ExcelRow(XmlNode xmlNode, ExcelSheet sheet, int row)
		{
            this.CellsWithHeader = new Dictionary<string, ExcelCell>();

            this.excelSheet = sheet;
			this._rowIndex = row;

			var list = xmlNode.SelectNodes("ns:Cell", this.excelSheet.openXmlReader.nsMgr);

            int column = 0;
			foreach (XmlNode n in list)
			{
				var v = n.InnerText.Trim();
                if (!string.IsNullOrEmpty(v))
                {
                    var cell = new ExcelCell(n.InnerText.Trim(), row, column++);
                    this.Add(cell);
                }
			}
		}

		public void SetHeader(List<string> headers)
		{
			if (this.Count == 0)
			{
				return;
			}

			if (this.Count != 0 && this.Count != headers.Count)
			{
                //Debug.LogErrorFormat("Row {0} has different cell count({1}) compared to header row({2})", this._rowIndex + 1, Count, headers.Count);
            } else
			{
                this._dataMap.Clear();
                this.CellsWithHeader.Clear();

                for (int i = 0; i < this.Count; i++)
				{
					var header = headers[i];
                    if (this._dataMap.ContainsKey(header))
                    {
                        throw new Exception("header " + header + " is already exist!");
                    } else
                    {
                        var cell = this[i];
                        cell.Header = header;
                        this._dataMap.Add(header, cell);
                        this.CellsWithHeader.Add(header, cell);
                    }
				}
			}
		}

		public IEnumerable<ParseError> ParseToObject(Type objectType, ref object obj, ParseSettings settings = null)
		{
            if (this.Count == 0)
            {
                return new ParseError[0];
            }

			var parser = new ObjectParser(this, settings);
			parser.ParseToObject(objectType, ref obj);
			return parser.Errors;
		}

        public IEnumerable<ParseError> ParseToObject<T>(out T t, ParseSettings settings = null)
        {
            if (this.Count == 0)
            {
                t = default(T);
                return new ParseError[0];
            }

            object obj = Activator.CreateInstance<T>();
            var parser = new ObjectParser(this, settings);
            parser.ParseToObject(typeof(T), ref obj);
            t = (T)obj;
            return parser.Errors;
        }
    }	
}