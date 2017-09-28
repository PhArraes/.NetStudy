using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HanoiService.Web.ViewModels
{
    public class HanoiImageViewModel
    {
        private List<List<int>> _status { get; set; }
        private Random _random { get; set; }
        public HanoiImageViewModel(List<List<int>> status)
        {
            _status = status;
            _random = new Random();
        }

        public MemoryStream ToMemoryStream()
        {
            int width = 1024, height = 768, baseHeight = 18, basePadding = 5;
            int avaWidth = width - 90, hasteWidth = 10, discMaxWidth = avaWidth / 3, currDiscWidth = 0;
            int avaHeight = height - 36;
            int discHeight = 32;
            int numDiscs = 0;
            _status.ForEach(list => numDiscs += list.Count);
            discHeight = Math.Min(discHeight, avaHeight / numDiscs);
            Bitmap hanoi = new Bitmap(width, height);
            Graphics hanoiGraphics = Graphics.FromImage(hanoi);
            //background
            hanoiGraphics.FillRectangle(Brushes.White, 0, 0, width, height);
            //base
            hanoiGraphics.FillRectangle(Brushes.Black, basePadding, height - baseHeight, width - 2 * basePadding, baseHeight);
            //hastes
            int[] posXHaste = { 204, 507, 810 };
            hanoiGraphics.FillRectangle(Brushes.Black, posXHaste[0], baseHeight, hasteWidth, height - baseHeight);
            hanoiGraphics.FillRectangle(Brushes.Black, posXHaste[1], baseHeight, hasteWidth, height - baseHeight);
            hanoiGraphics.FillRectangle(Brushes.Black, posXHaste[2], baseHeight, hasteWidth, height - baseHeight);
            int index = 0;
            foreach (var list in _status)
            {
                list.Reverse();
                int discCount = 0;
                foreach (var disc in list)
                {
                    currDiscWidth = discMaxWidth - (discMaxWidth / numDiscs) * discCount;
                    hanoiGraphics.FillRectangle(PickBrush(),
                        posXHaste[index] + hasteWidth / 2 - currDiscWidth / 2,
                        height - baseHeight * 2 - (discCount * discHeight),
                        currDiscWidth,
                        discHeight);
                    discCount++;
                }
                index++;
            }
            var ms = new MemoryStream();
            hanoi.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            return ms;
        }

        //https://stackoverflow.com/questions/6084398/pick-a-random-brush
        private Brush PickBrush()
        {
            Brush result = Brushes.Transparent;


            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = _random.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);

            return result;
        }
    }
}