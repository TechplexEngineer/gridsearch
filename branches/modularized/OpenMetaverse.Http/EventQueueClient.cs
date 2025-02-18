/*
 * Copyright (c) 2007-2008, openmetaverse.org
 * All rights reserved.
 *
 * - Redistribution and use in source and binary forms, with or without
 *   modification, are permitted provided that the following conditions are met:
 *
 * - Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 * - Neither the name of the openmetaverse.org nor the names
 *   of its contributors may be used to endorse or promote products derived from
 *   this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Net;
using System.Threading;
using OpenMetaverse.StructuredData;

namespace OpenMetaverse.Http
{
    public class EventQueueClient
    {
        /// <summary>=</summary>
        public const int REQUEST_TIMEOUT = 1000 * 120;

        public delegate void ConnectedCallback();
        public delegate void EventCallback(string eventName, OSDMap body);

        public ConnectedCallback OnConnected;
        public EventCallback OnEvent;

        public bool Running { get { return _Running; } }

        protected Uri _Address;
        protected bool _Dead;
        protected bool _Running;
        protected HttpWebRequest _Request;

        public EventQueueClient(Uri eventQueueLocation)
        {
            _Address = eventQueueLocation;
        }

        public void Start()
        {
            _Dead = false;

            // Create an EventQueueGet request
            OSDMap request = new OSDMap();
            request["ack"] = new OSD();
            request["done"] = OSD.FromBoolean(false);

            byte[] postData = OSDParser.SerializeLLSDXmlBytes(request);

            _Request = CapsBase.UploadDataAsync(_Address, null, "application/xml", postData, REQUEST_TIMEOUT, OpenWriteHandler, null, RequestCompletedHandler);
        }

        public void Stop(bool immediate)
        {
            _Dead = true;

            if (immediate)
                _Running = false;

            if (_Request != null)
                _Request.Abort();
        }

        void OpenWriteHandler(HttpWebRequest request)
        {
            _Running = true;
            _Request = request;

            Logger.Log.Debug("Capabilities event queue connected");

            // The event queue is starting up for the first time
            if (OnConnected != null)
            {
                try { OnConnected(); }
                catch (Exception ex) { Logger.Log.Error(ex.Message, ex); }
            }
        }

        void RequestCompletedHandler(HttpWebRequest request, HttpWebResponse response, byte[] responseData, Exception error)
        {
            // We don't care about this request now that it has completed
            _Request = null;

            OSDArray events = null;
            int ack = 0;

            if (responseData != null)
            {
                // Got a response
                OSDMap result = OSDParser.DeserializeLLSDXml(responseData) as OSDMap;

                if (result != null)
                {
                    events = result["events"] as OSDArray;
                    ack = result["id"].AsInteger();
                }
                else
                {
                    Logger.Log.Warn("Got an unparseable response from the event queue: \"" +
                        System.Text.Encoding.UTF8.GetString(responseData) + "\"");
                }
            }
            else if (error != null)
            {
                #region Error handling

                HttpStatusCode code = HttpStatusCode.OK;

                if (error is WebException)
                {
                    WebException webException = (WebException)error;

                    if (webException.Response != null)
                        code = ((HttpWebResponse)webException.Response).StatusCode;
                    else if (webException.Status == WebExceptionStatus.RequestCanceled)
                        goto HandlingDone;
                }

                if (error is WebException && ((WebException)error).Response != null)
                    code = ((HttpWebResponse)((WebException)error).Response).StatusCode;

                if (code == HttpStatusCode.NotFound || code == HttpStatusCode.Gone)
                {
                    Logger.Log.InfoFormat("Closing event queue at {0} due to missing caps URI", _Address);

                    _Running = false;
                    _Dead = true;
                }
                else if (code == HttpStatusCode.BadGateway)
                {
                    // This is not good (server) protocol design, but it's normal.
                    // The EventQueue server is a proxy that connects to a Squid
                    // cache which will time out periodically. The EventQueue server
                    // interprets this as a generic error and returns a 502 to us
                    // that we ignore
                }
                else
                {
                    // Try to log a meaningful error message
                    if (code != HttpStatusCode.OK)
                    {
                        Logger.Log.WarnFormat("Unrecognized caps connection problem from {0}: {1}",
                            _Address, code);
                    }
                    else if (error.InnerException != null)
                    {
                        Logger.Log.WarnFormat("Unrecognized caps exception from {0}: {1}",
                            _Address, error.InnerException.Message);
                    }
                    else
                    {
                        Logger.Log.WarnFormat("Unrecognized caps exception from {0}: {1}",
                            _Address, error.Message);
                    }
                }

                #endregion Error handling
            }
            else
            {
                Logger.Log.Warn("No response from the event queue but no reported error either");
            }

        HandlingDone:

            #region Resume the connection

            if (_Running)
            {
                OSDMap osdRequest = new OSDMap();
                if (ack != 0) osdRequest["ack"] = OSD.FromInteger(ack);
                else osdRequest["ack"] = new OSD();
                osdRequest["done"] = OSD.FromBoolean(_Dead);

                byte[] postData = OSDParser.SerializeLLSDXmlBytes(osdRequest);

                // Resume the connection. The event handler for the connection opening
                // just sets class _Request variable to the current HttpWebRequest
                CapsBase.UploadDataAsync(_Address, null, "application/xml", postData, REQUEST_TIMEOUT,
                    delegate(HttpWebRequest newRequest) { _Request = newRequest; }, null, RequestCompletedHandler);

                // If the event queue is dead at this point, turn it off since
                // that was the last thing we want to do
                if (_Dead)
                {
                    _Running = false;
                    Logger.Log.Debug("Sent event queue shutdown message");
                }
            }

            #endregion Resume the connection

            #region Handle incoming events

            if (OnEvent != null && events != null && events.Count > 0)
            {
                // Fire callbacks for each event received
                foreach (OSDMap evt in events)
                {
                    string msg = evt["message"].AsString();
                    OSDMap body = (OSDMap)evt["body"];

                    try { OnEvent(msg, body); }
                    catch (Exception ex) { Logger.Log.Error(ex.Message, ex); }
                }
            }

            #endregion Handle incoming events
        }
    }
}
