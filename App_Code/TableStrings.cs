using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TableStrings
/// </summary>
public class TableStrings
{
    int headerFontSize;
    public bool isPivotIview;

    /// <summary>
    /// Mark up tags for an word table.
    /// </summary>
    /// <returns></returns>
    public string GetTableStartString()
    {
        return " 	<w:tbl> "
                                   + "             <w:tblPr> "
                                   + "                 <w:tblW w:w=\"0\" w:type=\"auto\"/> "
                                   + "                 <w:tblBorders> "
                                   +
                                   "                     <w:top w:val=\"single\" w:sz=\"4\" wx:bdrwidth=\"10\" w:space=\"0\" w:color=\"000000\"/> "
                                   +
                                   "                     <w:left w:val=\"single\" w:sz=\"4\" wx:bdrwidth=\"10\" w:space=\"0\" w:color=\"000000\"/> "
                                   +
                                   "                     <w:bottom w:val=\"single\" w:sz=\"4\" wx:bdrwidth=\"10\" w:space=\"0\" w:color=\"000000\"/> "
                                   +
                                   "                     <w:right w:val=\"single\" w:sz=\"4\" wx:bdrwidth=\"10\" w:space=\"0\" w:color=\"000000\"/> "
                                   +
                                   "                     <w:insideH w:val=\"single\" w:sz=\"4\" wx:bdrwidth=\"10\" w:space=\"0\" w:color=\"000000\"/> "
                                   +
                                   "                     <w:insideV w:val=\"single\" w:sz=\"4\" wx:bdrwidth=\"10\" w:space=\"0\" w:color=\"000000\"/> "
                                   + "                 </w:tblBorders> "
                                   + "                 <w:tblLook w:val=\"00BF\"/> "
                                   + "             </w:tblPr> "
                                   + "             <w:tblGrid> "
                                   + "                 <w:gridCol w:w=\"4258\"/> "
                                   + "                 <w:gridCol w:w=\"4258\"/> "
                                   + "             </w:tblGrid> ";


    }
    /// <summary>
    /// Ending Tag of Table..
    /// </summary>
    /// <returns></returns>
    public string GetTableEndString()
    {
        return " 	</w:tbl>";
    }
    #region Header MarkUps.

    /// <summary>
    /// Creates a row where Cell/Table Defination for each row to go under this.
    /// </summary>
    /// <returns></returns>
    public string GetHeaderRowStartString()
    {
        return "<w:tr wsp:rsidR=\"00505659\" wsp:rsidRPr=\"004374EC\" wsp:rsidTr=\"004374EC\">";
    }

    /// <summary>
    /// (Header)THe Actual Mark up for creating a cell/Table defination for the table Headers
    /// </summary>
    /// <returns>Mark ups for a table header with the given string value.</returns> 
    public string GetHeaderString(string value, bool isNormal)
    {
        if (value.Contains('~'))
            value = value.Replace("~", " ");
        string removeButtomMargin = (isPivotIview && isNormal == false) ? "<w:tcBorders><w:top w:val=\"nil\"/></w:tcBorders>" : "";



        string markUp = " <w:trPr>"
        + "<w:tblHeader />"
            + "</w:trPr>"
            + "                 <w:tc> "
                                  + "                     <w:tcPr> "
                                  + "                         <w:tcW w:w=\"0\" w:type=\"auto\"/> "
                                  + removeButtomMargin
                                 + "<w:shd w:val=\"clear\" w:color=\"auto\" w:fill=\"E0E0E0\"/> "
                                  + "                     </w:tcPr> "
                                  +
                                  "                     <w:p wsp:rsidR=\"00505659\" wsp:rsidRPr=\"004374EC\" wsp:rsidRDefault=\"00505659\"> <w:rPr><w:rFonts w:ascii=\"Calibri\" w:h-ansi=\"Calibri\" w:cs=\"Calibri\"/><wx:font wx:val=\"Calibri\"/></w:rPr> "
                                  + "                         <w:pPr>  <w:jc w:val=\"center\" />"
                                            + "              <w:rPr>"
                                       + "                 <w:sz w:val=\"" + headerFontSize + "\"/>"
                                         + "               <w:sz-cs w:val=\"" + headerFontSize + "\"/>"
                                      + "              </w:rPr>"

                                  + "                             <w:rPr> "
                                  + "                                 <w:b/> "
                                  + "                             </w:rPr> "
                                  + "                         </w:pPr> "
                                  + "                         <w:r wsp:rsidRPr=\"004374EC\"> "
                                 + "                <w:rPr>"
                                  + "              <w:sz w:val=\"" + headerFontSize + "\"/>"
                                    + "            <w:sz-cs w:val=\"" + headerFontSize + "\"/>"
                                  + "          </w:rPr>"
                                  + "                             <w:rPr> "
                                  + "                                 <w:b/> "
                                  + "                             </w:rPr> "
                                  + "                             <w:t>" + value + "</w:t> "
                                  + "                         </w:r> "
                                  + "                     </w:p> "
                                  + "                 </w:tc> ";

        return markUp;
    }

