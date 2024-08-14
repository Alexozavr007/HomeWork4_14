using System.Text;

namespace monitor {
    class Program {

        static int counter = 0;

        static async Task Function() {
            Console.WriteLine($"Task стартує (thread={Thread.CurrentThread.GetHashCode()})");
            for (int i = 0; i < 50; ++i) {
                // Виконання деякої роботи потоком ...
                //Console.WriteLine(++counter);
                Console.Write($"{++counter} ");
            }
            Console.WriteLine($"Task закінчився");

        }

        static async Task Main() {
            Console.OutputEncoding = Encoding.Unicode;
            Task[] tasks = { Function(), Function(), Function() };

            foreach (var task in tasks) {
                await task;
            }

            // Delay
            Console.ReadKey();
        }
    }
}