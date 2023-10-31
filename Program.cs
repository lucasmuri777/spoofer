using System;
using System.Management;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Runtime.InteropServices;


namespace SpooferLoader
{
    public class Program
    {
        public static string GetHardwareId()
        {
            string hwid = string.Empty;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct"))
            {
                foreach (ManagementObject wmi in searcher.Get())
                {
                    hwid = wmi["UUID"].ToString();
                }
            }
            return hwid;
        }

        public static string GetMotherboardSerial()
        {
            string serial = string.Empty;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard"))
            {
                foreach (ManagementObject wmi in searcher.Get())
                {
                    serial = wmi["SerialNumber"].ToString();
                }
            }
            return serial;
        }
        public static string GetMacAddress()
        {
            string macAddress = string.Empty;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'"))
            {
                foreach (ManagementObject wmi in searcher.Get())
                {
                    macAddress = wmi["MacAddress"].ToString();
                    break;
                }
            }
            return macAddress;
        }
    }
     public static class AppConfig
    {
        private const string HWIDKey = "";
        private const string MotherboardSerialKey = "";
        private const string MacAddressKey = "";

        public static string GetHWID()
        {
            try
            {
                return ConfigurationManager.AppSettings[HWIDKey];
            }
            catch (Exception ex)
            {
                // Registre ou imprima detalhes da exceção para ajudar na depuração
                Console.WriteLine($"Erro ao obter HWID: {ex.Message}");
                return null;
            }
        }

        public static void SaveHWID(string hwid)
        {
            try
            {
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configuration.AppSettings.Settings.Add(HWIDKey, hwid);
                configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                // Registre ou imprima detalhes da exceção para ajudar na depuração
                Console.WriteLine($"Erro ao salvar HWID: {ex.Message}");
            }
        }

        public static string GetMotherboardSerial()
        {
            try
            {
                return ConfigurationManager.AppSettings[MotherboardSerialKey];
            }
            catch (Exception ex)
            {
                // Registre ou imprima detalhes da exceção para ajudar na depuração
                Console.WriteLine($"Erro ao obter número de série da placa-mãe: {ex.Message}");
                return null;
            }
        }

        public static void SaveMotherboardSerial(string serial)
        {
            try
            {
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configuration.AppSettings.Settings.Add(MotherboardSerialKey, serial);
                configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                // Registre ou imprima detalhes da exceção para ajudar na depuração
                Console.WriteLine($"Erro ao salvar número de série da placa-mãe: {ex.Message}");
            }
        }
         public static string GetMacAddress()
    {
        try
        {
            return ConfigurationManager.AppSettings[MacAddressKey];
        }
        catch (Exception ex)
        {
            // Registre ou imprima detalhes da exceção para ajudar na depuração
            Console.WriteLine($"Erro ao obter endereço MAC: {ex.Message}");
            return null;
        }
    }

    public static void SaveMacAddress(string macAddress)
    {
        try
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings.Add(MacAddressKey, macAddress);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        catch (Exception ex)
        {
            // Registre ou imprima detalhes da exceção para ajudar na depuração
            Console.WriteLine($"Erro ao salvar endereço MAC: {ex.Message}");
        }
    }
    }
    

     public static class HWIDSpoofer
    {
        public static bool SpoofHWID()
        {
            try
            {
                // Gere um novo HWID spoofed usando várias informações do sistema
                string spoofedHWID = GenerateRandomHWID();

                // Salve o HWID spoofed nas configurações do aplicativo ou onde for necessário
                AppConfig.SaveHWID(spoofedHWID);

                // Indique que o spoofing foi bem-sucedido
                return true;
            }
            catch (Exception ex)
            {
                // Registre ou imprima detalhes da exceção para ajudar na depuração
                Console.WriteLine($"Erro ao spoofar HWID: {ex.Message}");
                return false;
            }
        }

        private static string GenerateRandomHWID()
        {
            // Combine várias informações do sistema para criar um identificador único
            string cpuId = GetCPUId();
            string ramId = GetRAMId();
            string hddId = GetHDDId();

            // Combine as informações e gere um hash para criar o HWID spoofed
            string combinedData = cpuId + ramId + hddId;
            string spoofedHWID = Hash(combinedData);

            return spoofedHWID;
        }

        private static string GetCPUId()
        {
            string cpuId = "";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
            {
                ManagementObjectCollection collection = searcher.Get();
                foreach (ManagementObject obj in collection)
                {
                    cpuId = obj["ProcessorId"].ToString();
                    break;
                }
            }
            return cpuId;
        }

