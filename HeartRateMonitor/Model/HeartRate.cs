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
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace HeartRateMonitor.Model
{
    class HeartRate : INotifyPropertyChanged
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

        static private MainVM mainVM = null;

        private string _heartRate;
        public event PropertyChangedEventHandler PropertyChanged;
        private Thread keepHeartrateAliveThread;


        public HeartRate(MainVM x)
        {
            mainVM = x;
        }

        public async Task StartHeartrateMonitorAsync(BluetoothLEDevice bluetoothLE)
        {
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
        }
        static async public void Write(GattCharacteristic characteristic, byte[] data)
        {
            using (var stream = new DataWriter())
            {
                stream.WriteBytes(data);

                try
                {
                    GattCommunicationStatus r = await characteristic.WriteValueAsync(stream.DetachBuffer());

                    if (r != GattCommunicationStatus.Success)
                    {
                        Console.WriteLine(string.Format("Unable to write on {0} - {1}", characteristic.Uuid, r));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        byte[] EncryptAuthenticationNumber(byte[] number)
        {
            byte[] r;

            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(number, 0, number.Length);
                        cryptoStream.FlushFinalBlock();
                        r = stream.ToArray();
                    }
                }
            }

            return r;
        }

        public string HeartRateLevel
        {
            get { return _heartRate; }
            set {
                 _heartRate = value;
                OnPropertyChanged(nameof(HeartRateLevel));
            }
        }

     

        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var reader = DataReader.FromBuffer(args.CharacteristicValue);
            var x = reader.ReadInt16();
            HeartRateLevel = x.ToString();
          //  mainVM.ValueHeartRate = x.ToString();
           
            //var flags = reader.ReadByte();
            //var value = reader.ReadByte();
            //Console.WriteLine($"{flags}- {value}");
            //throw new NotImplementedException();
        }
        void RunHeartrateKeepAlive()
        {
            try
            {
                while (_heartrateCharacteristic != null)
                {
                    Write(_heartrateCharacteristic, new byte[] { 0x16 });
                    Thread.Sleep(5000);
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
    }

}
