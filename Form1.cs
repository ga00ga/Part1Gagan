using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsAppGagan11
{
    public partial class MainForm : Form
    {
        private List<string> programCommands = new List<string>();
        private Graphics graphics;
        private Pen pen = new Pen(Color.Black);
        private PointF currentPosition = PointF.Empty;
        private bool fillShape = false;

        public MainForm()
        {
            InitializeComponent();
            graphics = drawingPanel.CreateGraphics();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExecuteProgram();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            LoadProgramFromFile();
        }
       
        private void button3_Click_1(object sender, EventArgs e)
        {

            SaveProgramToFile();
        }

        private void ExecuteProgram()
        {
            ClearDrawingArea();
            currentPosition = PointF.Empty;

            foreach (string cmd in programCommands)
            {
                try
                {
                    textBox1.Text = cmd;
                    ParseAndExecuteCommand(cmd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Input string was in correct format", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
            }
        }

        private void ParseAndExecuteCommand(string command)
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
                    //throw new ArgumentException($"Invalid command: {command}");
                    break;
            }
        }

        private void MoveTo(string[] parts)
        {
            float x, y;
            if (parts.Length >= 3 && float.TryParse(parts[1], out x) && float.TryParse(parts[2], out y))
            {
                currentPosition = new PointF(x, y);
            }
            else
            {
                // Handle invalid input command format
                MessageBox.Show("Invalid input command format for position.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SetPenColor(string[] parts)
        {
          

            string colorName = parts[1];
            pen.Color = Color.FromName(colorName);
        }

        private void DrawTo(string[] parts)
        {
            

            float x = float.Parse(parts[1]);
            float y = float.Parse(parts[2]);

            // Move the current position to the specified coordinates
            MoveTo(new string[] { "position", x.ToString(), y.ToString() });
        }

        private void DrawRectangle(string[] parts)
        {
            //if (parts.Length != 3)
              //  throw new ArgumentException("Invalid rectangle command");

            int width = int.Parse(parts[1]);
            int height = int.Parse(parts[2]);

            if (fillShape)
                graphics.FillRectangle(new SolidBrush(pen.Color), currentPosition.X, currentPosition.Y, width, height);
            else
                graphics.DrawRectangle(pen, currentPosition.X, currentPosition.Y, width, height);
        }

        private void DrawCircle(string[] parts)
        {
            

            int radius = int.Parse(parts[1]);

            if (fillShape)
                graphics.FillEllipse(new SolidBrush(pen.Color), currentPosition.X - radius, currentPosition.Y - radius, radius * 2, radius * 2);
            else
                graphics.DrawEllipse(pen, currentPosition.X - radius, currentPosition.Y - radius, radius * 2, radius * 2);
        }

        private void DrawTriangle()
        {
            // Implement draw triangle functionality
            PointF[] points = new PointF[]
            {
                new PointF(currentPosition.X, currentPosition.Y),
                new PointF(currentPosition.X + 50, currentPosition.Y + 100),
                new PointF(currentPosition.X - 50, currentPosition.Y + 100)
            };

            if (fillShape)
                graphics.FillPolygon(new SolidBrush(pen.Color), points);
            else
                graphics.DrawPolygon(pen, points);
        }

        private void ClearDrawingArea()
        {
            graphics.Clear(Color.White);
        }

        private void ResetPenPosition()
        {
            currentPosition = PointF.Empty;
        }

        private void SetFillMode(string[] parts)
        {
            //if (parts.Length != 2)
              //  throw new ArgumentException("Invalid fill command");

            fillShape = parts[1].ToLower() == "on";
        }

        private void SaveProgramToFile()
        {
            using (StreamWriter writer = new StreamWriter("program.txt"))
            {
                foreach (string cmd in programCommands)
                {
                    writer.WriteLine(cmd);
                }
            }

            MessageBox.Show("Program saved to program.txt", "Save Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadProgramFromFile()
        {
            programCommands.Clear();
            using (StreamReader reader = new StreamReader("program.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    textBox1.Text=line;
                    programCommands.Add(line);
                    txtProgram.AppendText(line + Environment.NewLine);
                    
                }
            }

            MessageBox.Show("Program loaded from program.txt", "Load Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        
    }
}
