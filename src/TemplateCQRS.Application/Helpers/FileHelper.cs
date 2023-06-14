using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace TemplateCQRS.Application.Helpers;

public class FileHelper
{

    /// <summary>
    ///     Converts the provided IQueryable data into a CSV format and returns it as a FileStreamResult.
    /// </summary>
    /// <param name="query">The IQueryable data to convert into CSV format.</param>
    /// <param name="fileName">Optional filename to be used when the CSV file is downloaded.</param>
    public FileStreamResult ToCsv(IQueryable query, string fileName = null)
    {
        var columns = GetProperties(query.ElementType);

        var sb = new StringBuilder();

        foreach (var item in query)
        {
            sb.AppendLine(string.Join(",", columns.Select(column => $"{GetValue(item, column.Key)}".Trim()).ToArray()));
        }


        var result = new FileStreamResult(new MemoryStream(Encoding.Default.GetBytes($"{string.Join(",", columns.Select(c => c.Key))}{Environment.NewLine}{sb}")), "text/csv")
        {
            FileDownloadName = (!string.IsNullOrEmpty(fileName) ? fileName : "Export") + ".csv"
        };

        return result;
    }

    /// <summary>
    ///     Converts the provided IQueryable data into an Excel format and returns it as a FileStreamResult.
    /// </summary>
    /// <param name="query">The IQueryable data to convert into Excel format.</param>
    /// <param name="fileName">Optional filename to be used when the Excel file is downloaded.</param>
    /// <returns>A FileStreamResult containing the Excel formatted data.</returns>
    public FileStreamResult ToExcel(IQueryable query, string fileName = null)
    {
        var columns = GetProperties(query.ElementType);
        var stream = new MemoryStream();

        using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();

            var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            GenerateWorkbookStylesPartContent(workbookStylesPart);

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());
            var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
            sheets.Append(sheet);

            workbookPart.Workbook.Save();

            var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

            var headerRow = new Row();

            foreach (KeyValuePair<string, Type> column in columns)
            {
                headerRow.Append(new Cell()
                {
                    CellValue = new CellValue(column.Key),
                    DataType = new EnumValue<CellValues>(CellValues.String)
                });
            }

            sheetData.AppendChild(headerRow);

            foreach (var item in query)
            {
                var row = new Row();

                foreach (KeyValuePair<string, Type> column in columns)
                {
                    var value = GetValue(item, column.Key);
                    var stringValue = $"{value}".Trim();

                    var cell = new Cell();

                    var underlyingType = column.Value.IsGenericType &&
                                         column.Value.GetGenericTypeDefinition() == typeof(Nullable<>) ?
                        Nullable.GetUnderlyingType(column.Value) : column.Value;

                    var typeCode = Type.GetTypeCode(underlyingType);

                    if (typeCode == TypeCode.DateTime)
                    {
                        if (!string.IsNullOrWhiteSpace(stringValue))
                        {
                            cell.CellValue = new CellValue() { Text = ((DateTime)value).ToOADate().ToString(System.Globalization.CultureInfo.InvariantCulture) };
                            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                            cell.StyleIndex = (UInt32Value)1U;
                        }
                    }
                    else if (typeCode == TypeCode.Boolean)
                    {
                        cell.CellValue = new CellValue(stringValue.ToLower());
                        cell.DataType = new EnumValue<CellValues>(CellValues.Boolean);
                    }
                    else if (IsNumeric(typeCode))
                    {
                        if (value is null)
                        {
                            stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);
                        }
                        cell.CellValue = new CellValue(stringValue);
                        cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                    }
                    else
                    {
                        cell.CellValue = new CellValue(stringValue);
                        cell.DataType = new EnumValue<CellValues>(CellValues.String);
                    }

                    row.Append(cell);
                }

