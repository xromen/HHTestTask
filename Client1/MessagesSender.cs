using System.Net;

namespace Client1
{
    public static class MessagesSender
    {
        public static bool IsWorking { get { return task != null; } }
        private static CancellationTokenSource ts;
        private static Task task;

        private static Random random = new Random();

        public static void StartSend()
        {
            if (!IsWorking)
            {
                ts = new CancellationTokenSource();
                CancellationToken ct = ts.Token;
                task = Task.Run(() =>
                {
                    int i = 0;
                    while (true)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            break;
                        }

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://api/api/Message");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string json = "{\"text\":\"" + RandomString(random.Next(1, 128)) + "\"," +
                                          "\"serialNumber\":" + i + "}";

                            streamWriter.Write(json);
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                        }
                        i++;
                        Thread.Sleep(10000);
                    }
                });
            }
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void StopSend()
        {
            if (IsWorking)
            {
                ts.Cancel();
                task = null;
            }
        }
    }
}
