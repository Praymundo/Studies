using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fiddler;
using System.Threading;

namespace FiddlerCoreTest
{
    public partial class Principal : Form
    {
        static List<Fiddler.Session> oAllSessions = new List<Fiddler.Session>();
        static Proxy oSecureEndpoint;
        static string sSecureEndpointHostname;
        static int iSecureEndpointPort;
        static int iPort;
        static FiddlerCoreStartupFlags oFCSF;

        public Principal()
        {
            InitializeComponent();

            sSecureEndpointHostname = "localhost";
            iSecureEndpointPort = 7777;

            // NOTE: In the next line, you can pass 0 for the port (instead of 8877) to have FiddlerCore auto-select an available port
            iPort = 8877;

            Fiddler.FiddlerApplication.SetAppDisplayName("FiddlerCoreTest");
            Fiddler.FiddlerApplication.OnNotification += delegate(object sender, NotificationEventArgs oNEA) {
                this.AdicionarLog("** NotifyUser: " + oNEA.NotifyString);
            };
            Fiddler.FiddlerApplication.Log.OnLogString += delegate(object sender, LogEventArgs oLEA) {
                this.AdicionarLog("** LogString: " + oLEA.LogString);
            };
            Fiddler.FiddlerApplication.BeforeRequest += delegate(Fiddler.Session oS)
            {
                // this.AdicionarLog("Before request for:\t" + oS.fullUrl);
                // In order to enable response tampering, buffering mode MUST
                // be enabled; this allows FiddlerCore to permit modification of
                // the response in the BeforeResponse handler rather than streaming
                // the response to the client as the response comes in.
                oS.bBufferResponse = false;
                Monitor.Enter(oAllSessions);
                oAllSessions.Add(oS);
                Monitor.Exit(oAllSessions);
                oS["X-AutoAuth"] = "(default)";

                /* If the request is going to our secure endpoint, we'll echo back the response.
                
                Note: This BeforeRequest is getting called for both our main proxy tunnel AND our secure endpoint, 
                so we have to look at which Fiddler port the client connected to (pipeClient.LocalPort) to determine whether this request 
                was sent to secure endpoint, or was merely sent to the main proxy tunnel (e.g. a CONNECT) in order to *reach* the secure endpoint.

                As a result of this, if you run the demo and visit https://localhost:7777 in your browser, you'll see

                Session list contains...
                 
                    1 CONNECT http://localhost:7777
                    200                                         <-- CONNECT tunnel sent to the main proxy tunnel, port 8877

                    2 GET https://localhost:7777/
                    200 text/html                               <-- GET request decrypted on the main proxy tunnel, port 8877

                    3 GET https://localhost:7777/               
                    200 text/html                               <-- GET request received by the secure endpoint, port 7777
                */

                if ((oS.oRequest.pipeClient.LocalPort == iSecureEndpointPort) && (oS.hostname == sSecureEndpointHostname))
                {
                    oS.utilCreateResponseAndBypassServer();
                    oS.oResponse.headers.SetStatus(200, "Ok");
                    oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                    oS.oResponse["Cache-Control"] = "private, max-age=0";
                    oS.utilSetResponseBody("<html><body>Request for httpS://" + sSecureEndpointHostname + ":" + iSecureEndpointPort.ToString() + " received. Your request was:<br /><plaintext>" + oS.oRequest.headers.ToString());
                }
            };

            /*
                // The following event allows you to examine every response buffer read by Fiddler. Note that this isn't useful for the vast majority of
                // applications because the raw buffer is nearly useless; it's not decompressed, it includes both headers and body bytes, etc.
                //
                // This event is only useful for a handful of applications which need access to a raw, unprocessed byte-stream
                Fiddler.FiddlerApplication.OnReadResponseBuffer += new EventHandler<RawReadEventArgs>(FiddlerApplication_OnReadResponseBuffer);
            */
            /*
            Fiddler.FiddlerApplication.BeforeResponse += delegate(Fiddler.Session oS) {
                // this.AdicionarLog("{0}:HTTP {1} for {2}", oS.id, oS.responseCode, oS.fullUrl);
                
                // Uncomment the following two statements to decompress/unchunk the
                // HTTP response and subsequently modify any HTTP responses to replace 
                // instances of the word "Microsoft" with "Bayden". You MUST also
                // set bBufferResponse = true inside the beforeREQUEST method above.
                //
                //oS.utilDecodeResponse(); oS.utilReplaceInResponse("Microsoft", "Bayden");
            };*/

            Fiddler.FiddlerApplication.AfterSessionComplete += delegate(Fiddler.Session oS)
            {
                //this.AdicionarLog("Finished session:\t" + oS.fullUrl); 
                this.MudarTitulo("Session list contains: " + oAllSessions.Count.ToString() + " sessions");
            };

            // For the purposes of this demo, we'll forbid connections to HTTPS 
            // sites that use invalid certificates. Change this from the default only
            // if you know EXACTLY what that implies.
            Fiddler.CONFIG.IgnoreServerCertErrors = false;

            // ... but you can allow a specific (even invalid) certificate by implementing and assigning a callback...
            // FiddlerApplication.OnValidateServerCertificate += new System.EventHandler<ValidateServerCertificateEventArgs>(CheckCert);

            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);

