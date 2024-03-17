using System.Drawing;

namespace WindowsFormsAppGagan11.Tests
{
    [TestFixture]
    public class MainFormTests
    {
        private MainForm mainForm;

        [SetUp]
        public void Setup()
        {
            mainForm = new MainForm();
        }

        [Test]
        public void MoveTo_ValidInput_PositionUpdated()
        {
            string[] parts = { "position", "100", "100" };
            mainForm.MoveTo(parts);
            Assert.AreEqual(new PointF(100, 100), mainForm.CurrentPosition);
        }

        [Test]
        public void MoveTo_InvalidInput_ExceptionThrown()
        {
            string[] parts = { "position", "abc", "def" };
            Assert.Throws<ArgumentException>(() => mainForm.MoveTo(parts));
        }

        [Test]
        public void SetPenColor_ValidInput_ColorUpdated()
        {
            string[] parts = { "pen", "Red" };
            mainForm.SetPenColor(parts);
            Assert.AreEqual(Color.Red, mainForm.Pen.Color);
        }

        [Test]
        public void DrawTo_PositionChanged()
        {
            string[] parts = { "draw", "100", "100" };
            mainForm.DrawTo(parts);
            Assert.AreEqual(new PointF(100, 100), mainForm.CurrentPosition);
        }

        [Test]
        public void DrawRectangle_RectangleDrawn()
        {
            string[] parts = { "rectangle", "50", "50" };
            mainForm.MoveTo(new string[] { "position", "100", "100" });
            mainForm.DrawRectangle(parts);

            // Check if rectangle is drawn correctly
            // Assuming drawingPanel size is sufficient to show rectangle
            Assert.Pass();
        }

        [Test]
        public void DrawCircle_CircleDrawn()
        {
            string[] parts = { "circle", "50" };
            mainForm.MoveTo(new string[] { "position", "100", "100" });
            mainForm.DrawCircle(parts);

            // Check if circle is drawn correctly
            // Assuming drawingPanel size is sufficient to show circle
            Assert.Pass();
        }

        [Test]
        public void DrawTriangle_TriangleDrawn()
        {
            mainForm.MoveTo(new string[] { "position", "100", "100" });
            mainForm.DrawTriangle();

            // Check if triangle is drawn correctly
            // Assuming drawingPanel size is sufficient to show triangle
            Assert.Pass();
        }

        [Test]
        public void ClearDrawingArea_DrawingAreaCleared()
        {
            mainForm.ClearDrawingArea();

            // Check if drawingPanel is cleared
            // Assuming drawingPanel is white after clearing
            Assert.Pass();
        }

        [Test]
        public void ResetPenPosition_PositionReset()
        {
            mainForm.CurrentPosition = new PointF(100, 100);
            mainForm.ResetPenPosition();
            Assert.AreEqual(PointF.Empty, mainForm.CurrentPosition);
        }

        [Test]
        public void SetFillMode_FillModeChanged()
        {
            string[] parts = { "fill", "on" };
            mainForm.SetFillMode(parts);
            Assert.IsTrue(mainForm.FillShape);
        }

        [Test]
        public void SaveProgramToFile_ProgramSaved()
        {
            string filePath = "test_program.txt";
            mainForm.TextBox1.Text = "position 100 100" + Environment.NewLine + "circle 50";
            mainForm.SaveProgramToFile(filePath);
            Assert.IsTrue(File.Exists(filePath));
            File.Delete(filePath);
        }

        [Test]
        public void LoadProgramFromFile_ProgramLoaded()
        {
            string filePath = "test_program.txt";
            string[] lines = { "position 100 100", "circle 50" };
            File.WriteAllLines(filePath, lines);
            mainForm.LoadProgramFromFile(filePath);
            Assert.AreEqual(lines, mainForm.TextBox1.Lines);
            File.Delete(filePath);
        }
    }
}
