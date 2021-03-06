using System.Reflection.PortableExecutable;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using System.Net.NetworkInformation;
using System.Net.Sockets;

using System.Drawing;

using System.IO;

using QRCoder;
using Microsoft.AspNetCore.Components;

namespace SharerBlazorServer.Pages
{
    // data structure
    public class IpConfig
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public string UnsafeUrl
        {
            get
            {
                return $"http://{this.Ip}";
            }
        }
        public string SafeUrl
        {
            get
            {
                return $"https://{this.Ip}";
            }
        }
    }

    // data structure
    public class IpQr
    {
        public IpConfig Config { get; set; }
        public string UnsafeQr
        {
            get
            {
                return GetBase64Qr(this.Config.SafeUrl);
            }
        }
        public string SafeQr
        {
            get
            {
                return GetBase64Qr(this.Config.SafeUrl);
            }
        }

        private string GetBase64Qr(string content)
        {
            var bitmapBytes = BitmapToBytes(GetQR(content)); //Convert bitmap into a byte array
            string base64Image = Convert.ToBase64String(bitmapBytes);
            return "data:image/gif;base64," + base64Image;
        }

        private Bitmap GetQR(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }

        // This method is for converting bitmap into a byte array
        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }

    public partial class QrCode
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public List<IpQr> IpQrs { set; get; }

        public QrCode()
        {
            IpQrs = new List<IpQr>();
        }

        protected override async Task OnInitializedAsync()
        {
            List<IpConfig> ipConfigs = GetThisIPConfig();
            foreach (IpConfig ipConfig in ipConfigs)
            {
                IpQr pack = new IpQr();
                pack.Config = ipConfig;
                IpQrs.Add(pack);
            }
        }

        private List<IpConfig> GetThisIPConfig()
        {
            if (!this.NavigationManager.BaseUri.Contains("localhost")
            && !this.NavigationManager.BaseUri.Contains("127.0.0.1")
            && !this.NavigationManager.BaseUri.Contains("::1"))
            {
                return new List<IpConfig>();
            }
            List<IpConfig> configs = new List<IpConfig>();
            // order interfaces by speed and filter out down and loopback
            // take first of the remaining
            var upInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                // .OrderByDescending(c => c.Speed)
                .Where(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up)
                .ToList();
            if (upInterfaces != null)
            {
                foreach (NetworkInterface upInterface in upInterfaces)
                {
                    IpConfig config = new IpConfig();
                    config.Name = upInterface.Name;

                    var props = upInterface.GetIPProperties();
                    // get first IPV4 address assigned to this interface
                    var firstIpV4Address = props.UnicastAddresses
                        .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                        .Select(c => c.Address)
                        .FirstOrDefault();
                    config.Ip = firstIpV4Address.ToString();
                    configs.Add(config);
                }
            }

            return configs;
        }
    }
}
