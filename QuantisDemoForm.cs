using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using idQ;

namespace QuantisDemo
{
  public partial class QuantisDemoForm : Form
  {
    private QuantisDeviceType deviceType = QuantisDeviceType.QUANTIS_DEVICE_PCI;
    private uint deviceNumber = 0;

    public QuantisDemoForm()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      // Display library version
      textBoxInfo.Text = string.Concat("Using Quantis library version ", Quantis.GetLibVersion(), Environment.NewLine);
      
      // Count Quantis devices
      int countPci = Quantis.Count(QuantisDeviceType.QUANTIS_DEVICE_PCI);
      int countUsb = Quantis.Count(QuantisDeviceType.QUANTIS_DEVICE_USB);
      textBoxInfo.Text += string.Concat("Found: ", Environment.NewLine);
      textBoxInfo.Text += string.Concat("   - ", countPci, " Quantis PCI/PCIe", Environment.NewLine);
      textBoxInfo.Text += string.Concat("   - ", countUsb, " Quantis USB", Environment.NewLine);
      textBoxInfo.Text += Environment.NewLine;
      
      // Select first available device
      // Note that in a real application, it would be better to let the user choose the device he wants to use...
      if (countPci > 0)
      {
        deviceType = QuantisDeviceType.QUANTIS_DEVICE_PCI;
        deviceNumber = 0;
        textBoxInfo.Text += string.Concat("Using PCI card #", deviceNumber, Environment.NewLine);
      }
      else if (countUsb > 0)
      {
        deviceType = QuantisDeviceType.QUANTIS_DEVICE_USB;
        deviceNumber = 0;
        textBoxInfo.Text += string.Concat("Using USB device #", deviceNumber, Environment.NewLine);
      }
      else
      {
        textBoxInfo.Text = string.Concat("ERROR: No Quantis found!", Environment.NewLine);
        groupBoxData.Enabled = false;
        return;
      }

      // Display information about used device
      try
      {
        Quantis quantis = new Quantis(deviceType, deviceNumber);
        textBoxInfo.Text += string.Concat("   core version:  ", quantis.GetBoardVersion().ToString("X"), Environment.NewLine);
        textBoxInfo.Text += string.Concat("   serial number: ", quantis.GetSerialNumber(), Environment.NewLine);
        textBoxInfo.Text += string.Concat("   manufacturer: ", quantis.GetManufacturer(), Environment.NewLine);
      }
      catch(QuantisException ex)
      {
        textBoxInfo.Text += string.Concat("ERROR while retrieving Quantis information: ", ex.Message, Environment.NewLine);
      }
    }

    private void buttonGenerate_Click(object sender, EventArgs e)
    {
      try
      {
        Quantis quantis = new Quantis(deviceType, deviceNumber);

        // Read random data
        byte[] buffer = quantis.Read(Convert.ToUInt32(numericUpDown1.Value));

        // Convert data to HEX string and display it
        StringBuilder hex = new StringBuilder(buffer.Length * 3);
        foreach (byte b in buffer)
        {
          hex.AppendFormat("{0:x2} ", b);
        }

        textBoxBuffer.Text = hex.ToString().ToUpper();
      }
      catch (QuantisException ex)
      {
        textBoxBuffer.Text += string.Concat("ERROR while retrieving random data: ", ex.Message, Environment.NewLine);
      }
    }
  }
}
