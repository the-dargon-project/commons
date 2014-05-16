using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using ItzWarty.Web;
using ItzWarty.Web.Globals;
using ItzWarty.Web.HTTP.Headers;


namespace ItzWarty.Web.Pages
{
    public class Error404DirectoryListPage : IPage
    {
        public void InitPlugin() { }
        public void Generate(RequestHeader reqH, ResponseHeader respH, System.Net.Sockets.Socket socket)
        {
            if (reqH.location.Last() != '/')
                reqH.location = reqH.location + '/';

            //The file already doesn't exist, check if we're actually talking about a folder...
            StringBuilder final = new StringBuilder();
            final.Append("<html><head>");
            final.Append("<style> table{ border-width: 0px; border-style: outset; border-color: gray; border-collapse: collapse; background-color: white; } </style>");
            //final.Append("<style> td.title{ border-width: 1px; padding: 1px; border-style: inset; border-color: gray; background-color: white; } </style>");
            final.Append("<style> td.title{ background-color:#EEEEEE;} </style>");
            final.Append("</head><body>");
            final.Append("<font size='+3'><b>Directory Listing: {0}</b></font></br>".F(reqH.requestedLocation));
            if (reqH.requestedLocation.ToLower() != reqH.location.ToLower())
                final.Append("(which in reality redirects to {0})<br>".F(reqH.location));

            /* Breadcrumbs */
            string[] pathToMe = reqH.requestedLocation.Split(new string[] { "/" }, StringSplitOptions.None);
            for (int i = 0; i < pathToMe.Length-1; i++)
            {
                string relativeLoc = "";
                for (int j = i+1; j < pathToMe.Length - 1; j++)
                {
                    relativeLoc += "../";
                }
                if (i == 0)
                    pathToMe[0] = "Root";
                if (relativeLoc != "")
                    final.Append(
                        "<a href='" + relativeLoc + "'>" + pathToMe[i] + "</a>" + " &raquo; "
                    );
                else
                    final.Append(
                        "<b>" + pathToMe[i] + "</b>"
                    );
            }

            final.Append("<table style='border:1px; border-spacing:0px; border-collapse:collapse; width:100%;'><tr><td style='width:200px' class='title'>Type</td><td class='title'>fileName</td><td style='width:200px' class='title'>Last Update</td></tr>");
            //final.Append("<tr><td colspan='3' style='background-color:#000000'></td></tr>");

            //Redirected Directories
            List<string> redirectedDirectories = g.redirects.Keys.ToList();
            for (int i = 0; i < redirectedDirectories.Count; i++)
            {
                if (redirectedDirectories[i].ToLower().Replace(reqH.location.ToLower(), "").IndexOf("/") == -1 &&
                    redirectedDirectories[i].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Length == 
                        reqH.location.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Length + 1)
                {
                    //It's a directory in this directory.  Yay.
                    string dirName = g.redirects[redirectedDirectories[i]].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Last();
                    string tableRowHTML = "<tr><td>redirect dir</td><td><a href='{0}'>{1}</a></td><td>{2}</td></tr>".F("./" + dirName + '/', dirName, "");
                    final.Append(tableRowHTML);
                }
            }

            //Directory view...
            string[] directories = Directory.GetDirectories(g.webDir + reqH.location.Replace('/', '\\'));
            foreach (string dirPath in directories)
            {
                //Make sure no hidden files...
                string dirName = dirPath.Substring(g.webDir.Length + reqH.location.Length).Replace('\\', '/');
                if (dirName[0] != '.')
                {
                    string tableRowHTML = "<tr><td>dir</td><td><a href='{0}'>{1}</a></td><td>{2}</td></tr>".F("./"+dirName+'/', dirName, "");

                    final.Append(tableRowHTML);
                }
            }

            //files
            string[] files = Directory.GetFiles(g.webDir + reqH.location.Replace('/', '\\'));
            foreach (string filePath in files)
            {
                //Make sure no hidden files...
                string fileName = filePath.Substring(g.webDir.Length + reqH.location.Length);
                if (fileName[0] != '.')
                {
                    string tableRowHTML = "<tr><td>file</td><td><a href='{0}'>{1}</a></td><td>{2}</td></tr>".F(fileName, fileName, new FileInfo(filePath).LastWriteTime.ToString());

                    final.Append(tableRowHTML);
                }
            }

            final.Append("</table></body></html>");
            
            socket.Send(
                respH
                    .statusCode(StatusCode.NotFound)
                    .transferEncoding(TransferEncoding.Whole)
                    .cacheControl(CacheControl.noCache)
                    .contentType(ContentType.textHtml)
                    .date(DateTime.Now)
                    .finalize()
                    .toBytes()
            );

            socket.Send(
                System.Text.Encoding.ASCII.GetBytes(
                    final.ToString()
                )
            );

            final.Clear();
            final = null;
        }
    }
}