                sheetData.AppendChild(row);
            }

            workbookPart.Workbook.Save();
        }

        if (stream?.Length > 0)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        var result = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        result.FileDownloadName = (!string.IsNullOrEmpty(fileName) ? fileName : "Export") + ".xlsx";

        return result;
    }

    /// <summary>
    ///     Retrieves the value of a specified property from a given object.
    /// </summary>
    /// <param name="target">The object whose property value to retrieve.</param>
    /// <param name="name">The name of the property to retrieve.</param>
    /// <returns>The value of the specified property, or null if the property does not exist or cannot be accessed.</returns>
    public static object? GetValue(object target, string name) => target.GetType().GetProperty(name)?.GetValue(target);

    /// <summary>
    ///     Retrieves a collection of key-value pairs representing the properties and their corresponding types of a given Type.
    /// </summary>
    /// <param name="type">The Type to extract the properties and their types from.</param>
    /// <returns>An IEnumerable of KeyValuePair where the key is the property name and the value is the property type.</returns>
    public static IEnumerable<KeyValuePair<string, Type>> GetProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && IsSimpleType(p.PropertyType))
            .Select(p => new KeyValuePair<string, Type>(p.Name, p.PropertyType));
    }

    /// <summary>
    ///     Determines if the provided type is a simple type.
    /// A simple type is typically a primitive, a string, a decimal, a DateTime, a TimeSpan, a Guid or any nullable variant of these.
    /// </summary>
    /// <param name="type">The Type to evaluate.</param>
    /// <returns>True if the type is a simple type, otherwise false.</returns>
    public static bool IsSimpleType(Type type)
    {
        var underlyingType = type.IsGenericType &&
                             type.GetGenericTypeDefinition() == typeof(Nullable<>) ?
            Nullable.GetUnderlyingType(type) : type;

        if (underlyingType == typeof(System.Guid))
            return true;

        var typeCode = Type.GetTypeCode(underlyingType);

        switch (typeCode)
        {
            case TypeCode.Boolean:
            case TypeCode.Byte:
            case TypeCode.Char:
            case TypeCode.DateTime:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.SByte:
            case TypeCode.Single:
            case TypeCode.String:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    ///     Determines if the provided TypeCode represents a numeric type.
    /// </summary>
    /// <param name="typeCode">The TypeCode to evaluate.</param>
    /// <returns>True if the TypeCode is numeric, otherwise false.</returns>
    private static bool IsNumeric(TypeCode typeCode)
    {
        return typeCode switch
        {
            TypeCode.Decimal => true,
            TypeCode.Double => true,
            TypeCode.Int16 => true,
            TypeCode.Int32 => true,
            TypeCode.Int64 => true,
            TypeCode.UInt16 => true,
            TypeCode.UInt32 => true,
            TypeCode.UInt64 => true,
            _ => false
        };
    }

    /// <summary>
    ///     Generates the content for a WorkbookStylesPart,
    ///     which contains styles that can be used by the cells in the workbook.
    /// </summary>
    /// <param name="workbookStylesPart1">The WorkbookStylesPart to generate content for.</param>
    private static void GenerateWorkbookStylesPartContent(WorkbookStylesPart workbookStylesPart1)
    {
        var stylesheet1 = new Stylesheet
        {
            MCAttributes = new MarkupCompatibilityAttributes
            {
                Ignorable = "x14ac x16r2 xr"
            }
        };
        stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
        stylesheet1.AddNamespaceDeclaration("x16r2", "http://schemas.microsoft.com/office/spreadsheetml/2015/02/main");
        stylesheet1.AddNamespaceDeclaration("xr", "http://schemas.microsoft.com/office/spreadsheetml/2014/revision");

        var fonts1 = new Fonts
        {
            Count = (UInt32Value)1U,
            KnownFonts = true
        };

        var font1 = new Font();
        var fontSize1 = new FontSize
        {
            Val = 11D
        };
        var color1 = new Color
        {
            Theme = (UInt32Value)1U
        };
        var fontName1 = new FontName
        {
            Val = "Calibri"
        };
        var fontFamilyNumbering1 = new FontFamilyNumbering
        {
            Val = 2
        };
        var fontScheme1 = new FontScheme
        {
            Val = FontSchemeValues.Minor
        };

        font1.Append(fontSize1);
        font1.Append(color1);
        font1.Append(fontName1);
        font1.Append(fontFamilyNumbering1);
        font1.Append(fontScheme1);

        fonts1.Append(font1);

        var fills1 = new Fills
        {
            Count = (UInt32Value)2U
        };

        var fill1 = new Fill();
        var patternFill1 = new PatternFill
        {
            PatternType = PatternValues.None
        };

        fill1.Append(patternFill1);

        var fill2 = new Fill();
        var patternFill2 = new PatternFill
        {
            PatternType = PatternValues.Gray125
        };

        fill2.Append(patternFill2);

        fills1.Append(fill1);
        fills1.Append(fill2);

        var borders1 = new Borders
        {
            Count = (UInt32Value)1U
        };

        var border1 = new Border();
        var leftBorder1 = new LeftBorder();
        var rightBorder1 = new RightBorder();
        var topBorder1 = new TopBorder();
        var bottomBorder1 = new BottomBorder();
        var diagonalBorder1 = new DiagonalBorder();

        border1.Append(leftBorder1);
        border1.Append(rightBorder1);
        border1.Append(topBorder1);
        border1.Append(bottomBorder1);
        border1.Append(diagonalBorder1);

        borders1.Append(border1);

        var cellStyleFormats1 = new CellStyleFormats
        {
            Count = (UInt32Value)1U
        };
        var cellFormat1 = new CellFormat
        {
            NumberFormatId = (UInt32Value)0U,
            FontId = (UInt32Value)0U,
            FillId = (UInt32Value)0U,
            BorderId = (UInt32Value)0U
        };

        cellStyleFormats1.Append(cellFormat1);

        var cellFormats1 = new CellFormats
        {
            Count = (UInt32Value)2U
        };
        var cellFormat2 = new CellFormat
        {
            NumberFormatId = (UInt32Value)0U,
            FontId = (UInt32Value)0U,
            FillId = (UInt32Value)0U,
            BorderId = (UInt32Value)0U,
            FormatId = (UInt32Value)0U
        };
        var cellFormat3 = new CellFormat
        {
            NumberFormatId = (UInt32Value)14U,
            FontId = (UInt32Value)0U,
            FillId = (UInt32Value)0U,
            BorderId = (UInt32Value)0U,
            FormatId = (UInt32Value)0U,
            ApplyNumberFormat = true
        };

        cellFormats1.Append(cellFormat2);
        cellFormats1.Append(cellFormat3);

        var cellStyles1 = new CellStyles
        {
            Count = (UInt32Value)1U
        };
        var cellStyle1 = new CellStyle
        {
            Name = "Normal",
            FormatId = (UInt32Value)0U,
            BuiltinId = (UInt32Value)0U
        };

        cellStyles1.Append(cellStyle1);
        var differentialFormats1 = new DifferentialFormats
        {
            Count = (UInt32Value)0U
        };
        var tableStyles1 = new TableStyles
        {
            Count = (UInt32Value)0U,
            DefaultTableStyle = "TableStyleMedium2",
            DefaultPivotStyle = "PivotStyleLight16"
        };

        var stylesheetExtensionList1 = new StylesheetExtensionList();

        var stylesheetExtension1 = new StylesheetExtension
        {
            Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}"
        };
        stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");

        var stylesheetExtension2 = new StylesheetExtension
        {
            Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}"
        };
        stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");

        var openXmlUnknownElement4 = OpenXmlUnknownElement.CreateOpenXmlUnknownElement("<x15:timelineStyles defaultTimelineStyle=\"TimeSlicerStyleLight1\" xmlns:x15=\"http://schemas.microsoft.com/office/spreadsheetml/2010/11/main\" />");

        stylesheetExtension2.Append(openXmlUnknownElement4);

        stylesheetExtensionList1.Append(stylesheetExtension1);
        stylesheetExtensionList1.Append(stylesheetExtension2);

        stylesheet1.Append(fonts1);
        stylesheet1.Append(fills1);
        stylesheet1.Append(borders1);
        stylesheet1.Append(cellStyleFormats1);
        stylesheet1.Append(cellFormats1);
        stylesheet1.Append(cellStyles1);
        stylesheet1.Append(differentialFormats1);
        stylesheet1.Append(tableStyles1);
        stylesheet1.Append(stylesheetExtensionList1);

        workbookStylesPart1.Stylesheet = stylesheet1;
    }
}