    public string GetMergeHeaderString(string value, bool isMerge, bool removeButtomMargin, int mergeValue)
    {
        if (value.Contains('~'))
            value = value.Replace("~", " ");

        string mergeString = (isMerge == true) ? " <w:gridSpan w:val=\"" + mergeValue + "\"/> " : "";

        string removeMargin = (removeButtomMargin == true) ? " <w:tcBorders><w:bottom w:val=\"nil\"/></w:tcBorders>" : "";

        string markUp = " <w:trPr>"
       + "<w:tblHeader />"
           + "</w:trPr>"
           + "                 <w:tc> "
                                 + "                     <w:tcPr> "
                                 + "                         <w:tcW w:w=\"0\" w:type=\"auto\"/> "
                                 + mergeString + removeMargin
                                + "                         <w:shd w:val=\"clear\" w:color=\"auto\" w:fill=\"E0E0E0\"/> "
                                 + "                     </w:tcPr> "
                                 +
                                 "                     <w:p wsp:rsidR=\"00505659\" wsp:rsidRPr=\"004374EC\" wsp:rsidRDefault=\"00505659\"> <w:rPr><w:rFonts w:ascii=\"Calibri\" w:h-ansi=\"Calibri\" w:cs=\"Calibri\"/><wx:font wx:val=\"Calibri\"/></w:rPr> "
                                 + "                         <w:pPr>  <w:jc w:val=\"center\" />"
                                           + "              <w:rPr>"
                                      + "                 <w:sz w:val=\"" + headerFontSize + "\"/>"
                                        + "               <w:sz-cs w:val=\"" + headerFontSize + "\"/>"
                                     + "              </w:rPr>"

                                 + "                             <w:rPr> "
                                 + "                                 <w:b/> "
                                 + "                             </w:rPr> "
                                 + "                         </w:pPr> "
                                 + "                         <w:r wsp:rsidRPr=\"004374EC\"> "
                                + "                <w:rPr>"
                                 + "              <w:sz w:val=\"" + headerFontSize + "\"/>"
                                   + "            <w:sz-cs w:val=\"" + headerFontSize + "\"/>"
                                 + "          </w:rPr>"
                                 + "                             <w:rPr> "
                                 + "                                 <w:b/> "
                                 + "                             </w:rPr> "
                                 + "                             <w:t>" + value + "</w:t> "
                                 + "                         </w:r> "
                                 + "                     </w:p> "
                                 + "                 </w:tc> ";

        return markUp;


    }



    /// <summary>
    /// Ending Tag of Table Header Row..
    /// </summary>
    /// <returns></returns>
    public string GetHeaderRowEndString()
    {
        return "</w:tr>";
    }

