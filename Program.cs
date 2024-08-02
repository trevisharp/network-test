using System.Drawing;
using static System.Windows.Forms.Control;

using Pamella;
using System.Linq;
using System;

App.Open<MainForm>();

public class MainForm : View
{
    string input = "texto";
    RectangleF inputArea;
    PointF[] pts;

    protected override void OnStart(IGraphics g)
    {
        AlwaysInvalidateMode();
        g.SubscribeKeyDownEvent(e =>
        {
            char character = (char)e;
            string value = 
                IsKeyLocked(System.Windows.Forms.Keys.CapsLock) ?
                character.ToString().ToUpper() : character.ToString().ToLower();
            if (char.IsLetterOrDigit(character))
                input += value;
            
            if (e == Input.Space)
                input += " ";

            if (e == Input.Back && input.Length > 0)
                input = input[0..^1];

            if (e == Input.Escape)
                App.Close();
        });
        inputArea = new RectangleF(
            20, .9f * g.Height - 5,
            g.Width - 40, .1f * g.Height - 10
        );
        var centerX = inputArea.X + inputArea.Width / 2;
        var centerY = inputArea.Y + inputArea.Height / 2;
        pts = Enumerable.Range(0, 360)
            .Select(i => MathF.Tau * i / 360)
            .Select(a => new PointF(
                inputArea.Width / 2 * MathF.Sign(MathF.Cos(a)) * MathF.Pow(Math.Abs(MathF.Cos(a)), 0.1f) + centerX,
                inputArea.Height / 2 * MathF.Sign(MathF.Sin(a)) * MathF.Pow(Math.Abs(MathF.Sin(a)), 0.1f) + centerY
            ))
            .ToArray();
    }

    protected override void OnRender(IGraphics g)
    {
        g.Clear(Color.FromArgb(24, 24, 24));

        g.FillPolygon(pts, Brushes.White);

        g.DrawText(inputArea, new Font("arial", 12f),
            StringAlignment.Near, StringAlignment.Near,
            Brushes.Black, input
        );
    }
}