using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.PolygonClipper;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing.Processing.Processors.Drawing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using SixLabors.Fonts;
using System.Net;
using System.IO;

namespace PiCity
{
    class Program
    {
        static void Main(string[] args)
        {
            bool onceRendered = false;
            if (Console.ReadLine() == "render" && onceRendered == false)
            {
                onceRendered = true;
                Draw();

            }
            static void Draw()
            {
                Image<Rgba32> image = new(512, 64);

                Image<Rgba32> background = new(512, 64);
                for (int x = 0; x <= 512-64; x +=64)
                {
                    background.Mutate(
                    ctx => ctx
                    .DrawImage(Image.Load<Rgba32>("./Files/niebo.png"), new Point(x, 0), 1)
                    );
                }
                background.SaveAsPng("./Files/background.png");
                image.Mutate(ctx => ctx.DrawImage(background,1));
                char[] piNumbers = CalculatePi(21).ToCharArray();

                int[] piInts = new int[piNumbers.Length];
                for (int x = 0; x < piNumbers.Length; x++)
                {
                    char ch = piNumbers[x];
                    int intVal = ch - '0';
                    piInts[x] = intVal;
                }
                Image<Rgba32>[] thinBuilding = new Image<Rgba32>[4] { Image.Load<Rgba32>("./Files/cienki1.png"), Image.Load<Rgba32>("./Files/cienki2.png"), Image.Load<Rgba32>("./Files/cienki3.png"), Image.Load<Rgba32>("./Files/cienki4.png") };
                Image<Rgba32> thinRoof = Image.Load<Rgba32>("./Files/cienkiD.png");
                Image<Rgba32>[] thickBuilding = new Image<Rgba32>[3] { Image.Load<Rgba32>("./Files/gruby1.png"), Image.Load<Rgba32>("./Files/gruby2.png"), Image.Load<Rgba32>("./Files/gruby3.png") };
                Image<Rgba32> thickRoof = Image.Load<Rgba32>("./Files/grubyD.png");
                Random random = new(((int)Math.PI));
                //void Something(int)
                //{
                //    new PointF()
                //}
                int ActualX = 0;
                int ActualY = 60;
                Rgba32 colorrr = new(0, 3, 80);
                image.Mutate(
                                ctx => ctx
                                .DrawLines(new() {GraphicsOptions = new() {Antialias = false } },new Rgba32(0, 3, 80), 4 ,new PointF[2] { new(0,62), new(512,62) })
                                );
                for (int x = 0; x < piInts.Length; x++)
                {
                    ActualY = 60;
                    if (piInts[x] > 0)
                    {
                        if (random.Next(0, 2) == 1) //thick
                        {
                            for (int t = 0; t < piInts[x]; t++)
                            {
                                var thisThickBuliding = thickBuilding[random.Next(0, 3)];
                                ActualY -= thisThickBuliding.Height;
                                image.Mutate(
                                ctx => ctx
                                .DrawImage(thisThickBuliding, new Point(ActualX, ActualY), 1)
                                );
                            }
                            ActualY -= thickRoof.Height;
                            image.Mutate(
                                ctx => ctx
                                .DrawImage(thickRoof, new Point(ActualX, ActualY), 1)
                                );
                            ActualX += 24;
                        }
                        else //thin
                        {
                            for (int t = 0; t < piInts[x]; t++)
                            {
                                var thisThinBuliding = thinBuilding[random.Next(0, 4)];
                                ActualY -= thisThinBuliding.Height;
                                image.Mutate(
                                ctx => ctx
                                .DrawImage(thisThinBuliding, new Point(ActualX, ActualY), 1)
                                );
                            }
                            ActualY -= thinRoof.Height;
                            image.Mutate(
                                ctx => ctx
                                .DrawImage(thinRoof, new Point(ActualX, ActualY), 1)
                                );
                            ActualX += 16;
                        }
                    }


                }
                image.SaveAsPng("./Files/image.png");

            }

        }
        public static string CalculatePi(int digits)
        {
            digits++;

            uint[] x = new uint[digits * 10 / 3 + 2];
            uint[] r = new uint[digits * 10 / 3 + 2];

            uint[] pi = new uint[digits];

            for (int j = 0; j < x.Length; j++)
                x[j] = 20;

            for (int i = 0; i < digits; i++)
            {
                uint carry = 0;
                for (int j = 0; j < x.Length; j++)
                {
                    uint num = (uint)(x.Length - j - 1);
                    uint dem = num * 2 + 1;

                    x[j] += carry;

                    uint q = x[j] / dem;
                    r[j] = x[j] % dem;

                    carry = q * num;
                }


                pi[i] = (x[x.Length - 1] / 10);


                r[x.Length - 1] = x[x.Length - 1] % 10; ;

                for (int j = 0; j < x.Length; j++)
                    x[j] = r[j] * 10;
            }

            var result = "";

            uint c = 0;

            for (int i = pi.Length - 1; i >= 0; i--)
            {
                pi[i] += c;
                c = pi[i] / 10;

                result = (pi[i] % 10).ToString() + result;
            }

            return result;
        }
    }
}