    #endregion
    #region Extra Html To Xml MarkUps..
    public string UnorderedList(string value)
    {
        string markUp = "     /n<w:p wsp:rsidR=\"004F382E\" wsp:rsidRPr=\"004F382E\" wsp:rsidRDefault=\"008149C2\" wsp:rsidP=\"004F382E\">"
                          + "/n<w:pPr>"
                            + "   <w:listPr>"
                              + "   <w:ilvl w:val=\"0\"/>"
                               + "  <w:ilfo w:val=\"2\"/>"
                                + " <wx:t wx:val=\"·\"/>"
                               + "  <wx:font wx:val=\"Symbol\"/>"
                           + "  </w:listPr>"
                              + "/n<w:rPr>"
                                  + "/n<w:rFonts w:ascii=\"Mangal\" w:cs=\"Mangal\" />"
                              + "/n</w:rPr>"
                          + "/n</w:pPr>"
                          + "/n<w:r wsp:rsidRPr=\"00505659\">"
                              + "/n<w:rPr>"
                                  + "/n<wx:font wx:val=\"Calibri\" />"
                              + "/n</w:rPr>"
                              + "/n<w:t>" + value + "</w:t>"
                          + "/n</w:r>"
                      + "/n</w:p>";

        if (!string.IsNullOrEmpty(value))
            return markUp;
        else
            return "";
    }
    #endregion

    //Constructor.....
    public TableStrings(int rowFontSize)
    {
        headerFontSize = rowFontSize * 2;
    }
}
public class CellString
{

    int cellFontSize;
    string GetCellStartString = "<w:tc>    <w:tcPr>   <w:tcW w:w=\"0\" w:type=\"auto\"/>   </w:tcPr> ";
    string GetCellEndString = "  </w:tc> ";
    public string Align = string.Empty;


    #region Markup for creating table body row PS: Works Fine dont touch

    /// <summary>
    /// Creates a row where Cell/Table Defination for each row go under this.
    /// </summary>
    /// <returns></returns>
    public string GetCellRowStartString()
    {
        return "<w:tr wsp:rsidR=\"00505659\" wsp:rsidRPr=\"00505659\">";
    }
    /// <summary>
    /// Ending Tag of Table Header Row..
    /// </summary>
    /// <returns></returns>
    public string GetCellRowEndString()
    {
        return "</w:tr>";
    }

    #endregion

    string CellParagraghString(string value, string bold, bool isBoldAvailable)
    {
        string markUp = ""
                     + "  <w:p wsp:rsidR=\"00505659\" wsp:rsidRPr=\"00505659\" wsp:rsidRDefault=\"00505659\">"
                     + "     <w:pPr>" + Align
                     + "          <w:rPr>"
                     + "            <w:rFonts w:ascii=\"Mangal\" w:h-ansi=\"Mangal\" w:cs=\"Mangal\"/>"
                     + "            <wx:font wx:val=\"Mangal\"/>"
                     + "         </w:rPr> "
                     + "         </w:pPr>"
                     + "           <w:pPr>"
                     + "           <w:rPr>"
                     + "              <w:sz w:val=\"" + cellFontSize + "\"/>"
                     + "              <w:sz-cs w:val=\"" + cellFontSize + "\"/>"
                     + "            </w:rPr>"
                     + "            </w:pPr> boldMarkup "
                     + "              <w:r wsp:rsidRPr=\"00505659\"> "
                     + "           <w:rPr>"
                     + "              <w:sz w:val=\"" + cellFontSize + "\"/>"
                     + "              <w:sz-cs w:val=\"" + cellFontSize + "\"/>"
                     + "           </w:rPr>"
                     + "     <w:t>" + value + "</w:t> "
                     + "   </w:r> "
                     + "  </w:p> ";

        markUp = markUp.Replace("boldMarkup", bold);

        if (!string.IsNullOrEmpty(value))
            return markUp;
        else
            return "";
    }

