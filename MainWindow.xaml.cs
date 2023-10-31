using System;
using System.Management;
using System.Windows;
using System.Configuration;

namespace SpooferLoader
{
    public partial class MainWindow : Window
    {
        //public MainWindow()
        //{
          //  InitializeComponent();
        //}

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            string hwid = Program.GetHardwareId();
            string serial = Program.GetMotherboardSerial();
            string macAddress = Program.GetMacAddress();

            macAddressLabel.Content = macAddress;
            hwidLabel.Content = hwid;
            serialLabel.Content = serial;
        }

    private void SpoofarButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Spoofar o HWID
            bool hwidSpoofed = HWIDSpoofer.SpoofHWID();

            // Spoofar o número de série da placa-mãe
            bool motherboardSerialSpoofed = MotherboardSerialSpoofer.SpoofMotherboardSerial();

            // Spoofar o endereço MAC
            bool macAddressSpoofed = MacAddressSpoofer.SpoofMacAddress();

            if (hwidSpoofed && motherboardSerialSpoofed && macAddressSpoofed)
            {
                // Atualizar os rótulos no XAML com os valores spoofados
                //string hwid = AppConfig.GetHWID(); // Obtém o HWID spoofed da configuração
                //string motherboardSerial = AppConfig.GetMotherboardSerial(); // Obtém o número de série spoofed da configuração
                //string macAddress = AppConfig.GetMacAddress(); // Obtém o endereço MAC spoofed da configuração


                string hwid = ConfigurationManager.AppSettings["HWID"];
                string motherboardSerial = ConfigurationManager.AppSettings["MotherboardSerial"];
                string macAddress = ConfigurationManager.AppSettings["MacAddress"];

                hwidLabel.Content = hwid;
                serialLabel.Content =  motherboardSerial;
                macAddressLabel.Content = macAddress;

                MessageBox.Show("HWID, número de série da placa-mãe e endereço MAC spoofados com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Erro ao spoofar HWID, número de série da placa-mãe ou endereço MAC.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao spoofar HWID, número de série da placa-mãe ou endereço MAC: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    }
}
