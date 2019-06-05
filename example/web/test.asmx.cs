using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace JM.Common.Examples.web
{
    /// <summary>
    /// test1 的摘要说明
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class test1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string Get(string json)
        {
            var result = "<?xml version=\"1.0\"?>" +
"<ReportData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
  @"<HasError>false</HasError>
  <Tables>
    <DataTable>
      <Name>ReportControlCoinfig4</Name>
      <Columns>
        <DataColumn>
          <Name>dtStatDate</Name>
          <DisplayName>统计日期</DisplayName>
          <DataType>System.String</DataType>
          <Index>0</Index>
        </DataColumn>
        <DataColumn>
          <Name>iUserNum</Name>
          <DisplayName>扣费用户数</DisplayName>
          <DataType>System.UInt32</DataType>
          <Index>1</Index>
        </DataColumn>
        <DataColumn>
          <Name>iPayment</Name>
          <DisplayName>日扣费额(Q币)</DisplayName>
          <DataType>System.Decimal</DataType>
          <Index>2</Index>
        </DataColumn>
      </Columns>
      <Rows>
        <DataRow>
          <ItemArray>
            <DataItem>
              <ColumnName>dtStatDate</ColumnName>
              <Value>2012-04-11</Value>
            </DataItem>
            <DataItem>
              <ColumnName>iUserNum</ColumnName>
              <Value>2431</Value>
            </DataItem>
            <DataItem>
              <ColumnName>iPayment</ColumnName>
              <Value>440620.9</Value>
            </DataItem>
          </ItemArray>
        </DataRow>
        <DataRow>
          <ItemArray>
            <DataItem>
              <ColumnName>dtStatDate</ColumnName>
              <Value>2012-04-12</Value>
            </DataItem>
            <DataItem>
              <ColumnName>iUserNum</ColumnName>
              <Value>3225</Value>
            </DataItem>
            <DataItem>
              <ColumnName>iPayment</ColumnName>
              <Value>754549.5</Value>
            </DataItem>
          </ItemArray>
        </DataRow>
        <DataRow>
          <ItemArray>
            <DataItem>
              <ColumnName>dtStatDate</ColumnName>
              <Value>2012-04-13</Value>
            </DataItem>
            <DataItem>
              <ColumnName>iUserNum</ColumnName>
              <Value>2123</Value>
            </DataItem>
            <DataItem>
              <ColumnName>iPayment</ColumnName>
              <Value>325090.2</Value>
            </DataItem>
          </ItemArray>
        </DataRow>
        <DataRow>
          <ItemArray>
            <DataItem>
              <ColumnName>dtStatDate</ColumnName>
              <Value>2012-04-14</Value>
            </DataItem>
            <DataItem>
              <ColumnName>iUserNum</ColumnName>
              <Value>2206</Value>
            </DataItem>
            <DataItem>
              <ColumnName>iPayment</ColumnName>
              <Value>321449.4</Value>
            </DataItem>
          </ItemArray>
        </DataRow>
        <DataRow>
          <ItemArray>
            <DataItem>
              <ColumnName>dtStatDate</ColumnName>
              <Value>2012-04-15</Value>
            </DataItem>
            <DataItem>
              <ColumnName>iUserNum</ColumnName>
              <Value>2304</Value>
            </DataItem>
            <DataItem>
              <ColumnName>iPayment</ColumnName>
              <Value>356877.0</Value>
            </DataItem>
          </ItemArray>
        </DataRow>
      </Rows>
    </DataTable>
  </Tables>
</ReportData>";

            return (result);
        }
    }
}
