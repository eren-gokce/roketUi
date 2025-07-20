using System;
using System.IO.Ports;

namespace WindowsFormsApp1
{
    public sealed class SerialPortManager
    {
        private static readonly Lazy<SerialPortManager> _instance =
            new Lazy<SerialPortManager>(() => new SerialPortManager());
        public static SerialPortManager Instance => _instance.Value;

        private SerialPort _port;
        public event EventHandler<string> OnDataReceived;

        private SerialPortManager()
        {
            _port = new SerialPort();
            _port.DataReceived += Port_DataReceived;
        }

        public string[] GetPortNames() => SerialPort.GetPortNames();

        public void Open(string portName, int baudRate = 19200)
        {
            if (_port.IsOpen) _port.Close();
            _port.PortName = portName;
            _port.BaudRate = baudRate;
            _port.Parity = Parity.None;
            _port.DataBits = 8;
            _port.StopBits = StopBits.One;
            _port.Open();
        }

        public void Close()
        {
            if (_port.IsOpen) _port.Close();
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string line = _port.ReadLine();
                OnDataReceived?.Invoke(this, line);
            }
            catch
            {
                // Hata göz ardı edilebilir
            }
        }
    }
}
