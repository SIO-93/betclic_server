using System;
using System.Net;
using System.Text;
using System.IO;

class HttpServer
{
    static void Main()
    {
        // Adresse et port du serveur
        string url = "http://localhost:8080/";

        // Créer un objet HttpListener pour écouter les requêtes HTTP
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(url);

        listener.Start();
        Console.WriteLine("Serveur HTTP en écoute sur " + url);

        // Boucle pour accepter les requêtes
        while (true)
        {
            // Attendre une requête HTTP
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            Console.WriteLine("Requête reçue: " + request.HttpMethod + " " + request.Url);

            // Gestion des requêtes GET et POST
            if (request.HttpMethod == "GET")
            {
                HandleGetRequest(response);
            }
            else if (request.HttpMethod == "POST")
            {
                HandlePostRequest(request, response);
            }

            // Fermer la réponse
            response.OutputStream.Close();
        }
    }

    // Gestion des requêtes GET
    static void HandleGetRequest(HttpListenerResponse response)
    {
        string responseString = "<html><body><h1>GET Request Reçue</h1><p>Voici la réponse à votre requête GET</p></body></html>";
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);

        // Définir l'en-tête de la réponse
        response.ContentType = "text/html";
        response.ContentLength64 = buffer.Length;

        // Écrire la réponse dans le flux de sortie
        response.OutputStream.Write(buffer, 0, buffer.Length);
        Console.WriteLine("Réponse GET envoyée.");
    }

    // Gestion des requêtes POST
    static void HandlePostRequest(HttpListenerRequest request, HttpListenerResponse response)
    {
        // Lire les données envoyées dans la requête POST
        string postData;
        using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
        {
            postData = reader.ReadToEnd();
        }

        string responseString = "<html><body><h1>POST Request Reçue</h1><p>Voici les données envoyées :</p><pre>" + postData + "</pre></body></html>";
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);

        // Définir l'en-tête de la réponse
        response.ContentType = "text/html";
        response.ContentLength64 = buffer.Length;

        // Écrire la réponse dans le flux de sortie
        response.OutputStream.Write(buffer, 0, buffer.Length);
        Console.WriteLine("Réponse POST envoyée.");
    }
}