    string CellParagraghBoldString(string value)
    {
        string markUp = ""
                   + "   <w:r wsp:rsidRPr=\"s\">"
                             + "  <w:rPr>"
                               + "    <wx:font wx:val=\"Calibri\" />"
                               + "    <w:b />"
                            + "   </w:rPr>"
                              + " <w:t>" + value + "</w:t>"
                         + "  </w:r>";

        if (!string.IsNullOrEmpty(value))
            return markUp;
        else
            return "";
    }

    public string GetBoldOnlyString(string value)
    {

        string markup = ""
  + "  <w:tc>"
  + "                  <w:tcPr>"
    + "                    <w:tcW w:w=\"0\" w:type=\"auto\"/>"
      + "              </w:tcPr>"
        + "            <w:p wsp:rsidR=\"00505659\" wsp:rsidRPr=\"00FC0623\" wsp:rsidRDefault=\"00FC0623\">"
          + "              <w:pPr>"
            + "                <w:jc w:val=\"left\"/>"
              + "              <w:rPr>"
                + "                <w:rFonts w:ascii=\"Calibri\" w:h-ansi=\"Calibri\" w:cs=\"Mangal\"/>"
                  + "              <wx:font wx:val=\"Calibri\"/>"
                    + "            <w:b/>"
                      + "          <w:b-cs/>"
                        + "        <w:lang w:val=\"HY\"/>"
                          + "  </w:rPr>"
                        + "</w:pPr>"
                        + "<w:r wsp:rsidRPr=\"00FC0623\">"
                         + "   <w:rPr>"
                          + "      <w:rFonts w:ascii=\"Calibri\" w:h-ansi=\"Calibri\"/>"
                            + "    <wx:font wx:val=\"Calibri\"/>"
                              + "  <w:b/>"
                                + "<w:b-cs/>"
                                + "<w:lang w:val=\"HY\"/>"
                           + " </w:rPr>"
                            + "<w:t>" + value + "</w:t>"
                        + "</w:r>"
                   + " </w:p>"
                + "</w:tc>";
        return markup;
    }

    public string GetBoldForGT(string value)
    {

        string markup = ""
  + "  <w:tc>"
  + "                  <w:tcPr>"
    + "                    <w:tcW w:w=\"0\" w:type=\"auto\"/>"
      + "              </w:tcPr>"
        + "            <w:p wsp:rsidR=\"00505659\" wsp:rsidRPr=\"00FC0623\" wsp:rsidRDefault=\"00FC0623\">"
          + "              <w:pPr>"
            + "                <w:jc w:val=\"right\"/>"
              + "              <w:rPr>"
                + "                <w:rFonts w:ascii=\"Calibri\" w:h-ansi=\"Calibri\" w:cs=\"Mangal\"/>"
                  + "              <wx:font wx:val=\"Calibri\"/>"
                    + "            <w:b/>"
                      + "          <w:b-cs/>"
                        + "        <w:lang w:val=\"HY\"/>"
                          + "  </w:rPr>"
                        + "</w:pPr>"
                        + "<w:r wsp:rsidRPr=\"00FC0623\">"
                         + "   <w:rPr>"
                          + "      <w:rFonts w:ascii=\"Calibri\" w:h-ansi=\"Calibri\"/>"
                            + "    <wx:font wx:val=\"Calibri\"/>"
                              + "  <w:b/>"
                                + "<w:b-cs/>"
                                + "<w:lang w:val=\"HY\"/>"
                           + " </w:rPr>"
                            + "<w:t>" + value + "</w:t>"
                        + "</w:r>"
                   + " </w:p>"
                + "</w:tc>";
        return markup;
    }

