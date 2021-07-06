using System;

public class Chto {
    public static void Main(string[] args) {
        for (int i = 0; i < 100; ++i) {
            Console.WriteLine(new Random().Next(0, 5));
        }
    }
}