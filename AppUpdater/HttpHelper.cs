using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace AppUpdater
{
    public class RequestState
    {
        public const int BuffSize = 2048;
        public byte[] buffer;
        public List<byte> data;
        public HttpWebRequest request;
        public HttpWebResponse response;
        public Stream stream;

        public RequestState()
        {
            request = null;
            response = null;
            stream = null;
            buffer = new byte[BuffSize];
            data = new List<byte>();
        }
    }

    public class RequestItem
    {
        private ManualResetEvent allDone = new ManualResetEvent(false);
        public static int doneCount = 0;
        public static int errorCount = 0;
        public static int timeoutCount = 0;
        public static int recvCount = 0;

        private static object lock_done = new object();
        private static object lock_error = new object();
        private static object lock_timeout = new object();
        private static object lock_recv = new object();

        private HttpWebRequest m_Request = null;

        private List<byte> m_data = new List<byte>();
        public List<byte> Data
        {
            get
            {
                return m_data;
            }
        }

        private string m_session = string.Empty;
        public string Session
        {
            get
            {
                return m_session;
            }
        }

        public RequestItem(string requestUrl, int timeout)
        {
            m_Request = WebRequest.Create(requestUrl) as HttpWebRequest;
            m_Request.Method = WebRequestMethods.Http.Get;
            m_Request.Timeout = timeout;
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                RequestState state = ar.AsyncState as RequestState;
                Stream stream = state.stream;
                int read = stream.EndRead(ar);

                if (read > 0)
                {
                    byte[] buffer = new byte[read];
                    Array.Copy(state.buffer, buffer, read);
                    state.data.AddRange(buffer);
                    IAsyncResult asyncRead = stream.BeginRead(state.buffer, 0, RequestState.BuffSize, new AsyncCallback(ReadCallback), state);
                    return;
                }
                else
                {
                    if (state.data.Count > 1)
                    {
                        lock (lock_recv)
                        {
                            recvCount += state.data.Count;
                        }

                        m_data.AddRange(state.data);

                        lock (lock_done)
                        {
                            doneCount++;
                        }
                    }

                    stream.Close();
                }
            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            allDone.Set();
        }

        private void ResponseCallback(IAsyncResult ar)
        {
            try
            {
                RequestState state = ar.AsyncState as RequestState;
                HttpWebRequest request = state.request;
                state.response = request.EndGetResponse(ar) as HttpWebResponse;

                Stream stream = state.response.GetResponseStream();
                state.stream = stream;

                IAsyncResult asyncRead = stream.BeginRead(state.buffer, 0, RequestState.BuffSize, new AsyncCallback(ReadCallback), state);
                return;
            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            allDone.Set();
        }

        private void TimeoutCallback(object state, bool timeOut)
        {
            if (timeOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                    lock (lock_timeout)
                    {
                        timeoutCount++;
                    }
                }
            }
        }

        public void Get(List<string> header)
        {
            try
            {
                RequestState state = new RequestState();
                allDone.Reset();
                state.request = m_Request;

                if (header != null && header.Count != 0)
                {
                    foreach (string cookie in header)
                    {
                        m_Request.Headers.Add(cookie);
                    }
                }

                IAsyncResult result = m_Request.BeginGetResponse(new AsyncCallback(ResponseCallback), state);
                
                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), m_Request, m_Request.Timeout, true);
                allDone.WaitOne();

                if (state.response != null)
                {
                    state.response.Close();
                }
            }
            catch (WebException webEx)
            {
                lock (lock_error)
                {
                    errorCount++;
                }
                Console.WriteLine(webEx.Message);
            }
            catch (Exception ex)
            {
                lock (lock_error)
                {
                    errorCount++;
                }
                Console.WriteLine(ex.Message);
            }
        }
    }
}
