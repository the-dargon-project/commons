using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItzWarty;
using ItzWarty.ThirdParty;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ItzWarty.LoL
{
    public enum LoLState
    {
        NotInExecution,
        InGame,
        LoadingScreen,
        AirClient
    }
    public static class LoLAnalyzer
    {
        private static ScreenCapture    sc          = new ScreenCapture();
        private static LoLState         lolState    = LoLState.NotInExecution;
        private static IntPtr           lolHwnd     = new IntPtr(0);
        public static void Update()
        {
            lolState = LoLState.NotInExecution;
            WinAPI.EnumWindows(new WinAPI.CallBackPtr(EnumWindowsCallback), 0);
        }

        private static bool EnumWindowsCallback(int hWnd, int lParam)
        {try{
            String title = WinAPI.GetWindowTextAsString(new IntPtr(hWnd)).ToString();
            if (title == "") return true;

            String lowerTitle = title.ToLowerInvariant();
            if (lowerTitle.Contains("league of legends (tm) client"))
            {
                //Console.WriteLine(title);
                lolHwnd = new IntPtr(hWnd);
                AnalyzeInGame(new IntPtr(hWnd));
                return false;
            }
            else if (lowerTitle.Contains("pvp.net") && lowerTitle.Contains("client"))
            {
                lolHwnd = new IntPtr(hWnd);
                lolState = LoLState.AirClient;
                return false;
            }
            return true;
        }catch{return true;}
        }
        private static void AnalyzeInGame(IntPtr hWnd)
        {
            Image gameImage = sc.CaptureWindow(hWnd);
            Bitmap bmp = new Bitmap(gameImage);
            Graphics g = Graphics.FromImage(gameImage);

            //new Form1(bmp).Show();
            //while(true)
            //    Application.DoEvents();
            //Test that (10, 40) to (100, 40) are black
            bool loadingScreen = true;
            for (int x = 10; x <= 100; x++)
            {
                Color px = bmp.GetPixel(x, 40);
                if (px.R != 0 || px.G != 0 || px.B != 0)
                    loadingScreen = false;
            }

            if (loadingScreen)
                lolState = LoLState.LoadingScreen;
            else
                lolState = LoLState.InGame;
        }

        public static LoLState GetLoLState() { return LoLAnalyzer.lolState; }
        public static IntPtr GetLoLHandle() { return LoLAnalyzer.lolHwnd; }

        public static LoLTextOverlay CreateTextOverlay()
        {
            return new LoLTextOverlay(new Bitmap(10, 10));
        }
    }
}
