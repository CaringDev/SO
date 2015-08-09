namespace WiaCopyTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using WIA;

    public static class WiaCopy
    {
        public static IEnumerable<IDeviceInfo> GetAppleDevices()
        {
            return new DeviceManager().DeviceInfos.Cast<IDeviceInfo>().Where(di =>
                di.Type == WiaDeviceType.CameraDeviceType
                && di.Properties["Manufacturer"].get_Value().ToString() == "Apple Inc."
                && di.Properties["Description"].get_Value().ToString() == "Apple iPhone");
        }

        public static IEnumerable<Item> GetImgItems(IDeviceInfo deviceInfo)
        {
            var device = deviceInfo.Connect();
            return device.Items.Cast<Item>().Where(i => i.Properties["Item Name"].get_Value().ToString().StartsWith("IMG"));
        }

        public static void TransferJpgItem(Item item, string path)
        {
            var itemName = item.Properties["Item Name"].get_Value();
            if (!item.Formats.Cast<string>().Contains(FormatID.wiaFormatJPEG))
            {
                Console.WriteLine("Unexpected formats for item {0}, skipping.", itemName);
                return;
            }
            
            var targetName = itemName + ".jpg";
            Directory.CreateDirectory(path);
            ImageFile file = (ImageFile)item.Transfer(FormatID.wiaFormatJPEG);
            Console.WriteLine("Copying {0}", targetName);
            file.SaveFile(Path.Combine(path, targetName));
        }
    }
}
