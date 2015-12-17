using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Awesomium.Core;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace AwesomeTeste
{
    public partial class Form1 : Form
    {
        #region Fields

        // We are caching the delegates for our synchronous and asynchronous
        // callbacks, to use them easily and avoid continuous casting.
        // private JavascriptAsyncMethodHandler asyncCallback;
        // private JavascriptMethodHandler syncCallback;
        // private JavascriptAsyncMethodHandler _onMyInterval;
        // private JSFunctionAsyncHandler _onDOMMouseMove;
        private JavascriptMethodHandler _JsSyncMethods;
        private JavascriptAsyncMethodHandler _JsAsyncMethods;
        #endregion

        public Form1()
        {
            // Initialize delegates.
            // asyncCallback = (JavascriptAsyncMethodHandler)OnCustomJavascriptAsyncMethod;
            // syncCallback = (JavascriptMethodHandler)OnCustomJavascriptMethod;
            // _onMyInterval = (JavascriptAsyncMethodHandler)onMyInterval;
            // _onDOMMouseMove = (JSFunctionAsyncHandler)onDOMMouseMove;
            _JsSyncMethods = (JavascriptMethodHandler)JsSyncMethods;
            _JsAsyncMethods = (JavascriptAsyncMethodHandler)JsAsyncMethods;

            if (!WebCore.IsInitialized)
            {
                WebConfig webConf = WebConfig.Default;
                webConf.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";

                // Using our executable as a child rendering process, is not
                // available when debugging in VS.
                if (!Process.GetCurrentProcess().ProcessName.EndsWith("vshost"))
                    // We demonstrate using our own executable as child rendering process.
                    webConf.ChildProcessPath = Assembly.GetExecutingAssembly().Location;

                webConf.UserScript = "window2 = window; window = new Object(); window.indexedDB = window2.webkitIndexedDB;";
                WebCore.Initialize(webConf);
            }

            InitializeComponent();

            // Create and cache a WebSession.
            webControl1.WebSession = WebCore.CreateWebSession(
                String.Format("{0}{1}Cache", Path.GetDirectoryName(Application.ExecutablePath), Path.DirectorySeparatorChar),
                new WebPreferences()
                {
                    SmoothScrolling = true,
                    WebGL = true,
                    // Windowed views, support full hardware acceleration.
                    EnableGPUAcceleration = true
                });

            string appDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            char dirSep = System.IO.Path.DirectorySeparatorChar;

            this.webControl1.JavascriptRequest += this.Awesomium_Windows_Forms_WebControl_JavascriptRequest;
            this.webControl1.JavascriptMessage += this.Awesomium_Windows_Forms_WebControl_JavascriptMessage;
            this.webControl1.ConsoleMessage += this.Awesomium_Windows_Forms_WebControl_ConsoleMessage;
            this.webControl1.CertificateError += this.Awesomium_Windows_Forms_WebControl_CertificateError;
            this.webControl1.Crashed += this.Awesomium_Windows_Forms_WebControl_Crashed;
            this.webControl1.DocumentReady += this.Awesomium_Windows_Forms_WebControl_DocumentReady;

            // webControl1.Source = new Uri("http://nparashuram.com/trialtool/#example=/IndexedDB/trialtool/index.html&selected=#prereq&");
            webControl1.Source = new Uri(String.Format("{0}{1}{2}", appDir, dirSep, "File.htm"), UriKind.Absolute);

        }

        private void Awesomium_Windows_Forms_WebControl_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            Debug.Print("{0} at {1}: {2} at '{3}'", e.EventName, e.LineNumber, e.Message, e.Source);
        }

        private void Awesomium_Windows_Forms_WebControl_JavascriptMessage(object sender, JavascriptMessageEventArgs e)
        {
            Debug.Print("JS MSG: {0}", e.Message);
        }

        private void Awesomium_Windows_Forms_WebControl_JavascriptRequest(object sender, JavascriptRequestEventArgs e)
        {
            Debug.Print("JS REQ: {0}", e.Request.ToString());
        }

        private void Awesomium_Windows_Forms_WebControl_Crashed(object sender, CrashedEventArgs e)
        {
            Debug.Print("CRASH: {0}", e.ToString());
        }

        private void Awesomium_Windows_Forms_WebControl_CertificateError(object sender, CertificateErrorEventArgs e)
        {
            Debug.Print("CERT ERR: {0}", e.ToString());
        }

        private void Awesomium_Windows_Forms_WebControl_DocumentReady(object sender, DocumentReadyEventArgs e)
        {
            // Wait for the DOM to be loaded.
            if (e.ReadyState != DocumentReadyState.Loaded)
                return;

            // Make sure the view is alive.
            if (!webControl1.IsLive)
                return;

            // Do not do anything for child windows.
            if (webControl1.ParentView != null)
                return;

            // Do nothing if Javascript is disabled.
            if (!webControl1.IsJavascriptEnabled)
                return;

            // DocumentReady is called in an asynchronous Javascript Execution Context (JEC).
            // In instance of Global available in every asynchronous JEC, provides access to
            // essential objects in the currently loaded page. Using Global we avoid unnecessary
            // synchronous calls to acquire these objects.
            var global = e.Environment;

            // Global supports implicit casting to Boolean.
            if (!global)
                return;

            // You can now access 'window', 'document' etc., through 'global'. All members of Global 
            // are exposed as 'dynamic' so you can work with them using the Dynamic Language Runtime (DLR).
            // Use of Global is demonstrated in the 'ChangeHTML' method in the 'GlobalJavascriptObjectSample'
            // region below. We will not use it here, so that we present the regular CLR API for acquiring
            // and working with JavaScript objects.

            // NOTE THAT STARTING WITH v1.7.5, YOU NO LONGER NEED TO EXPLICITLY DISPOSE OBJECTS CREATED
            // OR ACQUIRED WITHIN A JAVASCRIPT EXECUTION CONTEXT (JEC). THESE ARE AUTOMATICALLY DISPOSED
            // UPON EXITING THE EVENT HANDLER OR ROUTINE ASSOCIATED WITH THE CONTEXT.

            // Gets the HTML Document Object Model (DOM) window object. No explicit cast is needed here.
            // JSValue supports implicit casting.
            JSObject window = webControl1.ExecuteJavascriptWithResult("window");

            // Make sure we have the new window. All synchronous calls of the Awesomium.NET API,
            // return a JSValue. Until version 1.7.5, attempting to cast an invalid ('null' or 'undefined')
            // JSValue to JSObject, would return a null reference (so you had to test against null).
            // Starting with Awesomium.NET 1.7.5, attempting to cast an invalid JSValue to JSObject,
            // will return a JSObject that is equivalent to JSValue.Undefined. Like JSValue, the JSObject type
            // now also contains an implicit operator to Boolean that when called upon this special undefined
            // JSObject or a null JSObject, will return false. So from now on, you can check if you got
            // a valid JSObject, like you would in JavaScript:
            if (!window)
                return;

            // Get the global object we had previously created and assign it to a dynamic.
            if (!CriarObjJs()) return;
            dynamic objGlobal = (JSObject)webControl1.ExecuteJavascriptWithResult("objGlobal");
            if (!objGlobal)
                return;

            ((JSObject)objGlobal).Bind("TesteSync", _JsSyncMethods);
            ((JSObject)objGlobal).BindAsync("TesteAsync", _JsAsyncMethods);
            // objGlobal.TesteSync = _JsSyncMethods;
            // objGlobal.TesteAsync = _JsAsyncMethods;
        }

        // Synchronous JavaScript methods' handler.
        private JSValue JsSyncMethods(object sender, JavascriptMethodEventArgs e)
        {
            // We can have the same handler handling many remote methods.
            // Check here the method that is calling the handler.
            switch (e.MethodName)
            {
                case "TesteSync":
                    // Print the text passed.
                    Debug.Print("SYNC: " + e.Arguments[0]);
                    System.Threading.Thread.Sleep(1000);
                    // Synchronously return a response.
                    return "Message Received!";

                default:
                    MessageBox.Show(String.Format(
                        "OnCustomJavascriptMethod is called for unknown method: {0}", e.MethodName));
                    // We are not bound to this method. Return 'undefined'.
                    return JSValue.Undefined;
            }
        }

        private void JsAsyncMethods(object sender, JavascriptMethodEventArgs e)
        {
            // We can have the same handler handling many remote methods.
            // Check here the method that is calling the handler.
            switch (e.MethodName)
            {
                case "TesteAsync":
                    // Print the text passed.
                    Debug.Print("ASYNC: " + e.Arguments[0]);
                    System.Threading.Thread.Sleep(1000);
                    break;

                default:
                    Debug.Print(String.Format(
                        "OnCustomJavascriptAsyncMethod is called for unknown method: {0}", e.MethodName));
                    break;
            }
        }

        private bool CriarObjJs()
        {
            JSObject window = webControl1.ExecuteJavascriptWithResult("window");
            if (!window["objGlobal"])
            {
            //dynamic objGlobal = (JSObject)webControl1.ExecuteJavascriptWithResult("objGlobal");
            //if (!objGlobal)
            //{
                JSObject objGlobal = webControl1.CreateGlobalJavascriptObject("objGlobal");
                if (!objGlobal)
                    return false;
            }
            return true;
        }
    }
}
