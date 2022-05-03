using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace HeartRateMonitor.Model
{
    class MiBand
    {
        const string AUTH_SRV_ID = "0000fee1-0000-1000-8000-00805f9b34fb";
        const string AUTH_CHAR_ID = "00000009-0000-3512-2118-0009af100700";

        byte[] _key;

        GattDeviceService _heartrateService = null;

        GattCharacteristic _heartrateCharacteristic = null;

        GattCharacteristic _heartrateNotifyCharacteristic = null;

        GattDeviceService _sensorService = null;

      

        public async Task AuthenticateAsync(BluetoothLEDevice bluetoothLE)
        {
            GattDeviceServicesResult service = await bluetoothLE.GetGattServicesForUuidAsync(new Guid(AUTH_SRV_ID));


            if (service.Status == GattCommunicationStatus.Success && service.Services.Count > 0)
            {
                GattCharacteristicsResult characteristic = await service.Services[0].GetCharacteristicsForUuidAsync(new Guid(AUTH_CHAR_ID));

                if (characteristic.Status == GattCommunicationStatus.Success && characteristic.Characteristics.Count > 0)
                {
                    GattCommunicationStatus notify = await characteristic.Characteristics[0].WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);

                    if (notify == GattCommunicationStatus.Success)
                    {
                        characteristic.Characteristics[0].ValueChanged += OnAuthenticateNotify;

                        _key = new SHA256Managed().ComputeHash(Guid.NewGuid().ToByteArray()).Take(16).ToArray();

                        using (var stream = new MemoryStream())
                        {
                            stream.Write(new byte[] { 0x01, 0x08 }, 0, 2);
                            stream.Write(_key, 0, _key.Length);
                            //BLE.Write(characteristic.Characteristics[0], stream.ToArray());
                            Write(characteristic.Characteristics[0], stream.ToArray());
                        }
                    }
                }
            }
        }

        void OnAuthenticateNotify(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            byte[] headers = new byte[3];

            using (DataReader reader = DataReader.FromBuffer(args.CharacteristicValue))
            {
                reader.ReadBytes(headers);

                if (headers[1] == 0x01)
                {
                    if (headers[2] == 0x01)
                    {
                        //BLE.Write(sender, new byte[] { 0x02, 0x08 });
                        Write(sender, new byte[] { 0x02, 0x08 });
                    }
                    else
                    {
                        //Extras.MessageWindow.ShowError("Authentication failed (1)");
                        Console.WriteLine("Authentication failed (1)");
                    }
                }
                else if (headers[1] == 0x02)
                {
                    byte[] number = new byte[reader.UnconsumedBufferLength];
                    reader.ReadBytes(number);

                    using (var stream = new MemoryStream())
                    {
                        stream.Write(new byte[] { 0x03, 0x08 }, 0, 2);

                        byte[] encryptedNumber = EncryptAuthenticationNumber(number);
                        stream.Write(encryptedNumber, 0, encryptedNumber.Length);

                        //BLE.Write(sender, stream.ToArray());
                        Write(sender, stream.ToArray());
                    }
                }
                else if (headers[1] == 0x03)
                {
                    if (headers[2] == 0x01)
                    {
                        //Status = Devices.DeviceStatus.ONLINE_AUTH;
                      //  Console.WriteLine("Device online auth");
                    }
                    else
                    {
                        //Extras.MessageWindow.ShowError("Authentication failed (3)");
                       // Console.WriteLine("Authentication failed (3)");
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
      
    }

   
}
