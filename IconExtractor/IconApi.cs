using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing.IconLib;
using System.IO;

namespace IconExtractor
{
    public class IconApi : IDisposable
    {
        private const uint RT_CURSOR = 0x00000001;
        private const uint RT_BITMAP = 0x00000002;
        private const uint RT_ICON = 0x00000003;
        private const uint RT_MENU = 0x00000004;
        private const uint RT_DIALOG = 0x00000005;
        private const uint RT_STRING = 0x00000006;
        private const uint RT_FONTDIR = 0x00000007;
        private const uint RT_FONT = 0x00000008;
        private const uint RT_ACCELERATOR = 0x00000009;
        private const uint RT_RCDATA = 0x00000010;
        private const uint RT_MESSAGETABLE = 0x0000000A;
        private const uint RT_ICON_GROUP = 0x0000000E;

        private const uint LOAD_LIBRARY_AS_DATAFILE = 0x00000002;
        private const uint LR_DEFAULTCOLOR = 0x00000000;

        private const int ERROR_RESOURCE_TYPE_NOT_FOUND = 0x715;

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        private struct ICONDIR
        {
            public ushort Reserved;
            public ushort Type;
            public ushort Count;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct GRPICONDIRENTRY
        {
            public byte Width;
            public byte Height;
            public byte ColorCount;
            public byte Reserved;
            public ushort Planes;
            public ushort BitCount;
            public int BytesInRes;
            public ushort ID;
        }

        public struct ICON_GROUP
        {
            public string ID;
            public List<GRPICONDIRENTRY> icon_groups;
        }

        private delegate bool EnumResNameDelegate(
          IntPtr hModule,
          IntPtr lpszType,
          IntPtr lpszName,
          IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibraryEx( string lpFileName, IntPtr hFile, uint dwFlags );

        [DllImport("kernel32.dll", EntryPoint = "EnumResourceNamesW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool EnumResourceNamesW(IntPtr hModule, uint lpszType, EnumResNameDelegate lpEnumFunc, IntPtr lParam);

        [DllImport("kernel32.dll", EntryPoint = "FindResourceW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr FindResourceW(IntPtr hModule, IntPtr lpName, IntPtr lpType);

        [DllImport("kernel32.dll", EntryPoint = "FindResource", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindResource(IntPtr hModule, string lpName, IntPtr lpType);

        [DllImport("kernel32.dll", EntryPoint = "LoadResource", SetLastError = true)]
        private static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("Kernel32.dll", EntryPoint = "SizeofResource", SetLastError = true)]
        private static extern uint SizeofResource(IntPtr hModule, IntPtr hResource);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LockResource(IntPtr hResData);

        [DllImport("user32.dll")]
        static extern IntPtr CreateIconFromResourceEx(byte[] data, uint cbIconBits, bool fIcon, uint dwVersion, int cxDesired, int cyDesired, uint uFlags);

        private List<ICON_GROUP> icons;
        private IntPtr hMod;

        public IconApi(String filename)
        {
            hMod = LoadLibraryEx(filename, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);
            if (hMod == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        ~IconApi()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            FreeLibrary(hMod);
        }
        
        public IconImage getIconImage(int id)
        {
            string lpName = "#" + id; // To use integer resource ID instead of string, and avoid using MAKEINTRESOURCE, prepend the integer with '#'. For example, to use resource ID 32512, enter "#32512" for lpszName.

            IntPtr hRsrc = FindResource(hMod, lpName, (IntPtr)RT_ICON);
            if (hRsrc == IntPtr.Zero) throw new Win32Exception();

            IntPtr hGlobal = LoadResource(hMod, hRsrc);
            if (hGlobal == IntPtr.Zero) throw new Win32Exception();

            IntPtr hRes = LockResource(hGlobal);
            if (hRes == IntPtr.Zero) throw new Win32Exception();

            uint size = SizeofResource(hMod, hRsrc);
            if (size == 0) throw new Win32Exception();

            byte[] data = new byte[size];
            Marshal.Copy(hRes, data, 0, (int)size);
            MemoryStream ms = new MemoryStream(data);
            return new IconImage(ms, (int)size);
        }

        public Icon getIcon(int id)
        {
            string lpName = "#" + id; // To use integer resource ID instead of string, and avoid using MAKEINTRESOURCE, prepend the integer with '#'. For example, to use resource ID 32512, enter "#32512" for lpszName.

            IntPtr hRsrc = FindResource(hMod, lpName, (IntPtr)RT_ICON);
            if (hRsrc == IntPtr.Zero) throw new Win32Exception();

            IntPtr hGlobal = LoadResource(hMod, hRsrc);
            if (hGlobal == IntPtr.Zero) throw new Win32Exception();

            IntPtr hRes = LockResource(hGlobal);  
            if (hRes == IntPtr.Zero) throw new Win32Exception();

            uint size = SizeofResource(hMod, hRsrc);
            if (size == 0) throw new Win32Exception();

            byte[] data = new byte[size];
            Marshal.Copy(hRes, data, 0, (int)size);

            IntPtr hIcon = CreateIconFromResourceEx(data, size, true, 0x00030000, 0, 0, LR_DEFAULTCOLOR);
            if (hIcon == IntPtr.Zero) throw new Win32Exception();

            return Icon.FromHandle(hIcon);
        }

        public List<ICON_GROUP> getIconGroups()
        {
            icons = new List<ICON_GROUP>();
            if (EnumResourceNamesW(hMod, RT_ICON_GROUP, new EnumResNameDelegate(EnumRes), IntPtr.Zero) == false)
            {
                int errcode = Marshal.GetLastWin32Error();
                if (errcode != ERROR_RESOURCE_TYPE_NOT_FOUND) throw new Win32Exception(errcode);
            }

            return icons;
        }

        private static bool IS_INTRESOURCE(IntPtr value)
        {
            if (((uint)value) > ushort.MaxValue)
                return false;
            return true;
        }

        private static string GET_RESOURCE_NAME(IntPtr value)
        {
            if (IS_INTRESOURCE(value) == true)
                return value.ToString();
            return Marshal.PtrToStringUni((IntPtr)value);
        }

        private bool EnumRes(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam) {
            IntPtr hRsrc = FindResourceW(hModule, lpszName, (IntPtr)RT_ICON_GROUP);
            IntPtr hGlobal = LoadResource(hModule, hRsrc);
            IntPtr hRes = LockResource(hGlobal);

            if (hRes == IntPtr.Zero) return true;

            int sizeDir = Marshal.SizeOf(typeof(ICONDIR));
            int sizeEntry = Marshal.SizeOf(typeof(GRPICONDIRENTRY));

            // Получаем хедер икон группы с количеством иконок
            byte[] data = new byte[sizeDir];
            Marshal.Copy(hRes, data, 0, sizeDir);

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            ICONDIR group = (ICONDIR)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(ICONDIR));

            // Получаем информацию о каждой иконке
            int grpSize = sizeDir + sizeEntry * group.Count;
            data = new byte[grpSize];
            Marshal.Copy(hRes, data, 0, grpSize); // 3 аргумент - это стартовый индекс МАССИВА назначения, а не смещение источника

            data = data.Skip(sizeDir).ToArray(); // Вручную пропускаем хедер

            ICON_GROUP result = new ICON_GROUP
            {
                ID = GET_RESOURCE_NAME(lpszName),
                icon_groups = new List<GRPICONDIRENTRY>()
            };

            for (int i=0;i<group.Count;i++)
            {
                GCHandle handle2 = GCHandle.Alloc(data, GCHandleType.Pinned);
                GRPICONDIRENTRY entry = (GRPICONDIRENTRY)Marshal.PtrToStructure(handle2.AddrOfPinnedObject(), typeof(GRPICONDIRENTRY));
                result.icon_groups.Add(entry);

                data = data.Skip(sizeEntry).ToArray();
            }
            icons.Add(result);
            return true;
        }

    }
}
