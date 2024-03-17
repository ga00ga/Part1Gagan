using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsAppGagan11
{
    public partial class MainForm : Form
    {
        public Graphics graphics;
        public Pen pen = new Pen(Color.Black);
        public PointF currentPosition = PointF.Empty;
        public bool fillShape = false;

        public MainForm()
        {
            InitializeComponent();
            graphics = drawingPanel.CreateGraphics();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            ExecuteProgram();
        }

        public void button3_Click(object sender, EventArgs e)
        {
            LoadProgramFromFile();
        }

        public void button4_Click(object sender, EventArgs e)
        {
            SaveProgramToFile();
        }

        public void ExecuteProgram()
        {
            ClearDrawingArea();
            currentPosition = PointF.Empty;

            List<string> allCommands = new List<string>();
            allCommands.AddRange(textBox1.Lines);
            allCommands.AddRange(txtProgram.Lines);

            foreach (string cmd in allCommands)
            {
                try
                {
                    ParseAndExecuteCommand(cmd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error executing command: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        public void ParseAndExecuteCommand(string command)
        {
            string[] parts = command.Split(' ');

            string keyword = parts[0].ToLower();
            switch (keyword)
            {
                case "position":
                    MoveTo(parts);
                    break;
                case "pen":
                    SetPenColor(parts);
                    break;
                case "draw":
                    DrawTo(parts);
                    break;
                case "rectangle":
                    DrawRectangle(parts);
                    break;
                case "circle":
                    DrawCircle(parts);
                    break;
                case "triangle":
                    DrawTriangle();
                    break;
                case "clear":
                    ClearDrawingArea();
                    break;
                case "reset":
                    ResetPenPosition();
                    break;
                case "fill":
                    SetFillMode(parts);
                    break;
                default:
                    throw new ArgumentException($"Invalid command: {command}");
            }
        }

        public void MoveTo(string[] parts)
        {
            float x, y;
            if (parts.Length >= 3 && float.TryParse(parts[1], out x) && float.TryParse(parts[2], out y))
            {
                currentPosition = new PointF(x, y);
            }
            else
            {
                throw new ArgumentException("Invalid input command format for position.");
            }
        }

        public void SetPenColor(string[] parts)
        {
            string colorName = parts[1];
            pen.Color = Color.FromName(colorName);
        }

        public void DrawTo(string[] parts)
        {
            float x = float.Parse(parts[1]);
            float y = float.Parse(parts[2]);
            //currentPosition = new PointF(x, y);
            MoveTo(new string[] { "position", x.ToString(), y.ToString() });
        }

        public void DrawRectangle(string[] parts)
        {
            int width = int.Parse(parts[1]);
            int height = int.Parse(parts[2]);

            MoveTo(parts); // Call MoveTo method before drawing the rectangle

            if (fillShape)
                graphics.FillRectangle(new SolidBrush(pen.Color), currentPosition.X, currentPosition.Y, width, height);
            else
                graphics.DrawRectangle(pen, currentPosition.X, currentPosition.Y, width, height);
        }

        public void DrawCircle(string[] parts)
        {
            int radius = int.Parse(parts[1]);

            MoveTo(parts); // Call MoveTo method before drawing the circle

            if (fillShape)
                graphics.FillEllipse(new SolidBrush(pen.Color), currentPosition.X - radius, currentPosition.Y - radius, radius * 2, radius * 2);
            else
                graphics.DrawEllipse(pen, currentPosition.X - radius, currentPosition.Y - radius, radius * 2, radius * 2);
        }

        public void DrawTriangle()
        {
            // Implement draw triangle functionality
            PointF[] points = new PointF[]
            {
        new PointF(currentPosition.X, currentPosition.Y),
        new PointF(currentPosition.X + 50, currentPosition.Y + 100),
        new PointF(currentPosition.X - 50, currentPosition.Y + 100)
            };

            // Calculate the bounding box of the triangle
            float minX = points.Min(p => p.X);
            float minY = points.Min(p => p.Y);
            float maxX = points.Max(p => p.X);
            float maxY = points.Max(p => p.Y);

            // Ensure that the triangle is fully visible within the drawing panel
            if (minX < 0 || minY < 0 || maxX > drawingPanel.Width || maxY > drawingPanel.Height)
            {
                // Adjust the triangle's position to fit within the drawing panel
                float offsetX = 0, offsetY = 0;
                if (minX < 0) offsetX = -minX;
                if (minY < 0) offsetY = -minY;
                if (maxX > drawingPanel.Width) offsetX = drawingPanel.Width - maxX;
                if (maxY > drawingPanel.Height) offsetY = drawingPanel.Height - maxY;

                // Adjust the positions of the triangle's vertices
                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = new PointF(points[i].X + offsetX, points[i].Y + offsetY);
                }
            }

            // Move the current position to the first vertex of the triangle
            MoveTo(new string[] { "position", points[0].X.ToString(), points[0].Y.ToString() });

            // Draw the triangle
            if (fillShape)
                graphics.FillPolygon(new SolidBrush(pen.Color), points);
            else
                graphics.DrawPolygon(pen, points);
        }


        public void ClearDrawingArea()
        {
            graphics.Clear(Color.White);
        }

        public void ResetPenPosition()
        {
            currentPosition = PointF.Empty;
        }

        public void SetFillMode(string[] parts)
        {
            fillShape = parts[1].ToLower() == "on";
        }

        public void SaveProgramToFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (string cmd in txtProgram.Lines)
                    {
                        writer.WriteLine(cmd);
                    }
                }

                MessageBox.Show($"Program saved to {filePath}", "Save Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        public void LoadProgramFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                txtProgram.Lines = File.ReadAllLines(filePath);
                MessageBox.Show($"Program loaded from {filePath}", "Load Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void txtProgram_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
