namespace WiaCopyTool
{
    using System;
    using System.IO;

    public static class WiaCopyTool
    {
        public static void Main()
        {
            var savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Auto Import");

            foreach (var iPhone in WiaCopy.GetAppleDevices())
            {
                foreach (var imgItem in WiaCopy.GetImgItems(iPhone))
                {
                    WiaCopy.TransferItem(imgItem, Path.Combine(savePath, iPhone.Properties["Name"].get_Value()));
                }
            }
        }
    }
}
