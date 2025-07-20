using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;     
using System.Threading;    


namespace WindowsFormsApp1
{
    public partial class TestStation : Form
    {
        private SerialPort serialPortRx = new SerialPort();
        private SerialPort serialPortTx = new SerialPort();

        public int BtnTestSend_Click { get; }

        public TestStation()
        {
            InitializeComponent();
            serialPortRx.DataReceived += SerialPortRx_DataReceived;
            serialPortRx.ErrorReceived += SerialPortRx_ErrorReceived;
            this.Load += TestStation_Load;
            
        }

        private void SerialPortRx_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SerialPortRx_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        // Mevcut COM portlarını listelemek için
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblRxTeamId(object sender, EventArgs e)
        {
            
        }
        // Jiroskop Y değeri
        private void lblRxGyroYCaption_Click(object sender, EventArgs e)
        {

        }
        // İvme X
        private void label4_Click(object sender, EventArgs e)
        {

        }
        // Durum 
        private void lblRxStateCaption_Click(object sender, EventArgs e)
        {

        }
        // Jiroskop X değeri 
        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void tlpRxData_Paint(object sender, PaintEventArgs e)
        {

        }
        // Jiroskop Y değeri
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblRxCounterCaption_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }
        // Kademe Boylam
        private void lblRxStageLngCaption_Click(object sender, EventArgs e)
        {

        }
        //Gps İrtifası
        private void TestStation_Load(object sender, EventArgs e)
        {
            
        }

        private void Gelen1_Click(object sender, EventArgs e)
        {

        }

        private void RxPortSetting_Click(object sender, EventArgs e)
        {

        }

        private void RxPortOn_Click(object sender, EventArgs e)
        {

        }

        private void TxPortSetting_Click(object sender, EventArgs e)
        {

        }

        private void TxPortOn_Click(object sender, EventArgs e)
        {

        }

        private void TxDataSend_Click(object sender, EventArgs e)
        {

        }
        //irtifa
        private void lblRxAltitudeCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblRxGpsAltitudeCaption_Click(object sender, EventArgs e)
        {

        }
        // Roket enlem 
        private void lblRxLatCaption_Click(object sender, EventArgs e)
        {

        }
        // Roket enlem değeri
        private void lblRxLat_Click(object sender, EventArgs e)
        {

        }

        // Roket boylam
        private void lblRxLngCaption_Click(object sender, EventArgs e)
        {

        }
        // Roket boylam değeri
        private void lblRxLng_Click(object sender, EventArgs e)
        {

        }
        //Görev Yükü GPS İrtifa
        private void lblRxPayloadGpsAltCaption_Click(object sender, EventArgs e)
        {

        }
        // Görev Yükü Enlem
        private void lblRxPayloadLatCaption_Click(object sender, EventArgs e)
        {

        }
        // Görev Yükü Boylam
        private void lblRxPayloadLngCaption_Click(object sender, EventArgs e)
        {

        }
        // Kademe GPS İrtifa
        private void lblRxStageGpsAltCaption_Click(object sender, EventArgs e)
        {

        }
        // Kademe Enlem
        private void lblRxStageLatCaption_Click(object sender, EventArgs e)
        {

        }
        // Jiroskop X
        private void lblRxGyroXCaption_Click(object sender, EventArgs e)
        {

        }
        // Jiroskop Z
        private void lblRxGyroZCaption_Click(object sender, EventArgs e)
        {

        }
        // İvme Y
        private void lblRxAccelYCaption_Click(object sender, EventArgs e)
        {

        }
        // İvme Z
        private void lblRxAccelZCaption_Click(object sender, EventArgs e)
        {

        }
        // Açı 
        private void lblRxAngleCaption_Click(object sender, EventArgs e)
        {

        }
        // Crc 
        private void lblRxCrcCaption_Click(object sender, EventArgs e)
        {

        }
        // Takım ID
        private void lblTxTeamIdCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxCounterCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxAltitudeCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxLatCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxLngCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxPayloadLatCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxStageGpsAltCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxStageLatCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxStageLngCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxGyroXCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxGyroYCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxGyroZCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxAccelXCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxAccelYCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxAccelZCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxAngleCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxStateCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxCrcCaption_Click(object sender, EventArgs e)
        {

        }

        private void lblTxTeamId_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxCounter_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxAltitude_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxGpsAltitude_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxLat_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxLng_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxPayloadGpsAlt_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxPayloadLat_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxPayloadLng_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxStageGpsAlt_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxStageLat_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxStageLng_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxGyroX_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxGyroY_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxGyroZ_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxAccelX_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxAccelY_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxAccelZ_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxAngle_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxState_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTxCrc_TextChanged(object sender, EventArgs e)
        {

        }

        private void Gelen2_Click(object sender, EventArgs e)
        {

        }

        private void Giden1_Click(object sender, EventArgs e)
        {

        }

        private void Giden2_Click(object sender, EventArgs e)
        {

        }

        private void lblRxCounter_Click(object sender, EventArgs e)
        {

        }
        // irtifa değeri 
        private void lblRxAltitude_Click(object sender, EventArgs e)
        {

        }
        // Gps irtifa değeri 
        private void lblRxGpsAltitude_Click(object sender, EventArgs e)
        {

        }


        //Görev Yükü GPS İrtifa değeri 
        private void lblRxPayloadGpsAltitude_Click(object sender, EventArgs e)
        {

        }
        //Görev Yükü Enlem değeri
        private void lblRxPayloadLat_Click(object sender, EventArgs e)
        {

        }
        // Görev Yükü Boylam değeri
        private void lblRxPayloadLng_Click(object sender, EventArgs e)
        {

        }
        // Kademe GPS İrtifa değeri
        private void lblRxStageGpsAltitude_Click(object sender, EventArgs e)
        {

        }
        // Kademe Enlem değeri
        private void lblRxStageLat_Click(object sender, EventArgs e)
        {

        }
        // Kademe Boylam değeri
        private void lblRxStageLng_Click(object sender, EventArgs e)
        {

        }
        // Jiroskop Z değeri
        private void lblRxGyroZ_Click(object sender, EventArgs e)
        {

        }
        // İvme X değeri
        private void lblRxAccelX_Click(object sender, EventArgs e)
        {

        }
        // İvme Y değeri
        private void lblRxAccelY_Click(object sender, EventArgs e)
        {

        }
        // İvme Z değeri
        private void lblRxAccelZ_Click(object sender, EventArgs e)
        {

        }
        // Açı değeri
        private void lblRxAngle_Click(object sender, EventArgs e)
        {

        }
        // Durum değeri
        private void lblRxState_Click(object sender, EventArgs e)
        {

        }

        private void RxTestGroupBox_Enter(object sender, EventArgs e)
        {

        }
        // Crc değeri
        private void lblRxCrc_Click(object sender, EventArgs e)
        {

        }
        // Doğrulama Kodu ?
        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
