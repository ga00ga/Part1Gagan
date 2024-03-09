using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GraphicalProgrammingLanguage
{
    public partial class MainForm : Form
    {
        private List<string> individualCommands = new List<string>();
        private List<string> completeProgram = new List<string>();
        private Graphics graphics;
        private Pen pen = new Pen(Color.Black);
        private PointF currentPosition = PointF.Empty;
        private bool fillShape = false;

        public MainForm()
        {
            InitializeComponent();
            graphics = drawingPanel.CreateGraphics();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string command = individualTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(command))
            {
                individualCommands.Add(command);
                UpdateIndividualCommandsBox();
                individualTextBox.Clear();
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            individualCommands.Clear();
            UpdateIndividualCommandsBox();
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            ExecuteProgram();
        }

        private void ExecuteProgram()
        {
            ClearDrawingArea();
            currentPosition = PointF.Empty;

            foreach (string cmd in completeProgram)
            {
                try
                {
                    ParseAndExecuteCommand(cmd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Refresh the drawing panel to display the drawn shapes
            drawingPanel.Refresh();
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
                    throw new ArgumentException($"Invalid command: {command}");
            }
        }

        private void MoveTo(string[] parts)
        {
            if (parts.Length != 3)
                throw new ArgumentException("Invalid position command");

            float x = float.Parse(parts[1]);
            float y = float.Parse(parts[2]);

            currentPosition = new PointF(x, y);
        }

        private void SetPenColor(string[] parts)
        {
            if (parts.Length != 2)
                throw new ArgumentException("Invalid pen command");

            string colorName = parts[1];
            pen.Color = Color.FromName(colorName);
        }

        private void DrawTo(string[] parts)
        {
            if (parts.Length >= 3)
            {
                if (float.TryParse(parts[1], out float x) && float.TryParse(parts[2], out float y))
                {
                    // Move the current position to the specified coordinates
                    MoveTo(new string[] { "position", x.ToString(), y.ToString() });
                }
                else
                {
                    MessageBox.Show("Invalid coordinates.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid command.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DrawRectangle(string[] parts)
        {
            if (parts.Length != 3)
                throw new ArgumentException("Invalid rectangle command");

            int width = int.Parse(parts[1]);
            int height = int.Parse(parts[2]);

            if (fillShape)
                graphics.FillRectangle(new SolidBrush(pen.Color), currentPosition.X, currentPosition.Y, width, height);
            else
                graphics.DrawRectangle(pen, currentPosition.X, currentPosition.Y, width, height);
        }

        private void DrawCircle(string[] parts)
        {
            if (parts.Length != 2)
                throw new ArgumentException("Invalid circle command");

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
            if (parts.Length != 2)
                throw new ArgumentException("Invalid fill command");

            fillShape = parts[1].ToLower() == "on";
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveProgramToFile();
        }

        private void LoadProgramFromFile()
        {
            completeProgram.Clear();
            using (StreamReader reader = new StreamReader("program.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    completeProgram.Add(line);
                }
            }

            MessageBox.Show("Program loaded from program.txt", "Load Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveProgramToFile()
        {
            using (StreamWriter writer = new StreamWriter("program.txt"))
            {
                foreach (string cmd in completeProgram)
                {
                    writer.WriteLine(cmd);
                }
            }

            MessageBox.Show("Program saved to program.txt", "Save Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateIndividualCommandsBox()
        {
            individualCommandsListBox.Items.Clear();
            individualCommandsListBox.Items.AddRange(individualCommands.ToArray());
        }

        private void individualCommandsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (individualCommandsListBox.SelectedIndex >= 0)
            {
                individualTextBox.Text = individualCommandsListBox.SelectedItem.ToString();
            }
        }

        private void addToCompleteButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(individualTextBox.Text))
            {
                completeProgram.Add(individualTextBox.Text);
                UpdateCompleteProgramBox();
                individualTextBox.Clear();
            }
        }

        private void UpdateCompleteProgramBox()
        {
            completeProgramTextBox.Clear();
            completeProgramTextBox.AppendText(string.Join(Environment.NewLine, completeProgram));
        }

        private void clearCompleteButton_Click(object sender, EventArgs e)
        {
            completeProgram.Clear();
            UpdateCompleteProgramBox();
        }
    }
}
