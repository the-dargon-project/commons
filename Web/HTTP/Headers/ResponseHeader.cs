using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItzWarty;

namespace ItzWarty.Web.HTTP.Headers
{
    public enum StatusCode
    {
        Okay,
        NotFound
    }
    public enum ContentType
    {
        textPlain,
        textHtml,
        textJavascript,
        imgPng,
        imgGif,
        imgBmp,
        multipartReplace
    }
    public enum CacheControl
    {
        noCache
    }
    public enum TransferEncoding
    {
        Chunked,
        Whole
    }
    public class ResponseHeader
    {   //This helps you generate a ResponseHeader
        public string value = "";
        public string serverName = "wServer";
        public ResponseHeader()
        {
        }
        public ResponseHeader(StatusCode statCode)
        {
            string sc = "warty screwed up lol";
            if (statCode == StatusCode.Okay) sc = "200 OK";
            else if (statCode == StatusCode.NotFound) sc = "404 Not Found";
            value = "HTTP/1.1 " + sc + "\r\n" +
                    "Server: " + serverName;
        }
        public ResponseHeader statusCode(StatusCode statCode)
        {
            string sc = "warty screwed up lol";
            if (statCode == StatusCode.Okay) sc = "200 OK";
            else if (statCode == StatusCode.NotFound) sc = "404 Not Found";
            value = "HTTP/1.1 " + sc + "\r\n" +
                    "Server: " + serverName;

            return this;
        }
        public ResponseHeader contentLength(int bytes)
        {
            this.value = this.value + "\r\n" +
                         "Content-Length: " + bytes.ToString();
            return this;
        }
        public ResponseHeader contentType(ContentType contentType)
        {
            string ct = "text/plain";
            if (contentType == ContentType.textHtml)
                ct = "text/html";
            else if (contentType == ContentType.textJavascript)
                ct = "text/javascript"; //Really, the standard is application/javascript but ie doesn't support
            else if (contentType == ContentType.imgPng)
                ct = "image/png";
            else if (contentType == ContentType.imgBmp)
                ct = "image/bmp";
            else if (contentType == ContentType.imgGif)
                ct = "image/gif";
            else if (contentType == ContentType.multipartReplace)
                ct = "multipart/x-mixed-replace";
            this.value = this.value + "\r\n" +
                         "Content-Type: " + ct;
            return this;
        }
        public ResponseHeader cacheControl(CacheControl cacheControl)
        {
            string cc = "";
            if (cacheControl == CacheControl.noCache) cc = "no-cache";
            this.value = this.value + "\r\n" +
                         "Cache-Control: " + cc;

            return this;
        }
        public ResponseHeader transferEncoding(TransferEncoding transferEncoding)
        {
            string te = "";
            if (transferEncoding == TransferEncoding.Chunked)
                te = "chunked";
            if (te != "")
                this.value = this.value + "\r\n" +
                            "Transfer-Encoding: " + te;
            return this;
        }
        public ResponseHeader date(DateTime when)
        {
            this.value = this.value + "\r\n" +
                         "Date: " + when.toHTTPTimestamp();
            return this;
        }
        public ResponseHeader setCookie(string cookieName, string cookieValue, DateTime expires)
        {
            this.value = this.value + "\r\n" +
                         "Set-Cookie: " + cookieName + "=" + cookieValue + ";" +
                         "expires= " + expires.toHTTPTimestamp() + ";" +
                         "path=/;";
            return this;
        }
        public ResponseHeader lol(string ResponseHeadername, string value)
        {
            this.value = this.value + "\r\n" +
                         "x-" + ResponseHeadername + ":" + value;
            return this;
        }
        public ResponseHeader addCRNL()
        {
            this.value += "\r\n";
            return this;
        }
        public byte[] toBytes()
        {
            return System.Text.Encoding.ASCII.GetBytes(this.value);
        }
        public ResponseHeader finalize()
        {
            return this.addCRNL().addCRNL();
        }
    }
}
