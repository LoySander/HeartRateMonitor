using HeartRateMonitor.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace HeartRateMonitor.Model
{
    public class HeartRate : MiBand,INotifyPropertyChanged
    {
        const string HEARTRATE_SRV_ID = "0000180d-0000-1000-8000-00805f9b34fb";
        const string HEARTRATE_CHAR_ID = "00002a39-0000-1000-8000-00805f9b34fb";
        const string HEARTRATE_NOTIFY_CHAR_ID = "00002a37-0000-1000-8000-00805f9b34fb";

        const string SENSOR_SRV_ID = "0000fee0-0000-1000-8000-00805f9b34fb";
        const string SENSOR_CHAR_ID = "00000001-0000-3512-2118-0009af100700";

        byte[] _key;

        GattDeviceService _heartrateService = null;
        GattCharacteristic _heartrateCharacteristic = null;
        GattCharacteristic _heartrateNotifyCharacteristic = null;
        GattDeviceService _sensorService = null;
       
        private static HeartRate instance;

        private string _heartRate;
        public event PropertyChangedEventHandler PropertyChanged;
        private Thread keepHeartrateAliveThread;
        private StringBuilder csvcontent = null;
        private bool isHeartRateStarted;
        private bool isSafeData = false;
        private bool isSound = false;
        private MediaPlayer player;
        private int norm = 0;

        public static HeartRate getInstance()
        {
            if (instance == null)
                instance = new HeartRate();
            return instance;
        }
       
        public HeartRate()
        {
            isHeartRateStarted = false;
             player = new MediaPlayer();
            player.Open(new Uri(@"C:\Users\Stas\source\repos\Test\Volume.mp3", UriKind.RelativeOrAbsolute));
            csvcontent = new StringBuilder();
            csvcontent.AppendLine("Date;Rate");
        }
        public bool IsSafeData
        {
            get { return isSafeData; }
            set
            {
                isSafeData = value;
            }
        }
        public bool IsSound
        {
            get { return isSound; }
            set { isSound = value;
                  
            }
        }
        public int Norm
        {
            get { return norm; }
            set
            {
                norm = value;
            }
        }
        public async Task StartHeartrateMonitorAsync(BluetoothLEDevice bluetoothLE)
        {
            if (isHeartRateStarted)
            {
                return;
            }
            
                GattCharacteristic sensorCharacteristic = null;
                GattDeviceServicesResult sensorService = await bluetoothLE.GetGattServicesForUuidAsync(new Guid(SENSOR_SRV_ID));

                if (sensorService.Status == GattCommunicationStatus.Success && sensorService.Services.Count > 0)
                {
                    _sensorService = sensorService.Services[0];

                    GattCharacteristicsResult characteristic = await _sensorService.GetCharacteristicsForUuidAsync(new Guid(SENSOR_CHAR_ID));

                    if (characteristic.Status == GattCommunicationStatus.Success && characteristic.Characteristics.Count > 0)
                    {
                        sensorCharacteristic = characteristic.Characteristics[0];
                        //BLE.Write(sensorCharacteristic, new byte[] { 0x01, 0x03, 0x19 });
                        Write(sensorCharacteristic, new byte[] { 0x01, 0x03, 0x19 });
                    }
                }

                GattDeviceServicesResult heartrateService = await bluetoothLE.GetGattServicesForUuidAsync(new Guid(HEARTRATE_SRV_ID));

                if (heartrateService.Status == GattCommunicationStatus.Success && heartrateService.Services.Count > 0)
                {
                    _heartrateService = heartrateService.Services[0];

                    GattCharacteristicsResult heartrateNotifyCharacteristic = await _heartrateService.GetCharacteristicsForUuidAsync(new Guid(HEARTRATE_NOTIFY_CHAR_ID));

                    if (heartrateNotifyCharacteristic.Status == GattCommunicationStatus.Success && heartrateNotifyCharacteristic.Characteristics.Count > 0)
                    {
                        GattCommunicationStatus notify = await heartrateNotifyCharacteristic.Characteristics[0].WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);

                        if (notify == GattCommunicationStatus.Success)
                        {
                            _heartrateNotifyCharacteristic = heartrateNotifyCharacteristic.Characteristics[0];
                            _heartrateNotifyCharacteristic.ValueChanged += Characteristic_ValueChanged;
                        }
                    }

                    GattCharacteristicsResult heartrateCharacteristicResult = await _heartrateService.GetCharacteristicsForUuidAsync(new Guid(HEARTRATE_CHAR_ID));

                    if (heartrateCharacteristicResult.Status == GattCommunicationStatus.Success && heartrateCharacteristicResult.Characteristics.Count > 0)
                    {
                        _heartrateCharacteristic = heartrateCharacteristicResult.Characteristics[0];

                        if (true)
                        {
                            Write(_heartrateCharacteristic, new byte[] { 0x15, 0x01, 0x01 });
                            keepHeartrateAliveThread = new Thread(new ThreadStart(RunHeartrateKeepAlive));
                            keepHeartrateAliveThread.Start();
                        }
                        else
                        {
                            Write(_heartrateCharacteristic, new byte[] { 0x15, 0x02, 0x01 });
                        }

                        if (sensorCharacteristic != null)
                        {
                            Write(sensorCharacteristic, new byte[] { 0x02 });
                        }
                    }
                }
            isHeartRateStarted = true;
        }
        private delegate void PlayerStart(); //Делегат, передаваемый в метод Dispatcher.BeginInvoke
        // А этот метод используется в качестве экземпляра передаваемого делегата TimeOutput
        private void StartPlay()
        {
            player.Play();
        }

        public string HeartRateLevel
        {
            get { return _heartRate; }
            set {
                 _heartRate = value;
                if (int.Parse(_heartRate) > norm && isSound == true)
                {
                    player.Dispatcher.BeginInvoke(new PlayerStart(StartPlay));
                }
                csvcontent.AppendLine($"{DateTime.Now.ToString("T")};{ _heartRate}");
                OnPropertyChanged(nameof(HeartRateLevel));
            }
        }

        public void SetNormHeartRate(int age, int heartRateSimple)
        {
            norm = ((int)(((206 - (0.685 * age)) - heartRateSimple) * 0.5 + heartRateSimple));
        }

        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var reader = DataReader.FromBuffer(args.CharacteristicValue);
            var x = reader.ReadInt16();
            HeartRateLevel = x.ToString();
        }
        void RunHeartrateKeepAlive()
        {
            try
            {
                while (_heartrateCharacteristic != null)
                {
                    Write(_heartrateCharacteristic, new byte[] { 0x16 });
                    Thread.Sleep(3000);
                }
            }
            catch (ThreadAbortException) { }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public void StopHeartRate()
        {
            if (isSafeData)
            {
                File.AppendAllText(@"C:\Users\Stas\Desktop\data.csv", csvcontent.ToString());
            }
            if (!isHeartRateStarted)
                return;

            if (keepHeartrateAliveThread != null)
            {
                keepHeartrateAliveThread.Abort();
                keepHeartrateAliveThread = null;
            }

            if (_heartrateCharacteristic != null)
            {
                Write(_heartrateCharacteristic, new byte[] { 0x15, 0x01, 0x00 });
                Write(_heartrateCharacteristic, new byte[] { 0x15, 0x02, 0x00 });
            }

            _heartrateCharacteristic = null;

            _heartrateNotifyCharacteristic = null;

            if (_heartrateService != null)
            {
                _heartrateService.Dispose();
                _heartrateService = null;
            }

            if (_sensorService != null)
            {
                _sensorService.Dispose();
                _sensorService = null;
            }
            HeartRateLevel = 0.ToString();
            isHeartRateStarted = false;
            GC.Collect();
        }
    }

}
