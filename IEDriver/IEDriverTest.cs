namespace IEAutomation {
	/// <summary>
	/// Summary description for IEDriverTest.
	/// </summary>
	public class IEDriverTest {
		public IEDriverTest() {
		}

		public void TestGoogle() {
			IEDriver driver = new IEDriver();
			driver.Navigate("http://www.google.com");

			driver.SetInputStringValue("q", "Internet Explorer Automation");
            System.Threading.Thread.Sleep(1000);
			driver.ClickButton("btnG");

            System.Threading.Thread.Sleep(5000);

            driver.Close(ref driver);
            System.Console.ReadLine();
		}
	}
}