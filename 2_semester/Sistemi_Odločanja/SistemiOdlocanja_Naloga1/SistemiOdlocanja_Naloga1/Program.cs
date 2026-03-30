using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.ImageSharp;

namespace Naloga1
{
    public class OdlocitveneSisteme
    {
        public double[][] podatki;
        public List<string> moznosti = new List<string>();
        public List<string> navpicno = new List<string>();

        public void Preberi(string datoteka)
        {
            string[] linije = File.ReadAllLines(datoteka);
            string[] glava = linije[0].Split(',');

            for (int i = 1; i < glava.Length; i++)
            {
                moznosti.Add(glava[i].Trim());
            }

            List<List<double>> temp = new List<List<double>>();
            for (int i = 1; i < linije.Length; i++)
            {
                string[] deli = linije[i].Split(',');
                navpicno.Add(deli[0].Trim());

                List<double> vrednosti = new List<double>();
                for (int j = 1; j < deli.Length; j++)
                {
                    vrednosti.Add(double.Parse(deli[j].Trim()));
                }
                temp.Add(vrednosti);
            }

            podatki = temp.Select(x => x.ToArray()).ToArray();
        }

        public int Optimist()
        {
            int best = 0;
            double maxVal = double.MinValue;

            for (int i = 0; i < moznosti.Count; i++)
            {
                double max = 0;
                for (int j = 0; j < navpicno.Count; j++)
                {
                    if (podatki[j][i] > max)
                        max = podatki[j][i];
                }

                if (max > maxVal)
                {
                    maxVal = max;
                    best = i;
                }
            }
            return best;
        }

        public int Pesimist()
        {
            int best = 0;
            double maxMin = double.MinValue;

            for (int i = 0; i < moznosti.Count; i++)
            {
                double min = double.MaxValue;
                for (int j = 0; j < navpicno.Count; j++)
                {
                    if (podatki[j][i] < min)
                        min = podatki[j][i];
                }

                if (min > maxMin)
                {
                    maxMin = min;
                    best = i;
                }
            }
            return best;
        }

        public int Laplace()
        {
            int best = 0;
            double maxAvg = double.MinValue;

            for (int i = 0; i < moznosti.Count; i++)
            {
                double sum = 0;
                //sesteje vse vrednosti

                for (int j = 0; j < navpicno.Count; j++)
                {
                    sum += podatki[j][i];
                }
                double avg = sum / navpicno.Count;

                if (avg > maxAvg)
                {
                    maxAvg = avg;
                    best = i;
                }
            }
            return best;
        }

        public (int best, double vrednost) Savage()
        {
            // max po vrsticah
            double[] najvpicnoMax = new double[navpicno.Count];
            for (int j = 0; j < navpicno.Count; j++)
            {
                najvpicnoMax[j] = 0;
                for (int i = 0; i < moznosti.Count; i++)
                {
                    if (podatki[j][i] > najvpicnoMax[j])
                        najvpicnoMax[j] = podatki[j][i];
                }
            }

            // zracunaj obzalovanje
            double[] maxObzalovanje = new double[moznosti.Count];
            for (int i = 0; i < moznosti.Count; i++)
            {
                maxObzalovanje[i] = 0;
                for (int j = 0; j < navpicno.Count; j++)
                {
                    double obzalovanje = najvpicnoMax[j] - podatki[j][i];  // izracuna vresnost stolpca
                    if (obzalovanje > maxObzalovanje[i])
                        maxObzalovanje[i] = obzalovanje;
                }
            }

            // Poisce minimum
            int best = 0;
            double minVal = maxObzalovanje[0];
            for (int i = 1; i < moznosti.Count; i++)
            {
                if (maxObzalovanje[i] < minVal)
                {
                    minVal = maxObzalovanje[i];
                    best = i;
                }
            }

            return (best, minVal);
        }