        private static string GetRAMId()
        {
            string ramId = "";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory"))
            {
                ManagementObjectCollection collection = searcher.Get();
                foreach (ManagementObject obj in collection)
                {
                    ramId = obj["Capacity"].ToString();
                    break;
                }
            }
            return ramId;
        }

        private static string GetHDDId()
        {
            string hddId = "";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive"))
            {
                ManagementObjectCollection collection = searcher.Get();
                foreach (ManagementObject obj in collection)
                {
                    hddId = obj["SerialNumber"].ToString();
                    break;
                }
            }
            return hddId;
        }

        private static string Hash(string data)
        {
            // Implemente a lógica para criar um hash das informações
            // Use um algoritmo de hash seguro, como SHA-256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }

    public static class MotherboardSerialSpoofer
    {
        private static string originalSerialNumber;

        public static bool SpoofMotherboardSerial()
        {
            try
            {
                // Obtém o número de série da placa-mãe original
                originalSerialNumber = GetOriginalMotherboardSerial();

                // Gere um novo número de série da placa-mãe spoofed usando informações do sistema
                string spoofedSerial = GenerateRandomSerial();

                // Aplica o número de série spoofed temporariamente
                SetMotherboardSerial(spoofedSerial);

                // Indique que o spoofing foi bem-sucedido
                return true;
            }
            catch (Exception ex)
            {
                // Registre ou imprima detalhes da exceção para ajudar na depuração
                Console.WriteLine($"Erro ao spoofar número de série da placa-mãe: {ex.Message}");
                return false;
            }
        }

        public static void RestoreOriginalMotherboardSerial()
        {
            try
            {
                // Restaura o número de série original da placa-mãe
                SetMotherboardSerial(originalSerialNumber);
            }
            catch (Exception ex)
            {
                // Registre ou imprima detalhes da exceção para ajudar na depuração
                Console.WriteLine($"Erro ao restaurar número de série original da placa-mãe: {ex.Message}");
            }
        }

        private static void SetMotherboardSerial(string serial)
        {
            // Implementa a lógica para alterar temporariamente o número de série da placa-mãe.
            // No Windows, você pode usar o comando WMIC para alterar o número de série da placa-mãe.

            string command = $"wmic bios set serialnumber={serial}";
            ExecuteCommand(command);
        }

        private static string GetOriginalMotherboardSerial()
        {
            // Implementa a lógica para obter o número de série original da placa-mãe como uma string.
            // No Windows, você pode usar o ManagementObjectSearcher para obter o número de série.

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
            ManagementObjectCollection collection = searcher.Get();
            foreach (ManagementObject obj in collection)
            {
                return obj["SerialNumber"].ToString();
            }

            return "SerialNumberPlaceholder";
        }

        private static void ExecuteCommand(string command)
        {
            // Implementa a lógica para executar um comando no prompt de comando.
            // No Windows, você pode usar o ProcessStartInfo para iniciar um processo cmd.exe.

            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            string result = proc.StandardOutput.ReadToEnd();

            Console.WriteLine(result);
        }

       private static string GenerateRandomSerial()
        {
            // Tamanho do número de série spoofed
            int length = 10;

            // Caracteres que podem ser usados no número de série
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Objeto Random para gerar números aleatórios
            Random random = new Random();

            // Gera um número de série aleatório usando os caracteres permitidos
            string spoofedSerial = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());

            return spoofedSerial;
        }

    }
      public static class MacAddressSpoofer
    {
        public static bool SpoofMacAddress()
        {
            try
            {
                // Gere um novo endereço MAC spoofed
                string spoofedMacAddress = GenerateRandomMacAddress();

                // Execute o comando netsh para alterar o endereço MAC
                ExecuteNetshCommand($"interface set interface \"Sua Interface de Rede\" admin=disable");
                ExecuteNetshCommand($"interface set interface \"Sua Interface de Rede\" admin=enable");
                ExecuteNetshCommand($"interface set interface \"Sua Interface de Rede\" newmac={spoofedMacAddress}");

                Console.WriteLine("Endereço MAC spoofed com sucesso!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao spoofar endereço MAC: {ex.Message}");
                return false;
            }
        }

        private static string GenerateRandomMacAddress()
        {
            Random random = new Random();
            byte[] macBytes = new byte[6];
            random.NextBytes(macBytes);
            macBytes[0] = (byte)(macBytes[0] & (byte)(254)); // Garante que o endereço MAC é unicast e administrado localmente
            return string.Join(":", macBytes);
        }

        private static void ExecuteNetshCommand(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "netsh";
            process.StartInfo.Arguments = command;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }
    }
 
   
    
}

  