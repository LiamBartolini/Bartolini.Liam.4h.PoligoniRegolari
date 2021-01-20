using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using System.IO;

namespace Bartolini.Liam._4h.PoligoniRegolari
{
    public partial class MainPage : ContentPage
    {
        // foglio di quaderno 20 cm x 30 cm
        const int LATO_X = 200;
        const int LATO_Y = 300;

        const int MARGINE_SINISTRO = 500;
        const int MARGINE_SUPERIORE = 300;

        SKPath disegnoPath = new SKPath();

        public MainPage()
        {
            InitializeComponent();
        }

        private void Tela(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPath path = new SKPath();

            // Angolo sinistro basso
            SKPoint p = new SKPoint(MARGINE_SINISTRO, MARGINE_SUPERIORE);
            path.MoveTo(p);

            // Angolo destro basso
            p = new SKPoint(MARGINE_SINISTRO + LATO_X, MARGINE_SUPERIORE);
            path.LineTo(p);

            // Angolo destro alto
            p = new SKPoint(MARGINE_SINISTRO + LATO_X, MARGINE_SUPERIORE + LATO_Y);
            path.LineTo(p);

            // Angolo sinistro alto
            p = new SKPoint(MARGINE_SINISTRO, MARGINE_SUPERIORE + LATO_Y);
            path.LineTo(p);
            
            // Angolo sinisto basso
            p = new SKPoint(MARGINE_SINISTRO, MARGINE_SUPERIORE);
            path.LineTo(p);

            SKPaint paintPath = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = new SKColor(168, 224, 131),
                StrokeWidth = 3
            };

            SKPaint paintDisegno = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = new SKColor(112, 230, 128),
                StrokeWidth = 3
            };

            // Sposto il disegno al centro del foglio
            path.MoveTo(info.Width / 2, info.Height/2);
            canvas.DrawPath(path, paintPath);
            canvas.DrawPath(disegnoPath, paintDisegno);

            TelaDaDisegno.InvalidateSurface();
        }

        // Disegnare dentro il foglio
        private void Disegna(object sender, EventArgs e)
        {
            string path = @"disegno.csv";
            StreamReader sr = new StreamReader(path);

            string row = sr.ReadLine();
            
            // Leggo i dati, li converto e li aggiungo al percorso
            while (row != null)
            {
                string[] colonne = row.Split(';');
                int X, Y;

                int.TryParse(colonne[1], out X);
                int.TryParse(colonne[2], out Y);

                SKPoint p = new SKPoint(ConverteInXForm(X), ConverteInYForm(Y));

                if (colonne[0] == "L")
                    disegnoPath.LineTo(p);
                else if (colonne[0] == "M")
                    disegnoPath.MoveTo(p);

                row = sr.ReadLine();
            }

            TelaDaDisegno.InvalidateSurface();
        }

        private int ConverteInXForm(int xUtente)
        {
            int xForm = xUtente + MARGINE_SINISTRO;

            if (xForm < 500)
                xForm += MARGINE_SINISTRO;
            else if (xForm > 700)
                xForm -= MARGINE_SINISTRO;

            return xForm;
        }

        private int ConverteInYForm(int yUtente)
        {            
            int yForm = MARGINE_SUPERIORE + LATO_Y - yUtente;

            if (yForm < 300)
                yForm += MARGINE_SUPERIORE;
            else if (yForm > 600)
                yForm -= MARGINE_SUPERIORE;

            return yForm;
        }
    }
}