        public void Hurwitz()
        {
            Console.WriteLine("\nHurwitzev kriterij:");

            Console.Write("h".PadRight(10));
            //izpise imena stolpecv
            foreach (var stolpec in moznosti)
            {
                Console.Write(stolpec.PadRight(18));
            }
            Console.WriteLine();

            List<double> h_vrednosti = new List<double>();
            List<List<double>> rezultati = new List<List<double>>();
            for (int i = 0; i < moznosti.Count; i++)
            {
                rezultati.Add(new List<double>());
            }

            for (int korak = 0; korak <= 10; korak++)
            {
                double h = korak / 10.0;  // dobimo h
                h_vrednosti.Add(h);

                Console.Write(h.ToString("F1").PadRight(10));

                for (int i = 0; i < moznosti.Count; i++)
                {
                    double min = double.MaxValue;
                    double max = double.MinValue;

                    for (int j = 0; j < navpicno.Count; j++)
                    {
                        if (podatki[j][i] < min) min = podatki[j][i];
                        if (podatki[j][i] > max) max = podatki[j][i];
                    }

                    double vrednost = h * max + (1 - h) * min;

                    rezultati[i].Add(vrednost);

                    Console.Write(vrednost.ToString("F1").PadRight(18));
                }
                Console.WriteLine();
            }

            Narisi(h_vrednosti, rezultati);
        }

        private void Narisi(List<double> h, List<List<double>> res)
        {
            var model = new PlotModel { Title = "Hurwitzev kriterij" };

            var xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "h", Minimum = 0, Maximum = 1 };  // x os
            var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Vrednost" };  // y os
            model.Axes.Add(xAxis);
            model.Axes.Add(yAxis);

            OxyColor[] barve = { OxyColors.Red, OxyColors.Blue, OxyColors.Green, OxyColors.Orange, OxyColors.Purple };

            for (int i = 0; i < res.Count; i++)
            {
                var series = new LineSeries
                {
                    Title = moznosti[i],
                    Color = barve[i % barve.Length],
                    StrokeThickness = 2,
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 4
                };

                for (int j = 0; j < h.Count; j++)
                {
                    series.Points.Add(new DataPoint(h[j], res[i][j]));
                }
                model.Series.Add(series);
            }

            OxyPlot.ImageSharp.PngExporter.Export(model, "graf.png", 1000, 600);
            Console.WriteLine("\nGraf je shranjen ");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            OdlocitveneSisteme sistem = new OdlocitveneSisteme();
            sistem.Preberi(@"C:\sola\solaUM\2_letnik\2_semester\Sistemi_Odločanja\SistemiOdlocanja_Naloga1\SistemiOdlocanja_Naloga1\test_02.csv");

            Console.WriteLine("===================================");
            Console.WriteLine("REZULTATI ODLOČANJA");
            Console.WriteLine("===================================");

            int idx = sistem.Optimist();
            double max = 0;
            for (int j = 0; j < sistem.navpicno.Count; j++)
                if (sistem.podatki[j][idx] > max)                 
                    max = sistem.podatki[j][idx];

            Console.WriteLine($"Optimist:  {sistem.moznosti[idx].PadRight(15)} ({max})");

            idx = sistem.Pesimist();
            double min = double.MaxValue;
            for (int j = 0; j < sistem.navpicno.Count; j++)
                if (sistem.podatki[j][idx] < min) 
                        min = sistem.podatki[j][idx];

            Console.WriteLine($"Pesimist:  {sistem.moznosti[idx].PadRight(15)} ({min})");

            idx = sistem.Laplace();
            double sum = 0;
            for (int j = 0; j < sistem.navpicno.Count; j++)
                sum += sistem.podatki[j][idx];
            double avg = sum / sistem.navpicno.Count;
            Console.WriteLine($"Laplace:   {sistem.moznosti[idx].PadRight(15)} ({avg:G})");

            (idx, double vrednost) = sistem.Savage();
            Console.WriteLine($"Savage:    {sistem.moznosti[idx].PadRight(15)} ({vrednost:G})");

            Console.WriteLine("===================================");
            sistem.Hurwitz();
        }
    }
}