    string CellUnorderedListString(string value, string bold, bool isBoldAvailable)
    {

        string markup = ""
   + "               <w:p wsp:rsidR=\"00505659\" wsp:rsidRPr=\"00505659\" wsp:rsidRDefault=\"00A26F4E\" wsp:rsidP=\"003826FB\">     "
   + "     <w:pPr>                                                                                                                  "
   + "       <w:listPr>                                                                                                             "
   + "         <w:ilvl w:val=\"0\"/>                                                                                                "
   + "         <w:ilfo w:val=\"1\"/>                                                                                                "
   + "       </w:listPr>                                                                                                            "
   + "       <w:rPr>                                                                                                                "
   + "         <w:rFonts w:ascii=\"Mangal\" w:cs=\"Mangal\"/>                                                                       "
   + "       </w:rPr>                                                                                                               "
   + "     </w:pPr>       boldMarkup                                                    "


   + "     <w:r wsp:rsidRPr=\"00505659\">                                                                                           "
   + "       <w:rPr>                                                                                                                "
   + "         <wx:font wx:val=\"Calibri\"/>                                                                                        "
   + "       </w:rPr>                                                                                                               "
   + "       <w:t>" + value + "</w:t>                                                                                               "
   + "     </w:r>                                                                                                                   "
   + "                                                                                                                              "
   + "   </w:p>";


        markup = markup.Replace("boldMarkup", bold);

        return markup;

    }

    public string GetCellValue(string value)
    {

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string result = string.Empty;
        string cellMarkup = string.Empty;
        if (!value.Contains("if gte mso 9"))
        {
            sb.AppendLine(GetCellStartString);
            sb.AppendLine(CheckHtmlType(value));
            sb.AppendLine(GetCellEndString);
            result = sb.ToString();
        }

        result = result.Replace("╩", "");
        result = result.Replace("┌", "");
        result = result.Replace("╕", "");
        result = result.Replace("╘", "");
        result = result.Replace("►", "");
        result = result.Replace("ô", "");
        result = result.Replace("µ", "");
        result = result.Replace("►", "");
        result = result.Replace("ô", "");

        return result;
    }

    public string GetSubHeadCellValue(string value, int count)
    {

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string result = string.Empty;
        string cellMarkup = string.Empty;

        sb.AppendLine(GetCellStartString);
        sb.AppendLine(SubheadingParagraghString(value, count));
        sb.AppendLine(GetCellEndString);

        return sb.ToString();
    }

