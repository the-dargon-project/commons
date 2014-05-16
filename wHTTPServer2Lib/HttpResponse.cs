using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wHTTPServer2Lib
{
    public class HttpResponse : IDisposable
    {
        public bool sendHeaders = true;

        #region protocolVersion
        public enum HttpProtocolVersion
        {
            http1_1
        }
        private HttpProtocolVersion httpProtocolVersion = HttpProtocolVersion.http1_1;
        public HttpResponse SetHttpProtocolVersion(HttpProtocolVersion protocolVersion)
        {
            this.httpProtocolVersion = protocolVersion;
            return this;
        }
        #endregion
        #region statusCode
        public enum HttpStatusCode:int
        {
            sc200Ok                 = 200,
            sc201Created            = 201,
            sc202Accepted           = 202,
            sc204NoContent          = 204,
            sc400BadRequest         = 400,
            sc401Unauthorized       = 401,
            sc403Forbidden          = 403,
            sc404NotFound           = 404,
            sc418ImATeapot          = 418,
            sc500ServerError        = 500,
            sc501NotImplemented     = 501,
            sc503ServiceUnavailable = 503
        }
        public string StatusCodeToString(int statusCode)
        {
            switch (statusCode)
            {
                case 200: return "OK";
                case 201: return "Created";
                case 202: return "Accepted";
                case 204: return "No Content";
                case 400: return "Bad Request";
                case 401: return "Unauthorized";
                case 403: return "Forbidden";
                case 404: return "Not Found";
                case 418: return "I'm a Teapot";
                case 500: return "Server Error";
                case 501: return "Not Implemented";
                case 503: return "Service Unavailable";
            }
            return "?";
        }
        private HttpStatusCode statusCode = 0;
        public HttpResponse SetStatusCode(HttpStatusCode value)
        {
            statusCode = value;
            return this;
        }
        #endregion
        #region cacheControl
        public enum CacheControl:byte
        {
            dontCache       = 0x00,
            keepForOneHour  = 0x01,
            keepForOneDay   = 0x02,
            keepForever     = 0x03
        }
        private CacheControl cacheControl = CacheControl.keepForever;
        public HttpResponse SetCacheControl(CacheControl cc)
        {
            if ((byte)this.cacheControl > (byte)cc)
            {
                this.cacheControl = cc;
            }
            return this; 
        }
        #endregion
        #region contentType
        public enum ContentType : byte
        {
            applicationJavascript = 0x01,
            imageGif = 0x02,
            imageJpeg = 0x03,
            imagePng = 0x04,
            imageVndMicrosoftIcon = 0x05,
            textPlain = 0x06,
            textCss = 0x07,
            textHtml = 0x08,
            textJavascript = 0x09,
            xShockwaveFlash = 0x10,
            multipartXMixedReplace = 0x11
        }
        private ContentType contentType = ContentType.textPlain;
        public HttpResponse SetContentType(ContentType value)
        {
            this.contentType = value;
            //System.Windows.Forms.MessageBox.Show(this.contentType.ToString());
            return this;
        }
        #endregion
        #region transferEncoding
        public enum TransferEncoding : byte
        {
            Chunked, Normal
        }
        private TransferEncoding transferEncoding = TransferEncoding.Normal;
        public HttpResponse SetTransferEncoding(TransferEncoding value)
        {
            this.transferEncoding = value;
            return this;
        }
        #endregion
        #region content
        private List<byte> content       = new List<byte>();
        public HttpResponse AppendContent(string s)
        {
            content.AddRange(
                Encoding.ASCII.GetBytes(s)
            );//yes, this is horribly inefficient
            return this;
        }
        public HttpResponse AppendContent(byte[] bytes)
        {
            content.AddRange(
                bytes
            );
            return this;
        }
        #endregion
        #region wTag
        public HttpResponse SetWTag(string tagName, string value)
        {
            content = new List<byte>(
                Encoding.ASCII.GetBytes(
                    Encoding.ASCII.GetString(this.content.ToArray()).Replace(
                        "<w name='" + tagName + "' />", value
                    ).Replace(
                        "<w name='" + tagName + "'/>", value
                    ).Replace(
                        "<w name='" + tagName + "' >", value
                    ).Replace(
                        "<w name='" + tagName + "'>", value
                    )
                )
            );
            return this;
        }
        #endregion
        public StringCollection outCookies = new StringCollection();
        public byte[] ToBytes()
        {
            List<byte> result = new List<byte>();
            if (sendHeaders)
            {
                switch (this.httpProtocolVersion)
                {
                    case HttpProtocolVersion.http1_1:
                        result.AddRange("HTTP/1.1 ".ToBytes()); break;
                }
                result.AddRange((((int)statusCode).ToString() + " ").ToBytes());
                result.AddRange((StatusCodeToString((int)statusCode) + "\r\n").ToBytes());
                result.AddRange("Server: wYvern\r\n".ToBytes());

                if (this.transferEncoding == TransferEncoding.Chunked)
                    result.AddRange("Transfer-Encoding: chunked\r\n".ToBytes());

                switch (this.cacheControl)
                {
                    case CacheControl.dontCache:
                        result.AddRange("Cache-Control: no-cache \r\n".ToBytes()); break;
                    case CacheControl.keepForever:
                        result.AddRange(("Cache-Control: max-age=" + new TimeSpan(365, 0, 0, 0).TotalSeconds.ToString() + "\r\n").ToBytes()); break;
                    case CacheControl.keepForOneDay:
                        result.AddRange(("Cache-Control: max-age=" + new TimeSpan(1, 0, 0, 0).TotalSeconds.ToString() + "\r\n").ToBytes()); break;
                    case CacheControl.keepForOneHour:
                        result.AddRange(("Cache-Control: max-age=" + new TimeSpan(0, 1, 0, 0).TotalSeconds.ToString() + "\r\n").ToBytes()); break;
                }
                string contentTypeString = "text/plain";
                switch (this.contentType)
                {
                    case ContentType.applicationJavascript:
                        contentTypeString = "application/javascript"; break;
                    case ContentType.imageGif:
                        contentTypeString = "image/gif"; break;
                    case ContentType.imageJpeg:
                        contentTypeString = "image/jpeg"; break;
                    case ContentType.imagePng:
                        contentTypeString = "image/png"; break;
                    case ContentType.imageVndMicrosoftIcon:
                        contentTypeString = "image/vnd.microsoft.icon"; break;
                    case ContentType.textCss:
                        contentTypeString = "text/css"; break;
                    case ContentType.textHtml:
                        contentTypeString = "text/html"; break;
                    case ContentType.textJavascript:
                        contentTypeString = "text/javascript"; break;
                    case ContentType.textPlain:
                        contentTypeString = "text/plain"; break;
                    case ContentType.xShockwaveFlash:
                        contentTypeString = "application/x-shockwave-flash"; break;
                    case ContentType.multipartXMixedReplace:
                        contentTypeString = "multipart/x-mixed-replace"; break;
                }
                result.AddRange(("Content-Type: " + contentTypeString + ";\r\n").ToBytes());

                result.AddRange(("Content-Length: " + this.content.Count.ToString() + "\r\n").ToBytes());

                foreach (string key in this.outCookies.Keys)
                {
                    result.AddRange((
                        "Set-Cookie: {0}={1}; path=/; domain={2}\r\n".F(key, this.outCookies[key], g.domain)
                    ).ToBytes());
                }

                result.AddRange("\r\n".ToBytes());
            }
            result.AddRange(this.content.ToArray());

            return result.ToArray();
        }

        void IDisposable.Dispose(){}
    }
}
