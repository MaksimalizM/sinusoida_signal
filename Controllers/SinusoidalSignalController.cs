using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SinusoidalSignalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SinusoidalSignalController : ControllerBase
    {
        [HttpPost]
        public IActionResult GenerateSignal([FromBody] SignalParameters parameters)
        {
            if (parameters == null)
            {
                return BadRequest("Параметры сигнала отсутствуют или неверны.");
            }

            var image = CreateSinusoidalImage(parameters);

            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                var imageData = stream.ToArray();

                return File(imageData, "image/png", "sinusoidal_signal.png");
            }
        }

        private Bitmap CreateSinusoidalImage(SignalParameters parameters)
        {
            int width = 800;
            int height = 400;

            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);

            double amplitude = parameters.Amplitude;
            double sampleRate = parameters.SampleRate;
            double frequency = parameters.Frequency;
            int numPeriods = parameters.NumPeriods;

            Pen pen = new Pen(Color.Blue);

            for (int x = 0; x < width; x++)
            {
                double t = x / sampleRate;
                double y = amplitude * Math.Sin(2 * Math.PI * frequency * t);

                int yPixel = height / 2 - (int)(y * height / 2);
                bitmap.SetPixel(x, yPixel, Color.Blue);
            }

            return bitmap;
        }
    }

    public class SignalParameters
    {
        public double Amplitude { get; set; }
        public double SampleRate { get; set; }
        public double Frequency { get; set; }
        public int NumPeriods { get; set; }
    }
}