using System;
using IEAutomation;

namespace IEDriver {
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class MainClass {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			// IEDriverTest ieDriverTest = new IEDriverTest();
			// ieDriverTest.TestGoogle();

            // System.Threading.Thread t = new System.Threading.Thread(

            SHDocVw.ShellWindows m_IEFoundBrowsers = new SHDocVw.ShellWindowsClass();
            foreach (SHDocVw.InternetExplorer Browser in m_IEFoundBrowsers)
            {
                // Browser.BeforeNavigate2 += new SHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(Browser_BeforeNavigate2);
                // Browser.NavigateComplete2 += new SHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(Browser_NavigateComplete2);
                // Browser.WindowClosing += new SHDocVw.DWebBrowserEvents2_WindowClosingEventHandler(Browser_WindowClosing);

                

                System.Threading.Thread T = new System.Threading.Thread(() => GetExternal(Browser));
                T.SetApartmentState(System.Threading.ApartmentState.STA);
                T.Start();
            }
            Console.ReadKey();
		}

        static public void GetExternal(SHDocVw.InternetExplorer Browser)
        {
            mshtml.IHTMLDocument2 doc = Browser.Document as mshtml.IHTMLDocument2;
            mshtml.IHTMLWindow2 win = doc.parentWindow as mshtml.IHTMLWindow2;

            Console.WriteLine(win.external.ToString());
        }

        static void Browser_WindowClosing(bool IsChildWindow, ref bool Cancel)
        {
            Console.WriteLine("WindowClosing: ");
        }

        static void Browser_NavigateComplete2(object pDisp, ref object URL)
        {
            Console.WriteLine("NavigateComplete: " + URL);
        }

        static void Browser_BeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
        {
            Console.WriteLine("BeforeNavigate: " + URL);
        }
	}
}