    string SubheadingParagraghString(string value, int totalRows)
    {
        string markUp = ""
                     + "  <w:p wsp:rsidR=\"00505659\" wsp:rsidRPr=\"00505659\" wsp:rsidRDefault=\"00505659\">"
                     + "     <w:pPr>"
                     + Align
                     + "   <w:gridSpan w:val=\"" + totalRows + "\"/>"
                     + "          <w:rPr>"
                     + "            <w:rFonts w:ascii=\"Mangal\" w:h-ansi=\"Mangal\" w:cs=\"Mangal\"/>"
                     + "            <wx:font wx:val=\"Mangal\"/><w:b/>"
                     + "         </w:rPr> "
                     + "         </w:pPr>"
                     + "           <w:pPr>"
                     + "           <w:rPr>"
                     + "              <w:sz w:val=\"" + cellFontSize + "\"/>"
                     + "              <w:sz-cs w:val=\"" + cellFontSize + "\"/>"
                     + "            </w:rPr>"
                     + "            </w:pPr> <w:b /> "
                     + "              <w:r wsp:rsidRPr=\"00505659\"> "
                     + "           <w:rPr>"
                     + "              <w:sz w:val=\"" + cellFontSize + "\"/>"
                     + "              <w:sz-cs w:val=\"" + cellFontSize + "\"/>"
                     + "           </w:rPr>"
                     + "     <w:t>" + value + "</w:t> "
                     + "   </w:r> "
                     + "  </w:p> ";

        markUp = markUp.Replace("boldMarkup", "");
        if (!string.IsNullOrEmpty(value))
            return markUp;
        else
            return "";
    }
    private string CheckHtmlType(string value)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (value.Contains("►"))
        {
            if (value.Contains("╘"))
            {
                int count = value.Count(x => x == '╘');
                for (int i = 0; i < count; i++)
                {
                    string boldMarkUp = string.Empty;
                    bool isBoldAvailable = false;
                    int startIndex = value.IndexOf("╕");
                    int endIndex = value.IndexOf("╘", startIndex);
                    string paraString = value.Substring(startIndex, (endIndex - startIndex) - 1);
                    value = value.Remove(startIndex, (endIndex - startIndex));

                    if (paraString != "╕╘" && !string.IsNullOrEmpty(paraString))
                    {
                        startIndex = paraString.IndexOf("╕");
                        endIndex = paraString.IndexOf("╘", startIndex);

                        if (startIndex > 0 && endIndex > 0)
                        {
                            paraString = paraString.Remove(startIndex, 1);
                            paraString = paraString.Remove(endIndex, 1);
                        }
                    }
                    if (paraString.Contains("╩"))
                    {
                        boldMarkUp = CellBoldCharString(paraString, out paraString);
                        isBoldAvailable = true;
                    }
                    if (paraString != "╕╘" && !string.IsNullOrEmpty(paraString))
                        sb.Append(CellParagraghString(paraString, boldMarkUp, isBoldAvailable));
                }
            }

            string[] values;
            value = value.Replace("►", "");
            value = value.Replace("ô", "");

            values = value.Split('µ');

            for (int i = 0; i < values.Length; i++)
            {
                string boldMarkUp = string.Empty;
                bool isBoldAvailable = false;
                string currentCell = values[i].ToString();
                currentCell = currentCell.Replace("µ", "");
                currentCell = currentCell.Replace('ä', ' ');

                if (currentCell.Contains("╩"))
                {
                    boldMarkUp = CellBoldCharString(currentCell, out currentCell);
                    isBoldAvailable = true;
                }

                sb.Append(CellUnorderedListString(currentCell, boldMarkUp, isBoldAvailable));
            }
        }

        else
        {
            if (value.Contains("╘"))
            {
                int count = value.Count(x => x == '╘');
                for (int i = 0; i < count; i++)
                {
                    string boldMarkUp = string.Empty;
                    bool isBoldAvailable = false;

                    int startIndex = value.IndexOf("╕");
                    int endIndex = value.IndexOf("╘", startIndex);
                    string paraString = value.Substring(startIndex, (endIndex - startIndex) + 1);
                    value = value.Remove(startIndex, (endIndex - startIndex) + 1);
                    if (paraString.Contains("╩"))
                    {
                        boldMarkUp = CellBoldCharString(paraString, out paraString);
                        isBoldAvailable = true;
                    }
                    sb.Append(CellParagraghString(paraString, boldMarkUp, isBoldAvailable));
                }
            }
            else
                sb.Append(CellParagraghString(value, "", false));
        }

        return sb.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values">Untamed string with the bold markups</param>
    /// <param name="value">Manuplated script after removing bold markups</param>   
    /// <returns></returns>
    private string CellBoldCharString(string values, out string value)
    {
        string result = string.Empty;
        value = string.Empty;

        //value = value.Replace("<style>", "☻");
        //int totalCount = value.Count(x => x == '☻');
        //for (int i = 0; i < totalCount; i++)
        //{
        int startIndex = values.IndexOf('╩');
        int endIndex = values.IndexOf('┌', startIndex);
        //    value = value.Remove(startIndex, (endIndex - startIndex) + 8);

        //}

        if (startIndex > 0 && endIndex > 0)
        {
            string paraString = values.Substring(startIndex, (endIndex - values.IndexOf('╩') + 1));
            value = values.Remove(startIndex, (endIndex - values.IndexOf('╩') + 1));
            value = values.Replace(paraString, "");
            paraString = paraString.Replace('╩', ' ');
            paraString = paraString.Replace('┌', ' ');
            result = CellParagraghBoldString(paraString);
        }

        return result;
    }

    public CellString(int tdFontSize)
    {
        cellFontSize = tdFontSize * 2;
    }
}


