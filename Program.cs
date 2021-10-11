
using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


namespace davidapp
{
    class Program
    {
        private static string blob_url = "https://{storage}.blob.core.windows.net/data/NewFile.txt";
        private static string local_blob = "/./NewFile.txt";




        static void Main(string[] args)
        {
            //Use this if you decide to use application objects.
            //First register the application on Active Directory

            try
            {
                var t = Task.Run(() => ApplicationObjects());
                t.Wait();

                Console.WriteLine("File Donwloaded");
                Console.ReadKey();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        static void ApplicationObjects()
        {
            //You can configure these as invironment variables to take them off the application
            //This key has limited access to the storage account.

            string tenantid = "";
            string clientid = "";
            string clientsecret = "secret";

            //zqJ7Q~2UaZIVQNTdWOLBtqgkgjFJnw0kXAMic

            //string env = Environment.GetEnvironmentVariable("windir");

            ClientSecretCredential _client_credential = new ClientSecretCredential(tenantid, clientid, clientsecret);

            Uri blob_uri = new Uri(blob_url);

            BlobClient _blob_client = new BlobClient(blob_uri, _client_credential);
            _blob_client.DownloadTo(local_blob);



        }
        static void UsermanagedIdentity()
        {
            string clientId = "d934849a-a9cc-40c8-b9c2-88ef2e95cbc9";

            //var credential = new ManagedIdentityCredential(clientId: clientId);
            DefaultAzureCredential credential =  new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = clientId });
            Uri blob_uri = new Uri(blob_url);
            BlobClient _blob_client = new BlobClient(blobUri: blob_uri, credential: credential);

            //TODO Download file or implement everything else




        }
        static void SystemmanagedIdentity()
        {
            //This must be in an environment 

            var credential = new ManagedIdentityCredential();

            Uri blob_uri = new Uri(blob_url);
            BlobClient _blob_client = new BlobClient(blobUri: blob_uri, credential: credential);

            _blob_client.DownloadTo(local_blob);



        }
        static async Task containersAsync()
        {
            string connectionString = "";

            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            //Create a unique name for the container
            string containerName = "data";// + Guid.NewGuid().ToString();

            // Create the container and return a container client object

            

            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                Console.WriteLine("\t" + blobItem.Name);
            }
        }
        static void getKV()
        {
            string clientId = "d934849a-a9cc-40c8-b9c2-88ef2e95cbc9";

            DefaultAzureCredential credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = clientId });
            
            Uri KVUri = new Uri("https://davidappvault.vault.azure.net/");

          
            SecretClient client = new SecretClient(vaultUri: KVUri,credential:credential);

            Console.WriteLine(client.GetSecret("tanantid").Value);


        }
    }
}