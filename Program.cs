using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;



namespace tfsapi
{
    class Program
    {

        static void Main(string[] args)
        {
            StreamWriter logTextSPIResults = new StreamWriter("TextSPI-Results.txt");
            string serverPath = @"$/{project}/packages.config";

            // Create a request for the URL.   
            WebRequest request = WebRequest.Create(
              "https://EUCTFS-server/{project}/_apis/tfvc/items?fileName=readme.txt&download=false&api-version=4.1");
            // If required by the server, set the credentials.  
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.  
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            // Display the content.  
            Console.WriteLine(responseFromServer);
            // Clean up the streams and the response.  
            reader.Close();
            response.Close();


            if (responseFromServer.IndexOf("TextSPI") > 0)
            {
                logTextSPIResults.WriteLine(serverPath + " - TextSPI Found");
            };

        }
        
    }

}
