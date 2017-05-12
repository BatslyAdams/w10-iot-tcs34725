using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.I2c;
using Windows.Devices.Gpio;
using Windows.Devices.Enumeration;
using Windows.UI;
using System.Diagnostics;

namespace TCS34725Test
{
    class TCS34725
    {
        private I2cDevice _i2c;
        public const byte I2C_DEVICE_ADDRESS = 0x29;    // Determined from datasheet
        public async void Init()
        {
            var i2cSettings = new I2cConnectionSettings(I2C_DEVICE_ADDRESS);
            i2cSettings.BusSpeed = I2cBusSpeed.FastMode;

            var i2cPath = I2cDevice.GetDeviceSelector();
            var i2cStr = await DeviceInformation.FindAllAsync(i2cPath);
            _i2c = await I2cDevice.FromIdAsync(i2cStr[0].Id, i2cSettings);
        }

        private byte[] ReadDevice(byte reg, int count)
        {
            byte[] writeBuffer = new byte[1];
            byte[] readBuffer = new byte[count];

            _i2c.Write(new byte[] { 0x80, reg });

            writeBuffer[0] = (byte)(0x80 | reg);

            _i2c.WriteRead(writeBuffer, readBuffer);

            return readBuffer;
        }

        public byte ReadByte(byte reg)
        {
            return ReadDevice(reg, 1)[0];
        }

        public short ReadShort(byte reg)
        {
            var readBuffer = ReadDevice(reg, 2);
            return (short)(readBuffer[1] | (readBuffer[0] >> 8));
        }

        public void WriteRegister(byte reg, byte val)
        {
            byte[] writeBuffer = new byte[2];

            writeBuffer[0] = (byte)(0x80 | (0x01 << 5) | reg);
            writeBuffer[1] = (byte)(val);

            //_i2c.Write(writeBuffer);
        }
        
        public Color ReadColor()
        {
            Color c;
            c.A = 0xFF; // Set alpha to full for now
            c.R = (byte)(ReadShort(0x16));
            c.G = (byte)(ReadShort(0x18));
            c.B = (byte)(ReadShort(0x1A));
            //Debug.WriteLine(c);
            return c;
        }
    }
}
