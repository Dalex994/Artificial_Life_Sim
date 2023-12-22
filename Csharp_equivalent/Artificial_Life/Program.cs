using System;

namespace ArtificialLife
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Artificial life simulation");
            // Define the canvas to fill the page
            int canvasWidth = Console.WindowWidth;
            int canvasHeight = Console.WindowHeight - 1; // Adjusting for the console header
            // Set coordinates
            void Draw(int x, int y, string c, int s)
            {
                Console.SetCursorPosition(x, y);
                Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), c, true);
                Console.Write(new string(' ', s));
            }
            // Define particles
            var particles = new System.Collections.Generic.List<Particle>();
            Particle Particle(int x, int y, string c)
            {
                return new Particle { X = x, Y = y, Vx = 0, Vy = 0, Color = c };
            }
            // Randomize their position
            Random random = new Random();
            double Random()
            {
                return random.Next(50, 450);
            }
            // Function to permit the creation of the particles
            System.Collections.Generic.List<Particle> Create(int number, string color)
            {
                var group = new System.Collections.Generic.List<Particle>();
                for (int i = 0; i < number; i++)
                {
                    var p = Particle((int)Random(), (int)Random(), color);
                    group.Add(p);
                    particles.Add(p);
                }
                return group;
            }
            // Function that models the interaction forces between them using 2nd and 3rd Newton's laws (mass of 1)
            void Rule(System.Collections.Generic.List<Particle> particles1, System.Collections.Generic.List<Particle> particles2, double g)
            {
                foreach (var a in particles1)
                {
                    double fx = 0;
                    double fy = 0;
                    foreach (var b in particles2)
                    {
                        double dx = a.X - b.X;
                        double dy = a.Y - b.Y;
                        double d = Math.Sqrt(dx * dx + dy * dy);
                        if (d > 0 && d < 80)
                        {
                            double F = g * 1 / d;
                            fx += (F * dx);
                            fy += (F * dy);
                        }
                    }
                    a.Vx = (a.Vx + fx) * 0.5;
                    a.Vy = (a.Vy + fy) * 0.5;
                    a.X += (int)a.Vx;
                    a.Y += (int)a.Vy;
                    if (a.X <= 0 || a.X >= canvasWidth) { a.Vx *= -1; }
                    if (a.Y <= 0 || a.Y >= canvasHeight) { a.Vy *= -1; }
                }
            }

            var yellow = Create(300, "Yellow");
            var red = Create(300, "Red");
            var green = Create(300, "Green");
            var white = Create(300, "White");

            // Creating the function that instances the interactions between those particles
            void Update()
            {
                Rule(red, red, -0.1);
                Rule(yellow, red, 0.01);
                Rule(green, green, -0.32);
                Rule(green, red, 0.17);
                Rule(green, yellow, 0.14);
                Rule(yellow, yellow, -0.15);
                Rule(yellow, green, -0.2);
                Rule(red, green, -0.34);
                Rule(white, white, -0.1);
                Rule(white, yellow, -0.25);
                Rule(white, red, 0.3);
                Rule(white, green, 0.18);
                Rule(yellow, white, 0.11);
                Rule(red, white, -0.35);
                Rule(green, white, -0.18);

                // Shows particles on screen
                Console.Clear();
                for (int i = 0; i < particles.Count; i++)
                {
                    Draw(particles[i].X, particles[i].Y, particles[i].Color, 5);
                }
                System.Threading.Thread.Sleep(100);
                Update();
            }

            Update();
        }
    }

    class Particle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Vx { get; set; }
        public double Vy { get; set; }
        public string Color { get; set; }
    }
}
