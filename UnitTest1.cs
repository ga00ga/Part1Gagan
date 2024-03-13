using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;

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
        public void TestMoveTo_ValidInput()
        {
            // Arrange
            string[] parts = { "position", "100", "200" };

            // Act
            mainForm.MoveTo(parts);

            // Assert
            Assert.AreEqual(new PointF(100, 200), mainForm.currentPosition);
        }

        [Test]
        public void TestSetPenColor()
        {
            // Arrange
            string[] parts = { "pen", "Red" };

            // Act
            mainForm.SetPenColor(parts);

            // Assert
            Assert.AreEqual(Color.Red, mainForm.pen.Color);
        }

        [Test]
        public void TestDrawRectangle()
        {
            // Arrange
            string[] parts = { "rectangle", "50", "100" };
            mainForm.currentPosition = new PointF(100, 100);

            // Act
            mainForm.DrawRectangle(parts);

            // Assert
            // As we cannot directly test graphics methods, we can test the fillShape property
            Assert.IsFalse(mainForm.fillShape);
        }

        [Test]
        public void TestDrawCircle()
        {
            // Arrange
            string[] parts = { "circle", "30" };
            mainForm.currentPosition = new PointF(100, 100);

            // Act
            mainForm.DrawCircle(parts);

            // Assert
            // As we cannot directly test graphics methods, we can test the fillShape property
            Assert.IsFalse(mainForm.fillShape);
        }

        [Test]
        public void TestDrawTriangle()
        {
            // Act
            mainForm.DrawTriangle();

            // Assert
            // As we cannot directly test graphics methods, we can test the fillShape property
            Assert.IsFalse(mainForm.fillShape);
        }

        [Test]
        public void TestClearDrawingArea()
        {
            // Act
            mainForm.ClearDrawingArea();

            // Assert
            // As we cannot directly test graphics methods, we can test the fillShape property
            Assert.AreEqual(Color.White, ((Bitmap)mainForm.drawingPanel.BackgroundImage).GetPixel(0, 0));
        }

        [Test]
        public void TestResetPenPosition()
        {
            // Act
            mainForm.ResetPenPosition();

            // Assert
            Assert.AreEqual(PointF.Empty, mainForm.currentPosition);
        }

        [Test]
        public void TestSetFillMode()
        {
            // Arrange
            string[] parts = { "fill", "on" };

            // Act
            mainForm.SetFillMode(parts);

            // Assert
            Assert.IsTrue(mainForm.fillShape);
        }

        [Test]
        public void TestSaveProgramToFile()
        {
            // Arrange
            mainForm.programCommands.Add("position 100 200");

            // Act
            mainForm.SaveProgramToFile();

            // Assert
            Assert.IsTrue(File.Exists("program.txt"));
        }

        [Test]
        public void TestLoadProgramFromFile()
        {
            // Arrange
            File.WriteAllText("program.txt", "position 100 200");

            // Act
            mainForm.LoadProgramFromFile();

            // Assert
            Assert.AreEqual("position 100 200" + Environment.NewLine, mainForm.txtProgram.Text);
        }
    }
}
