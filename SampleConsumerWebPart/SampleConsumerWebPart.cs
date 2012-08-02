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

namespace ClientConnectionSample
{
    [Guid("0bab1085-f5f8-42ba-a406-9c6f8619121e")]
    public class SampleConsumerWebPart : Microsoft.SharePoint.WebPartPages.WebPart, ICellConsumer
    {
        [Obsolete]
        public override void EnsureInterfaces()
        {
            RegisterInterface("MyCellConsumer_WPQ_",
                InterfaceTypes.ICellConsumer,
                Microsoft.SharePoint.WebPartPages.WebPart.LimitOneConnection,
                ConnectionRunAt.Client,
                this,
                "CellConsumer_WPQ_",
                "Receive from MyCellProvider",
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
            // 以下はいずれも、クライアント側の接続では呼ばれません
            // (サーバー側の接続の場合のみ、実装)
        }

        protected override void RenderWebPart(HtmlTextWriter output)
        {
            EnsureChildControls(); // <-- 今回は、なくても良い

            output.Write(ReplaceTokens("<input type='text' id='MyText_WPQ_' />\n"));

            output.Write(ReplaceTokens(
                "<script language='javascript'>\n" +
                "var CellConsumer_WPQ_ = new funcCellConsumer_WPQ_();\n" +
                "function funcCellConsumer_WPQ_() {\n" +

                "  this.PartCommunicationInit = myInit;\n" +
                "  this.CellProviderInit = myCellProviderInit;\n" +
                "  this.CellReady = myCellReady;\n" +

                "  function myInit() {\n" +
                "    var cellConsumerInitArgs = new Object();\n" +
                "    cellConsumerInitArgs.FieldName = 'CellName';\n" +
                "    WPSC.RaiseConnectionEvent('MyCellConsumer_WPQ_', 'CellConsumerInit', cellConsumerInitArgs);\n" +
                "  }\n" +

                "  function myCellProviderInit(sender, cellProviderInitArgs) { }\n" +

                "  function myCellReady(sender, cellReadyArgs) {\n" +
                "    document.all('MyText_WPQ_').value = cellReadyArgs.Cell;\n" +
                "  }\n" +

                "}\n" +

                "</script>\n"));
        }

        #region ICellConsumer メンバ

        // 以下はいずれも、クライアント側の接続では呼ばれません
        // (サーバー側の接続の場合のみ、実装)
        public event CellConsumerInitEventHandler CellConsumerInit;
        public void CellProviderInit(object sender, CellProviderInitEventArgs cellProviderInitArgs)
        {
        }
        public void CellReady(object sender, CellReadyEventArgs cellReadyArgs)
        {
        }

        #endregion
    }
}
