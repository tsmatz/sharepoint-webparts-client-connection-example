using System;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Serialization;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint.WebPartPages.Communication;

namespace ClientConnectionWebPart
{
    [Guid("9ddf3991-37ad-49db-9993-e874b65893b6")]
    public class SampleProviderWebPart : Microsoft.SharePoint.WebPartPages.WebPart, ICellProvider
    {
        [Obsolete]
        public override void EnsureInterfaces()
        {
            RegisterInterface("MyCellProvider_WPQ_",
               InterfaceTypes.ICellProvider,
               Microsoft.SharePoint.WebPartPages.WebPart.LimitOneConnection,
               ConnectionRunAt.Client,
               this,
               "CellProvider_WPQ_",
               "Send to MyCellConsumer",
               "this is test");
        }

        [Obsolete]
        public override ConnectionRunAt CanRunAt()
        {
            return ConnectionRunAt.Client;
        }

        [Obsolete]
        public override void PartCommunicationConnect(string interfaceName, Microsoft.SharePoint.WebPartPages.WebPart connectedPart, string connectedInterfaceName, ConnectionRunAt runAt)
        {
            if (runAt == ConnectionRunAt.Client)
            {
                // 接続時に呼ばれるため、接続時の処理はここに書きます。
                // 今回は、何もしない . . .
            }
            EnsureChildControls(); // <-- 今回は、なくても良い
        }

        [Obsolete]
        public override void PartCommunicationInit()
        {
            // クライアント側の接続では、ここは呼ばれません
            // (サーバー側の接続の場合のみ、実装)
        }

        [Obsolete]
        public override void PartCommunicationMain()
        {
            // クライアント側の接続では、ここは呼ばれません
            // (サーバー側の接続の場合のみ、実装)
        }

        protected override void RenderWebPart(HtmlTextWriter output)
        {
            EnsureChildControls(); // <-- 今回は、なくても良い

            output.Write(ReplaceTokens(
                "<select id='MySelect_WPQ_' onchange='MenuChange_WPQ_()'>\n" +
                "<option value='1'>1番選択</option>\n" +
                "<option value='2'>2番選択</option>\n" +
                "</select>"));

            output.Write(ReplaceTokens(
                "<script language='javascript'>\n" +
                "var CellProvider_WPQ_ = new funcCellProvider_WPQ_();\n" +
                "function funcCellProvider_WPQ_() {\n" +

                "  this.PartCommunicationInit = myInit;\n" +
                "  this.PartCommunicationMain = myMain;\n" +
                "  this.CellConsumerInit = myCellConsumerInit;\n" +

                "  function myInit() {\n" +
                "    var cellProviderInitArgs = new Object();\n" +
                "    cellProviderInitArgs.FieldName = 'CellName';\n" +
                "    WPSC.RaiseConnectionEvent('MyCellProvider_WPQ_', 'CellProviderInit', cellProviderInitArgs);\n" +
                "  }\n" +

                "  function myMain() {\n" +
                "    var cellReadyArgs = new Object();\n" +
                "    cellReadyArgs.Cell = '';\n" +
                "    WPSC.RaiseConnectionEvent('MyCellProvider_WPQ_', 'CellReady', cellReadyArgs);\n" +
                "  }\n" +

                "  function myCellConsumerInit(sender, cellConsumerInitArgs) { }\n" +

                "}\n" +

                "function MenuChange_WPQ_() {\n" +
                "  var cellReadyArgs = new Object();" +
                "  cellReadyArgs.Cell = document.all('MySelect_WPQ_').value;\n" +
                "  WPSC.RaiseConnectionEvent('MyCellProvider_WPQ_', 'CellReady', cellReadyArgs);\n" +
                "}\n" +

                "</script>\n"));
        }

        #region ICellProvider メンバ

        // 以下はいずれも、クライアント側の接続では呼ばれません
        // (サーバー側の接続の場合のみ、実装)
        public void CellConsumerInit(object sender, CellConsumerInitEventArgs cellConsumerInitEventArgs)
        {
        }
        public event CellProviderInitEventHandler CellProviderInit;
        public event CellReadyEventHandler CellReady;

        #endregion
    }
}
