using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Management;
using System.Reflection;

namespace God2Iso
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try
            {
                windowsTaskbar = TaskbarManager.Instance;
            }
            catch (Exception) { }
            Text = GetAppName();
        }

        private string lastPath = null;
        private TaskbarManager windowsTaskbar = null;
        private System.Windows.Forms.Button buttonScanFolder;

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog browser = new OpenFileDialog();
            browser.CheckFileExists = true;
            if (lastPath != null && Directory.Exists(lastPath)) browser.InitialDirectory = lastPath;
            if (browser.ShowDialog() == DialogResult.OK)
            {
                string path = Path.Combine(browser.FileName + ".data", "Data0000");
                if (listPackages.Items.Contains(browser.FileName))
                {
                    MessageBox.Show("God package already already in the list.", "God2Iso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (File.Exists(path))
                {
                    listPackages.Items.Add(browser.FileName);
                    lastPath = Path.GetDirectoryName(browser.FileName);
                }
                else
                {
                    MessageBox.Show("Could not find associated data file: " + path, "God2Iso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.ShowNewFolderButton = true;
            browser.Description = "Select an output folder for the iso...";
            if (Directory.Exists(textOutput.Text)) browser.SelectedPath = textOutput.Text;
            if (browser.ShowDialog() == DialogResult.OK)
            {
                textOutput.Text = browser.SelectedPath;
            }
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            try
            {
                if (listPackages.Items.Count < 1)
                {
                    MessageBox.Show("No God packages specified.", "God2Iso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (textOutput.Text.Length < 1)
                {
                    MessageBox.Show("You must specify an output directory.", "God2Iso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!Directory.Exists(textOutput.Text))
                {
                    MessageBox.Show("Output directory does not exist.", "God2Iso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                progressIso.Value = 0;
                progressTotal.Value = 0;
                listPackages.SelectedIndex = -1;
                buttonAdd.Enabled = false;
                buttonGo.Enabled = false;
                buttonBrowse.Enabled = false;
                buttonClear.Enabled = false;
                listPackages.Enabled = false;
                textOutput.Enabled = false;
                cbFix.Enabled = false;

                // calculate total number & size of files
                int totalFiles = 0;
                ulong totalSize = 0;
                for (int i = 0; i < listPackages.Items.Count; i++)
                {
                    int count = 0;
                    while (true)
                    {
                        string path = Path.Combine(((string)listPackages.Items[i]) + ".data", "Data" + count.ToString("D4"));
                        if (!File.Exists(path)) break;
                        count++;
                        FileInfo info = new FileInfo(path);
                        totalSize += (ulong)info.Length;
                    }
                    totalFiles += count;
                }

                try
                {
                    ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + textOutput.Text.Substring(0, 1) + ":\"");
                    disk.Get();
                    ulong freeSpace = (ulong)disk["FreeSpace"];
                    if (totalSize > freeSpace)
                    {
                        MessageBox.Show("Not enough free space in output directory - need approximately " + FormatSize(totalSize) + '.', "God2Iso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // reset buttons
                        backgroundWorker_RunWorkerCompleted(null, null);
                        return;
                    }
                }
                catch (Exception) { }

                progressTotal.Maximum = totalFiles;
                if (windowsTaskbar != null) windowsTaskbar.SetProgressState(TaskbarProgressBarState.Normal);
                backgroundWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                textBox1.Text = ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        private string FormatSize(ulong byteCount)
        {
            string size;
            if (byteCount >= 1073741824)
            {
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " gb";
            }
            else if (byteCount >= 1048576)
            {
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " mb";
            }
            else if (byteCount >= 1024)
            {
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " kb";
            }
            else
            {
                size = byteCount + " b";
            }
            return size;
        }

        private void listPackages_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonRemove.Enabled = listPackages.SelectedIndex >= 0;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int selected = listPackages.SelectedIndex;
            if (selected < listPackages.Items.Count - 1)
            {
                listPackages.SelectedIndex = selected + 1;
            }
            else if (listPackages.Items.Count > 1)
            {
                listPackages.SelectedIndex = selected - 1;
            }
            listPackages.Items.RemoveAt(selected);
        }

        private bool HasXSFHeader(string file)
        {
            byte[] buff = new byte[3];

            FileStream data = new FileStream(file, FileMode.Open, FileAccess.Read);
            data.Position = 0x2000;
            data.Read(buff, 0, buff.Length);
            data.Close();

            if (buff[0] != 0x58) return false;
            if (buff[1] != 0x53) return false;
            if (buff[2] != 0x46) return false;

            return true;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 0; i < listPackages.Items.Count; i++)
            {
                FileStream iso = null;
                FileStream data = null;

                try
                {
                    string baseName = Path.GetFileName((string)listPackages.Items[i]);
                    string dataPath = (string)listPackages.Items[i] + ".data";

                    // count files
                    int totalFiles;
                    for (totalFiles = 0; true; totalFiles++)
                    {
                        string path = Path.Combine(dataPath, "Data" + totalFiles.ToString("D4"));
                        if (!File.Exists(path)) break;
                    }
                    if (totalFiles < 1) return;

                    // check for xsf header
                    bool hasXSF = HasXSFHeader(Path.Combine(dataPath, "Data0000"));

                    // open new iso file
                    iso = new FileStream(Path.Combine(textOutput.Text, baseName + ".iso"), FileMode.Create, FileAccess.ReadWrite);

                    // add header, if needed
                    if (!hasXSF) iso.Write(Properties.Resources.XSFHeader, 0, Properties.Resources.XSFHeader.Length);

                    // loop through data parts
                    for (int fileNum = 0; fileNum < totalFiles; fileNum++)
                    {
                        string path = Path.Combine(dataPath, "Data" + fileNum.ToString("D4"));
                        data = new FileStream(path, FileMode.Open, FileAccess.Read);
                        data.Position = 0x2000;
                        int len = 0;
                        while (true)
                        {
                            byte[] buff = new byte[0xcc000];
                            len = data.Read(buff, 0, buff.Length);
                            iso.Write(buff, 0, len);
                            if (len < 0xcc000) break;
                            len = data.Read(buff, 0, 0x1000);
                            if (len < 0x1000) break;
                        }
                        data.Close();
                        data = null;
                        worker.ReportProgress(0, new MyState(fileNum + 1, totalFiles));
                    }

                    if (!hasXSF)
                    {
                        FixXFSHeader(iso);
                        FixSectorOffsets(iso, (string)listPackages.Items[i]);
                    }
                    if (cbFix.Checked) FixCreateIsoGoodHeader(iso);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "God2Iso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (iso != null) try { iso.Close(); } catch (Exception) { }
                    if (data != null) try { data.Close(); } catch (Exception) { }
                }
            }
        }

        private void FixSectorOffsets(FileStream iso, string godPath)
        {
            int sector, size, offset;
            byte[] buffer;
            Queue<DirEntry> directories = new Queue<DirEntry>();

            buffer = File.ReadAllBytes(godPath);
            if ((buffer[0x391] & 0x40) != 0x40) return;
            offset = BitConverter.ToInt32(buffer, 0x395);
            if (offset == 0) return;
            offset *= 2;
            offset -= 34;

            buffer = new byte[4];
            iso.Position = 0x10014;
            iso.Read(buffer, 0, 4);
            sector = BitConverter.ToInt32(buffer, 0);
            if (sector > 0)
            {
                sector -= offset;
                byte[] corrected = BitConverter.GetBytes(sector);
                iso.Position -= 4;
                iso.Write(corrected, 0, 4);
                iso.Read(buffer, 0, 4);
                size = BitConverter.ToInt32(buffer, 0);
                directories.Enqueue(new DirEntry(sector, size));
            }

            while (directories.Count > 0)
            {
                DirEntry dirEntry = directories.Dequeue();
                iso.Position = dirEntry.StartPos();
                while ((iso.Position + 4) < dirEntry.EndPos())
                {
                    if ((iso.Position + 4) / 2048L > iso.Position / 2048L)
                    {
                        iso.Position += 2048L - (iso.Position % 2048L);
                    }
                    iso.Read(buffer, 0, 4);
                    if (buffer[0] == 0xff && buffer[1] == 0xff && buffer[2] == 0xff && buffer[3] == 0xff)
                    {
                        if (dirEntry.EndPos() - iso.Position > 2048)
                        {
                            iso.Position += 2048L - (iso.Position % 2048L);
                            continue;
                        }
                        break;
                    }

                    iso.Read(buffer, 0, 4);
                    sector = BitConverter.ToInt32(buffer, 0);
                    if (sector > 0)
                    {
                        sector -= offset;
                        byte[] corrected = BitConverter.GetBytes(sector);
                        iso.Position -= 4;
                        iso.Write(corrected, 0, 4);
                    }

                    iso.Read(buffer, 0, 4);
                    size = BitConverter.ToInt32(buffer, 0);
                    iso.Read(buffer, 0, 1);

                    if ((buffer[0] & 0x10) == 0x10) directories.Enqueue(new DirEntry(sector, size));
                    iso.Read(buffer, 0, 1);
                    iso.Position += buffer[0];

                    if ((14 + buffer[0]) % 4 > 0) iso.Position += 4 - ((14 + buffer[0]) % 4);
                }
            }
        }

        private void FixCreateIsoGoodHeader(FileStream iso)
        {
            byte[] bytes = new byte[8];
            iso.Position = 8;
            iso.Read(bytes, 0, 8);
            if (BitConverter.ToInt64(bytes, 0) == 2587648L)
            {
                iso.Position = 0;
                iso.Write(Properties.Resources.XSFHeader, 0, Properties.Resources.XSFHeader.Length);
                FixXFSHeader(iso);
            }
        }

        private void FixXFSHeader(FileStream iso)
        {
            byte[] bytes;

            iso.Position = 8;
            bytes = BitConverter.GetBytes(iso.Length - 0x400);
            iso.Write(bytes, 0, bytes.Length);

            iso.Position = 0x8050;
            bytes = BitConverter.GetBytes((uint)(iso.Length / 2048));
            iso.Write(bytes, 0, bytes.Length);
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                iso.WriteByte(bytes[i]);
            }

            iso.Position = 0x7a69;
            bytes = Encoding.ASCII.GetBytes(GetAppName());
            iso.Write(bytes, 0, bytes.Length);
        }

        private string GetAppName()
        {
            string s = "God2Iso v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return s.Substring(0, s.Length - 2);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (windowsTaskbar != null) windowsTaskbar.SetProgressState(TaskbarProgressBarState.NoProgress);
            buttonAdd.Enabled = true;
            buttonGo.Enabled = true;
            buttonBrowse.Enabled = true;
            buttonClear.Enabled = true;
            listPackages.Enabled = true;
            textOutput.Enabled = true;
            cbFix.Enabled = true;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listPackages.SelectedIndex = -1;
            listPackages.Items.Clear();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MyState state = (MyState)e.UserState;
            progressIso.Value = state.file;
            progressIso.Maximum = state.files;
            progressTotal.PerformStep();
            if (windowsTaskbar != null) windowsTaskbar.SetProgressValue(progressTotal.Value, progressTotal.Maximum);
        }

        private class MyState
        {
            public int file;
            public int files;
            public MyState(int file, int files)
            {
                this.file = file;
                this.files = files;
            }
        }

        private class DirEntry
        {
            public int sector;
            public int length;
            public DirEntry(int sector, int length)
            {
                this.sector = sector;
                this.length = length;
            }
            public long StartPos()
            {
                return sector * 2048L;
            }
            public long EndPos()
            {
                return (sector * 2048L) + length;
            }
        }

        private void buttonScanFolder_Click(object sender, EventArgs e)
        {
            // Open FolderBrowserDialog to allow the user to select a folder
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.ShowNewFolderButton = false; // Prevent creating new folders
            browser.Description = "Select a folder to scan for files...";

            if (browser.ShowDialog() == DialogResult.OK)
            {
                string folderPath = browser.SelectedPath;

                // Call the method to scan the folder and its subfolders
                ScanFolderRecursively(folderPath);

                // Optionally, show a message once scanning is complete
                MessageBox.Show("Folder scan complete.", "God2Iso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ScanFolderRecursively(string folderPath)
        {
            // Get all files in the current folder
            string[] files = Directory.GetFiles(folderPath);

            // Iterate through each file and add it to the listPackages
            foreach (var file in files)
            {
                // Check if the file is already in the list
                if (!listPackages.Items.Contains(file))
                {
                    // Check for the associated .data file (just like in the buttonAdd logic)
                    string dataPath = Path.Combine(file + ".data", "Data0000");

                    // Only add the file if it exists and is valid
                    if (File.Exists(dataPath))
                    {
                        // Get the file extension to preserve it
                        string fileExtension = Path.GetExtension(file);

                        // Get the full path to the folder where the file is located
                        string fullFolderPath = Path.GetDirectoryName(file);

                        // Split the full path into folder components
                        string[] folderParts = fullFolderPath.Split(Path.DirectorySeparatorChar);

                        // Check if we have at least 3 levels (root folder, subfolder, and the target file folder)
                        if (folderParts.Length < 3) // Ensure the folder has enough structure
                        {
                            MessageBox.Show("The folder structure is not as expected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // The root folder (third-to-last folder in the path structure)
                        string rootFolderName = folderParts[folderParts.Length - 3];

                        // Create the new file name based on the root folder name
                        string newFileName = Path.Combine(Path.GetDirectoryName(file), rootFolderName + fileExtension);

                        // Rename the file
                        File.Move(file, newFileName);

                        // Now rename the **last folder** inside the directory to match the root folder name and add `.data` suffix
                        string lastFolderPath = Path.Combine(fullFolderPath, Path.GetFileName(file) + ".data"); // This should point to the last folder (e.g., "05E15F869CA8572DE8C6.data")
                        string newLastFolderName = Path.Combine(Path.GetDirectoryName(lastFolderPath), rootFolderName + ".data"); // Append .data to the new folder name

                        // Check if the folder with the new name already exists
                        if (Directory.Exists(newLastFolderName))
                        {
                            MessageBox.Show("The folder already exists: " + newLastFolderName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Rename the folder (only if it's not already named the same)
                        if (lastFolderPath != newLastFolderName && Directory.Exists(lastFolderPath))
                        {
                            try
                            {
                                Directory.Move(lastFolderPath, newLastFolderName);
                            }
                            catch (IOException ex)
                            {
                                MessageBox.Show("An error occurred while renaming the folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        // Remove the old file path and add the renamed path
                        listPackages.Items.Remove(file);  // Remove old path
                        listPackages.Items.Add(newFileName);  // Add the renamed path
                    }
                }
            }

            // Recursively scan all subfolders
            string[] subfolders = Directory.GetDirectories(folderPath);
            foreach (var subfolder in subfolders)
            {
                // Call the method recursively for each subfolder
                ScanFolderRecursively(subfolder);
            }
        }

    }
}