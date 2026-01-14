<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <HTML>
      <HEAD>
        <!--<link rel="stylesheet" type="text/css" href="http://localhost:56647/Css/globalPrintFont.css" />-->
        <STYLE>
          .stdPageMainHdr { color: #808080; text-align: center; padding-left: 4px; padding-top: 4px; padding-bottom: 4px; width: 100%; font-size: 22pt; }
          .stdPVTblLCell { background-color: #00a7e7; color: #808080; font-weight: bold; text-align: center; padding-left: 4px; padding-top: 4px; padding-bottom: 4px; width: 100%; font-size: 13pt; }
          .stdPageHdr { color: #808080; font-style:normal;text-align: center; padding-left: 4px; padding-top: 4px; padding-bottom: 4px; width: 100%; font-size: 12pt; }
          .stdAddlHdr { color: black; font-style:normal;font-weight: bold;text-align: center; padding-left: 4px; padding-top: 4px; width: 100%; font-size: 13.5pt; }
          .gridHeader { color: #000000;   font-weight: bold;font-size: 12pt;vertical-align:middle; text-align:center; border: solid thin Black; }
          .subhead { color: #8B0000; font-size: 12pt;border: solid thin Black;text-decoration : underline;}
          .stot { color: #8B0000; font-size: 12pt; font-weight: normal;border: solid thin Black;}
          .gtot { color: #2B8B00; font-size: 12pt; font-weight: normal;border: solid thin Black;text-decoration : underline;}
          .subhead-left { color: #8B0000; font-size: 12pt;text-align:left;border: solid thin Black;text-decoration : underline;}
          .stot-left { color: #8B0000; font-size: 12pt; font-weight: normal;text-align:left;border: solid thin Black;}
          .gtot-left { color: #2B8B00; font-size: 12pt; font-weight: normal;text-align:left;border: solid thin Black;text-decoration : underline;}
          .subhead-right { color: #8B0000; font-size: 12pt;text-align:right;border: solid thin Black;text-decoration : underline;}
          .stot-right { color: #8B0000; font-size: 12pt; font-weight: normal;text-align:right;border: solid thin Black;}
          .gtot-right { color: #2B8B00; font-size: 12pt; font-weight: normal;text-align:right;border: solid thin Black;text-decoration : underline;}
          .subhead-middle { color: #8B0000; font-size: 12pt;text-align:midddle;border: solid thin Black;text-decoration : underline;}
          .stot-middle { color: #8B0000; font-size: 12pt; font-weight: normal;text-align:midddle;border: solid thin Black;}
          .gtot-middle { color: #2B8B00; font-size: 12pt; font-weight: normal;text-align:midddle;border: solid thin Black;text-decoration : underline;}
          .subhead-center { color: #8B0000; font-size: 12pt;text-align:center;border: solid thin Black;text-decoration : underline;}
          .stot-center { color: #8B0000; font-size: 12pt; font-weight: normal;text-align:center;border: solid thin Black;}
          .gtot-center { color: #2B8B00; font-size: 12pt; font-weight: normal;text-align:center;border: solid thin Black;text-decoration : underline;}
          .searchresultitem, .searchresultitem- {  color: Black; font-size: 8pt;border: solid thin Black; }
          .searchresultitem-left {  color: Black; font-size: 8pt;text-align:left;border: solid thin Black; }
          .searchresultitem-right {  color: Black; font-size: 8pt;text-align:right;border: solid thin Black; }
          .searchresultitem-middle {  color: Black; font-size: 8pt;text-align:midddle;border: solid thin Black; }
          .searchresultitem-center {  color: Black; font-size: 8pt;text-align:center;border: solid thin Black; }
          .searchresultaltitem {  color: Black; font-size: 8pt;border: solid thin Black; }
          .searchresultaltitem-left {  color: Black; font-size: 8pt;text-align:left;border: solid thin Black; }
          .searchresultaltitem-right {  color: Black; font-size: 8pt;text-align:right;border: solid thin Black; }
          .searchresultaltitem-middle {  color: Black; font-size: 8pt;text-align:midddle;border: solid thin Black; }
          .searchresultaltitem-center {  color: Black; font-size: 8pt;text-align:center;border: solid thin Black; }
          .stdPageftr { color: #808080; font-style:normal;text-align: center; padding-left: 4px; padding-top: 4px; padding-bottom: 4px; width: 100%; font-size: 12pt; }
          .right { text-align:right; }
          .left { text-align:left; }
          .middle { text-align:midddle; }
        </STYLE>
      </HEAD>
      <BODY>
        <TABLE>
          <TR>
            <TD class="stdPageMainHdr" colspan="7">
              <xsl:for-each select="/data/headrow">
                <xsl:value-of select="@projectName"/>
              </xsl:for-each >
            </TD>
          </TR>
          <TR>
            <TD class="stdPageHdr" colspan="7">
              <xsl:for-each select="/data/headrow">
                <xsl:value-of select="@caption"/>
              </xsl:for-each >
            </TD>
          </TR>

          <xsl:if test="/data/printHeaders/headerData">
            <xsl:for-each select="/data/printHeaders/headerData">
              <xsl:variable name="headerText" select ="."></xsl:variable>
              <xsl:if test="$headerText != ''">
              <TR>
                <TD class="stdPageHdr" colspan="7">
                  <xsl:value-of select="." />
                </TD>
              </TR>
              </xsl:if>
            </xsl:for-each >
          </xsl:if>

          <TR>
            <TD colspan="7" align="right">
              Date :
              <xsl:for-each select="/data/headrow">
                <xsl:value-of select="@crdate"/>
              </xsl:for-each >
            </TD>
          </TR>
          <TR>
            <TD class="stdAddlHdr" colspan="7">
              <xsl:for-each select="/data/headrow">
                <xsl:variable name="acap" select ="@addlcaption"></xsl:variable>
                <xsl:variable name="empty_string"/>
                <xsl:variable name="first" select="substring-before($acap, '*,*')"></xsl:variable >
                <xsl:variable name="second" select="substring-after($acap, '*,*')"></xsl:variable >
                <xsl:if test ="$first != $empty_string">
                  <xsl:value-of select= "$first"/>
                </xsl:if>
                <br style="mso-data-placement:same-cell;"></br>
                <xsl:if test ="$second != $empty_string">
                  <xsl:variable name="sfir" select="substring-before($second, '*,*')"></xsl:variable >
                  <xsl:if test ="$sfir != $empty_string">
                    <xsl:value-of select= "$sfir"/>
                  </xsl:if>
                  <br style="mso-data-placement:same-cell;"></br>
                  <xsl:variable name="sremain" select="substring-after($second, '*,*')"></xsl:variable >
                  <xsl:if test ="$sremain != $empty_string">
                    <xsl:variable name="tfir" select="substring-before($sremain, '*,*')"></xsl:variable >
                    <xsl:if test ="$tfir != $empty_string">
                      <xsl:value-of select= "$tfir"/>
                    </xsl:if>
                    <br style="mso-data-placement:same-cell;"></br>
                    <xsl:variable name="tremain" select="substring-after($sremain, '*,*')"></xsl:variable >
                    <xsl:if test ="$tremain != $empty_string">
                      <xsl:value-of select= "$tremain"/>
                      <xsl:variable name="ffir" select="substring-before($tremain, '*,*')"></xsl:variable >
                      <xsl:if test ="$ffir != $empty_string">
                        <xsl:value-of select= "$ffir"/>
                      </xsl:if>
                    </xsl:if>
                  </xsl:if>
                </xsl:if>
              </xsl:for-each >

            </TD >
          </TR>

          <TR>
            <TD colspan="7" align="left">
              <xsl:for-each select="/data/headrow">
                <xsl:variable name="paramval" select ="@params"></xsl:variable>
                <xsl:call-template name="tokenize">
                  <xsl:with-param name="text" select ="$paramval"/>
                </xsl:call-template>
              </xsl:for-each >
            </TD>
          </TR>

          <xsl:for-each select ="/*/*">
            <xsl:if test="position() = 1">
              <TR>
                <xsl:for-each select="head/ghead">

                  <TD>
                    <xsl:attribute name="class">
                      <xsl:value-of select ="@class"/>
                    </xsl:attribute>
                    <xsl:attribute name="style">
                      <xsl:value-of select ="/data/headrow/@valign"/>
                    </xsl:attribute>
                    <xsl:attribute name="colspan">
                      <xsl:value-of select ="@colspan"/>
                    </xsl:attribute>
                    <xsl:value-of select="." />
                  </TD>
              
                </xsl:for-each >
              </TR>
            </xsl:if>
            <xsl:if test="position() = 3">
              <TR>
                <xsl:for-each select="*">
                  <TD class="gridHeader">
                    <xsl:attribute name="style">
                      <xsl:value-of select ="/data/headrow/@valign"/>
                    </xsl:attribute>
                    <xsl:value-of select="." />
                  </TD>
                </xsl:for-each >
              </TR>
            </xsl:if>
            <xsl:if test="position() &gt; 3">
              <xsl:if test="position() mod 2 = 1">
                <TR>
                  <xsl:for-each select="*">
                    <TD>
                      <xsl:attribute name="class">
                        <xsl:value-of select ="@class"/>
                      </xsl:attribute>
                      <xsl:attribute name="style">
                        <xsl:value-of select ="/data/headrow/@valign"/>
                      </xsl:attribute>
                      <xsl:value-of select="." />
                    </TD>
                  </xsl:for-each >
                </TR>
              </xsl:if>
              <xsl:if test="position() mod 2 != 1">
                <TR>
                  <xsl:for-each select="*">
                    <TD>
                      <xsl:attribute name="class">
                        <xsl:value-of select ="@class"/>
                      </xsl:attribute>
                      <xsl:attribute name="style">
                        <xsl:value-of select ="/data/headrow/@valign"/>
                      </xsl:attribute>
                      <xsl:value-of select="." />
                    </TD>
                  </xsl:for-each >
                </TR>
              </xsl:if>
            </xsl:if >

          </xsl:for-each>

          <TR>
            <TD class="stdPageftr" colspan="7">
              <xsl:for-each select="/data/headrow">
                <xsl:variable name="acap" select="@footer"></xsl:variable>
                <xsl:variable name="empty_string"/>
                <xsl:variable name="first" select="substring-before($acap, '~')"></xsl:variable >
                <xsl:variable name="second" select="substring-after($acap, '~')"></xsl:variable >
                <br></br>
                <xsl:if test ="$first != $empty_string">
                  <xsl:value-of select= "$first"/>
                </xsl:if>
                <br style="mso-data-placement:same-cell;"></br>
                <xsl:if test ="$second != $empty_string">
                  <xsl:value-of select= "$second"/>
                </xsl:if>
              </xsl:for-each >




            </TD>
          </TR>

        </TABLE>
      </BODY>
    </HTML>

  </xsl:template>
  <xsl:template match="text/text()" name="tokenize">
    <xsl:param name="text" select="."/>
    <xsl:variable name="separator" select="'~'"/>
    <xsl:choose>
      <xsl:when test="not(contains($text, $separator))">
        <xsl:variable name="newtext" select="translate($text,',','-')"/>
        <xsl:value-of select="normalize-space($newtext)"/>
        <br style="mso-data-placement:same-cell;"></br>
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="newtext" select="translate(normalize-space(substring-before($text, $separator)),',','-')"/>
        <xsl:value-of select="normalize-space($newtext)"/>
        <br style="mso-data-placement:same-cell;"></br>

        <xsl:call-template name="tokenize">
          <xsl:with-param name="text" select="substring-after($text, $separator)"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template >
</xsl:stylesheet>