            // For forward-compatibility with updated FiddlerCore libraries, it is strongly recommended that you 
            // start with the DEFAULT options and manually disable specific unwanted options.
            oFCSF = FiddlerCoreStartupFlags.Default;

            // E.g. If you want to add a flag, start with the .Default and "OR" the new flag on:
            // oFCSF = (oFCSF | FiddlerCoreStartupFlags.CaptureFTP);

            // ... or if you don't want a flag in the defaults, "and not" it out:
            // Uncomment the next line if you don't want FiddlerCore to act as the system proxy
            // oFCSF = (oFCSF & ~FiddlerCoreStartupFlags.RegisterAsSystemProxy);

            // *******************************
            // Important HTTPS Decryption Info
            // *******************************
            // When FiddlerCoreStartupFlags.DecryptSSL is enabled, you must include either
            //
            //     MakeCert.exe
            //
            // *or*
            //
            //     CertMaker.dll
            //     BCMakeCert.dll
            //
            // ... in the folder where your executable and FiddlerCore.dll live. These files
            // are needed to generate the self-signed certificates used to man-in-the-middle
            // secure traffic. MakeCert.exe uses Windows APIs to generate certificates which
            // are stored in the user's \Personal\ Certificates store. These certificates are
            // NOT compatible with iOS devices which require specific fields in the certificate
            // which are not set by MakeCert.exe. 
            //
            // In contrast, CertMaker.dll uses the BouncyCastle C# library (BCMakeCert.dll) to
            // generate new certificates from scratch. These certificates are stored in memory
            // only, and are compatible with iOS devices.

            // Uncomment the next line if you don't want to decrypt SSL traffic.
            // oFCSF = (oFCSF & ~FiddlerCoreStartupFlags.DecryptSSL);
        }

        private void MudarTitulo(string strTitulo)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.MudarTitulo(strTitulo);
                });
                return;
            }

            this.Text = strTitulo;
        }

        private void AdicionarLog(string strLog)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.AdicionarLog(strLog);
                });
                return;
            }

            if (this.txtLog.Text != "")
            {
                this.txtLog.Text += Environment.NewLine;
            }
            this.txtLog.Text = strLog;
        }

        private void Principal_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!Fiddler.FiddlerApplication.IsStarted())
            {
                this.AdicionarLog(String.Format("Starting {0}...", Fiddler.FiddlerApplication.GetVersionString()));
                Fiddler.FiddlerApplication.Startup(iPort, oFCSF);
                FiddlerApplication.Log.LogFormat("Created endpoint listening on port {0}", iPort);
                FiddlerApplication.Log.LogFormat("Starting with settings: [{0}]", oFCSF);
                FiddlerApplication.Log.LogFormat("Gateway: {0}", CONFIG.UpstreamGateway.ToString());
                // We'll also create a HTTPS listener, useful for when FiddlerCore is masquerading as a HTTPS server
                // instead of acting as a normal CERN-style proxy server.
                oSecureEndpoint = FiddlerApplication.CreateProxyEndpoint(iSecureEndpointPort, true, sSecureEndpointHostname);
                if (null != oSecureEndpoint)
                {
                    FiddlerApplication.Log.LogFormat("Created secure endpoint listening on port {0}, using a HTTPS certificate for '{1}'", iSecureEndpointPort, sSecureEndpointHostname);
                }
                this.AdicionarLog("Started.");
            }
            else
            {
                this.AdicionarLog("Running");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (Fiddler.FiddlerApplication.IsStarted())
            {
                this.AdicionarLog("Stoping...");
                if (null != oSecureEndpoint) oSecureEndpoint.Dispose();
                Fiddler.FiddlerApplication.Shutdown();
                Thread.Sleep(500);
                this.AdicionarLog("Stoped.");
            }
            else
            {
                this.AdicionarLog("Not running");
            }
        }
    }
}
