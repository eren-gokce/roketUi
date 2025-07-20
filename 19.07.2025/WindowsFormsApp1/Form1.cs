using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Globalization;
using System.Windows.Shapes;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SerialPort _loraPort;
        private GMapOverlay _markersOverlay;
        private double _lastLat = 0, _lastLon = 0;
        private string _selectedPortName;
        private bool _hasReceivedData = false;

        // Terminal için değişkenler
        private RichTextBox _terminalOutput;
        private StringBuilder _rawDataBuffer = new StringBuilder();
        private const int MAX_TERMINAL_LINES = 1000;

        public Form1()
        {
            InitializeComponent();
            _terminalOutput = rtbDataTerminal;

            this.Load += Form1_Load;
            cmbComPorts.SelectedIndexChanged += cmbComPorts_SelectedIndexChanged;
            btnConnect.Click += btnConnect_Click;

            SetParachute1State(false);
            SetParachute2State(false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbComPorts.Items.Clear();
            cmbComPorts.Items.AddRange(SerialPort.GetPortNames());
            if (cmbComPorts.Items.Count > 0)
                cmbComPorts.SelectedIndex = 0;

            // Harita ve terminali başlat
            InitializeTerminal();
            InitializeMap();
            _markersOverlay = new GMapOverlay("markers");
            gMap.Overlays.Add(_markersOverlay);

        }

        private void InitializeTerminal()
        {
            _terminalOutput = rtbDataTerminal;
            _terminalOutput.BackColor = Color.Black;
            _terminalOutput.ForeColor = Color.LimeGreen;
            _terminalOutput.Font = new Font("Consolas", 9);
            _terminalOutput.ReadOnly = true;
            _terminalOutput.ScrollBars = RichTextBoxScrollBars.Vertical;
            _terminalOutput.WordWrap = true;
            _terminalOutput.Text = "=== LoRa Terminal - Başlatılıyor ===\n";
        }

        private void InitializeMap()
        {
            // ===== Harita Ayarları =====
            gMap.MapProvider = GMapProviders.GoogleMap;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gMap.MinZoom = 2;
            gMap.MaxZoom = 18;
            gMap.Zoom = 5;

            // Başlangıç koordinatları
            double başlangıçLat = 39.96922983;
            double başlangıçLng = 32.74309033;
            gMap.Position = new PointLatLng(başlangıçLat, başlangıçLng);

            gMap.ShowCenter = false;
            gMap.CanDragMap = true;
            gMap.DragButton = MouseButtons.Left;

            // ===== Overlay (Marker Katmanı) Oluşturma =====
            _markersOverlay = new GMapOverlay("markers");
            gMap.Overlays.Add(_markersOverlay);

            // ===== İlk Mavi Pushpin (Initial Marker) =====
            var initialMarker = new GMarkerGoogle(
                new PointLatLng(başlangıçLat, başlangıçLng),
                GMarkerGoogleType.blue_pushpin
            );
            _markersOverlay.Markers.Add(initialMarker);
        }
        private void LoRaPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // 1) Ham veriyi oku
                string receivedData = _loraPort.ReadExisting();
                if (string.IsNullOrEmpty(receivedData))
                    return;

                // 2) İlk veri geldiğinde “Bağlandı” durumuna geç
                if (!_hasReceivedData)
                {
                    _hasReceivedData = true;
                    this.Invoke((MethodInvoker)(() =>
                    {
                        AddToTerminal("✓ Veri akışı başladı — Bağlandı!");
                        lblRxStatus.Text = "Bağlandı";
                        lblRxStatus.ForeColor = Color.LimeGreen;
                    }));
                }

                // 3) Ham veriyi terminale yaz
                this.Invoke((MethodInvoker)(() =>
                {
                    AddToTerminal($"[{DateTime.Now:HH:mm:ss.fff}] RAW: {receivedData.Replace('\n', '↵').Replace('\r', '↵')}");
                }));

                // 4) Satır bazlı parse etmek için buffer’a ekle
                _rawDataBuffer.Append(receivedData);
                string bufferContent = _rawDataBuffer.ToString();
                string[] lines = bufferContent.Split('\n');

                // 5) Tam satırları işle, kalanını buffer’da tut
                _rawDataBuffer.Clear();
                if (!bufferContent.EndsWith("\n"))
                {
                    _rawDataBuffer.Append(lines.Last());
                    lines = lines.Take(lines.Length - 1).ToArray();
                }

                // 6) Her satırı temizle ve işleme gönder
                foreach (string rawLine in lines)
                {
                    if (string.IsNullOrWhiteSpace(rawLine))
                        continue;

                    string cleanLine = rawLine.Trim();
                    ProcessDataLine(cleanLine);
                }
            }
            catch (Exception ex)
            {
                // Hata varsa terminale yaz
                this.Invoke((MethodInvoker)(() =>
                {
                    AddToTerminal($"✗ Veri işleme hatası: {ex.Message}");
                }));
            }
        }


        private void ProcessDataLine(string line)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                AddToTerminal($"[{DateTime.Now:HH:mm:ss.fff}] PARSE: {line}");
            }));

            var parts = line.Split(new[] { ':' }, 2);
            if (parts.Length != 2) return;

            string key = parts[0].Trim();
            string value = parts[1].Trim();

            this.Invoke((MethodInvoker)(() =>
            {
                switch (key)
                {
                    case "TX bytes total":
                        lblPacketCounter.Text = value;
                        AddToTerminal($"✓ Paket sayacı güncellendi: {value}");
                        break;

                    case "BaroAlt":
                        if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var ba))
                        {
                            lblBaroAlt.Text = ba.ToString("F2");
                            AddToTerminal($"✓ Baro irtifa: {ba:F2}m");
                        }
                        break;

                    case "GPSAlt":
                        if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var ga))
                        {
                            lblGPSAlt.Text = ga.ToString("F2");
                            AddToTerminal($"✓ GPS irtifa: {ga:F2}m");
                        }
                        break;

                    case "Lat":
                        if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat))
                        {
                            _lastLat = lat;
                            lblLat.Text = lat.ToString("F6");
                            AddToTerminal($"✓ Enlem: {lat:F6}°");
                            if (_lastLon != 0) UpdateMapWithGps(_lastLat, _lastLon);
                        }
                        break;

                    case "Lon":
                        if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var lon))
                        {
                            _lastLon = lon;
                            lblLon.Text = lon.ToString("F6");
                            AddToTerminal($"✓ Boylam: {lon:F6}°");
                            if (_lastLat != 0) UpdateMapWithGps(_lastLat, _lastLon);
                        }
                        break;

                    case "Yaw":
                        if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var yaw))
                        {
                            lblYaw.Text = yaw.ToString("F2");
                            AddToTerminal($"✓ Yaw: {yaw:F2}°");
                        }
                        break;

                    case "Pitch":
                        if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var pitch))
                        {
                            lblPitch.Text = pitch.ToString("F2");
                            AddToTerminal($"✓ Pitch: {pitch:F2}°");
                        }
                        break;

                    case "row":
                        if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var row))
                        {
                            lblRoll.Text = row.ToString("F2");
                            AddToTerminal($"✓ Roll: {row:F2}°");
                        }
                        break;

                    case "Accel":
                        var acc = value.Split(',');
                        if (acc.Length == 3
                            && float.TryParse(acc[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var ax)
                            && float.TryParse(acc[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var ay)
                            && float.TryParse(acc[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var az))
                        {
                            lblAccelX.Text = ax.ToString("F3");
                            lblAccelY.Text = ay.ToString("F3");
                            lblAccelZ.Text = az.ToString("F3");
                            AddToTerminal($"✓ İvme: X={ax:F3}, Y={ay:F3}, Z={az:F3}");
                        }
                        break;

                    case "Speed":
                        if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var sp))
                        {
                            lblSpeed.Text = sp.ToString("F2");
                            AddToTerminal($"✓ Hız: {sp:F2}");
                        }
                        break;

                    case "Status":
                        lblStatus.Text = value;
                        if (int.TryParse(value, out var st))
                        {
                            SetParachute1State((st & 1) != 0);
                            SetParachute2State((st & 2) != 0);
                            AddToTerminal($"✓ Durum: {st} (Para1:{(st & 1) != 0}, Para2:{(st & 2) != 0})");
                        }
                        break;

                    default:
                        AddToTerminal($"? Bilinmeyen anahtar: {key} = {value}");
                        break;
                }
            }));
        }

        private void AddToTerminal(string message)
        {
            try
            {
                if (_terminalOutput.InvokeRequired)
                {
                    _terminalOutput.Invoke((MethodInvoker)(() => AddToTerminal(message)));
                    return;
                }
                _terminalOutput.AppendText($"{message}\n");
                if (_terminalOutput.Lines.Length > MAX_TERMINAL_LINES)
                {
                    var lines = _terminalOutput.Lines.Skip(_terminalOutput.Lines.Length - MAX_TERMINAL_LINES + 100).ToArray();
                    _terminalOutput.Lines = lines;
                }
                _terminalOutput.SelectionStart = _terminalOutput.Text.Length;
                _terminalOutput.ScrollToCaret();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Terminal hatası: {ex.Message}");
            }
        }

        private void UpdateMapWithGps(double latitude, double longitude)
        {
            try
            {
                gMap.Position = new PointLatLng(latitude, longitude);
                _markersOverlay.Markers.Clear();
                _markersOverlay.Markers.Add(new GMarkerGoogle(
                    new PointLatLng(latitude, longitude),
                   GMarkerGoogleType.blue_pushpin));
                gMap.Zoom = 15;

                AddToTerminal($"✓ Harita güncellendi: {latitude:F6}, {longitude:F6}");
            }
            catch (Exception ex)
            {
                AddToTerminal($"✗ Harita güncellenemedi: {ex.Message}");
            }
        }

        private void SetParachute1State(bool isActive)
        {
            label_parachute1Active.Visible = isActive;
            label_parachute1Pasif.Visible = !isActive;
        }

        private void SetParachute2State(bool isActive)
        {
            label_parachute2Active.Visible = isActive;
            label_parachute2Pasif.Visible = !isActive;
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                if (_loraPort?.IsOpen == true)
                {
                    _loraPort.Close();
                    _loraPort.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Port kapatma hatası: {ex.Message}");
            }
            base.OnFormClosed(e);
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void button2_Click(object sender, EventArgs e)
        {
            var tsForm = new TestStation();
            tsForm.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            var pForm = new Payload();
            pForm.Show();
        }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged_1(object sender, EventArgs e) { }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void pictureBox1_Click_1(object sender, EventArgs e) { }
        private void label_parachute1Pasif_Click(object sender, EventArgs e) { }
        private void label_parachute1Active_Click(object sender, EventArgs e) { }
        private void label_parachute2Pasif_Click(object sender, EventArgs e) { }
        private void AnaParasutDef_Click(object sender, EventArgs e) { }
        private void ApogeeDef_Click(object sender, EventArgs e) { }
        private void secondParachutePicture_Click(object sender, EventArgs e) { }
        private void label_parachute2Active_Click(object sender, EventArgs e) { }
        private void gMap_Load(object sender, EventArgs e) { }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedPortName))
            {
                MessageBox.Show("Önce bir port seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_loraPort?.IsOpen == true)
                    _loraPort.Close();

                _loraPort = new SerialPort(_selectedPortName, 9600 , Parity.None, 8, StopBits.One)
                {
                    NewLine = "\r\n",
                    ReadTimeout = 500
                };
                _loraPort.DataReceived += LoRaPort_DataReceived;
                _loraPort.Open();

                // ➊ Port açıldı, şimdi dinliyoruz:
                AddToTerminal($"→ {_selectedPortName} portu dinleniyor...");
                lblRxStatus.Text = "Dinleniyor";
                lblRxStatus.ForeColor = Color.Orange;
                _hasReceivedData = false;
                btnConnect.Enabled = false;
            }
            catch (Exception ex)
            {
                AddToTerminal($"✗ Port açma hatası: {ex.Message}");
                MessageBox.Show($"Port açılamadı:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e) { }
        private void lblPacketCounter_Click(object sender, EventArgs e) { }
        private void lblBaroAlt_Click(object sender, EventArgs e) { }
        private void lblTeamID_Click(object sender, EventArgs e) { }
        private void lblRoll_Click(object sender, EventArgs e) { }
        private void lblPitch_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter_1(object sender, EventArgs e) { }
        private void lblGPSAlt_Click(object sender, EventArgs e) { }
        private void lblAccelX_Click(object sender, EventArgs e) { }
        private void lblAccelY_Click(object sender, EventArgs e) { }
        private void lblAccelZ_Click(object sender, EventArgs e) { }
        private void lblLat_Click(object sender, EventArgs e) { }
        private void GyroX_Click_1(object sender, EventArgs e) { }
        private void lblYaw_Click(object sender, EventArgs e) { }
        private void GyroZ_Click(object sender, EventArgs e) { }
        private void GyroY_Click(object sender, EventArgs e) { }
        private void rdoPort1Status_Click(object sender, EventArgs e) { }
        private void btnAutoSend_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged_2(object sender, EventArgs e) { }
        private void cmbComPorts_SelectedIndexChanged(object sender, EventArgs e) {
            _selectedPortName = cmbComPorts.SelectedItem?.ToString();
            AddToTerminal($"→ Seçilen port: {_selectedPortName}");
            btnConnect.Enabled = true;
        }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void rtbDataTerminal_TextChanged(object sender, EventArgs e)
        {
            rtbDataTerminal.SelectionStart = rtbDataTerminal.Text.Length;
            rtbDataTerminal.ScrollToCaret();
        }

        private void groupBoxReceiver_Enter(object sender, EventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void rdoPort3Status_Click(object sender, EventArgs e) { }
    }
}