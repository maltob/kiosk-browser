using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Kiosky.Handlers
{
    class KioskyRequestHandler : RequestHandler

    {
        List<String> _allowedHosts;
        Boolean _allowSubframeNavigation;
        Boolean _showDialogWhenBlocked;
        public KioskyRequestHandler (Settings.Settings settings)
        {
            if (settings.AllowedDomains == null)
                _allowedHosts = new List<string>();
            else
                _allowedHosts = new List<string>(settings.AllowedDomains);


            _allowSubframeNavigation = (!(settings.AllowAllSubframeDomains==false));
            _showDialogWhenBlocked = settings.PopupWhenDomainBlocked==true;

        }
        protected override bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            return false;
        }

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            //Disable forward and back
            if(request.TransitionType.HasFlag( TransitionType.ForwardBack))
            {
                browser.StopLoad();
                return new BlockBrowseRequestHandler();
            }
            else
            {
                return base.GetResourceRequestHandler(chromiumWebBrowser, browser, frame, request, isNavigation, isDownload, requestInitiator, ref disableDefaultHandling);
            }
            
        }

        protected override bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            Uri uri = new Uri(request.Url);

            // If the allowed hosts list isn't empty, or if the host is an allowed host or if we allow all SubFrame navigation and this is a subframe navigation allow us to browse
            if(_allowedHosts.Count == 0 || ((uri != null && _allowedHosts.Contains(uri.Host)) || ((!_allowSubframeNavigation) && request.TransitionType.HasFlag(TransitionType.AutoSubFrame)))) {
                

                return false;
            }
            else
            {
                //Popup a dialog to let the user know this isn't allowed
                if(_showDialogWhenBlocked)
                {
                    frame.EvaluateScriptAsync(String.Format("alert('{0} is not an allowed url')", uri.Host));
                }
                

                //Block the request
                return true;
            }
            
        }

        protected override bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            return true;
        }

        protected override bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return true;
        }

        protected override void OnPluginCrashed(IWebBrowser chromiumWebBrowser, IBrowser browser, string pluginPath)
        {
            
        }

        protected override bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            return true;
        }

        protected override void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status)
        {
            
        }

        protected override void OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            
        }

        protected override bool OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            return true;
        }
    }
}
