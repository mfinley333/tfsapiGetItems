using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;



namespace tfsapi
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {

                string serverPath = string.Empty;
                StreamWriter logTextSPIResults = new StreamWriter("TextSPI-Results.txt");
                StreamReader reader;
                using (reader = new StreamReader(@"C:\Dev\SYSTFS_TFS_ADMINS\Tools\20180601105559_TFSCollection3.csv"))
                {
                    int i = 0;
                    while (!reader.EndOfStream)
                    {

                        var line = reader.ReadLine();
                        //var values = line.Split(',');
                        string pattern = @"""\s*,\s*""";

                        // input.Substring(1, input.Length - 2) removes the first and last " from the string
                        string[] values = System.Text.RegularExpressions.Regex.Split(
                        line.Substring(1, line.Length - 2), pattern);

                        if (i != 0)
                        {
                            string url = "http://server/tfs/" + values[0] + "/_apis/tfvc/items?path=" + values[2].Replace("&", "%26") + "&api-version=1.0";
                            //url = System.Uri.EscapeUriString(url);
                            // Create a request for the URL.   
                            WebRequest request = WebRequest.Create(url);
                            // If required by the server, set the credentials.  
                            request.UseDefaultCredentials = true;
                            // Get the response.  
                            WebResponse response = null;
                            string responseFromServer = string.Empty;
                            try
                            {
                                response = request.GetResponse();
                                // Display the status.  
                                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                                // Get the stream containing content returned by the server.  
                                Stream dataStream = response.GetResponseStream();
                                // Open the stream using a StreamReader for easy access.  
                                StreamReader reader2 = new StreamReader(dataStream);
                                // Read the content.  
                                responseFromServer = reader2.ReadToEnd();
                                // Display the content.  
                                Console.WriteLine(responseFromServer);
                                // Clean up the streams and the response.  
                                reader2.Close();
                                response.Close();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                            serverPath = values[0] + " - " + values[2];
                            if (responseFromServer.IndexOf("TextSPI") > 0)
                            {
                                logTextSPIResults.WriteLine(serverPath + " - TextSPI Found");
                            };
                        }
                        i++;
                    }
                }

                logTextSPIResults.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

    }

}
