﻿using ApplicationClasses;
using System;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using System.IO;

namespace CourseworkApp
{
    public partial class MainWindow
    {
        private void NewProjectToolStripMenuItem_Click(object sender, EventArgs e) => Build_Click(sender, e);
        private void OpenProjectToolStripMenuItem_Click(object sender, EventArgs e) => Open_Click(sender, e);


        #region Digraph saving

        /// <summary>
        /// Saves digraph data
        /// </summary>
        private void DataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fileDialog = SaveFileDialogForDataSaving())
            {
                if (fileDialog.ShowDialog() != DialogResult.OK) return;

                using (FileStream stream = new FileStream(fileDialog.FileName, FileMode.Create))
                {
                    XmlSerializer format = new XmlSerializer(typeof(Digraph));
                    format.Serialize(stream, Digraph);
                }
            }
        }

        /// <summary>
        /// Saves digraph image
        /// </summary>
        private void SaveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fileDialog = SaveFileDialogForImageSaving())
            {
                if (fileDialog.ShowDialog() != DialogResult.OK) return;

                using (FileStream stream = new FileStream(fileDialog.FileName, FileMode.Create))
                {
                    graphDrawing.DrawTheWholeGraph(Digraph);
                    graphDrawing.Image.Save(stream, ImageFormat.Jpeg);
                }
            }
        }

        /// <summary>
        /// Saves digraph data and image to a folder
        /// </summary>
        private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var folderDialog = FolderBrowserDialogForGraphSaving())
            {
                if (folderDialog.ShowDialog() != DialogResult.OK) return;

                using (var stream = new FileStream(folderDialog.SelectedPath + @"\Image.jpg", FileMode.Create))
                {
                    graphDrawing.DrawTheWholeGraph(Digraph);
                    graphDrawing.Image.Save(stream, ImageFormat.Jpeg);
                }

                using (var stream = new StreamWriter(folderDialog.SelectedPath + @"\Data.digraph", false))
                {
                    XmlSerializer format = new XmlSerializer(typeof(Digraph));
                    format.Serialize(stream, Digraph);
                }
            }
        }

        #endregion


        /// <summary>
        /// Saves the graph if user wants to and closes the app
        /// </summary>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Digraph.Vertices.Count != 0
                    && SaveGraph("Would you like to save the graph before leaving?", "Saving") == DialogResult.Cancel)
                return;

            if (MessageBox.Show("Are you sure you want to leave?", "Leaving",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                Close();
        }

        /// <summary>
        /// Saves the graph if user wants to and goes to a start window
        /// </summary>
        private void MainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Digraph.Vertices.Count != 0
                && SaveGraph("Would you like to save the graph? Otherwise, your graph will be lost.",
                    "Saving") == DialogResult.Cancel) return;

            ChangeDrawingElementsState(false);
            RefreshVariables();
            ChangeMainMenuState(true);
        }


        /// <summary>
        /// Shows a user manual (maybe in the future)
        /// </summary>
        private void UserManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming soon...", "In development");
        }


        /// <summary>
        /// Giving information about the application and the developer
        /// </summary>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e) =>
            MessageBox.Show(AboutApp, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);

        
        /// <summary>
        /// Saves the digraph to file if user wants to
        /// </summary>
        private DialogResult SaveGraph(string message, string caption)
        {
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                SaveAllToolStripMenuItem_Click(this, null);
            return result;
        }
    }
}