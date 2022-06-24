using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace HeartRateMonitor.Model
{
    class ConnectionToBLE
    {
        private static ConnectionToBLE instance;
        private ConnectionToBLE()
        {}

        public static ConnectionToBLE getInstance()
        {
            if(instance == null)
            {
                instance = new ConnectionToBLE();
            }
            return instance;
        }
        
        public static BluetoothLEDevice bluetoothLE { get; set; }

        public async Task ConnectAsync(DeviceInformation device)
        {
            //Console.WriteLine("Нажмите на любую кнопку для соединения c фитнес браслетом ");
            //Console.ReadKey();
            bluetoothLE = await BluetoothLEDevice.FromIdAsync(device.Id);
            // делаем на всякий случай
            bluetoothLE.Dispose();
            bluetoothLE = await BluetoothLEDevice.FromIdAsync(device.Id);
           // Console.WriteLine(bluetoothLE.ConnectionStatus.ToString());
            if (bluetoothLE.ConnectionStatus == BluetoothConnectionStatus.Disconnected)
            {
                device.Pairing.Custom.PairingRequested += handlerPairingReq;
                var checkPaired = await device.Pairing.Custom.PairAsync(DevicePairingKinds.ConfirmOnly);
                //Console.WriteLine(checkPaired.Status.ToString());
            }
        }
        public BluetoothLEDevice GetBluetoothLE()
        {
            return bluetoothLE;
        }

        private static void handlerPairingReq(DeviceInformationCustomPairing sender, DevicePairingRequestedEventArgs args)
        {
            args.Accept();
        }

        public async void Disconnect(DeviceInformation device)
        {
            bluetoothLE.Dispose();
            await device.Pairing.UnpairAsync();
        }
    }
}
