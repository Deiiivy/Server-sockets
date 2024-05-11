using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
       
        IPAddress ip = IPAddress.Parse("127.0.0.1"); 
        int port = 8888; 

        
        TcpListener server = new TcpListener(ip, port);

       
        server.Start();
        Console.WriteLine("Servidor iniciado...");

        while (true)
        {
          
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Cliente conectado");

            
            NetworkStream stream = client.GetStream();

           
            byte[] data = new byte[256];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;

            do
            {
                
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.ASCII.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);

     
            string expression = builder.ToString();
            Console.WriteLine("Expresión recibida: " + expression);

           
            string[] expressions = expression.Split('?');
            StringBuilder resultBuilder = new StringBuilder();

            foreach (string expr in expressions)
            {
                string result = EvaluateExpression(expr).ToString();
                resultBuilder.Append(result + ",");
            }

            // Enviar los resultados al cliente
            byte[] response = Encoding.ASCII.GetBytes(resultBuilder.ToString());
            stream.Write(response, 0, response.Length);

       
            client.Close();
            Console.WriteLine("Cliente desconectado");
        }
    }

    static double EvaluateExpression(string expression)
    {
        return Convert.ToDouble(new DataTable().Compute(expression, null));
    }
}
